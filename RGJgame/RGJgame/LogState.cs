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

        private Dictionary<Keys, bool> keystates;

        public LogState(Game game)
            : base(game)
        { }

        protected override void LoadContent()
        {
            logFont = Game.Content.Load<SpriteFont>(@"logtext");
            background = Game.Content.Load<Texture2D>(@"backgrounds/log");

            hackString = new String("Log State".ToCharArray());
            keystates = new Dictionary<Keys, bool>();

            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                keystates[key] = false;
            }

            // HACK so that the switching states ' ' isn't picked up automatically
            keystates[Keys.Space] = true;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector3 logColor1 = new Vector3(0.0f, 0.0f, 1.0f);
            Vector3 logColor2 = new Vector3(1.0f, 0.1f, 0.0f);

            Vector3 lcolor;
            lcolor = GameState.player.detection * logColor1 + (1 - GameState.player.detection) * logColor2;
            Color logActual = new Color(lcolor);

            spriteBatch.Draw(background, new Vector2(800, 0), null, logActual, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.1f);
            spriteBatch.DrawString(logFont, hackString, new Vector2(820, 100), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
	    }

        public override void Update(GameTime gameTime)
        {
            foreach (Keys key in Enum.GetValues(typeof(Keys)))
            {
                if (KeyHandler.keyDown(key))
                {
                    char[] tmp = new char[1];
                    tmp[0] = (char)(int)key;

                    // The char trick above only works for numbers and A-Z
                    if (!keystates[key] && !key.Equals(Keys.Space)
                        && (((int)key > 47 && (int)key < 58) || ((int)key > 64 && (int)key < 91)))
                    {
                        hackString = hackString.Insert(hackString.Length, new String(tmp));
                    }
                    // ' ' Char
                    else if (!keystates[key] && key.Equals(Keys.Space))
                    {
                        hackString = hackString.Insert(hackString.Length, new String(" ".ToCharArray()));
                    }
                    // '/' Char
                    else if (!keystates[key] && (int)key == 191)
                    {
                        hackString = hackString.Insert(hackString.Length, new String("/".ToCharArray()));
                    }

                    keystates[key] = true;
                }
                else
                {
                    keystates[key] = false;
                }
            }
        }

        public void parseInput()
        {
            // Parse the hacker string here.... Call toMod stuff, abilities, etc

            // TEMP
            if (hackString.Equals(new String("TAG".ToCharArray())))
            {
                toMod.Exit();
            }
        }

        public void clearInput()
        {
            hackString = new String("".ToCharArray());
        }
    }
}
