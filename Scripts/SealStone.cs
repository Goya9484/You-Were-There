using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealStone : MonoBehaviour
{
    public GameObject gate;
    public static bool isSealRestoration;

    public void Awake()
    {
        isSealRestoration = false;
    }

    public void Update()
    {
        if(isSealRestoration)
            gate.gameObject.GetComponent<BoxCollider>().enabled = true;
    }

}
