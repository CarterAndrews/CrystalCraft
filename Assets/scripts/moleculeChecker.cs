using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moleculeChecker : MonoBehaviour {
    public GameObject activeMolecule;
    private int CUBE_BONDS = 12;
    private int CUBE_ATOMS = 8;
	// Use this for initialization
	void Start () {
		
	}
    private void Update()
    {
        //kain turned off debug log
        //Debug.Log("is the molecule a cube? "+checkIfCube(activeMolecule));
        if (checkIfCube(activeMolecule)){
            completeMolecule(activeMolecule);
        }
    }
    private void completeMolecule(GameObject molecule)
    {
        atom[] materials = molecule.GetComponentsInChildren<atom>();
        foreach (atom a in materials)
            a.complete = true;   
        Debug.Log("molecule complete");
    }
    public bool checkIfCube(GameObject molecule)
    {
        int bondCount = molecule.GetComponentsInChildren<bond>().Length;
        int atomCount = molecule.GetComponentsInChildren<atom>().Length;
        bool uniformLengths = true;
        float totalLength = 0;
        var bonds = molecule.GetComponentsInChildren<bond>();
        foreach(bond a in bonds)
            totalLength += a.length;        
        float sideLength = totalLength / bondCount;
        foreach (bond a in bonds)
        {
            if (Mathf.Abs((sideLength - a.length) / sideLength) > 0.4f)
                uniformLengths = false;
                
        }
        return bondCount == CUBE_BONDS && atomCount == CUBE_ATOMS &&uniformLengths;
    }
}
