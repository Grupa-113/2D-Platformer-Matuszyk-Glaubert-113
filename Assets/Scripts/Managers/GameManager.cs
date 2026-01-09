using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public enum GameState { GAME, PAUSE_MENU, LEVEL_COMPLETED, GS_OPTIONS, STOP, GAMEOVER }




public class GameManager : MonoBehaviour
{

    [Header("Canvas")]
    public Canvas pauseMenuCanvas;
    public Canvas GameCanvas;
    public Canvas levelCompletedCanvas;
    public Canvas optionsCanvas;
    public Canvas GameOverCanvas;


    //----game canvas 
    public TMP_Text scoreLabel;
    public TMP_Text timeLabel;
    public TMP_Text enemyLabel;

    public GameObject heartPrefab;   
    public Transform heartsContainer;
    private List<GameObject> heartIcons = new List<GameObject>();


    //----options 
    public TMP_Text qualityLabel;

    //----level_completed 
    public TMP_Text scoreComletedLabel;
    public TMP_Text highScoreLabel;

    [Space]

    public GameState currentGameState = GameState.GAME;
    public static GameManager instance;

    private const int DefaultLivesNumber = 5;



    public int livesNumber=5;
    private float timer = 0;
    private int enemyNumber = 0;
    private int score = 0;


    [Space]
    //--------------keys
    private int keysFound = 0;
    private const int maxKeysNumber = 3;
    private bool keysCompleted = false;
    public Image[] keysTab;
    const string keyHighScore = "HighScoreLevel1;";
    //--------------keys
    public AudioClip lvlCompletedSound;
    public AudioClip lvlNotCompletedSound;
    private AudioSource source;

    void Update()
    {
        if (currentGameState == GameState.STOP) {
            return;
        }

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
        RestartGameParams();
        if (keysTab == null || keysTab.Length == 0)
        {
            keysTab = new Image[maxKeysNumber];
        }

        source = GetComponent<AudioSource>();
    }


    public void OnRestartButtonClicked()
    {
        RestartGameParams();
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
        SetGameState(GameState.GAMEOVER);
    }


    //---------------------OPTIONS
    public void Options()
    {
        SetGameState(GameState.GS_OPTIONS);
    }
    public void PlusButton()
    {
        QualitySettings.IncreaseLevel();
        UpdateQualityLabel();

    }
    public void MinusButton()
    {
        QualitySettings.DecreaseLevel();
        UpdateQualityLabel();
    }

    public void UpdateQualityLabel()
    {
        if (qualityLabel != null)
        {
            qualityLabel.text = QualitySettings.names[QualitySettings.GetQualityLevel()];
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
        GameOverCanvas.enabled = (currentGameState == GameState.GAMEOVER);

        if (currentGameState == GameState.LEVEL_COMPLETED)
        {
            if (keysFound >= maxKeysNumber)
            {
                levelCompletedCanvas.enabled = true;
                UpdateLevelCompletedUI(); 
            }
            else
            {
                levelCompletedCanvas.enabled = false;
                InGame();
            }
        }
        else
        {
            levelCompletedCanvas.enabled = false;
        }


        if (optionsCanvas.enabled || pauseMenuCanvas.enabled || GameOverCanvas.enabled || levelCompletedCanvas.enabled)
        {
            Time.timeScale = 0.0f;
        }
        else
        {
            Time.timeScale = 1.0f;
        }


    }



    private void UpdateLevelCompletedUI()
    {
        const int POINTS_PER_LIFE = 100;   
        const int POINTS_PER_BONUS = 5;     
        const int POINTS_PER_ENEMY = 100;  
        const int TIME_PENALTY = 5;    

        int calculatedScore = (livesNumber * POINTS_PER_LIFE)
                            + (score * POINTS_PER_BONUS)
                            + (enemyNumber * POINTS_PER_ENEMY)
                            - (int)(timer * TIME_PENALTY);

        if (calculatedScore < 0) calculatedScore = 0;



        int highScore = PlayerPrefs.GetInt(keyHighScore);


        if (highScore < calculatedScore)
        {
            highScore = calculatedScore;
            PlayerPrefs.SetInt(keyHighScore, highScore);
            PlayerPrefs.Save();
        }
        scoreComletedLabel.text = "Your score " + calculatedScore.ToString();
        highScoreLabel.text = "The best score " + highScore.ToString();
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
    public void UpdateLives(int lives)
    {
        this.livesNumber += lives;

        UpdateHeartsUI();

        if (livesNumber <= 0)
        {
            GameOver();
        }

        
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
    public void DisplayGameParams()
    {
        if (scoreLabel != null) scoreLabel.text = score.ToString();
        if (enemyLabel != null) enemyLabel.text = enemyNumber.ToString();
        UpdateHeartsUI();
    }
    public void RestartGameParams()
    {
        livesNumber = DefaultLivesNumber;
        score = 0;
        enemyNumber = 0;
        timer = 0;
        keysFound = 0;
        keysCompleted = false;
        UpdateHeartsUI();
    }

    public void UpdateHeartsUI()
    {
        if (heartsContainer == null) return;


        foreach (GameObject heart in heartIcons)
        {
            Destroy(heart);
        }
        heartIcons.Clear();

        for (int i = 0; i < livesNumber; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartsContainer);
            heartIcons.Add(newHeart);
        }
    }

  public void CheckLevelCompleted()
    {
        if (keysFound >= maxKeysNumber)
        {
            LevelCompleted();
            source.PlayOneShot(lvlCompletedSound, AudioListener.volume);
        }
        else
        {
            source.PlayOneShot(lvlNotCompletedSound, AudioListener.volume);
        }
    }
}
