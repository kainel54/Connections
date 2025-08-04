using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace YH.Players
{
    public enum ControlType
    {
        WASD,
        PointNClick
    }

    [CreateAssetMenu(fileName = "PlayerInputSO", menuName = "SO/PlayerInputSO")]
    public class PlayerInputSO : ScriptableObject, Controls.IWASDActions, Controls.IPointNClickActions
    {
        public Action<bool> MoveEvent;
        public Action StopEvent;
        public Action DashEvent;
        public event Action<bool> LeftClickWaitEvent;
        public Action<bool> AttackEvent;
        public Action InteractEvent;

        public List<Action> SkillActions = new List<Action>(4)
        {
            null, null, null, null
        };

        private List<string> pointNClickSkillKeyNames = new List<string>(4)
        {
            "Q", "W", "E", "R"
        };

        private List<string> WASDSkillKeyNames = new List<string>(4)
        {
            "Q", "E", "C", "Shift"
        };

        public Vector2 Movement { get; private set; }
        public Vector2 MousePosition { get; private set; }

        [SerializeField] private LayerMask _whatIsEnemy;
        [SerializeField] private LayerMask _whatIsGround;
        [SerializeField] private LayerMask _whatIsMouseCheck;
        [SerializeField] private ControlType _controlType;

        public ControlType ControlType => _controlType;

        private Controls _controls;
        public Controls Controls => _controls;

        private Vector3 _beforeMousePos;

        public void ClearSubscription()
        {
            DashEvent = null;
            AttackEvent = null;
            StopEvent = null;
            MoveEvent = null;
            InteractEvent = null;
            for(byte i = 0; i < SkillActions.Count; i++)
                SkillActions[i] = null;
        }

        public string GetSkillKeyName(int skillIdx)
        {
            if (_controlType == ControlType.PointNClick)
                return pointNClickSkillKeyNames[skillIdx];
            else
                return WASDSkillKeyNames[skillIdx];
        }   
        private void OnEnable()
        {
            if (_controls == null)
            {
                _controls = new Controls();

                switch (_controlType)
                {
                    case ControlType.WASD:
                        _controls.WASD.SetCallbacks(this);
                        break;
                    case ControlType.PointNClick:
                        _controls.PointNClick.SetCallbacks(this);
                        break;
                }
            }

            GetActionMap()?.Enable();
        }

        private void OnDisable()
        {
            GetActionMap()?.Disable();
        }
        
        public InputActionMap GetActionMap()
        {
            return _controlType switch
            {
                ControlType.WASD => _controls.WASD,
                ControlType.PointNClick => _controls.PointNClick,
                _ => null
            };
        }

        public void OnDash(InputAction.CallbackContext context)
        {
            if (context.performed)
                DashEvent?.Invoke();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.performed || context.started)
                MoveEvent?.Invoke(true);
            else
                MoveEvent?.Invoke(false);

            Movement = context.ReadValue<Vector2>();
        }
        public Vector3 GetWorldMousePosition()
        {
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _whatIsMouseCheck))
            {
                _beforeMousePos = hitInfo.point;
                return hitInfo.point;
            }
            return _beforeMousePos;
        }

        public RaycastHit GetMouseGroundHit()
        {
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);
            RaycastHit hit = default;

            if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, _whatIsGround))
                hit = hitInfo;
            
            return hit;
        }
        
        public RaycastHit GetMouseHitInfo()
        {
            float radius = 0.5f;
            Ray ray = Camera.main.ScreenPointToRay(MousePosition);

            if (Physics.SphereCast(ray, radius, out RaycastHit hitInfo, Mathf.Infinity, _whatIsEnemy))
            {
                return hitInfo;
            }
            return default;
        }

        public void OnInteract(InputAction.CallbackContext context)
        {
            if (context.performed)
                InteractEvent?.Invoke();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            AttackCheck(context);
        }

        private void AttackCheck(InputAction.CallbackContext context)
        {
            if (context.performed)
                LeftClickWaitEvent?.Invoke(true);
            else if (context.canceled)
                LeftClickWaitEvent?.Invoke(false);

        }
        
        public void OnSkill1(InputAction.CallbackContext context)
        {
            if (context.performed)
                SkillActions[0]?.Invoke();
        }

        public void OnSkill2(InputAction.CallbackContext context)
        {
            if (context.performed)
                SkillActions[1]?.Invoke();
        }

        public void OnSkill3(InputAction.CallbackContext context)
        {
            if (context.performed)
                SkillActions[2]?.Invoke();
        }

        public void OnSkill4(InputAction.CallbackContext context)
        {
            if (context.performed)
                SkillActions[3]?.Invoke();
        }

        public void OnMousePosition(InputAction.CallbackContext context)
        {
            MousePosition = context.ReadValue<Vector2>();
        }

        public void OnStop(InputAction.CallbackContext context)
        {
            if (context.performed)
                StopEvent?.Invoke();
        }

        public void OnMoveClick(InputAction.CallbackContext context)
        {
            if (context.performed)
                MoveEvent?.Invoke(true);
            else
                MoveEvent?.Invoke(false);
        }
    }
}
