using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelConnector : MonoBehaviour
{

    [Header("UI References")]
    public TMP_Text sceneScoreLabel;
    public TMP_Text sceneTimeLabel;
    public TMP_Text sceneLivesLabel;
    public TMP_Text sceneEnemyLabel;

    [Header("UI References (Buttons)")] 
    public Button resumeButton;
    public Button restartButton;
    public Button menuButton;

    [Header("Canvases")]
    public Canvas sceneGameCanvas;
    public Canvas scenePauseCanvas;
    public Canvas sceneLevelCompletedCanvas;

    void Start()
    {

        if (GameManager.instance != null)
        {
            GameManager gm = GameManager.instance;


            if (sceneScoreLabel != null) gm.scoreLabel = sceneScoreLabel;
            if (sceneTimeLabel != null) gm.timeLabel = sceneTimeLabel;
            if (sceneLivesLabel != null) gm.livesLabel = sceneLivesLabel;
            if (sceneEnemyLabel != null) gm.enemyLabel = sceneEnemyLabel;

            if (sceneGameCanvas != null) gm.GameCanvas = sceneGameCanvas;
            if (scenePauseCanvas != null) gm.pauseMenuCanvas = scenePauseCanvas;
            if (sceneLevelCompletedCanvas != null) gm.levelCompletedCanvas = sceneLevelCompletedCanvas;


            if (resumeButton != null)
            {
                resumeButton.onClick.RemoveAllListeners();
                resumeButton.onClick.AddListener(gm.OnResumeButtonClicked);
            }

            if (restartButton != null)
            {
                restartButton.onClick.RemoveAllListeners();
                restartButton.onClick.AddListener(gm.OnRetartButtonClicked);
            }

            if (menuButton != null)
            {
                menuButton.onClick.RemoveAllListeners();
                menuButton.onClick.AddListener(gm.OnRetunrToMainMenuButtonClicked);
            }


            gm.InGame();
        }
        else
        {
            Debug.LogError("LevelConnector: Nie znaleziono GameManagera!");
        }
    }
}