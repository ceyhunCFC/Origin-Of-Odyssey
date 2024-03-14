using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    public LayerMask layerMask;
    private GameObject lastHitObject;// Raycasting'in hedefleyeceği katmanlar

    void Update()
    {
        // Fare pozisyonunu al
        Vector3 mousePosition = Input.mousePosition;

        // Fare pozisyonunu dünya koordinatlarına dönüştür
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        // Raycast yaparak 3D nesneleri hedefle
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            // Eğer raycast bir nesneye çarparsa
            GameObject hitObject = hit.collider.gameObject;

            // Daha önce vurulan nesne farklı ise, o nesnenin OnPointerExit metodu çağrılır
            if (hitObject != lastHitObject && lastHitObject != null)
            {
                lastHitObject.GetComponent<CardCanvasController>().OnPointerExit();
            }

            // Yeni vurulan nesnenin OnPointerEnter metodu çağrılır
            hitObject.GetComponent<CardCanvasController>().OnPointerEnter();

            // Son vurulan nesne güncellenir
            lastHitObject = hitObject;
        }
        else
        {
            // Eğer raycast bir nesneye çarpmazsa
            // Son vurulan nesnenin OnPointerExit metodu çağrılır
            if (lastHitObject != null)
            {
                lastHitObject.GetComponent<CardCanvasController>().OnPointerExit();
                lastHitObject = null; // Son vurulan nesne sıfırlanır
            }
        }
    }
}
