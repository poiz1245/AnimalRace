using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out KartLapController kart))
        {
            kart.ProcessFinishLine(this);
            GameManager.Instance.userName = kart.myName;

        }
    }

}
