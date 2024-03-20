using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject mainCardsCanvas;
    public GameObject NavbarPanel;

    void Start()
    {
        playPanel.SetActive(true);
        NavbarPanel.SetActive(true);
        mainCardsCanvas.SetActive(false);
    }
    public void OpenCardsCanvas()
    {
        mainCardsCanvas.SetActive(true);
    }
    public void CloseCardsCanvas()
    {
        mainCardsCanvas.SetActive(false);
    }

    public void OpenRoomPanel()
    {
        SceneManager.LoadScene("Lobby");
    }
    
    public void QuitButton()
    {
        PlayerPrefs.SetString("Username", null);
        PlayerPrefs.SetString("Password", null);
        PlayerPrefs.SetInt("IsLoggedIn", 0);
    }
}