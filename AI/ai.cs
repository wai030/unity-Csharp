using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ai : MonoBehaviour
{

    [SerializeField] int currentH, maxH = 100;
    [SerializeField] Transform ptr, tr;
    [SerializeField] Animator pan, kan;
    [SerializeField] int speed = 5, i = 0, tmpi =4, ttmpi =0 ;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] CapsuleCollider2D pcc, kcc;
    [SerializeField] float white_time;
    [SerializeField] GameObject flag;
     enum mind { aggressive, passive, middle }
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Material original, white;
    [SerializeField] mind md = new mind();
    [SerializeField] state st = new state();
    [SerializeField] bool can_attack = true;
    [SerializeField] showcanvas sc;
    float xdtr, t = 0, dasht =-3, at = -3, bt =-10;
    int score = 0, direction;
    bool is_aggressive = false,  is_facing, is_left, is_dashing = false, invincible= false, dead =false, pause = false;
    int[] sttime = new int[6];
    

    void Start()
    {
        st = state.idle;
        currentH = maxH;
        original = sr.material;
    }
    void Update()
    {
        pause = sc.returnpause();
        if (!dead)
        {
            if (!pause)
            {
                facing_dir();
                is_inrange();
                score = pan.GetInteger("Score");
                score = Mathf.Clamp(score, -50, 50);
                determine();
                relocate();
            }
            else
            {
                kan.Play("idle");
                
            }
        }
    }

    void determine()
    {
        if (score > 30)
        {
            t = Time.time;
            md = mind.passive;
        }
        else if (pan.GetInteger("Score") <= 30 && pan.GetInteger("Score") >= -20)
        {
            md = mind.middle;
        } 
        else if (Time.time - t > 10 || pan.GetInteger("Score") == -50)
        {
            md = mind.aggressive;
        }
        
    }

    void relocate()
    {
        if (!is_inrange())
        {
            if(st == state.block)
            {
                bt = -10; 
                st = state.walk;
                kan.Play("idle");
            }
            if (can_attack && md == mind.aggressive && (st == state.idle || st == state.walk) && Mathf.Abs(xdtr) > 7 && Time.time - dasht > 3)
            {
                dasht = Time.time;
                StartCoroutine(dash());
            }
            else if (can_attack && (st == state.idle || st == state.walk))
            {
                walk();
            }
        }
        else if (can_attack && is_inrange())
            action();
            
    }

    void walk()
    {
        if (!is_inrange())
        {
            kan.Play("run");
            st = state.walk;
            if (is_left)
            {
                tr.Translate(-1 * Time.deltaTime * speed, 0, 0);
                tr.localScale = new Vector3(-1, 1, 1);
            }

            else
            {
                tr.Translate(1 * Time.deltaTime * speed, 0, 0);
                tr.localScale = new Vector3(1, 1, 1);
            }

        }
        else
        {
            kangoidle();
        }
    }

    IEnumerator dash()
    {
        Debug.Log("dash");
        st = state.dash;
        can_attack = false;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(1000000f * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.2f);
        rb.gravityScale = gravity;
        rb.velocity = new Vector2(0f, 0f);
        can_attack = true;
        kangoidle();
    }

    void action()
    {
            switch (md)
            {
                case mind.aggressive:
                    if ( Time.time - at > 3)
                        attack();
                    else
                        kangoidle();
                    break;
                case mind.middle:
                    if (pan.GetBool("Grounded") && !pan.GetBool("canattack")&& is_facing)
                        StartCoroutine(roll());
                else if (Time.time - at > 5 && pan.GetBool("Grounded"))
                {
                    attack();
                }
                else
                        kangoidle();
                break;
                case mind.passive:
                if (st != state.block && !pan.GetBool("canattack") && is_facing)
                    {
                        bt = Time.time;
                        kan.Play("block");
                        st = state.block;
                    }
                    else if (Time.time - bt > Random.Range(4,8))
                    {
                    kangoidle();
                    }
                        break;
            }

    }

    IEnumerator roll()
    {
        kan.Play("roll");
        Physics2D.IgnoreCollision(pcc, kcc, true);
        st = state.roll;
        can_attack = false;
        if (is_left)
        {
            tr.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            tr.localScale = new Vector3(1, 1, 1);
        }
        st = state.roll;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(130000f * direction, 0f), ForceMode2D.Impulse);
        float gravity = rb.gravityScale;
        rb.gravityScale = 0;
        yield return new WaitForSeconds(0.7f);
        rb.gravityScale = gravity;
        rb.velocity = new Vector2(0f, 0f);
        Physics2D.IgnoreCollision(pcc, kcc, false);
        can_attack = true;
        kangoidle();
    }

    void attack()
    {
        i = Random.Range(0, 3);
        if (tmpi != i)
            tmpi = i;
        else
        {
            switch (i) 
            {
                case 0:
                    i= Random.Range(1, 3);
                    break;
                case 1:
                    ttmpi = Random.Range(0, 2);
                    switch (ttmpi) 
                    {
                        case 0:
                            i = 0;
                            break;
                        case 1:
                            i = 2;
                            break;
                    }
                    break;
                case 2:
                    i = Random.Range(0, 2);
                    break;
            }

        }
        switch (i) 
        {
            case 0:
                kan.Play("attack");
                can_attack = false;
                st = state.attack;
                at = Time.time;
                break;
            case 1:
                kan.Play("attack2");
                can_attack = false;
                st = state.attack;
                at = Time.time;
                break;
            case 2:
                kan.Play("attack3");
                can_attack = false;
                st = state.attack;
                at = Time.time;
                break;

        }

    }
    void keepattack()
    {
        if (is_aggressive && is_inrange() && can_attack)
        {

        }
    }

    public void takedamage(int damage)
    {
        if (!invincible) { 
        currentH -= damage;
            if (currentH <= 0)
            {
                dead = true;
                die();
            }
            StartCoroutine(turn_white(white_time));
        }
    }
    IEnumerator turn_white(float t)
    {
        sr.material = white;
        yield return new WaitForSeconds(t);
        sr.material = original;
    }
    void die()
    {
        kan.Play("death");
        st = state.dead;
    }


    enum state
    {
        attack,
        walk,
        deflect,
        block,
        roll, 
        idle,
        dead,
        dash
    }
   
    void facing_dir()
    {
        xdtr = tr.position.x - ptr.position.x;
        if (xdtr > 0)
        {
            direction = -1;
            is_left = true;
            if (ptr.localScale.x > 0)
                is_facing = true;
            else
                is_facing = false;
        }
        else
        {
            direction = 1;
            is_left = false;
            if (ptr.localScale.x > 0)
                is_facing = false;
            else
                is_facing = true;
        }
        pan.SetBool("is_facing", is_facing);
    }
    
    bool is_inrange()
    {
        return kan.GetBool("in_range");
    }

    public void kangoidle()
    {
        kan.Play("idle");
        st = state.idle;
        can_attack = true;
    }
    //anim-function{
    void already_dead()
    {
        Physics2D.IgnoreCollision(pcc, kcc, true);
        flag.SetActive(true);
    }

    public void vincible()
    {
        invincible = true;
    }
    
    public void vinciblenot()
    {
        invincible = false;
    }
    //}anim-function
}
