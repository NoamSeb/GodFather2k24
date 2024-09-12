using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using ScriptableObjects;
using UnityEngine;

public class ThemeUI : MonoBehaviour
{

    [SerializeField] private ShowJoke showJokeScriptP1;
    [SerializeField] private ShowJoke showJokeScriptP2;

    public void ShowJoke(JokeThemeSO jokeTheme, string joke, int playerIndex, string accessory = "Pas d'accessoire !", string intonation = "Pas d'intonation !")
    {
        if(playerIndex == 0)
        {
            showJokeScriptP1.gameObject.SetActive(true);
            showJokeScriptP2.gameObject.SetActive(false);
            
            showJokeScriptP1.DisplayPlayerJoke(jokeTheme, joke, accessory, intonation);

        }
        else if(playerIndex == 1) { 
            showJokeScriptP1.gameObject.SetActive(false);
            showJokeScriptP2.gameObject.SetActive(true);

            showJokeScriptP2.DisplayPlayerJoke(jokeTheme, joke, accessory, intonation);
        }
        else
        {
            Debug.Log("playerIndex is : " + playerIndex);
        }
    }
}
