using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;
//using UnityEngine.UIElements;


public class UIInventory : MonoBehaviour
{
    //플레이어가 들고있는 몬스터정보 복사 
    public List<Monster> playerMonsterList = new List<Monster>();
    //선택된 몬스터가 임시로 저장되는 공간 
    public List<Monster> TempSelectedMonsterList = new List<Monster>();
    public List<Monster> currentSelectedMonsterList = new List<Monster>();

    // 인벤토리에 띄울 몬스터 전체 리스트
    public List<GameObject> textureMonsterPrefabsList;
    // 인벤토리에 띄울 몬스터 리스트
    public List<GameObject> texturePlayerMonsterList;

    public GameObject invetoryCameraPrefab;
    public Sprite[] element;
    public Sprite[] elementBackground;

    public RenderTexture[] renderTexture;
    public RenderTexture emptyRenderTexture;
    
    public int choiceNum = 0;
    //아이템 갯수 
    public int[] potionNum = new int[3] { 3, 3, 3 };


    public RectTransform monsterCardPos;

    private Player player;
    private InventoryPopUp inventoryPopUp;
    private InventoryButton inventoryButton;
    private MonsterDataManager monsterDataManager;
    
    public List<GameObject> cardList;

    public GameObject selectedMonster;

    public GameObject monsterCardPrefab;


    private Item item;
   
    private void Start()
    {
        inventoryPopUp = FindObjectOfType<InventoryPopUp>();
        inventoryButton = FindObjectOfType<InventoryButton>();
        player = FindObjectOfType<Player>();
        item = FindObjectOfType<Item>();
        monsterDataManager = FindObjectOfType<MonsterDataManager>();
        if (monsterDataManager == null)
        {
            Debug.Log("monsterDataManager가 없습니다");
        }
        cardList = new List<GameObject>(playerMonsterList.Count);
        
        Debug.Log($"Items Count: {item.items.Count}");

        LoadMonsterPrefabs();
        BringPlayerMonsterList();
        FindSameMonsters();
        InstantiatePlayerMonster();
    }

    private void OnEnable()
    {
       

        
    }

    private void Update()
    {
     
    }


