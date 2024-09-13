using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedIcon : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _appearScale;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _scaleSpeed;
    [SerializeField] private Vector2[] _targetPositions;
    private float _currScale;
    private int _playerIndex;
    private Vector3 _initialPos;
    private bool _isMoving;
    private float _moveProgress;
    private ScopeController _scopeController;


    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _currScale = 0;
        _moveProgress = 0;
        _initialPos = transform.position;
    }

    public void Setup(ScopeController scopeController, Sprite sprite)
    {
        _playerIndex = scopeController.GetPlayerIndex();
        _spriteRenderer.sprite = sprite;
        _scopeController = scopeController;
    } 

    private void Start()
    {
        StartCoroutine(Appear());
    }

    private IEnumerator Appear()
    {
        while (transform.localScale.x < _appearScale)
        {
            transform.localScale = Vector3.one * _currScale;
            _currScale += 0.01f * _scaleSpeed;
            yield return new WaitForSeconds(0.01f);
        }
        
        _isMoving = true;
    }

    private void FixedUpdate()
    {
        if (!_isMoving) return;
        transform.position = Vector3.Lerp(_initialPos, _targetPositions[_playerIndex], _moveProgress);
        transform.localScale = Vector3.Lerp( Vector3.one * _appearScale, Vector3.zero, _moveProgress);
        _moveProgress += Time.deltaTime * _moveSpeed;
        if (_moveProgress>=1)
        {
            _scopeController.UpdateScore();
            Destroy(gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_targetPositions[0],0.3f);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_targetPositions[1],0.3f);
    }
}
