using Morpeh;
using UnityEngine;
using UnityEngine.UI;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct GameStartUITimerComponent : IComponent {
    public Timer Timer;
    public Text Text;
    public Animator Animator;
}