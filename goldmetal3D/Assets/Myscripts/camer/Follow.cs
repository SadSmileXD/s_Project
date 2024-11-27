using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;


    // Update is called once per frame
    /// <summary>
    /// 모든 Update 함수가 호출된 후, 마지막으로 호출됩니다. 
    /// 주로 오브젝트를 따라가게 설정한 카메라는 LateUpdate 를 사용합니다
    /// (카메라가 따라가는 오브젝트가 Update함수 안에서 움직일 경우가 있기 때문).
    /// </summary>
    void LateUpdate()
    {
        transform.position = target.position + offset;
    }
}
