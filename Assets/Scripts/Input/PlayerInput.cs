using UnityEngine;
using UnityEngine.Events;

namespace Game.Inputsystem
{
    public class PlayerInput : MonoBehaviour
    {
        public enum PlayerMovementInputStateEnum { Idle, Moving }
        public enum PlayerAttackInputStateEnum { NotAttacking, ShortAttacking, LongAttacking }

        public PlayerMovementInputStateEnum PlayerMovementInputState { get; private set; }
        public PlayerAttackInputStateEnum PlayerAttackInputState { get; private set; }

        public UnityEvent<Vector3> OnMoveInput = new UnityEvent<Vector3>();
        public UnityEvent<Vector3> OnMousePosition = new UnityEvent<Vector3>();
        public UnityEvent OnShortAttack = new UnityEvent();
        public UnityEvent OnLongAttack = new UnityEvent();
        public UnityEvent OnInputStateChange = new UnityEvent();

        private void Update()
        {
            HandleMoveInput();
            HandleMouseInput();
            HandleAttackInput();
        }

        public void SetPlayerAttackState(PlayerAttackInputStateEnum state)
        {
            PlayerAttackInputState = state;
            OnInputStateChange.Invoke();
        }

        void HandleMoveInput()
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveZ = Input.GetAxisRaw("Vertical");

            Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

            if (move != Vector3.zero)
            {
                PlayerMovementInputState = PlayerMovementInputStateEnum.Moving;
                OnMoveInput.Invoke(move);
            }
            else
            {
                PlayerMovementInputState = PlayerMovementInputStateEnum.Idle;
            }

            OnInputStateChange.Invoke();
        }

        void HandleMouseInput()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                OnMousePosition.Invoke(hitInfo.point);
            }
        }

        void HandleAttackInput()
        {
            if (PlayerAttackInputState == PlayerAttackInputStateEnum.NotAttacking)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    SetPlayerAttackState(PlayerAttackInputStateEnum.ShortAttacking);
                    OnShortAttack.Invoke();
                }
                else if (Input.GetMouseButtonDown(1))
                {
                    SetPlayerAttackState(PlayerAttackInputStateEnum.LongAttacking);
                    OnLongAttack.Invoke();
                }
            }
        } 
    }
}
