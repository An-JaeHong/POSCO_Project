using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPopupManager : MonoBehaviour
{
    private static UIPopupManager instance;
    public static UIPopupManager Instance { get { return instance; } }

    //버튼을 띄울 Background
    [SerializeField] private GameObject popupPrefab;
    //소환할 버튼 프리팹
    [SerializeField] private GameObject buttonPrefab;
    //팝업을 소환할 위치
    [SerializeField] private RectTransform canvasTransform;

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

    //실질적으로 팝업을 띄우는 함수
    public void ShowPopup(string title, Dictionary<string, UnityAction> buttons)
    {
        //팝업 프리팹 없으면
        if (popupPrefab == null)
        {
            Debug.LogError("popupPrefab이 없습니다");
            return;
        }

        //UI는 겹치면 안된다
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        //UIPopup창 소환
        currentPopup = Instantiate(popupPrefab, canvasTransform);
        UIPopup popup = currentPopup.GetComponent<UIPopup>();

        popup.SetTitle(title);

        //버튼 소환
        foreach (var button in buttons)
        {
            if (buttonPrefab == null)
            {
                return;
            }
            var buttonObj = Instantiate(buttonPrefab, popup.ButtonContainer);
            var uiButton = buttonObj.GetComponent<UIButton>();

            uiButton.ButtonPrefabSetup(button.Key, button.Value);
        }
    }

    //현재 열려있는 팝업창 닫기
    public void ClosePopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            activeButton.Clear();
        }
    }
}
