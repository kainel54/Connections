using System.Collections;
using UnityEngine;
using YH.Entities;

public class DefaultEnemyMovement : EnemyMovement, IKnockBackable
{
    [SerializeField] private float _knockbackThreshold;
    [SerializeField] private float _maxKnockbackTime;

    private float _knockBackStartTime;
    private bool _isKnockBack;
    public bool IsKnockBack => _isKnockBack;
    
    private Coroutine _knockBackCoroutine;

    private void OnDisable()
    {
        _isKnockBack = false;
        DisableRigidbody();
        
        if(_knockBackCoroutine != null)
            StopCoroutine(_knockBackCoroutine);
    }

    public override void SetStop(bool isStop)
    {
        if (_isKnockBack) 
            return;

        base.SetStop(isStop);
    }

    public void KnockBack(Vector3 force, Vector3 point)
    {
        if (_isKnockBack)
            return;
        
        _knockBackCoroutine = StartCoroutine(ApplyKnockBackCoroutine(force, point));
    }

    private IEnumerator ApplyKnockBackCoroutine(Vector3 force, Vector3 point)
    {
        if(_isKnockBack)
            yield break;
        
        _navAgent.enabled = false;
        _rbCompo.useGravity = true;
        _rbCompo.isKinematic = false;
        _rbCompo.AddForce(force, ForceMode.Impulse);
        _knockBackStartTime = Time.time;

        _isKnockBack = true;
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        yield return new WaitUntil(
            () => _rbCompo.linearVelocity.magnitude < _knockbackThreshold ||
                  Time.time > _knockBackStartTime + _maxKnockbackTime);

        DisableRigidbody();

        _navAgent.enabled = true;
        _isKnockBack = false;

        yield return null;
    }

    private void DisableRigidbody()
    {
        if(_rbCompo.isKinematic)
            return;
        
        _rbCompo.linearVelocity = Vector3.zero;
        _rbCompo.angularVelocity = Vector3.zero;
        _rbCompo.useGravity = false;
        _rbCompo.isKinematic = true;
    }

    public void Stun(float time)
    {
        throw new System.NotImplementedException();
    }
}

