using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RarityText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countTXT;
    private int _count=0;
    
    public void AddCount(int count)
    {
        _count += count;
        countTXT.text = _count.ToString();
        gameObject.SetActive(_count > 0);
    }

    public void SetCount(int i)
    {
        _count = i;
        countTXT.text = _count.ToString();
        gameObject.SetActive(_count > 0);
    }
}
