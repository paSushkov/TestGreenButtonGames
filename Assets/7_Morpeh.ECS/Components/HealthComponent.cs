using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Realtime;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct HealthComponent : IComponent {
    public int Health;
    public Transform Transform;
    public GameObject GameObject;
    public Player Owner;
    public Team Team;
}