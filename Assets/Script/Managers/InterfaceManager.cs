using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceManager : MonoBehaviour
{
    [SerializeField] ProfileSetupUI profileSetup;

    // Start is called before the first frame update
    void Start()
    {
        profileSetup.AssertProfileSetUP();
    }

}
