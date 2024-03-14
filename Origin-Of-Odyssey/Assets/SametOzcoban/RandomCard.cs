using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomCard {
    public string _name;
    public virtual string GetName()
    {
        return _name;
    }
    // Diğer kart özelliklerini ekleyin
}
