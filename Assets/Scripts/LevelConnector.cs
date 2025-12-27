using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelConnector : MonoBehaviour
{
    [Header("Canvases")]
    public Canvas sceneGameCanvas;
    public Canvas scenePauseCanvas;
    public Canvas sceneLevelCompletedCanvas;
    public Canvas sceneOptionsCanvas;


    //---------------------------------GAME CANVAS 
    [Header("GAME UI")]
    public TMP_Text sceneScoreLabel;
    public TMP_Text sceneTimeLabel;
    public TMP_Text sceneLivesLabel;
    public TMP_Text sceneEnemyLabel;

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


    
    void Start()
    {

        if (GameManager.instance != null)
        {
            GameManager gm = GameManager.instance;

            bindCanvas(gm);
            bindPauseUI(gm);
            bindGameUI(gm);
            bindOptionsUI(gm);

            gm.InGame();
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
    }

    void bindPauseUI(GameManager gm)
    {
        bindButton(resumeButton, gm.OnResumeButtonClicked);

        bindButton(restartButton, gm.OnRetartButtonClicked);

        bindButton(menuButton, gm.OnRetunrToMainMenuButtonClicked);

        bindButton(optionsButton, gm.Options);
    }

    void bindGameUI(GameManager gm)
    {
        gm.scoreLabel = sceneScoreLabel;
        gm.timeLabel = sceneTimeLabel;
        gm.livesLabel = sceneLivesLabel;
        gm.enemyLabel = sceneEnemyLabel;

    }

    void bindOptionsUI(GameManager gm)
    {

        bindButton(resumeButton_options, gm.OnResumeButtonClicked);
        bindButton(plus_options, gm.PlusButton);
        bindButton(minus_options, gm.MinusButton);
        bindButton(menuButton_options, gm.PauseMenu);

        slider_volume_options.onValueChanged.RemoveAllListeners();
        slider_volume_options.onValueChanged.AddListener(gm.SetVolume);

        if (quality != null) gm.qualityLabel = quality;
        if (volume != null) gm.volumeLabel = volume;
    }


}