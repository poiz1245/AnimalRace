using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItemUI : MonoBehaviour
{
    public Text username;
    public Text kartId;



    public GameObject userKartImage;
    public GameObject userKartImage2;
    public GameObject userKartImage3;
    public GameObject userKartImage4;
    public GameObject[] images = new GameObject[4];
    public Image ready;
    public Image leader;
    public int myKartId;

    RoomPlayer player;

    private void Start()
    {
        userKartImage.GetComponent<RawImage>().texture = ResourceManager.Instance.kartImage[0];
        userKartImage2.GetComponent<RawImage>().texture = ResourceManager.Instance.kartImage[1];
        userKartImage3.GetComponent<RawImage>().texture = ResourceManager.Instance.kartImage[2];
        userKartImage4.GetComponent<RawImage>().texture = ResourceManager.Instance.kartImage[3];
        images[0] = userKartImage;
        images[1] = userKartImage2;
        images[2] = userKartImage3;
        images[3] = userKartImage4;
    }

    public void SetPlayer(RoomPlayer player)
    {
        this.player = player;
    }

    void Update()
    {
        if(player.Object != null && player.Object.IsValid)
        {
            myKartId = player.KartId;

            for (int i = 0; i < 4; i++)
            {
                images[i].SetActive(true);
                if (i != player.KartId)
                    images[i].SetActive(false) ;
            }

            username.text = player.Username.Value;
            ready.gameObject.SetActive(player.IsReady);
        }
    }
}
