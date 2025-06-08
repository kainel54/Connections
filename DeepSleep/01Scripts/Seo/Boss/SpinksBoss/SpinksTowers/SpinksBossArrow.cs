using DG.Tweening;
using UnityEngine;

public class SpinksBossArrow : MonoBehaviour
{
    public void SetMove()
    {
        transform.DOLocalMoveY(4, 1).SetLoops(-1, LoopType.Yoyo);
        transform.DOLocalRotate(Vector3.up * 360, 3f).SetLoops(-1);
    }
}
