using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    public void PlayGame() => SceneManager.LoadScene("First");

    public void QuitGame() => Application.Quit();
}