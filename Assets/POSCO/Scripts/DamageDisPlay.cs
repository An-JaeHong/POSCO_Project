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
            Debug.Log("ShowDamage called"); // 호출 횟수 확인

            // 프리팹을 인스턴스화하여 데미지 텍스트를 생성
            GameObject damageTextInstance = Instantiate(damageTextPrefab, transform.position + Vector3.up * 2, Quaternion.identity, transform);
            damageTextInstance.transform.SetParent(transform, false); // 부모를 설정하여 위치를 상대적으로 유지

            Camera mainCamera = CameraManager.Instance.currentCamera;
            //damageTextInstance.transform.LookAt(damageTextInstance.transform.position + mainCamera.transform.rotation * Vector3.forward, mainCamera.transform.rotation * Vector3.up);

            damageTextInstance.transform.LookAt(mainCamera.transform);
            damageTextInstance.transform.Rotate(0, 30f, 0); // LookAt으로 인해 뒤집힌 경우 보정
            TextMeshPro textMesh = damageTextInstance.GetComponentInChildren<TextMeshPro>();
            if (textMesh != null)
            {
                textMesh.text = damage.ToString(); // 데미지 값을 텍스트로 설정
            }
            Destroy(damageTextInstance, 1.5f); // 1.5초 후에 텍스트 제거
        }
    }
}
