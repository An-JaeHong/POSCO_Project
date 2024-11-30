using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPopupManager : MonoBehaviour
{
    private static UIPopupManager instance;
    public static UIPopupManager Instance { get { return instance; } }

    //�˾���� Background
    [SerializeField] private GameObject popupPrefab;
    //��ư
    [SerializeField] private GameObject buttonPrefab;
    //��� ��ġ
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

    //���������� ��ư�޸� UI�� �����ϴ� �Լ�
    public void ShowPopup(string title, Dictionary<string, UnityAction> buttons)
    {
        //�������� �ִ��� üũ
        if (popupPrefab == null)
        {
            Debug.LogError("popupPrefab�� �Ҵ���� �ʾҽ��ϴ�");
            return;
        }

        //UI�� �ߺ��ؼ� �� �� ����.
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        //�ϴ� Background ���
        currentPopup = Instantiate(popupPrefab, canvasTransform);
        UIPopup popup = currentPopup.GetComponent<UIPopup>();
        //print($"popup Created : {popup}");
        //print($"ButtonContainer : {popup.ButtonContainer}");
        //�˾� ���� �ٲ��ٰ�
        if (popup == null)
        {
            Debug.LogError("UIPopup ������Ʈ�� ã�� �� �����ϴ�!");
            return;
        }

        if (popup.ButtonContainer == null)
        {
            Debug.LogError("ButtonContainer�� �Ҵ���� �ʾҽ��ϴ�!");
            return;
        }

        popup.SetTitle(title);
        //popup.SetContent(content);


        //activeButton.Clear();
        //��ư �����ϴ� ��
        foreach (var button in buttons)
        {
            if (buttonPrefab == null)
            {
                Debug.LogError("Button Prefab�� �Ҵ���� �ʾҽ��ϴ�!");
                return;
            }
            var buttonObj = Instantiate(buttonPrefab, popup.ButtonContainer);
            var uiButton = buttonObj.GetComponent<UIButton>();

            if (uiButton == null)
            {
                Debug.LogError("UIButton ������Ʈ�� ã�� �� �����ϴ�!");
                return;
            }

            print($"SetPopup - button.key : {button.Key}");
            uiButton.ButtonPrefabSetup(button.Key, button.Value);
            //activeButton[button.Key] = uiButton;
        }
    }

    //�˾� �������� ���� �ִ� ������ �� �ı�
    public void ClosePopup()
    {
        if (currentPopup != null)
        {
            Destroy(currentPopup);
            activeButton.Clear();
        }
    }
}
