using PropHunt.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : NetworkBehaviour
{
    private Collider _playerCollider;
    private LayerMask _excludeLayer;
    protected MovementController _movementController;
    public Camera Camera;
    protected ClassController _currentController;
    [SerializeField] Menu _mainMenu;

    private bool _mainMenuIsDispay = false;
    //public NetworkVariable<bool> isHunter;

    public ActionInput _actionInput;
    public Animator _animator;
    [SerializeField] PropController _propController;
    //[SerializeField] HunterController _hunterController;

    private Vector3 lastSpawnPoint;
    private int checkpoint = 0;

    private void Awake()
    {
        _playerCollider = GetComponentInChildren<Collider>();
        _excludeLayer = LayerMask.NameToLayer("Player");
        _movementController = GetComponent<MovementController>();
        _propController = GetComponentInChildren<PropController>();
        _movementController.ClassController = _propController;
        _actionInput.SetClassInput(_propController.ClassInput);
        //_propController.Activate();
        /*isHunter.OnValueChanged += SwapTeam;
        if (_propController == null)
        {
            *//*
        }
        if(_hunterController == null)
        {
            _hunterController = GetComponentInChildren<HunterController>();
        }*/
        if (_actionInput == null)
        {
            _actionInput = GetComponent<ActionInput>();
        }
        if (Camera == null) Camera = GetComponentInChildren<Camera>(true);
    }
    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        //SwapTeam(true, false);
        if (IsOwner)
        {
            GetComponent<PlayerInput>().enabled = true;
            GetComponent<AudioListener>().enabled = true;
            _movementController.enabled = true;
            Camera.gameObject.SetActive(true);
            _movementController.SetAnimator(GetComponent<Animator>());
            return;
        }
        Camera.gameObject.SetActive(false);
    }

    /*[ServerRpc]
    public void SwapTeamServerRPC()
    {
        isHunter.Value = !isHunter.Value;
    }

    public void SwapTeam(bool previousIsHunterValue, bool newIsHunterValue)
    {
        if (newIsHunterValue)
        {
            _movementController.ClassController = _hunterController;
            _actionInput.SetClassInput(_hunterController.ClassInput);
            _propController.Deactivate();
            _hunterController.Activate();
            return;
        }
        _movementController.ClassController = _propController;
        _actionInput.SetClassInput(_propController.ClassInput);
        _hunterController.Deactivate();
        _propController.Activate();
    }*/

    public void ToggleCursorLock()
    {
        bool isLocked = !_movementController.cursorLocked;
        Cursor.lockState = isLocked? CursorLockMode.Locked : CursorLockMode.None;
        _movementController.cursorLocked = isLocked;
    }

    public void ActionMenu()
    {
        if (_mainMenuIsDispay)
        {
            HideGlobalMenu();
            _mainMenuIsDispay = false;
        } else
        {
            DisplayGlobalMenu();
            _mainMenuIsDispay = true;
        }
    }

    public void DisplayGlobalMenu()
    {
        /*bool isLocked = !_movementController.cursorLocked;
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        _movementController.cursorLocked = isLocked;*/
        ToggleCursorLock();
        _mainMenu.gameObject.SetActive(true);
    }

    public void HideGlobalMenu()
    {
        /*bool isLocked = !_movementController.cursorLocked;
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        _movementController.cursorLocked = isLocked;*/
        ToggleCursorLock();
        _mainMenu.gameObject.SetActive(false);
    }

    public void CheckPointUpdate(int checkPointNum,Vector3 position)
    {
        lastSpawnPoint = position;
        checkpoint = checkPointNum;
    }

    public void Respawn()
    {
        transform.position = lastSpawnPoint;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }

    public void ActivateCollider()
    {
        _playerCollider.excludeLayers = new LayerMask();
    }

    public void DeactivateCollider()
    {
        _playerCollider.excludeLayers = _excludeLayer;
        
    }

    public int GetActualCheckPoint()
    {
        return checkpoint;
    }
}
