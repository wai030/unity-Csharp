using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class movement : MonoBehaviour
{
    [SerializeField] bool at = false, isjump = false;
    [SerializeField] float speed = 250, move, whiteT;
    [SerializeField] Animator an;
    [SerializeField] Rigidbody2D rig;
    [SerializeField] Vector2 tr;
    [SerializeField] LayerMask mask;
    [SerializeField] ai AI;
    [SerializeField] string currentst;
    [SerializeField] int currentH = 50, jumppforce;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Material original, white;
    int i, score;
    string[] stnam;
    Dictionary<int, a> dd = new Dictionary<int, a>();
    bool b = false;
    float op, ed;
    /*attack atta = new attack();*/
    Vector2 ve;
    void Start()
    {
        
        mask = LayerMask.GetMask("Platforms");
        stnam = new string[] { "Idle", "Walk", "Hurt", "Jump" };
        original = sr.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentH <= 0)
        {
            die();
            an.SetBool("dead", true);
        }
        else
        {
            if (at)
            {
                b = true;
                score = 50;
                StartCoroutine(Tt());
            }
            ed = Time.time;
            tr = new Vector2(rig.position.x, rig.position.y);
            /*if (isground() && rig.velocity.y < -0.5)
            {
                rig.velocity = new Vector2(rig.velocity.x, -1);
            }*/
            an.SetFloat("Yforce", rig.velocity.y);
            an.SetFloat("Speed", Mathf.Abs(move));
            an.SetBool("Grounded", isground());
            if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0)
                an.SetBool("wantmove", true);
            else
                an.SetBool("wantmove", false);
            jump();
            if (Input.GetButtonUp("Horizontal"))
                move = 0;

            if (an.GetBool("canmove"))
                move = Input.GetAxis("Horizontal") * speed;
            else
                move = 0;
            if (move != 0)
                transform.localScale = new Vector3(choose(-1, 1, move) * 5, 5, 1);
            ve = rig.velocity;
        }
        Debug.Log(Time.time);
    }
    void FixedUpdate()
    {
        if (currentH <= 0)
            rig.velocity = new Vector2(0, -50);
        else
        {
            Physics2D.gravity = new Vector2(0, -9.8f);
            if (!dd.ContainsKey(an.GetCurrentAnimatorStateInfo(0).shortNameHash))
            {
                a z = new a(stname(), an.GetCurrentAnimatorStateInfo(0).length);
                dd.Add(an.GetCurrentAnimatorStateInfo(0).shortNameHash, z);
            }

            rig.velocity = new Vector2(move * Time.fixedDeltaTime, ve.y);
        }
    }

    public int choose(int min, int max, float i)
    {
        if (i == 0)
        {
            return 0;
        }
        else if (i < 0)
        {
            return min;
        }
        else
        {
            return max;
        }
    }



    public bool isground()
    {
        RaycastHit2D ra = Physics2D.CircleCast(tr, 0.1f, Vector2.down * .1f, 1.1f, mask);
        return ra.collider != null;
    }

    class a
    {
        public string name;
        public float length;
        public a(string _name, float _length)
        {
            name = _name;
            length = _length;
        }
    }

    string stname()
    {
        foreach (string s in stnam)
        {
            if (an.GetCurrentAnimatorStateInfo(0).IsName(s))
            {
                return s;
            }
        }
        return "invalid name";
    }


    IEnumerator Tt()
    {
        op = Time.time;
        at = false;
        while (score > 0)
        {
            score -= 1;
            yield return new WaitForSeconds(1);
        }
        if (score == 0 && b == true)
        {
            Debug.Log(ed - op);
            b = false;

        }
    }
    public void takedamage(int damage)
    {
            currentH -= damage;
        if (currentH <= 0)
            die();
/*        Time.timeScale = 0f;
*/        StartCoroutine(turn_white(whiteT, 4));
    }
    IEnumerator turn_white(float t, int blink)
    {
        Time.timeScale = 0;
        float pauseT = Time.realtimeSinceStartup + t;
        bool b = true;

        while (Time.realtimeSinceStartup < pauseT)
        {
            
            if (b)
            {
                b = false;
                sr.material = white;
                yield return new WaitForSecondsRealtime(t/blink);

            }
            else
            {
                b = true;
                sr.material = original; 
                yield return new WaitForSecondsRealtime(t/blink);
            }
        }
        Time.timeScale = 1f;
/*        yield return null;
*/    }
    void die()
    {
        an.Play("die");
    }
    void jump()
    {
        if (Input.GetButtonDown("Jump") && isground() && an.GetBool("canjump"))
        {
            rig.AddForce(Vector2.up * jumppforce, ForceMode2D.Impulse);
            an.SetTrigger("Jump");
        }
    }

}
