using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{

    //��ư ����
   private UIInventory uiInventory;
   private InventoryPopUp inventoryPopUp;


    private void Awake()
    {
        
    }
    private void Start()
    {
        uiInventory = FindObjectOfType<UIInventory>(); // �� ���� UIInventory�� ã��
        inventoryPopUp = FindObjectOfType<InventoryPopUp>(); // �� ���� InventoryPopUp�� ã��
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
