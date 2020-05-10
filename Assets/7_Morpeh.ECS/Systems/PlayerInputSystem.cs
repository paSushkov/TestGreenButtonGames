using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(PlayerInputSystem))]
public sealed class PlayerInputSystem : UpdateSystem
{
    Filter movable;
    Filter shootable;
    Filter players;

    KeyCode moveUpButton = KeyCode.W;
    KeyCode moveDownButton = KeyCode.S;
    KeyCode moveLeftButton = KeyCode.A;
    KeyCode moveRightButton = KeyCode.D;
    KeyCode shootButton = KeyCode.Space;

    public override void OnAwake()
    {
        this.movable = this.World.Filter.With<PlayerMoveComponent>();
        this.shootable = this.World.Filter.With<PlayerShootComponent>();
        this.players = this.World.Filter.With<PlayerComponent>();
    }
    private void setDirectionAndSpeed()
    {
        int xDir = 0;
        int yDir = 0;
        float rotationAngle = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            yDir = 1;
            rotationAngle = 0f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            yDir = -1;
            rotationAngle = 180f;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            xDir = -1;
            rotationAngle = 90f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xDir = 1;
            rotationAngle = 270f;
        }
        foreach (var ent in this.movable)
        {
            ref var unit = ref ent.GetComponent<PlayerMoveComponent>();
            if (unit._PhotonView.IsMine)
            {
                unit.MoveDirection.x = xDir;
                unit.MoveDirection.y = yDir;
                unit.rotationAngleZ = rotationAngle;
                unit.CurrentMoveSpeed = unit.NormalMoveSpeed;
                unit.NeedToMove = true;
            }
        }
    }
    private void Shoot()
    {
        foreach (var ent in this.shootable)
        {
            ref var unit = ref ent.GetComponent<PlayerShootComponent>();
            if (unit.Timer == null)
            {
                unit.Timer = new Timer(TimerType.ShootTimer);
            }
            if (unit._PhotonView.IsMine && unit.Timer.Ready)
            {
                GameObject newBullet = PhotonNetwork.Instantiate("Bullet", unit._Transofrm.position + unit._Transofrm.up * 0.33f, unit._Transofrm.rotation);
                
                foreach (var PlayerEnt in players)
                {
                    ref var player = ref PlayerEnt.GetComponent<PlayerComponent>();
                    if (player.Player == unit._PhotonView.Owner)
                    {
                        var compProvider = newBullet.GetComponent<BulletComponentProvider>();
                        ref var bulletComp = ref compProvider.GetData();
                        bulletComp.OriginOwner = unit._PhotonView.Owner;
                        bulletComp.Team = player.Team;
                        bulletComp.Damage = MasterManager.GameSettings.BulletDamage;
                    }
                }
                newBullet.GetPhotonView().TransferOwnership(PhotonNetwork.MasterClient);
                
                unit.Timer.Ready = false;
            }
        }
    }

    public override void OnUpdate(float deltaTime)
    {
        if (Input.anyKey)
        {
            if (MasterManager.GameStatus == GameStatusEnum.GameplayProcess)
            {
                if (Input.GetKey(moveUpButton) || Input.GetKey(moveDownButton) || Input.GetKey(moveLeftButton) || Input.GetKey(moveRightButton) || Input.GetKey(moveRightButton))
                {
                    setDirectionAndSpeed();
                }
                if (Input.GetKeyDown(shootButton))
                {
                    Shoot();
                }
            }
        }
    }
}