using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
   public void LoadMission1()
    {
        SceneManager.LoadScene("Mission1");
    }
    public void LoadMission2()
    {
        SceneManager.LoadScene("Mission2");
    }

    public void LoadMission3()
    {
        SceneManager.LoadScene("Mission3");
    }

    
    public void Exit()
    {
        Application.Quit();
    }
}
