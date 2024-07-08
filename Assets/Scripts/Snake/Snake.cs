using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Snake
{
    public class Snake : PlayerController2D
    {
        private bool gameLose;
        private bool ate = false;
        private Vector3 origPos;
        [SerializeField] public GameObject segmentPrefab;
        private static float timeToMove = 0.8f;
        private static readonly float startingTimeToMove = 0.8f;
        private Vector2 direction = Vector2.zero;
        public List<Transform> segments;
        [FormerlySerializedAs("AteSound")] public AudioClip ateSound;
        [FormerlySerializedAs("HitSound")] public AudioClip hitSound;
        void Start()
        {
            segments = new List<Transform>();
            StartCoroutine(MoveSnake());
        }

        public void Reset(){
            timeToMove=startingTimeToMove;
            segments = new List<Transform>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public static float TimeToMove { get => timeToMove; set => timeToMove = value; }

        public IEnumerator MoveSnake()
        {
            while (!gameLose)
            {
                origPos = transform.position;
                yield return new WaitForSeconds(TimeToMove);
                transform.position = new Vector3(origPos.x + direction.x, origPos.y + direction.y, 0.0f);

                if (ate) addSegment();

                if (segments.Count > 0) moveSegments();
            }
        }

        public void addSegment(){
            GameObject g = Instantiate(segmentPrefab,
                        origPos,
                        Quaternion.identity);
            segments.Insert(0, g.transform);
            ate = false;
        }

        void moveSegments(){
            segments.Last().position = origPos;

            segments.Insert(0, segments.Last());

            segments.RemoveAt(segments.Count - 1);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Food"))
            {
                StartCoroutine(PlaySound(ateSound));
                ate = true;
                TimeToMove *= 0.97f;
            }
            else if(col.gameObject.layer == LayerMask.NameToLayer("Tilemap"))
            {
                StopCoroutine(MoveSnake());
                StartCoroutine(PlaySound(hitSound));
                FindObjectOfType<GameManager>().GameOver();
            }
        }

        public static readonly (int, float)[] speedReductions = { (8, 0.125f), (16, 0.25f), (-1, 0.0f) };

        private IEnumerator PlaySound(AudioClip sound)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.clip = sound;
            audioSource.Play();
            yield return new WaitForSecondsRealtime(audioSource.clip.length);
        }

        protected override void HandleMovementInput(Vector2 movementVector)
        {
                 if (movementVector.y > 0 && direction != Vector2.down) direction = Vector2.up;
            else if (movementVector.x < 0 && direction != Vector2.right) direction = Vector2.left;
            else if (movementVector.y < 0 && direction != Vector2.up) direction = Vector2.down;
            else if (movementVector.x > 0 && direction != Vector2.left) direction = Vector2.right; 
        }

    }
}

