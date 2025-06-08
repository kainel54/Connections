/*using DG.Tweening;
using UnityEngine;

public class Totem : MonoBehaviour
{
    private DropListSO _itemList;
    private DropItem _dropItem = null;

    private float _originYPos = 0.0f;

    public void Initialize()
    {
        _dropItem = Instantiate(_itemList.RandItem(), transform) as DropItem;
    }

    public void RaiseTotem()
    {
        transform.DOMoveY(_originYPos, 1).SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                SpawnObjects();
            });
    }

    public void LowerTotem()
    {

    }

    public void Clear()
    {
        _dropItem = null;
    }

    public void SpawnObjects()
    {
        _dropItem = Instantiate(_itemList.RandItem(), transform) as DropItem;
        _dropItem.transform.position = transform.position;

        if (_dropItem is ISpecialInitItem specialInitItem)
        {
            ItemDataSO dataSo = null;

            if (_dropItem as PartDropObject)
                dataSo = _itemList.RandSkillPart();
            if (_dropItem as SkillDropObject)
                dataSo = _itemList.RandSkill();
            if (_dropItem as NodeAbilityDropObject)
                dataSo = _itemList.RandNodeAbility();

            specialInitItem.SpecialInit(dataSo);
            specialInitItem.VisualInit();
        }

        _dropItem.transform.DOLocalMoveY(_itemYDelta, _tweenTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
        }
    }


    //[SerializeField] private DropListSO _itemList;

    //private float _originYPos = 0.0f;
    //private float _targetYPos = 10.0f;
    //private float _itemYDelta = 13.0f;
    //private float _tweenTime = 0.8f;

    //private bool _isDestroyed = false;
    //private DropItemSpawner _spawner;

    //private void Start()
    //{
    //    _originYPos = transform.position.y;
    //    _spawner = new DropItemSpawner(_itemList);
    //}

    //public void SpawnTotem()
    //{
    //    transform.DOMoveY(_originYPos, 1).SetEase(Ease.InOutSine)
    //        .OnComplete(() =>
    //        {
    //            SpawnObjects();
    //        });
    //}

    //public void DestroyTotem()
    //{
    //    if (_isDestroyed)
    //        GetObject();

    //    transform.DOMoveY(_originYPos, 1).SetEase(Ease.InOutSine);
    //}

    //public void SpawnObjects()
    //{
    //    _dropItem = Instantiate(_itemList.RandItem(), transform) as DropItem;
    //    _dropItem.transform.position = transform.position;

    //    if (_dropItem is ISpecialInitItem specialInitItem)
    //    {
    //        ItemDataSO dataSo = null;

    //        if (_dropItem as PartDropObject)
    //            dataSo = _itemList.RandSkillPart();
    //        if (_dropItem as SkillDropObject)
    //            dataSo = _itemList.RandSkill();
    //        if (_dropItem as NodeAbilityDropObject)
    //            dataSo = _itemList.RandNodeAbility();

    //        specialInitItem.SpecialInit(dataSo);
    //        specialInitItem.VisualInit();
    //    }

    //    _dropItem.transform.DOLocalMoveY(_itemYDelta, _tweenTime);
    //}

    //public void GetObject()
    //{
    //    print("collect");
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Bullet"))
    //    {
    //        _isDestroyed = true;

    //        TotemManager.Instance.Collect();
    //    }
    //}
}
*/