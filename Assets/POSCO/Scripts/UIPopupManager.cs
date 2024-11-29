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

    //���������� ��ư�޸� UI�� �����ϴ� �Լ�
    public void ShowPopup(string title, string content, Dictionary<string, UnityAction> buttons)
    {
        //UI�� �ߺ��ؼ� �� �� ����.
        if (currentPopup != null)
        {
            Destroy(currentPopup);
        }

        //�ϴ� Background ���
        currentPopup = Instantiate(popupPrefab, canvasTransform);
        UIPopup popup = currentPopup.GetComponent<UIPopup>();

        //�˾� ���� �ٲ��ٰ�
        popup.SetTitle(title);
        //popup.SetContent(content);


        activeButton.Clear();
        //��ư �����ϴ� ��
        foreach (var button in buttons)
        {
            var buttonObj = Instantiate(buttonPrefab, popup.ButtonContainer);
            var uiButton = buttonObj.GetComponent<UIButton>();
            uiButton.ButtonPrefabSetup(button.Key, button.Value);
            activeButton[button.Key] = uiButton;
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
