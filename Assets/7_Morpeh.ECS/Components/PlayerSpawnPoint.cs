using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Realtime;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct PlayerSpawnPoint : IComponent {
    public Transform transform;
    public Team Team;
}