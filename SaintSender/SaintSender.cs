using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MimeKit;
using System.Threading;

namespace SaintSender
{
    public partial class SaintSender : Form
    {
        Account account;
        List<MimeKit.MimeMessage> messageList;

        public SaintSender()
        {
            InitializeComponent();

        }

        private void SaintSender_Load(object sender, EventArgs e)
        {
            this.MaximizeBox = false;
            account = Account.Deserialize();
            if (!account.GetLoginStatus())
            {
                ToggleElements();
                txtEmailAddress.Text = "kovacs.adam.mate.test@gmail.com";
                txtPassword.Text = "codecooltest";


            }
            getNewMails();
            refreshMails();

        }

        private void btnSendEmail_Click(object sender, EventArgs e)
        {
            FrmSendMail sendMail = new FrmSendMail();
            sendMail.Show();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            account.LogOut();
            ToggleElements();
            txtEmailAddress.Text = "kovacs.adam.mate.test@gmail.com";
            txtPassword.Text = "codecooltest";

        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            if (RegexValidation.IsItEmail(txtEmailAddress.Text))
            {
                try
                {

                    Account account = new Account(txtEmailAddress.Text, txtPassword.Text);
                    account.Login();
                    ToggleElements();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    MessageBox.Show(ex.Message);
                }
            }
            if (!RegexValidation.IsItEmail(txtEmailAddress.Text))
                MessageBox.Show("Not a valid email");

        }

        public void ToggleElements()
        {
            pnlMain.Visible = !pnlMain.Visible;
            pnlLogIn.Visible = !pnlLogIn.Visible;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(getNewMails));
            Thread main = Thread.CurrentThread;
            t.Start();
            refreshMails();
        }
        
        private void getNewMails()
        {
            messageList = account.GetMailsFromInbox();
        }

        private void refreshMails()
        {
            listViewEmails.Items.Clear();
            
            for (int i = messageList.Count - 1; i != 0; i--)
            {

                ListViewItem mail = new ListViewItem(messageList[i].From.ToString(),0);
                ListViewItem.ListViewSubItem[] subItems = new ListViewItem.ListViewSubItem[]
                          {
                                new ListViewItem.ListViewSubItem(mail, messageList[i].Date.ToString()),
                                new ListViewItem.ListViewSubItem(mail, messageList[i].Subject),
                                new ListViewItem.ListViewSubItem(mail, messageList[i].TextBody),
                          };
                mail.SubItems.AddRange(subItems);
                listViewEmails.Items.Add(mail);
            }
            listViewEmails.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            account.ArchiveMails(messageList);
        }

        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            Thread t = new Thread(new ThreadStart(getNewMails));
            Thread main = Thread.CurrentThread;
            t.Start();
            refreshMails();
        }
    }
}