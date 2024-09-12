using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class BonusBubbleBehaviour : MonoBehaviour
{
    [SerializeField] private float _speedDefault;
    [SerializeField] private float _sizeMultiplier = 0.5f;
    [SerializeField] private Sprite[] _spriteList;
    [SerializeField] private SpriteRenderer _iconSprite;
    [SerializeField] private GameObject[] _resizedParticles;
    [SerializeField] private TrailRenderer _cloudTrail;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private BonusType _bonusType;
    private float _speedMultiplier;

    public BonusType GetBonusType() => _bonusType;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        transform.localScale *= _sizeMultiplier;
        GetComponent<CapsuleCollider2D>().size *= _sizeMultiplier;
        _cloudTrail.widthMultiplier *= transform.localScale.x;
        foreach (GameObject resizedParticle in _resizedParticles)
        {
            resizedParticle.transform.localScale = transform.localScale;
        }
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
