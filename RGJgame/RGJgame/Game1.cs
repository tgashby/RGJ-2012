using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace RGJgame
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MenuSystem menus;

        Texture2D bg;
        SpriteFont font;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            setScreenSize(1200, 600);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            menus = new MenuSystem(this);

            menus.turnOn(MenuSystem.MAIN);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            font = Content.Load<SpriteFont>(@"logtext");
            bg = Content.Load<Texture2D>(@"backgrounds/bg");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if ((Input.BACK(1) || Input.BACK(2) || Input.BACK(3) || Input.BACK(4)))
                this.Exit();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            // TODO: Add your update logic here
            if (menus.menusBusy)
            {
                menus.Update(gameTime.ElapsedGameTime.Milliseconds);
            }
            else
            {
                if ((Input.START(1) || Input.START(2) || Input.START(3) || Input.START(4)))
                    menus.turnOn(MenuSystem.PAUSE);


                // all the rest...

            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.FrontToBack, SaveStateMode.None);

            // TODO: Add your drawing code here
            if (menus.menusBusy)
            {
                menus.Draw(spriteBatch);
            }
            else
            {
                // all the rest
                spriteBatch.Draw(bg, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 4, SpriteEffects.None, 0f);
                spriteBatch.DrawString(font, "Awesome!", new Vector2(200, 100), new Color(0.1f, 0.5f, 0.8f), 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }








        public void NewGame()
        {
            // do all your game init stuffs here (like making new player or tilemaps)
        }




        // Stuff menus need
        #region
        public void EndGame()
        {
            this.Exit();
        }

        public void toggleFullScreen()
        {
            this.graphics.ToggleFullScreen();
        }

        public void setScreenSize(int w, int h)
        {
            this.graphics.PreferredBackBufferWidth = w;
            this.graphics.PreferredBackBufferHeight = h;
            this.graphics.ApplyChanges();
        }

        #endregion

    }
}
