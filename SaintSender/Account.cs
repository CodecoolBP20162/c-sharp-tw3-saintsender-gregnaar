using MailKit;
using MailKit.Net.Imap;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SaintSender
{
    [Serializable()]
    class Account
    {
        private string EmailAddress { get; set; }
        private string Password { get; set; }
        private bool LoggedIn { get; set; }

        public Account() { }
        public Account(string EmailAddress = "kovacs.adam.mate.test@gmail.com", string Password = "codecooltest")
        {
            this.EmailAddress = EmailAddress;
            this.Password = Password;
            LoggedIn = true;
        }

        public Account(SerializationInfo info, StreamingContext context)
        {
            EmailAddress = (string)info.GetValue("EmailAddress", typeof(string));
            Password = (string)info.GetValue("Password", typeof(string));
            LoggedIn = (bool)info.GetValue("Loggedin", typeof(bool));
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("EmailAddress", EmailAddress);
            info.AddValue("Password", Password);
            info.AddValue("LoggedIn", LoggedIn);
        }

        public void Serialize()
        {
            Stream stream = File.Open("user.dat", FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();

            bf.Serialize(stream, this);
            stream.Close();
        }

        public static Account Deserialize()
        {
            Stream stream = File.Open("user.dat", FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();

            Account account = (Account)bf.Deserialize(stream);
            stream.Close();
            return account;
        }

        public bool SendMail(string toWho, string subject, string body)
        {
            try
            {
                MailMessage message = new MailMessage(EmailAddress, toWho);
                message.Subject = subject;
                message.Body = body;
                SmtpClient mailer = new SmtpClient("smtp.gmail.com", 587);
                mailer.Credentials = new NetworkCredential(EmailAddress, Password);
                mailer.EnableSsl = true;
                mailer.Send(message);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public List<MimeKit.MimeMessage> GetMailsFromInbox()
        {
            List<MimeKit.MimeMessage> messageList = new List<MimeKit.MimeMessage>();

            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("imap.gmail.com", 993, true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(this.EmailAddress, this.Password);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                for (int i = 0; i < inbox.Count; i++)
                {
                    MimeKit.MimeMessage message = inbox.GetMessage(i);
                    messageList.Add(message);
                }
                client.Disconnect(true);
            }
            return messageList;
        }

        public void ArchiveMails(List<MimeKit.MimeMessage> emailList)
        {
            foreach(MimeKit.MimeMessage message in emailList)
            {
                
                string path = message.Date.ToString().Substring(0, 19).Replace(".","").Replace(":", "") + ".txt";
                Console.WriteLine(path);
                if (!File.Exists(path))
                {
                    //File.Create(path);
                    TextWriter tw = new StreamWriter(path);
                    tw.WriteLine("From: " + message.From.ToString());
                    tw.WriteLine("Date: " + message.Date.ToString());
                    tw.WriteLine("Subject: " + message.Subject.ToString());
                    tw.WriteLine("Body: \n" + message.TextBody.ToString());
                    tw.Close();
                    
                }
            }
        }

        public bool GetLoginStatus()
        {
            return LoggedIn;
        }

        public void Login()
        {
            LoggedIn = true;
            this.Serialize();
        }

        public void LogOut()
        {
            LoggedIn = false;
            this.Serialize();
        }
    }
}
