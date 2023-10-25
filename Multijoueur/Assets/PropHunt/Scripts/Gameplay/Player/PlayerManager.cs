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
    [SerializeField] private Menu _mainMenu;

    private bool _mainMenuIsDispay = false;
    //public NetworkVariable<bool> isHunter;

    public ActionInput _actionInput;
    public Animator _animator;
    [SerializeField] PropController _propController;
    //[SerializeField] HunterController _hunterController;

    private AlignPlayersOnStartLine _spawnPoint;

    private Vector3 _spawnPointPosition;
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

        _spawnPointPosition = Vector3.zero;

        if (_actionInput == null)
        {
            _actionInput = GetComponent<ActionInput>();
        }
        if (Camera == null) Camera = GetComponentInChildren<Camera>(true);
    }

    private void Update()
    {
        _spawnPoint = FindAnyObjectByType<AlignPlayersOnStartLine>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        transform.position = new Vector3(0, 100f, 0);
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
        } else
        {
            DisplayGlobalMenu();
        }
    }

    public void DisplayGlobalMenu()
    {
        ToggleCursorLock();
        _mainMenu.gameObject.SetActive(true);
        _mainMenuIsDispay = true;
    }

    public void HideGlobalMenu()
    {
        ToggleCursorLock();
        _mainMenu.gameObject.SetActive(false);
        _mainMenuIsDispay = false;
    }

    public void CheckPointUpdate(int checkPointNum,Vector3 position)
    {
        lastSpawnPoint = position;
        checkpoint = checkPointNum;
    }

    public void SpawnPoint(Vector3 _spawnPoint)
    {
        transform.position = _spawnPoint;
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

    public void LauchParty()
    {
        if (IsHost)
        {
            Debug.Log("Host clicked LaunchButton");

            // Trigger an RPC or network event to start the game on all clients.
            // Example: MyNetworkedGameController.StartGame();

            // If you have a game controller that starts the game, call it here.
            NetworkManager.Singleton.SceneManager.LoadScene("Tests", UnityEngine.SceneManagement.LoadSceneMode.Single);
            PartyManager.LaunchParty();
        }
    }

    public void TestSpawn()
    {
        //_spawnPoint.AlignPlayers();
    }
}
