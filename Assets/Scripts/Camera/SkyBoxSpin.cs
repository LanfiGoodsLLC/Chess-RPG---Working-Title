using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxSpin : MonoBehaviour
{
    public float skyBoxRotateSpeed;

    // Update is called once per frame
    void Update()
    {
        //spin skybox
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * skyBoxRotateSpeed);
        //To set the speed, just multiply Time.time with whatever amount you want.
    }
}
