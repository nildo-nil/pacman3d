using UnityEngine;
using UnityEngine.SceneManagement;
public class LoadSceneOnClick : MonoBehaviour
{
    [Tooltip("Điền tên của scene Main Menu vào đây (ví dụ: MainMenu)")]
    public string sceneToLoad;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
