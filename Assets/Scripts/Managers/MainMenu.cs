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


    private IEnumerator StartGame( string levelName)
    {
        GameManager gm = GameManager.instance;
        gm.RestartGameParams();

        yield return new WaitForSeconds(0.1f);
        SceneManager.LoadScene(levelName);
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateQualityLabel();
        SetGameState(MenuState.MENU);
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InMenu()
    {
        SetGameState(MenuState.MENU);
    }

    public void Options()
    {
        SetGameState(MenuState.OPTIONS);
    }

    void SetGameState(MenuState newGameState)
    {
        currentGameState = newGameState;
        optionsCanvas.enabled = (currentGameState == MenuState.OPTIONS);
        menuCanvas.enabled = (currentGameState == MenuState.MENU);
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
}
