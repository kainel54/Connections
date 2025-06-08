using UnityEngine;
using YH.Combat;
using YH.Entities;

public class Shield : MonoBehaviour
{
    private Entity _owner;

    private void Awake()
    {
        _owner = GetComponentInParent<Entity>();
    }

    public void ApplyDamage(HitData hitData)
    {
        _owner.GetCompo<EntityHealth>().ApplyDamage(hitData,true,true,0.4f);
    }
}
