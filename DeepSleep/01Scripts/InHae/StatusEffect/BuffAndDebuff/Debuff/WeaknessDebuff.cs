using UnityEngine;
using YH.Entities;

public class WeaknessDebuff : BuffAndDebuffStat
{
    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        // 받는 데미지 1.2배
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
        // 받는 데미지 1.2배 제거
    }
}
