using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{    
    public GameObject loginmenu;
    public GameObject regmenu;
    public GameObject mainmenu;

    public static SuccessfulLogin login_session = null;
    public static int session_start_minute = 0;

    /*
    [RuntimeInitializeOnLoadMethod]
    static void ResetStatics()
    {
        login_session = null;
        session_start_minute = 0;
    }
    */

    // Start is called before the first frame update
    private void Awake()
    {
        loginmenu.SetActive(false);
        regmenu.SetActive(false);
        mainmenu.SetActive(true);
    }
    public void GotoRegistration()
    {
        mainmenu.SetActive(false);
        loginmenu.SetActive(false);
        regmenu.SetActive(true);
    }

    public void GotoLogin()
    {
        mainmenu.SetActive(false);        
        regmenu.SetActive(false);
        loginmenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BacktoMain()
    {
        loginmenu.SetActive(false);
        regmenu.SetActive(false);
        mainmenu.SetActive(true);
    }
    

}
