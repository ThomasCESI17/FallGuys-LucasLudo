using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class ActionInput : MonoBehaviour
{
    [Header("Player Input Values")]
    public Vector2 move;
    public Vector2 look;
    public bool jump;

    [Header("Mouse Cursor Settings")]
    public bool cursorLocked = true;

    [Header("Properties")]
    [SerializeField] private PlayerManager _playerManager;
    private ClassInput _currentClassInput;

    private void Awake()
    {
        _playerManager = GetComponent<PlayerManager>();
    }

    public void OnMove(CallbackContext context)
    {
        move = context.ReadValue<Vector2>();
    }

    public void OnLook(CallbackContext context)
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            look = context.ReadValue<Vector2>();
        }
    }
    public void OnJump(CallbackContext context)
    {
        if (!context.performed) return;
        jump = true;
    }

    public void SetClassInput(ClassInput _classInput)
    {
        _currentClassInput = _classInput;
    }

    public void OnMenuTrigger(CallbackContext context)
    {
        if (!context.performed) return;
        //_playerManager.TriggerMenu();
    }

    public void OnCursorLockToggle(CallbackContext context)
    {
        if (!context.performed) return;
        _playerManager.ToggleCursorLock();
    }

    public void OnMenuOpenClose (CallbackContext context)
    {
        if (!context.performed) return;
        _playerManager.ActionMenu();
    }

    public void OnPartyLaunch(CallbackContext context)
    {
        if (!context.performed) return;
        _playerManager.LauchParty();
    }

    public void OnSpawn(CallbackContext context)
    {
        if (!context.performed) return;
        _playerManager.TestSpawn();
    }

}
