using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float orbitSpeed;
    private  Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
  
    }

    // Update is called once per frame
    void Update()
    {
        if(target ==null)
        {
            Destroy(this.gameObject);
        }
        transform.position = target.position  + offset   ;
       
        transform.RotateAround(target.position,
                                Vector3.up,
                                 orbitSpeed * Time.deltaTime);
        offset = transform.position - target.position ;
    }
}
