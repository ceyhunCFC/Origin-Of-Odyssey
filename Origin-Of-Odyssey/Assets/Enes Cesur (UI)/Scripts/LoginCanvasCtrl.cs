using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCtrl : MonoBehaviour
{
    public GameObject loginPage;
    public GameObject signUpPage;
    public GameObject darkPanel;
    public Toggle toggle,toggle1,toggleConfirm;
    public GameObject PasswordPanel;
    public GameObject confirmationPanel;

    private bool isFirstToggleClick = true;

    void Start()
    {
       // loginPage.SetActive(true);
        StartCoroutine(Intro());
      
    }

    IEnumerator Intro()
    {
        loginPage.SetActive(false);
        signUpPage.SetActive(false);
        darkPanel.SetActive(false);
        yield return new WaitForSeconds(2);
        loginPage.SetActive(true);
       
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
            //loginPage.SetActive(false);
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
            //loginPage.SetActive(false);
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

    public void ToggleConfirmChanged(bool newValue)
    {
        confirmationPanel.SetActive(true);
    }

    public void CloseConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
    }
}
