using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewVFX : MonoBehaviour
{
    public float moveRange = 1f;  // Hareket menzili (ne kadar k���k ya da b�y�k hareket edece�i)
    public float moveSpeed = 1f;  // Hareket h�z�
    public float limitTarget = 2f;
    private Vector3 targetPosition;
    private Vector3 firstPosition;


    void Start()
    {
        // Ba�lang��ta hedef pozisyonu �u anki pozisyon olarak al�yoruz
        targetPosition = transform.position;
        firstPosition   = transform.position;
    }

    void Update()
    {
        // E�er mevcut pozisyona �ok yakla��ld�ysa yeni bir hedef pozisyon belirle

        if (Vector3.Distance(transform.position, firstPosition) > limitTarget)
        {
            targetPosition = firstPosition;
           
        }
        else if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {

            targetPosition = transform.position + new Vector3(Random.Range(-moveRange, moveRange), Random.Range(-moveRange, moveRange), Random.Range(-moveRange, moveRange));

        }

        // Hedef pozisyona do�ru hareket et
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }
}
