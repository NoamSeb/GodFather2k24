using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField, Foldout("References")] private BubbleManager _bubbleManager;
    [SerializeField, Foldout("References")] private RandomManager _randomManager;
    [SerializeField] private float _phase1Timer = 30f;
    
    private int _gamePhase;
    private ScopeController[] _players;
    private List<JokeThemeSO>[] _capturedJokes;

    private int[] _themeIndex;
    private int _currPlayerIndexJoke;
    public int GamePhase => _gamePhase;

    private void Awake()
    {
        _players = new ScopeController[2];
        _themeIndex = new [] { 0, 0 };
        _capturedJokes = new List<JokeThemeSO>[2]; 
    }
    
    [Button]
    public void StartGame()
    {
        _gamePhase = 1;
        _bubbleManager.StartBubbles(_phase1Timer, this);
    }

    public void SetPlayer(ScopeController scopeController, int playerIndex) { _players[playerIndex] = scopeController; }

    public void StartPhase2()
    {
        _gamePhase = 2;
        _capturedJokes[0] = _players[0].GetCapturedJokes();
        _capturedJokes[1] = _players[1].GetCapturedJokes();
        _currPlayerIndexJoke = 0;
        JokeThemeSO jokeThemeSo = _capturedJokes[0][_themeIndex[0]];
        _randomManager.GetJokeFromTheme(jokeThemeSo);
        //tonObj.ShowJoke(jokeThemeSo, _randomManager.GetJokeFromTheme(jokeThemeSo);, _currPlayerIndexJoke)
    }

    public void PassToNextJoke(int playerIndex)
    {
        //If Player that wants to pass to next Joke has current joke
        if (_currPlayerIndexJoke == playerIndex)
        {
            _themeIndex[playerIndex]++;
            _currPlayerIndexJoke = _currPlayerIndexJoke == 0 ? 1 : 0;
            
            //Has one player ran out of jokes
            if (_capturedJokes[_currPlayerIndexJoke].Count <= _themeIndex[_currPlayerIndexJoke])
            {
                Debug.Log("Player " + _currPlayerIndexJoke + " has run out of Jokes");
                _currPlayerIndexJoke = _currPlayerIndexJoke == 0 ? 1 : 0;
                if (_capturedJokes[_currPlayerIndexJoke].Count <= _themeIndex[_currPlayerIndexJoke])
                {
                    GameEnd();
                    return;
                }
            }
            
            JokeThemeSO jokeThemeSo = _capturedJokes[_currPlayerIndexJoke][_themeIndex[_currPlayerIndexJoke]];
            _randomManager.GetJokeFromTheme(jokeThemeSo);
            //tonObj.ShowJoke(jokeThemeSo, _randomManager.GetJokeFromTheme(jokeThemeSo);, _currPlayerIndexJoke)
        }
    }

    private void GameEnd()
    {
        //EndPhase;
    }
}
