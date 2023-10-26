using PropHunt.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : NetworkBehaviour
{
    private string _playerName;
    private Collider _playerCollider;
    private LayerMask _excludeLayer;
    protected MovementController _movementController;
    public Camera Camera;
    protected ClassController _currentController;
    [SerializeField] private Menu _mainMenu;

    public TextMeshProUGUI _textMesh;

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
        _playerName = "Player" + (PartyManager.GetListOfPlayer().Count + 1).ToString();
        _playerCollider = GetComponentInChildren<Collider>();
        _excludeLayer = LayerMask.NameToLayer("Player");
        _movementController = GetComponent<MovementController>();
        _propController = GetComponentInChildren<PropController>();
        _movementController.ClassController = _propController;
        _actionInput.SetClassInput(_propController.ClassInput);

        //OnNetworkSpawn();

        _spawnPointPosition = Vector3.zero;

        if (_actionInput == null)
        {
            _actionInput = GetComponent<ActionInput>();
        }
        if (Camera == null) Camera = GetComponentInChildren<Camera>(true);
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        Debug.Log("test");
        transform.position = new Vector3(0, 55f, 0);
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

    [ClientRpc]
    public void SpawnPointClientRPC(Vector3 _spawnPoint, bool IsStartPosition)
    {
        _movementController.BlockMoveAnimator();
        _movementController.enabled = false;
        if (IsStartPosition)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        transform.position = _spawnPoint;
        lastSpawnPoint = _spawnPoint;
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

            //NetworkManager.Singleton.SceneManager.LoadScene("Tests", UnityEngine.SceneManagement.LoadSceneMode.Single);
            PartyManager.LaunchParty();
        }
    }

    [ClientRpc]
    public void UpdateRankingClientRPC(string text)
    {
        _textMesh.text = text;
    }

    public string GetPlayerName()
    {
        return _playerName;
    }

    [ClientRpc]
    public void FreezeAnimationClientRPC()
    {
        _movementController.enabled = false;
    }

    [ClientRpc]
    public void UnFreezeAnimationClientRPC()
    {
        _movementController.enabled = true;
    }


}
