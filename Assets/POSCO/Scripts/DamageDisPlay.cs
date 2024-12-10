using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageDisPlay : MonoBehaviour
{
    public GameObject damageTextPrefab;

    public void ShowDamage(float damage)
    {
        if (damageTextPrefab != null)
        {
            Debug.Log("ShowDamage called"); // ȣ�� Ƚ�� Ȯ��

            // �������� �ν��Ͻ�ȭ�Ͽ� ������ �ؽ�Ʈ�� ����
            GameObject damageTextInstance = Instantiate(damageTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            damageTextInstance.transform.SetParent(transform, false); // �θ� �����Ͽ� ��ġ�� ��������� ����

            Camera mainCamera = CameraManager.Instance.currentCamera;
            //damageTextInstance.transform.LookAt(damageTextInstance.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);

            damageTextInstance.transform.LookAt(mainCamera.transform);
            damageTextInstance.transform.Rotate(0, 30f, 0); // LookAt���� ���� ������ ��� ����
            TextMeshPro textMesh = damageTextInstance.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = damage.ToString(); // ������ ���� �ؽ�Ʈ�� ����
            }
            Destroy(damageTextInstance, 1.5f); // 1.5�� �Ŀ� �ؽ�Ʈ ����
        }
    }
}
