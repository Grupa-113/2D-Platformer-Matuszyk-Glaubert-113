using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

public class LevelConnector : MonoBehaviour
{

    [Header("UI References")]
    public TMP_Text sceneScoreLabel;
    public TMP_Text sceneTimeLabel;
    public TMP_Text sceneLivesLabel;
    public TMP_Text sceneEnemyLabel;

    [Header("Buttons pause")] 
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;
    public Button optionsButton;

    [Header("Buttons options")]
    public Button resumeButton_options;
    public Button plus_options;
    public Button minus_options;
    public Slider slider_volume_options;

    [Header("UI options")]
    public TMP_Text quality;
    public TMP_Text volume;


    [Header("Canvases")]
    public Canvas sceneGameCanvas;
    public Canvas scenePauseCanvas;
    public Canvas sceneLevelCompletedCanvas;
    public Canvas sceneOptionsCanvas;

    void Start()
    {

        if (GameManager.instance != null)
        {
            GameManager gm = GameManager.instance;


            if (sceneScoreLabel != null) gm.scoreLabel = sceneScoreLabel;
            if (sceneTimeLabel != null) gm.timeLabel = sceneTimeLabel;
            if (sceneLivesLabel != null) gm.livesLabel = sceneLivesLabel;
            if (sceneEnemyLabel != null) gm.enemyLabel = sceneEnemyLabel;

            if (quality != null) gm.qualityLabel = quality;
            if (volume != null) gm.volumeLabel = volume;



            if (sceneGameCanvas != null) gm.GameCanvas = sceneGameCanvas;
            if (scenePauseCanvas != null) gm.pauseMenuCanvas = scenePauseCanvas;
            if (sceneLevelCompletedCanvas != null) gm.levelCompletedCanvas = sceneLevelCompletedCanvas;
            if (sceneOptionsCanvas != null) gm.optionsCanvas = sceneOptionsCanvas;


            //-----PAUSE MENU
            bindButton(resumeButton, gm.OnResumeButtonClicked);

            bindButton(restartButton, gm.OnRetartButtonClicked);

            bindButton(menuButton, gm.OnRetunrToMainMenuButtonClicked);

            bindButton(optionsButton, gm.Options);
            //-----PAUSE MENU


            //-----OPTIONS 
            bindButton(resumeButton_options, gm.OnResumeButtonClicked);
            bindButton(plus_options, gm.PlusButton);
            bindButton(minus_options, gm.MinusButton);

            slider_volume_options.onValueChanged.RemoveAllListeners();
            slider_volume_options.onValueChanged.AddListener(gm.SetVolume);
            //-----OPTIONS 


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


}