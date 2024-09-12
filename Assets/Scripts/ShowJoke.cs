using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class ShowJoke : MonoBehaviour
{
 
    [SerializeField] private TextMeshProUGUI JokeThemeText;
    [SerializeField] private TextMeshProUGUI Joke;
    [SerializeField] private TextMeshProUGUI accessory1;
    [SerializeField] private TextMeshProUGUI accessory2;


    public void DisplayPlayerJoke(JokeThemeSO jokeTheme, string joke)
    {
        //TODO: get JokeThemeSO from _capturedThemes

        JokeThemeText.text = jokeTheme.Theme;
        Joke.text = joke;
        accessory1.text = "Barbe Blanche";
        accessory2.text = "Accent du nord";

    }
}
