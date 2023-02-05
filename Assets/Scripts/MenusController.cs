using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    public void OnPlay()
    {
        SceneManager.LoadScene(1);
    }
    public void OnQuit()
    {
        Application.Quit();
    }
}
