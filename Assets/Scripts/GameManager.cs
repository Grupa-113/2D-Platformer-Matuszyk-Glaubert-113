using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public enum GameState { GAME, PAUSE_MENU, LEVEL_COMPLETED, GS_OPTIONS }

public class GameManager : MonoBehaviour
{
    //----game canvas 
    public TMP_Text scoreLabel;
    public TMP_Text timeLabel;
    public TMP_Text livesLabel; 
    public TMP_Text enemyLabel;

    //----options 
    public TMP_Text qualityLabel;
    public TMP_Text volumeLabel;

    //----level_completed 
    public TMP_Text scoreComletedLabel;
    public TMP_Text highScoreLabel;

    public Canvas pauseMenuCanvas;
    public Canvas GameCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;


    private int livesNumber=3;
    float timer = 0;
    public int enemyNumber = 0;
    public GameState currentGameState = GameState.GAME;
    public static GameManager instance;
    [SerializeField] private int score = 0;

    public Vector3 lastCheckPointPos = Vector3.zero;


    //--------------keys
    private int keysFound = 0;
    public const int maxKeysNumber = 3;
    public bool keysCompleted = false;
    public Image[] keysTab;
    const string keyHighScore = "HighScoreLevel1;";
    //--------------keys


    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (currentGameState == GameState.GAME)
        {
            timer += Time.deltaTime;
            int minute = (int)(timer / 60) % 60;
            timeLabel.text = string.Format("{0:00}:{1:00}", minute, timer % 60);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (currentGameState == GameState.GAME)
            {
                PauseMenu();
            }

            else if (currentGameState == GameState.PAUSE_MENU)
            {
                InGame();
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }


    public void OnRetartButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnRetunrToMainMenuButtonClicked()
    {
 
        SceneManager.LoadScene("MainMenu");
    }

    public void OnResumeButtonClicked()
    {
        InGame();
    }

    public void OnBackMenuButtonClicked()
    {
        PauseMenu();
    }

    public void PauseMenu()
    {
        SetGameState(GameState.PAUSE_MENU);
    }

    public void InGame()
    {
        SetGameState(GameState.GAME);
    }

    public void LevelCompleted()
    {
        SetGameState(GameState.LEVEL_COMPLETED);
    }

    public void GameOver()
    {
        SetGameState(GameState.PAUSE_MENU);
    }


    //---------------------OPTIONS
    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }
    public void PlusButton()
    {
        QualitySettings.IncreaseLevel();

        qualityLabel.text = QualitySettings.names[QualitySettings.GetQualityLevel()];

    }
    public void MinusButton()
    {
        QualitySettings.DecreaseLevel();
        qualityLabel.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
    }

    //---------------------OPTIONS


    public void SetVolume(float vol)
    {
        AudioListener.volume = vol;

        if (volumeLabel != null)
        {
            int volumePercentage = Mathf.RoundToInt(vol * 100);
            volumeLabel.text = volumePercentage.ToString() + "%";
        }
        
    }

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (currentGameState == GameState.GAME)
        {
            GameCanvas.enabled = true;
        }

        optionsCanvas.enabled = (currentGameState == GameState.GS_OPTIONS);
        pauseMenuCanvas.enabled = (currentGameState == GameState.PAUSE_MENU);
        levelCompletedCanvas.enabled = (currentGameState == GameState.LEVEL_COMPLETED);

        if (optionsCanvas.enabled || pauseMenuCanvas.enabled)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }

        //----levelCompleted
        if (levelCompletedCanvas.enabled)
        {
            int highScore = 0;

            if (SceneManager.GetActiveScene().name == "Level 1")
            {
                highScore = PlayerPrefs.GetInt(keyHighScore);
                if (highScore < score)
                {
                    highScore = score;
                    PlayerPrefs.SetInt(keyHighScore, highScore);
                }
            }
            scoreComletedLabel.text = "Your score " + score.ToString();
            highScoreLabel.text = "The best score " + highScore.ToString();


        }


    }





    public void AddPoints(int points)
    {
        this.score += points;
        scoreLabel.text = score.ToString();
    }
    public void AddenemiesKill(int enemy)
    {
        this.enemyNumber += enemy;
        enemyLabel.text = enemyNumber.ToString();
    }
    public void AddLives(int lives)
    {
        this.livesNumber += lives;
        livesLabel.text = livesNumber.ToString();
    }

    public void AddKeys()
    {

        keysTab[keysFound].color = Color.white;
        keysFound++;

        if (keysFound >= maxKeysNumber)
        {
            keysCompleted = true;
            Debug.Log("max klucze");
        }

    }
    //private void ResetGameParams()
    //{
    //    InGame();
    //    if (scoreLabel != null) scoreLabel.text = score.ToString();
    //    if (enemyLabel != null) enemyLabel.text = enemyNumber.ToString();
    //    if (livesLabel != null) livesLabel.text = livesNumber.ToString();
    //}

}
