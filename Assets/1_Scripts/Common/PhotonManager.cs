using ExitGames.Client.Photon;
using Morpeh;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private static PhotonManager instance;
    public PhotonView PhotonView;
    // Implement better singleton
    public static PhotonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PhotonManager>();
                if (instance == null)
                {
                    instance = new GameObject("PhotonManager").AddComponent<PhotonManager>();
                }
            }
            return instance;
        }
    }
    private List<Player> LoadedPlayers;

    void Awake()
    {
        LoadedPlayers = new List<Player>();
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        PhotonView = GetComponent<PhotonView>();
    }
    public override void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.NetworkingClient.EventReceived += NetworkingClient_EventReceived;
    }
    public override void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.NetworkingClient.EventReceived -= NetworkingClient_EventReceived;
    }

    private void NetworkingClient_EventReceived(EventData obj)
    {
        if (obj.Code == (byte)EventCode.PlayerLoaded)
        {
            object[] data = (object[])obj.CustomData;
            Player test = (Player)data[0];
            LoadedPlayers.Add((Player)data[0]);
            if (LoadedPlayers.Count == PhotonNetwork.CurrentRoom.PlayerCount && PhotonNetwork.IsMasterClient)
            {
                // Sending event to start countdown
                object[] emptyMessage = new object[] { };
                PhotonNetwork.RaiseEvent((byte)EventCode.EveryoneLoaded, emptyMessage, RaiseEventOptions.Default, SendOptions.SendUnreliable);
                // Starting own countdown
                StartCountDown();
            }
        }
        if (obj.Code == (byte)EventCode.PlayerTookDamage)
        {
            object[] data = (object[])obj.CustomData;
            Player playerMSG = (Player)data[0];
            int livesNumMSG = (int)data[1];
            ChangePlayerLives(playerMSG, livesNumMSG);
        }
        if (obj.Code == (byte)EventCode.EveryoneLoaded)
        {
            StartCountDown();
        }
    }
    [PunRPC]
    private void SetTankHealthProperties(int viewId, Player owner, Team team, int health)
    {
        GameObject Tank = PhotonView.Find(viewId).gameObject;
        HealthComponentProvider healthProvider = Tank.GetComponent<HealthComponentProvider>();
        ref var healthComp = ref healthProvider.GetData();
        healthComp.Team = team;
        healthComp.Owner = owner;
        healthComp.Health = health;
    }

    [PunRPC]
    private void LocalDestroy(int viewId)
    {
        if (PhotonView.Find(viewId).gameObject != null)
        {
            GameObject.Destroy(PhotonView.Find(viewId).gameObject);
        }
    }
    [PunRPC]
    private void SetBulletParameters(int viewId, Player OriginOwner, Team Team, int Damage, float Speed)
    {
        BulletComponentProvider provider = PhotonView.Find(viewId).gameObject.GetComponent<BulletComponentProvider>();
        ref var bulletComp = ref provider.GetData();
        bulletComp.Damage = Damage;
        bulletComp.Speed = Speed;
        bulletComp.OriginOwner = OriginOwner;
        bulletComp.Team = Team;
    }

    [PunRPC]
    private void SetActive(int viewId, bool active)
    {
        PhotonView.Find(viewId).gameObject.SetActive(active);
    }
    [PunRPC]
    private void ActivateImmunityComp(int viewId)
    {
        ImmunityComponentProvider provider = PhotonView.Find(viewId).gameObject.GetComponent<ImmunityComponentProvider>();
        ref var immuneComp = ref provider.GetData();
        immuneComp.ActiveImmunity = true;
    }

    void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
    {
        // Add self to loaded list
        LoadedPlayers.Add(PhotonNetwork.LocalPlayer);

        // Notify other player`s that you are ready
        RaiseEventOptions options = RaiseEventOptions.Default;
        options.CachingOption = EventCaching.AddToRoomCache;
        object[] data = new object[] { PhotonNetwork.LocalPlayer };
        PhotonNetwork.RaiseEvent((byte)EventCode.PlayerLoaded, data, options, SendOptions.SendUnreliable);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        LoadedPlayers.Remove(otherPlayer);

        World world = World.Default;
        Filter players = world.Filter.With<PlayerComponent>();
        foreach (var ent in players)
        {
            ref var playerComp = ref ent.GetComponent<PlayerComponent>();
            if (playerComp.Player == otherPlayer)
            {
                playerComp.InRoom = false;
            }
        }


    }
    public void StartCountDown()
    {
        World world = World.Default;
        Filter filter = world.Filter.With<GameStartUITimerComponent>();
        foreach (var ent in filter)
        {
            ref var timer = ref ent.GetComponent<GameStartUITimerComponent>();
            if (timer.Timer == null)
                timer.Timer = new Timer(TimerType.StartTimer);
            timer.Timer.Active = true;
        }
        MasterManager.GameStatus = GameStatusEnum.WaitToStart;
    }

    public void ChangePlayerLives(Player player, int damage)
    {
        // To notify others to do the same
        if (PhotonNetwork.IsMasterClient)
        {
            object[] data = new object[] { player, damage };
            PhotonNetwork.RaiseEvent((byte)EventCode.PlayerTookDamage, data, RaiseEventOptions.Default, SendOptions.SendUnreliable);
        }

        World world = World.Default;
        Filter players = world.Filter.With<PlayerComponent>();
        foreach (var ent in players)
        {
            ref var playerComp = ref ent.GetComponent<PlayerComponent>();
            if (playerComp.Player == player)
            {
                playerComp.Lives -= damage;
                if (playerComp.Lives > 0 && playerComp.Tank != null)
                {
                    if (playerComp.Tank.GetPhotonView().IsMine)
                    {
                        if (playerComp.SpawnPoints.Count > 0)
                        {
                            int respawnPointID = UnityEngine.Random.Range(0, playerComp.SpawnPoints.Count);
                            playerComp.Tank.transform.position = playerComp.SpawnPoints[respawnPointID].transform.position;
                        }
                    }
                }
                else if (playerComp.Lives <= 0 && playerComp.Tank != null)
                {
                    if (playerComp.Tank.GetPhotonView().IsMine)
                    {
                        PhotonNetwork.Destroy(playerComp.Tank.gameObject);
                    }
                }
            }
        }
    }
}

public enum EventCode : byte
{
    Test = 0,
    PlayerLoaded = 1,
    EveryoneLoaded = 2,
    PlayerTookDamage = 5,
}
