﻿using D365DeveloperExtensions.Core.Models;
using D365DeveloperExtensions.Core.Resources;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Linq;
using System.Text;
using EnvDTE;

namespace D365DeveloperExtensions.Core
{
    public static class HostWindow
    {
        public static string GetCaption(string caption, CrmServiceClient client)
        {
            string[] parts = caption.Split('|');

            StringBuilder sb = new StringBuilder();
            sb.Append(parts[0]);

            var url = client?.ConnectedOrgUniqueName == null
                ? Resource.HostWindow_SetCaption_NotConnected
                : WebBrowser.GetBaseCrmUrlFomClient(client).ToString();

            if (!string.IsNullOrEmpty(url))
            {
                sb.Append($" | {Resource.HostWindow_SetCaption_ConnectedTo}: ");

                if (url.EndsWith("/"))
                    url = url.TrimEnd('/');

                sb.Append(url);
            }

            string version = client?.ConnectedOrgVersion.ToString() ?? "";
            if (!string.IsNullOrEmpty(version))
            {
                sb.Append($" | {Resource.HostWindow_SetCaption_Version}: ");
                sb.Append(version);
            }

            return sb.ToString();
        }

        public static bool IsD365DevExWindow(EnvDTE.Window window)
        {
            if (window.ObjectKind == null)
                return false;

            Guid windowGuid = new Guid(StringFormatting.RemoveBracesToUpper(window.ObjectKind));

            return ExtensionConstants.D365DevExToolWindows.Count(w => w.ToolWindowsId == windowGuid) > 0;
        }

        public static ToolWindow GetD365DevExWindow(EnvDTE.Window window)
        {
            if (window.ObjectKind == null)
                return null;

            Guid windowGuid = new Guid(StringFormatting.RemoveBracesToUpper(window.ObjectKind));

            return ExtensionConstants.D365DevExToolWindows.FirstOrDefault(w => w.ToolWindowsId == windowGuid);
        }

        public static bool IsCrmDexWindowOpen(DTE dte)
        {
            foreach (Window window in dte.Windows)
            {
                if (IsD365DevExWindow(window))
                    return true;
            }

            return false;
        }
    }
}