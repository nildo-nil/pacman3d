using DG.Tweening.Core.Easing;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI scoreValueText;
    public GameObject inGameScoreUI;
    public GameObject winPanel;
    public TextMeshProUGUI winScoreValueText;

    private CharacterController controller;
    private float movementX;
    private float movementY;

    private Vector3 playerVelocity;
    private bool isGrounded;
    public float gravityValue = -9.81f;

    public int levelToUnlock;

    public float speed = 0;
    public float rotationSpeed = 10f;
    private int score = 0;

    private int totalCoins = 0;
    private int coinsEaten = 0;

    public GameObject gameOverPanel;

    public GameObject pausePanel;
    private bool isPaused = false;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        scoreValueText.text = score.ToString();

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] superCoins = GameObject.FindGameObjectsWithTag("SuperCoin");
        totalCoins = coins.Length + superCoins.Length;

        Debug.Log("Tổng số coin: " + totalCoins);

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        if (movement.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        controller.Move(movement * speed * Time.deltaTime);
        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Coin"))
        {
            score += 1;
            scoreValueText.text = score.ToString();
            Destroy(collision.gameObject);
            HandleCoinEaten();
        }
        else if (collision.CompareTag("SuperCoin"))
        {
            score += 5;
            scoreValueText.text = score.ToString();
            Destroy(collision.gameObject);
            HandleCoinEaten();
        }
        else if (collision.CompareTag("Ghost"))
        {
            ShowGameOverScreen();
        }
    }
    private void HandleCoinEaten()
    {
        coinsEaten++;
        if (coinsEaten >= totalCoins)
        {
            ShowWinScreen();
        }
    }

    private void ShowWinScreen()
    {
        if (winPanel != null && winScoreValueText != null)
        {
            winPanel.SetActive(true);
            winScoreValueText.text = score.ToString();
            if (inGameScoreUI != null)
            {
                inGameScoreUI.SetActive(false);
            }
            int currentLevelReached = PlayerPrefs.GetInt("LevelReached", 1);
            if (levelToUnlock > currentLevelReached)
            {
                PlayerPrefs.SetInt("LevelReached", levelToUnlock);
                PlayerPrefs.Save(); 
                Debug.Log("Đã mở khóa Level " + levelToUnlock);
            }
            Time.timeScale = 0f;
        }
    }

    public void LoadNextLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void LoadCurrentLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ShowGameOverScreen()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            Time.timeScale = 0f;
            this.enabled = false;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            if (pausePanel != null) pausePanel.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            if (pausePanel != null) pausePanel.SetActive(false);
        }
    }

    public void ResumeGame()
    {
        TogglePause();
    }
}