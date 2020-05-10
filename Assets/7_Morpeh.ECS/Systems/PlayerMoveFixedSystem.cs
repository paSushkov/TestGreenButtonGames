using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerMoveFixedSystem))]
public sealed class PlayerMoveFixedSystem : FixedUpdateSystem
{
    Filter commonFilter;

    public override void OnAwake()
    {
        commonFilter = this.World.Filter.With<PlayerMoveComponent>();
    }
    public override void OnUpdate(float deltaTime)
    {
        foreach (var entity in this.commonFilter)
        {
            ref var unit = ref entity.GetComponent<PlayerMoveComponent>();
            if (unit._PhotonView.IsMine && unit.NeedToMove)
            {
                unit._Rigidbody.MovePosition(unit._Transofrm.position + unit.MoveDirection * unit.CurrentMoveSpeed * deltaTime);
                unit._Transofrm.rotation = Quaternion.Euler(0, 0, unit.rotationAngleZ);
                unit.NeedToMove = false;
            }
        }
    }
}