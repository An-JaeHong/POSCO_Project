using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventory : MonoBehaviour
{
    //플레이어가 들고있는 몬스터정보 복사 (확인을 위해 public으로 설정해둠 오류가 없음이 확인되면 private변경)
    public List<Monster> playerMonsterList = new List<Monster>();
    //선택된 몬스터가 임시로 저장되는 공간 (확인을 위해 public으로 설정해둠 오류가 없음이 확인되면 private변경)
    public List<Monster> TempSelectedMonsterList = new List<Monster>();

    // 인벤토리에 띄울 몬스터 전체 리스트(public ->private)
    public List<GameObject> textureMonsterPrefabsList;
    // 인벤토리에 띄울 몬스터 리스트(public ->private)
    public List<GameObject> texturePlayerMonsterList; 

    

    public GameObject MonsterCardPrefab;

    private Player player;

    private void Start()
    {

        //player의 몬스터 정보 가지고오기
        BringPlaterMonsterList();
    }

    private void Update()
    {
        
    }

    //GameObject-> Monster 변환 후 playerMonsterList에 저장 

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







}


