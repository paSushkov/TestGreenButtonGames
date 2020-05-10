using UnityEngine;
using Photon.Pun;

[CreateAssetMenu(menuName = "Manager/GameSettings")]
public class GameSettings : ScriptableObject
{
    [SerializeField]
    private string _gameVersion = "1";
    public string GameVersion { get => _gameVersion; }

    [SerializeField]
    private byte _maxPlayers;
    public byte MaxPlayers { get => _maxPlayers; set => _maxPlayers = value; }

    [SerializeField]
    private string _nickName = "Player";
    public string NickName
    {
        get => _nickName;
        set
        {
            _nickName = value;
            PhotonNetwork.NickName = MasterManager.GameSettings.NickName;
        }
    }

    public const string playerNamePrefKey = "PlayerName";
    public string PlayerNamePrefKey => playerNamePrefKey;

    // Change this variables in Editor on scriptable object!
    public int PlayersLives;
    public int BulletDamage;
    public float BulletSpeed;

    public int FlagsLives;

    public float ShootRechargeTime;
    public float ImmuneTime;

}
    public enum Team : int
    {
        Neutral = 0,
        Blue = 1,
        Red = 2
    }
