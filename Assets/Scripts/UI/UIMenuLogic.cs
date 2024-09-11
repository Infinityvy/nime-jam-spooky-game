using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuLogic : MonoBehaviour
{
    [SerializeField]
    private GameObject activeMenu;

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

    public void quit()
    {
        Application.Quit();
    }
}
