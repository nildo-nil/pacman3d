using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameStartCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public PlayerController playerController;
    public Image dimmerPanel;
    public Ghost[] allGhosts;


    void Awake()
    {
        //TẮT GHOST NGAY TỪ AWAKE() - TRƯỚC KHI Start() CHẠY!
        DisableAllGhosts();
    }

    void Start()
    {
        StartCoroutine(CountdownSequence());
    }

    IEnumerator CountdownSequence()
    {
        if (playerController != null)
        {
            playerController.enabled = false;
        }


        dimmerPanel.gameObject.SetActive(true);
        countdownText.gameObject.SetActive(true);

        countdownText.text = "READY!";
        yield return new WaitForSeconds(1f);

        countdownText.text = "111";
        yield return new WaitForSeconds(1f);

        countdownText.text = "11";
        yield return new WaitForSeconds(1f);

        countdownText.text = "1";
        yield return new WaitForSeconds(1f);

        countdownText.text = "START!";
        yield return new WaitForSeconds(0.5f);

        EnableAllGhosts();

        countdownText.gameObject.SetActive(false);
        dimmerPanel.gameObject.SetActive(false);
    }

    void DisableAllGhosts()
    {
        foreach (var ghost in allGhosts)
        {
            if (ghost != null)
            {
                ghost.enabled = false; // Tắt script → Dừng AI!
                Debug.Log($"ghost {ghost.name} DISABLED");
            }
        }
    }

    void EnableAllGhosts()
    {
        if (playerController != null)
        {
            playerController.enabled = true;
        }

        foreach (var ghost in allGhosts)
        {
            if (ghost != null)
            {
                ghost.enabled = true; // Bật script → Bắt đầu AI!
                Debug.Log($"Ghost {ghost.name} ({ghost.ghostType}) START CHASING!");
            }
        }
    }
}
