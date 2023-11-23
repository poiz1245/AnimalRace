using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] public int index;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out KartLapController kart))
        {
            kart.ProcessCheckpoint(this);
        }
    }
}
