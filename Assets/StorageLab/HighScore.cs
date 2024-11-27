using UnityEngine;
using TMPro;
public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Update is called once per frame
    void Update()
    {
        string highScoresText = string.Join(", ", GameController.gCtrl.highScores);
        text.text = "High score: " + highScoresText;
    }
}