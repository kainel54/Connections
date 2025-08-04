using UnityEngine;

namespace YH.Players
{
    public class WASDPlayer : Player
    {
        private CharacterController _playerController;

        protected override void Awake()
        {
            base.Awake();
            _playerController = GetComponent<CharacterController>();
        }

        public override void PlayerLevelMove(Vector3 position)
        {
            _playerController.enabled = false;
            transform.position = position;
            _playerController.enabled = true;
        }
    }
}
