using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusPanelScript : MonoBehaviour
{
    private GameObject currentStatusElementUI;
    [SerializeField] private GameObject[] UIStatusElements;
    private GameStatusEnum currentStatus;
    private void Start()
    {
        UpdateStaus(MasterManager.GameStatus);
        MasterManager.Notify += UpdateStaus;
    }
    private void OnDestroy()
    {
        MasterManager.Notify -= UpdateStaus;
    }
    public void UpdateStaus(GameStatusEnum newStatus)
    {
        if (currentStatus == newStatus)
            return;
        currentStatus = newStatus;
        switch (currentStatus)
        {
            case (GameStatusEnum.InLobbySearch):
                Destroy(currentStatusElementUI);
                currentStatusElementUI = Instantiate(UIStatusElements[0], this.transform);
                break;
            case (GameStatusEnum.InLobbyIddle):
                Destroy(currentStatusElementUI);
                // implement visual element (tank roaming around?) for iddle
                break;
        }
    }
}
