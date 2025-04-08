using System;

[Serializable]
public abstract class PartNode
{
    public PartType partType;
    public abstract void EquipPart(Skill skill);
    public abstract void UnEquipPart(Skill skill);

    public virtual void InitPart(Skill skill)
    {
        
    }
}
