using UnityEngine;
using System.Collections;

namespace Game.Entity
{
    public class Zombie : Entity
    {
        [SerializeField] private float detectionRadius = 5f;
        [SerializeField] private int damageAmount = 10;
        [SerializeField] private float damageInterval = 1f;
        [SerializeField] private LayerMask obstacleLayer; // Layer mask to detect obstacles
        [SerializeField] private float contextRadius = 1f; // Radius to evaluate context
        [SerializeField] private float maxWeight = 10f; // Maximum weight for a direction
        private Animator _animator;
        private Transform player;
        private IDamage playerDamage;
        private bool isDamagingPlayer = false;

        private enum ZombieState
        {
            Walking,
            Attacking
        }

        private ZombieState currentState;

        new void Start()
        {
            base.Start();
            _animator = GetComponent<Animator>();
            GameObject playerObject = GameObject.FindGameObjectWithTag("player");
            if (playerObject != null)
            {
                player = playerObject.transform;
                playerDamage = playerObject.GetComponent<IDamage>();
            }
            ChangeState(ZombieState.Walking);
            OnEntityStatusChange.AddListener(Die);
        }

        void Update()
        {
            if (_entityStatus == EntityStatus.Alive)
            {
                switch (currentState)
                {
                    case ZombieState.Walking:
                        MoveAndRotateTowardsPlayer();
                        break;
                    case ZombieState.Attacking:
                        if (!isDamagingPlayer)
                        {
                            StartCoroutine(DamagePlayer());
                        }
                        break;
                }
            }
        }

        void ChangeState(ZombieState newState)
        {
            if (currentState == newState) return; // Avoid redundant state changes

            currentState = newState;
            switch (currentState)
            {
                case ZombieState.Walking:
                    _animator.Play("zombie walk");
                    break;
                case ZombieState.Attacking:
                    _animator.Play("zombie attack");
                    break;
            }
        }

        void Die(EntityStatus state)
        {
            if (state == EntityStatus.Dead)
            {
                _animator.Play("zombie die");
                Destroy(gameObject, 5f);
            }
        }

        void MoveAndRotateTowardsPlayer()
        {
            if (player == null) return;

            Vector3 bestDirection = GetBestDirection();

            // Move towards the best direction
            transform.position += bestDirection.normalized * _speed * Time.deltaTime;

            // Rotate towards the best direction
            if (bestDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(bestDirection);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360 * Time.deltaTime); // Rotate more quickly
            }

            if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                ChangeState(ZombieState.Attacking);
            }
        }

        Vector3 GetBestDirection()
        {
            Vector3 bestDirection = Vector3.zero;
            float bestWeight = -Mathf.Infinity;

            for (int i = 0; i < 360; i += 10) // Check directions in increments of 10 degrees
            {
                Vector3 direction = Quaternion.Euler(0, i, 0) * Vector3.forward;
                float weight = EvaluateDirection(direction);

                if (weight > bestWeight)
                {
                    bestWeight = weight;
                    bestDirection = direction;
                }
            }

            return bestDirection;
        }

        float EvaluateDirection(Vector3 direction)
        {
            float weight = maxWeight;

            // Check for obstacles
            RaycastHit hit;
            if (Physics.SphereCast(transform.position, contextRadius, direction, out hit, contextRadius, obstacleLayer))
            {
                weight -= maxWeight;
            }

            // Check player direction
            Vector3 toPlayer = (player.position - transform.position).normalized;
            weight -= Vector3.Angle(direction, toPlayer) / 180f * maxWeight;

            return weight;
        }

        private IEnumerator DamagePlayer()
        {
            isDamagingPlayer = true;

            while (currentState == ZombieState.Attacking)
            {
                if (Vector3.Distance(transform.position, player.position) > detectionRadius)
                {
                    ChangeState(ZombieState.Walking);
                    break;
                }

                if (playerDamage != null)
                {
                    playerDamage.TakeDamage(damageAmount);
                }

                yield return new WaitForSeconds(damageInterval);
            }

            isDamagingPlayer = false;
        }

        void OnDestroy()
        {
            OnEntityStatusChange.RemoveListener(Die);
        }
    }
}
