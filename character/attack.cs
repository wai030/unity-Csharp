using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class attack : MonoBehaviour
{
    public Animator an;
    float at = 0, t1 = 0, decisiont =-3;
    [SerializeField]
    PolygonCollider2D at1, at2, at3;
    int tmp = 0;
    bool is_facing, pause;
    public struct customdata 
    {
        public int score;
        public float time;
    }
    [SerializeField] showcanvas sc;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pause = sc.returnpause();
        if (!an.GetBool("dead"))
        {
            if (!pause)
            {
                is_facing = an.GetBool("is_facing");
                facingscore();
                if (an.GetBool("Grounded") && Time.time >= at && Input.GetButtonDown("Fire1") && an.GetBool("canattack"))
                {
                    at = Time.time + 0.7f;
                    t1 = Time.time + 0.4f;
                    an.Play("player_attack2");
                }
                if (an.GetBool("Grounded") && ((Time.time >= at && Input.GetButton("Fire2")) && an.GetBool("canattack") || (Time.time >= t1 && Input.GetButtonDown("Fire2"))))
                {
                    at = Time.time + 1f;
                    t1 = Time.time + 1f;
                    an.Play("player_attack3");
                }
                if (an.GetBool("Grounded") && ((Time.time >= at && Input.GetButton("Fire3")) && an.GetBool("canattack") || (Time.time >= t1 && Input.GetButtonDown("Fire3"))))
                {
                    at = Time.time + 1.2f;
                    t1 = Time.time + 1.2f;
                    an.Play("player_attack");
                }
            }
        }
     }
    public void block()
    {
        Debug.Log("blocking");
    }
    public void unblocking()
    {
        Debug.Log("unblocking");
    }
    public void canmove()
    {
        an.SetBool("canmove", true);
        an.SetBool("canjump", true);
    }
    public void cantmove()
    {
        an.SetBool("canmove", false);
        an.SetBool("canjump", false);
    }

    public void canattack()
    {
        an.SetBool("canattack", true);
    }
    public void cantattack()
    {
        an.SetBool("canattack", false);
    }
    public void attacking()
    {
        an.SetBool("attacking", true);
    }
    public void attacknot()
    {
        an.SetBool("attacking", false);
    }

    void facingscore()
    {
        if(an.GetInteger("Score")<= 0) { 
            if (is_facing)
            {
                an.SetInteger("Score", 0);
            }
            else if(!is_facing && (Time.time - decisiont >3 || an.GetInteger("Score")==0))
            {
                an.SetInteger("Score", -20);
            }
        }
    }
    public void scoree(int score)
    {
        if (is_facing)
        {
            tmp = an.GetInteger("Score") + score;
            tmp = Mathf.Clamp(tmp, 0, 50);
            an.SetInteger("Score", tmp);
        }
        else
        {
            decisiont = Time.time;
            tmp = -50;
            an.SetInteger("Score", -50);
        }
    }

    /*public void set_score(int score)
    {
        an.SetInteger("Score", score);
    }*/
    public void set_tmp_score()
    {
        an.SetInteger("Score", tmp);
    }


}
