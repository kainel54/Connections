using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using IH.EventSystem.SoundEvent;
using UnityEngine;
using YH.EventSystem;
using Random = UnityEngine.Random;

public abstract class DropItem : MonoBehaviour
{
    [SerializeField] private GameEventChannelSO _soundEventChannelSO;
    [SerializeField] private SoundSO _soundSo;
    
    public ItemDataSO itemData;
    
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _rotateSpeed = 10f;
    
    protected bool _alreadyCollected = false;
    public event Action CollectEvent;
    private bool _dropEnd = false;

    public SphereCollider SphereCollider { get; private set; }

    protected virtual void Awake()
    {
        SphereCollider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(_alreadyCollected)
                return;

            PlaySound();
            _alreadyCollected = true;
            PickUp(other);
            CollectEvent?.Invoke();
        }
    }
    
    private void Update()
    {
        transform.Rotate(Vector3.up, _rotateSpeed * Time.deltaTime);
    }
    
    public void SetItemDropPosition(Vector3 destination)
    {
        if(Physics.Raycast(destination, Vector3.down, out RaycastHit hit, 10f, _whatIsGround))
        {
            destination = hit.point + new Vector3(0, 0.5f);
        }
        
        float jumpPower = Random.Range(1.5f, 2f);
        float duration = Random.Range(0.7f, 1.2f);
        int jumpCount = 1;
        
        transform.DOJump(destination, jumpPower, jumpCount, duration, false).OnComplete(()=> _dropEnd = true);
    }
    
    public bool PickUpItem(Transform pickerTrm)
    {
        if (_alreadyCollected || _dropEnd == false) 
            return false;

        StartCoroutine(PickUpCoroutine(pickerTrm));
        return true;
    }
    
    private IEnumerator PickUpCoroutine(Transform pickerTrm)
    {
        if(!SphereCollider.isTrigger)
            yield break;
        
        Vector3 startPos = transform.position;
        //처음 먹을 때 플레이어와 코인간의 거리를 측정
        float distance = (pickerTrm.position - startPos).magnitude;

        float totalTime = distance * 0.1f;
        float current = 0;
        
        while(current / totalTime <= 1)
        {
            current += Time.deltaTime;

            transform.position = Vector3.Lerp(startPos, pickerTrm.position, current / totalTime);
            yield return null;
        }
    }

    public abstract void PickUp(Collider other);
    
    public void PlaySound()
    {
        var soundEvt = SoundEvents.PlaySfxEvent;
        soundEvt.position = transform.position;
        soundEvt.clipData = _soundSo;
        _soundEventChannelSO.RaiseEvent(soundEvt);
    }
}
