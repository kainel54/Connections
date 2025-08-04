using UnityEngine;

public interface IKnockBackable
{
    public void KnockBack(Vector3 force, Vector3 point);
    public bool IsKnockBack { get; }
}
