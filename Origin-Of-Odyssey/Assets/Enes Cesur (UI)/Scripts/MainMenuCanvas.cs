using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject mainCardsCanvas;
    public GameObject settingsCanvas;
    public GameObject profileCanvas;
    public Text ProfileName;

    void Start()
    {
        playPanel.SetActive(true);
        mainCardsCanvas.SetActive(false);
        PlayerData user = new PlayerData();
        ProfileName.text = user.userName;
    }
    public void OpenCardsCanvas()
    {
        mainCardsCanvas.SetActive(true);
    }
    public void CloseCardsCanvas()
    {
        mainCardsCanvas.SetActive(false);
    }

    public void OpenSettingsCanvas()
    {
        settingsCanvas.SetActive(true);
    }

    public void CloseSettingsCanvas()
    {
        settingsCanvas.SetActive(false);
    }

    public void OpenProfileCanvas()
    {
        profileCanvas.SetActive(true);
    }

    public void CloseProfileCanvas()
    {
        profileCanvas?.SetActive(false);
    }

    public void OpenRoomPanel()
    {
        SceneManager.LoadScene("Lobby");
    }

    public void OpenOfflinePanel()
    {
        SceneManager.LoadScene("BattleMapOffline");
    }

    public void OpenTutorialPanel()
    {
        SceneManager.LoadScene("TutorialBattleMap");
    }
    
    public void QuitButton()
    {
        PlayerPrefs.SetString("Username", null);
        PlayerPrefs.SetString("Password", null);
        PlayerPrefs.SetInt("IsLoggedIn", 0);
    }
}