using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public GameObject clearPanel;
    public GameObject buttonLayout;
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
        if (clearPanel != null)
        {
            clearPanel.SetActive(true);
            buttonLayout.SetActive(false);
        }
    }

    public void HandleYesClear()
    {
        Debug.Log("Accepted clearing data");
        PlayerPrefs.DeleteAll();
        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
            buttonLayout.SetActive(true);
        }
        SceneManager.LoadScene("Main Menu");
    }
    public void HandleNoClear()
    {
        Debug.Log("Cancelled clearing data");
        if (clearPanel != null)
        {
            clearPanel.SetActive(false);
            buttonLayout.SetActive(true);
        }
    }
    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
