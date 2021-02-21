using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NovaWebSolution.Services
{
    public static class ToastrNotificationService
    {
        public static string RenderNotifications()
        {
            if (HttpContext.Current.Session["Notifications"] == null) return null;
            string jsBodyOpen = "<script>";
            string jsBody = "";
            string jsBodyClose = "</script>";

            var notifications = JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());
            foreach (var note in notifications)
            {
                if (note.NotificationType == NotificationType.Error)
                {
                    jsBody += $"toastr.error('{note.Message}', '{note.Title}');";
                }
                else if (note.NotificationType == NotificationType.Success)
                {
                    jsBody += $"toastr.success('{note.Message}', '{note.Title}');";
                }
                else if (note.NotificationType == NotificationType.Info)
                {
                    jsBody += $"toastr.info('{note.Message}', '{note.Title}');";
                }
                else if (note.NotificationType == NotificationType.Warning)
                {
                    jsBody += $"toastr.warning('{note.Message}', '{note.Title}');";
                }
            }
            Clear();
            return string.Join(" ", new string[] { jsBodyOpen, jsBody, jsBodyClose });
        }

        /// <summary>
        /// Clears session["Notifications"] object
        /// </summary>
        public static void Clear()
        {
            HttpContext.Current.Session["Notifications"] = null;
        }

        public static void AddNotification(Notification notification)
        {
            var note = new Notification()
            {
                Message = notification.Message,
                NotificationType = notification.NotificationType,
                Title = notification.Title
            };
            var notifications = new List<Notification>();

            if (HttpContext.Current.Session["Notifications"] != null)
            {
                notifications = JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());
            }
            notifications.Add(note);

            HttpContext.Current.Session["Notifications"] = JsonConvert.SerializeObject(notifications);
        }

        public static void AddSuccessNotification(string message, string title)
        {
            var note = new Notification()
            {
                Message = message,
                NotificationType = NotificationType.Success,
                Title = title
            };
            var notifications = new List<Notification>();

            if (HttpContext.Current.Session["Notifications"] != null)
            {
                notifications = JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());
            }
            notifications.Add(note);

            HttpContext.Current.Session["Notifications"] = JsonConvert.SerializeObject(notifications);
        }

        public static void AddInfoNotification(string message, string title)
        {
            var note = new Notification()
            {
                Message = message,
                NotificationType = NotificationType.Info,
                Title = title
            };
            var notifications = new List<Notification>();

            if (HttpContext.Current.Session["Notifications"] != null)
            {
                notifications = JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());
            }
            notifications.Add(note);

            HttpContext.Current.Session["Notifications"] = JsonConvert.SerializeObject(notifications);
        }

        public static void AddWarningNotification(string message, string title)
        {
            var note = new Notification()
            {
                Message = message,
                NotificationType = NotificationType.Warning,
                Title = title
            };
            var notifications = new List<Notification>();

            if (HttpContext.Current.Session["Notifications"] != null)
            {
                notifications = JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());
            }
            notifications.Add(note);

            HttpContext.Current.Session["Notifications"] = JsonConvert.SerializeObject(notifications);
        }

        public static void AddErrorNotification(string message, string title)
        {
            var note = new Notification()
            {
                Message = message,
                NotificationType = NotificationType.Error,
                Title = title
            };
            var notifications = new List<Notification>();

            if (HttpContext.Current.Session["Notifications"] != null)
            {
                notifications = JsonConvert.DeserializeObject<List<Notification>>(HttpContext.Current.Session["Notifications"].ToString());
            }
            notifications.Add(note);

            HttpContext.Current.Session["Notifications"] = JsonConvert.SerializeObject(notifications);
        }
    }

    public enum NotificationType
    {
        Success,
        Error,
        Warning,
        Info
    }

    public class Notification
    {
        public string Message { get; set; }
        public NotificationType NotificationType { get; set; }
        public string Title { get; set; }
    }
}