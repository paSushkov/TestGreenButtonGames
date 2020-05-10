using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

public class ChangeNIckName : MonoBehaviour
{
    public Text NewNickNameInput;
    public Text CurrentNickNameText;

    public void Awake()
    {
        if (PlayerPrefs.HasKey(MasterManager.GameSettings.PlayerNamePrefKey))
        {
            MasterManager.GameSettings.NickName = PlayerPrefs.GetString(MasterManager.GameSettings.PlayerNamePrefKey);
            CurrentNickNameText.text = MasterManager.GameSettings.NickName;
        }
        else
        { 
        CurrentNickNameText.text = MasterManager.GameSettings.NickName;
        }
    }

    public void OnBtnClick_ChangeNickName()
    {
        if (!string.IsNullOrEmpty(NewNickNameInput.text))
        {
            MasterManager.GameSettings.NickName = NewNickNameInput.text;
            PlayerPrefs.SetString(MasterManager.GameSettings.PlayerNamePrefKey, MasterManager.GameSettings.NickName);
            CurrentNickNameText.text = MasterManager.GameSettings.NickName;
        }
    }
}
