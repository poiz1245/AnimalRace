using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvokeOnEnabel : MonoBehaviour
{
    public UnityEvent onEnable;

    private void OnEnable()
    {
        onEnable.Invoke();
    }
}
