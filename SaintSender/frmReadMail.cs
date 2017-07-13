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
    public partial class frmReadMail : Form
    {
        private MimeKit.MimeMessage message;

        public frmReadMail(MimeKit.MimeMessage message)
        {
            InitializeComponent();
            this.message = message;
        }

        private void frmReadMail_Load(object sender, EventArgs e)
        {
            txtFrom.Text = message.From.ToString();
            txtSubject.Text = message.Subject;
            txtBody.Text = message.TextBody;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReply_Click(object sender, EventArgs e)
        {
            FrmSendMail sndr = new FrmSendMail(message);
            sndr.Show();
        }
    }
}
