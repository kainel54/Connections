using System;
using UnityEngine;

public enum PartType
{
    // 노드를 연결할 때, 전에 있는 노드들 중 같은 카테고리가 있으면 그 노드를 비활성화 함
    // Default 는 카테고리를 체크하지 않는 것들 (중복 비활성화 X)
    Default,
    Trajectory,
}

public abstract class SkillPart : MonoBehaviour
{
    protected Skill _skill;

    protected virtual void Awake()
    {
        _skill = GetComponentInParent<Skill>();
    }

    // 기본 스텟들은 스킬 쪽에서 기본 값 바꿔주고 있음
    // 지뢰 같이 특수한 파츠들만 오버라이드해서 초기화하는 작업해주면 됨
    public virtual void InitSetting()
    {
        
    }
}
