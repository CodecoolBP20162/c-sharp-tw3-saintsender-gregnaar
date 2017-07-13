using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaintSender
{
    public partial class FrmSendMail : Form
    {
        Account account;

        public FrmSendMail()
        {
            InitializeComponent();
        }

        private void FrmSendMail_Load(object sender, EventArgs e)
        {
            account = Account.Deserialize();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (account.SendMail(txtTo.Text, txtSubject.Text, txtBody.Text))
            {
                MessageBox.Show("Message Sent!");
            }
            else
            {
                MessageBox.Show("Oops someone shot the messanger! Please contact IT department.");
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
