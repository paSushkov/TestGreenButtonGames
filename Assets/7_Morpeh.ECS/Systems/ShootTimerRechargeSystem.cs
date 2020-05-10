using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ShootTimerRechargeSystem))]
public sealed class ShootTimerRechargeSystem : UpdateSystem
{

    Filter shootable;

    public override void OnAwake()
    {
        this.shootable = this.World.Filter.With<PlayerShootComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var ent in this.shootable)
        {
            ref var unit = ref ent.GetComponent<PlayerShootComponent>();
            {
                if (unit.Timer == null)
                {
                    unit.Timer = new Timer(TimerType.ShootTimer);
                }
                if (unit.Timer.Active)
                {
                    if (unit.Timer.Ready)
                    { continue; }
                    if (unit.Timer.Current <= unit.Timer.End)
                    { 
                        unit.Timer.Ready = true;
                        unit.Timer.Current = unit.Timer.Start;
                    }
                    else
                    {
                        unit.Timer.Current -= deltaTime;
                    }
                }
            }
        }
    }
}