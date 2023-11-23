using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KartSelectUI : MonoBehaviour
{

    public Slider speedStatBar;
    public Slider turnStatBar;
    public Slider accelStatBar;

    public float fillSpeed = 2f;

    private void OnEnable()
    {
        SelectKart(ClientInfo.KartId);
    }
    public void SelectKart(int kartIndex)
    {
        ClientInfo.KartId = kartIndex;
        KartDefinition def = ResourceManager.Instance.kartDefinitions[ClientInfo.KartId];
        SportlightGroup.Instance.KartOnOff(kartIndex);

        StartCoroutine(ChangeSpeedSliderValue(def.SpeedStat));
        StartCoroutine(ChangeTurnSliderValue(def.TurnStat));
        StartCoroutine(ChangeAccelSliderValue(def.AccelStat));

        if (RoomPlayer.Local != null)
        {
            RoomPlayer.Local.RPC_SetKartId(kartIndex);
        }
    }


    IEnumerator ChangeSpeedSliderValue(float target)
    {
        float startValue = speedStatBar.value;
        float elapsedTime = 0.0f;

        while(elapsedTime < fillSpeed)
        {
            speedStatBar.value = Mathf.Lerp(startValue, target, elapsedTime/fillSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        speedStatBar.value = target;
    }
    IEnumerator ChangeTurnSliderValue(float target)
    {
        float startValue = turnStatBar.value;
        float elapsedTime = 0.0f;

        while (elapsedTime < fillSpeed)
        {
            turnStatBar.value = Mathf.Lerp(startValue, target, elapsedTime / fillSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        turnStatBar.value = target;
    }
    IEnumerator ChangeAccelSliderValue(float target)
    {
        float startValue = accelStatBar.value;
        float elapsedTime = 0.0f;

        while (elapsedTime < fillSpeed)
        {
            accelStatBar.value = Mathf.Lerp(startValue, target, elapsedTime / fillSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        accelStatBar.value = target;
    }


}
