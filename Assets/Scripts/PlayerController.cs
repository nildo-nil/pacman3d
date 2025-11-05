using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SocialPlatforms.Impl;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    private CharacterController controller;
    private float movementX;
    private float movementY;

    private Vector3 playerVelocity;
    private bool isGrounded;
    public float gravityValue = -9.81f;

    public float speed = 0;   
    public float rotationSpeed = 10f;
    private int score = 0;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        scoreText.text = "Score: " + score;
    }

    void Update()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
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
            scoreText.text = "Score: " + score;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("SuperCoin"))
        {
            score += 5;
            scoreText.text = "Score: " + score;
            Destroy(collision.gameObject);
        }
    }
}