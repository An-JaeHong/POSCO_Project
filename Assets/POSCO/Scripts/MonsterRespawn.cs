using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MonsterRespawn : MonoBehaviour
{
    public List<Vector3> respawnPos;

    public List<GameObject> prefabToSpawn; // ��ȯ�� ������
    public Vector3 spawnPosition; // ��ȯ�� ��ġ
    
    private List<GameObject> spawnMonsterList;
    private int maxMonsterCount= 12;

    public int monsterCount = 0;

    private void Awake()
    {
        respawnPos = new List<Vector3>();
        

        foreach (Transform pos in transform)
        {
            respawnPos.Add(pos.position);
        }
    }
    private void Start()
    {
        
        spawnMonsterList = new List<GameObject>(new GameObject[maxMonsterCount]);

        

        //�ʱ����
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
                
            }
            else { print("�̹� ���� ����"); }
        }
    }

    private void SpawnPrefab(GameObject monsterPrefab,Vector3 position, int number)
    {
         if (monsterCount < 8)
         {
            // �������� Ư�� ��ġ�� ��ȯ
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

    
}
