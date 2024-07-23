using UnityEngine;
using UnityEngine.Events;

namespace Game.Entity 
{
    public abstract class Entity : MonoBehaviour, IDamage
    {
        [SerializeField] protected float _speed;
        public int _health;
        [HideInInspector] public UnityEvent<float> OnHealthChange;     

        public enum EntityStatus 
        {
            Alive,
            Dead,
        }

        protected EntityStatus _entityStatus;
        public EntityStatus GEntityStatus => _entityStatus;
        [HideInInspector] public UnityEvent<EntityStatus> OnEntityStatusChange;

        protected void Start()
        {
            _entityStatus = EntityStatus.Alive;
        }

        public void ChangeEntityState(EntityStatus state)
        {
            _entityStatus = state;
            OnEntityStatusChange.Invoke(state);
        }

        public void TakeDamage(int damageAmount)
        {
            _health -= damageAmount;
            Mathf.Clamp(_health,0,100);
            OnHealthChange.Invoke(_health);
            if (_health <= 0)
            {
                if(_entityStatus != EntityStatus.Dead)
                    ChangeEntityState(EntityStatus.Dead);
            }
        } 
    }
}

