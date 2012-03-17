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
    class LogState : State
    {
        private SpriteFont logFont;
        private Texture2D background;
        private String hackString;

        public LogState(Game game)
            : base(game)
        { }

        protected override void LoadContent()
        {
            logFont = Game.Content.Load<SpriteFont>(@"logtext");
            background = Game.Content.Load<Texture2D>(@"backgrounds/bg");

            hackString = new String("Log State".ToCharArray());
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, new Vector2(0, 0), null, Color.White, 0f, Vector2.Zero, 4, SpriteEffects.None, 0f);
            spriteBatch.DrawString(logFont, hackString, new Vector2(100, 100), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
