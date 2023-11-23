using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisabledUI : MonoBehaviour
{
    private void Awake()
    {

        foreach (var behaviour in GetComponentsInChildren<IDisabledUI>(true)) behaviour.Setup();
    }

    private void OnDestroy()
    {
        foreach (var behaviour in GetComponentsInChildren<IDisabledUI>(true)) behaviour.OnDestruction();

    }
}
