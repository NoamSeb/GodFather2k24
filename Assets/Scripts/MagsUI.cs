using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class MagsUI : MonoBehaviour
{
    [SerializeField] private BulletManager player1Mag;
    [SerializeField] private BulletManager player2Mag;
    [SerializeField] private int playerID;

    [Button]
    //TODO add the real int playerIndex as params
    private void shootTrigger()
    {

        if (playerID == 0)
        {
            player1Mag.Shoot();
        }
        else if (playerID == 1)
        {
            player2Mag.Shoot();
        }
    }
}
