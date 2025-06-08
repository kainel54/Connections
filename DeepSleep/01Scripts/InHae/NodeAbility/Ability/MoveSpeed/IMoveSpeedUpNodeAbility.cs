using UnityEngine;

public interface IMoveSpeedUpNodeAbility
{
    public void MoveSpeedUp(int selfIdx, int targetIdx, float value);
    public void MoveSpeedDown(int selfIdx, int targetIdx, float value);
}
