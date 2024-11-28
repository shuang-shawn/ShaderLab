using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PaddleController : MonoBehaviour
{
    public GameObject paddleOne;
    public GameObject paddleTwo;
    public GameObject consoleUI;
    private bool isConsoleActive = false;
    public InputAction inputActionOne;
    public float moveSpeedOne = 20f;
    public Rigidbody rbOne;
    private Vector2 MoveDirectionOne = new Vector2();

    
    public InputAction inputActionTwo;
    public float moveSpeedTwo = 20f;
    public Rigidbody rbTwo;
    private Vector2 MoveDirectionTwo = new Vector2();

    private void OnEnable() {
        inputActionOne.Enable();
        inputActionTwo.Enable();
    }

    private void OnDisable() {
        inputActionOne.Disable();
        inputActionTwo.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        Renderer rendererOne = paddleOne.GetComponent<Renderer>();
        Renderer rendererTwo = paddleTwo.GetComponent<Renderer>();
        if (rendererOne != null){
            rendererOne.material.color = Color.red;
        }
        if (rendererTwo != null){
            rendererTwo.material.color = Color.blue;
        }  
    }

    // Update is called once per frame
    void Update()
    {
        MoveDirectionOne = inputActionOne.ReadValue<Vector2>();
        MoveDirectionTwo = inputActionTwo.ReadValue<Vector2>();
        // isConsoleActive = inputActionOne.ReadValue<bool>();
        // Debug.Log(isConsoleActive);
    }

    private void FixedUpdate() {
        rbOne.velocity = new Vector3(0, 0, moveSpeedOne * MoveDirectionOne.x);
        rbTwo.velocity = new Vector3(0, 0, moveSpeedTwo * MoveDirectionTwo.x);
        // transform.position += new Vector3(0, 0, moveSpeed * MoveDirection.x);
    }
    
    
}
