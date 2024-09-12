using System;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    [SerializeField] private GameObject _playerMag;

    // List to track the state of bullets (active/inactive)
    private List<string> _MagsBullets = new List<string>();

    private void Start()
    {
        Reload();
    }

    public void Shoot()
    {
        Debug.Log("Decreasing in UI");
        if (_MagsBullets.Count > 0)
        {
  
            string bulletName = _MagsBullets[0];
            _MagsBullets.RemoveAt(0); 

            
            Transform bulletToDeactivate = _playerMag.transform.Find(bulletName);
            if (bulletToDeactivate != null)
            {
                bulletToDeactivate.gameObject.SetActive(false); 
            }
            else
            {
                Debug.LogError("Bullet not found: " + bulletName);
            }
        }
    }
    public void Reload()
    {
        // Get all child transforms of _playerMag
        Transform[] bulletList = _playerMag.GetComponentsInChildren<Transform>(true);

        // Add each bullet's name to _MagsBullets to manage its state
        foreach (var bullet in bulletList)
        {
            if (bullet.gameObject != _playerMag)
            {
                bullet.gameObject.SetActive(true);
                _MagsBullets.Add(bullet.gameObject.name);
            }
        }
    }   
}
