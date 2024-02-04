using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Capybara_1.Engine;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace Capybara_1.States
{
    public enum GameStateEnum
    {
        // Add Game State Enums here for each Game State
        NONE = 0,
    }

    public interface IGameState
    {
        GameStateEnum GameState { get; set; }
        InputManager InputManager { get; set; }
        void Initialize();
        void LoadContent(ContentManager _contentManager);
        void UnloadContent();
        void Update(GameTime gameTime);
        void Draw(SpriteBatch _spriteBatch);
        void Begin(); // Begin music, intro camera animations, intro transitions, etc...
        void HandleInput()
        {
            InputManager.HandleInput(Keyboard.GetState(), Mouse.GetState());
        }
    }
}
