using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;
using Photon.Realtime;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct BulletComponent : IComponent {
    public float Speed;
    public int Damage;
    public Rigidbody2D _Rigidbody2D;
    public Transform _Transform;
    public BoxCollider2D _BoxCollider2D;
    public Player OriginOwner;
    public Team Team;
    public PhotonView _PhotonView;
}