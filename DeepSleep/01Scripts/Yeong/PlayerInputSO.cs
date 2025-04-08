using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YH.Players
{
    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInputSO")]
    public class PlayerInputSO : ScriptableObject, Controls.IPlayerActions
    {
        public event Action DashEvent;
        public event Action<bool> FireEvent;
        public event Action QSkillEvent;
        public event Action ESkillEvent;
        public event Action ShiftSkillEvent;
        public event Action FSkillEvent;
        public event Action UseSkillEvent;
        public event Action ReloadEvent;

        public Vector2 Movement { get; private set; }
        public Vector2 MousePosition { get; private set; }

        [SerializeField] private LayerMask _whatIsEnemy;
        [SerializeField] private LayerMask _whatIsGround;

        private Controls _controls;
        public Controls Controls => _controls;

        private Vector3 _beforeMousePos;

        public void ClearSubscription()
        {
            DashEvent = null;
            FireEvent = null;
        }

        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();
                _controls.Player.SetCallbacks(this);
            }
            _controls.Player.Enable();
        }

        private void OnDisable()
        {
            _controls.Player.Disable();
        }


        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
                DashEvent?.Invoke();
        }


        public void OnMousePos(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Movement = context.ReadValue<Vector2>();
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            // UI 클릭했을 때 발사 안되게 일단 처리 근데 마음에 안들어서 나중에 바꿀 계획
            if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                return;

            if (context.performed)
                FireEvent?.Invoke(true);
            else if (context.canceled)
                FireEvent?.Invoke(false);
        }
        public Vector3 GetWorldMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _whatIsGround))
            {
                _beforeMousePos = hitInfo.point;
                return hitInfo.point;
            }
            return _beforeMousePos;
        }


        public RaycastHit GetMouseHitInfo()
        {
            float radius = 1.5f;
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);

            if (Physics.SphereCast(ray, radius, out RaycastHit hitInfo, Mathf.Infinity, _whatIsEnemy))
            {
                return hitInfo;
            }
            return default;
        }

        public void OnQSkill(InputAction.CallbackContext context)
        {
            if (context.performed)
                QSkillEvent?.Invoke();
        }

        public void OnESkill(InputAction.CallbackContext context)
        {
            if (context.performed)
                ESkillEvent?.Invoke();
        }

        public void OnFSkill(InputAction.CallbackContext context)
        {
            if (context.performed)
                FSkillEvent?.Invoke();
        }
        public void OnShiftSkill(InputAction.CallbackContext context)
        {
            if (context.performed)
                ShiftSkillEvent?.Invoke();
        }

        public void OnUseSkill(InputAction.CallbackContext context)
        {
            if (context.performed)
                UseSkillEvent?.Invoke();
        }

       

        public void OnReload(InputAction.CallbackContext context)
        {
            if (context.performed)
                ReloadEvent?.Invoke();
        }
    }
}
