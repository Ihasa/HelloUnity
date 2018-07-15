using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Texter : MonoBehaviour {
    private string dispText;
    public float duration;
    private UnityEngine.UI.Text text;
    private float times = 0;
    private int chars = 0;
    private int chars_pre = 0;

	// Use this for initialization
	void Start () {
        text = GetComponent<UnityEngine.UI.Text>();
        dispText = text.text;
        text.text = "";
	}
	
	// Update is called once per frame
	void Update () {
        if (times < maxTime())
        {
            times += Time.deltaTime;
            chars = (int)(times / duration);
            if (chars != chars_pre)
            {
                text.text = dispText.Substring(0, chars);
                chars_pre = chars;
            }
        } else if (times < maxTime()+2.0f)
        {
            times += Time.deltaTime;
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    private float maxTime()
    {
        return dispText.Length * duration;
    }
}
