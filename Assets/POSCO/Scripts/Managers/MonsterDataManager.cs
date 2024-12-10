using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterDataManager : MonoBehaviour
{
    private static MonsterDataManager instance;
    public static MonsterDataManager Instance { get { return instance; } }

    //�÷��̾��� ��� ���� �����͸� ������ ����Ʈ
    public List<Monster> allMonsterDataList = new List<Monster>();
    //���õ� ���� ������ ����Ʈ
    public List<Monster> selectedMonsterDataList = new List<Monster>();

    private Player player;

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
        //�÷��̾� ����
        allMonsterDataList.Clear();
        selectedMonsterDataList.Clear();
        player = FindAnyObjectByType<Player>();
        BringPlayerAllMonsterData();

        //�÷��̾ 3������ ���͸� �����ϸ� �Լ��� ����ȴ�.
        player.onClickSelectButton += InitializeSelectedPlayerMonsterData;
    }

    //���� �÷��̾ ����ִ� ���� �������� ������ �� �ҷ��´� -> �� �Ŀ� ���� ����� �޾��ش� 
    private void BringPlayerAllMonsterData()
    {
        foreach(GameObject monsterObj in player.playerMonsterPrefabList)
        {
            if (monsterObj.TryGetComponent<Monster>(out Monster originalMonster))
            {
                //���� ������Ʈ�� �����Ѵ� -> �ֳ��ϸ� Monster�� MonoBehaviour�� ��� �޾Ƽ� ��ü�� �ʿ��ϴ�
                GameObject cloneObj = Instantiate(monsterObj);
                if (cloneObj.TryGetComponent<Monster>(out Monster cloneMonster))
                {
                    cloneMonster.name = originalMonster.name;
                    cloneMonster.hp = originalMonster.hp;
                    cloneMonster.maxHp = originalMonster.maxHp;
                    cloneMonster.level = originalMonster.level;
                    cloneMonster.currentExp = originalMonster.currentExp;
                    cloneMonster.expToNextLevelArr = originalMonster.expToNextLevelArr != null ? (int[])originalMonster.expToNextLevelArr.Clone() : null;
                    //cloneMonster.hpAmount = originalMonster.hpAmount;
                    cloneMonster.damage = originalMonster.damage;
                    cloneMonster.isEnemy = originalMonster.isEnemy;
                    //cloneMonster.skillDataArr = originalMonster.skillDataArr != null ? (SkillData[])originalMonster.skillDataArr.Clone() : null;
                    //cloneMonster.selectedSkill = originalMonster.skillDataArr[0];
                    //cloneMonster.selectedSkill.skillCount = originalMonster.skillDataArr[0].skillCount;
                    cloneMonster.animator = originalMonster.animator;
                    //cloneMonster.playParticleDuration = originalMonster.playParticleDuration;
                    cloneMonster.attackType = originalMonster.attackType;

                    if (originalMonster.skillDataArr != null && originalMonster.skillDataArr.Length > 0)
                    {
                        // SkillData �迭�� ���� �����Ͽ� ������ �����ϵ��� ����
                        cloneMonster.skillDataArr = originalMonster.skillDataArr;

                        // Skill ��ü�� ���� �����Ͽ� skillCount�� ���������� �����ǵ��� ����
                        cloneMonster.selectedSkill = new Skill(originalMonster.skillDataArr[0]);
                    }

                    allMonsterDataList.Add(cloneMonster);

                    //if (originalMonster.skillDataArr != null && originalMonster.skillDataArr.Length > 0)
                    //{
                    //    //cloneMonster.skillDataArr = (SkillData[])originalMonster.skillDataArr.Clone();
                    //    //cloneMonster.selectedSkill = new Skill(cloneMonster.skillDataArr[0]);
                    //    //cloneMonster.selectedSkill.skillCount = cloneMonster.skillDataArr[0].skillCount;
                    //    cloneMonster.skillDataArr = new SkillData[originalMonster.skillDataArr.Length];
                    //    for (int i = 0; i < originalMonster.skillDataArr.Length; i++)
                    //    {
                    //        // SkillData�� ���� ����
                    //        cloneMonster.skillDataArr[i] = new SkillData(originalMonster.skillDataArr[i]);
                    //    }
                    //    cloneMonster.selectedSkill = new Skill(cloneMonster.skillDataArr[0]);
                    //}

                    //allMonsterDataList.Add(cloneMonster);
                }
            }
        }
    }

    //select�ϴ� ��ư�� ������ �̺�Ʈ�� �߰����ָ� ���� �� -> �̰� allMonsterDataList���� �����;���
    //���߿� select��ư�� 3������ ü���� 0 �̻��϶� ���� �� �ְ� �� �ؾ��Ѵ�.
    public void InitializeSelectedPlayerMonsterData()
    {
        //���� ���õȰ� �ϴ� ����� -> �ٵ� �̰� ��ư�� ������ ��Ŀ��� ������.
        selectedMonsterDataList.Clear();

        //�÷��̾��� ���� ������ 3���� �Ǹ�
        if (player.selectedMonsterList.Count == 3)
        {
            //��� ���� �������� ����Ʈ���� 
            foreach(Monster selectedMonster in player.selectedMonsterList)
            {
                Monster matchedMonster = allMonsterDataList.Find(monster => monster.name == selectedMonster.name);
                if (matchedMonster != null)
                {
                    selectedMonsterDataList.Add(matchedMonster);
                }
            }
        }
    }

    //selectButton ���� -> �÷��̾ ���� ������ ���� ����Ʈ�� �״�� �޾ƿ� (�����͸�)
    //-> �������� �� �޾ƿ� ����Ʈ�� ������ ���� -> �׷��� �÷��̾� ������ ���� ü�µ� ����ִ� �����̰�, ���� ���õ� ����ü�µ�
    //-> ����ִ� �����̴�. -> ������ ������ �� �������� ������ 5�� �ִ� ������ �Ѱ��ش�. �� �׷��� 5������ �̾ƿ��� ������ �ؾ߰ڳ�
    //-> �Ѱ��ٶ� ���õ� ���� ����Ʈ�� �״�� ��������, ���� ü���� 0���� ������ ����Ʈ���� ���ִ� ������?
    //-> ��� �ʱ�ȭ ��ư ������ ����Ʈ�� �ʱ�ȭ �ؾ��ҵ�
    //-> �ƴ� �÷��̾��� ���õ� ������ ���ڰ� 3������ �׶� �ʱ�ȭ�� �����ص� �ɵ� ���� �̰� ������? �׷��� 3���� ���ϸ�
    //-> �ƿ� ������ �ȳѾ���°��� �׸��� ������ �÷��̾��� ���� ������ 3���� ���ϸ� �ȵȴٴ� ��� �߰� �׷� ��¥�� ������ �ȵ���
    //��� ������
}
