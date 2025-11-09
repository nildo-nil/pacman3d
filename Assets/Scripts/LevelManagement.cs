using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LevelManagement : MonoBehaviour
{
    [Header("Level Buttons")]
    public Button level1Button;
    public Button level2Button;
    public Button level3Button;

    [Header("Lock Icons")]
    public GameObject level2LockIcon; 
    public GameObject level3LockIcon; 

    void Start()
    {
        int levelReached = PlayerPrefs.GetInt("LevelReached", 1);

        if (level1Button != null)
        {
            level1Button.interactable = true;
        }

        if (level2Button != null && level2LockIcon != null)
        {
            if (levelReached >= 2)
            {
                level2Button.interactable = true; 
                level2LockIcon.SetActive(false);  
            }
            else
            {
                level2Button.interactable = false; 
                level2LockIcon.SetActive(true);   
            }
        }

        if (level3Button != null && level3LockIcon != null)
        {
            if (levelReached >= 3)
            {
                level3Button.interactable = true; 
                level3LockIcon.SetActive(false);  
            }
            else
            {
                level3Button.interactable = false; 
                level3LockIcon.SetActive(true);   
            }
        }
    }
    public void Level1()
    {
        SceneManager.LoadScene("Level 1");
    }
    public void Level2()
    {
        SceneManager.LoadScene("Level 2");
    }
    public void Level3()
    {
        SceneManager.LoadScene("Level 3");
    }

    public void Cancel()
    {
        SceneManager.LoadScene("Main Menu");
    }

}
