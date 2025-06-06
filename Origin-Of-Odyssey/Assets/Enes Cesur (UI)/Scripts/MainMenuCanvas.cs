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
    public GameObject eventCanvas;
    public GameObject buyCardCanvas;
    public Text ProfileName;

    void Start()
    {
        playPanel.SetActive(true);
        mainCardsCanvas.SetActive(false);
        PlayerData user = new PlayerData();
        ProfileName.text = user.UserName;
    }
    public void OpenCardsCanvas()
    {
        mainCardsCanvas.SetActive(true);
    }
    public void CloseCardsCanvas()
    {
        CardCtrl cardCtrl = FindAnyObjectByType<CardCtrl>();
        cardCtrl?.DeleteDeck();
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

    public void OpenEventCanvas()
    {
        eventCanvas.SetActive(true);
    }
    public void CloseEventCavnas()
    {
        eventCanvas?.SetActive(false);
    }

    public void OpenBuyCardCanvas()
    {
        buyCardCanvas.SetActive(true);
    }

    public void CloseBuyCardCanvas()
    {
        buyCardCanvas?.SetActive(false);
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