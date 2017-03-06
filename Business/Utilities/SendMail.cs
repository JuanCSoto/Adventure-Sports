// <copyright file="SendMail.cs" company="Dasigno">
//     Copyright (c) 2013 All Rights Reserved
// </copyright>
// <author>Jimmy Rodriguez</author>
namespace Business
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Mail;
    using System.Threading;
    
    /// <summary>
    /// is responsible for sending email
    /// </summary>
    public class SendMail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendMail"/> class
        /// </summary>
        public SendMail() 
        {
        }

        /// <summary>
        /// Gets or sets the from address for this e-mail message.
        /// </summary>
        public string From { get; set; }

        /// <summary>
        ///  Gets or sets the address collection that contains the recipients of this e-mail message.
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the attachment used to store data attached to this e-mail
        /// message.
        /// </summary>
        public string AtachmentFilePath { get; set; }

        /// <summary>
        ///  Gets or sets the message body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the subject line for this e-mail message.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the priority of this e-mail message.
        /// </summary>
        public MailPriority Priority { get; set; }

        /// <summary>
        /// Gets or sets the address collection that contains the blind carbon copy (BCC) recipients
        /// for this e-mail message.
        /// </summary>
        public List<string> ColBcc { get; set; }
        
        /// <summary>
        /// send the email
        /// </summary>
        public void SendMessage()
        {
            if (!string.IsNullOrEmpty(this.To))
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(this.From);
                mail.To.Add(new MailAddress(this.To));
                mail.Body = this.Body;
                mail.IsBodyHtml = true;
                mail.Priority = this.Priority;
                mail.Subject = this.Subject;

                if (this.ColBcc != null)
                {
                    foreach (string item in this.ColBcc)
                    {
                        mail.Bcc.Add(item);
                    }
                }

                if (!string.IsNullOrEmpty(this.AtachmentFilePath) && File.Exists(this.AtachmentFilePath))
                {
                    mail.Attachments.Add(new System.Net.Mail.Attachment(this.AtachmentFilePath));
                }

                ThreadPool.QueueUserWorkItem(action => this.BackgroundEmailSend(mail));
            }
        }

        /// <summary>
        /// executes a send message
        /// </summary>
        /// <param name="mail"><c>mailmessage</c> object</param>
        private void BackgroundEmailSend(MailMessage mail)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Send(mail);
            }
            catch (Exception) 
            { 
            }
        }
    }
}
