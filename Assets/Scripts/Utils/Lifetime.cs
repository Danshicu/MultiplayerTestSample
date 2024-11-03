using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifetime 
{
    public Lifetime(GameObject gameObject, float lifetime)
    {
        GameObject.Destroy(gameObject, lifetime);
    }
}
