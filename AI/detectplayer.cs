using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detectplayer : MonoBehaviour
{
    [SerializeField] Animator an;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            an.SetBool("in_range", true);
        }
    }
   
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            an.SetBool("in_range", false);
        }
    }
}
