using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField, Foldout("References")] private BubbleManager _bubbleManager;
    [SerializeField, Foldout("References")] private RandomManager _randomManager;
    [SerializeField, Foldout("References")] private GameObject _phase2Canvas;
    [SerializeField, Foldout("References")] private ThemeUI _themeUI;
    [SerializeField] private string[] _emptySentences;
    [SerializeField] private float _phase1Timer = 30f;
    
    private int _gamePhase;
    private ScopeController[] _players;
    private List<JokeThemeSO>[] _capturedJokes;
    private List<BonusType>[] _bonusTypesP1;
    private List<BonusType>[] _bonusTypesP2;

    private int[] _themeIndex;
    private int _currPlayerIndexJoke;
    public int GamePhase => _gamePhase;

    private void Awake()
    {
        _players = new ScopeController[2];
        _themeIndex = new [] { 0, 0 };
        _capturedJokes = new List<JokeThemeSO>[2];
        _currPlayerIndexJoke = -1;
    }

    private void Start()
    {
        StartGame();
    }

    [Button]
    public void StartGame()
    {
        _phase2Canvas.SetActive(false);
        _gamePhase = 1;
        _bubbleManager.StartBubbles(_phase1Timer, this);
    }

    public void SetPlayer(ScopeController scopeController, int playerIndex) { _players[playerIndex] = scopeController; }

    public void StartPhase2()
    {
        _gamePhase = 2;
        _phase2Canvas.SetActive(true);
        _capturedJokes[0] = _players[0].GetCapturedJokes();
        _capturedJokes[1] = _players[1].GetCapturedJokes();
        _currPlayerIndexJoke = _players[0].GetPlayerScore() < _players[1].GetPlayerScore() ? 1 : _players[0].GetPlayerScore() == _players[1].GetPlayerScore() ? _currPlayerIndexJoke : 0;
        _bonusTypesP1 = new List<BonusType>[_capturedJokes[0].Count];
        _bonusTypesP2 = new List<BonusType>[_capturedJokes[1].Count];
        
        //Define bonusType lists
        for (int i = 0; i < _bonusTypesP1.Length; i++)
        {
            _bonusTypesP1[i] = new List<BonusType>();
        }
        for (int i = 0; i < _bonusTypesP2.Length; i++)
        {
            _bonusTypesP2[i] = new List<BonusType>();
        }
        //Player 1 Bonus
        int bCount = 0;
        foreach (BonusType capturedBonus in _players[0].GetCapturedBonus())
        {
            int r = Random.Range(0, _bonusTypesP1.Length);
            while (_bonusTypesP1[r].Contains(capturedBonus))
            {
                r = Random.Range(0, _bonusTypesP1.Length);
            }
            _bonusTypesP1[r].Add(capturedBonus);
            bCount++;
            if (bCount >= _bonusTypesP1.Length) break;
        }
        //Player 2 Bonus
        foreach (BonusType capturedBonus in _players[1].GetCapturedBonus())
        {
            int r = Random.Range(0, _bonusTypesP2.Length);
            while (_bonusTypesP2[r].Contains(capturedBonus))
            {
                r = Random.Range(0, _bonusTypesP2.Length);
            }
            _bonusTypesP2[r].Add(capturedBonus);
            if (bCount >= _bonusTypesP2.Length) break;
        }
        if (_capturedJokes[0].Count == 0 & _capturedJokes[1].Count == 0) //No Jokes were took
        {
            GameEnd();
            return;
        }
        JokeThemeSO jokeThemeSo = _capturedJokes[0][_themeIndex[0]];
        _randomManager.GetJokeFromTheme(jokeThemeSo);
        (string accessory, string intonation) = GetBonus();
        _themeUI.ShowJoke(jokeThemeSo, _randomManager.GetJokeFromTheme(jokeThemeSo), _currPlayerIndexJoke, accessory, intonation);
    }

    public void PassToNextJoke(int playerIndex)
    {
        if (_gamePhase != 2) return;
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
                    Debug.Log("All Jokes Used");
                    GameEnd();
                    return;
                }
            }
            
            JokeThemeSO jokeThemeSo = _capturedJokes[_currPlayerIndexJoke][_themeIndex[_currPlayerIndexJoke]];
            _randomManager.GetJokeFromTheme(jokeThemeSo);
            (string accessory, string intonation) = GetBonus();
            _themeUI.ShowJoke(jokeThemeSo, _randomManager.GetJokeFromTheme(jokeThemeSo), _currPlayerIndexJoke, accessory, intonation);
        }
    }

    private (string accessory, string intonation) GetBonus()
    {
        List<BonusType> bonusTypes;
        string accessory = _emptySentences[0];
        string intonation = _emptySentences[1];
        if (_currPlayerIndexJoke == 0)
        {
            bonusTypes = _bonusTypesP1[_themeIndex[0]];
        }
        else
        {
            bonusTypes = _bonusTypesP2[_themeIndex[1]];
        }

        
        if (bonusTypes.Count>0)
        {
            foreach (BonusType bonus in bonusTypes)
            {
                if (bonus == BonusType.Accessory)
                {
                    accessory = "Tu as un Accessoire !";
                }
                else
                {
                    intonation = _randomManager.GetRandomIntonation();
                }
            }
        }

        return (accessory, intonation);
    }

    private void GameEnd()
    {
        Debug.Log("GameOver");
        //EndPhase;
    }

    public void FirstCloudCaptured(int playerIndex)
    {
        if (_currPlayerIndexJoke == -1) _currPlayerIndexJoke = playerIndex;
    }
}
