using Photon.Pun;
using Photon.Realtime;
using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(PlayerInitializer))]

// This Initializer is to:
// 1. Create enity for each player in room (every client creates it`s own "list" of players!)
// !!! Master client manages bullet collisions. In case of his disconnection another player becomes Master, so he should have a copy of data
// 2. For each created enity add PlayerComponent, then:
//                                                      * Assign link to Player (Photon) in room, 
//                                                      * Assign Team, 
//                                                      * Assign Lives number,
//                                                      * Assign Tank level (NOT IMPLEMENTED YET),
//                                                      * Checks every spawn point if it match player`s team. In case of match - add`s it to the player`s list of spawn points
//                                                      * Spawn tank GameObject (via Photon) 
//
public sealed class PlayerInitializer : Initializer
{
    Filter spawnPointFilter;
    public override void OnAwake()
    {
        // Creating entity for each player in room
        foreach (Player networkPlayer in PhotonNetwork.PlayerList)
        {
            var ent = this.World.CreateEntity();
            ent.AddComponent<PlayerComponent>();
            ref var player = ref ent.GetComponent<PlayerComponent>();

            // Assignig parameters
            player.Player = networkPlayer;
            player.InRoom = true;
            player.Lives = MasterManager.GameSettings.PlayersLives;             // TODO make custom property in room.Properies???
            player.TankLevel = 1;                                               // TODO implement tank upgrade system
            // Assigning team
            if (networkPlayer.ActorNumber / 2 == 1)
            {
                player.Team = Team.Blue;
            }
            else
            {
                player.Team = Team.Red;
            }
            // Adding spawn points to player`s list 
            player.SpawnPoints = new List<PlayerSpawnPoint>();
            spawnPointFilter = this.World.Filter.With<PlayerSpawnPoint>();
            foreach (var pointEntity in this.spawnPointFilter)
            {
                ref var point = ref pointEntity.GetComponent<PlayerSpawnPoint>();
                if (point.Team == player.Team)
                {
                    player.SpawnPoints.Add(point);
                }
            }
            // Spawning Tank GameObject for player
            // Check if enity links to localPlayer
            if (player.Player == PhotonNetwork.LocalPlayer)
            {
                // Checks if we have point to spawn Tank GameObject
                if (player.SpawnPoints.Count > 0)
                {
                    // Determine spawn point;
                    int SpawnPointID = UnityEngine.Random.Range(0, player.SpawnPoints.Count - 1);
                    PlayerSpawnPoint pointToSpawn = player.SpawnPoints[SpawnPointID];
                    // Spawn tank gameobject via Photon
                    player.Tank = PhotonNetwork.Instantiate("Player", pointToSpawn.transform.position, Quaternion.identity);
                    PhotonManager.Instance.PhotonView.RPC("SetTankHealthProperties", RpcTarget.All, player.Tank.transform.gameObject.GetPhotonView().ViewID, player.Player, player.Team, player.Lives);
                }
                else
                {
                    Debug.LogError("Not found any spawn points for "+ Enum.GetName(typeof(Team), player.Team)+" team in scene: " + SceneManager.GetActiveScene().name);
                }
            }
        }
    }
    public override void Dispose()
    {
    }
}