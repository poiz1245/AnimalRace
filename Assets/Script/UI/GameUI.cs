using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameUI : NetworkBehaviour
{
    [SerializeField] TextMeshProUGUI winnerName;
    [SerializeField] GameObject endingPanel;

    public TextMeshProUGUI gameTime;
    public TextMeshProUGUI lapCount;
    public KartLapController controller;
    public KartMove kart;
    

    public void Init(KartLapController kart)
    {
        this.controller = kart;
    }

    private void Update()
    {
        if (controller.isFinish)
        {
            SetEndRaceTime(controller.endRaceTime);
            //SetWinnerName();
            //endingPanel.SetActive(true);
            return;
        }
        else
        {
            SetRaceTimeText(controller.GetRaceTime());
            SetLapCount(controller.processedCheckpoints.Count);
        }
    }
    public void SetWinnerName()
    {
        winnerName.text = ClientInfo.Username;
    }
    public void SetEndRaceTime(float time)
    {
        gameTime.text = $"{(int)(time / 60):00}:{time % 60:00.000}";
    }
    public void SetRaceTimeText(float time)
    {
        gameTime.text = $"{(int)(time / 60):00}:{time % 60:00.000}";
    }

    public void SetLapCount(int count)
    {
        lapCount.text = count.ToString();
    }

    public void EndGameButton()
    {
        NetworkCallBack.NC.OnShutdown(NetworkCallBack.NC.runner, Fusion.ShutdownReason.GameClosed);
    }
}
