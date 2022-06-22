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
        public ListItem objRef;
        private bool deleted;
        private bool printed;
        public int id;
        public static int n = 0;
        private int instanceID;

        public Document(String name, String path, ListItem objRef) {
            String ext = ".txt"; // fallback to txt file

                int pathLen = path.LastIndexOf('.');
                if (pathLen > 0)
                { 
                    ext = path.Substring(path.LastIndexOf('.'));
                }
                name = name.Replace('/', ' ').Replace('*', ' ').Replace('"', ' ').Replace('\\', ' ').Replace('[', ' ').Replace(']', ' ').Replace(':', ' ').Replace(';', ' ').Replace('|', ' ').Replace('=', ' ').Replace(',', ' ').Replace('.', ' ');
                this.name = name + ext;
                this.path = path;
                fileLoc = null;
                this.objRef = objRef;
                deleted = false;
                printed = false;
                id = objRef.Id;
                instanceID = n++;

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
            deleteLocalCopy();
           
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(path);
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            request.Timeout = 10000;
            request.AllowWriteStreamBuffering = false;
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream s = response.GetResponseStream();

            string dlDir = "temp";
            if (!Directory.Exists(dlDir)) {
                Directory.CreateDirectory(dlDir);
            }
            string res = dlDir + "\\" + instanceID + name;
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

        public void deleteLocalCopy() {
            if (this.fileLoc == null) {
                return;
            }
            try {
                System.IO.File.Delete(fileLoc);
                this.fileLoc = null;
                return;
            } catch (IOException) {
                Debug.Fail("Couldn't delete file" + this.fileLoc);
                return;
            }
        }

        public bool doPrint() {
            if (deleted) {
                return false;
            }
            download();
            Process proc = null;
            ProcessStartInfo info = new ProcessStartInfo(fileLoc.Trim());
            info.Verb = "Print";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;

            printed = true;
            try {
                proc = Process.Start(info);
                int i = 0;
                while (!proc.HasExited) {
                    System.Threading.Thread.Sleep(50);
                    ++i;
                    if (i > 100) {
                        MessageBox.Show(
                           "It seems to be taking a long time to print this document. "+ 
                           "Please check for error messages from the program, and press ok when you are ready "+
                           "to print the next document",
                           "Warning",
                           MessageBoxButtons.OK);
                                return true;
                            }
                }
                return true;
            } catch (Exception ex) {
                MessageBox.Show(
                    "An error occured\n" + ex.ToString() + "\n the document was not printed",
                    "Warning",
                    MessageBoxButtons.OK);
                return false;
            }
        }

        //<summary>
        // Provides a dialogue box if the document hasn't been printed in this session
        //</summary>
        public int deleteCheck() {
            if (!printed) {
                DialogResult dialogResult = MessageBox.Show(
                    "The file " + name + " has not been printed, are you sure you want to delete this item?",
                    "Warning",
                    MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.No) {
                    return 0;
                } else if (dialogResult == DialogResult.Cancel) {
                    return -1;
                }
            }
            deleted = true;
            return 1;
        }
    }
}
