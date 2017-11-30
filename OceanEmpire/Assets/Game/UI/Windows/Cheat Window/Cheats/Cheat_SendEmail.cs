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
        string completeText = "Device info:\n"
            + "ID: " + SystemInfo.deviceUniqueIdentifier
            + "\nName: " + SystemInfo.deviceName
            + "\nModel: " + SystemInfo.deviceModel
            + "\nSupport Gyroscope: " + SystemInfo.supportsGyroscope
            + "\nSupport Accelerometer: " + SystemInfo.supportsAccelerometer;

        completeText += "\n\nTask Reports:\n\n";

        ReadOnlyCollection<TimedTaskReport> reports = History.instance.GetTaskReports();
        if (reports != null)
            for (int i = reports.Count - 1; i >= 0; i--)
            {
                completeText += reports[i].ToString() + "\n\n\n";
            }

        SendEmail(completeText);
    }

    public static void SendEmail(string body)
    {
        try
        {
            MailMessage mail = new MailMessage();

            mail.From = new MailAddress(email);
            mail.To.Add(email);
            mail.Subject = "Jeu de poisson - Device: " + SystemInfo.deviceUniqueIdentifier;
            mail.Body = body;

            SmtpClient smtpServer = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential(email, password) as ICredentialsByHost,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false                
            };
            ServicePointManager.ServerCertificateValidationCallback =
                delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
                { return true; };
            smtpServer.Send(mail);

            MessagePopup.DisplayMessage("Courriel envoy\u00E9!");
        }
        catch (Exception e)
        {
            MessagePopup.DisplayMessage("Erreur lors de l'envoie du courriel.\n\n" + e.Message);
        }
    }
}