using System;
using UnityEngine;

namespace YH.Players
{
    [CreateAssetMenu(fileName = "PlayerManagerSO", menuName = "SO/PlayerManager")]
    public class PlayerManagerSO : ScriptableObject
    {
        public Player Player { get; private set; }
        public Transform PlayerTrm { get; private set; }

        public event Action SetUpPlayerEvent;
        
        private int _currentCoin;
        public int CurrentCoin => _currentCoin;
        
        // 1. 현재 코인, 2. 획득량
        public event Action<int, int> OnCoinChanged;

        private void OnEnable()
        {
            InitCoin();
        }

        public void InitCoin()
        {
            _currentCoin = 0;
            OnCoinChanged?.Invoke(_currentCoin, _currentCoin);
        }

        public void SetPlayer(Player player)
        {
            Player = player;
            PlayerTrm = player.transform;
            
            SetUpPlayerEvent?.Invoke();
        }
        
        public void AddCoin(int value)
        {
            _currentCoin += value;
            OnCoinChanged?.Invoke(_currentCoin, value);
        }
    }
}
