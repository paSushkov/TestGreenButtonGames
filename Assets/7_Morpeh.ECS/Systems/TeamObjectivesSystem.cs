using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;
using System.Collections.Generic;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(TeamObjectivesSystem))]
public sealed class TeamObjectivesSystem : UpdateSystem
{
    Filter teams;
    Filter objectives;


    Filter playersFilt;
    public override void OnAwake()
    {
        List<Team> TeamsInGame;

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
            teamComp.Objectives = new List<GameObject>();
            foreach (var objectiveEnt in objectives)
            {
                ref var objectiveHealth = ref objectiveEnt.GetComponent<HealthComponent>();
                objectiveHealth.Health = MasterManager.GameSettings.FlagsLives;
                if (objectiveHealth.Team == teamComp.Team)
                {
                    if (objectiveHealth.Transform.gameObject != null)
                        teamComp.Objectives.Add(objectiveHealth.Transform.gameObject);
                    teamComp.FlagsLives += objectiveHealth.Health;
                }
            }
        }

        teams = this.World.Filter.With<TeamComponent>();
    }

    public override void OnUpdate(float deltaTime)
    {
        foreach (var teamEnt in teams)
        {
            ref var teamComp = ref teamEnt.GetComponent<TeamComponent>();
            teamComp.FlagsLives = 0;

            for (int i = 0; i < teamComp.Objectives.Count; i++)
            {
                if (teamComp.Objectives[i] == null)
                {
                    teamComp.Objectives.RemoveAt(i);
                    continue;
                }
                var objectiveHealthProvider = teamComp.Objectives[i].GetComponent<HealthComponentProvider>();
                ref var objectiveHealthComp = ref objectiveHealthProvider.GetData();
                teamComp.FlagsLives += objectiveHealthComp.Health;
            }

            if (teamComp.FlagsLives == 0)
            {
                MasterManager.LevelManager.LeaveRoomGoToLobby();
            }



        }
    }
}