using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(HealthSystem))]
public sealed class HealthSystem : UpdateSystem
{
    Filter filter;
    public override void OnAwake()
    {
        filter = this.World.Filter.With<HealthComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {

        foreach (var ent in filter)
        {
            ref var healthComp = ref ent.GetComponent<HealthComponent>();
            {
                if (healthComp.Health <= 0)
                {
                    if (healthComp.Owner== PhotonNetwork.LocalPlayer && healthComp.Transform!=null)
                    {
                        PhotonNetwork.Destroy(healthComp.Transform.gameObject);
                    }
                    else if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonManager.Instance.PhotonView.RPC("LocalDestroy", RpcTarget.All, healthComp.Transform.gameObject.GetPhotonView().ViewID);
                    }
                }
            }
        }

    }
}