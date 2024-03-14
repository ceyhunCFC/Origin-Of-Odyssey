using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    // Kartları saklayacak olan dizi
    private CardStats[] cardsArray = new CardStats[6]; // Örneğin, 6 kart var

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
        List<int> selectedIndices = new List<int>();
    
        // Kartları rastgele seç
        while (selectedIndices.Count < 3)
        {
            int randomIndex = Random.Range(0, cardsArray.Length);
            if (!selectedIndices.Contains(randomIndex))
            {
                selectedIndices.Add(randomIndex);
            }
        }

        // Seçilen kartları belirli konumlara yerleştir
        for (int i = 0; i < selectedIndices.Count; i++)
        {
            int index = selectedIndices[i];
            // Kartın ismine göre prefabı al
            GameObject prefab = null;
            switch (cardsArray[index].cardName)
            {
                case "Zeus":
                    prefab = zeusPrefab;
                    break;
                case "Genghis":
                    prefab = genghisPrefab;
                    break;
                case "Leonardo Da Vinci":
                    prefab = leonardoPrefab;
                    break;
                case "Odin":
                    prefab = odinPrefab;
                    break;
                case "Dustin":
                    prefab = dustinPrefab;
                    break;
                case "Anubis":
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

                if (spawnPoint != null)
                {
                    Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
                }
            }
        }
    }
}
