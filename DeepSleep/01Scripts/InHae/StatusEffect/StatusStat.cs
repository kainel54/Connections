using System;
using UnityEngine;
using YH.Entities;

[Serializable]
public abstract class StatusStat : ICloneable
{
    protected Entity _entity;

    protected float _totalTimer;
    public float lifeTime;
    
    public bool lifeMode;
    public bool infiniteMode;
    public bool isActive;

    public event Action<float, float> updateEvent;
    public event Action endEvent;

    public virtual void StatusInit(Entity entity)
    {
        _entity = entity;
        isActive = true;
    }

    public virtual void StatusUpdate()
    {
        if (!isActive)
            return;

        if (lifeMode)
        {
            updateEvent?.Invoke(lifeTime, _totalTimer);

            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                RemoveStatus();
            }
        }
    }

    public virtual void RemoveStatus()
    {
        isActive = false;
        infiniteMode = false;
        lifeMode = false;
        _totalTimer = 0f;
        lifeTime = 0f;
        
        endEvent?.Invoke();
    }
    
    public object Clone()
    {
        StatusStat stat = MemberwiseClone() as StatusStat;
        return stat;
    }
}