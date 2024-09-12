using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThoughtScreen : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("ThoughtBubble") || other.gameObject.CompareTag("BonusBubble"))
        {
            Destroy(other.gameObject,1);
        }
    }
}
