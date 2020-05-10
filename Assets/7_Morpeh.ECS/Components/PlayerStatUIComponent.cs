using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using UnityEngine.UI;
using System.Collections.Generic;
using Photon.Realtime;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct PlayerStatUIComponent : IComponent {
    public Player Player;
    public Text NickName;
    public GameObject HeartPrefab;
    public GameObject HeartsContainer;
    public List<GameObject> Hearts;
}