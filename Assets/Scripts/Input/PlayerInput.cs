using UnityEngine;
using UnityEngine.Events;

public class PlayerInput : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Vector3> OnMoveInput;
    [HideInInspector] public UnityEvent<Vector3> OnMousePosition;

    void Update()
    {
        HandleMoveInput();
        HandleMouseInput();
    }

    void HandleMoveInput()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(moveX, 0, moveZ).normalized;

        if (move != Vector3.zero)
        {
            OnMoveInput.Invoke(move);
        }
    }

    void HandleMouseInput()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            if (hitInfo.transform && OnMousePosition != null)
            {
                OnMousePosition.Invoke(hitInfo.point);
            }
        }
    }
}