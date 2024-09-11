using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

public class ThemeBubbleBehaviour : MonoBehaviour
{
    [SerializeField] private float _speedDefault;
    [SerializeField] private SpriteRenderer _iconSprite;
    private Vector2 _direction;
    private Rigidbody2D _rb;
    private JokeThemeSO _jokeThemeSo;
    private float _speedMultiplier;

    public JokeThemeSO GetThemeSo() => _jokeThemeSo;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetupBubble(Vector2 direction, JokeThemeSO jokeThemeSo)
    {
        _jokeThemeSo = jokeThemeSo;
        _iconSprite.sprite = jokeThemeSo.ThemeSprite;
        _speedMultiplier = jokeThemeSo.BubbleSpeedMulti;
        _direction = direction;
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * (_speedDefault * _speedMultiplier);
    }
    
}
