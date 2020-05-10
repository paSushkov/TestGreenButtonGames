using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(StartTimerSystem))]
public sealed class StartTimerSystem : UpdateSystem
{
    private Filter filter;

    public override void OnAwake()
    {
        filter = this.World.Filter.With<GameStartUITimerComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var ent in filter)
        {
            ref var timer = ref ent.GetComponent<GameStartUITimerComponent>();
            {
                if (timer.Timer == null)
                {
                    timer.Timer = new Timer(TimerType.StartTimer);
                }
                if (timer.Timer.Active && (MasterManager.GameStatus == GameStatusEnum.WaitToStart))
                {
                    timer.Timer.Current -= deltaTime;
                    timer.Text.text = timer.Timer.Current.ToString("f0");
                    if (timer.Timer.Current <= timer.Timer.End)
                    {
                        timer.Timer.Active = false;
                        timer.Timer.Ready = true;
                        timer.Timer.Current = timer.Timer.Start;
                        timer.Animator.SetTrigger("fadeOut");
                        MasterManager.GameStatus = GameStatusEnum.GameplayProcess;
                    }
 
                }
            }
        }
    }
}