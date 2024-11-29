using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Command Pattern을 쓸 예정
public interface ICommand
{
    //실행시킬 함수
    public void Execute();

    //뒤로 돌릴 함수인데, 쓸 이유는 없을 것 같지만 혹시 모르니 작성
    public void Undo();
}
