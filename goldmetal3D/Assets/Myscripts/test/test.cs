using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class test : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;

    private bool hasTriggered = false; // 카메라 전환이 한 번 발생했는지 여부를 저장할 플래그
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
            if (other.CompareTag("Player")) // 첫 번째 진입 시에만 작동
            {
                virtualCamera.Priority = 101; // 우선순위를 높게 설정하여 카메라 활성화
               
                Debug.Log("test 실행");
                Invoke("flagfalse", 2.0f);
            }
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        if (  other.CompareTag("Player")) // 첫 번째 진입 시에만 작동
        {
            flag = true;


        }
    }
}
