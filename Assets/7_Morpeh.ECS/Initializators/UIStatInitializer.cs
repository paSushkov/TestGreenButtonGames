using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(UIStatInitializer))]
public sealed class UIStatInitializer : Initializer {
    Filter players;
    Filter teamsUI;
    Filter playersUI;

    public override void OnAwake() {
        players = this.World.Filter.With<PlayerComponent>();
        teamsUI = this.World.Filter.With<TeamStatUIComponent>();
        foreach (var UI_TeamPanelEnt in teamsUI)
        {
            ref var teamPanel = ref UI_TeamPanelEnt.GetComponent<TeamStatUIComponent>();
            teamPanel.TeamName.text = Enum.GetName(typeof(Team), teamPanel.Team) + " team";
            foreach (var playerEnt in players)
            { 
            ref var player = ref playerEnt.GetComponent<PlayerComponent>();
                if (player.Team == teamPanel.Team)
                {
                   GameObject playerListingObj = Instantiate(teamPanel.ListingPrefab, teamPanel.Transform);
                    var provider = playerListingObj.GetComponent<PlayerStatUIComponentProvider>();
                    ref var playerPanelComp = ref provider.GetData();
                    playerPanelComp.Player = player.Player;
                    playerPanelComp.NickName.text = playerPanelComp.Player.NickName;
                    for (int i = 0; i < player.Lives; i++)
                    {
                        Instantiate(playerPanelComp.HeartPrefab, playerPanelComp.HeartsContainer.transform);
                    }

                }
            }
        }
    }

    public override void Dispose() {
    }
}