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
            background = Game.Content.Load<Texture2D>(@"backgrounds/log");

            hackString = new String("Log State".ToCharArray());
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector3 logColor2 = new Vector3(1.0f, 0.1f, 0.0f);

            Vector3 lcolor;
            lcolor = GameState.player.detection * logColor1 + (1 - GameState.player.detection) * logColor2;
            Color logActual = new Color(lcolor);

            spriteBatch.Draw(background, new Vector2(800, 0), null, logActual, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.1f);
            spriteBatch.DrawString(logFont, "Log State", new Vector2(820, 100), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
	}

        public override void Update(GameTime gameTime)
        {

        }
    }
}
