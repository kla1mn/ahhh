using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static void LoadScene(int sceneId)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneId);
    }

    public static void Quit()
    {
        Application.Quit();
    }

    internal static void LoadScene(object nextId)
    {
        throw new NotImplementedException();
    }
}
