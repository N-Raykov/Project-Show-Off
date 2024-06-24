using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "New Player Movement", menuName = "Player Input")]
public class PlayerInputReader : ScriptableObject, CustomPlayerInput.IPlayerActions, CustomPlayerInput.IIngameMenuActions, CustomPlayerInput.IBeforeAfterGameMenuActions
{
    //For IPlayerActions
    public event Action<Vector2> moveEventPerformed;
    public event Action moveEventCancelled;
    public event Action jumpEventPerformed;
    public event Action jumpEventCancelled;
    public event Action interactEventPerformed;
    public event Action abilityEventPerformed;
    public event Action openMenuEventPerformed;

    //For IIngameMenuActions
    public event Action closeMenuEventPerformed;

    //For IBeforeAfterGameMenuActions
    public event Action skipEventPerformed;
    public event Action continueEventPerformed;

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
            openMenuEventPerformed?.Invoke(); //Debug.Log("OnOpenMenu: Player: " + (input.Player.enabled ? "ENABLED" : "DISABLED") + " UI: " + (input.UI.enabled ? "ENABLED" : "DISABLED"));
        }
    }

    public void OnCloseMenu(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            closeMenuEventPerformed?.Invoke(); //Debug.Log("OnOpenMenu: Player: " + (input.Player.enabled ? "ENABLED" : "DISABLED") + " UI: " + (input.UI.enabled ? "ENABLED" : "DISABLED"));
        }
    }

    public void OnNavigateMenu(InputAction.CallbackContext pContext) {}
    
    public void OnSkip(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            skipEventPerformed?.Invoke();
        }
    }

    public void OnContinue(InputAction.CallbackContext pContext)
    {
        if (pContext.phase == InputActionPhase.Performed)
        {
            continueEventPerformed?.Invoke();
        }
    }

    public void SetEnabledActionMap(bool pEnablePlayer, bool pEnableIngameMenu, bool pEnableBeforeAfterGameMenu)
    {
        if (pEnablePlayer)
        {
            input.Player.Enable();
        } else
        {
            input.Player.Disable();
        }

        if (pEnableIngameMenu)
        {
            input.IngameMenu.Enable();
        }
        else
        {
            input.IngameMenu.Disable();
        }

        if (pEnableBeforeAfterGameMenu)
        {
            input.BeforeAfterGameMenu.Enable();
        }
        else
        {
            input.BeforeAfterGameMenu.Disable();
        }
    } 

    private void OnEnable()
    {
        if (input == null)
        {
            input = new CustomPlayerInput();
            input.Player.SetCallbacks(this);
            input.IngameMenu.SetCallbacks(this);
            input.BeforeAfterGameMenu.SetCallbacks(this);
        }

        input.Player.Disable();
        input.IngameMenu.Disable();
        input.BeforeAfterGameMenu.Enable();
    }

    private void OnDisable()
    {
        if (input != null)
        {
            input.Player.Disable();
            input.IngameMenu.Disable();
            input.BeforeAfterGameMenu.Disable();
        }
    }
}
