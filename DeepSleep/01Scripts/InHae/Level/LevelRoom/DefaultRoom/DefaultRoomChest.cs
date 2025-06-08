using ObjectPooling;
using UnityEngine;
using Random = UnityEngine.Random;

public class DefaultRoomChest : MonoBehaviour
{
    [SerializeField] private int _dropCount = 20;
    [SerializeField] private float _dropRange;

    [SerializeField] private DropListSO _itemList;

    [SerializeField] private Transform _spawnPoint;
    private MeshFilter _chestMeshFilter;
    [SerializeField] private Mesh _openedChestMest;
    [SerializeField] private ParticleSystem _openParticle;
    [SerializeField] private SoundSO _openSO;

    private void OnEnable()
    {
        _chestMeshFilter = GetComponent<MeshFilter>();
    }
    
    private void Update()
    {
#if UNITY_STANDALONE_WIN
        if (Input.GetKeyDown(KeyCode.O) && Input.GetKey(KeyCode.LeftControl))
        {
            Open();
        }
#endif
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.O))
        {
            Open();
        }
#endif
    }

    public void Open()
    {
        //ParticleSystem openParticle = Instantiate(_openParticle, _spawnPoint.position, Quaternion.identity);
        _chestMeshFilter.mesh = _openedChestMest;

        SoundPlayer sound = PoolManager.Instance.Pop(ObjectType.SoundPlayer) as SoundPlayer;
        sound.PlaySound(_openSO);
        sound.transform.position = transform.position;

        for (int i = 0; i < _dropCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * _dropRange;
            Vector3 dropPos = new Vector3(transform.position.x + randomPoint.y * 1.2f, transform.position.y,
                transform.position.z + randomPoint.x * 0.8f);

            DropItem dropItem = Instantiate(_itemList.RandItem(), transform, true);

            if (dropItem is ISpecialInitItem specialInitItem)
            {
                ItemDataSO dataSo = null;

                if (dropItem as PartDropObject)
                    dataSo = _itemList.RandSkillPart();
                if(dropItem as SkillDropObject)
                    dataSo = _itemList.RandSkill();
                if(dropItem as NodeAbilityDropObject)
                    dataSo = _itemList.RandNodeAbility();
                
                specialInitItem.SpecialInit(dataSo);
                specialInitItem.VisualInit();
            }

            dropItem.transform.position = _spawnPoint.position;
            dropItem.SetItemDropPosition(dropPos);
        }

        for(int i = 0; i < 5; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * _dropRange;
            Vector3 dropPos = new Vector3(transform.position.x + randomPoint.y * 1.2f, transform.position.y,
                transform.position.z + randomPoint.x * 0.8f);

            Coin coin = PoolManager.Instance.Pop(ObjectType.Coin) as Coin;
            coin.transform.position = _spawnPoint.position;

            coin.SetItemDropPosition(dropPos);
            coin.value = Random.Range(5, 10);
        }
    }
}
