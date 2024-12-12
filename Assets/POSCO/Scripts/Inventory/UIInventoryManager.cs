using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using static UnityEditor.Progress;

public class UIInventoryManager : MonoBehaviour
{
    private static UIInventoryManager instance;
    public static UIInventoryManager Instance {  get { return instance; } }

    public Stack<GameObject> popupStack = new Stack<GameObject>();

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
        print(popupPrefab.name);
        print("ÆË¾÷ÀÌ ¿­¸²");
        print(popupStack.Count);
    }

    public void ClosePopup()
    {
        if (popupStack.Count > 0)
        {
            print("´ÝÈû");
            GameObject popup = popupStack.Pop(); // °¡Àå ÃÖ±Ù ÆË¾÷
            
            if (popup.name.Contains("ShowSellectMonsterBackground"))
            {
                if (inventory != null)
                {
                    print("½ÇÇàµÊ"); 
                    inventory.AllCardbuttonStop();
                }
            }

            Destroy(popup);
            print(popupStack.Count);
        }
        //if (inventory.cardList != null)
        //{
        //    inventory.AllCardbuttonStop();
        //}
    }

    public int IsPopupOpen()
    {
        return popupStack.Count; // ÇöÀç ¿­¸° ÆË¾÷ÀÌ ÀÖ´ÂÁö È®ÀÎ
    }


  
}
