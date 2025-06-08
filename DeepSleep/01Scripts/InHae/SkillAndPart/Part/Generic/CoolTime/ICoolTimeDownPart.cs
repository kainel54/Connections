public interface ICoolTimeDownPart
{
    public enum ModifyType
    {
        Add = 0,
        Percent
    }

    public void DeCreaseCoolTime(float time, ModifyType modifyType);
    public void InCreaseCoolTime(float time, ModifyType modifyType);
}
