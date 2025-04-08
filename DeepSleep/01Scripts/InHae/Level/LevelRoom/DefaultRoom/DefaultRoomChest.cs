using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DefaultRoomChest : MonoBehaviour
{
    [SerializeField] private int _dropCount = 20;
    [SerializeField] private float _dropRange;

    // 나중에 드랍테이블로 변경 예정
    [SerializeField] private List<PartItemSO> partList;
    [SerializeField] private List<SkillItemSO> skillList;
    [SerializeField] private PartsObject _partsObject;
    [SerializeField] private SkillObject _skillObject;
    [SerializeField] private DropListSO _itemList;

    [SerializeField] private Transform _spawnPoint;
    private MeshFilter _chestMeshFilter;
    [SerializeField] private Mesh _openedChestMest;
    [SerializeField] private ParticleSystem _openParticle;

    private void OnEnable()
    {
        _chestMeshFilter = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            Open();
        }
    }

    public void Open()
    {
        //ParticleSystem openParticle = Instantiate(_openParticle, _spawnPoint.position, Quaternion.identity);
        _chestMeshFilter.mesh = _openedChestMest;

        for (int i = 0; i < _dropCount; i++)
        {
            Vector2 randomPoint = Random.insideUnitCircle * _dropRange;
            Vector3 partsDropPos = new Vector3(transform.position.x + randomPoint.x, transform.position.y,
                transform.position.z + randomPoint.y);
            Vector3 skillDropPos = new Vector3(transform.position.x + randomPoint.y * 1.2f, transform.position.y,
                transform.position.z + randomPoint.x * 0.8f);

            DropItem dropItem = Instantiate(_itemList.RandItem());
            

            if (dropItem is PartsObject partsObject)
            {
                partsObject.PartInit(_itemList.RandSkillParts());
                partsObject.VisualInit();
                
                partsObject.transform.SetParent(transform);
                partsObject.transform.position = _spawnPoint.position;
                partsObject.SetItemDropPosition(skillDropPos);
            }

            if (dropItem is SkillObject skillObject)
            {
                skillObject.SkillInit(_itemList.RandSkill());
                skillObject.VisualInit();
                
                skillObject.transform.SetParent(transform);
                skillObject.transform.position = _spawnPoint.position;
                skillObject.SetItemDropPosition(skillDropPos);
            }
        }
    }
}
