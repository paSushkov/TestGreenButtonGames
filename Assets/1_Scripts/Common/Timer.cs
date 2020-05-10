public class Timer
{
    TimerType TimerType;
    public bool Active;
    public bool Ready;
    public float Start;
    public float Current;
    public float End;

    public Timer(TimerType Type)
    {
        TimerType = Type;
        switch (Type)
        {
            case (TimerType.StartTimer):
                Active = false;
                Ready = false;
                Start = 3f;
                Current = Start;
                End = 0;
                break;
            case (TimerType.ShootTimer):
                Active = true;
                Ready = true;
                Start = MasterManager.GameSettings.ShootRechargeTime;
                Current = Start;
                End = 0;
                break;
            case (TimerType.ImmunityTimer):
                Active = true;
                Ready = true;
                Start = MasterManager.GameSettings.ImmuneTime;
                Current = Start;
                End = 0;
                break;
        }

    }

}
public enum TimerType
{
    StartTimer = 0,
    ShootTimer = 2,
    ImmunityTimer = 3
}