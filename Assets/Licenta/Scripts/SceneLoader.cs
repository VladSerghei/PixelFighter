using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    } 

    public void LoadGame()
    {
        SceneManager.LoadScene("Level");
    }
   

    public void LoadGameOver()
    {
        SceneManager.LoadScene("Game Over");
    }

    public void LoadTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
