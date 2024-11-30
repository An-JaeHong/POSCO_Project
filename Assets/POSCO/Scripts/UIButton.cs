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
    [SerializeField] private Button button;
    private UnityAction callback;

    //��ư ����
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    //��ư ������ ����
    public void ButtonPrefabSetup(string text, UnityAction onClick)
    {
        buttonText.text = text;

        //���� ������ ����
        if(callback != null)
        {
            button.onClick.RemoveListener(callback);
        }

        callback = onClick;
        if (callback != null)
        {
            button.onClick.AddListener(() => {
                Debug.Log($"Button {text} clicked!");
                callback.Invoke();
            });
            Debug.Log($"Callback added for button {text}");
        }
        else
        {
            Debug.LogError($"Callback is null for button {text}");
        }
    }

    //��ư ������� �����ڵ� ��������
    private void OnDisable()
    {
        if (callback != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
