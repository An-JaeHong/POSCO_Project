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
            print("이미 열린 팝업이 있음");
            return;
        }

        InventoryPopUp popupInstance = Instantiate(popupPrefab).GetComponent<InventoryPopUp>();
        inventoryPopup.Push(popupInstance);
        popupInstance.gameObject.SetActive(true); // 팝업 활성화
        print("팝업이 열림");
    }

    public void ClosePopup()
    {
        if (inventoryPopup.Count > 0)
        {
            InventoryPopUp popup = inventoryPopup.Pop(); // 가장 최근 팝업
            Destroy(popup.gameObject); // 팝업 파괴
        }
    }
    public bool IsPopupOpen()
    {
        return inventoryPopup.Count > 0; // 현재 열린 팝업이 있는지 확인
    }


  
}
