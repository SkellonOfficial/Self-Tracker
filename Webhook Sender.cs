using BepInEx;
using SelfTracker.UI;
using System.Net;
using System.Text;

namespace SelfTracker.Background
{
    [BepInPlugin("org.skellon.gorillatag.webhooksender", "Skellons Webhook Sender", "1.0.0")]
    class WebhookSender : BaseUnityPlugin
    {
        void Awake()
        {
            if (LoggerURL == "N/A")
            {
                Log = false;
            }
            else
            {
                Log = true;
            }
        }
        public static void SendMessageToWebhook(string message)
        {
            try
            {
                WebClient client = new WebClient();
                client.Headers.Add("Content-Type", "application/json");
                string payload = "{\"content\": \"" + message + "\"}";
                client.UploadData(webHookURL, Encoding.UTF8.GetBytes(payload));
            }
            catch
            {
                Main.Log("Error Sending Message [ " + message + " ] Could Be A Webhook Error?", true);
            }
        }
        public static void SendLogToWebhook(string log)
        {
            if (Log)
            {
                try
                {
                    WebClient client = new WebClient();
                    client.Headers.Add("Content-Type", "application/json");
                    string payload = "{\"content\": \"" + log + "\"}";
                    client.UploadData(LoggerURL, Encoding.UTF8.GetBytes(payload));
                }
                catch
                {
                    UnityEngine.Debug.LogError("Error Sending Log To LoggerURL, Check For Mistakes In The File!");
                    NotifiLib.SendNotification("<color=red> [ERROR] </color> <color=cyan> Error Sending Log To LoggerURL, Check For Mistakes In The File! </color>");
                }
            }
        }
        static string webHookURL = "nuh uh";
        static string LoggerURL = "N/A";
        static bool Log = false;
    }
}
