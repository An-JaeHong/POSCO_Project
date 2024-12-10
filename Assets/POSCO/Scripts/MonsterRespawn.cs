using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterRespawn : MonoBehaviour
{
    private static MonsterRespawn instance;
    public static MonsterRespawn Instance { get { return instance; } }


    public List<Vector3> respawnPos;

    public List<GameObject> prefabToSpawn; // 소환할 프리팹
    public Vector3 spawnPosition; // 소환할 위치
    
    private List<GameObject> spawnMonsterList;
    private int maxMonsterCount= 12;

    public int monsterCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        respawnPos = new List<Vector3>();


        foreach (Transform pos in transform)
        {
            respawnPos.Add(pos.position);
        }

        spawnMonsterList = new List<GameObject>(new GameObject[maxMonsterCount]);

        //초기몬스터
        for (int i = 0; i < maxMonsterCount; i++)
        {
            int random =Random.Range(0, 18);
            
            SpawnPrefab(prefabToSpawn[random], respawnPos[i % respawnPos.Count], i % respawnPos.Count);
            print(i);
        }


    }

    private void Update()
    {
        if (monsterCount < maxMonsterCount)
        {   

            int random = Random.Range(0, 12);
            int random2 = Random.Range(0, 18);
            if (spawnMonsterList[random] == null)
            {
                SpawnPrefab(prefabToSpawn[random2], respawnPos[random], random);
                print("생성됨");
            }
            else { print("이미 몬스터 있음"); }
        }
    }

    private void SpawnPrefab(GameObject monsterPrefab,Vector3 position, int number)
    {
        
            // 프리팹을 특정 위치에 소환
            GameObject monster = Instantiate(monsterPrefab, position, Quaternion.identity);
            monsterCount++;
            
            spawnMonsterList[number] = monster;
            //Unit unitScript = monster.GetComponent<Unit>();
            //if (unitScript != null)
            //{
            //    unitScript.
            //}
         
       
    }

    
}
