using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : MonoBehaviour
{
    //�÷��̾ ����ִ� �������� ���� 
    public List<Monster> playerMonsterList = new List<Monster>();
    //���õ� ���Ͱ� �ӽ÷� ����Ǵ� ���� 
    public List<Monster> TempSelectedMonsterList = new List<Monster>();


    // �κ��丮�� ��� ���� ��ü ����Ʈ
    public List<GameObject> textureMonsterPrefabsList;
    // �κ��丮�� ��� ���� ����Ʈ
    public List<GameObject> texturePlayerMonsterList;

    public GameObject invetoryCameraPrefab;
    public Sprite[] element;
    public Sprite[] elementBackground;

    public RenderTexture[] renderTexture;
    public RenderTexture emptyRenderTexture;
    
    private int choiceNum = 0;

    public RectTransform monsterCardPos;

    private Player player;
    private InventoryPopUp inventoryPopUp;

    public GameObject monsterCardPrefab;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        LoadMonsterPrefabs();
        player = FindObjectOfType<Player>();
        BringPlaterMonsterList();
        FindSameMonsters();
        InstantiatePlayerMonster();
    }

    private void Update()
    {
        
    }

    //GameObject-> Monster ��ȯ �� playerMonster   List�� ���� 

    private void BringPlaterMonsterList()
    {
     
        foreach (GameObject monsterObj in player.playerMonsterPrefabList)
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster monster))
            {
                playerMonsterList.Add(monster);
            }
        }
    }
    private void LoadMonsterPrefabs()
    {
        textureMonsterPrefabsList = new List<GameObject>(Resources.LoadAll<GameObject>("TextureRenderer"));

    }

    public void FindSameMonsters()
    {
       
   
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
              
            foreach (var monster in textureMonsterPrefabsList)
            {
                   
                if (monster.name == playerMonsterList[i].name)
                {
                    //print("�ִ�");
                    texturePlayerMonsterList.Add(monster);
                    break;
                }
            }
        }
        //print(texturePlayerMonsterList.Count);
    }

    public void InstantiatePlayerMonster()
    {
        //print("��ȯ�� 2");
        UIMonster newTextureMonster;
        GameObject newMonsterCamera;
        //print(playerMonsterList.Count);
        //print(player.playerMonsterPrefabList.Count);
        //print(texturePlayerMonsterList.Count);
      
       
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            float posNum = i * 10f;
      
            newTextureMonster = Instantiate(texturePlayerMonsterList[i]).GetComponent<UIMonster>();
            newTextureMonster.transform.position = new Vector3(20 - posNum, 0, 0);
            newMonsterCamera = Instantiate(invetoryCameraPrefab);
            newMonsterCamera.transform.position = new Vector3(20 - posNum, 2, 4);
            newMonsterCamera.transform.rotation = Quaternion.Euler(15, 180, 0);
            Camera camera = newMonsterCamera.GetComponent<Camera>();
            camera.targetTexture = renderTexture[i];




        }

    }
    //������ ���� ī�� �Ӽ��� �°� ����
    public void InstantiateMonsterCard(GameObject monsterCardBackgroundPrefab)
    {
        Transform target = monsterCardBackgroundPrefab.transform;
        target = monsterCardBackgroundPrefab.transform.Find("MonsterCardGirdLayoutGroup");
        monsterCardPos = target.GetComponent<RectTransform>();

        print(monsterCardPos.transform);
            
        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            GameObject monstercard = Instantiate(monsterCardPrefab, monsterCardPos);
            Image backgroundImage = monstercard.GetComponent<Image>();
            Transform targetElementObject = monstercard.transform.Find("RoleIcon/Icon");
            Image elementIconObject = targetElementObject.GetComponent<Image>();
            Transform targetTexture = monstercard.transform.Find("MonsterCardButton");
            RawImage rawImage = targetTexture.GetComponent<RawImage>();
            rawImage.texture = renderTexture[i];
            Transform targetText = monstercard.transform.Find("TextName");
            TMP_Text inputText = targetText.GetComponent<TMP_Text>();
            print (texturePlayerMonsterList[i].name);
            inputText.text = texturePlayerMonsterList[i].name;

            switch (playerMonsterList[i].element)
            {
                case Element.Fire:
        print("������?3");
                    backgroundImage.sprite = elementBackground[0];
                    elementIconObject.sprite = element[0];
                    break;
                case Element.Water:
                    print("������?4");
                    backgroundImage.sprite = elementBackground[1];
                    elementIconObject.sprite = element[1];
                    break;
                case Element.Grass:
                    print("������?5");
                    backgroundImage.sprite = elementBackground[2];
                    elementIconObject.sprite = element[2];
                    break;
            
            }

        }



        //������ �������־�


    }
    //    public void OnMonsterCard(int number)
    //{
    //    print($"{playerMonsterList[0]}");
    //    if (choiceNum < 3)
    //    {
    //        switch (number)
    //        {
    //            case 0:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum.GetComponentInChildren<RawImage>();
    //                print("ù��° ī�弱��");
    //                break;
    //            case 1:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("�ι�° ī�弱��");
    //                break;
    //            case 2:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("3��° ī�� ����");

    //                break;
    //            case 3:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("4��° ī�� ����");

    //                break;
    //            case 4:
    //                TempSelectedMonsterList.Add(playerMonsterList[0]);
    //                monsterCardNum[0].interactable = false;
    //                targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
    //                print("5��° ī�� ����");

    //                break;
    //        }

    //        choiceNum++;

    //    }
    //    else
    //    {
    //        print("��Ʋ ���ʹ��ִ� 3���� �Դϴ�.");
    //    }

    //    ShowSetCelectMonster(choiceNum - 1);
    //    //}
    //}







}


