using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryButton : MonoBehaviour
{

    //버튼 세팅
    UIInventory uiInventory;
    private InventoryPopUp inventoryPopUpManager;

    public void OnShowMonster()
    {
        inventoryPopUpManager.InstantiateShowMonster();
        
    }

    public void OnShowSelectedMonster()
    {
        inventoryPopUpManager.InstantiateSelectedMonster();
    }

    public void OnShowItem()
    {
        inventoryPopUpManager.InstantiateShowItem();
    }


    public void OnCardButton()
    { 
    
    }



    //private void ShowSetCelectMonster(int num)
    //{
    //    targetGameObject = ShowColectedMonster[num];
    //    rawImage = targetGameObject.GetComponent<RawImage>();
    //    rawImage.texture = targetRawImage.texture;
    //}

  

}
