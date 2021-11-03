using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;


public class showcanvas : MonoBehaviour
{
    // Start is called before the first frame update
    public bool inrange = false;
    public Text t;
    public create_text_array ta;
    public Canvas cvs;
    public GameObject Panel,opt1, opt2, opt3;
    bool txtfinish= true, stop= false, pl= false;
    public static int i = 0;
    public float txtspeed;
    string txtn= "start";
    bool pause = false;
    void Awake()
    {
        t.text = "";
        if (!pl)
        {
            Panel.SetActive(true);
            pl = true;
        }
    }
    void Start()
    {
        talk();
        pause = true;
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Time.timeScale = pause ? 0f : 1f;
        if (Input.GetButtonDown("interact") && inrange == true )
        {
            talk();
        }
    }

    void talk()
    {
        t.text = "";
        if (i  == ta.getlength(txtn))
        {
            i = 0;
            Panel.SetActive(false);
            pl = false;
        }
        else if (txtfinish && !stop)
        {
            StartCoroutine(settext(ta.gettext(txtn, i)));
        }
        else if (!txtfinish)
        {
            stop = true;
        }
    }

    /*void reset_one_time()
    {
        onetime = false;
    }*/

    IEnumerator settext( string s)
    {
        txtfinish = false;
       
        switch (s)
            {

                case string a when a.Contains("!?!option1"):
                Panel.SetActive(false);
                GameObject op = Instantiate(opt2, cvs.transform);
                int e = (int)char.GetNumericValue(a[9]);//1
                lp(e, op);
                    break;

                case string a when a.Contains("!?!option2"):
                    Panel.SetActive(false);
                    GameObject ok =Instantiate(opt2, cvs.transform);
                    int k = (int)char.GetNumericValue(a[9]);//2
                    lp(k, ok);
                    break;

                case string a when a.Contains("!?!option3"):
                    Panel.SetActive(false);
                    GameObject oj = Instantiate(opt2, cvs.transform);
                    int z = (int)char.GetNumericValue(a[9]);//3
                    lp(z, oj);
                    break;
               case string a when a.Contains("###End"):
                Panel.SetActive(false);
                pl = false;
                pause = false;
                t.text = "";

                break;
                default:
                    int j = 0;
                    while (!stop && j < ta.gettext(txtn, i).Length)
                    {
                        t.text += ta.gettext(txtn, i)[j];
                        j++;
                        yield return StartCoroutine(WaitForRealSeconds(txtspeed));
                    }
                    t.text = ta.gettext(txtn, i);
                    i += 1;
                    stop = false;
                    txtfinish = true;
                    break;
            }
        Time.timeScale = pause ? 0f : 1f;



    }

    IEnumerator WaitForRealSeconds(float seconds)
    {
        float startTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup - startTime < seconds)
        {
            yield return null;
        }
    }
    void lp(int k, GameObject op)
    {
        string n = "optionpanel(Clone)/Button (0)/Text";
        n = n.Insert(0, k.ToString());
        string opn = n.Substring(0, 19);
        for (int t = 0; t < k; t++)
        {
            i += 1;
            string z = ta.gettext(txtn, i);
            n = n.Remove(28, 1);
            n = n.Insert(28, t.ToString());
            string btn = n.Replace("/Text", "");
            Text f = cvs.transform.Find(n).GetComponent<Text>();
            Button bt = cvs.transform.Find(btn).GetComponent<Button>();
            
            if (ta.gettext(txtn, i).Contains("#!("))
            {
                int first= z.IndexOf("(");
                int last = z.IndexOf(")");
                int diff = last - first;
                string fil = z.Substring(first+1, diff-1);
                int r= z.LastIndexOf("))");
                z = z.Substring(r + 2);
                bt.onClick.AddListener(() => TaskOnClick(fil, op));
            }
            
            f.text = z;
        }
    }
    void TaskOnClick(string q, GameObject opt)
    {
        txtn = q;
        i = 0;
        txtfinish = true;
        stop = false;
        Panel.SetActive(true);
        opt.SetActive(false);
        talk();
    }
    public bool returnpause()
    {
        return pause;
    }

    public void another_speech(string i)
    {
        txtn = i;
        talk();
        Panel.SetActive(true);
        pl = true;
    }
}
