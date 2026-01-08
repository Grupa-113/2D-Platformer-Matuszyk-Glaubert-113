using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public enum MenuState { MENU, OPTIONS }

public class MainMenu : MonoBehaviour
{
    public Canvas optionsCanvas;
    public Canvas menuCanvas;
    public MenuState currentGameState;
    public TMP_Text qualityLabel;

    void Start()
    {
        UpdateQualityLabel();
        SetGameState(MenuState.MENU);
        Time.timeScale = 1f;
    }

    void Update()
    {
        
    }

    public void InMenu()
    {
        SetGameState(MenuState.MENU);
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

    public void Options()
    {
        SetGameState(MenuState.OPTIONS);
    }

    public void OnLevel2ButtonPressed()
    {
        StartCoroutine(StartGame("Level 2"));
    }

    public void OnExitToDesktopButtonPressed()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
        Application.Quit();
    }

    void SetGameState(MenuState newGameState)
    {
        currentGameState = newGameState;
        optionsCanvas.enabled = (currentGameState == MenuState.OPTIONS);
        menuCanvas.enabled = (currentGameState == MenuState.MENU);
    }


    private IEnumerator StartGame(string levelName)
    {
        GameManager gm = GameManager.instance;
        gm.RestartGameParams();

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
    }
    
}
