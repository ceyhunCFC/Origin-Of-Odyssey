using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
   // Kartları saklayacak olan dizi
    private RandomCard[] cardsArray = new RandomCard[6]; // Örneğin, 6 kart var

    // Prefablar
    public GameObject zeusPrefab;
    public GameObject genghisPrefab;
    public GameObject leonardoPrefab;
    public GameObject odinPrefab;
    public GameObject dustinPrefab;
    public GameObject anubisPrefab;

    // Üç farklı pozisyon
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    void Start()
    {
        // Kartları oluştur
        cardsArray[0] = new ZeusCard();
        cardsArray[1] = new GenghisCard();
        cardsArray[2] = new LeonardoCard();
        cardsArray[3] = new OdinCard();
        cardsArray[4] = new DustinCard();
        cardsArray[5] = new AnubisCard();

        // Oyunu başlat
        StartGame();
    }

    void StartGame()
    {
        // 3 kartı rastgele seçerek yerleştir
        List<RandomCard> selectedCards = new List<RandomCard>();

        // Rastgele seçilen kartları al
        while (selectedCards.Count < 3)
        {
            RandomCard randomCard = cardsArray[Random.Range(0, cardsArray.Length)];
            if (!selectedCards.Contains(randomCard))
            {
                selectedCards.Add(randomCard);
            }
        }

        // Seçilen kartları belirli konumlara yerleştir
        for (int i = 0; i < selectedCards.Count; i++)
        {
            // Kartın ismine göre prefabı al
            GameObject prefab = null;
            switch (selectedCards[i].GetName())
            {
                case "Zeus":
                    prefab = zeusPrefab;
                    break;
                case "Genghis":
                    prefab = genghisPrefab;
                    break;
                case "Leonardo":
                    prefab = leonardoPrefab;
                    break;
                case "Odin":
                    prefab = odinPrefab;
                    break;
                case "Dustin":
                    prefab = dustinPrefab;
                    break;
                case "Anibus":
                    prefab = anubisPrefab;
                    break;
            }

            // Prefabı varsa objeyi oluştur
            if (prefab != null)
            {
                // Doğru spawnPoint'i seç
                Transform spawnPoint;
                if (i == 0)
                    spawnPoint = spawnPoint1;
                else if (i == 1)
                    spawnPoint = spawnPoint2;
                else
                    spawnPoint = spawnPoint3;

                // Prefabı doğru pozisyonda oluştur
                Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            }
        }

        // Oyunu devam ettirme veya başka işlemler...
    }
    
    public class ZeusCard : RandomCard
    {
        public ZeusCard()
        {
            _name = "Zeus";
        }
    }

    public class GenghisCard : RandomCard
    {
        public GenghisCard()
        {
            _name = "Genghis";
        }
    }
    public class LeonardoCard : RandomCard
    {
        public LeonardoCard()
        {
            _name = "Leonardo";
        }
    }

    public class OdinCard : RandomCard
    {
        public OdinCard()
        {
            _name = "Odin";
        }
    }
    public class DustinCard : RandomCard
    {
        public DustinCard()
        {
            _name = "Dustin";
        }
    }
    public class AnubisCard : RandomCard
    {
        public AnubisCard()
        {
            _name = "Anibus";
        }
    }
}
