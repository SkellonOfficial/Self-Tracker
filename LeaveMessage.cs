using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using SelfTracker.Background;

namespace SelfTracker.UI
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerLeftRoom")]
    internal class LeavePatch : MonoBehaviour
    {
        private static void Prefix(Player otherPlayer)
        {
            if (otherPlayer != PhotonNetwork.LocalPlayer && otherPlayer != a)
            {
                NotifiLib.SendNotification("<color=yellow> [INFO] </color><color=cyan> Player [ " + otherPlayer.NickName + " ] Left The Lobby! </color>");
                WebhookSender.SendMessageToWebhook("A Player Just Left The Lobby! Current Player Count [ **" + PhotonNetwork.CurrentRoom.PlayerCount + "** ]");
                a = otherPlayer;
            }
        }
        private static Player a;
    }
}