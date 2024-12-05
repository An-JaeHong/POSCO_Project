using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
//using UnityEngine.UIElements;


public class UIInventory : MonoBehaviour
{
    //�÷��̾ ����ִ� �������� ���� 
    public List<Monster> playerMonsterList = new List<Monster>();
    //���õ� ���Ͱ� �ӽ÷� ����Ǵ� ���� 
    public List<Monster> TempSelectedMonsterList = new List<Monster>();
    public List<Monster> currentSelectedMonsterList = new List<Monster>();

    // �κ��丮�� ��� ���� ��ü ����Ʈ
    public List<GameObject> textureMonsterPrefabsList;
    // �κ��丮�� ��� ���� ����Ʈ
    public List<GameObject> texturePlayerMonsterList;

    public GameObject invetoryCameraPrefab;
    public Sprite[] element;
    public Sprite[] elementBackground;

    public RenderTexture[] renderTexture;
    public RenderTexture emptyRenderTexture;
    
    public int choiceNum = 0;

    public RectTransform monsterCardPos;

    private Player player;
    private InventoryPopUp inventoryPopUp;
    private InventoryButton inventoryButton;
    public List<GameObject> cardList;

    public GameObject selectedMonster;

    public GameObject monsterCardPrefab;

    //private List<GameObject> selectedCard;
    private List<GameObject> battleCard;

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        inventoryPopUp = FindObjectOfType<InventoryPopUp>();
        inventoryButton = FindObjectOfType<InventoryButton>();
        player = FindObjectOfType<Player>();
        
        cardList = new List<GameObject>(playerMonsterList.Count);
        battleCard = new List<GameObject>(3);

