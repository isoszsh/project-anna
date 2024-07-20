using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraActiveForDialog : MonoBehaviour
{
    //2 game object al camera olarak
    public GameObject playerCam;
    public GameObject DialogCam;

    public void isSpeakerCamActive(bool active)
    {
        //camera1 aktif ise
        if (active)
        {
            playerCam.SetActive(false);
            DialogCam.SetActive(true);
        }
        //camera2 aktif ise
        else
        {
            playerCam.SetActive(true);
            DialogCam.SetActive(false);
        }
    }
}
