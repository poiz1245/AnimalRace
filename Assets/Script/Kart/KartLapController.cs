using ExitGames.Client.Photon;
using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class KartLapController : NetworkBehaviour
{
    [Networked] public int startRaceTick { get; set; }
    [Networked] public float endRaceTime { get; set; }

    [SerializeField] int lapIndex = 0;

    public GameUI hud;
   

    [Networked] public NetworkBool isFinish { get; set; }

    public HashSet<int> processedCheckpoints = new HashSet<int>();

    public string myName;


    public override void Spawned()
    {
        if (Object.HasInputAuthority)
        {
            myName = ClientInfo.Username;
            hud = Instantiate(ResourceManager.Instance.hudPrefab);
            hud.Init(this);
        }
    }

    public override void FixedUpdateNetwork()
    {
        OnRaceStart();

        if (transform.position.y < -3)
        {
            transform.position = new Vector3(60, 0, 30);
            transform.Rotate(0, 90, 0);
            processedCheckpoints.Clear();
        }
    }

    public void OnRaceStart()
    {
        startRaceTick = Runner.Simulation.Tick;
    }

    public float GetRaceTime()
    {
        if (!Runner.IsRunning || startRaceTick == 0)
        {
            return 0;
        }


        return startRaceTick * Runner.DeltaTime;
    }

    public void ProcessCheckpoint(Checkpoint checkpoint)
    {
        if (!processedCheckpoints.Contains(checkpoint.index))
        {
            processedCheckpoints.Add(checkpoint.index);
        }
    }

    public void ProcessFinishLine(FinishLine finishLine)
    {
        if(processedCheckpoints.Count >= 5)
        {
            lapIndex++;
        }

        if (lapIndex >= 1)
        {
            isFinish = true;
            CheckPlayerStatus(true);
            SetTotalRaceTime(startRaceTick);
        }
    }

    public void CheckPlayerStatus(NetworkBool isFinish)
    {
        this.isFinish = isFinish;
        GameManager.Instance.isFinish = true;
    }

    public float GetTotalRaceTime()
    {
        return endRaceTime;
    }

    public void SetTotalRaceTime(int startRaceTick)
    {
        endRaceTime = startRaceTick * Runner.DeltaTime;
    }

}
