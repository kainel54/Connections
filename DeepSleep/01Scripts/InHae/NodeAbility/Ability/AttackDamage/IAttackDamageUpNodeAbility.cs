using UnityEngine;

public interface IAttackDamageUpNodeAbility
{
    public void AttackDamageUp(int selfIdx, int targetIdx, float value);
    public void AttackDamageDown(int selfIdx, int targetIdx, float value);
}
