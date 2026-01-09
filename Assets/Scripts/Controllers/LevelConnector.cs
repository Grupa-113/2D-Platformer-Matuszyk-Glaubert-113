using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

public class LevelConnector : MonoBehaviour
{
    [Header("Canvases")]
    public Canvas sceneGameCanvas;
    public Canvas scenePauseCanvas;
    public Canvas sceneLevelCompletedCanvas;
    public Canvas sceneOptionsCanvas;
    public Canvas sceneGameOverCanvas;

    //---------------------------------GAME CANVAS 
    [Header("GAME UI")]
    public TMP_Text sceneScoreLabel;
    public TMP_Text sceneTimeLabel;
    public Transform heartsContainer;
    public Image[] keysTab;
    public TMP_Text sceneEnemyLabel;
    public GameObject heartPrefab;

    //---------------------------------PAUSE CANVAS 
    [Header("PAUSE UI")]
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;
    public Button optionsButton;

    //---------------------------------OPTIONS CANVAS 


    [Header("OPTIONS UI")]
    public Button resumeButton_options;
    public Button plus_options;
    public Button minus_options;
    public Button menuButton_options;
    public Slider slider_volume_options;
    public TMP_Text quality;
    public TMP_Text volume;

    [Header("GAMEOVER UI")]
    public Button restartButton_gameOver;
    public Button menuButton_gameOver;

    [Header("WIN UI")]
    public Button restartButton_win;
    public Button menuButton_win;
    public TMP_Text score_win;
    public TMP_Text highscore_win;

    [Header("SOUNDS")]
    public AudioClip lvlCompletedSound;
    public AudioClip lvlNotCompletedSound;
    void Start()
    {

        if (GameManager.instance != null)
        {
            GameManager gm = GameManager.instance;

            bindCanvas(gm);
            bindPauseUI(gm);
            bindGameUI(gm);
            bindOptionsUI(gm);
            bindGameOverUI(gm);
            bindWinUI(gm);
            bindSounds(gm);

            gm.UpdateQualityLabel();

            gm.InGame();
            gm.DisplayGameParams();
        }
        else
        {
            Debug.LogError("LevelConnector: Nie znaleziono GameManagera!");
        }
    }

    void bindButton(Button button, UnityAction action)
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(action);
    }

    void bindCanvas(GameManager gm)
    {
        if (sceneGameCanvas != null) gm.GameCanvas = sceneGameCanvas;
        if (scenePauseCanvas != null) gm.pauseMenuCanvas = scenePauseCanvas;
        if (sceneLevelCompletedCanvas != null) gm.levelCompletedCanvas = sceneLevelCompletedCanvas;
        if (sceneOptionsCanvas != null) gm.optionsCanvas = sceneOptionsCanvas;
        if (sceneGameOverCanvas != null) gm.GameOverCanvas = sceneGameOverCanvas;
    }

    void bindPauseUI(GameManager gm)
    {
        bindButton(resumeButton, gm.OnResumeButtonClicked);

        bindButton(restartButton, gm.OnRestartButtonClicked);

        bindButton(menuButton, gm.OnRetunrToMainMenuButtonClicked);

        bindButton(optionsButton, gm.Options);
    }

    void bindGameUI(GameManager gm)
    {
        gm.UpdateHeartsUI();
        gm.heartsContainer = this.heartsContainer;
        gm.scoreLabel = sceneScoreLabel;
        gm.timeLabel = sceneTimeLabel;
        gm.enemyLabel = sceneEnemyLabel;
        gm.heartPrefab = this.heartPrefab;

        for (int i = 0; i < gm.keysTab.Length && i < this.keysTab.Length; i++)
        {
            gm.keysTab[i] = this.keysTab[i];
        }

    }

    void bindOptionsUI(GameManager gm)
    {

        bindButton(resumeButton_options, gm.OnResumeButtonClicked);
        bindButton(plus_options, gm.PlusButton);
        bindButton(minus_options, gm.MinusButton);
        bindButton(menuButton_options, gm.PauseMenu);

        slider_volume_options.onValueChanged.RemoveAllListeners();
        //slider_volume_options.onValueChanged.AddListener(gm.SetVolume);

        if (quality != null) gm.qualityLabel = quality;
        //if (volume != null) gm.volumeLabel = volume;
    }

    void bindGameOverUI(GameManager gm)
    {
        bindButton(restartButton_gameOver, gm.OnRestartButtonClicked);
        bindButton(menuButton_gameOver, gm.OnRetunrToMainMenuButtonClicked);
    }

    void bindWinUI(GameManager gm)
    {
        bindButton(restartButton_win, gm.OnRestartButtonClicked);
        bindButton(menuButton_win, gm.OnRetunrToMainMenuButtonClicked);
        gm.scoreComletedLabel = score_win;
        gm.highScoreLabel = highscore_win;
    }

    void bindSounds(GameManager gm)
    {
        gm.lvlCompletedSound = this.lvlCompletedSound;
        gm.lvlNotCompletedSound = this.lvlNotCompletedSound;
    }
}
