using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomManager : MonoBehaviour
{
    [SerializeField] private List<JokeThemeElement> _jokeThemeElements;
    private List<string> _usedJokes;
    

    private void Awake()
    {
        _usedJokes = new List<string>();
    }
    
    public List<JokeThemeSO> GetThemeList()
    {
        List<JokeThemeSO> jokeThemeList = new List<JokeThemeSO>();
        foreach (JokeThemeElement jokeThemeEl in _jokeThemeElements)
        {
            for (int i = 0; i < Random.Range(jokeThemeEl.MinAppear,jokeThemeEl.MaxAppear); i++)
            {
                jokeThemeList.Add(jokeThemeEl.JokeThemeSo);
            }
        }

        List<JokeThemeSO> temp = new List<JokeThemeSO>(0);
        int nb = jokeThemeList.Count;
        for (int i = 0; i < nb; i++)
        {
            int index = Random.Range(0, jokeThemeList.Count);
            temp.Add(jokeThemeList[index]);
            jokeThemeList.RemoveAt(index);
            
        }
        return temp;
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