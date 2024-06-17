using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Player Movement", menuName = "Player Input")]
public class PlayerInputReader : ScriptableObject, CustomPlayerInput.IPlayerActions
{
    public Action<Vector2> moveEventPerformed;
    public Action moveEventCancelled;
    public Action jumpEventCancelled;
    public event Action jumpEventPerformed;
    public event Action interactEventPerformed;
    public event Action abilityEventPerformed;
    public event Action pauseEventPerformed;

    private CustomPlayerInput input;

    public void OnJump(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            jumpEventPerformed?.Invoke();
        }
        else if (pContext.phase == InputActionPhase.Canceled)
        {
            jumpEventCancelled?.Invoke();
        }
    }

    public void OnAbility(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            abilityEventPerformed?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            interactEventPerformed?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            moveEventPerformed?.Invoke(pContext.ReadValue<Vector2>());
        }
        else if (pContext.phase == InputActionPhase.Canceled)
        {
            moveEventCancelled?.Invoke();
        }
    }

    public void OnPause(InputAction.CallbackContext pContext) {
        if (pContext.phase == InputActionPhase.Performed)
        {
            pauseEventPerformed?.Invoke();
        }
    }


    private void OnEnable()
    {
        if (input == null)
        {
            input = new CustomPlayerInput();
            input.Player.SetCallbacks(this);
        }

        input.Player.Enable();
    }

    private void OnDisable()
    {
        if (input != null) input.Player.Disable();
    }
}
