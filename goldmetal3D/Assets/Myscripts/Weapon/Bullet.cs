using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool isMelee;
    public bool isRock;

 
    private void Awake()
    {
       
    }

    void OnCollisionEnter(Collision collision)
    { 
        
        if(!isRock && collision.gameObject.tag =="Floor")
        {
            Destroy(this.gameObject,3f);
            Debug.Log("ªË¡¶");
        }
      
       
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall")
        {

            Destroy(this.gameObject);
        }
    }
}
