﻿using CozyKxlol.Engine;
using CozyKxlol.Kxlol.Scene;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Starbound.UI.Resources;
using Starbound.UI.XNA.Resources;

namespace CozyKxlol
{
    public class KxlolGame : CozyGame
    {
        IResourceManager resourceManager;

        public KxlolGame()
        {
            graphics.PreferredBackBufferWidth = 800;
            graphics.PreferredBackBufferHeight = 600;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            director.RunWithScene(new BallGameScene());
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            resourceManager = new XNAResourceManager(Content);
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }
}
