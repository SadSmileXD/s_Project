using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type { Melee,Range};
    public Type type;

    public int damage;
    public int maxAmmo;
    public int curAmmo;

    public float rate;

    public BoxCollider meleeArea;

    public TrailRenderer trailEffect;

    private Coroutine check;

    public Transform bulletPos;
    public Transform bulletCasePos;

    public GameObject bulletCase;
    public GameObject bullet;
    public void Use()
    {
       
        if (type == Type.Melee)
        {
            if(check !=null)
            {
                StopCoroutine("Swing");
            }
            check = StartCoroutine("Swing");
        }
        else if (type == Type.Range && curAmmo >0)
        {
            curAmmo--;
            check= StartCoroutine("shot");
        }
    }

    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.1f); //0.1초 대기
        meleeArea.enabled = true;
        trailEffect.enabled = true;
        yield return new WaitForSeconds(0.2f);
        meleeArea.enabled = false;
        yield return new WaitForSeconds(0.1f);
        trailEffect.enabled = false;
    }
    IEnumerator shot()
    {
        //#총알 발사
        GameObject intantBullet =Instantiate(bullet,bulletPos.position,bulletPos.rotation);
        Rigidbody bulletRigid=intantBullet.GetComponent<Rigidbody>();
        bulletRigid.velocity = bulletPos.forward * 50;
        yield return null;
        //탄피 발사
        GameObject intantCase = Instantiate(bulletCase, bulletCasePos.position, bulletCasePos.rotation);
        Rigidbody caseRigid = intantCase.GetComponent<Rigidbody>();
        Vector3 caseVec = bulletCasePos.forward * Random.Range(-3, -2)+Vector3.up* Random.Range(2, 3);
        caseRigid.AddForce(caseVec*10,ForceMode.Impulse);
        caseRigid.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
}
