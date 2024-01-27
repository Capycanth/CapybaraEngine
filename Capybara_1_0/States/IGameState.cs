using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Capybara_1.States
{
    public enum GameStateEnum
    {
        // Add Game State Enums here for each Game State
    }

    public interface IGameState
    {
        GameStateEnum GameState { get; set; }
        void Initialize();
        void LoadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch spriteBatch);
        void HandleInput();
        void UnloadContent();
    }
}
