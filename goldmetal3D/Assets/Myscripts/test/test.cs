using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class test : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private bool hasTriggered = false; // ī�޶� ��ȯ�� �� �� �߻��ߴ��� ���θ� ������ �÷���
    private int save = 0;
    private bool flag=true;
    private void Awake()
    {
        save = virtualCamera.Priority;
    }
    void flagfalse()
    {
        flag = false;
        virtualCamera.Priority = save;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(flag)
        {
            if (other.CompareTag("Player")) // ù ��° ���� �ÿ��� �۵�
            {
                virtualCamera.Priority = 101; // �켱������ ���� �����Ͽ� ī�޶� Ȱ��ȭ
               
                Debug.Log("test ����");
                Invoke("flagfalse", 2.0f);
            }
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (  other.CompareTag("Player")) // ù ��° ���� �ÿ��� �۵�
        {
            flag = true;


        }
    }
}
