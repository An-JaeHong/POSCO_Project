using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPopupManager : MonoBehaviour
{
    private static UIPopupManager instance;
    public static UIPopupManager Instance { get { return instance; } }

    //팝업띄울 Background
    [SerializeField] private GameObject popupPrefab;
    //버튼
    [SerializeField] private GameObject buttonPrefab;
    //띄울 위치
    [SerializeField] private Transform canvasTransform;

    private GameObject currentPopup;
    private Dictionary<string, UIButton> activeButton = new Dictionary<string, UIButton>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //실질적으로 버튼달린 UI를 생성하는 함수
    public void ShowPopup(string title, string content, Dictionary<string, UnityAction> buttons)
    {
        //UI는 중복해서 뜰 수 없다.
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        //일단 Background 띄움
        currentPopup = Instantiate(popupPrefab, canvasTransform);
        UIPopup popup = currentPopup.GetComponent<UIPopup>();

        //팝업 내용 바꿔줄곳
        popup.SetTitle(title);
        //popup.SetContent(content);


        activeButton.Clear();
        //버튼 생성하는 곳
        foreach (var button in buttons)
        {
            var buttonObj = Instantiate(buttonPrefab, popup.ButtonContainer);
            var uiButton = buttonObj.GetComponent<UIButton>();
            uiButton.ButtonPrefabSetup(button.Key, button.Value);
            activeButton[button.Key] = uiButton;
        }
    }

    //팝업 닫을때도 현재 있는 정보들 다 파괴
    public void ClosePopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            activeButton.Clear();
        }
    }
}
