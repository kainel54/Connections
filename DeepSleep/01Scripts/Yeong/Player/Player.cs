using System;
using IH.EventSystem.StatusEvent;
using YH.Entities;
using YH.StatSystem;
using UnityEngine;
using YH.EventSystem;
using Unity.AppUI.UI;

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
        [SerializeField] private LayerMask _whatIsGround,_whatIsTower;
        private Outline _outline;

        private EntityStat _statCompo;
        public bool isShooting { get; set; }

        public float attackDistance;
        [HideInInspector] public float lastAttackTime;
        [HideInInspector] public Transform target;

        public LayerMask whatIsEnemy;
        public Transform fireTrm;

        public EntityHealth EntityHealth { get; private set; }
        protected override void Awake()
        {
            base.Awake();
            _playerManagerSO.SetPlayer(this);
            _playerManagerSO.InitCoin();
            _outline = GetComponent<Outline>();
            _outline.enabled = false;

            EntityHealth = GetComponent<EntityHealth>();
        }

        protected override void AfterInitComponents()
        {
            base.AfterInitComponents();

            _statCompo = GetCompo<EntityStat>();

            attackCooldownStat = _statCompo.GetElement(_attackCooldownSO);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            PlayerInput.ClearSubscription();
        }

        private void Update()
        {
            SetOutLine();
#if UNITY_STANDALONE_WIN
            if (Input.GetKeyDown(KeyCode.B) && Input.GetKey(KeyCode.LeftControl))
            {
                GetCompo<EntityHealth>().SetInvincible(true);
            }
            if (Input.GetKeyDown(KeyCode.K) && Input.GetKey(KeyCode.LeftControl))
            {
                _playerManagerSO.AddCoin(1000);
            }
#endif
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.B))
            {
                GetCompo<EntityHealth>().SetInvincible(true);
            }
            if (Input.GetKeyDown(KeyCode.K))
            {
                _playerManagerSO.AddCoin(1000);
            }
#endif
        }

        private void SetOutLine()
        {
            Vector3 origin = Camera.main.transform.position;
            Vector3 direction = (transform.position - origin).normalized;
            float distance = Vector3.Distance(origin, transform.position);

            if (Physics.SphereCast(origin, 1f, direction, out RaycastHit hit, distance, _whatIsTower))
            {
                _outline.enabled = true;
            }
            else
            {
                _outline.enabled = false;
            }
        }

        
        public void SetDead()
        {
            PlayerInput.Controls.Disable();
            IsDead = true;
        }
    }
}
