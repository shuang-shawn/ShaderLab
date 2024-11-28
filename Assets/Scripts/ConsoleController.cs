using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // New Input System


using TMPro;

public class ConsoleController : MonoBehaviour
{
    public GameObject consoleUI; // Reference to the console UI GameObject
    private string input;
    public TextMeshProUGUI inputFieldText;
    public TextMeshProUGUI outputText; // Reference to the output text component

    public ConsoleIA consoleController;
    public GameObject floor;
    private bool isConsoleOpen = false;
    private InputAction open;
    private InputAction close;
    private InputAction submit;
    public GameObject ball;
    

    private void Awake()
    {
        consoleController = new ConsoleIA();
        
        // Get the PlayerInput component
        
    }

    private void OnEnable()
    {
        open = consoleController.Console.OpenConsole;
        open.Enable();
        open.performed += OpenConsole;

        close = consoleController.Console.CloseConsole;
        close.Enable();
        close.performed += CloseConsole;
       
        // submit = consoleController.Console.SubmitCommand;
        // submit.Enable();
        // submit.performed += SubmitCommand;
        // Subscribe to input events
        // playerInput.actions.Enable();
       
    }

    private void OnDisable()
    {
        open.Disable();
        close.Disable();
        // submit.Disable();
        // playerInput.actions.Disable();
        // Unsubscribe from input events
        
    }
    // Start is called before the first frame update

    void Start()
    {
        Debug.Log(consoleUI);
        // inputField = consoleUI.GetComponent<InputField>();
        // outputText = consoleUI.GetComponent<Text>();
        consoleUI.SetActive(false);
    }


    private void OpenConsole(InputAction.CallbackContext context) {
        Debug.Log("opening console");
        consoleUI.SetActive(true);
        isConsoleOpen = true;
        Time.timeScale = 0f;
    }
    private void CloseConsole(InputAction.CallbackContext context) {
        Debug.Log("cloasing console");
        consoleUI.SetActive(false);
        isConsoleOpen = false;
        Time.timeScale = 1f;
    }

    public void SubmitCommand(string s) {
        
        
        ProcessInput(s);
        // Time.timeScale = 1f;
    }

    

    private void ProcessInput(string s)
    {
        if (!isConsoleOpen) return;

        
        if (s.StartsWith("background "))
        {
            string colorName = s.Substring("background ".Length).Trim();
            ChangeBackgroundColor(colorName);
        } else if (s.ToLower().Trim() == "reset") {
            PongGameController.EndGame();
        }
        else
        {
            outputText.text = "Unknown command: " + s;
        }


    }

        private void ChangeBackgroundColor(string colorName)
    {
        Color color;
        switch (colorName.ToLower())
        {
            case "blue":
                color = Color.blue;
                break;
            case "red":
                color = Color.red;
                break;
            case "green":
                color = Color.green;
                break;
            case "yellow":
                color = Color.yellow;
                break;
            default:
                outputText.text = "Invalid color: " + colorName;
                return;
        }
        Renderer floorRenderer = floor.GetComponent<Renderer>();
        if (floorRenderer != null){
            floorRenderer.material.color = color;
        }
        outputText.text = "Background color changed to " + colorName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
