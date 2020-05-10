using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;
using Morpeh;

[CreateAssetMenu(menuName = "Manager/LevelManager")]
public class LevelManager : ScriptableObject
{
    [SerializeField]
    private GameObject transitionerObj;
    private LevelTransitioner transitioner;
    public void LoadScene(int SceneID)
    {
        if (transitionerObj != null)
        {
            if (transitionerObj.GetComponent<LevelTransitioner>() != null)
            {
                transitioner = transitionerObj.GetComponent<LevelTransitioner>();
                transitioner.LoadScene(SceneID);
            }
        }
        else
        {
            SceneManager.LoadScene(SceneID);
        }
    }
    public void LoadScene(string SceneName)
    {
        if (transitionerObj == null)
        {
            transitionerObj = FindObjectOfType<LevelTransitioner>().gameObject;

            if (transitionerObj == null)
            {
                SceneManager.LoadScene(SceneName);
            }
            else
            {
                transitioner = transitionerObj.GetComponent<LevelTransitioner>();
                transitioner.LoadScene(SceneName);
            }
        }
        else
        {
            if (transitionerObj.GetComponent<LevelTransitioner>() != null)
            {
                transitioner = transitionerObj.GetComponent<LevelTransitioner>();
                transitioner.LoadScene(SceneName);
            }
        }
    }
    public void LeaveRoomGoToLobby()
    {
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom();

        /////////////////////////////////////////////////////
        // This stuff should be done somehow in another way...
        World world = World.Default;
        Filter filter = world.Filter.With<PlayerComponent>();
        foreach (var ent in filter)
        {
            world.RemoveEntity(ent);
        }
        Filter filter2 = world.Filter.With<TeamComponent>();
        foreach (var ent in filter2)
        {
            world.RemoveEntity(ent);
        }
        ///////////////////////////////////////////////////////


        MasterManager.GameStatus = GameStatusEnum.InLobbyIddle;
        LoadScene("Lobby");
    }
    public void Exit()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Disconnect();
        }
        Application.Quit();
    }
    public void LoadRandomGameLevel()
    {
        int levelID = UnityEngine.Random.Range(0, Enum.GetNames(typeof(GameLevels)).Length - 1);
        string levelName = Enum.GetName(typeof(GameLevels), levelID);
        if (transitionerObj != null)
        {
            if (transitionerObj.GetComponent<LevelTransitioner>() != null)
                transitioner = transitionerObj.GetComponent<LevelTransitioner>();
            transitioner.LoadScene(levelName, true);
        }
        else
        {
            PhotonNetwork.LoadLevel(levelName);
        }
    }
}
public enum GameLevels : uint
{
    GameLevel0 = 0
}