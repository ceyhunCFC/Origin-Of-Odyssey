using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public static CardManager Instance; // Singleton örneği
    private List<GameObject> cardsList = new List<GameObject>(); // Oluşturulan kartları tutacak liste
    private List<GameObject> selectedCardPrefab = new List<GameObject>();

    public GameObject zeusPrefab;
    public GameObject genghisPrefab;
    public GameObject leonardoPrefab;
    public GameObject odinPrefab;
    public GameObject dustinPrefab;
    public GameObject anubisPrefab;

    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public Transform gameStarPosition;
    public Transform gameStarPosition1;
    public Transform gameStarPosition2;

    public static event Action OnCardSpawned;
    
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
    
    private void Start()
    {
        // Oyunu başlat
        StartGame();
    }

    private void StartGame()
    {
        // Kartları oluştur ve listeye ekle
        AddCard(zeusPrefab);
        AddCard(genghisPrefab);
        AddCard(leonardoPrefab);
        AddCard(odinPrefab);
        AddCard(dustinPrefab);
        AddCard(anubisPrefab);

        // Kartları yerleştir
        PlaceCards();
    }

    private void Update()
    {
        CardPosition();
    }

    private void AddCard(GameObject prefab)
    {
        cardsList.Add(prefab);
    }

    private void PlaceCards()
    {
        // Kartları rastgele seçerek yerleştir
        List<int> selectedIndices = new List<int>();

        // Kartları rastgele seç
        while (selectedIndices.Count < 3)
        {
            int randomIndex = Random.Range(0, cardsList.Count);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
            }
        }

        // Seçilen kartları belirli konumlara yerleştir
        for (int i = 0; i < selectedIndices.Count; i++)
        {
            int index = selectedIndices[i];
            GameObject prefab = cardsList[index];

            if (prefab != null)
            {
                Transform spawnPoint = i == 0 ? spawnPoint1 : (i == 1 ? spawnPoint2 : spawnPoint3);
                GameObject cardObject = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                selectedCardPrefab.Add(cardObject);
                OnCardSpawned?.Invoke();
            }
        }
    
    }
    
    public void ChangeSelectedCard()
    {
        Debug.Log("ChangeSelectedCard function called");
        if (!UIManager.isStart)
        {
            // Seçili kartın referansını al
            GameObject selectedCard = RayCast.Instance.GetSelectedCard();
        
            if (selectedCard != null)
            {
                // Rastgele bir kart seç
                int randomIndex = Random.Range(0, cardsList.Count);
                GameObject newCardPrefab = cardsList[randomIndex];

                // Seçilen kartın pozisyonunu ve dönüşünü al
                Vector3 spawnPosition = selectedCard.transform.position;
                Quaternion spawnRotation = selectedCard.transform.rotation;

                // Seçilen kartı yok et ve yeni kartı oluştur
                Destroy(selectedCard);
                selectedCardPrefab.Remove(selectedCard);
                GameObject newCardObject = Instantiate(newCardPrefab, spawnPosition, spawnRotation);
                // Yeni kartı seçili kart olarak belirle
                RayCast.Instance.SelectCard(newCardObject);
                selectedCardPrefab.Add(newCardObject);
            }
            else
            {
                Debug.LogWarning("No card selected!");
            }
        }
    }

    public void CardPosition()
    {
        if (UIManager.isStart)
        {
            // Oluşturulan kartların sayısını kontrol et
            if (selectedCardPrefab.Count != 3)
            {
                Debug.LogError("Kartlar henüz oluşturulmamış!");
                return;
            }

            // İstediğiniz konumları al ve kartları bu konumlara taşı
            for (int i = 0; i < selectedCardPrefab.Count; i++)
            {
                Transform spawnPoint = i == 0 ? gameStarPosition : (i == 1 ? gameStarPosition1 : gameStarPosition2);
                selectedCardPrefab[i].transform.position = spawnPoint.position;
                selectedCardPrefab[i].transform.rotation = spawnPoint.rotation;
            }
        }
    }
}
