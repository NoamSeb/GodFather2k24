using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput _playerInput;
    private ScopeController _scopeController;
    private GameManager _gameManager;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            Debug.Log(player.gameObject.name);
            ScopeController scopeController = player.GetComponent<ScopeController>();
            if (scopeController.GetPlayerIndex() == _playerInput.playerIndex)
            {
                _scopeController = scopeController;
            }
        }
        if (_scopeController == null)
        {
            throw new NullReferenceException("No Scope Controller where found for player index : " + _playerInput.playerIndex);
        }
        _gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
    }

    public void OnMove(InputAction.CallbackContext context) {_scopeController.ReadMoveInput(context);}
    
    public void OnShoot(InputAction.CallbackContext context) {_scopeController.ReadShootInput(context);}
    public void OnOK(InputAction.CallbackContext context) {_scopeController.ReadOKInput(context);}
    public void OnReload(InputAction.CallbackContext context) {_gameManager.TryReload(context);}

}
