using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public GameObject loginPage;
    public GameObject signUpPage;
    public GameObject darkPanel;
    public Toggle toggle,toggle1;
    public GameObject PasswordPanel;

    private bool isFirstToggleClick = true;

    void Start()
    {
        loginPage.SetActive(true);
        signUpPage.SetActive(false);
        darkPanel.SetActive(false);
    }

    public void OpenSignUpPage()
    {
        signUpPage.SetActive(true);
        loginPage.SetActive(false);
        darkPanel.SetActive(false);
        isFirstToggleClick = true;
    }
    public void loginbtn()
    {
        loginPage.SetActive(true);
        signUpPage.SetActive(false);
        darkPanel.SetActive(false);
    }

    public void ToggleChanged(bool newValue)
    {
        if (isFirstToggleClick)
        {
            darkPanel.SetActive(true);
            isFirstToggleClick = false;
            loginPage.SetActive(false);
            if (!toggle.isOn)
                toggle.interactable = false;
        }
    }

    public void ToggleChanged1(bool newValue)
    {
        if (isFirstToggleClick)
        {
            darkPanel.SetActive(true);
            isFirstToggleClick = false;
            loginPage.SetActive(false);
            if (!toggle1.isOn)
                toggle1.interactable = false;
        }
    }
    
    public void passwordPanel()
    {
        PasswordPanel.SetActive(true);
        loginPage.SetActive(false);

    }
    public void closePasswordPanel()
    {
        PasswordPanel.SetActive(false);
        loginPage.SetActive(true);
    }
}
