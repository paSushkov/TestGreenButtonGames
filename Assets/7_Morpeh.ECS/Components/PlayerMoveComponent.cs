using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct PlayerMoveComponent : IComponent {
    public bool NeedToMove;
    public Transform _Transofrm;
    public Rigidbody2D _Rigidbody;
    public PhotonView _PhotonView;

    public Vector3 MoveDirection;
    
    public float NormalMoveSpeed;
    public float CurrentMoveSpeed;
    
    public float rotationAngleZ;
}