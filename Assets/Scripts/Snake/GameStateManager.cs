using UnityEngine;
using Snake.UI;

namespace Snake
{
    public sealed class GameStateManager
    {
        public enum GameState {GAME, OVER, WIN, PAUSE, SETUP}
        private GameState state;
        private GameStateManager() {}
        private static GameStateManager _instance;
        public UI.Controller PauseUI;
        public UI.Controller GameOverUI;
        public GameState State => state;
        public void Pause() { state = GameState.PAUSE; PauseUI.Activate(); }
        public bool isPaused() => state != GameState.GAME;
        public void Resume() { state = GameState.GAME; DisableUI(); }
        public void GameOver() { state = GameState.OVER; GameOverUI.Activate(); }
        public void Setup() => state = GameState.SETUP;
        
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