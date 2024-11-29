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
        //���ð� �Է¿� ���� ��ư�� �������ֱ�
        buttonText.text = text;
        button.onClick.AddListener(onClick);
        callback = onClick;
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
