using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class MatchPanelScript : MonoBehaviourPunCallbacks
{
    public GameObject[] UIStatusElements = new GameObject[1];
    public GameObject StatusPanel;


    public void onBtnClick_FindMatch()
    {
        if (!PhotonNetwork.InRoom)
        {
        PhotonNetwork.JoinRandomRoom();
        }
    }
    public void onBtnClick_StopSearch()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            MasterManager.GameStatus = GameStatusEnum.InLobbyIddle;
        }
    }
    public override void OnJoinedRoom()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            // Waiting for other players
            MasterManager.GameStatus = GameStatusEnum.InLobbySearch;
        }
        else
        {
            // Waiting till master load scene 
            MasterManager.GameStatus = GameStatusEnum.InMatch;
            // Load scene in case if room was created for 1 player
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.CurrentRoom.IsOpen = false;
                PhotonNetwork.CurrentRoom.IsVisible = false;
                MasterManager.GameStatus = GameStatusEnum.InMatch;
                MasterManager.LevelManager.LoadRandomGameLevel();
            }
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = MasterManager.GameSettings.MaxPlayers});
        }
    }
    public override void OnPlayerEnteredRoom(Player other)
    {
        if (PhotonNetwork.IsMasterClient && (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers))
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
            MasterManager.GameStatus = GameStatusEnum.InMatch;
            MasterManager.LevelManager.LoadRandomGameLevel();
        }
    }
}
