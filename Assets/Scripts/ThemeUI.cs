using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using TMPro;
using UnityEngine;

public class ThemeUI : MonoBehaviour
{
    [SerializeField] private RandomManager _randomManager;
    [SerializeField] private TextMeshProUGUI _themeText;

    /*public void ShowTheme()
    {
        JokeThemeSO theme = _randomManager.Get();
        Debug.Log(theme);
        _themeText.text = theme.Theme;
    }*/
}
