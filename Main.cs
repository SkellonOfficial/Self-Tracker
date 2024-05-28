using BepInEx;
using BepInEx.Logging;
using Photon.Pun;
using UnityEngine;
using HarmonyLib;
using SelfTracker.UI;
using System.Reflection;

namespace SelfTracker.Background
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    class Main : BaseUnityPlugin
    {
        static ManualLogSource LogMessage = BepInEx.Logging.Logger.CreateLogSource(PluginInfo.Name);
        static Rect windowRect = new Rect(0, 0, 215, 210);
        void Awake()
        {
            WebhookSender.SendMessageToWebhook(PluginInfo.UserName + " Has Launched Gorilla Tag!");
            new Harmony("selftracker").PatchAll(Assembly.GetExecutingAssembly());
        }
        void Update()
        {
            Timer--;
            if (Timer < 1 && UnityInput.Current.GetKey(KeyCode.LeftAlt) || Timer < 1 && UnityInput.Current.GetKey(KeyCode.RightAlt))
            {
                Timer = 30;
                ShowGUI = !ShowGUI;
            }
            if (PhotonNetwork.InRoom && !SentJoin)
            {
                SentJoin = true;
                try
                {
                    try
                    {
                        WebhookSender.SendMessageToWebhook(PluginInfo.UserName + " Joined A Lobby!");
                    }
                    finally
                    {
                        WebhookSender.SendMessageToWebhook("Lobby Code [ **" + PhotonNetwork.CurrentRoom.Name + "** ] ");
                        WebhookSender.SendMessageToWebhook("Gamemode [ **" + GameMode() + "** ]");
                        WebhookSender.SendMessageToWebhook("Lobby Player Count [** " + PhotonNetwork.CurrentRoom.PlayerCount + "** ] ");
                        LastLobby = PhotonNetwork.CurrentRoom.Name;
                        Log("Sent Join Lobby Message Successfully!", false);
                    }

                }
                catch
                {
                    Log("A Problem Came Up Sending The Join Lobby Message, Please Contact A Dev (skellon) To Fix The Bug!", true);
                }
            }
            if (!PhotonNetwork.InRoom && SentJoin)
            {
                SentJoin = false;
                try
                {
                    try
                    {
                        WebhookSender.SendMessageToWebhook(PluginInfo.UserName + " Left Lobby [ **" + LastLobby + "** ] ");
                        Log("Sent Leave Lobby Message Successfully!", false);
                    }
                    finally
                    {
                        LastLobby = " ";
                    }
                }
                catch
                {
                    Log("A Problem Came Up Sending The Leave Lobby Message, Please Contact A Dev (skellon) To Fix The Bug!", true);
                }
            }
            UpdateNames();
        }
        public static void Log(string message, bool error)
        {
            if (!error)
            {
                WebhookSender.SendLogToWebhook("[INFO] " + "From (" + PluginInfo.UserName + ") [ " + message + " ] ");
                LogMessage.LogInfo(message);
                NotifiLib.SendNotification("<color=yellow> [INFO] </color> <color=cyan> " + message + " </color>");
            }
            else if (error)
            {
                WebhookSender.SendLogToWebhook("[ERROR] " + "From (" + PluginInfo.UserName + ") [ " + message + " ] ");
                LogMessage.LogError(message);
                NotifiLib.SendNotification("<color=red> [ERROR] </color> <color=cyan> " + message + " </color>");
            }
        }
        bool SentJoin;
        string LastLobby = "";
        string GameMode()
        {
            string text = PhotonNetwork.CurrentRoom.CustomProperties["gameMode"].ToString();
            if (text.Contains("INFECTION"))
            {
                return "INFECTION";
            }
            if (text.Contains("HUNT"))
            {
                return "HUNT";
            }
            if (text.Contains("BATTLE"))
            {
                return "BATTLE";
            }
            if (text.Contains("CASUAL"))
            {
                return "CASUAL";
            }
            return "ERROR";
        }
        void MainGUI(int windowID)
        {
            GUI.skin.box.normal.textColor = Color.cyan;
            GUI.skin.button.normal.textColor = Color.cyan;
            GUI.skin.label.normal.textColor = Color.cyan;
            GUI.skin.button.active.textColor = Color.cyan;
            GUI.backgroundColor = Color.black;
            GUILayout.BeginVertical();
            GUILayout.Label("Message Sender", GUILayout.Width(175));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            MessageToSend = GUILayout.TextField(MessageToSend, GUILayout.Width(235), GUILayout.Height(65));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Send", GUILayout.Width(62), GUILayout.Height(26)))
            {
                if (Spoiler && !Bold)
                {
                    WebhookSender.SendMessageToWebhook("||" + MessageToSend + "||");
                }
                if (Bold && !Spoiler)
                {
                    WebhookSender.SendMessageToWebhook("**" + MessageToSend + "**");
                }
                if (Spoiler && Bold)
                {
                    WebhookSender.SendMessageToWebhook("**||" + MessageToSend + "||**");
                }
                if (!Spoiler && !Bold)
                {
                    WebhookSender.SendMessageToWebhook(MessageToSend);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            Spoiler = GUILayout.Toggle(Spoiler, SpoilerToggle, GUILayout.Width(120), GUILayout.Height(24));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            Bold = GUILayout.Toggle(Bold, BoldToggle, GUILayout.Width(120), GUILayout.Height(24));
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUI.DragWindow();
        }
        void UpdateNames()
        {
            if (Bold)
            {
                BoldToggle = "<color=green>Bold</color>";
            }
            if (!Bold)
            {
                BoldToggle = "<color=red>Bold</color>";
            }
            if (Spoiler)
            {
                SpoilerToggle = "<color=green>Spoiler</color>";
            }
            if (!Spoiler)
            {
                SpoilerToggle = "<color=red>Spoiler</color>";
            }
        }
        void OnGUI()
        {
            GUI.backgroundColor = Color.black;
            if (ShowGUI)
            {
                windowRect = GUILayout.Window(0, windowRect, MainGUI, "");
            }
            else
            {
                windowRect = new Rect(0, 0, 215, 210);
            }
        }
        bool ShowGUI;
        bool Spoiler;
        bool Bold;
        string BoldToggle = "";
        string MessageToSend = "[Put Message Here]";
        string SpoilerToggle = "";
        float Timer = 0;
    }
}
