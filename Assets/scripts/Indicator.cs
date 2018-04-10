using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour {

    public GameObject indicator;
    public SteamVrController controller;
    // Use this for initialization
    void Start () {
        indicator = Instantiate(indicator, transform.position, Quaternion.identity);
        indicator.transform.SetParent(transform);
        indicator.transform.localPosition = Vector3.zero;
        indicator.transform.localScale = Vector3.one * controller.grabRange;
        controller = gameObject.GetComponent<SteamVrController>();
    }
	
	// Update is called once per frame
	void Update () {
        indicator.transform.localScale = Vector3.one * controller.grabRange;
    }
}
