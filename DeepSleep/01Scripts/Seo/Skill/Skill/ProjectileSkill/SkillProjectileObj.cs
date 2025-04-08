using System;
using System.Collections;
using UnityEngine;

public class SkillProjectileObj : SkillObj
{
    [HideInInspector] public Skill skill;
    private TrajectoryManager _trajectoryManager;
    private BaseTrajectory _trajectory;
    protected bool _ispenetration = false;
    protected bool _canBeHit = false;

    private void Awake()
    {
        _trajectoryManager = GetComponentInChildren<TrajectoryManager>();
    }

    public void Initialize(Skill _skill)
    {
        skill = _skill;

        SetTrajectory();
    }
    public void SetTrajectory()
    {
        if (skill.GetSkillData(SkillFieldDataType.Projectile) is ProjectileSkillDataSO projectileSkillDataSO)
        {
            _trajectory = _trajectoryManager.GetTrajectory(projectileSkillDataSO.currentTrajectoryList);
            _trajectory.Init(this);
        }
    }

    public void DestroyObject(GameObject gameObject)
    {
        Debug.Log("DestroyObject");
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        Vector3 dir = _trajectory.UpdateTrajectory();

        // transform.rotation = Quaternion.Euler(dir);
        transform.position += dir;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("DetectWall")
            || other.gameObject.layer == LayerMask.NameToLayer("Wall"))
        {
            CallDestroyEvent();
        }
    }
}
