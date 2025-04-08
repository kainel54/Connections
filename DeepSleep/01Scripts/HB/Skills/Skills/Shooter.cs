using UnityEngine;
using UnityEngine.InputSystem;

namespace HB.Skill
{
    public class Shooter : MonoBehaviour
    {
        [SerializeField] private GameObject _skillObject;
        public Vector3 firePos;

        private void Update()
        {
            if (Mouse.current.rightButton.wasPressedThisFrame)
            {
                GameObject energyBall = Instantiate(_skillObject, transform.position, Quaternion.identity);
            }
        }
    }
}