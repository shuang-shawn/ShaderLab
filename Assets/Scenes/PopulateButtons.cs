using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PopulateButtons : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform content;
    public int numButtons = 10;
    List<string> buttonNames = new List<string>();
    // Start is called before the first frame update
    void Start()
    {
        buttonNames.Add("Fire");
        buttonNames.Add("Smoke");
        buttonNames.Add("Snow");
        buttonNames.Add("Raandom");

        for (int i = 0; i < buttonNames.Count; i++)
        {
            // Instantiate(buttonPrefab, content);
            MyButton b = Instantiate(buttonPrefab, content).GetComponent<MyButton>();
            b.SetText(buttonNames[i]);
            b.callback = ButtonClicked;
        }
    }

    public void ButtonClicked(string buttonText)
    {
        Debug.Log("Clicked " + buttonText);
    }
}
