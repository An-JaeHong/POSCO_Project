using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryManager : MonoBehaviour
{
    private static UIInventoryManager instance;
    public static UIInventoryManager Instance {  get { return instance; } }

    private Stack<GameObject> popupStack = new Stack<GameObject>();
    private HashSet<string> activePopupTypes = new HashSet<string>();
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


    private Stack<InventoryPopUp> inventoryPopup = new Stack<InventoryPopUp>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopup();
        }
    }

    public void OpenPopup(GameObject popupPrefab)
    {
        if (inventoryPopup.Count > 0)
        {
            print("�̹� ���� �˾��� ����");
            return;
        }

        InventoryPopUp popupInstance = Instantiate(popupPrefab).GetComponent<InventoryPopUp>();
        inventoryPopup.Push(popupInstance);
        popupInstance.gameObject.SetActive(true); // �˾� Ȱ��ȭ
        print("�˾��� ����");
    }

    public void ClosePopup()
    {
        if (inventoryPopup.Count > 0)
        {
            InventoryPopUp popup = inventoryPopup.Pop(); // ���� �ֱ� �˾�
            Destroy(popup.gameObject); // �˾� �ı�
        }
    }
    public bool IsPopupOpen()
    {
        return inventoryPopup.Count > 0; // ���� ���� �˾��� �ִ��� Ȯ��
    }


  
}
