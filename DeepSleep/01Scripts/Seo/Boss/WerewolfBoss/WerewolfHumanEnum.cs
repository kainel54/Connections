using Unity.Behavior;
[BlackboardEnum]
public enum WerewolfHumanEnum
{
    BasicAttack,
    SliceTwice, //두번 가르기 첫번쨰 짧게 두번째 매우 긴범위 공격
    Move,  // Move To Target
    RunAway,  // 공격을 받으려면 뒤로 빼기 그 마법사 기능 갖고 오기
    Pattern1,//이속 저하 먹이고 오른쪽으로 가면서 간보다각 도끼 투척 (바닥에 박히기 ) 도끼로 돌진 후 도끼 획득 -> 회전 배기
}


//TODO_SE_Make the pattern