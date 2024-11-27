using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;

    

    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(3f);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity= Vector3.zero;

        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayHits = Physics.SphereCastAll(
            transform.position  , 15, Vector3.up, 0f, LayerMask.GetMask("enemy"));

        foreach (var hit in rayHits )
        {
            hit.transform.GetComponent<Enemy>().HitByGrenade(transform.position);
        }
        Destroy(gameObject, 5);
    }
    void Start()
    {
        StartCoroutine(Explosion());
    }

    
    void Update()
    {
        
    }
}