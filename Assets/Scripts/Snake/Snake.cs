using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace SnakeGame
{
    public class Snake : MonoBehaviour
    {
        private bool isMoving;
        private bool gameLose;
        [SerializeField] private bool ate = false;
        private Vector3 origPos, targetPos;
        [SerializeField] public GameObject segmentPrefab;
        public static float timeToMove = 0.4f;

        private Vector2 direction = Vector2.zero;
        public List<Transform> segments;

        [FormerlySerializedAs("AteSound")] public AudioClip ateSound;
        [FormerlySerializedAs("HitSound")] public AudioClip hitSound;
        void Start()
        {
            segments = new List<Transform>();
            StartCoroutine(MoveSnake());
        }

        // Update is called once per frame
        void Update()
        {
            if (Up() && direction != Vector2.down) direction = Vector2.up;
            else if (Left() && direction != Vector2.left) direction = Vector2.right;
            else if (Down() && direction != Vector2.up) direction = Vector2.down;
            else if (Right() && direction != Vector2.right) direction = Vector2.left; 
        }

        bool Up()
        {
            return Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
        }
        
        bool Left()
        {
            return Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow);
        }
        
        bool Down()
        {
            return Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);
        }
        
        bool Right()
        {
            return Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow);
        }
        
        public IEnumerator MoveSnake()
        {
            while (!gameLose)
            {
                origPos = transform.position;
                yield return new WaitForSeconds(timeToMove);
                transform.position = new Vector3(origPos.x + direction.x, origPos.y + direction.y, 0.0f);
                if (ate)
                {
                    GameObject g = (GameObject)Instantiate(segmentPrefab,
                        origPos,
                        Quaternion.identity);
                    segments.Insert(0, g.transform);
                    ate = false;
                }

                if (segments.Count > 0)
                {
                    segments.Last().position = origPos;

                    segments.Insert(0, segments.Last());
                    segments.RemoveAt(segments.Count - 1);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Food"))
            {
                StartCoroutine(PlaySound(ateSound));
                ate = true;
                timeToMove *= 0.95f;
            }
            else
            {
                StopCoroutine(MoveSnake());
                StartCoroutine(PlaySound(hitSound));
                FindObjectOfType<GameManager>().GameOver();
            }
        }

        private IEnumerator PlaySound(AudioClip sound)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);
        }
    }
}

