﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoxDestroyScript : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Rain")
        {
            Destroy(gameObject);
        }
    }
}
