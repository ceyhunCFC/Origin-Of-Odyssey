using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RayCast : MonoBehaviour
{
    public static RayCast Instance;
    private Camera mainCamera;
    private GameObject selectedCard;

    public static event Action OnCardSelected;
    
    
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
        // Ana kamera referansını al
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Fare tıklamasını algıla
        if (Input.GetMouseButtonDown(0))
        {
            // Fare pozisyonunu 3D dünyada bir ışına dönüştür
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Işının çarptığı nesneyi bul
            if (Physics.Raycast(ray, out hit))
            {
                // Eğer çarptığı nesne bir kart ise seç
                if (hit.collider.CompareTag("Card"))
                {
                    // Kartı seçme işlemleri
                    SelectCard(hit.collider.gameObject);
                    OnCardSelected?.Invoke();
                }
               // // Eğer çarptığı nesne bir buton ise
               // else if (hit.collider.CompareTag("Button"))
               // {
               //     
               //     Debug.Log("Butona tıkladık");
               //     HandleButtonClick(hit.collider.gameObject);
               // }
            }
        }
    }

    public void SelectCard(GameObject card)
    {
        // Seçili kartı değiştir
        selectedCard = card;

        // Seçili kartın üzerindeki Outline bileşenini al
        Outline outline = selectedCard.GetComponent<Outline>();
        if (outline != null)
        {
            // Outline'ı etkinleştir veya devre dışı bırak
            outline.enabled = !outline.enabled;
        }

        // Seçilen kart ile yapılacak işlemleri gerçekleştirin
        Debug.Log("Selected card: " + selectedCard.name);
    }
    
    //private void HandleButtonClick(GameObject buttonObject)
    //{
    //   CardManager.Instance.ChangeSelectedCard();
    //    
    //}
    
    // Butona tıklandığında çalışacak olan metot
    public GameObject GetSelectedCard()
    {
        return selectedCard;
    }
}
