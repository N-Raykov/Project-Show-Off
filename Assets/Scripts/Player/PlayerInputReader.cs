using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Player Movement", menuName = "Player Input")]
public class PlayerInputReader : ScriptableObject, CustomPlayerInput.IPlayerActions, CustomPlayerInput.IUIActions
{
    public event Action<Vector2> moveEventPerformed;
    public event Action moveEventCancelled;
    public event Action jumpEventCancelled;
    public event Action jumpEventPerformed;
    public event Action interactEventPerformed;
    public event Action abilityEventPerformed;
    public event Action openMenuEventPerformed;
    public event Action closeMenuEventPerformed;
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

    public void OnOpenMenu(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            openMenuEventPerformed?.Invoke();
            //Debug.Log("OnOpenMenu: Player: " + (input.Player.enabled ? "ENABLED" : "DISABLED") + " UI: " + (input.UI.enabled ? "ENABLED" : "DISABLED"));
        }
    }

    public void OnCloseMenu(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            closeMenuEventPerformed?.Invoke();
            //Debug.Log("OnCloseMenu: Player: " + (input.Player.enabled ? "ENABLED" : "DISABLED") + " UI: " + (input.UI.enabled ? "ENABLED" : "DISABLED"));
        }
    }

    public void OnNavigateMenu(InputAction.CallbackContext pContext) {}

    public void OnPause(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            pauseEventPerformed?.Invoke();
        }
    }

    public void SwitchInputActionMaps()
    {
        if (input.Player.enabled)
        {
            input.Player.Disable();
            input.UI.Enable();
        } else
        {
            input.UI.Disable();
            input.Player.Enable();
        }
    } 

    private void OnEnable()
    {
        if (input == null)
        {
            input = new CustomPlayerInput();
            input.Player.SetCallbacks(this);
            input.UI.SetCallbacks(this);
        }

        input.Player.Enable();
        input.UI.Enable();
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.Player.Disable();
            input.UI.Disable();
        }
    }
}
