using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
public class Enemy : MonoBehaviour
{
    public enum Type { A,B,C,D};
    public Type m_enemyType;

    public int maxHealth;
    public int curHealth;

    public Transform target;

   public bool isChase;
    public bool isAttack;
    public bool isDead;
    public Rigidbody rigid;
    public BoxCollider boxCollider;
    public MeshRenderer [] meshs;

    public BoxCollider meleeArea;

    public NavMeshAgent nav;

    public Animator ani;

    public GameObject bullet;

   
    private  void Targerting()
    {
        if(!isDead && m_enemyType != Type.D)
        {
            float targetRadius = 0;
            float tartgetRange = 0;

            switch (m_enemyType)
            {
                case Type.A:
                    targetRadius = 1.5f;
                    tartgetRange = 1f;
                    break;
                case Type.B:
                    targetRadius = 1f;
                    tartgetRange = 10f;
                    break;

                case Type.C:
                    targetRadius = 0.5f;
                    tartgetRange = 25f;
                    break;
            }

            RaycastHit[] rayHits = Physics.SphereCastAll(
                transform.position, targetRadius, transform.forward, tartgetRange, LayerMask.GetMask("Player"));
            if (rayHits.Length > 0 && !isAttack)
            {
                StartCoroutine(Attack());
            }
        }
      
    }
      IEnumerator Attack()
    {
        isChase = false;//추적을 멈추고
        isAttack = true;//공격
        ani.SetBool("isAttack", true);//공격애니메이션 실행
       
        switch(m_enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                break;

            case Type.B://돌격
                yield return new WaitForSeconds(0.1f);
                rigid.AddForce(transform.forward * 20, ForceMode.Impulse);
                meleeArea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                meleeArea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantBullet=Instantiate(bullet,transform.position+new Vector3(1,1,0),transform.rotation);
                Rigidbody rigidBullet=instantBullet.GetComponent<Rigidbody>();
                rigidBullet.velocity = transform.forward * 20;

                yield return new WaitForSeconds(2f);
                break;

        }
     
       

        isChase = true;//추적을 멈추고
        isAttack = false;//공격
        ani.SetBool("isAttack", false);//공격애니메이션 실행
    }
      void FixedUpdate()
    {
        Targerting();
        if (isChase)
        {
            //물리충돌로 인한 이상현상 차단
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
     
    }
      void Awake()
    {
       // target = GameObject.Find("Player").GetComponent<Transform>();
        ani = GetComponentInChildren<Animator>();
        nav = GetComponent<NavMeshAgent>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        rigid = GetComponent<Rigidbody>();
        boxCollider= GetComponent<BoxCollider>();

        if(m_enemyType != Type.D)
        {
        Invoke("ChaseStart", 2.0f);

        }
       
       
    }
     
    void ChaseStart()
    { ///몬스터 네비게이션 2초뒤에 실행됨isChase true여야만 몬스터가 작동
        isChase = true;
        ani.SetBool("isWalk",true);
    }
    void Update()
    {
        if(nav.enabled && m_enemyType != Type.D)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !isChase;
        }
       
    }

      void OnTriggerEnter(Collider other)
    {
        if(other.tag =="Melee")
        {
            Weapon weapon=other.GetComponent<Weapon>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            Debug.Log("Melee : " + curHealth);
            StartCoroutine(OnDamage(reactVec) );
        }
        else if(other.tag =="Bullet")
        {
            Bullet weapon = other.GetComponent<Bullet>();
            curHealth -= weapon.damage;
            Vector3 reactVec = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactVec));
            Debug.Log("Range : "+curHealth);
        }
    }

    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade = false)
    {
        foreach (var mesh in meshs)
        {
            mesh.material.color = Color.red;
        }
        /// <summary>
        ///  플레이어한테 맞으면 실행된다. reactVec는 몬스터가 죽을때 뒤로 좀 넉백
        ///  되게 하려고 값을 받는다.
        ///</summary>
       
        yield return new WaitForSeconds(0.1f);
        /// <summary>
        ///  플레이어가 몬스터에게 피격시 메테리얼 에 접근해서 컬러값을 빨간색으로 바꾸고
        ///  몬스터 가 죽지 않으면 다시 하얀색으로 돌아오고 
        ///  몬스터가 죽으면 회색으로 바꾼다음 레이어 값을 변경한다.
        ///  레이어 값을 변경한 이유는 physic 부분에서 레이어 11은 죽은 몬스터 이기에
        ///  죽으면 다시 플레이어에게 맞지 않게 하기 위해 레이어를 올린다.
        ///   ///</summary>
        if (curHealth > 0)
        {
            foreach (var mesh in meshs)
            {
                mesh.material.color = Color.white;
            }
        }
        else
        {
            foreach (var mesh in meshs)
            {
                mesh.material.color = Color.gray;
            }
            gameObject.layer = 11;
            isChase = false;//죽으면 비활성화
            nav.enabled = false; //죽으면 네비게이션 비활성화
            isDead = true;
            ani.SetTrigger("doDie");//죽으면 죽음 애니메이션 실행

            if (isGrenade)
            { 
                reactVec = reactVec.normalized;
                reactVec += Vector3.up*3;
                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec*15,ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
            }
            if(m_enemyType != Type.D)
            Destroy(this.gameObject, 4);
        }

    }
    public void HitByGrenade(Vector3 explosionPos)
    {
        curHealth -= 100;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec,true));
    }
}
