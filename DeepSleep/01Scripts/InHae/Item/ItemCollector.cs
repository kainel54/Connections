using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    public float collectRadius;
    [SerializeField] private LayerMask _whatIsItem;
    [SerializeField] private int _collectCount;

    private Collider[] _colliders;
    
    private void Awake()
    {
        _colliders = new Collider[100];
    }

    private void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(transform.position, collectRadius, 
            _colliders, _whatIsItem);

        int collectAbleCount = 0;
        for (int i = 0; i < count; i++)
        {
            if (collectAbleCount >= _collectCount)
                break;

            var collider = _colliders[i];
            if (collider.TryGetComponent(out DropItem item) && item.IsCollectAble)
            {
                item.PickUpItem(transform);
                collectAbleCount++;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}
