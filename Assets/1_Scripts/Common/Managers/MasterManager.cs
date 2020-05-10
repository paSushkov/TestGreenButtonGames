using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Singletons/MasterManager")]
public class MasterManager : ScriptableObjectSingleton<MasterManager>
{
    #region Managers
    // Stores a PlayerNickname & game version
    [SerializeField]
    private GameSettings _gameSettings;
    public static GameSettings GameSettings { get => Instance._gameSettings; }
    // Handles scene load ( & scene transition animated objects if it`s used, otherwise straight load scenes)
    [SerializeField]
    private LevelManager _levelManager;
    public static LevelManager LevelManager { get => Instance._levelManager; }
    #endregion

    // Stores current game status (see variants below)
    private static GameStatusEnum _gameStatus;
    public static GameStatusEnum GameStatus
    {
        get => _gameStatus;
        set
        {
            _gameStatus = value;
            if (Notify != null) Notify(_gameStatus);
            // TODO: Implement call for reconnect system, not sure if it should be here
        }
    }
    // Event to notify any subscribed objetcs in case of changes on GameStatus field.
    public static event StatusHandler Notify;
}

public delegate void StatusHandler(GameStatusEnum status);
public enum GameStatusEnum : uint
{
    TryingToConnect = 0,
    Connected = 10,
    InLobbyIddle = 20,
    InLobbySearch = 30,
    InMatch = 40,
    WaitToStart = 50,
    GameplayProcess = 60,
    ConnectionLost = 100
}