using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f; // Sensitivity for look rotation
    public Transform cameraTransform;
    public MazeGenerator mazeGenerator;
    public TMP_Text wallStatus;
    

    public Camera freeLookCamera; // Reference to the FreeLook camera
    public GameObject ballPrefab;
    public float throwForce = 10f;

    

    private CharacterController characterController;
    private PlayerInputActions playerInputActions;
    private Vector2 moveInput;
    private Vector2 lookInput;
    private float verticalRotation = 0f;
    private AudioController aCtrl;
    public float stepInterval = 0.5f; // Time between each step sound
    private float stepTimer;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();

        // Initialize input actions
        playerInputActions = new PlayerInputActions();

        // Bind methods to input actions
        playerInputActions.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Move.canceled += ctx => moveInput = Vector2.zero;
        playerInputActions.Player.Look.performed += ctx => lookInput = ctx.ReadValue<Vector2>();
        playerInputActions.Player.Look.canceled += ctx => lookInput = Vector2.zero;
        playerInputActions.Player.PassThrough.performed += OnPassThrough;
        playerInputActions.Player.ResetGame.performed += OnResetGame;
        playerInputActions.Player.Throw.performed += ThrowBall;
        if (AudioController.aCtrl != null) {
            aCtrl = AudioController.aCtrl;
        }
        stepTimer = 0f;


    }

    private void OnEnable()
    {
        // Enable the input action map
        playerInputActions.Player.Enable();
    }

    private void OnDisable()
    {
        // Disable the input action map
        playerInputActions.Player.Disable();
    }

    private void Update()
    {
        if (Application.isFocused) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
            HandleMovement();
            // HandleLook();
            RotateCharacterToCamera();
            HandleWalkingSound();
    }
    private void HandleWalkingSound() {
        if (characterController != null && characterController.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                // Debug.Log("Play walking sound");
                AudioController.aCtrl.PlayWalk();
                stepTimer = stepInterval; // Reset the timer
            }
        }
        else
        {
            // Reset timer if the player stops moving
            stepTimer = 0f;
        }
    }

    private void OnPassThrough(InputAction.CallbackContext context)
    {
        // This function is called when the PassThrough action is triggered
        Debug.Log("PassThrough action triggered!");
        // Add your logic for passing through walls or any other functionality here
        bool status = mazeGenerator.ToggleWallLayer();
        wallStatus.text = "Wall: " + status.ToString();

    }

    private void OnResetGame(InputAction.CallbackContext context) {
        Debug.Log("Reset Game action triggered!");

        mazeGenerator.ResetGame();
    }

    void HandleMovement()
    {
        // Convert moveInput to a movement direction in world space
        Vector3 moveDirection = (transform.right * moveInput.x + transform.forward * moveInput.y).normalized;
        
        // Apply movement to the CharacterController
        characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }

    void HandleLook()
    {
        // Horizontal rotation
        float mouseX = lookInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        // Vertical rotation with clamping
        verticalRotation -= lookInput.y * rotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -90f, 90f);
        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void RotateCharacterToCamera()
    {
        // Get the horizontal rotation of the camera
        Vector3 cameraForward = freeLookCamera.transform.forward;
        cameraForward.y = 0; // Zero out the Y component to keep the rotation horizontal only
        if (cameraForward != Vector3.zero) {
            Quaternion targetRotation = Quaternion.LookRotation(cameraForward);

            // rb.MoveRotation(targetRotation);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        // Calculate the target rotation based on camera's forward direction

        // Smoothly rotate the character toward the target rotation

    }
    private void ThrowBall(InputAction.CallbackContext context)
    {
        // Instantiate the ball
        GameObject ball = Instantiate(ballPrefab, transform.position + new Vector3(0,0.5f,0), transform.rotation);

        // Get Rigidbody and apply force in the forward direction
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.velocity = transform.forward * throwForce;
    }
}
