using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BangObjectsSystem))]
public sealed class BangObjectsSystem : UpdateSystem {

    Filter filter;

    public override void OnAwake() {
        filter = this.World.Filter.With<BangComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        filter.Update();
        foreach (var ent in filter)
        {
            ref var bang = ref ent.GetComponent<BangComponent>();
            if (bang.PhotonView.IsMine)
            {
                if (bang.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !bang.Animator.IsInTransition(0))
                {
                    PhotonNetwork.Destroy(bang.Transform.gameObject);
                }
            
            }
        }
    }
}