using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Drawing.Printing;
using Microsoft.SharePoint.Client;
using System.Windows.Forms;

namespace SharepointBatchPrint
{
    class Document
    {
        public String name { get;  set; }
        private String path { get;  set; }
        private String fileLoc;
        private ListItem objRef;
        private bool deleted;
        private bool printed;

        public Document(String name, String path, ListItem objRef) {
            this.name = name;
            this.path = path;
            fileLoc = null;
            this.objRef = objRef;
            deleted = false;
            printed = false;
        }

        public override String ToString() {
            return name;
        }

        /// <summary>
        /// downloads a document from a given URL
        /// </summary>
        /// </returns>
        private void download() {
            if (fileLoc != null || deleted) {
                return;
            }
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            request.AllowWriteStreamBuffering = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();

            string dlDir = "test";

            if (!Directory.Exists(dlDir)) {
                Directory.CreateDirectory(dlDir);
            }
            string res = dlDir + "\\" + name;
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

            this.fileLoc = res;
        }

        public void print() {
            if (deleted) {
                return;
            }
            download();
            ProcessStartInfo info = new ProcessStartInfo(fileLoc.Trim());
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            printed = true;
            try {
                Process.Start(info);
            } catch (Exception ex) {
                ex.ToString();
                return;
            }
        }

        public void delete() {
            if (!printed) {
                DialogResult dialogResult = MessageBox.Show(
                    "The file " + name + " has not been printed, are you sure you want to delete this item?",
                    "Warning",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No) {
                    return;
                }
            }
            objRef.DeleteObject();
            deleted = true;
        }
    }
}
