using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
    private InventoryButton inventoryButton;
    public List<GameObject> cardList;

    public GameObject selectedMonster;

    public GameObject monsterCardPrefab;

    private void Start()
    {
     
    }

    private void OnEnable()
    {
        cardList = new List<GameObject>(playerMonsterList.Count);
        inventoryButton = FindObjectOfType<InventoryButton>();
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
    public void SetSelectButton(GameObject monsterCardBackgroundPrefab)
    {
        
    }


    //������ ���� ī�� �Ӽ��� �°� ����
    public void InstantiateMonsterCard(GameObject monsterCardBackgroundPrefab)
    {
        inventoryButton = FindObjectOfType<InventoryButton>();
        Transform target = monsterCardBackgroundPrefab.transform;
        target = monsterCardBackgroundPrefab.transform.Find("MonsterCardGirdLayoutGroup");
        monsterCardPos = target.GetComponent<RectTransform>();

        Transform targetButton = monsterCardBackgroundPrefab.transform.Find("ChoiceBattleMonster");
        Button button = targetButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() =>
        {

            inventoryButton.OnChioseBattleMonsterButton();

        });

        //UnityAction action1 = () => inventoryButton.OnChioseBattleMonsterButton();
        //button.onClick.AddListener(action1);

        print(monsterCardPos.transform);

        for (int i = 0; i < playerMonsterList.Count; i++)
        {
            //ī�� ��ȯ
            GameObject monsterCard = Instantiate(monsterCardPrefab, monsterCardPos);

            //��ȯ�� ī�� ��� �ٲٱ�
            Image backgroundImage = monsterCard.GetComponent<Image>();
            Transform targetElementObject = monsterCard.transform.Find("RoleIcon/Icon");
            Image elementIconObject = targetElementObject.GetComponent<Image>();

            //��ȯ�� ī�� ǥ���Ǵ� �̹��� �ٲٱ�(���� ���) 
            Transform targetTexture = monsterCard.transform.Find("MonsterCardButton");
            RawImage rawImage = targetTexture.GetComponent<RawImage>();
            rawImage.texture = renderTexture[i];

            //��ȯ�� ī�忡 �̸� �����ϱ�
            Transform targetText = monsterCard.transform.Find("TextName");
            TMP_Text inputText = targetText.GetComponent<TMP_Text>();
            inputText.text = texturePlayerMonsterList[i].name;

            //��ȯ�� ī�忡 ��ư �����ϱ�

            // ��ȯ�� ī�忡 ��ư �����ϱ�
            targetButton = monsterCard.transform.Find("MonsterCardButton");
            button = targetButton.GetComponentInChildren<Button>();

            int parameterValue = i;

            // UnityAction�� ����Ͽ� ��ư Ŭ�� �̺�Ʈ �߰�
            UnityAction action = () => inventoryButton.OnSelectCardButton(parameterValue);
            button.onClick.AddListener(action);
            button.interactable = false;

            cardList.Add(monsterCard);

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
    }

    public void OnCardListInteractable()
    {
        Transform targetButton;
        foreach (var card in cardList)
        {
            targetButton = card.transform.Find("MonsterCardButton"); ;
            Button button = targetButton.GetComponent<Button>();
            button.interactable = true;
        }
    }
    //������ �������־�

    public void OnMonsterCard(int number)
    {
        Transform targetButton;
        Button button;

        RawImage targetRawImage;
        RawImage rawImage;

        if (choiceNum < 3)
        {
            switch (number)
            {
                case 0:
                    targetButton = cardList[0].transform.Find("MonsterCardButton"); ;
                    button = targetButton.GetComponent<Button>();
                    button.interactable = false;

                    rawImage = cardList[0].GetComponent<RawImage>();
                    print(selectedMonster.name);
                    targetRawImage = selectedMonster.GetComponent<RawImage>();
                                
                    //TempSelectedMonsterList.Add(playerMonsterList[0]);
                                          
                        
                    print("ù��° ī�弱��");
                    break;
                //case 1:
                //    TempSelectedMonsterList.Add(playerMonsterList[0]);
                //    monsterCardNum[0].interactable = false;
                //    targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
                //    print("�ι�° ī�弱��");
                //    break;
                //case 2:
                //    TempSelectedMonsterList.Add(playerMonsterList[0]);
                //    monsterCardNum[0].interactable = false;
                //    targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
                //    print("3��° ī�� ����");

                //    break;
                //case 3:
                //    TempSelectedMonsterList.Add(playerMonsterList[0]);
                //    monsterCardNum[0].interactable = false;
                //    targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
                //    print("4��° ī�� ����");

                //    break;
                //case 4:
                //    TempSelectedMonsterList.Add(playerMonsterList[0]);
                //    monsterCardNum[0].interactable = false;
                //    targetRawImage = monsterCardNum[0].GetComponentInChildren<RawImage>();
                //    print("5��° ī�� ����");

                //    break;
            }

            choiceNum++;

        }
        else
        {
            print("��Ʋ ���ʹ��ִ� 3���� �Դϴ�.");
        }

        //ShowSetCelectMonster(choiceNum - 1);
        //}
    }
}











