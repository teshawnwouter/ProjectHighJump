using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonManager : MonoBehaviour
{
    public GameObject credits;
    public GameObject main;
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void Credits()
    {
        credits.SetActive(true);
        main.SetActive(false);
    }

    public void Back()
    {
        main.SetActive(true);
        credits.SetActive(false);
    }
}
