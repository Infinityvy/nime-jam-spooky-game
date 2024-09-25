using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuLogic : MonoBehaviour
{
    public static UIMenuLogic instance;

    [SerializeField]
    private GameObject activeMenu;
    private GameObject defaultMenu;

    private void Start()
    {
        instance = this;
        defaultMenu = activeMenu;
    }

    public void gotoMenu(GameObject menu)
    {
        activeMenu.SetActive(false);
        menu.SetActive(true);
        activeMenu = menu;
    }

    public void play()
    {
        SceneManager.LoadScene("Main");
    }

    public void mainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void quit()
    {
        Application.Quit();
    }

    public void toggleGamePaused(bool state)
    {
        Session.instance.togglePaused(state);
    }

    public void toggleMenu(bool state)
    {
        if(activeMenu != defaultMenu) gotoMenu(defaultMenu); 

        activeMenu.SetActive(state);
    }
}
