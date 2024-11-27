using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;


    // Update is called once per frame
    /// <summary>
    /// ��� Update �Լ��� ȣ��� ��, ���������� ȣ��˴ϴ�. 
    /// �ַ� ������Ʈ�� ���󰡰� ������ ī�޶�� LateUpdate �� ����մϴ�
    /// (ī�޶� ���󰡴� ������Ʈ�� Update�Լ� �ȿ��� ������ ��찡 �ֱ� ����).
    /// </summary>
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
