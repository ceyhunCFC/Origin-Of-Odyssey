using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionMarkManager : MonoBehaviour
{
    public static QuestionMarkManager Instance; // Singleton yapýsý
    private bool isCardOpen = false;    // Kart açýk mý?

    private void Awake()
    {
        // Singleton yapý
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Kart açma izni
    public bool CanOpenCard()
    {
        return !isCardOpen; // Eðer açýk kart yoksa true döner
    }

    // Kart açýldý bilgisi
    public void SetCardOpen(bool isOpen)
    {
        isCardOpen = isOpen;
    }
}
