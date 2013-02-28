using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppPackager
{
    public static class UIHelper
    {
        public static void ShowInformation(string infomsg)
        {
            ShowInformation(infomsg, System.Windows.Forms.Application.ProductName);
        }

        public static void ShowInformation(string infomsg, string title)
        {
            MessageBox.Show(infomsg, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowError(string errorMsg)
        {
            ShowInformation(errorMsg, System.Windows.Forms.Application.ProductName);
        }

        public static void ShowError(string errorMsg, string title)
        {
            MessageBox.Show(errorMsg, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
