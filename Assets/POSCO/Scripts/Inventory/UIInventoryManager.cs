using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class UIInventoryManager : MonoBehaviour
{
    private static UIInventoryManager instance;
    public static UIInventoryManager Instance {  get { return instance; } }

    private Stack<GameObject> popupStack = new Stack<GameObject>();

    private UIInventory inventory;
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
      
        inventory=FindObjectOfType<UIInventory>();

    }


    private void Update()
    {
        
    }

    public void OpenPopup(GameObject popupPrefab)
    {

        popupStack.Push(popupPrefab);
        print("팝업이 열림");
        print(popupPrefab.name);
        //print(popupStack.Count);
    }

    public void ClosePopup()
    {
        if (popupStack.Count > 0)
        {
            print("실행됨");
            GameObject popup = popupStack.Pop(); // 가장 최근 팝업
            Destroy(popup);
            //print(popupStack.Count);
        }
        if(inventory.cardList != null)
        { inventory.AllCardbuttonStop(); }
                }
    public int IsPopupOpen()
    {
        return popupStack.Count; // 현재 열린 팝업이 있는지 확인
    }


  
}
