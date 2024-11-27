using UnityEngine;

public class RotateTowardsMouse3D : MonoBehaviour
{
    void Update()
    {
        // 마우스 위치를 스크린 좌표에서 월드 좌표로 변환
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Ray가 부딪히는 지점이 있을 경우 해당 위치로 회전
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;

            // 마우스가 있는 방향을 바라보도록 회전
            Vector3 direction = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // 부드럽게 회전하려면 Slerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.3f);
        }
    }
}
