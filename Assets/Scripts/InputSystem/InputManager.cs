using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class InputManager : MonoBehaviour
{
    public bool IsGamePad = true;

    private InputActions _inputActions;
    private InputActions.PlayerActions _playerActions;
    private InputActions.GlobalInventoryActions _globalInventoryActions;
    
    //player
    public event EventHandler OnJumpStarted;
    public event EventHandler OnJumpPerformed;
    public event EventHandler OnJumpCanceled;
    public event EventHandler OnDashAction;
    public event EventHandler OnSprintStarted;
    public event EventHandler OnSprintCanceled;
    public event EventHandler OnRespawnStarted;
    public event EventHandler OnRespawnCanceled;
    public event EventHandler OnCrouchPerformed;
    public event EventHandler OnAttackStarted;
    public event EventHandler OnAttackCanceled;
    public event EventHandler OnBlockStarted;
    public event EventHandler OnBlockCanceled;
    public event EventHandler OnWeaponChange;
    public event EventHandler OnPausePerfomed;
    public event EventHandler OnGlobalInterfaceOpen;
    public event EventHandler OnKeyBoard;
    public event EventHandler OnGamePad;
    public event EventHandler OnUsePerfomed;
    public event EventHandler OnHintPerfomed;

    //inventory
    public event EventHandler OnInvBack;
    public event EventHandler OnInvLeft;
    public event EventHandler OnInvRight;

    //player
    private void JumpOnStart(InputAction.CallbackContext obj) 
        => OnJumpStarted?.Invoke(this, EventArgs.Empty);
    private void JumpOnPerform(InputAction.CallbackContext obj) 
        => OnJumpPerformed?.Invoke(this, EventArgs.Empty);
    private void JumpOnCancel(InputAction.CallbackContext obj) 
        => OnJumpCanceled?.Invoke(this, EventArgs.Empty);
    private void DashOnPerformed(InputAction.CallbackContext obj) 
        => OnDashAction?.Invoke(this, EventArgs.Empty);
    private void SprintOnStart(InputAction.CallbackContext obj) 
        => OnSprintStarted?.Invoke(this, EventArgs.Empty);
    private void SprintOnCancel(InputAction.CallbackContext obj) 
        => OnSprintCanceled?.Invoke(this, EventArgs.Empty);
    private void RespawnOnStart(InputAction.CallbackContext obj)
        => OnRespawnStarted?.Invoke(this, EventArgs.Empty);
    private void RespawnOnCancel(InputAction.CallbackContext obj)
        => OnRespawnCanceled?.Invoke(this, EventArgs.Empty);
    private void CrouchOnPerform(InputAction.CallbackContext obj) 
        => OnCrouchPerformed?.Invoke(this, EventArgs.Empty);
    private void AttackOnCancel(InputAction.CallbackContext obj)
        => OnAttackCanceled?.Invoke(this, EventArgs.Empty);
    private void AttackOnStart(InputAction.CallbackContext obj)
       => OnAttackStarted?.Invoke(this, EventArgs.Empty);
    private void BlockOnCancel(InputAction.CallbackContext obj)
        => OnBlockCanceled?.Invoke(this, EventArgs.Empty);
    private void BlockOnStart(InputAction.CallbackContext obj)
       => OnBlockStarted?.Invoke(this, EventArgs.Empty);
    private void WeaponOnChange(InputAction.CallbackContext obj)
       => OnWeaponChange?.Invoke(this, EventArgs.Empty);
    private void PauseOnPerfomed(InputAction.CallbackContext obj)
      => OnPausePerfomed?.Invoke(this, EventArgs.Empty);
    private void HintOnPerfomed(InputAction.CallbackContext obj)
      => OnHintPerfomed?.Invoke(this, EventArgs.Empty);
    //inventory
    private void InvOnOpen(InputAction.CallbackContext obj)
       => OnGlobalInterfaceOpen?.Invoke(this, EventArgs.Empty);

    private void InvBackOnPerfomed(InputAction.CallbackContext obj)
       => OnInvBack?.Invoke(this, EventArgs.Empty);
    private void InvLeftOnPerfomed(InputAction.CallbackContext obj)
       => OnInvLeft?.Invoke(this, EventArgs.Empty);
    private void InvRightOnPerfomed(InputAction.CallbackContext obj)
       => OnInvRight?.Invoke(this, EventArgs.Empty);

    private void KeyBoardPerfomed(InputAction.CallbackContext obj)
    {
        IsGamePad = false;
        OnKeyBoard?.Invoke(this, EventArgs.Empty);
    }

    private void GamePadPerfomed(InputAction.CallbackContext obj)
    {
        IsGamePad = true;
        OnGamePad?.Invoke(this, EventArgs.Empty);
    }
    private void UseOnPerfomed(InputAction.CallbackContext obj)
       => OnUsePerfomed?.Invoke(this, EventArgs.Empty);


    private void SubscribeOnEvents()
    {
        _playerActions.Jump.started += JumpOnStart;
        _playerActions.Jump.performed += JumpOnPerform;
        _playerActions.Jump.canceled += JumpOnCancel;
        _playerActions.Dash.performed += DashOnPerformed;
        _playerActions.Sprint.started += SprintOnStart;
        _playerActions.Sprint.canceled += SprintOnCancel;
        _playerActions.Crouch.performed += CrouchOnPerform;

        _playerActions.Attack.performed += AttackOnStart;
        _playerActions.Attack.canceled += AttackOnCancel;
        _playerActions.Block.performed += BlockOnStart;
        _playerActions.Block.canceled += BlockOnCancel;
        _playerActions.ChangeWeapon.performed += WeaponOnChange;

        _playerActions.Pause.performed += PauseOnPerfomed;
        _playerActions.Respawn.started += RespawnOnStart;
        _playerActions.Respawn.canceled += RespawnOnCancel;
        _playerActions.GlobalInterface.performed += InvOnOpen;
        _playerActions.Hints.performed += HintOnPerfomed;

        _playerActions.IsKeyBoard.performed += KeyBoardPerfomed;
        _playerActions.OnGamePad.performed += GamePadPerfomed;

        _globalInventoryActions.Back.performed += InvBackOnPerfomed;
        _globalInventoryActions.Right.performed += InvRightOnPerfomed;
        _globalInventoryActions.Left.performed += InvLeftOnPerfomed;

        _playerActions.Use.performed += UseOnPerfomed;
    }

    private void UnsubscribeFromEvents()
    {
        _playerActions.Jump.started -= JumpOnStart;
        _playerActions.Jump.canceled -= JumpOnCancel;
        _playerActions.Dash.performed -= DashOnPerformed;
        _playerActions.Sprint.started -= SprintOnStart;
        _playerActions.Sprint.canceled -= SprintOnCancel;
        _playerActions.Attack.canceled -= AttackOnCancel;
        _playerActions.Crouch.performed -= CrouchOnPerform;

        _playerActions.Attack.performed -= AttackOnStart;
        _playerActions.Block.performed -= BlockOnStart;
        _playerActions.Block.canceled -= BlockOnCancel;

        _playerActions.Pause.performed -= PauseOnPerfomed;
        _playerActions.Respawn.started -= RespawnOnStart;
        _playerActions.Respawn.canceled -= RespawnOnCancel;
        _playerActions.GlobalInterface.performed -= InvOnOpen;
        _playerActions.Hints.performed -= HintOnPerfomed;

        _playerActions.IsKeyBoard.performed -= KeyBoardPerfomed;
        _playerActions.OnGamePad.performed -= GamePadPerfomed;

        _globalInventoryActions.Back.performed -= InvBackOnPerfomed;
        _globalInventoryActions.Right.performed -= InvRightOnPerfomed;
        _globalInventoryActions.Left.performed -= InvLeftOnPerfomed;

        _playerActions.Use.performed -= UseOnPerfomed;
    }

    public Vector2 GetPlayerMovementVector()
    {
        return _playerActions.Movement.ReadValue<Vector2>().normalized;
    }

    
    private void OnEnable()
    {
        _inputActions = new InputActions();
        _playerActions = _inputActions.Player;
        _globalInventoryActions = _inputActions.GlobalInventory;
        SetPlayerActions();
        SubscribeOnEvents();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
        _globalInventoryActions.Disable();
        UnsubscribeFromEvents();
        _inputActions = null;
    }


    public void SetPlayerActions()
    {
        _playerActions.Enable();
        _globalInventoryActions.Disable();
    }
}
