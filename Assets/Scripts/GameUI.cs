using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private BulletManager[] _magPlayers;
    [SerializeField] private TextMeshProUGUI[] _scoreTexts;

    public void Shooting(int playerIndex) => _magPlayers[playerIndex].Shoot();
    public void Reload(int playerIndex) => _magPlayers[playerIndex].Reload();

    public void UpdateScoreUI(int playerIndex, int value) => _scoreTexts[playerIndex].text = value.ToString();
}
