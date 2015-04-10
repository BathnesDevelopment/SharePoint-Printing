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

namespace SharepointBatchPrint
{
    public partial class Main : System.Windows.Forms.Form
    {
        private ClientContext context;
        private string siteURL;
        private List ls;

        public Main() {
            InitializeComponent();
            boxxy.CheckOnClick = true; 
            // Otherwise checking needs 2 clicks - makes it feel much more responsive
            // http://stackoverflow.com/questions/4083703/odd-behavior-when-toggling-checkedlistbox-items-checked-state-via-mouseclick-wh

            siteURL = ConfigurationManager.AppSettings["siteURL"];

            context = new ClientContext(siteURL);
            updateItems();
        }

        public void updateItems () {
            boxxy.Items.Clear();
            foreach (Document e in populateList()) {
                boxxy.Items.Add(e);
            }
        }
               

        private List<Document> populateList() {
            var res = new List<Document>();

            // Assume the web has a list named "Announcements". 
            ls = context.Web.Lists.GetByTitle(ConfigurationManager.AppSettings["list"]);

            // This creates a CamlQuery that has a RowLimit of 100, and also specifies Scope="RecursiveAll" 
            // so that it grabs all list items, regardless of the folder they are in. 
            CamlQuery query = CamlQuery.CreateAllItemsQuery(100);
            ListItemCollection items = ls.GetItems(query);

            // Retrieve all items in the ListItemCollection from List.GetItems(Query). 
            context.Load(items);
            context.ExecuteQuery();
            
            foreach (ListItem listItem in items) {
                String title = "";
                String fileLeafRef = "";
                int i = 0;
                String key = "";
                String URI = siteURL;

                while (i < listItem.FieldValues.Count || ((title == "") && (URI == ""))) {
                    key = listItem.FieldValues.ElementAt(i).Key;
                    if (key == "FileLeafRef") {
                        fileLeafRef = listItem.FieldValues.ElementAt(i).Value.ToString();
                    } else if (key == "FileRef") {
                        URI += listItem.FieldValues.ElementAt(i).Value.ToString();
                    } else if (key == "Title" && (listItem.FieldValues.ElementAt(i).Value != null) && false) {
                        title = listItem.FieldValues.ElementAt(i).Value.ToString();
                    }
                    ++i;
                } // while

                if (title != "") {
                    res.Add(new Document(title, URI, listItem));
                } else { //fallback on fileLeafRef
                    res.Add(new Document(fileLeafRef, URI, listItem));
                }
            } // foreach

            return res;
        }


        private void btnPrint_Click(object sender, EventArgs args) {
            foreach (Document item in boxxy.CheckedItems) {
                item.print();
            }
        }
        private void btnDelete_Click(object sender, EventArgs args) {
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
                item.objRef.DeleteObject();
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
            updateItems();
        }
        private void btnRefresh_Click(object sender, EventArgs args) {
            updateItems();
        }
    }
}
