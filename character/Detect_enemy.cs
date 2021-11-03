using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detect_enemy : MonoBehaviour
{
    [SerializeField]
    Animator an;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            an.SetInteger("Score", 0);
        }
    }
}
