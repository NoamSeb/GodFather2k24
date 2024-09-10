using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ThemeUI : MonoBehaviour
{
    [SerializeField] private ThemeManager _themeManager;
    [SerializeField] private TextMeshProUGUI _themeText;

    public void ShowTheme()
    {
        string theme = _themeManager.GetTheme();
        Debug.Log(theme);
        _themeText.text = theme;
    }
}
