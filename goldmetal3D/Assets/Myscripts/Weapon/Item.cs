using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Ammo,
        Coin,
        Grenade,
        Heart,
        Weapon
    };
    Rigidbody rigid;
    SphereCollider sphereCollider;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider= GetComponent<SphereCollider>();
    }
    

    public Type type;
    public int value;
    private void Update()
    {
        transform.Rotate(Vector3.up * 30 * Time.deltaTime);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag =="Floor")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
}