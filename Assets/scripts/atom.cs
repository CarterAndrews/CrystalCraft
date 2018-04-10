using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class atom : MonoBehaviour {
    public Material unSelectedMat;
    public Material selectedMat;
    public Material bondingMat;
    public Material completeMat;
    public bool complete = false;
    public bool bonding = false;
    public bool selectedLeft=false;
    public bool selectedRight = false;
    public Vector3 spin;
    public void Start()
    {
        if (GameObject.FindGameObjectWithTag("atom"))
        {
            transform.localScale = GameObject.FindGameObjectWithTag("atom").transform.localScale;
        }
        
    }
    private void Update()
    {
        transform.Rotate(spin);
        if (complete)
            gameObject.GetComponent<MeshRenderer>().material = completeMat;
        else if (bonding)
            gameObject.GetComponent<MeshRenderer>().material = bondingMat;
        else if(selectedLeft||selectedRight)
            gameObject.GetComponent<MeshRenderer>().material = selectedMat;       
        else        
            gameObject.GetComponent<MeshRenderer>().material = unSelectedMat;
            
        
    }


}
