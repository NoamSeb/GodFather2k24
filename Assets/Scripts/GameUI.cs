using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private BulletManager[] _magPlayers;
    [SerializeField] private TextMeshProUGUI[] _scoreTexts;
    [SerializeField] private Transform _timerClockHand;
    [SerializeField] private Image _fillTimer;

    public void Shooting(int playerIndex) => _magPlayers[playerIndex].Shoot();
    public void Reload(int playerIndex) => _magPlayers[playerIndex].Reload();

    public void UpdateScoreUI(int playerIndex, int value) => _scoreTexts[playerIndex].text = value.ToString();

    public void UpdateTimer(float completionRate)
    {
        Quaternion rotation = _timerClockHand.localRotation;
        _timerClockHand.localRotation = Quaternion.Euler(rotation.x, rotation.y, completionRate * -360);
        _fillTimer.fillAmount = completionRate;
    }

    public void EndTimer()
    {
        _fillTimer.gameObject.SetActive(false);
        _timerClockHand.gameObject.SetActive(false);
    }
}
