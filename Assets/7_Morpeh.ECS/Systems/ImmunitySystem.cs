using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ImmunitySystem))]
public sealed class ImmunitySystem : UpdateSystem
{

    Filter filter;
    public override void OnAwake()
    {
        filter = this.World.Filter.With<ImmunityComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var ent in filter)
        {
            
            ref var immuneComp = ref ent.GetComponent<ImmunityComponent>();
           
            if (immuneComp.Timer == null)
            {
                immuneComp.Timer = new Timer(TimerType.ImmunityTimer);
            }

            // Timer and switch off mechanism 
            if (immuneComp.ActiveImmunity && immuneComp.Timer.Active)
            {
                if (immuneComp.Timer.Current > immuneComp.Timer.End)
                {
                    immuneComp.Timer.Current -= deltaTime;
                }
                else
                {
                    immuneComp.ActiveImmunity = false;
                    immuneComp.Timer.Current = immuneComp.Timer.Start;
                }
            }
            
            // Visuals
            if (immuneComp.Transform.gameObject.GetPhotonView().IsMine)
            {
                if (immuneComp.VisualGO == null)
                {
                    immuneComp.VisualGO  = PhotonNetwork.Instantiate("Immunity", immuneComp.Transform.position, Quaternion.identity);
                }


                if (immuneComp.ActiveImmunity)
                {
                    if (immuneComp.VisualGO.active)
                        immuneComp.VisualGO.transform.position = immuneComp.Transform.position;
                    else
                    {
                        PhotonManager.Instance.PhotonView.RPC("SetActive", RpcTarget.All, immuneComp.VisualGO.GetPhotonView().ViewID, true);
                        immuneComp.VisualGO.transform.position = immuneComp.Transform.position;
                    }
                }
                else
                {
                    PhotonManager.Instance.PhotonView.RPC("SetActive", RpcTarget.All, immuneComp.VisualGO.GetPhotonView().ViewID, false);
                }
            }
            
        }
    }
}