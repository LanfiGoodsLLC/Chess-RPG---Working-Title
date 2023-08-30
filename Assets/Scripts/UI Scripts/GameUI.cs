using UnityEngine;
using TMPro;
using System;

public class GameUI : MonoBehaviour
{

    public static GameUI instance { set; get; }

    public Server server;
    public Client client;

    [SerializeField] private Animator menuAnimator;
    [SerializeField] private TMP_InputField addressInput;

    public bool isGameActive;

    public Action<bool> SetLocalGame;

    

    private void Awake()
    {
        instance = this;

        //something I added below
        isGameActive = false;
        //something I added above

        RegisterEvents();

    }

    //buttons

    public void OnLocalGameButton()
    {
        //stuff not in video
        isGameActive = true;
        BattleCamera.instance.onMenu = false;
        BattleCamera.instance.CameraMovedAmount = 1;
        //my stuff above

        menuAnimator.SetTrigger("In Game Menu");
        SetLocalGame?.Invoke(true);
        server.Init(8007);
        client.Init("127.0.0.1", 8007);
    }

    public void OnOnlineGameButton()
    {
        menuAnimator.SetTrigger("Online Menu");
    }

    public void OnOnlineHostButton()
    {
        SetLocalGame?.Invoke(false);
        server.Init(8007);
        client.Init("127.0.0.1", 8007);
        menuAnimator.SetTrigger("Host Menu");
    }

    public void OnOnlineConnectButton()
    {
        SetLocalGame?.Invoke(false);
        client.Init(addressInput.text, 8007);
        Debug.Log("Connect Button"); //$$
    }

    public void OnOnlineBackButton()
    {
        menuAnimator.SetTrigger("Start Menu");
    }

    public void OnHostBackButton()
    {
        server.Shutdown();
        client.Shutdown();
        menuAnimator.SetTrigger("Online Menu");
    }

    public void OnLeaveFromGameMenu()
    {
        BattleCamera.instance.onMenu = true;
        BattleCamera.instance.CameraMovedAmount = 0;
        isGameActive = false;
        menuAnimator.SetTrigger("Start Menu");
    }

    public void QuitMenu()
    {
        menuAnimator.SetTrigger("Quit Menu");
    }

    public void ResumeMatchButton()
    {       
        menuAnimator.SetTrigger("In Game Menu");
        BattleCamera.instance.onMenu = false;
        BattleCamera.instance.CameraMovedAmount = 1;
        isGameActive = true;
    }

    public void OnExitButton()
    {
        Application.Quit();
    }

    private void ShutdownRelay()
    {
        Server.instance.Shutdown();
        Client.instance.Shutdown();
    }

    #region
    private void RegisterEvents()
    {
        NetUtility.C_START_GAME += OnStartGameClient;
    }
    private void UnregisterEvents()
    {
        NetUtility.C_START_GAME += OnStartGameClient;
    }

    private void OnStartGameClient(NetMessage obj)
    {
        menuAnimator.SetTrigger("In Game Menu");
    }
    #endregion


}
