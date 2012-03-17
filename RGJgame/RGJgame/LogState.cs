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
        private String hackString, promptString, activePowers;

        private Dictionary<Keys, bool> keystates;

        public LogState(Game game)
            : base(game)
        { }

        protected override void LoadContent()
        {
            logFont = Game.Content.Load<SpriteFont>(@"logtext");
            background = Game.Content.Load<Texture2D>(@"backgrounds/log");

            hackString = new String("".ToCharArray());
            activePowers = new String("".ToCharArray());

            promptString = new String(
                 ("List of Available Commands:\n" +
                 "GRAVITY OFF, " +
                // Add power names and a comma, or a \n at the end of powers list
                 "SUPER JUMP\n" +
                 "Currently Active Powers:\n" +
                 activePowers + "Enter Power:\n").ToCharArray());

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

            spriteBatch.Draw(background, new Vector2(800, 0), null, logActual, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
            spriteBatch.DrawString(logFont, promptString.Insert(promptString.Length, hackString), new Vector2(820, 100), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
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
                    // Backspace
                    else if (!keystates[key] && (int)key == 8)
                    {
                        if (hackString.Length > 0)
                            hackString = hackString.Remove(hackString.Length - 1, 1);
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
            // Strip initial spaces
            while (hackString.StartsWith(" "))
            {
                hackString = hackString.Remove(0, 1);
            }

            // Parse the hacker string here.... Call toMod stuff, abilities, etc
            switch (hackString)
            {
                case PlayerPower.GRAVITY_OFF:
                    GameState.player.GRAVITY = 0.0f;
                    activePowers += (hackString + "\n");
                    break;

                case PlayerPower.SUPER_JUMP:
                    activePowers += (hackString + "\n");
                    GameState.player.JUMP = -3.0f;
                    break;

                case PlayerPower.LOW_GRAV:

                    break;

                case PlayerPower.MASSIVE_GRAV:

                    break;

                case PlayerPower.REV_GRAVITY:

                    break;

                case PlayerPower.SUPERSPEED:

                    break;

                case PlayerPower.BULLET:

                    break;

                case PlayerPower.BULLET_SPREAD:

                    break;

                case PlayerPower.AUTO_FIRE:

                    break;

                case PlayerPower.STRONG_BULLETS:

                    break;

                case PlayerPower.FREEZE_ENEMIES:

                    break;

                case PlayerPower.BURN_ENEMIES:

                    break;

                case PlayerPower.THROW_ENEMY:

                    break;

                case PlayerPower.NUKE:

                    break;

                case PlayerPower.TELEPORT:

                    break;

                case PlayerPower.LAZER:

                    break;

                case PlayerPower.DECREASE_ENEMY_SPEED:

                    break;

                case PlayerPower.GET_ENEMY_ID:

                    break;

                case PlayerPower.KILL_ID:

                    break;

                case PlayerPower.ROOT_PRIV:

                    break;

                default:
                    hackString = new String("Unknown Command!".ToCharArray());
                    break;
            }
        }

        public void clearInput()
        {
            promptString = new String(
                ("List of Available Commands:\n" +
                "GRAVITY OFF, " +
                // Add power names and a comma, or a \n at the end of powers list
                "SUPER JUMP\n" +
                "Currently Active Powers:\n" +
                activePowers + "Enter Power:\n").ToCharArray());

            if (hackString.Length > 0)
                hackString = hackString.Remove(0);
        }
    }
}
