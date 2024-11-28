using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Player : MonoBehaviour
{
    //�÷��̾ ���۽� ������ �ִ� ����
    public List<GameObject> playerMonsterPrefabList = new List<GameObject>();
    //�÷��̾ ������ ���� ������ ����
    public List<Monster> selectedMonsterList = new List<Monster>();

    public bool canMove = true; //true : ������ �� ����, false : ������ �� ����

    //���� ����
    public PlayerStateBase currentState;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.U))
        {
            //nt($"{selectedMonsters[0].name}");
            if (selectedMonsterList.Count == 0)
            {
                print("�������͸� �����ϼ���");
            }
            else
            {
                print("1");
            }
        }
    }

    //�÷��̾��� ���¸� �ٲٴ� �Լ�
    public void ChangeState(PlayerStateBase newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public List<GameObject> GetMonsterPrefabList()
    {
        return playerMonsterPrefabList;
    }

    public List<Monster> GetSeletedMonsterList()
    {
        return selectedMonsterList;
    }

    public void SetSelectedMonsters(List<Monster> selecedMonater)
    {
        //List<Monster> temp = new List<Monster>();
        //temp = selecedMonater;
        this.selectedMonsterList = selecedMonater;
        print(selectedMonsterList[1].name);
    }

    //�����ϴ� ����� ������ ����
    public void OnCollisionEnter(Collision collision)
    {
        //���� ���·� ����
        currentState.HandleCollision(collision);

    }
}
