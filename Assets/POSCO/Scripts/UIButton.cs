using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//��ư �����տ� �� ��ũ��Ʈ
public class UIButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    private Button button;
    private UnityAction callback;

    //��ư ����
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    //��ư ������ ����
    public void ButtonPrefabSetup(string text, UnityAction onClick)
    {
        //if (button == null)
        //{
        //    Debug.LogError("Button component�� ����");
        //    return;
        //}
        //print($"button setup - text : {text}");
        //���ð� �Է¿� ���� ��ư�� �������ֱ�
        buttonText.text = text;
        if(callback != null)
        {
            button.onClick.RemoveListener(callback);
        }

        button.onClick.AddListener(() => Debug.Log($"Button {text} clicked!"));
        callback = onClick;
        Debug.Log($"ButtonListener : {button.onClick.GetPersistentEventCount()}");
    }

    //��ư ������� �����ڵ� ��������
    private void OnDisable()
    {
        if (callback != null)
        {
            button.onClick.RemoveListener(callback);
        }
    }
}
