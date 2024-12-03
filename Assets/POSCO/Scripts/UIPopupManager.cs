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

        popup.SetTitle(title);

        //��ư �����ϴ� ��
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
