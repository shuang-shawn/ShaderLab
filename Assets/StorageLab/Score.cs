using UnityEngine;
using TMPro;
public class Score : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + GameController.gCtrl.GetCurrentScore();
    }
}