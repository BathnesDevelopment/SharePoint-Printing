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

            siteURL = ConfigurationManager.AppSettings["siteURL"];

            context = new ClientContext(siteURL);
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

                while (i < listItem.FieldValues.Count || ((title == "") && (siteURL == ""))) {
                    key = listItem.FieldValues.ElementAt(i).Key;
                    if (key == "FileLeafRef") {
                        fileLeafRef = listItem.FieldValues.ElementAt(i).Value.ToString();
                    } else if (key == "FileRef") {
                        siteURL += listItem.FieldValues.ElementAt(i).Value.ToString();
                    } else if (key == "Title" && (listItem.FieldValues.ElementAt(i).Value != null) && false) {
                        title = listItem.FieldValues.ElementAt(i).Value.ToString();
                    }
                    ++i;
                } // while

                if (title != "") {
                    res.Add(new Document(title, siteURL, listItem));
                } else { //fallback on fileLeafRef
                    res.Add(new Document(fileLeafRef, siteURL, listItem));
                }
            } // foreach

            return res;
        }

        /*
        /// <summary>
        /// downloads a document from a given URL
        /// </summary>
        /// <param name="URL">The URL of the document</param>
        /// <param name="filename">The file name.</param>
        /// <returns>
        /// The file path of the downloaded file.
        /// </returns>
        private String downloadDocument(string URL, string fileName) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            request.AllowWriteStreamBuffering = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();

            string dlDir = "test";

            if (!Directory.Exists(dlDir)) {
                Directory.CreateDirectory(dlDir);
            }
            string res = dlDir + "\\" + fileName;
            FileStream fs = new FileStream(res, FileMode.Create);
            byte[] read = new byte[256];
            int count = s.Read(read, 0, read.Length);
            while (count > 0) {
                fs.Write(read, 0, count);
                count = s.Read(read, 0, read.Length);
            }

            // Close everything
            fs.Close();
            s.Close();
            response.Close();

            return res;
        }*/

        private void btnPrint_Click(object sender, EventArgs args) {
            foreach (Document item in boxxy.CheckedItems) {
                item.print();
            }
        }
        private void btnDelete_Click(object sender, EventArgs args) {
            foreach (Document item in boxxy.CheckedItems) {
                item.delete();
            }
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
    }
}
