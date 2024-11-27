using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    private Transform m_tartget;

    protected void SetTransform()
    {
        m_tartget = GameObject.Find("Player").GetComponent<Transform>();//씬에서 플레이어 찾아서 Transform컴포넌트 가져옴
        if (m_tartget == null)
        {
            return;
        }
    }
    protected void Awake()
    {
        SetTransform();
 
    }

    // Update is called once per frame
    protected  void Update()
    {
        if(m_tartget !=null)
        {
            transform.LookAt(m_tartget);
        }
        else
        {
            SetTransform();
        }
       
    }
}
