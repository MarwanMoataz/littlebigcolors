using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject gameUI;

    public void StartGame()
    {
        SceneManager.LoadScene("Levels");
    }

    public void RetryGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reload the scene
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
