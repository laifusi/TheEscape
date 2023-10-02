using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public void StartLevel(int index)
    {
        SceneManager.LoadScene(index);
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(GetCurrentLevel() + 1);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(GetCurrentLevel());
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }

    public int GetCurrentLevel()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
