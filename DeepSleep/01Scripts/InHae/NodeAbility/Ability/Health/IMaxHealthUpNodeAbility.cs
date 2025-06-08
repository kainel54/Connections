using UnityEngine;

public interface IMaxHealthUpNodeAbility
{
    public void MaxHealthUp(int selfIdx, int targetIdx, float value);
    public void MaxHealthDown(int selfIdx, int targetIdx, float value);
}
