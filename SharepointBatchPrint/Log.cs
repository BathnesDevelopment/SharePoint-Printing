using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharepointBatchPrint
{
    class Log
    {
        private ClientContext context;
        private List logList;

        public Log(string siteURL, string list) {
            context = new ClientContext(siteURL);
            logList = context.Web.Lists.GetByTitle(list);
        }

        public void logAction(string action, string logContent) {
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            ListItem res = logList.AddItem(itemCreateInfo);

            res["Title"] = action;
            res["Time"] = DateTime.Now;
            res["Log"] = logContent;

            res.Update();
            context.ExecuteQuery();
        }

    }
}
