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
    private Button button;
    private UnityAction callback;

    //버튼 연결
    private void Awake()
    {
        button = GetComponent<Button>();
    }

    //버튼 프리팹 세팅
    public void ButtonPrefabSetup(string text, UnityAction onClick)
    {
        //세팅값 입력에 따라 버튼값 설정해주기
        buttonText.text = text;
        button.onClick.AddListener(onClick);
        callback = onClick;
    }

    //버튼 사라지면 구독자들 지워야함
    private void OnDisable()
    {
        if (callback != null)
        {
            button.onClick.RemoveListener(callback);
        }
    }
}
