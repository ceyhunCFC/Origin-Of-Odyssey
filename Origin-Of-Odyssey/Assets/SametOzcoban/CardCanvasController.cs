using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardCanvasController : MonoBehaviour
{
    public GameObject _cardUi;

    // Fare kartın üzerine gelince çalışacak metot
    public void OnPointerEnter()
    {
        Debug.Log("Üzerine geldi");
        _cardUi.SetActive(true); // Kanvası aktif hale getir
    }

    // Fare kartın üzerinden çıkınca çalışacak metot
    public void OnPointerExit()
    {
        _cardUi.SetActive(false); // Kanvası pasif hale getir
    }
}
