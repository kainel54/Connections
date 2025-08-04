
using Unity.Behavior;

[BlackboardEnum]
public enum WerewolfBossEnum
{
    Intro, // 타임 라인 써먹자
    Phase1, // 기본 돌진 콤보 공격 진행, 투사체 발사, 도끼 투척(벽에 닿으면 튕김),
    PhaseChanging, // 몸 변화 일어남 타임 라인 써먹자 보름달 떠오르기 
    Phase2, // 지그제그 돌진 콤보 공격 진행, 도끼 투척 2개, 달리기, 분신 
    Phase3, // 이건 있을지 없을지 모른다잉
    Dead
}

