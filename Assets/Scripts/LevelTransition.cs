using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransition : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ToNextLevel()
    {
        
        var nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextIndex >= SceneManager.sceneCountInBuildSettings)
            Debug.LogWarning("Это последний уровень в списке Build Settings");
        
        SceneManager.LoadScene(nextIndex % SceneManager.sceneCountInBuildSettings);
    }
}