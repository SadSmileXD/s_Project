using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPos : MonoBehaviour
{
    public Transform pos;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = pos.position; 
    }
}
