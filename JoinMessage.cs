using SelfTracker.Background;
using HarmonyLib;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace SelfTracker.UI
{
    [HarmonyPatch(typeof(MonoBehaviourPunCallbacks), "OnPlayerEnteredRoom")]
    internal class JoinPatch : MonoBehaviour
    {
        private static void Prefix(Player newPlayer)
        {
            if (newPlayer != oldnewplayer)
            {
                NotifiLib.SendNotification("<color=yellow> [INFO] </color><color=cyan> Player [ " + newPlayer.NickName + " ] Joined The Lobby! </color>");
                WebhookSender.SendMessageToWebhook("A Player Just Joined The Lobby! Current Player Count [ **" + PhotonNetwork.CurrentRoom.PlayerCount + "** ]");
                oldnewplayer = newPlayer;
                
            }
        }
        private static Player oldnewplayer;
    }
}
