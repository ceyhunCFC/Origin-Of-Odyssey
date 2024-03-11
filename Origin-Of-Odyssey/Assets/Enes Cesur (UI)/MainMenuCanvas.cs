using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuCanvas : MonoBehaviour
{
    public GameObject playPanel;
    public GameObject mainCardsPanel;
    public GameObject roomPanel;
    public GameObject gameModelsPanel;
    public GameObject navbarPanel;

    private GameObject lastOpenedPanel;

    void Start()
    {
        lastOpenedPanel = playPanel;
        OpenPanel(playPanel);
    }

    public void OpenCardsPanel()
    {
        lastOpenedPanel.SetActive(false);
        OpenPanel(mainCardsPanel);
    }

    public void OpenRoomPanel()
    {
        lastOpenedPanel.SetActive(false);
        OpenPanel(roomPanel);
    }

    public void SetupRoom()
    {
        lastOpenedPanel.SetActive(false);
        OpenPanel(gameModelsPanel);
    }

    public void ClosePanel()
    {
        lastOpenedPanel.SetActive(false);
        OpenPanel(lastOpenedPanel);
    }

    private void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        navbarPanel.SetActive(true);
        lastOpenedPanel = panel;
    }
}