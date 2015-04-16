﻿using Microsoft.SharePoint.Client;
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
        private Log log;

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
            removeWorkflow = wfAssociations.GetByName("Remove from batch print queue");

            log = new Log(siteURL, ConfigurationManager.AppSettings["logName"]);
            updateItems();
        }

        public void updateItems () {
            boxxy.Items.Clear();
            foreach (Document e in populateList()) {
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
            CamlQuery query = CamlQuery.CreateAllItemsQuery(100);
            ListItemCollection items = ls.GetItems(query);

            // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
            context.Load(items);
            context.ExecuteQuery();
            
            foreach (ListItem listItem in items) {
                context.Load(listItem);
                context.ExecuteQuery();
                listItem.Update();
                context.ExecuteQuery();
                string title = (String)listItem["Title"];
                if (title == null) {
                    title = "No Title";
                }
                string URI = (String)listItem["DocumentUrl"];
                res.Add(new Document(title, URI, listItem));
            } // foreach
            return res;
        }


        private void btnPrint_Click(object sender, EventArgs args) {
            foreach (Document item in boxxy.CheckedItems) {
                if (item.doPrint()) {
                    log.logAction("Print", "The document " + item.name + " was sent to the printer");
                    item.objRef["Printed"] = DateTime.Now;
                    item.objRef.Update();
                    context.ExecuteQuery();
                }
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
            foreach (Document item in boxxy.CheckedItems) {
                int doDelete = item.deleteCheck();
                if (doDelete == -1) {
                    return;
                } else if (doDelete == 0) {
                    continue;
                }

                // Start the deletion workflow
                Dictionary<string,object> workflowHistory = new Dictionary<string, object>();
                workflowHistory.Add("WorkflowHistory", "Hello from the Remote Event Receiver! - " + DateTime.Now.ToString());

                var subs = subscriptionService.EnumerateSubscriptionsByList(listID);
                context.Load(subs);
                context.ExecuteQuery();

                instanceService.StartWorkflowOnListItem(subs[0], item.id, workflowHistory);
                context.ExecuteQuery();
                log.logAction("Remove", "The document " + item.name + " was removed from the Batch Print Queue");
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
            updateItems();
        }
    }
}
