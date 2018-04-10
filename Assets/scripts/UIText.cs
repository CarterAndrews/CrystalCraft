using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UIText : MonoBehaviour {
    TextMeshProUGUI modeText;
    public Color[] colors;
	// Use this for initialization
	void Start () {
        modeText = GetComponent<TextMeshProUGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void changeText(string newText)
    {
        modeText.SetText(newText);
        
    }
    public void changeColor(int number)
    {
        modeText.color = colors[number];
    }
}
