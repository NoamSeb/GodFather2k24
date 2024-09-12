using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;

public class ThemeUI : MonoBehaviour
{
    [SerializeField] private JokeThemeSO jokeThemeSO;
    [SerializeField] private string joke;

    [SerializeField] private ShowJoke showJokeScriptP1;
    [SerializeField] private ShowJoke showJokeScriptP2;
    [SerializeField] private int playerId;

    private void Start()
    {
        showJokeScriptP1.gameObject.SetActive(false);
        showJokeScriptP2.gameObject.SetActive(false);
    }

    [Button]
    private void test()
    {
        ShowJoke(jokeThemeSO, joke, playerId);
    }

    public void ShowJoke(JokeThemeSO jokeTheme, string joke, int playerIndex)
    {

        if(playerIndex == 0)
        {
            showJokeScriptP1.gameObject.SetActive(true);
            showJokeScriptP2.gameObject.SetActive(false);

            showJokeScriptP1.DisplayPlayerJoke(jokeTheme, joke);

        }
        else if(playerIndex == 1) { 
            showJokeScriptP1.gameObject.SetActive(false);
            showJokeScriptP2.gameObject.SetActive(true);

            showJokeScriptP2.DisplayPlayerJoke(jokeTheme, joke);
        }
    }
}
