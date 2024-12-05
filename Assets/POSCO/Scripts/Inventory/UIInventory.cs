using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class UIInventory : MonoBehaviour
{
    //플레이어가 들고있는 몬스터정보 복사 
    public List<Monster> playerMonsterList = new List<Monster>();
    //선택된 몬스터가 임시로 저장되는 공간 
    public List<Monster> TempSelectedMonsterList = new List<Monster>();


    // 인벤토리에 띄울 몬스터 전체 리스트
    public List<GameObject> textureMonsterPrefabsList;
    // 인벤토리에 띄울 몬스터 리스트
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
        inventoryPopUp = FindObjectOfType<InventoryPopUp>();
        cardList = new List<GameObject>(playerMonsterList.Count);
        inventoryButton = FindObjectOfType<InventoryButton>();
        LoadMonsterPrefabs();
        player = FindObjectOfType<Player>();
        BringPlayerMonsterList();
        FindSameMonsters();
        InstantiatePlayerMonster();
    }

    private void Update()
    {
        
    }

    //GameObject-> Monster 변환 후 playerMonster   List에 저장 

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

            //소환된 카드에 이름 삽입하기
            Transform targetText = monsterCard.transform.Find("TextName");
            TMP_Text inputText = targetText.GetComponent<TMP_Text>();
            inputText.text = texturePlayerMonsterList[i].name;

            //소환된 카드에 버튼 삽입하기

            // 소환된 카드에 버튼 삽입하기
            targetButton = monsterCard.transform.Find("MonsterCardButton");
            button = targetButton.GetComponentInChildren<Button>();

            int parameterValue = i;

            // UnityAction을 사용하여 버튼 클릭 이벤트 추가
            UnityAction action = () => inventoryButton.OnSelectCardButton(parameterValue);
            button.onClick.AddListener(action);
            button.interactable = false;

            cardList.Add(monsterCard);

            switch (playerMonsterList[i].element)
            {
                case Element.Fire:
                    print("진입함?3");
                    backgroundImage.sprite = elementBackground[0];
                    elementIconObject.sprite = element[0];
                    break;
                case Element.Water:
                    print("진입함?4");
                    backgroundImage.sprite = elementBackground[1];
                    elementIconObject.sprite = element[1];
                    break;
                case Element.Grass:
                    print("진입함?5");
                    backgroundImage.sprite = elementBackground[2];
                    elementIconObject.sprite = element[2];
                    break;

            }
          
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
    }

    public void ResetCelectedMonster()
    {
        Transform targetButton;
        UIInventoryManager.Instance.ClosePopup();
        foreach(var cardListButton in cardList)
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