        LoadMonsterPrefabs();
        BringPlayerMonsterList();
        FindSameMonsters();
        InstantiatePlayerMonster();

        
    }

    private void Update()
    {
      
    }


    //GameObject-> Monster ��ȯ �� playerMonsterList�� ���� 

    private void BringPlayerMonsterList()
    {
        print(player.playerMonsterPrefabList[1]);
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

    //�̹����� ��� ����
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
       
        Transform target = monsterCardBackgroundPrefab.transform;
        target = monsterCardBackgroundPrefab.transform.Find("MonsterCardGirdLayoutGroup");
        monsterCardPos = target.GetComponent<RectTransform>();

        Transform targetButton = monsterCardBackgroundPrefab.transform.Find("ChoiceBattleMonster");
        Button button = targetButton.GetComponentInChildren<Button>();
        button.onClick.AddListener(() => inventoryButton.OnChioseBattleMonsterButton());

     

        //UnityAction action1 = () => inventoryButton.OnChioseBattleMonsterButton();
        //button.onClick.AddListener(action1);

        //print(monsterCardPos.transform);
        
        cardList.Clear();
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
            //ü�� ����
            targetText = monsterCard.transform.Find("Hp/Slider_Hp/Text");
            inputText = targetText.GetComponent<TMP_Text>();
            inputText.text = $"{playerMonsterList[i].hp}/{playerMonsterList[i].maxHp}";
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
            
            print(cardList[i].name);
            switch (playerMonsterList[i].element)
            {
                case Element.Fire:
                    //print("������?3");
                    backgroundImage.sprite = elementBackground[0];
                    elementIconObject.sprite = element[0];
                    break;
                case Element.Water:
                    //print("������?4");
                    backgroundImage.sprite = elementBackground[1];
                    elementIconObject.sprite = element[1];
                    break;
                case Element.Grass:
                    //print("������?5");
                    backgroundImage.sprite = elementBackground[2];
                    elementIconObject.sprite = element[2];
                    break;

            }

         
        }
    }

    //��Ʋ���� ǥ��
    public void InstantiateMyBattleMonster(RectTransform MyBattleMonsterPrefab)
    {
        if (currentSelectedMonsterList.Count == 3)
        {
            Transform target = MyBattleMonsterPrefab.transform;
            target = MyBattleMonsterPrefab.transform.Find("MonsterCardGirdLayoutGroup");
            monsterCardPos = target.GetComponent<RectTransform>();

            Transform targetButton = MyBattleMonsterPrefab.transform.Find("ChoiceBattleMonster");

        
            for (int i = 0; i < 3; i++)
            {

                //ī�� ��ȯ
                GameObject monsterCard = Instantiate(monsterCardPrefab, monsterCardPos);

                //��ȯ�� ī�� ��� �ٲٱ�
                Image backgroundImage = monsterCard.GetComponent<Image>();
                Transform targetElementObject = monsterCard.transform.Find("RoleIcon/Icon");
                Image elementIconObject = targetElementObject.GetComponent<Image>();

                //��ȯ�� ī�� ǥ���Ǵ� �̹��� �ٲٱ�(���� ���) 
                for (int k = 0; k < 5; k++)
                {
                    if (playerMonsterList[k].name == TempSelectedMonsterList[i].name)
                    {
                        Transform targetTexture = monsterCard.transform.Find("MonsterCardButton");
                        RawImage rawImage = targetTexture.GetComponent<RawImage>();
                        rawImage.texture = renderTexture[k];
                        break;
                    }
                }
                //��ȯ�� ī�忡 �̸� �����ϱ�
                Transform targetText = monsterCard.transform.Find("TextName");
                TMP_Text inputText = targetText.GetComponent<TMP_Text>();
                inputText.text = TempSelectedMonsterList[i].name;
                //ü�� ����
                targetText = monsterCard.transform.Find("Hp/Slider_Hp/Text");
                inputText = targetText.GetComponent<TMP_Text>();
                inputText.text = $"{TempSelectedMonsterList[i].hp}/{TempSelectedMonsterList[i].maxHp}";

                switch (TempSelectedMonsterList[i].element)
                {
                    case Element.Fire:
                        //print("������?3");
                        backgroundImage.sprite = elementBackground[0];
                        elementIconObject.sprite = element[0];
                        break;
                    case Element.Water:
                        //print("������?4");
                        backgroundImage.sprite = elementBackground[1];
                        elementIconObject.sprite = element[1];
                        break;
                    case Element.Grass:
                        //print("������?5");
                        backgroundImage.sprite = elementBackground[2];
                        elementIconObject.sprite = element[2];
                        break;

                }


            }
         
        }
        else
        {
            print("���õ� ���� ����");
        }
    }
    public void OnCardButtonInteractable()
    {
        
            Transform targetButton;
            foreach (var card in cardList)
            {
           
                targetButton = card.transform.Find("MonsterCardButton");
                Button button = targetButton.GetComponent<Button>();
                button.interactable = true;
            }
        
    }
    //�Ӽ��� �´� �̹��� �ֱ�, ��ư Ȱ����Ȱ��
    public void OnMonsterCard(int number)
    {
        
        Transform targetButton;
        Button button;
        Transform targetTransform;
        RawImage targetRawImage;
        RawImage rawImage;
        Image image;
        Image targetImage;
        ColorBlock colorBlock;
        print(cardList[1]);
        if (choiceNum < 3) // �ִ� 3���� ���� ����
        {
            
            // ���õ� ī�� ��Ȱ��ȭ
            targetButton = cardList[number].transform.Find("MonsterCardButton");
            button = targetButton.GetComponent<Button>();
            button.interactable = false; // ��ư ��Ȱ��ȭ


            //���õ� ī�� ���� ����
            
            colorBlock = button.colors;
            colorBlock.colorMultiplier = 1f;
            button.colors = colorBlock;

            rawImage = targetButton.GetComponent<RawImage>();
            image = cardList[number].GetComponent<Image>();

            // ���õ� ���� ������ ������ ��ġ ����
            targetTransform = selectedMonster.transform.Find($"SelectedMonster{choiceNum + 1}/SelectedMonsterBackGround{choiceNum + 1}");
            targetImage = targetTransform.GetComponent<Image>();
            targetImage.sprite = image.sprite; // ���� �̹��� ����

            targetTransform = selectedMonster.transform.Find($"SelectedMonster{choiceNum + 1}/ColectedCardTexture{choiceNum + 1}");
            targetRawImage = targetTransform.GetComponent<RawImage>();
            targetRawImage.texture = rawImage.texture; // ���� �ؽ�ó ����

            print($"{choiceNum + 1}��° ī�� ����"); // ������ ī�� ���

            choiceNum++; // ������ ī�� �� ����
            TempSelectedMonsterList.Add(playerMonsterList[number]);
            currentSelectedMonsterList.Add(playerMonsterList[number]);
            GameObject copiedCard = Instantiate(cardList[number]);
            battleCard.Add(copiedCard);
            
        }
        else
        {
            print("��Ʋ ���ʹ� �ִ� 3���� �Դϴ�.");
        }
    }

    public void SetSelectMonster()
    {
        if (choiceNum == 3)
        {
            player.SetSelectedMonsters(TempSelectedMonsterList);
        }
        Transform targetButton;
        Button button;
        ColorBlock colorBlock;
        foreach (var cardListButton in cardList)
        {
            targetButton = cardListButton.transform.Find("MonsterCardButton");
            button = targetButton.GetComponent<Button>();
            button.interactable = false;
            colorBlock = button.colors;
            colorBlock.colorMultiplier = 5f;
            button.colors = colorBlock;
        }
        
       

    }
    

    public void ResetCelectedMonster()
    {
        Transform targetButton;
        UIInventoryManager.Instance.ClosePopup();
        //cardList.Clear();
        foreach (var cardListButton in cardList)
        {
            targetButton = cardListButton.transform.Find("MonsterCardButton");
            Button button = targetButton.GetComponent<Button>();
            button.interactable = true;
        }
        
        inventoryPopUp.InstantiateSelectedMonster();
        TempSelectedMonsterList.Clear();
     
        choiceNum = 0;
    }




   

}
