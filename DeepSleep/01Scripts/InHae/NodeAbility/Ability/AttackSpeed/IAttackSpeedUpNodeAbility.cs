using UnityEngine;

public interface IAttackSpeedUpNodeAbility
{
    public void AttackSpeedUp(int selfIdx, int targetIdx, float value);
    public void AttackSpeedDown(int selfIdx, int targetIdx, float value);
}
