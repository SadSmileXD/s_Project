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
        isChase = false;//������ ���߰�
        isAttack = true;//����
        ani.SetBool("isAttack", true);//���ݾִϸ��̼� ����
       
        switch(m_enemyType)
        {
            case Type.A:
                yield return new WaitForSeconds(0.2f);
                meleeArea.enabled = true;
                yield return new WaitForSeconds(1f);
                meleeArea.enabled = false;
                break;

            case Type.B://����
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
     
       

        isChase = true;//������ ���߰�
        isAttack = false;//����
        ani.SetBool("isAttack", false);//���ݾִϸ��̼� ����
    }
      void FixedUpdate()
    {
        Targerting();
        if (isChase)
        {
            //�����浹�� ���� �̻����� ����
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
    { ///���� �׺���̼� 2�ʵڿ� �����isChase true���߸� ���Ͱ� �۵�
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
        ///  �÷��̾����� ������ ����ȴ�. reactVec�� ���Ͱ� ������ �ڷ� �� �˹�
        ///  �ǰ� �Ϸ��� ���� �޴´�.
        ///</summary>
       
        yield return new WaitForSeconds(0.1f);
        /// <summary>
        ///  �÷��̾ ���Ϳ��� �ǰݽ� ���׸��� �� �����ؼ� �÷����� ���������� �ٲٰ�
        ///  ���� �� ���� ������ �ٽ� �Ͼ������ ���ƿ��� 
        ///  ���Ͱ� ������ ȸ������ �ٲ۴��� ���̾� ���� �����Ѵ�.
        ///  ���̾� ���� ������ ������ physic �κп��� ���̾� 11�� ���� ���� �̱⿡
        ///  ������ �ٽ� �÷��̾�� ���� �ʰ� �ϱ� ���� ���̾ �ø���.
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
            isChase = false;//������ ��Ȱ��ȭ
            nav.enabled = false; //������ �׺���̼� ��Ȱ��ȭ
            isDead = true;
            ani.SetTrigger("doDie");//������ ���� �ִϸ��̼� ����

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
