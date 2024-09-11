using System;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomManager : MonoBehaviour
{
    [SerializeField] private List<JokeThemeSO> _jokeThemes;
    private List<JokeThemeSO> _currJokeThemes;
    private List<string> _usedJokes;

    private void Awake()
    {
        _currJokeThemes = new List<JokeThemeSO>();
        foreach (JokeThemeSO jTheme in _jokeThemes)
        {
            _currJokeThemes.Add(jTheme);
        }
        _usedJokes = new List<string>();
    }

    [Button]
    private void GetThemeTest()
    {
        JokeThemeSO testTheme = GetTheme();
        Debug.Log("theme: " + testTheme.Theme);
    }
    
    public JokeThemeSO GetTheme()
    {
        JokeThemeSO jokeTheme = _currJokeThemes[Random.Range(0, _currJokeThemes.Count)];
        _currJokeThemes.Remove(jokeTheme);
        if (_currJokeThemes.Count <= 0)
        {
            foreach (JokeThemeSO jTheme in _jokeThemes)
            {
                _currJokeThemes.Add(jTheme);
            }
        }
        return jokeTheme;
    }

    public string GetJokeFromTheme(JokeThemeSO jokeThemeSo)
    {
        string joke = jokeThemeSo.Joke(_usedJokes);
        if (_usedJokes.Contains(joke))
        {
            Debug.Log("All " + jokeThemeSo.Theme + " jokes have been used");
            foreach (string jokeEl in jokeThemeSo.JokeList)
            {
                _usedJokes.Remove(jokeEl);
            }
        }

        _usedJokes.Remove(joke);
        return joke;
    }
}