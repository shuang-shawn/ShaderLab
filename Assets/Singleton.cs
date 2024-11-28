using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class Singleton : MonoBehaviour
{
    public TextMeshProUGUI endgame;
    

    // Start is called before the first frame update
    void Start()
    {
        int left = PlayerPrefs.GetInt("left");
        int right = PlayerPrefs.GetInt("right");
        if (SceneManager.GetActiveScene().name == "StartMenu"){
        endgame.text = "Welcome";

        } else {
            endgame.text = left + " : " + right + "\nGame End";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void vsPlayer(){
        PlayerPrefs.SetInt("mode", 0);
        PlayerPrefs.Save(); // Make sure to save the changes

        SceneManager.LoadScene("Game");
    }

    public void vsAi() {
        PlayerPrefs.SetInt("mode", 1);
        PlayerPrefs.Save(); // Make sure to save the changes

        SceneManager.LoadScene("Game");
    }

    public void QuitGame() {
        Application.Quit();
    }
}
