using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class MainMenuManager : MonoBehaviour
{
    public GameObject clearPanel; // Bảng thông báo xác nhận (Yes/No)
    public GameObject buttonLayout; // Nhóm các nút chính (Start, Options, Quit...)
    public void StartGame()
    {
        SceneManager.LoadScene("Select Level");
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
        // Xoá tất cả dữ liệu đã lưu
        PlayerPrefs.DeleteAll();
        // Trả lại trạng thái UI ban đầu (Ẩn bảng hỏi, hiện lại menu)
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
