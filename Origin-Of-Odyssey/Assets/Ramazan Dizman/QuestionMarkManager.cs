using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkManager : MonoBehaviour
{
    public static QuestionMarkManager Instance; // Singleton yap�s�
    private bool isCardOpen = false;    // Kart a��k m�?

    private void Awake()
    {
        // Singleton yap�
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Kart a�ma izni
    public bool CanOpenCard()
    {
        return !isCardOpen; // E�er a��k kart yoksa true d�ner
    }

    // Kart a��ld� bilgisi
    public void SetCardOpen(bool isOpen)
    {
        isCardOpen = isOpen;
    }
}
