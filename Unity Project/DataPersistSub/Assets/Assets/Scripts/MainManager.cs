using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using Random = UnityEngine.Random;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    public Text currentPlayer;
    public Text highscorePlayer;
    
    private bool m_Started = false;
    public int m_Points;
    
    private bool m_GameOver = false;

    private static int HighScore;
    private static string PlayersHighScore;

    private void Awake()
    {
        LoadHighScore();
    }

    // Start is called before the first frame update
    void Start()
    {
        SetHighScore();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        ReadInput._ReadInput.score = m_Points;
    }

    public void GameOver()
    {
        m_GameOver = true;
        CheckScore();
        GameOverText.SetActive(true);
    }

    private void CheckScore()
    {
        int currentScore = ReadInput._ReadInput.score;

        if (currentScore > HighScore)
        {
            PlayersHighScore = ReadInput._ReadInput.InputField.text;
            HighScore = currentScore;
            
            SaveHighScore();
        }
    }

    private void SetHighScore()
    {
        if (PlayersHighScore == null & HighScore == 0)
        {
            highscorePlayer.text = "None";
        }
        else
        {
            highscorePlayer.text = $"High Score - {PlayersHighScore}:{HighScore}";
        }
        
    }

    public void SaveHighScore()
    {
        SaveData data = new SaveData();

        data.TopPlayer = PlayersHighScore;
        data.HighScore = HighScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json",json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            PlayersHighScore = data.TopPlayer;
            HighScore = data.HighScore;
        }
    }
    
   
}
[System.Serializable]
public class SaveData
{
    public int HighScore;
    public string TopPlayer;
}
