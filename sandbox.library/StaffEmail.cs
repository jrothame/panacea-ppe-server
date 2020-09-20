using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;

namespace sandbox.library
{
    public class EmailItem
    {
        public string email { get; set; }
        public EmailItem() { }
    }

    public class StaffEmail
    {
        public string from_address { get; set;}
        public EmailItem[] to_address { get; set; }  //maybe a string delimited with semicolons or commas
        public string from_address_display { get; set; }
        public string subject { get; set; }
        public string body { get; set; }
        public string entryUrl { get; set; }
        public bool emailSent { get; set; }
        public string start_name { get; set; }
        public string end_name { get; set; }
        public string buildingName { get; set; }
        public string buildingImgUrl { get; set; }
        public string buildingId { get; set; }
        public string language { get; set; }
        public string navigateLinkText { get; set; }
        public List<string> failed_recipients { get; set; }

        private string smtpHost = ConfigurationManager.AppSettings["smtpHost"];
        private string smtpPort = ConfigurationManager.AppSettings["smtpPort"];
        private string smtpUser = ConfigurationManager.AppSettings["smtpUser"];
        private string smtpPwd = ConfigurationManager.AppSettings["smtpPwd"];


        public StaffEmail() { }

        public bool Send()
        {
            failed_recipients = new List<string>();
            
            SmtpClient sc = new SmtpClient();           
            sc.UseDefaultCredentials = true;
            sc.Host = smtpHost;  //"smtp.gmail.com";
            sc.Port = Convert.ToInt32(smtpPort);
            sc.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPwd);
            sc.EnableSsl = true;   // cbsecure.Checked;

            from_address = "photonavigation@eyedog.us"; 
            from_address_display = "photonavigation@eyedog.us";
            //subject = String.Format("Navigation from '{0}' to '{1}'", start_name, end_name);

            //get recipients emails
            var emailArray = to_address;
            foreach(var item in emailArray)
            {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress(from_address, from_address_display);
                msg.To.Add(new MailAddress(item.email));

                msg.Subject = subject;

                msg.Body = body;

                msg.IsBodyHtml = true;

                //msg.ReplyTo = "info@somecompany.com";
                
                try
                {
                    sc.Send(msg);
                }
                catch (SmtpFailedRecipientsException sfex)
                {
                    failed_recipients.Add(item.email);
                    for (int i = 0; i < sfex.InnerExceptions.Length; i++)
                    {
                        SmtpStatusCode status = sfex.InnerExceptions[i].StatusCode;
                        if (status == SmtpStatusCode.MailboxBusy ||
                            status == SmtpStatusCode.MailboxUnavailable)
                        {
                            LogWriter.Instance.WriteToLog(LogMessageType.ERROR, "Delivery failed - retrying in 5 seconds.");
                            System.Threading.Thread.Sleep(5000);
                            sc.Send(msg);
                        }
                        else
                        {
                            LogWriter.Instance.WriteToLog(LogMessageType.ERROR, 
                                String.Format("Failed to deliver message to {0}", sfex.InnerExceptions[i].FailedRecipient));
                        }
                    }

                    LogWriter.Instance.WriteToLog(LogMessageType.ERROR, "Email recipient: " + item.email + " failed for " + sfex.Message);
                }
                catch (Exception ex)
                {
                    LogWriter.Instance.WriteToLog(LogMessageType.ERROR, "StaffEmail.Send() encountered exception: " + ex.Message);
                }

            }
            sc.Dispose();

            if (failed_recipients.Count > 0)
                emailSent = false;
            else
                emailSent = true;

            return true;
        }

        
    }
}
