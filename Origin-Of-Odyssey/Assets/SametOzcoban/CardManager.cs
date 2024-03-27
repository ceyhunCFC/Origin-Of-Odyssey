using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance; // Singleton örneği
    public GameObject[] cards; // Oluşturulan kartlar
    public GameObject changeButton; // Değiştir butonu
    public GameObject selectedCard ; 
    
    void Start()
    {
        cards = GameObject.FindGameObjectsWithTag("Card"); // Kartları bul
    }
    private void Awake()
    {
        // Eğer daha önce bir örneği yoksa, bu sınıftaki örneği bu instance'a ata
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            // Eğer başka bir örneği varsa, bu örneği yok et
            Destroy(gameObject);
        }
    }
    
    void Update()
    {
        // Fare tıklaması veya dokunma kontrolü
        if (Input.GetMouseButtonDown(0)) // Sol tık veya dokunma kontrolü
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Kartın üzerine tıklandığında
                if (hit.collider.CompareTag("Card"))
                {
                    // Kartı seç ve değiştir butonunu etkinleştir
                    SelectCard(hit.collider.gameObject);
                }
            }
        }
    }

    void SelectCard(GameObject clickedCard)
    {
        selectedCard = clickedCard;
        Debug.Log(selectedCard);// Seçilen kartı kaydet
        // Diğer kartları seçilemez yap
        foreach (GameObject card in cards)
        {
            if (card != selectedCard)
            {
                // Diğer kartları gri yapabilir veya seçilemez hale getirebilirsiniz.
            }
        }
        // Değiştir butonunu etkinleştir
        changeButton.SetActive(true);
    }

    public void ChangeCard()
    {
        // Kart değiştir butonuna basıldığında yapılacak işlemler buraya yazılır
        if (selectedCard != null)
        {
           // //string[] ownDeck = PlayerController.OwnDeck
           // int randomIndex = Random.Range(0, ownDeck.Length);
           // string newCardName = ownDeck[randomIndex];
            //
           // // Yeni kart ismini seçilen karta atıyoruz
           // selectedCard.GetComponent<CardPrefab>().Card1 = newCardName;
           // // Kartın bilgilerini güncelliyoruz
           // selectedCard.GetComponent<CardPrefab>().SetInformation();
        }
    }
}
