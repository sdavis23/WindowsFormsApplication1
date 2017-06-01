using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;
using System.Net.Mail;
using System.Net;


/* 
 *  Class responsible form communicating with the outside world the results
 *  of the process automation. 
 * 
 * 
 * Author: Sean Davis
 * Date: May 18th 2017
 * Version: 1.0
 */

namespace WindowsFormsApplication
{
   public class Communicator
    {

        public String EmailAddress;
        public String Project_Path;
        public String scan_dir;
        private bool invalid_email;
        
        private string DomainMapper(Match match)
        {
            // IdnMapping class with default property values.
            IdnMapping idn = new IdnMapping();

            string domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid_email = true;
            }
            return match.Groups[1].Value + domainName;
        }

        /*
         * Checks to see if the email, represented by strIn 
         * is in fact going to work as an email address.
         * 
         * Author: Sean Davis
         * Date: May 18th 2016 
         */
        public bool IsValidEmail(string strIn)
        {
            invalid_email = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", this.DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }

            if (invalid_email)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn,
                      @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                      @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                      RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        public Communicator()
        {

        }

        public Communicator(String Email)
        {
          
            if(IsValidEmail(Email))
            {
                this.EmailAddress = Email;
            }
            else
            {
                throw new IOException("Invalid Email entered");
            }


        }

        public void sendMessage(OPPROC process)
        {
            String job_string;
            String body_string;

            if(process == OPPROC.PREPROCESS_SCANS)
            {
                job_string = "PreProcessing";
                body_string = "The preprocessing phase of the scan. Ready for registration.";
            }
            else
            {

                job_string = "Apply Pictures and Export";
                body_string = "Applying the pictures, filtering the scans and exporting the project. We're ready to move it forward.";
            }


            var fromAddress = new MailAddress("seadav.17@gmail.com", "Automation");
            var toAddress = new MailAddress(EmailAddress, "Sean Davis");
            const string fromPassword = "mpgbxjiiwzjpviux";
            string subject = "SCENE - Automation Done (" + job_string + ")";
            string body = "Scene has finished : " + body_string;

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }

        }

    }
}
