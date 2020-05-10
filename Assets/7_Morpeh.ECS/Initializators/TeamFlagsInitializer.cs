using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Initializers/" + nameof(TeamFlagsInitializer))]
public sealed class TeamFlagsInitializer : Initializer {
    Filter playersFilt;
    List<Team> TeamsInGame;
    Filter objectives;
    public override void OnAwake() {
        playersFilt = this.World.Filter.With<PlayerComponent>();
        objectives = this.World.Filter.With<ObjectiveMarker>();

        TeamsInGame = new List<Team>();
        // Creating list of teams that used in this match
        foreach (var playerEnt in playersFilt)
        {
            ref var playerComp = ref playerEnt.GetComponent<PlayerComponent>();
            if (TeamsInGame.Contains(playerComp.Team))
            { continue; }
            else
            {
                TeamsInGame.Add(playerComp.Team);
            }
        }
        foreach (Team team in TeamsInGame)
        {
            var newTeamEnt = this.World.CreateEntity();
            newTeamEnt.AddComponent<TeamComponent>();
            ref var teamComp = ref newTeamEnt.GetComponent<TeamComponent>();
            teamComp.Team = team;
            foreach (var objectiveEnt in objectives)
            {
                ref var objectiveHealth = ref objectiveEnt.GetComponent<HealthComponent>();
                objectiveHealth.Health = MasterManager.GameSettings.FlagsLives;
                if (objectiveHealth.Team == team)
                {
                    teamComp.Objectives.Add(objectiveHealth.Transform.gameObject);
                    //teamComp.Objectives.Add(objectiveHealth.Transform);
                    teamComp.FlagsLives += objectiveHealth.Health;
                }
            }
        }
    }

    public override void Dispose() {
    }
}