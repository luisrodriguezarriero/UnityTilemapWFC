using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

namespace SnakeGame
{
    public class Food : MonoBehaviour
    {

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                FindObjectOfType<GameManager>().RemoveFood(this);
                Destroy(this);
            }
        }
    }
}
