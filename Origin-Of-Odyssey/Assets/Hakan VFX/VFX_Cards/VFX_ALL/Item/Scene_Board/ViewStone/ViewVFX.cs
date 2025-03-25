using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVFX : MonoBehaviour
{
    public float moveRange = 1f;  // Hareket menzili (ne kadar küçük ya da büyük hareket edeceði)
    public float moveSpeed = 1f;  // Hareket hýzý
    public float limitTarget = 2f;
    private Vector3 targetPosition;
    private Vector3 firstPosition;


    void Start()
    {
        // Baþlangýçta hedef pozisyonu þu anki pozisyon olarak alýyoruz
        targetPosition = transform.position;
        firstPosition   = transform.position;
    }

    void Update()
    {
        // Eðer mevcut pozisyona çok yaklaþýldýysa yeni bir hedef pozisyon belirle

        if (Vector3.Distance(transform.position, firstPosition) > limitTarget)
        {
            targetPosition = firstPosition;
           
        }
        else if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {

            targetPosition = transform.position + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange, moveRange), Random.Range(-moveRange, moveRange));

        }

        // Hedef pozisyona doðru hareket et
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
