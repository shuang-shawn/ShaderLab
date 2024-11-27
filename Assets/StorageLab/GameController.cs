using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections.Generic;
public class GameController : MonoBehaviour
{
    public List<int> highScores = new List<int>(); //use persistentDataPath to save high score in custom GameData object
    const string fileName = "/highscore.dat";
    public static GameController gCtrl;
    public void Awake()
    {
        if (gCtrl == null)
        {
            DontDestroyOnLoad(gameObject);
            gCtrl = this;
            LoadScore();
        }
    }

    //Small custom class to hold high score that will be saved and serialized using persistentDataPath
    //Serialization: automatic process of transforming data structures or object states into a format that Unity can store and reconstruct later
    [Serializable]
    class GameData
    {
        public List<int> savedHighScores;
    };

    //use persistentDataPath to load high score
    public void LoadScore()
    {
        if (File.Exists(Application.persistentDataPath + fileName))
        {
            BinaryFormatter bf = new BinaryFormatter(); //class to help serialize and deserialize data
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.Open, FileAccess.Read); //open file path for reading
            
            GameData data = (GameData)bf.Deserialize(fs); //deserialize data at filepath using Binary formatter, cast into GameData object
            fs.Close();
            gCtrl.highScores = data.savedHighScores; //set current high score to saved high score
        }
    }

    //use persistentDataPath to save high score
    public void SaveScore(int score)
    {

       
            // Add the new score
            highScores.Add(score);
            
            // Sort the list in ascending order
            highScores.Sort();
            
            // If the list exceeds 5 elements, remove the lowest (first element)
            if (highScores.Count > 5)
            {
                highScores.Sort((a, b) => b.CompareTo(a));
            
            // Take only the first 5 elements (top 5 highest scores)
                highScores = highScores.GetRange(0, 5);
            }

      
            gCtrl.highScores = highScores;
            BinaryFormatter bf = new BinaryFormatter(); //class to help serialize and deserialize data
            FileStream fs = File.Open(Application.persistentDataPath + fileName, FileMode.OpenOrCreate); //open file path for writing
                                    
            GameData data = new GameData(); //create new GameData object set high score to be saved
            data.savedHighScores = highScores;
            bf.Serialize(fs, data); //use binary formatter to serialize data at filepath
            fs.Close();
        
    
        //if we have a new high score
       
            
            
        
    }
    //Use PlayerPrefs to store current score
    public int GetCurrentScore()
    {
        return PlayerPrefs.GetInt("CurrentScore");
    }
    //Use PlayerPrefs to store current score
    public void SetCurrentScore(int num)
    {
        PlayerPrefs.SetInt("CurrentScore", num);
    }
    public void AddScorePressed()
    {
        int score = GetCurrentScore();
        score++;
        SetCurrentScore(score); //saves current score to playerPrefs
        SaveScore(score); //saves current score if it's a new high score
    }
    public void MinusScorePressed()
    {
        int score = GetCurrentScore();
        score--;
        SetCurrentScore(score); //saves current score to playerPrefs
    }
}