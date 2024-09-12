using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    [SerializeField] private GameUI _gameUI;
    private float _clock;
    private float _timer = -1;

    public void StartTimer(float timer)
    {
        _clock = 0;
        _timer = timer;
    }

    void Update()
    {
        if (_timer == -1) return;
        _clock += Time.deltaTime;
        _gameUI.UpdateTimer(_clock/_timer);
        if (_clock >= _timer)
        {
            _gameUI.EndTimer();
            this.enabled = false;
        }
    }

}