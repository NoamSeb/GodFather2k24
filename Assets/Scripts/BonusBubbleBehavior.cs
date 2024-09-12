using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class BonusBubbleBehaviour : MonoBehaviour
{
    [SerializeField] private float _speedDefault;
    [SerializeField] private Sprite[] _spriteList;
    [SerializeField]  SpriteRenderer _iconSprite;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private BonusType _bonusType;
    private float _speedMultiplier;

    public BonusType GetBonusType() => _bonusType;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetupBubble(Vector2 direction, BonusType bonusType)
    {
        _bonusType = bonusType;
        _iconSprite.sprite = _spriteList[bonusType==BonusType.Accessory? 0 : 1];
        _direction = direction;
        _speedMultiplier = 1;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * (_speedDefault * _speedMultiplier);
    }

    public void Capture()
    {
        Debug.Log("Thought bubble of bonus " + _bonusType + " was captured !");
        Destroy(gameObject);
    }

}
