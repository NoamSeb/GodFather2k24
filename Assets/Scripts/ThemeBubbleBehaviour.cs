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
    [SerializeField] private GameObject[] _resizedParticles;
    [SerializeField] private TrailRenderer _cloudTrail;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private GameObject[] _toDisableWhenExplode;


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
        transform.localScale *= jokeThemeSo.SizeMultiplier;
        GetComponent<CapsuleCollider2D>().size *= jokeThemeSo.SizeMultiplier;
        _cloudTrail.widthMultiplier *= transform.localScale.x;
        foreach (GameObject resizedParticle in _resizedParticles) 
        {
            resizedParticle.transform.localScale = transform.localScale;
        }
    }

    private void FixedUpdate()
    {
        _rb.velocity = _direction * (_speedDefault * _speedMultiplier);
    }

    public void Capture()
    {
        Debug.Log("Thought bubble of theme " + _jokeThemeSo.Theme + " was captured !");
        foreach (GameObject toDisable in _toDisableWhenExplode)
        {
            toDisable.SetActive(false);
        }
        _explosion.SetActive(true);
        Destroy(gameObject,1);
    }

}
