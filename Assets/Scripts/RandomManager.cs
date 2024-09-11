using System;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomManager : MonoBehaviour
{
    [SerializeField,Expandable] private List<JokeThemeElement> _jokeThemes;
    private List<JokeThemeSO> _currJokeThemes;
    private List<string> _usedJokes;
    

    private void Awake()
    {
        _currJokeThemes = new List<JokeThemeSO>();
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

[Serializable]
public struct JokeThemeElement
{
    [SerializeField] private JokeThemeSO jokeThemeSo;
    [SerializeField] private int minAppear;
    [SerializeField] private int maxAppear;

    public JokeThemeSO JokeThemeSo => jokeThemeSo;
    public int MaxAppear => maxAppear;
    public int MinAppear => minAppear;
}