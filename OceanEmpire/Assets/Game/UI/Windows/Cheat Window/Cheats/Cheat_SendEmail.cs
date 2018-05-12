using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.ObjectModel;

public class Cheat_SendEmail : MonoBehaviour
{
    public const string email = "jeudepoissons@gmail.com";
    public const string password = "bobo4ever";

    public void SendHistoryByEmail()
    {
        SendEmail(StartExtractingProcess.GetAllExtractableData());
    }

    public static void SendEmail(string body)
    {
        try
        {
            MailMessage mail = new MailMessage
            {
                From = new MailAddress(email)
            };
            mail.To.Add(email);
            mail.Subject = "Super-marin - Device: " + SystemInfo.deviceUniqueIdentifier;
            mail.Body = body;

            SmtpClient smtpServer = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                Credentials = new NetworkCredential(email, password) as ICredentialsByHost,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            smtpServer.Send(mail);

            MessagePopup.DisplayMessage("Courriel envoyé!");
        }
        catch (Exception e)
        {
            MessagePopup.DisplayMessage(e.Message);
        }
    }
}