using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;
public enum GameState { GAME, PAUSE_MENU, LEVEL_COMPLETED }

public class GameManager : MonoBehaviour
{
    public TMP_Text scoreLabel;
    public TMP_Text timeLabel;
    public TMP_Text livesLabel; 
    public TMP_Text enemyLabel; 

    public Canvas pauseMenuCanvas;
    public Canvas GameCanvas;
    public Canvas levelCompletedCanvas;

    private int livesNumber=3;
    float timer = 0;
    public int enemyNumber = 0;
    public GameState currentGameState = GameState.GAME;
    public static GameManager instance;
    [SerializeField] private int score = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
            Debug.LogError("Duplicated Game Manager", gameObject);
            Destroy(gameObject);
            return;
        }
        ResetGameParams();

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

    void SetGameState(GameState newGameState)
    {
        currentGameState = newGameState;
        if (currentGameState == GameState.GAME)
        {

            GameCanvas.enabled = true;
        }
        pauseMenuCanvas.enabled = (currentGameState == GameState.PAUSE_MENU);
        levelCompletedCanvas.enabled = (currentGameState == GameState.LEVEL_COMPLETED);
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

    private void ResetGameParams()
    {
        InGame();
        if (scoreLabel != null) scoreLabel.text = score.ToString();
        if (enemyLabel != null) enemyLabel.text = enemyNumber.ToString();
        if (livesLabel != null) livesLabel.text = livesNumber.ToString();
    }

}
