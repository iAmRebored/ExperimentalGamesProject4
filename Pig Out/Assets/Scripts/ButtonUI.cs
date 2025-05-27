using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class ButtonUI : MonoBehaviour
{
    public void LoadGameLevel()
    {
        Timer.timeRemaining = 30;
        SceneManager.LoadScene("Drag and Drop test");
    }

    public void QuitGame()
    {
        UnityEngine.Application.Quit();
    }
}
