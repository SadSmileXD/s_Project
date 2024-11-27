using UnityEngine;

public class RotateTowardsMouse3D : MonoBehaviour
{
    void Update()
    {
        // ���콺 ��ġ�� ��ũ�� ��ǥ���� ���� ��ǥ�� ��ȯ
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Ray�� �ε����� ������ ���� ��� �ش� ��ġ�� ȸ��
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.point;

            // ���콺�� �ִ� ������ �ٶ󺸵��� ȸ��
            Vector3 direction = targetPosition - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction);

            // �ε巴�� ȸ���Ϸ��� Slerp ���
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 0.3f);
        }
    }
}
