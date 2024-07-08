using UnityEngine;
using UnityEngine.InputSystem;

namespace Snake
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class PlayerController2D : MonoBehaviour
    {
        public GameStateManager game; 
        private Rigidbody2D rb;
        public float speed {get; private set;} = 1;
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            game = GameStateManager.GetInstance();
        }
        void OnMove(InputValue movementValue){
            Vector2 movementVector = movementValue.Get<Vector2>();
            HandleMovementInput(movementVector);
        }

        void onPause(InputAction inputAction){

        }

        void onLeave(InputAction inputAction){

        }

        void onUp(InputAction inputAction){

        }

        void onDown(InputAction inputAction){

        }

        void onEnter(InputAction inputAction){

        }

        protected abstract void HandleMovementInput(Vector2 movementVector);

    }
}

