using Capybara_1.Config.Internal;
using Capybara_1.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Capybara_1.Engine
{
    public class GameStateManager
    {
        #region Singleton
        private static GameStateManager _singleton;

        public static GameStateManager GetGameStateManager(ContentManager _contentManager)
        {
            if (_singleton == null)
            {
                _singleton = new GameStateManager();
                _singleton._contentManager = _contentManager;
            }
            return _singleton;
        }
        #endregion

        private Queue<IGameState> StateQueue = new Queue<IGameState>();
        private Queue<GameStateTransition> GSTs = new Queue<GameStateTransition>();
        private ContentManager _contentManager;
        private bool popState = false;

        public void Update(GameTime gameTime)
        {
            if (popState)
            {
                DestroyState(StateQueue.Dequeue());
                popState = false;
                StateQueue.Peek().Begin();
            }

            if (GSTs.Count == 0 && StateQueue.Count > 0)
            {
                StateQueue.Peek().HandleInput();
                StateQueue.Peek().Update(gameTime);
            }
            else
            {
                if (GSTs.Peek().IsCompleted)
                {
                    GSTs.Dequeue();
                    return;
                }
                switch (GSTs.Peek().GSTEnum)
                {
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (GSTs.Count == 0 && StateQueue.Count > 0)
            {
                StateQueue.Peek().Draw(spriteBatch);
            }
            else
            {
                switch (GSTs.Peek().GSTEnum)
                {
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        public async Task PushToStackAndInitialize(IGameState newGameState, Action TransitionComplete)
        {
            await Task.Run(() =>
            {
                newGameState.LoadContent(_contentManager);
                newGameState.Initialize();
                StateQueue.Enqueue(newGameState);
                if (StateQueue.Count > 1)
                {
                    this.popState = true;
                }
                TransitionComplete();
            });
        }

        public async Task DestroyState(IGameState state)
        {
            await Task.Run(() =>
            {
                state.UnloadContent();
            });
        }
    }

    public enum GSTEnum
    {
        DELAY = 0,
        BLACK_FADE = 1,
        LOAD = 2,
    }

    public abstract class GameStateTransition
    {
        protected Timer timer = new();
        protected GSTEnum gstEnum;
        protected int runtime;
        protected bool completed = false;

        public GameStateTransition(int runtime)
        {
            this.runtime = runtime;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);

        public GSTEnum GSTEnum { get { return gstEnum; } set { this.gstEnum = value; } }
        public bool IsCompleted { get { return completed; } }
    }

    public class GSTBlackFade : GameStateTransition
    {
        private Rectangle rect;
        private Texture2D texture;
        private float frameFade;
        private float currentFade;
        private Color color;

        public GSTBlackFade(int runtime, bool fadeOut) : base(runtime)
        {
            this.GSTEnum = GSTEnum.BLACK_FADE;
            rect = new Rectangle(0, 0, 1080, 720);
            texture = Game._cache.TextureCache["BlackPixel"];

            if (fadeOut)
            {
                frameFade = 1f / runtime;
                currentFade = 0f;
            }
            else
            {
                frameFade = -1f / runtime;
                currentFade = 1f;
            }
            this.color = new Color(Color.White, currentFade);
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(this.texture, this.rect, this.color);
        }

        public override void Update(GameTime gameTime)
        {
            this.currentFade += (this.frameFade * gameTime.ElapsedGameTime.Milliseconds);
            Math.Clamp(currentFade, 0f, 1f);
            if (currentFade == 1f || currentFade == 0)
            {
                this.completed = true;
                return;
            }
            this.color = new Color(Color.White, currentFade);
        }
    }

    public class GSTDelay : GameStateTransition
    {
        public GSTDelay(int runtime) : base(runtime)
        {
            this.GSTEnum = GSTEnum.DELAY;
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            timer.updateTime(gameTime.ElapsedGameTime.Milliseconds);
            if (timer.isTimeMet(1000))
            {
                this.completed = true;
            }
        }
    }

    public class GSTLoad : GameStateTransition
    {
        private IGameState toState;
        private Rectangle rect;
        private Texture2D texture;
        private bool waiting;
        public GSTLoad(int runtime, IGameState toState) : base(runtime)
        {
            this.toState = toState;
            this.GSTEnum = GSTEnum.LOAD;
            this.waiting = false;
            rect = new Rectangle(0, 0, 1080, 720);
            texture = Game._cache.TextureCache["BlackPixel"];
        }

        public override void Draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture, rect, Color.White);
        }

        public override void Update(GameTime gameTime)
        {
            if (!waiting)
            {
                Game._gameStateManager.PushToStackAndInitialize(this.toState, () => { this.completed = true; });
                waiting = true;
            }
        }
    }
}
