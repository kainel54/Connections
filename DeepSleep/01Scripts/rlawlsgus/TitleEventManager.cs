using System;
using System.Collections;
using UnityEngine;

public class TitleEvent : MonoBehaviour
{
    [SerializeField] private GameObject fireEffect;
    [SerializeField] private GameObject fireEffect2;

    public void Start()
    {
        StartCoroutine(TitleFireRoutine());
    }

    private IEnumerator TitleFireRoutine()
    {
        yield return new WaitForSeconds(0.8f);
        fireEffect.SetActive(true);
        yield return new WaitForSeconds(0.025f);
        fireEffect2.SetActive(true);
    }
}
