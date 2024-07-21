using UnityEngine;
using Game.Inputsystem;
using System.Collections;

namespace Game.Entity
{
    [RequireComponent(typeof(Animator))]
    public class Player : Entity
    {
        private Camera _camera; // Reference to the Camera
        private Animator _animator;
        [SerializeField] private PlayerInput _playerInput;

        private bool _canMove = true;

        new void Start()
        {
            base.Start();

            _camera = Camera.main;
            if (_playerInput == null)
            {
                Debug.LogError("Player input not found!");
                enabled = false;
                return;
            }

            _animator = GetComponent<Animator>();
            _animator.updateMode = AnimatorUpdateMode.Normal; // Ensures immediate animation updates

            _playerInput.OnMoveInput.AddListener(Move);
            _playerInput.OnMousePosition.AddListener(RotateTowardsMouse);
            _playerInput.OnShortAttack.AddListener(ShortAttack);
            _playerInput.OnLongAttack.AddListener(LongAttack);
            _playerInput.OnInputStateChange.AddListener(PlayAnimations);
        }

        void Update()
        {
            PlayAnimations();
        }

        void PlayAnimations()
        {
            if (_playerInput.PlayerAttackInputState == PlayerInput.PlayerAttackInputStateEnum.NotAttacking)
            {
                switch (_playerInput.PlayerMovementInputState)
                {
                    case PlayerInput.PlayerMovementInputStateEnum.Idle:
                        _animator.Play("idle");
                        break;
                    case PlayerInput.PlayerMovementInputStateEnum.Moving:
                        _animator.Play("walk");
                        break;
                }
            }
            else
            {
                switch (_playerInput.PlayerAttackInputState)
                {
                    case PlayerInput.PlayerAttackInputStateEnum.ShortAttacking:
                        _animator.Play("slash");
                        StartCoroutine(ResetAttackStateAfterAnimation());
                        break;
                    case PlayerInput.PlayerAttackInputStateEnum.LongAttacking:
                        _animator.Play("casting");
                        StartCoroutine(ResetAttackStateAfterAnimation());
                        break;
                }
            }
        }

        private void Move(Vector3 moveInput)
        {
            if (!_canMove) return;

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

        void ShortAttack()
        {
            if (_playerInput.PlayerAttackInputState == PlayerInput.PlayerAttackInputStateEnum.NotAttacking)
            {
                _playerInput.SetPlayerAttackState(PlayerInput.PlayerAttackInputStateEnum.ShortAttacking);
                _animator.Play("slash");
                StartCoroutine(ResetAttackStateAfterAnimation());
            }
        }

        void LongAttack()
        {
            if (_playerInput.PlayerAttackInputState == PlayerInput.PlayerAttackInputStateEnum.NotAttacking)
            {
                _playerInput.SetPlayerAttackState(PlayerInput.PlayerAttackInputStateEnum.LongAttacking);
                _animator.Play("casting");
                StartCoroutine(ResetAttackStateAfterAnimation());
            }
        }

        private IEnumerator ResetAttackStateAfterAnimation()
        {
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || _animator.IsInTransition(0))
            {
                yield return null;
            }
            _playerInput.SetPlayerAttackState(PlayerInput.PlayerAttackInputStateEnum.NotAttacking);
        }

        private void OnDestroy()
        {
            _playerInput.OnMoveInput.RemoveListener(Move);
            _playerInput.OnMousePosition.RemoveListener(RotateTowardsMouse);
            _playerInput.OnShortAttack.RemoveListener(ShortAttack);
            _playerInput.OnLongAttack.RemoveListener(LongAttack);
            _playerInput.OnInputStateChange.RemoveListener(PlayAnimations);
        }
    }
}