    //GameObject-> Monster 변환 후 playerMonsterList에 저장 

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
                    //print("있다");
                    texturePlayerMonsterList.Add(monster);
                    break;
                }
            }
        }
        //print(texturePlayerMonsterList.Count);
    }

    //이미지를 띄울 몬스터
    public void InstantiatePlayerMonster()
    {
        //print("소환됨 2");
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


    //생성된 몬스터 카드 속성에 맞게 생성
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
        
        
        //TODO: 몬스터 정보 받아와야함
        cardList.Clear();
        for (int i = 0; i < playerMonsterList.Count; i++)
        {

            //카드 소환
            GameObject monsterCard = Instantiate(monsterCardPrefab, monsterCardPos);

            //소환된 카드 배경 바꾸기
            Image backgroundImage = monsterCard.GetComponent<Image>();
            Transform targetElementObject = monsterCard.transform.Find("RoleIcon/Icon");
            Image elementIconObject = targetElementObject.GetComponent<Image>();


            //소환된 카드 표현되는 이미지 바꾸기(몬스터 모양) 
            Transform targetTexture = monsterCard.transform.Find("MonsterCardButton");
            RawImage rawImage = targetTexture.GetComponent<RawImage>();
            rawImage.texture = renderTexture[i];
            if (monsterDataManager.allMonsterDataList[i].hp == 0)
            {
                print(monsterDataManager.allMonsterDataList[i].hp);
                rawImage.color = new Color(0.25f,0.25f,0.25f);
            }

            //소환된 카드에 이름 삽입하기
            Transform targetText = monsterCard.transform.Find("TextName");
            TMP_Text inputText = targetText.GetComponent<TMP_Text>();
            inputText.text = texturePlayerMonsterList[i].name;

            //체력 삽입
            targetText = monsterCard.transform.Find("Hp/Slider_Hp/Text");
            inputText = targetText.GetComponent<TMP_Text>();
            inputText.text = $"{monsterDataManager.allMonsterDataList[i].hp}/{monsterDataManager.allMonsterDataList[i].maxHp}";

            //체력바 입력 // 나중에 MonsterDataManager에서 정보를 받아야함.
            Slider Slider;
            Transform targetSlider;
            targetSlider = monsterCard.transform.Find("Hp/Slider_Hp");
            Slider = targetSlider.GetComponent<Slider>();
            Slider.value = monsterDataManager.allMonsterDataList[i].hpAmount;


            // 소환된 카드에 버튼 삽입하기
            targetButton = monsterCard.transform.Find("MonsterCardButton");
            button = targetButton.GetComponentInChildren<Button>();


            //체력바 추가

            int parameterValue = i;

            // UnityAction을 사용하여 버튼 클릭 이벤트 추가
            UnityAction action = () => inventoryButton.OnSelectCardButton(parameterValue);
            button.onClick.AddListener(action);        
            button.interactable = false;


            targetButton = monsterCard.transform.Find("InfoButton");
            button = targetButton.GetComponentInChildren<Button>();
            action = () => inventoryButton.OnShowMonsterInfoButton(parameterValue);
            button.onClick.AddListener(action);
            button.interactable =true;


            cardList.Add(monsterCard);
            
            print(cardList[i].name);
            switch (playerMonsterList[i].element)
            {
                case Element.Fire:
                    //print("진입함?3");
                    backgroundImage.sprite = elementBackground[0];
                    elementIconObject.sprite = element[0];
                    break;
                case Element.Water:
                    //print("진입함?4");
                    backgroundImage.sprite = elementBackground[1];
                    elementIconObject.sprite = element[1];
                    break;
                case Element.Grass:
                    //print("진입함?5");
                    backgroundImage.sprite = elementBackground[2];
                    elementIconObject.sprite = element[2];
                    break;

            }

         
        }
    }

    //배틀몬스터 표현
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

                //카드 소환
                GameObject monsterCard = Instantiate(monsterCardPrefab, monsterCardPos);

                //소환된 카드 배경 바꾸기
                Image backgroundImage = monsterCard.GetComponent<Image>();
                Transform targetElementObject = monsterCard.transform.Find("RoleIcon/Icon");
                Image elementIconObject = targetElementObject.GetComponent<Image>();

                //소환된 카드 표현되는 이미지 바꾸기(몬스터 모양) 
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
                //소환된 카드에 이름 삽입하기
                Transform targetText = monsterCard.transform.Find("TextName");
                TMP_Text inputText = targetText.GetComponent<TMP_Text>();
                inputText.text = TempSelectedMonsterList[i].name;

                //체력 삽입
               
                targetText = monsterCard.transform.Find("Hp/Slider_Hp/Text");
                inputText = targetText.GetComponent<TMP_Text>();
                inputText.text = $"{monsterDataManager.selectedMonsterDataList[i].hp}/{monsterDataManager.selectedMonsterDataList[i].maxHp}";
                
                //체력바 입력
                Slider Slider;
                Transform targetSlider;
                targetSlider = monsterCard.transform.Find("Hp/Slider_Hp");
                Slider = targetSlider.GetComponent<Slider>();
                Slider.value = monsterDataManager.selectedMonsterDataList[i].hpAmount;
                switch (TempSelectedMonsterList[i].element)
                {
                    case Element.Fire:
                        //print("진입함?3");
                        backgroundImage.sprite = elementBackground[0];
                        elementIconObject.sprite = element[0];
                        break;
                    case Element.Water:
                        //print("진입함?4");
                        backgroundImage.sprite = elementBackground[1];
                        elementIconObject.sprite = element[1];
                        break;
                    case Element.Grass:
                        //print("진입함?5");
                        backgroundImage.sprite = elementBackground[2];
                        elementIconObject.sprite = element[2];
                        break;

                }


            }
         
        }
        else
        {
            print("선택된 몬스터 없음");
        }
    }
    public void OnCardButtonInteractable()
    {
        
            Transform targetButton;
            Button button;
        for (int i = 0; i < cardList.Count; i++)
        {
            targetButton = cardList[i].transform.Find("MonsterCardButton");
            button = targetButton.GetComponent<Button>();
            if (monsterDataManager.allMonsterDataList[i].hp > 0)
            {
                button.interactable = true;
            }
            else
            {
                button.interactable = false;
            }
            
        }

    }
    //속성에 맞는 이미지 넣기, 버튼 활성비활성
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
        if (choiceNum < 3) // 최대 3마리 선택 가능
        {
            
            // 선택된 카드 비활성화
            targetButton = cardList[number].transform.Find("MonsterCardButton");
            button = targetButton.GetComponent<Button>();
            button.interactable = false; // 버튼 비활성화


            //선택된 카드 색상 변경
            
            colorBlock = button.colors;
            colorBlock.colorMultiplier = 1f;
            button.colors = colorBlock;

            rawImage = targetButton.GetComponent<RawImage>();
            image = cardList[number].GetComponent<Image>();

            // 선택된 몬스터 정보를 저장할 위치 결정
            targetTransform = selectedMonster.transform.Find($"SelectedMonster{choiceNum + 1}/SelectedMonsterBackGround{choiceNum + 1}");
            targetImage = targetTransform.GetComponent<Image>();
            targetImage.sprite = image.sprite; // 몬스터 이미지 저장

            targetTransform = selectedMonster.transform.Find($"SelectedMonster{choiceNum + 1}/ColectedCardTexture{choiceNum + 1}");
            targetRawImage = targetTransform.GetComponent<RawImage>();
            targetRawImage.texture = rawImage.texture; // 몬스터 텍스처 저장

            print($"{choiceNum + 1}번째 카드 선택"); // 선택한 카드 출력

            choiceNum++; // 선택한 카드 수 증가
            TempSelectedMonsterList.Add(playerMonsterList[number]);
            currentSelectedMonsterList.Add(playerMonsterList[number]);
           
          
            
        }
        else
        {
            print("배틀 몬스터는 최대 3마리 입니다.");
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


    //아이템 정보 불러오기
    public string UpdateItemInfo(int index)
    {
        ItemInfo itemInfo = item.GetItemInfo(index);
        string result = $"아이템 이름 : {itemInfo.itemName}\n";
        if (itemInfo.healingAmount > 0)
        {
            result += $"회복량 : {itemInfo.healingAmount}"; // 고정 회복량
        }
        else
        {
            result += $"회복 비율 : {itemInfo.healingPercentage * 100}%"; // 비율 회복량
        }
        return result;
    }

    public string UpdateMOnsterInfo(int index)
    {
        Monster selectedMonster = monsterDataManager.allMonsterDataList[index];
        print(potionNum[index]);
        string result = $"몬스터 이름 : {selectedMonster.name}\n";
        result += $"체력 : {selectedMonster.hp} / {selectedMonster.maxHp}\n";
        result += $"레벨 : {selectedMonster.level}\n";
        result += $"공격력 : {selectedMonster.damage}\n";
        result += $"보유량 : {potionNum[index]} 개";
        return result;
    }

}
