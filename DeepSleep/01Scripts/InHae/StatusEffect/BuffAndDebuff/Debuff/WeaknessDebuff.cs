using UnityEngine;
using YH.Entities;

public class WeaknessDebuff : BuffAndDebuffStat
{
    public override void StatusInit(Entity entity)
    {
        base.StatusInit(entity);
        // �޴� ������ 1.2��
    }

    public override void RemoveStatus()
    {
        base.RemoveStatus();
        // �޴� ������ 1.2�� ����
    }
}
