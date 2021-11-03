using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class create_text_array : MonoBehaviour
{

    public TextAsset[] txt;
    public Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
    // Start is called before the first frame update

    private void Awake()
    {
        txt = Resources.LoadAll<TextAsset>("txt");
        for (int i = 0; i < txt.Length; i++)
        {
            List<string> words = new List<string>();
            var line = txt[i].text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var l in line)
            {
                words.Add(l);
            }
            dict.Add(txt[i].name, words);
        }
    }



    public string gettext(string textname, int i)
    {
        return dict[textname][i];
    }
  
    public int getlength(string txt)
    {
        return dict[txt].Count;
    }
}
