using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int _gamePhase;

    public void StartGame()
    {
        _gamePhase = 1;
    }
}
