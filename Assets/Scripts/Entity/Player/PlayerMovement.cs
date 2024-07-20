using UnityEditor.PackageManager;
using UnityEngine;

namespace Game.Entity 
{
    public class PlayerMovement : Entity
    {
        private Camera _camera; // Reference to the Camera
        [SerializeField] private PlayerInput _playerInput;

        new void Start()
        {
            base.Start();

            _camera = Camera.main;
            if (_playerInput == null) 
            {
                Debug.Log("Player input not found!"); // Fail early.
                enabled = false;
            }

            _playerInput.OnMoveInput.AddListener(Move);
            _playerInput.OnMousePosition.AddListener(RotateTowardsMouse);
        }

        private void Move(Vector3 moveInput)
        {
            transform.position += moveInput * _speed * Time.deltaTime;
        }

        void RotateTowardsMouse(Vector3 hitInfoPoint)
        {
            Vector3 lookAtPosition = hitInfoPoint;
            lookAtPosition.y = transform.position.y;
            Vector3 direction = (lookAtPosition - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        private void OnDestroy()
        {
            _playerInput.OnMoveInput.RemoveListener(Move);
            _playerInput.OnMousePosition.RemoveListener(RotateTowardsMouse);
        }
    }
}