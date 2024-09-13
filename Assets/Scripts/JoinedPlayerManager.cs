using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class JoinedPlayerManager : MonoBehaviour
{
    [SerializeField] private GameObject JoinCanvas;
    [SerializeField] private GameObject _EnterGameField;

    private bool _player1Joined;

    [Button]
    private void test()
    {
        playerAsJoined();
    }
    public void playerAsJoined()
    {
        if (_player1Joined)
        {
            JoinCanvas.SetActive(false);
        }
        
        _EnterGameField.SetActive(false);
        _player1Joined = true;
    }
}
