using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleCamera : MonoBehaviour
{
    public static BattleCamera instance { set; get; }

    //transformers for objects

    public Transform battleCameraWhiteTeamT;
    public Transform battleCameraLeftT;
    public Transform battleCameraRightT;
    public Transform battleCameraBlackTeamT;
    public Transform battleCameraTopT;
    public Transform mainCameraT;
    public Transform startCameraT;

    //Checks button Press Amount
    public int CameraMovedAmount;

    //Floats
    public float smoothTime;
    public float rotateSpeed = 100;
    public float waitTime;

    //Vector3's
    public Vector3 velocity = Vector3.zero;

    //bools
    public bool onMenu;

    private void Awake()
    {
        instance = this;

        //sets the startiing camera position to face the main player
        MoveCameraMenu();
        CameraMovedAmount = 0;
        onMenu = true;
    }

    private void Update()
    {
        //sets camera to not look at the board
        if (CameraMovedAmount == 0)
        {
            MoveCameraMenu();
        }
        //if you press escape while in game you will be brought to the quit menu
        if (Input.GetKeyDown(KeyCode.Escape) && GameUI.instance.isGameActive == true)
        {
            GameUI.instance.QuitMenu();
            CameraMovedAmount = 0;
            onMenu = true;          
        }
        //if you press Q while in game you can change the angle of the camera
        if (GameUI.instance.isGameActive == true && onMenu == false)
        {



            if (Input.GetKeyDown(KeyCode.Q))
            {
                CameraMovedAmount += 1;
            }

            //pressing Q moves the camera and counts how many times the button is pressed to change the angles of the camera
            if (CameraMovedAmount == 1)
            {
                MoveCameraWhiteTeam();
            }

            if (CameraMovedAmount == 2)
            {
                MoveCameraBlackTeam();
            }

            if (CameraMovedAmount == 3)
            {
                MoveCamercaRight();
            }

            if (CameraMovedAmount == 4)
            {
                MoveCameraTop();
            }

            if (CameraMovedAmount == 5)
            {
                MoveCameraLeft();
            }

            if (CameraMovedAmount == 6)
            {
                CameraMovedAmount = 1;
            }
        }


        //This line ends the camera movement code
    }

    //Moves Battle Camera to the Right of the player
    public void MoveCamercaRight()
    {
        mainCameraT.position = Vector3.SmoothDamp(mainCameraT.position, battleCameraRightT.position, ref velocity, smoothTime);

        mainCameraT.rotation = Quaternion.RotateTowards(mainCameraT.rotation, battleCameraRightT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //Moves Battle Camera to the right of the enemy - left of player
    public void MoveCameraLeft()
    {
        mainCameraT.position = Vector3.SmoothDamp(mainCameraT.position, battleCameraLeftT.position, ref velocity, smoothTime);

        mainCameraT.rotation = Quaternion.RotateTowards(mainCameraT.rotation, battleCameraLeftT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //Moves the Battle Camera to the enemys backside view.
    public void MoveCameraBlackTeam()
    {
        mainCameraT.position = Vector3.SmoothDamp(mainCameraT.position, battleCameraBlackTeamT.position, ref velocity, smoothTime);

        mainCameraT.rotation = Quaternion.RotateTowards(mainCameraT.rotation, battleCameraBlackTeamT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //Moves the Battle Camera to the players backside view.
    public void MoveCameraWhiteTeam()
    {
        mainCameraT.position = Vector3.SmoothDamp(mainCameraT.position, battleCameraWhiteTeamT.position, ref velocity, smoothTime);

        mainCameraT.rotation = Quaternion.RotateTowards(mainCameraT.rotation, battleCameraWhiteTeamT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //Moves the camera to the character/player focus position.
    public void MoveCameraTop()
    {
        mainCameraT.position = Vector3.SmoothDamp(mainCameraT.position, battleCameraTopT.position, ref velocity, smoothTime);

        mainCameraT.rotation = Quaternion.RotateTowards(mainCameraT.rotation, battleCameraTopT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //moves the camera to the menu screen
    public void MoveCameraMenu()
    {
        mainCameraT.position = Vector3.SmoothDamp(mainCameraT.position, startCameraT.position, ref velocity, smoothTime);

        mainCameraT.rotation = Quaternion.RotateTowards(mainCameraT.rotation, startCameraT.rotation, rotateSpeed * Time.deltaTime);
        StartCoroutine(WaitForCamera());
    }

    //waits for the camera to finish moving
    IEnumerator WaitForCamera()
    {
        yield return new WaitForSeconds(waitTime);
    }
}

