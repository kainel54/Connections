using UnityEngine;
using UnityEngine.AI;
using YH.Entities;

public class BossMovement : EnemyMovement
{

    private void Start()
    {
        _navAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
        _navAgent.avoidancePriority = 1;
    }
}
