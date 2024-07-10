using UnityEngine;
using Snake.UI;
using System;

namespace Snake
{
    public sealed class GameStateManager
    {
        public int level {get; private set;}
        public enum GameState {GAME, OVER, WIN, PAUSE, SETUP}
        private GameState state;
        private GameStateManager() {
            level = 1;
            state = GameState.SETUP;
        }
        private static GameStateManager _instance;
        private UI.Controller pauseUI;
        private UI.Controller gameOverUI;
        public GameState State => state;

        public Controller PauseUI { get => pauseUI; set => pauseUI = value; }
        public Controller GameOverUI { get => gameOverUI; set => gameOverUI = value; }

        public void Pause() { state = GameState.PAUSE; PauseUI.Activate(); }
        public bool isPaused() => state != GameState.GAME;
        public void Resume() { state = GameState.GAME; DisableUI(); }
        public void GameOver() { state = GameState.OVER; level = 1; GameOverUI.Activate(); }
        public void Setup() => state = GameState.SETUP;

        public int Victory()
        {
            state = GameState.WIN;
            level++;
            return level;
        }

        public static GameStateManager GetInstance()
        {
            if (_instance == null)
            {
                _instance = new GameStateManager();
            }
            return _instance;
        }

        void DisableUI()
        {
            PauseUI.DeActivate();
            GameOverUI.DeActivate();
        }
    }
}