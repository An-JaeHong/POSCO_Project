using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{

    //버튼 세팅
   private UIInventory uiInventory;
   private InventoryPopUp inventoryPopUp;


    private void Awake()
    {
        
    }
    private void Start()
    {
        uiInventory = FindObjectOfType<UIInventory>(); // 씬 내의 UIInventory을 찾음
        inventoryPopUp = FindObjectOfType<InventoryPopUp>(); // 씬 내의 InventoryPopUp을 찾음
    }

    public void OnShowMonster()
    {
        inventoryPopUp.InstantiateShowMonster();
    }

    public void OnShowSelectedMonster()
    {
        inventoryPopUp.InstantiateSelectedMonster();
    }

    public void OnShowItem()
    {
        inventoryPopUp.InstantiateShowItem();
    }


    public void OnChioseBattleMonsterButton()
    {
        uiInventory.OnCardListInteractable();
        inventoryPopUp.InstantiateSelectedMonster();
    }

    public void OnSelectCardButton(int num)
    {
        uiInventory.OnMonsterCard(num);
    }


    //private void ShowSetCelectMonster(int num)
    //{
    //    targetGameObject = ShowColectedMonster[num];
    //    rawImage = targetGameObject.GetComponent<RawImage>();
    //    rawImage.texture = targetRawImage.texture;
    //}

  

}
