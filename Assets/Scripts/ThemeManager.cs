using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private List<string> _jokeThemes;
    private List<string> _currJokeThemes;

    private void Awake()
    {
        _currJokeThemes = new List<string>();
        foreach (string jTheme in _jokeThemes)
        {
            _currJokeThemes.Add(jTheme);
        }
    }

    [Button]
    private void GetThemeTest()
    {
        string testTheme = GetTheme();
        Debug.Log("theme: " + testTheme);
    }
    
    public string GetTheme()
    {
        string jokeTheme = _currJokeThemes[Random.Range(0, _currJokeThemes.Count)];
        _currJokeThemes.Remove(jokeTheme);
        if (_currJokeThemes.Count <= 0)
        {
            foreach (string jTheme in _jokeThemes)
            {
                _currJokeThemes.Add(jTheme);
            }
        }
        return jokeTheme;
    }
}