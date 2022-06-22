using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Configuration;
using Microsoft.SharePoint.Client.WorkflowServices;
using Microsoft.SharePoint.Client.Workflow;

namespace SharepointBatchPrint
{
    public partial class Main : System.Windows.Forms.Form
    {
        private ClientContext context;
        private string siteURL;
        private List ls;
        private WorkflowServicesManager workflowManager;
        private Guid listID;
        private WorkflowAssociation removeWorkflow;
        private InteropService interopService;
        private WorkflowSubscriptionService subscriptionService;
        private WorkflowInstanceService instanceService;

        public Main() {
            InitializeComponent();

            boxxy.CheckOnClick = true; 
            // Otherwise checking needs 2 clicks - makes it feel much more responsive
            // http://stackoverflow.com/questions/4083703/odd-behavior-when-toggling-checkedlistbox-items-checked-state-via-mouseclick-wh

            // Setup global objects
            siteURL = ConfigurationManager.AppSettings["siteURL"];
            context = new ClientContext(siteURL);


            workflowManager = new WorkflowServicesManager(context, context.Web);
            interopService = workflowManager.GetWorkflowInteropService();
            subscriptionService = workflowManager.GetWorkflowSubscriptionService();
            context.Load(subscriptionService);
            context.ExecuteQuery(); // if in doubt, sprinkle around the code
            WorkflowAssociationCollection wfAssociations = context.Web.WorkflowAssociations;
            instanceService = workflowManager.GetWorkflowInstanceService();
            removeWorkflow = wfAssociations.GetByName("Withdraw");

            updateItems();
        }

        public void updateItems () {
            boxxy.Items.Clear();
            foreach (Document e in populateList())
            {
                boxxy.Items.Add(e);
            }
        }

        private void loadAndExecute(ClientObject clientObject) {
            context.Load(clientObject);
            context.ExecuteQuery();
        }

        private List<Document> populateList() {
            var res = new List<Document>();

            // Assume the web has a list named "Announcements". 
            ls = context.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["list"]);
            context.Load(ls);
            context.ExecuteQuery();
            listID = ls.Id;
           

            // This creates a CamlQuery that has a RowLimit of 100, and also specifies Scope="RecursiveAll" 
            // so that it grabs all list items, regardless of the folder they are in. 
            CamlQuery query = CamlQuery.CreateAllItemsQuery(40);
            ListItemCollection items = ls.GetItems(query);

            // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
            context.Load(items);
            context.ExecuteQuery();

            foreach (ListItem listItem in items)
            {
                try
                {
                    context.Load(listItem);
                    context.ExecuteQuery();
                    listItem.Update();
                    context.ExecuteQuery();
                    string title = (String)listItem["Title"];
                    if (title == null)
                    {
                        title = "No Title";
                    }
                    string URI = (String)listItem["DocumentUrl"];
                    res.Add(new Document(title, URI, listItem));
                }
                catch (Microsoft.SharePoint.Client.ServerException)
                {
                    continue;
                }

            } // foreach
            txtNDocs.Text = "Selected: " + boxxy.CheckedItems.Count;
            return res;
        }



        private void btnPrint_Click(object sender, EventArgs args) {

            var subs = subscriptionService.EnumerateSubscriptionsByList(listID);
            context.Load(subs);
            context.ExecuteQuery();
            int wfID = -1;

            for (int i = 0; i < subs.ToList().Count && wfID == -1; ++i) {
                // Find the worlflow ID in the subscription service
                if (subs[i].Name == "Print item in queue") {
                    wfID = i;
                }
            }

            if (wfID == -1) { // die
                MessageBox.Show("No print log workflow found. Printing will continue", "Error");
            }


            foreach (Document item in boxxy.CheckedItems) {
                if (item.doPrint()) {
                    item.objRef["Printed"] = DateTime.Now;
                    item.objRef.Update();
                    context.ExecuteQuery();
                    // Log workflow
                    instanceService.StartWorkflowOnListItem(subs[wfID], item.id, new Dictionary<string, object>());
                    context.ExecuteQuery();
                }
                //System.Threading.Thread.Sleep(2000); // 2 second delay

            }
            
        }
        private void btnDelete_Click(object sender, EventArgs args) {

            context.Load(subscriptionService);
            context.ExecuteQuery();


            if (boxxy.CheckedItems.Count == 0) {
                MessageBox.Show("No items selected", "Error", MessageBoxButtons.OK);
                return;
            }
            DialogResult confirmBox = MessageBox.Show(
                    "This will remove " + boxxy.CheckedItems.Count + " files from the batch print queue. Continue?",
                    "Delete",
                    MessageBoxButtons.OKCancel);
            if (confirmBox == DialogResult.Cancel) {
                return;
            }

            var subs = subscriptionService.EnumerateSubscriptionsByList(listID);
            context.Load(subs);
            context.ExecuteQuery();
            int wfID = -1;

            for ( int i = 0; i < subs.ToList().Count && wfID == -1; ++i ) {
                // Find the worlflow ID in the subscription service
                if (subs[i].Name == "Withdraw") {
                    wfID = i;
                }
            }

            if (wfID == -1) { // die
                MessageBox.Show("No withdrawal workflow found. Cannot continue.", "Critical Error");
                return;
            }

            foreach (Document item in boxxy.CheckedItems) {
                int doDelete = item.deleteCheck();
                if (doDelete == -1) {
                    // cancel
                    return;
                } else if (doDelete == 0) {
                    // No
                    continue;
                }

                // Start the deletion workflow
                instanceService.StartWorkflowOnListItem(subs[wfID], item.id, new Dictionary<string, object>());
                context.ExecuteQuery();
            }
            updateItems();

        }
        private void btnSelAll_Click(object sender, System.EventArgs e) {
            for (int i = 0; i < boxxy.Items.Count; i++) {
                boxxy.SetItemChecked(i, true);
            }
        }

        private void btnInvertSel_Click(object sender, System.EventArgs e) {
            for (int i = 0; i < boxxy.Items.Count; i++) {
                boxxy.SetItemChecked(i, !boxxy.GetItemChecked(i));
            }
        }
        private void btnRefresh_Click(object sender, EventArgs args) {
            foreach (Document item in boxxy.Items) {
                item.deleteLocalCopy();
            }
            updateItems();
        }

        private void boxxy_updateCount(object sender, MouseEventArgs e) {
            txtNDocs.Text = "Selected: " +  boxxy.CheckedItems.Count;
        }

        private void button1_Click(object sender, EventArgs e) {
            MessageBox.Show("SharepointBatchPrint\nVersion 1.1\n\n"
                +"Select the documents you want to print and click print, please ensure the default printer is set up as the printer you wish to print to.\n\n"
                +"The invert selection button selects all unselected items, and simultaniously deselects all selected items.\n\n"
                +"The Delete button triggers the deletion workflow for all selected items. You will be prompted if you haven't printed the documents in this session.", "Help");
        }

        private void Main_Load(object sender, EventArgs e) {

        }

    }
}
