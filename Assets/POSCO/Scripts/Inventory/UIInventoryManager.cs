using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    private static UIInventoryManager instance;
    public static UIInventoryManager Instance {  get { return instance; } }

    private Stack<GameObject> popupStack = new Stack<GameObject>();

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
    }

    public void OpenPopup(GameObject popupPrefab)
    {

        popupStack.Push(popupPrefab);
        print("�˾��� ����");
        print(popupPrefab.name);
        print(popupStack.Count);
    }

    public void ClosePopup()
    {
        if (popupStack.Count > 0)
        {
            print("�����");
            GameObject popup = popupStack.Pop(); // ���� �ֱ� �˾�
            Destroy(popup);
            print(popupStack.Count);
        }
    }
    public int IsPopupOpen()
    {
        return popupStack.Count; // ���� ���� �˾��� �ִ��� Ȯ��
    }


  
}
