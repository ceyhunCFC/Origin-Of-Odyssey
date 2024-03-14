using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject mainCardsCanvas;
    public GameObject roomPanel;
    public GameObject GameModelsPanel;
    public GameObject NavbarPanel;
    public GameObject CreateRoom;
    public GameObject InfoRoomPanel;
    public InputField roomNameInput;
    public Text warningText;
    public Text roomNameText;

    void Start()
    {
        playPanel.SetActive(true);
        NavbarPanel.SetActive(true);
        mainCardsCanvas.SetActive(false);
        roomPanel.SetActive(false);
        GameModelsPanel.SetActive(false);
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
        roomPanel.SetActive(true);
        NavbarPanel.SetActive(true);
        mainCardsCanvas.SetActive(false);
        playPanel.SetActive(false);
        GameModelsPanel.SetActive(false);
    }
    public void CloseRoomPanel()
    {
        roomPanel.SetActive(false);
        playPanel.SetActive(true);
    }
    public void SetupRoom()
    {
        GameModelsPanel.SetActive(true);
        NavbarPanel.SetActive(true);
        playPanel.SetActive(false);
        mainCardsCanvas.SetActive(false);
        roomPanel.SetActive(false);
    }
    public void CloseSetupRoom()
    {
        GameModelsPanel.SetActive(false);
        roomPanel.SetActive(true);
    }
    public void OpenCreateRoom()
    {
        CreateRoom.SetActive(true);
    }
    public void CloseCreateRoom()
    {
        CreateRoom.SetActive(false);
    }
    public void OpenInfoRoom()
    {
        if (string.IsNullOrEmpty(roomNameInput.text))
        {
            warningText.text = "Please Enter a Room Name!";
            warningText.color = Color.red;
            return;
        }
        roomNameText.text = roomNameInput.text;
        InfoRoomPanel.SetActive(true);
    }
    public void CloseInfoRoom()
    {
        InfoRoomPanel.SetActive(false);
    }

    public void QuitButton()
    {
        PlayerPrefs.SetString("Username", null);
        PlayerPrefs.SetString("Password", null);
        PlayerPrefs.SetInt("IsLoggedIn", 0);
    }
}