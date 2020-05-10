using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;
using System.Collections;
using Photon.Realtime;
using ExitGames.Client.Photon;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(BulletSystem))]
public sealed class BulletSystem : FixedUpdateSystem
{
    Filter bulletFilter;
    Filter players;

    int colliderArraySize = 2;
    Collider2D[] colliders;
    int contacts = 1;
    public override void OnAwake()
    {
        colliders = new Collider2D[colliderArraySize];
        bulletFilter = this.World.Filter.With<BulletComponent>();
        players = this.World.Filter.With<PlayerComponent>();
    }
    public override void OnUpdate(float deltaTime)
    {
        bulletFilter.Update();
        foreach (var bulletEnt in this.bulletFilter)
        {
            ref var bullet = ref bulletEnt.GetComponent<BulletComponent>();

            // All bullets ownership transfers to master at the moment of instantiation
            if (bullet._PhotonView.IsMine)
            {
                // Moving bullet
                bullet._Rigidbody2D.MovePosition(bullet._Transform.position + bullet._Transform.up * bullet.Speed * deltaTime);

                // Detecting collision contacts
                contacts = bullet._BoxCollider2D.GetContacts(colliders);

                // Looking through collider array in case bullet collides with GameObject that have mark
                if (contacts > 0)
                {
                    for (int i = 0; i < contacts; i++)
                    {
                        // Checking each collider if it have Health component
                        if (colliders[i].GetComponent<HealthComponentProvider>() != null)
                        {
                            HealthComponentProvider healthProvider = colliders[i].GetComponent<HealthComponentProvider>();
                            ref var healthComp = ref healthProvider.GetData();

                            // Checking if collider health component marked as another team
                            if (bullet.Team != healthComp.Team)
                            {
                                // Checking if it can be immune
                                bool canBeImmune = colliders[i].GetComponent<ImmunityComponentProvider>() == null ? false : true;
                                if (canBeImmune)
                                {
                                    ImmunityComponentProvider immunityProvider = colliders[i].GetComponent<ImmunityComponentProvider>();
                                    ref var immuneComponent = ref immunityProvider.GetData();
                                    
                                    // If immune at this moment - check next collider. Else - enable immunity
                                    if (immuneComponent.ActiveImmunity)
                                        continue;
                                    else
                                        PhotonManager.Instance.PhotonView.RPC("ActivateImmunityComp", RpcTarget.All, immuneComponent.Transform.gameObject.GetPhotonView().ViewID);
                                }
                                // Deal damage to the GO`s component
                                healthComp.Health -= bullet.Damage;
                                // If it belongs to someone - change someone`s lives count and synchronize data via event
                                if (healthComp.Owner != null)
                                {
                                    PhotonManager.Instance.ChangePlayerLives(healthComp.Owner, bullet.Damage);
                                    PhotonNetwork.Instantiate("Bang", bullet._Transform.position, Quaternion.identity);
                                }
                            }
                        }
                    }
                    PhotonNetwork.Destroy(bullet._Transform.gameObject);
                }
            }
        }
    }
}