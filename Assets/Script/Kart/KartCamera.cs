using Cinemachine;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartCamera : MonoBehaviour//, ICameraControll
{
    //[SerializeField] CinemachineVirtualCamera VC;
    private void Start()
    {
        transform.parent = null;
        //VC.Follow = 
    }

 /*   public void SetFollow(Transform playerPos)
    {

    }*/
    /*    public bool ControlCamera(CinemachineVirtualCamera VC)
        {
            if(this.Equals(null)) return false;

            VC.Follow = gameObject.transform;

            return true;
        }

        public override void Render()
        {
            base.Render();

            if(Object.HasInputAuthority && !GameManager.IsCameraControll)
            {
                GameManager.GetCameraControl(this);
            }
        }*/
}
