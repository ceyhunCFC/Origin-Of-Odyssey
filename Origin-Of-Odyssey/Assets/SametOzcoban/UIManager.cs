using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;


public class UIManager : MonoBehaviour
{
    [Header("Değiştir Butonu Kontrol")]
    [SerializeField] private  Button changeButton;
    [Header("Değiştir ve Başla UI")]
    [SerializeField] private GameObject _startGameUI;

    public static bool isStart;
    public bool isStartPressed;
    public bool isChangePressed;
    private void Update()
    {

        if (isStartPressed)
        {
            _startGameUI.SetActive(false);
            isStart = true;
        }
    }

    private void OnEnable()
    {
        RayCast.OnCardSelected += ActivateCardSelected;
        CardManager.OnCardSpawned += ActivateStartUI;
    }
    private void OnDisable()
    {
        RayCast.OnCardSelected -= ActivateCardSelected;
        CardManager.OnCardSpawned -= ActivateStartUI;
        
    }

    public void ActivateCardSelected()
    {
        changeButton.interactable = true;
        isChangePressed = true;
        CardManager.Instance.ChangeSelectedCard();
        _startGameUI.SetActive(false);
    }
    public void ActivateStartUI()
    {
        _startGameUI.SetActive(true);
    }
}
