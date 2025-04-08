using YH.Entities;
using YH.FSM;
using YH.StatSystem;
using UnityEngine;
using YH.EventSystem;

namespace YH.Players
{
    public class Player : Entity
    {
        // Debug
        [SerializeField] private GameEventChannelSO _statusEventChannel;

        [SerializeField] private PlayerManagerSO _playerManagerSO;

        [field: SerializeField] private StatElementSO _attackCooldownSO;
        public StatElement attackCooldownStat { get; private set; }
        [field: SerializeField] public PlayerInputSO PlayerInput { get; private set; }

        private StateMachine _stateMachine;

        private StatCompo _statCompo;
        public bool isShooting { get; set; }

        public float attackDistance;
        [HideInInspector] public float lastAttackTime;
        [HideInInspector] public Transform target;

        public LayerMask whatIsEnemy;
        public Transform fireTrm;
        
        protected override void Awake()
        {
            base.Awake();
            _playerManagerSO.SetPlayer(this);
            _playerManagerSO.InitCoin();
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();

            _statCompo = GetCompo<StatCompo>();

            attackCooldownStat = _statCompo.GetElement(_attackCooldownSO);
        }

        private void OnDestroy()
        {
            PlayerInput.ClearSubscription();
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                GetCompo<HealthCompo>().SetInvincible(true);
            }
            
            if (Input.GetKeyDown(KeyCode.K))
            {
                var evt = StatusEvents.AddTimeStatusEvent;
                evt.entity = this;
                evt.time = 4f;
                evt.status = StatusEnum.SpeedDownDebuff;

                _statusEventChannel.RaiseEvent(evt);
            }
            
            if (Input.GetKeyDown(KeyCode.J))
            {
                var evt = StatusEvents.RemoveStatusEvent;
                evt.entity = this;
                evt.status = StatusEnum.SpeedDownDebuff;

                _statusEventChannel.RaiseEvent(evt);
            }
        }
#endif

        protected override void HandleDeadEvent()
        {
            base.HandleDeadEvent();
        }
        
        public void SetDead()
        {
            PlayerInput.Controls.Disable();
            IsDead = true;
        }
    }
}
