using UnityEngine;
using TMPro;

public class GameUI : MonoBehaviour
{

    public static GameUI instance { set; get; }

    [SerializeField] private Animator menuAnimator;

    private void Awake()
    {
        instance = this;

    }

    //buttons

    public void OnLocalGameButton()
    {
        menuAnimator.SetTrigger("In Game Menu");
    }


    public void OnOnlineGameButton()
    {
        menuAnimator.SetTrigger("Online Menu");
    }

    public void OnOnlineHostButton()
    {
        menuAnimator.SetTrigger("Host Menu");
    }

    public void OnOnlineConnectButton()
    {
        Debug.Log("Connect Button"); //$$
    }

    public void OnOnlineBackButton()
    {
        menuAnimator.SetTrigger("Start Menu");
    }

    public void OnHostBackButton()
    {
        menuAnimator.SetTrigger("Online Menu");
    }

}
