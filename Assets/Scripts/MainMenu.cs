using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private Cognitive3D.DynamicObject thisDynamics;
    public UIButtonsAndMenus[] UIButtons = new UIButtonsAndMenus[2];
    private void Awake()
    {
        thisDynamics = GetComponent<Cognitive3D.DynamicObject>();
        loginmenu.SetActive(false);
        regmenu.SetActive(false);
        mainmenu.SetActive(true);

        for (int i = 0; i < UIButtons.Length; i++)
        {
            int temp = i;
            UIButtons[i].button.onClick.AddListener(() => ShowMenu(UIButtons[temp].button));
        }

    }
    

    //Alternative method, have the buttons and meanu objects in arrays
    /*public Button[] buttons; // Assign your buttons in the inspector.

    private void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => HandleButtonClick(btn));
        }
    }

    private void HandleButtonClick(Button clickedButton)
    {
        foreach (Button btn in buttons)
        {
            if (btn != clickedButton)
            {
                btn.gameObject.SetActive(false);
            }
        }
    */
    public void GotoRegistration()
    {
        /*
        mainmenu.SetActive(false);
        loginmenu.SetActive(false);
        regmenu.SetActive(true);
        */
    }

    public void GotoLogin()
    {    /*   
        mainmenu.SetActive(false);        
        regmenu.SetActive(false);
        loginmenu.SetActive(true);
        */
    }

    public void ExitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        //EditorApplication.isPlaying = false;
        Debug.Log  ("Should have quit");
#endif
    }

    public void BacktoMain()
    {        
        loginmenu.SetActive(false);
        regmenu.SetActive(false);
        mainmenu.SetActive(true);        
    }

   

    private void OnEnable()
    {
        
    }

    private void ShowMenu(Button pressed_button)
    {
        for( int i = 0; i < UIButtons.Length; i++)
        {
            int temp = i;
            if(pressed_button != UIButtons[temp].button)
            {
                UIButtons[temp].menu.SetActive(false);
            }
            else
            {
                UIButtons[temp].menu.SetActive(true);
            }
        }
        mainmenu.SetActive(false);
    }
    [Serializable]
    public struct UIButtonsAndMenus
    {
        public Button button;
        public GameObject menu;
    }


}

