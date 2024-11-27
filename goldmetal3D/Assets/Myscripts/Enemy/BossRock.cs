using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class BossRock : Bullet
{
    Rigidbody rigid;

    float m_angularPower = 4;
    float m_scalseValue = 0.1f;

    bool m_isShoot;

    private void Awake()
    {
        rigid=GetComponent<Rigidbody>();
        StartCoroutine(GainPowerTimer());
        StartCoroutine(GainPower());
    }
    IEnumerator GainPowerTimer()
    {
        yield return  new WaitForSeconds(3f);
        m_isShoot = true;
    }
    IEnumerator GainPower()
    {
        while(!m_isShoot)
        {
            m_angularPower += 0.05f;
            m_scalseValue += 0.007f;
            transform.localScale = Vector3.one * m_scalseValue;
            rigid.AddTorque(transform.right * m_angularPower, ForceMode.Acceleration);
            yield return null;
          
        }
    }
  
}
