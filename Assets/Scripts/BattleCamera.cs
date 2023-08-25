using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleCamera : MonoBehaviour
{
    //transformers for objects
    public Transform BattleCameraPlayerT;
    public Transform BattleCameraLeftT;
    public Transform BattleCameraRightT;
    public Transform BattleCameraEnemyT;
    public Transform BattleCameraCharacterFocusT;
    public Transform MainCameraT;

    //Checks button Press Amount
    public int CameraMovedAmount;

    //Floats
    public float smoothTime;
    public float rotateSpeed = 100;
    public float waitTime;

    //Vector3's
    public Vector3 velocity = Vector3.zero;

    private void Start()
    {
        //sets the startiing camera position to face the main player
        MainCameraT.position = BattleCameraPlayerT.position;
        MainCameraT.rotation = BattleCameraPlayerT.rotation;
        CameraMovedAmount = 0;
    }

    private void Update()
    {
        //pressing Q moves the camera and counts how many times the button is pressed to change the angles of the camera
        if (CameraMovedAmount == 0)
        {
            MoveCameraPlayer();
        }

        if (CameraMovedAmount == 1)
        {
            MoveCamercaRight();
        }

        if (CameraMovedAmount == 2)
        {
            MoveCameraEnemy();
        }

        if (CameraMovedAmount == 3)
        {
            MoveCameraLeft();
        }

        if (CameraMovedAmount == 4)
        {
            MoveCameraPlayerFocus();
        }

        if (CameraMovedAmount == 5)
        {
            CameraMovedAmount = 0;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            CameraMovedAmount += 1;
        }
        //This line ends the camera movement code
    }

    //Moves Battle Camera to the Right of the player
    public void MoveCamercaRight()
    {
        MainCameraT.position = Vector3.SmoothDamp(MainCameraT.position, BattleCameraRightT.position, ref velocity, smoothTime);

        MainCameraT.rotation = Quaternion.RotateTowards(MainCameraT.rotation, BattleCameraRightT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }


    //Moves Battle Camera to the right of the enemy - left of player
    public void MoveCameraLeft()
    {
        MainCameraT.position = Vector3.SmoothDamp(MainCameraT.position, BattleCameraLeftT.position, ref velocity, smoothTime);

        MainCameraT.rotation = Quaternion.RotateTowards(MainCameraT.rotation, BattleCameraLeftT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //Moves the Battle Camera to the enemys backside view.
    public void MoveCameraEnemy()
    {
        MainCameraT.position = Vector3.SmoothDamp(MainCameraT.position, BattleCameraEnemyT.position, ref velocity, smoothTime);

        MainCameraT.rotation = Quaternion.RotateTowards(MainCameraT.rotation, BattleCameraEnemyT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //Moves the Battle Camera to the players backside view.
    public void MoveCameraPlayer()
    {
        MainCameraT.position = Vector3.SmoothDamp(MainCameraT.position, BattleCameraPlayerT.position, ref velocity, smoothTime);

        MainCameraT.rotation = Quaternion.RotateTowards(MainCameraT.rotation, BattleCameraPlayerT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //Moves the camera to the character/player focus position.
    public void MoveCameraPlayerFocus()
    {
        MainCameraT.position = Vector3.SmoothDamp(MainCameraT.position, BattleCameraCharacterFocusT.position, ref velocity, smoothTime);

        MainCameraT.rotation = Quaternion.RotateTowards(MainCameraT.rotation, BattleCameraCharacterFocusT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //waits for the camera to finish moving
    IEnumerator WaitForCamera()
    {
        yield return new WaitForSeconds(waitTime);
    }
}

