using System;
using TMPro;
using UnityEngine;
using YH.Players;

public class PlayerGoldText : MonoBehaviour
{
    [SerializeField] private PlayerManagerSO _playerManagerSo;
    [SerializeField] private TextMeshProUGUI _amountText;

    private void Start()
    {
        _amountText.text = "0";
        _playerManagerSo.OnCoinChanged += HandleGoldChange;
    }

    private void OnDestroy()
    {
        _playerManagerSo.OnCoinChanged -= HandleGoldChange;
    }

    private void HandleGoldChange(int currentCoin, int getValue)
    {
        _amountText.text = currentCoin.ToString();
    }
}
