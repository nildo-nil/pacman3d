using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class GameStartCountdown : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public PlayerController playerController;
    public Image dimmerPanel;

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

        countdownText.gameObject.SetActive(false);
        dimmerPanel.gameObject.SetActive(false);

        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }
}
