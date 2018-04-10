using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamVrController : MonoBehaviour {
    public GameObject atom;
    public Transform activeMolecule;
    public enum modes{ create,move,destroy,bond,plane,dupe,scale,vector};
    public modes currentMode;
    public float grabRange;
    RaycastHit hit;
    public bool carrying = false;
    Transform selected;
    public GameObject bondPrefab;
    public GameObject vectorPrefab;
    GameObject activeBond;
    //float distAtStartOfScale;
    //bool scaling;
    //float scaleChange;
    UIText Text;
    public GameObject planePrefab;
    List<Transform> vertices;
    public bool isLeft;
    public float scaleRate = 0.005f;
    bool scaling = false;
    GameObject scaler;
    public Transform centerMarker;
    private void Start()
    {
        scaler = new GameObject();
        vertices = new List<Transform>();
        if (activeMolecule == null)
            activeMolecule = GameObject.FindGameObjectWithTag("molecule").transform;
    }
    //kain added kainmode
    bool kainmode;

    private SteamVR_TrackedObject trackedObj;
    float padDiff=0;
    // 2
    private SteamVR_Controller.Device Controller
    {
        get { return SteamVR_Controller.Input((int)trackedObj.index); }
    }
    // runs when the game starts
    void Awake()
    {
        //kain added kainmode true
        kainmode = true;

        Text = GetComponentInChildren<UIText>();
        trackedObj = GetComponentInParent<SteamVR_TrackedObject>();
        currentMode = modes.create;

    }
    private void OnEnable()
    {
        //atom = GameObject.FindGameObjectWithTag("atom");
        //activeMolecule = GameObject.FindGameObjectWithTag("molecule").transform;
    }
    //this method cycles through the availiable modes
    private void changeMode()
    {
        currentMode = currentMode + 1;
        if ( (int) currentMode== System.Enum.GetNames(typeof(modes)).Length)
        {
            currentMode = 0;
        }
        letGo();
        if (selected != null)
        {
            selected.GetComponent<atom>().bonding = false;
            selected = null;
        }
        Text.changeText(currentMode.ToString());
        Text.changeColor((int)currentMode);
        Haptic();
    }
    //kain minus mode
    private void KainbackMode()
    {
        if (currentMode == 0)
        {
            currentMode = currentMode + System.Enum.GetValues(typeof(modes)).Length - 1;
        }
        else
        {
            currentMode = currentMode - 1;
        }

        if ((int)currentMode == System.Enum.GetNames(typeof(modes)).Length)
        {
            currentMode = 0;
        }
        letGo();
        if (selected != null)
        {
            selected.GetComponent<atom>().bonding = false;
            selected = null;
        }
        Text.changeText(currentMode.ToString());
        Text.changeColor((int)currentMode);
        Haptic();
    }
    //endkain

    void highLight()
    {
        foreach (Transform child in activeMolecule)
        {
            if (child.gameObject.CompareTag("atom"))
            {

                //kain divided grabrange by 2 & +0.025
                if (Vector3.Distance(child.position, transform.position) < grabRange/2+0.025)
                {
                    
                    if(isLeft)
                        child.gameObject.GetComponent<atom>().selectedLeft=true;
                    else
                        child.gameObject.GetComponent<atom>().selectedRight = true;
                }
                else
                {
                    if (isLeft)
                        child.gameObject.GetComponent<atom>().selectedLeft = false;
                    else
                        child.gameObject.GetComponent<atom>().selectedRight = false;
                }
            }
        }
    }
    //runs every frame checks for input and calls apropriate method
    private void Update()
    {
        if (!carrying)
            letGo();
        if ((int)currentMode == 6 && !scaling)
        {
            beginScale();
        }
        if((int)currentMode != 6)
        {
            endScale();
        }
        highLight();

        if (((int)currentMode == 0 || (int)currentMode == 3))
        {
            grabRange = 0.1f;
        }

        if (Controller.GetAxis().y-padDiff != 0)
        {

            //kain added if x < 0.5
            if ((Mathf.Abs(Controller.GetAxis().x) < 0.5) && ((int)currentMode == 1||(int)currentMode == 2 || (int)currentMode == 5))
            {
                grabRange = LimitToRange(grabRange + 0.01f * (Controller.GetAxis().y - padDiff), 0.1f, 1f);

                if (Mathf.Abs(Controller.GetAxis().y) > 0.2)
                {
                    Controller.TriggerHapticPulse(100);
                }
            }
            if ((Mathf.Abs(Controller.GetAxis().x) < 0.5) && ((int)currentMode == 6))
            {
                if (!scaling) { return; }               
                    if (Controller.GetAxis().y > 0)                   
                        scaler.transform.localScale += Controller.GetAxis().y * Vector3.one * scaleRate;                    
                    else                   
                        if (scaler.transform.localScale.x > 0)                        
                        scaler.transform.localScale += Controller.GetAxis().y * Vector3.one * scaleRate;                          
                /*
                int totalAtoms=0;
                foreach(Transform child in activeMolecule)
                {
                    if (transform.gameObject.CompareTag("atom"))
                    {
                        scaler.transform.position = scaler.transform.position+ child.position;
                        totalAtoms++;
                    }
                }
                if (totalAtoms != 0)
                    scaler.transform.position /= totalAtoms;
                foreach (Transform child in activeMolecule)
                {
                    child.parent = scaler.transform;
                    
                }
                
                if (Controller.GetAxis().y > 0)
                    scaler.transform.localScale += Controller.GetAxis().y * Vector3.one * scaleRate;
                else
                   scaler.transform.localScale += Controller.GetAxis().y * Vector3.one * scaleRate;
                foreach (Transform child in scaler.transform)
                {
                    child.parent = activeMolecule;

                }
                */
            }

        }
        padDiff =0;
        
        // 2 kain delted this
        //if (Controller.GetHairTriggerDown())
        //{
        //    Debug.Log(gameObject.name + " Trigger Press");
        //    
            /*if (currentMode == modes.scale)
                startScale(); */
        //}
        
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            //Debug.Log("left grip press");
        }
        // 3
        if (Controller.GetHairTriggerDown()||Input.GetKeyDown(KeyCode.Space))
        {
            Haptic();
            //Debug.Log(gameObject.name + " Trigger Release");
            if (currentMode == modes.create)
                create();
            if (currentMode == modes.bond)
                bond();
            if (currentMode == modes.vector)
                vector();
            if (currentMode == modes.destroy)
                delete();
            if (currentMode == modes.move)
                grab();
            if (currentMode == modes.plane)
                planeCreate();
            if (currentMode == modes.dupe)
                dupe();
            
        }

        if (Controller.GetHairTriggerUp()||Input.GetKeyDown(KeyCode.Space))
        {
            if (currentMode == modes.move|| currentMode == modes.dupe)
            {
                letGo();
                Haptic();
            }
        }

        // 4
        if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
        {
            //Debug.Log(gameObject.name + " Grip Press");
        }

        // 5 Kain added x axis control DEBUG MAY BE FALSE
        if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip)||Input.GetKeyDown(KeyCode.RightControl)|| (Controller.GetAxis().x > 0.7 && kainmode == true))
        {
            kainmode = false;
            //Debug.Log(gameObject.name + " Grip Release");
            changeMode();
        }
        //kain previous mode with x DEBUG MAY BE FALSE
        if ((Controller.GetAxis().x < -0.7 && kainmode == true))
        {
            kainmode = false;
            KainbackMode();
        }
        if (Mathf.Abs(Controller.GetAxis().x) < 0.6)
        {
            kainmode = true;
        }
        //kainend

    }
    void beginScale()
    {
        scaling = true;
        int totalAtoms = 0;
        Vector3 averageAtomPos = Vector3.zero;
        
        foreach (Transform child in activeMolecule)
        {
            if (child.gameObject.CompareTag("atom"))
            {
                Debug.Log("atom found");
                averageAtomPos += child.position;
                totalAtoms++;
            }
        }
        if (totalAtoms != 0)
            averageAtomPos /= totalAtoms;
        Debug.Log(averageAtomPos);
        scaler = new GameObject();
        scaler.transform.position = averageAtomPos;
        for (int i = activeMolecule.childCount - 1; i >= 0; --i)
        {
            Transform child = activeMolecule.GetChild(i);
            if (child.gameObject.CompareTag("atom"))
            {
                Debug.Log("moving object: " + child.name);
                child.SetParent(scaler.transform, true);
            }
        }
    }
    void endScale()
    {
        Debug.Log("endScale");
        for (int i = scaler.transform.childCount - 1; i >= 0; --i)
        {
            Transform child = scaler.transform.GetChild(i);
            Debug.Log("moving object: " + child.name);
            child.SetParent(activeMolecule, true);
        }
        scaling = false;
    }
    void dupe()
    {
        List<Transform> pickups = new List<Transform>();
        foreach (Transform child in activeMolecule)
        {
            //kain div 2 & +0.025
            if (Vector3.Distance(child.position, transform.position) < grabRange / 2 + 0.025)
            {
                Debug.Log("pickup");
                carrying = true;
                pickups.Add(child);
            }
        }
        GameObject cloneSegment = new GameObject();
        GameObject baseSegment = new GameObject();
        foreach (Transform pickup in pickups)
        {
            pickup.SetParent(baseSegment.transform);
        }
        cloneSegment = Instantiate(baseSegment);
        while (cloneSegment.transform.GetChildCount() != 0)
        {
            foreach (Transform pickup in cloneSegment.transform)
            {
                pickup.SetParent(transform);
            }
        }
        while (baseSegment.transform.GetChildCount() != 0)
        {
            foreach (Transform pickup in baseSegment.transform)
            {
                pickup.SetParent(activeMolecule);
            }
        }
        Destroy(baseSegment);
        Destroy(cloneSegment);
    }
    void planeCreate()
    {
        float lowest = 100;
        foreach (Transform child in activeMolecule)
        {
            //kain div 2 & +0.025
            if (Vector3.Distance(child.position, transform.position) < grabRange / 2 + 0.025)
            {

                if (Vector3.Distance(child.position, transform.position) < lowest && child.gameObject.CompareTag("atom"))
                {
                    lowest = Vector3.Distance(child.position, transform.position);
                    vertices.Add(child);
                    child.gameObject.GetComponent<atom>().bonding = true;
                }
            }
        }
        if (vertices.Count == 3)
        {
            GameObject plane= Instantiate(planePrefab, new Vector3(0, 0, 0),Quaternion.identity);
            plane.transform.SetParent(activeMolecule);

            plane.GetComponentInChildren<PlaneController>().setup(vertices);
            foreach(Transform a in vertices)
            {
                a.gameObject.GetComponent<atom>().bonding = false;
            }
            vertices.Clear();
        }
            
    }
    // creates a new atom at the controllers position
    void create()
    {
        Instantiate(atom, transform.position, Quaternion.identity, activeMolecule);
        //Debug.Log("keyboard working");
    }
    //if a bond is not being made start one 
    //else connect bond to other selected items 
    void bond()
    {
        float lowest = 100;
        if (selected == null)
        {
            
            
            foreach (Transform child in activeMolecule)
            {
                //kain div 2 & +0.025
                if (Vector3.Distance(child.position, transform.position) < grabRange/2+0.025)
                {

                    if (Vector3.Distance(child.position, transform.position) < lowest&&child.gameObject.CompareTag("atom")) {
                        lowest = Vector3.Distance(child.position, transform.position);
                        selected=child;
                        selected.GetComponent<atom>().bonding = true;
                    }
                }
            }
            
                
        }
        else
        {
            
            Transform other = selected;
            foreach (Transform child in activeMolecule)
            {
                if (Vector3.Distance(child.position, transform.position) < lowest && child.gameObject.CompareTag("atom"))
                {
                    lowest = Vector3.Distance(child.position, transform.position);
                    selected.gameObject.GetComponent<atom>().bonding = false;
                    selected = child;
                }
                
                
            }
            activeBond = Instantiate(bondPrefab, transform.position, Quaternion.identity);
            activeBond.transform.SetParent(activeMolecule);
            activeBond.GetComponent<bond>().Target1 = other;
            activeBond.GetComponent<bond>().Target2 = selected;
            selected.gameObject.GetComponent<atom>().bonding = false;
            selected = null;
        }
        
    }
    void vector()
    {
        float lowest = 100;
        if (selected == null)
        {


            foreach (Transform child in activeMolecule)
            {
                //kain div 2 & +0.025
                if (Vector3.Distance(child.position, transform.position) < grabRange / 2 + 0.025)
                {

                    if (Vector3.Distance(child.position, transform.position) < lowest && child.gameObject.CompareTag("atom"))
                    {
                        lowest = Vector3.Distance(child.position, transform.position);
                        selected = child;
                        selected.GetComponent<atom>().bonding = true;
                    }
                }
            }


        }
        else
        {

            Transform other = selected;
            foreach (Transform child in activeMolecule)
            {
                if (Vector3.Distance(child.position, transform.position) < lowest && child.gameObject.CompareTag("atom"))
                {
                    lowest = Vector3.Distance(child.position, transform.position);
                    selected.gameObject.GetComponent<atom>().bonding = false;
                    selected = child;
                }


            }
            activeBond = Instantiate(vectorPrefab, transform.position, Quaternion.identity);
            activeBond.transform.SetParent(activeMolecule);
            activeBond.GetComponent<bond>().Target1 = other;
            activeBond.GetComponent<bond>().Target2 = selected;
            selected.gameObject.GetComponent<atom>().bonding = false;
            selected = null;
        }

    }
    //destory atoms near controller
    void delete()
    {

        foreach (Transform child in activeMolecule) {
            //kain div 2 & +0.025
            if (Vector3.Distance(child.position, transform.position)<grabRange/2+0.025)
            {
                if (child.gameObject.CompareTag("atom") || child.gameObject.CompareTag("bond") || child.CompareTag("plane"))
                    Destroy(child.gameObject);
            }
        }
        
        
    }
    //attach selected atom to controller when trigger pressed down 
    void grab()
    {
        List<Transform> pickups = new List<Transform>();
        foreach (Transform child in activeMolecule)
        {
            //kain div 2 & +0.025
            if (Vector3.Distance(child.position, transform.position) < grabRange/2+0.025 && child.gameObject.CompareTag("atom"))
            {
                Debug.Log("pickup");
                carrying = true;
                
                pickups.Add(child);
            }
        }
        foreach(Transform pickup in pickups)
        {
            pickup.gameObject.transform.SetParent(transform);
        }
        
    }
    //release atom from grab when trigger let go
    void letGo()
    {
        carrying = false;

            foreach (Transform child in transform)
            {
            if (child.gameObject.CompareTag("atom") || child.gameObject.CompareTag("bond") || child.gameObject.CompareTag("plane"))
                {                    
                        child.gameObject.transform.SetParent(activeMolecule.transform);                   
                }
            }
    }
    /*
    //when the player presses the trigger increase or decrease scale based on proximity to the active molecule
    void startScale()
    {
        distAtStartOfScale = Vector3.Distance(transform.position, activeMolecule.transform.position);
        scaling = true;
    }
    //when the player lets the trigger go stop adjusting the scale
    void endScale()
    {
        scaling = false;
    }
    //runs every frame if scale mode and the trigger is pressed
    void scaleUpdate()
    {
        
        scaleChange = distAtStartOfScale / Vector3.Distance(transform.position, activeMolecule.transform.position);
        if (Mathf.Abs(scaleChange - 1) > 0.1f)
        {
            activeMolecule.transform.localScale *= scaleChange;
            startScale();
        }
    }
    */
    //limits a variable to a specified range
    float LimitToRange(float value, float min, float max)
    {
        if (value < min) { return min; }
        if (value > max) { return max; }
        return value;
    }
    public void Haptic()
    {
        Controller.TriggerHapticPulse(500);
    }
}
