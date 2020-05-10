using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ConnectToServerScript : MonoBehaviourPunCallbacks
{
    public Image icon;
    public Text text;
    public Sprite[] iconVariants = new Sprite[2];

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        PhotonNetwork.SendRate = 40;
        PhotonNetwork.SerializationRate = 20;

        if (text != null)
            text.text = "Connecting to server";
        try
        {
            icon.sprite = iconVariants[0];
        }
        catch (NullReferenceException ex)
        {
            Debug.LogWarning(ex.Message);
        }
    }

    void Start()
    {
        PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        if (icon != null)
            RotateOverTime.CreateComponent(icon.gameObject, Vector3.forward, -45f);
        MasterManager.GameStatus = GameStatusEnum.TryingToConnect;
        Connect();
    }
    public override void OnConnectedToMaster()
    {
        MasterManager.GameStatus = GameStatusEnum.Connected;
        if (text != null)
            text.text = "Connected!";
        try
        {
            icon.rectTransform.rotation = Quaternion.identity;
            icon.sprite = iconVariants[1];
            icon.gameObject.GetComponent<RotateOverTime>().enabled = false;
        }
        catch (NullReferenceException ex)
        {
            Debug.LogWarning(ex.Message);
        }
        MasterManager.LevelManager.LoadScene("Lobby");
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        MasterManager.GameStatus = GameStatusEnum.ConnectionLost;
        Debug.Log("Disconnected from server for reason " + cause.ToString());
    }
    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
            PhotonNetwork.GameVersion = MasterManager.GameSettings.GameVersion;
        }
    }
}
