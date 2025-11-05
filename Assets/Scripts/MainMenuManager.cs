using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Select Level");
    }
    public void OpenOptions()
    {
        Debug.Log("Opening options...");
    }
    public void ClearData()
    {
        Debug.Log("Clearing data...");
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
