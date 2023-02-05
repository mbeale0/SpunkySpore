using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    public void OnPlay()
    {
        if(GameObject.FindGameObjectWithTag("Player") != null) AkSoundEngine.StopAll(GameObject.FindGameObjectWithTag("Player"));
        SceneManager.LoadScene(1);
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
