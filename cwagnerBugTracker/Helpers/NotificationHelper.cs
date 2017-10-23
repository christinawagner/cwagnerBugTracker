using cwagnerBugTracker.Hubs;
using cwagnerBugTracker.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Net.Mail;

namespace cwagnerBugTracker.Helpers
{
    public class NotificationHelper
    {
        ApplicationDbContext db = new ApplicationDbContext();
        Notification Notification = new Notification();

        public void Notify(string userId, string subject, string message, bool sendEmail)
        {

            var notification = new Notification
            {
                Subject = subject,
                Message = message,
                IsNew = true,
                RecipientId = userId,
                Sent = DateTimeOffset.UtcNow,
            };

            db.Notifications.Add(notification);
            db.SaveChanges();

            NotificationHub.UpdateNotifications(userId);

            if (sendEmail)
            {
                try
                {
                    var user = db.Users.Find(userId);

                    var from = "CWagnerBugTracker<BugTrackerDONOTREPLY@email.com>";
                    var email = new MailMessage(from, user.Email)
                    {
                        IsBodyHtml = true,
                        Subject = subject,
                        Body = message,
                    };

                    var svc = new PersonalEmail();
                    svc.Send(email);
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}