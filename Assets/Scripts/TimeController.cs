using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float m_remainingTime;

    void Update()
    {
        UpdateTimer();
        CheckRemainingTime();
    }

    /// <summary>
    /// UpdateTimer function manage a cooldown from given value to 0
    /// </summary>
    void UpdateTimer()
    {
        if (m_remainingTime > 0)
        {
            m_remainingTime -= Time.deltaTime;
        }
        else if (m_remainingTime < 0)
        {
            m_remainingTime = 0;
            timerText.color = Color.red;
        }

        int minutes = Mathf.FloorToInt(m_remainingTime / 60);
        int seconds = Mathf.FloorToInt(m_remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void CheckRemainingTime()
    {
        if (m_remainingTime == 0)
        {
            Debug.Log("GAME OVER !");
        }
    }
}