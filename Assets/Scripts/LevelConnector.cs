using UnityEngine;
using TMPro;

public class LevelConnector : MonoBehaviour
{

    [Header("UI References")]
    public TMP_Text sceneScoreLabel;
    public TMP_Text sceneTimeLabel;
    public TMP_Text sceneLivesLabel;
    public TMP_Text sceneEnemyLabel;

    [Header("Canvases")]
    public Canvas sceneGameCanvas;
    public Canvas scenePauseCanvas;
    public Canvas sceneLevelCompletedCanvas;

    void Start()
    {

        if (GameManager.instance != null)
        {
            GameManager gm = GameManager.instance;


            gm.scoreLabel = sceneScoreLabel;
            gm.timeLabel = sceneTimeLabel;
            gm.livesLabel = sceneLivesLabel;
            gm.enemyLabel = sceneEnemyLabel;

            gm.GameCanvas = sceneGameCanvas;
            gm.pauseMenuCanvas = scenePauseCanvas;
            gm.levelCompletedCanvas = sceneLevelCompletedCanvas;

            gm.InGame();
        }
    }
}