using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class BossMissile : Bullet
{
    public Transform target;
    NavMeshAgent nav;
    private void Awake()
    {
        target=GameObject.Find("Player").GetComponent<Transform>();
    }
    void Start()
    {
        nav=GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target ==null)
        {
            Destroy(this.gameObject);
        }
        nav.SetDestination(target.position);
    }
}
