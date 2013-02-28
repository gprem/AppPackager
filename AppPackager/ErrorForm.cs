using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AppPackager
{
    public partial class ErrorForm : Form
    {
        private List<string> errorList = null;
        public ErrorForm()
        {
            InitializeComponent();
        }

        public string ErrorMessage
        {
            get { return this.lblErrorMessage.Text; }
            set { this.lblErrorMessage.Text = value; }
        }

        public List<string> ErrorDetails
        {
            get { return this.errorList; }
            set
            {
                this.errorList = value;
            }
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {
            this.tbErrorList.Clear();

            if (this.errorList == null)
                return;

            foreach (var errStr in this.errorList)
            {
                this.tbErrorList.Text += string.Concat(errStr, Environment.NewLine);
            }
        }
    
    }
}
