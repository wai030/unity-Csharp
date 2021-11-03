﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class at1 : MonoBehaviour
{
    [SerializeField]
    Animator an;
    int tmp;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            tmp = an.GetInteger("Score") + 40;
            tmp = Mathf.Clamp(tmp, -50, 50);
            an.SetInteger("Score", tmp); 
        }
    }
    
}
