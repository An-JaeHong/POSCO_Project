using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    public List<Monster> allMonsterDataList = new List<Monster>();
    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {

            BringPlayerMonsterList();
        }
    }

    private void BringPlayerMonsterList()
    {
        foreach(GameObject monsterObj in player.playerMonsterPrefabList)
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster monster))
            {
                allMonsterDataList.Add(monster);
            }
            print($"{allMonsterDataList[0]}");
        }
    }
}
