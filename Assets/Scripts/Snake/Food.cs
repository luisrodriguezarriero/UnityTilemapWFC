
using UnityEngine;

namespace Snake
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
