using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SportlightGroup : MonoBehaviour
{
    public static SportlightGroup Instance;

    public GameObject[] kartImage;
    int currentIndex =0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void KartOnOff(int index)
    {
        if (currentIndex == index)
        {
            kartImage[index].SetActive(true);
            currentIndex = index;
        }
        else
        {
            kartImage[currentIndex].SetActive(false);
            kartImage[index].SetActive(true);
            currentIndex = index;
        }
    }


}
