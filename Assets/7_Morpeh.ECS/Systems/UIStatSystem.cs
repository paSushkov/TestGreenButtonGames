using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using Photon.Pun;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(UIStatSystem))]
public sealed class UIStatSystem : UpdateSystem {
    Filter Stat_UIFIlt;
    Filter PlayerFilt;
    public override void OnAwake() {
        Stat_UIFIlt = this.World.Filter.With<PlayerStatUIComponent>();
        PlayerFilt = this.World.Filter.With<PlayerComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var panelEnt in Stat_UIFIlt)
        {
            ref var panel = ref panelEnt.GetComponent<PlayerStatUIComponent>();
            foreach (var playerEnt in PlayerFilt)
            { 
            ref var player = ref playerEnt.GetComponent<PlayerComponent>();
                if (player.Player == panel.Player)
                {
                    if (player.InRoom == false)
                        panel.NickName.color = Color.red;
                    else
                        panel.NickName.color = Color.green;

                    if ( panel.HeartsContainer.transform.childCount!=player.Lives)
                    {
                        if (panel.HeartsContainer.transform.childCount > 0 && panel.HeartsContainer.transform.childCount > player.Lives)
                        {
                            Destroy(panel.HeartsContainer.transform.GetChild(0).gameObject);
                        }
                        else if (panel.HeartsContainer.transform.childCount < player.Lives)
                        {
                            Instantiate(panel.HeartPrefab, panel.HeartsContainer.transform);
                        }
                    }

                }
            }


        }


    }
}