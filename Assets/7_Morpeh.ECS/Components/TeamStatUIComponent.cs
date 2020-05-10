using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;
using UnityEngine.UI;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct TeamStatUIComponent : IComponent {
    public Transform Transform;
    public Team Team;
    public Text TeamName;
    public GameObject ListingPrefab;
    public List<GameObject> PlayersListing;
}