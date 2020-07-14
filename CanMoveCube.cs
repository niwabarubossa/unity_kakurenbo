using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanMoveCube : MonoBehaviour
{
    //public GameObject obstacle;
    //[HideInInspector]
    public Rigidbody myRb;


    public void setRb() {
        this.myRb = GetComponent<Rigidbody>();
    }


    void Awake()
    {
        //this.myRb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
