using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missilePoartA;
    public Transform missilePoartB;

    Vector3 lookVec;
    Vector3 tauntVec;

   public  bool isLook;
   
   private void Awake()
    {
        ani = GetComponentInChildren<Animator>();
      
        meshs = GetComponentsInChildren<MeshRenderer>();

        rigid = GetComponent<Rigidbody>();
        boxCollider =GetComponent<BoxCollider>();
        nav = GetComponent<NavMeshAgent>();
        
        nav.isStopped = false;
        
        StartCoroutine(Think());
      
    }
     void FixedUpdate()
    {
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
    }
    // Update is called once per frame
    void Update()
    {
        if (isDead)
        {
            StopAllCoroutines();
            return;
        }
       
        if (isLook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookVec=new Vector3(h,0, v)* 5f;
            transform.LookAt(target.position + lookVec);
        }
        else
        {if(!isLook)
            nav.SetDestination(tauntVec);
        }
    }

    IEnumerator Think()
    {
        yield return new WaitForSeconds(0.1f);

        int ranAction= UnityEngine.Random.Range(0,4);
        ranAction = 4;
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(MissileShot());
                break;
            case 2:
            case 3:
                StartCoroutine(Rockshout());
                break;
            case 4:
              StartCoroutine(Taunt());
                break;
        }
        
    }
 
  
    IEnumerator MissileShot()
    {
        ani.SetTrigger("doShot");
        yield return new WaitForSeconds(0.2f);
        GameObject instantMissileA=Instantiate(missile,missilePoartA.position, missilePoartA.rotation);
        BossMissile bossMissileA=instantMissileA.GetComponent<BossMissile>();
        bossMissileA.target= GameObject.Find("Player").GetComponent<Transform>() ;

        yield return new WaitForSeconds(0.3f);
        GameObject instantMissileB = Instantiate(missile, missilePoartB.position, missilePoartB.rotation);
        BossMissile bossMissileB = instantMissileB.GetComponent<BossMissile>();
        bossMissileA.target = GameObject.Find("Player").GetComponent<Transform>();
        

        yield return new WaitForSeconds(2.5f);

        StartCoroutine(Think());
    }

    IEnumerator Rockshout()
    {
        isLook = false;
        ani.SetTrigger("doBigShot");
        Instantiate(bullet,transform.position,transform.rotation);
        yield return new WaitForSeconds(3f);
        isLook = true;
        StartCoroutine(Think());
    }

    IEnumerator Taunt()
    {
        tauntVec=target.position  ;
       
       

        isLook = false;
        nav.isStopped=false;
        boxCollider.enabled = false;
        ani.SetTrigger("doTaunt");
      
        yield return new WaitForSeconds(3f);
        meleeArea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        meleeArea.enabled = false;

        yield return new WaitForSeconds(1f);
        isLook = true;
       
        nav.isStopped = true;
        boxCollider.enabled = true;

        yield return new WaitForSeconds(3f);
        StartCoroutine(Think());
    }
}
