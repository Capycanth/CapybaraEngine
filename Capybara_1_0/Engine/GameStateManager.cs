using Capybara_1.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capybara_1.Engine
{
    public class GameStateManager
    {
        #region Singleton
        private static GameStateManager _singleton;

        public static GameStateManager GetGameStateManager()
        {
            if (_singleton == null)
            {
                _singleton = new GameStateManager();
            }
            return _singleton;
        }
        #endregion

        private IGameState currentState;

        public void ChangeState(IGameState newState)
        {
            currentState?.UnloadContent(); // Unload content of the current state if it exists
            currentState = newState;
            currentState.Initialize();
            currentState.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            currentState?.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            currentState?.Draw(spriteBatch);
        }

        public void HandleInput()
        {
            currentState?.HandleInput();
        }
    }
}
