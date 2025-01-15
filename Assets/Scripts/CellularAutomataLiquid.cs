using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellularAutomataLiquid : MonoBehaviour
{
    public bool isSolid = false;
    public bool isSpawner = false;
    public bool isLogged = false;
    public int volume = 1000; // volume of liquid in ml
    // Start is called before the first frame update
    void Start()
    {
        UpdateVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateVolume()
    {
        if(volume == 0)
        {
            this.GetComponent<MeshRenderer>().enabled = false;
            return;
        }
        this.GetComponent<MeshRenderer>().enabled = true;
        float newScale = volume / 1000f;
        if(newScale > 1) 
        {
            newScale = 1;
        }
        //Debug.Log(newScale);
        this.transform.position -= new Vector3(0, (this.transform.localScale.y - newScale) / 2f, 0);
        this.transform.localScale = new Vector3(1, newScale, 1);
        
    }
}
