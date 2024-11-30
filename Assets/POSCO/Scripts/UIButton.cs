using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//버튼 프리팹에 달 스크립트
public class UIButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buttonText;
    [SerializeField] private Button button;
    private UnityAction callback;

    //버튼 연결
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    //버튼 프리팹 세팅
    public void ButtonPrefabSetup(string text, UnityAction onClick)
    {
        buttonText.text = text;

        //이전 리스너 제거
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

    //버튼 사라지면 구독자들 지워야함
    private void OnDisable()
    {
        if (callback != null)
        {
            button.onClick.RemoveAllListeners();
        }
    }
}
