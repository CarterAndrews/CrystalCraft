using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bond : MonoBehaviour {
    public Transform Target1;
    public Transform Target2;
    public float length;
    public float scaleConstant;
	// Use this for initialization
	void Start () {
        
	}

    // Update is called once per frame
    void Update() {
        //changes linerenderer endpoints to its own positions and its targets position

        if (Target1 == null || Target2 == null)
        {
            Destroy(gameObject);
        }
        else
        {
            if (Target1 == Target2)
            {
                Destroy(gameObject);
            }
            Vector3[] positions = { Target1.position, Target2.position };
            length = Vector3.Distance(Target1.position, Target2.position);
            setTransform();
            if (Target1.Equals(Target2))
                Destroy(gameObject);
        }
       
	}
    void setTransform()
    {
        transform.localScale = new Vector3(transform.localScale.x, Vector3.Distance(Target1.position,Target2.position)*scaleConstant, transform.localScale.z);
        transform.position = (Target1.position - Target2.position) / 2 +Target1.position - (Target1.position - Target2.position);
        transform.LookAt(Target1);
        transform.Rotate(90, 0, 0);
    }
}
