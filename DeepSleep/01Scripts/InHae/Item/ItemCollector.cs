using UnityEngine;
using YH.Players;

public class ItemCollector : MonoBehaviour
{
    public float collectRadius;
    [SerializeField] private LayerMask _whatIsItem;
    [SerializeField] private int _collectCount;

    private Collider[] _colliders;

    private Player _player;

    private void Awake()
    {
        _colliders = new Collider[_collectCount];
        _player = GetComponent<Player>();
    }

    private void FixedUpdate()
    {
        int count = Physics.OverlapSphereNonAlloc(
            transform.position, collectRadius, _colliders, _whatIsItem);

        for (int i = 0; i <count; i++)
        {
            if (_colliders[i].TryGetComponent(out DropItem item))
            {
                item.PickUpItem(transform);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
}
