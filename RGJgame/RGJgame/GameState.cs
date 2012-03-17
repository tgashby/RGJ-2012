using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    class GameState : State
    {
        private SpriteFont gameFont;
        private Texture2D background;
        public static Player player;
        public const int PARALAX = 10;

        public GameState(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>(@"logtext");
            background = Game.Content.Load<Texture2D>(@"backgrounds/bg");

            player = new Player(new Vector2(300, 300));
            player.LoadContent(Game);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, -player.position / PARALAX, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);
            spriteBatch.DrawString(gameFont, "Game State", new Vector2(20, 100), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);

            player.draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            player.update(gameTime.ElapsedGameTime.Milliseconds);
        }
    }
}
