using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Realtime;
using System.Collections.Generic;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct PlayerComponent : IComponent {
    public Player Player;
    public bool InRoom;
    public Team Team;
    public int Lives;
    public int TankLevel;
    public GameObject Tank;
    public List<PlayerSpawnPoint> SpawnPoints;
}