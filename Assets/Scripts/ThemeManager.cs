using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class ThemeManager : MonoBehaviour
{
    [SerializeField] private List<JokeTheme> _jokeThemes;
    private List<JokeTheme> _currJokeThemes;

    private void Awake()
    {
        _currJokeThemes = new List<JokeTheme>();
        foreach (JokeTheme jTheme in _jokeThemes)
        {
            _currJokeThemes.Add(jTheme);
        }
    }

    [Button]
    private void GetThemeTest()
    {
        JokeTheme testTheme = GetTheme();
        Debug.Log("theme: " + testTheme.theme + ", multi: " + testTheme.multiplicator);
    }
    
    private JokeTheme GetTheme()
    {
        JokeTheme jokeTheme = _currJokeThemes[Random.Range(0, _currJokeThemes.Count)];
        _currJokeThemes.Remove(jokeTheme);
        if (_currJokeThemes.Count <= 0)
        {
            foreach (JokeTheme jTheme in _jokeThemes)
            {
                _currJokeThemes.Add(jTheme);
            }
        }
        return jokeTheme;
    }
}

[Serializable]
public struct JokeTheme
{
    [SerializeField] public string theme;
    [SerializeField] public int multiplicator;

    public string Theme => theme;
    public int Multiplicator => multiplicator;
}
