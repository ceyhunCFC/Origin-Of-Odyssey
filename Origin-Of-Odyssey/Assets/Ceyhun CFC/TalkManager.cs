using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Remover());
    }

    IEnumerator Remover()
    {

        yield return new WaitForSeconds(2);

        Destroy(this.gameObject);
    }
    
}
