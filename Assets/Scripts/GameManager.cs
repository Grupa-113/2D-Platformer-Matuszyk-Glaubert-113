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
    public TMP_Text enemylabel; 

    public Canvas pauseMenuCanvas;
    public Canvas GameCanvas; 
    private int livesNumber=3;
    private float time=0;
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
        time += Time.deltaTime;
        timeLabel.text = string.Format("{0:00}:{1:00}", time / 60, time);
        livesLabel.text = livesNumber.ToString();
        enemylabel.text = enemyNumber.ToString();

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

    public void AddLives(int lives)
    {
        this.livesNumber += lives;
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
        pauseMenuCanvas.enabled = !(currentGameState == GameState.PAUSE_MENU);
        currentGameState = newGameState;

        //if(currentGameState == GameState.GAME)
        //{
        //    pauseMenuCanvas.enabled = true;
        //}
        //else
        //{
        //    pauseMenuCanvas.enabled = false;
        //}
    }

    private void Awake()
    {

        
        InGame();
        scoreLabel.text = score.ToString();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Duplicated Game Manager", gameObject);
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
        
    }
}
