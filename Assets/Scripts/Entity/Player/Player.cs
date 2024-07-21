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
        [SerializeField] private LayerMask _hitLayerMask;
        [SerializeField] private float shortAttackRange = 1f; // Define appropriate attack range
        [SerializeField] private int shortAttackDamage = 10; // Define appropriate damage

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
            _playerInput.OnInputStateChange.AddListener(PlayAliveAnimations);
            OnEntityStatusChange.AddListener(Die);
        }

        void Update()
        {
            if (_entityStatus == EntityStatus.Alive)
                PlayAliveAnimations();
        }

        void PlayAliveAnimations()
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
                if (_playerInput.PlayerAttackInputState == PlayerInput.PlayerAttackInputStateEnum.ShortAttacking)
                {
                    _animator.Play("slash");
                    StartCoroutine(ResetAttackStateAfterAnimation());
                }
            }
        }

        void Die(EntityStatus status)
        {
            if (status == EntityStatus.Dead)
            {
                _playerInput.SetCanInput(false);
                _animator.Play("die");
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
                StartCoroutine(ResetAttackStateAfterAnimation());
            }
        }

        private IEnumerator ResetAttackStateAfterAnimation()
        {
            _animator.Play("slash");
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || _animator.IsInTransition(0))
            {
                yield return null;
            }
            CheckForEnemiesAndDealDamage(shortAttackRange, shortAttackDamage);
            _playerInput.SetPlayerAttackState(PlayerInput.PlayerAttackInputStateEnum.NotAttacking);
        }

        void CheckForEnemiesAndDealDamage(float attackRange, int attackDamage)
        {
            Debug.Log($"Performing attack check with range: {attackRange} and damage: {attackDamage}");

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, _hitLayerMask);
            Debug.Log($"Number of colliders detected: {hitColliders.Length}");

            foreach (var hitCollider in hitColliders)
            {
                Debug.Log($"Detected: {hitCollider.name}");

                Entity enemy = hitCollider.GetComponent<Entity>();
                if (enemy != null)
                {
                    Debug.Log($"Enemy found: {enemy.gameObject.name}");
                    if (enemy.GEntityStatus == EntityStatus.Alive)
                    {
                        Debug.Log($"Damaging enemy: {enemy.gameObject.name}");
                        enemy.TakeDamage(attackDamage);
                    }
                    else
                    {
                        Debug.Log($"Enemy not alive: {enemy.gameObject.name}");
                    }
                }
                else
                {
                    Debug.Log($"No Entity component found on: {hitCollider.name}");
                }
            }
        }

        private void OnDestroy()
        {
            _playerInput.OnMoveInput.RemoveListener(Move);
            _playerInput.OnMousePosition.RemoveListener(RotateTowardsMouse);
            _playerInput.OnShortAttack.RemoveListener(ShortAttack);
            _playerInput.OnInputStateChange.RemoveListener(PlayAliveAnimations);
            OnEntityStatusChange.RemoveListener(Die);
        }
    }
}