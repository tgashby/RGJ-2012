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
        private String hackString, promptString, prevLogEntries, promptDefault;

        public static LogState instance;

        private Dictionary<Keys, bool> keystates;

        public LogState(Game game)
            : base(game)
        {
            LogState.instance = this;
        }

        protected override void LoadContent()
        {
            logFont = Game.Content.Load<SpriteFont>(@"logtext");
            background = Game.Content.Load<Texture2D>(@"backgrounds/log");

            hackString = new String("".ToCharArray());
            prevLogEntries = new String("".ToCharArray());

            promptDefault = new String(
                ("List of Available Commands:\n" +
                 "RESET\n").ToCharArray());

            promptString = new String(
                 (promptDefault + "Previous Log Entries:\n" + prevLogEntries + "Enter Power:\n").ToCharArray());

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

            promptString = cullString(promptString);   

            spriteBatch.Draw(background, new Vector2(800, 0), null, logActual, 0f, Vector2.Zero, 1, SpriteEffects.None, 0.9f);
            spriteBatch.DrawString(logFont, promptString.Insert(promptString.Length, hackString), new Vector2(830, 50), Color.Orange, 0f, Vector2.Zero, 1f, SpriteEffects.None, 1f);
	    }

        public String cullString(String from)
        {
            int num = 0, index = from.Length - 1;
            char[] str = from.ToCharArray();

            while (index >= 0 && num < 20)
            {
                if (str[index] == '\n')
                {
                    num++;
                }
                index--;
            }
            if (index >= 0)
            {
                from = from.Substring(index + 2);
            }
            return from;
        }

        public void catIntoLog(String str)
        {
            promptString += str;
            prevLogEntries += str;
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
                    // '.' Char
                    else if (!keystates[key] && (int)key == 190)
                    {   
                        hackString = hackString.Insert(hackString.Length, new String(".".ToCharArray()));
                    }
                    // '-' char
                    else if (!keystates[key] && (int)key == 189)
                    {
                        hackString = hackString.Insert(hackString.Length, new String("-".ToCharArray()));
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
            hackString = hackString.Trim();

            if (hackString.IndexOf(PlayerPower.TELEPORT) != -1)
            {
                String[] tokens = hackString.Split(new char[] { ' ' });

                if (GameState.player.powers.isAvailable(PlayerPower.TELEPORT))
                {
                    if (tokens.Length == 3)
                    {
                        try
                        {
                            float xCoord = float.Parse(tokens[1]) * 3;
                            float yCoord = float.Parse(tokens[2]) * 3;

                            //GameState.player.position.X += xCoord;
                            //GameState.player.position.Y += yCoord;
                            Vector2 movement = GameState.gameMap.teleportCheck(GameState.player, new Vector2(xCoord, yCoord));
                            if (movement != null)
                            {
                                GameState.player.position = movement;
                            }
                            else
                            {
                                GameState.player.health = 0;
                            }
                            prevLogEntries += (hackString + "\n");
                        }
                        catch (Exception e)
                        { }
                    }
                }
                else
                {
                    prevLogEntries += (hackString + " not available yet\n");
                }
            }
            else if (hackString.IndexOf(PlayerPower.KILL_ID) != -1)
            {
                String[] tokens = hackString.Split(new char[] { ' ' });

                if (GameState.player.powers.isAvailable(PlayerPower.TELEPORT))
                {
                    if (tokens.Length == 1)
                    {
                        try
                        {
                            int pid = int.Parse(tokens[1]);

                            // TODO: KILL IT!
                            // Something like: enemies[pid].kill()
                        }
                        catch (Exception e) 
                        { }
                    }
                }
                else
                {
                    prevLogEntries += (hackString + " not available yet\n");
                }
            }
            // Parse the hacker string here.... Call toMod stuff, abilities, etc
            else if (GameState.player.powers.isAvailable(hackString))
            {
                switch (hackString)
                {
                    case PlayerPower.GRAVITY_OFF:
                        GameState.player.GRAVITY = 0.0f;
                        prevLogEntries += (hackString + "\n");
                        GameState.player.usePower(PlayerPower.GRAVITY_OFF);
                        GameState.player.disablePower(PlayerPower.GRAVITY_NORMAL);
                        GameState.player.disablePower(PlayerPower.LOW_GRAV);
                        GameState.player.disablePower(PlayerPower.MASSIVE_GRAV);
                        break;
                    case PlayerPower.GRAVITY_NORMAL:
                        GameState.player.GRAVITY = 0.08f;
                        prevLogEntries += (hackString + "\n");
                        GameState.player.disablePower(PlayerPower.GRAVITY_OFF);
                        GameState.player.usePower(PlayerPower.GRAVITY_NORMAL);
                        GameState.player.disablePower(PlayerPower.LOW_GRAV);
                        GameState.player.disablePower(PlayerPower.MASSIVE_GRAV);
                        break;
                    case PlayerPower.LOW_GRAV:
                        GameState.player.GRAVITY = 0.05f;
                        prevLogEntries += (hackString + "\n");
                        GameState.player.disablePower(PlayerPower.GRAVITY_OFF);
                        GameState.player.disablePower(PlayerPower.GRAVITY_NORMAL);
                        GameState.player.usePower(PlayerPower.LOW_GRAV);
                        GameState.player.disablePower(PlayerPower.MASSIVE_GRAV);
                        break;
                    case PlayerPower.MASSIVE_GRAV:
                        GameState.player.GRAVITY = 0.15f;
                        prevLogEntries += (hackString + "\n");
                        GameState.player.disablePower(PlayerPower.GRAVITY_OFF);
                        GameState.player.disablePower(PlayerPower.GRAVITY_NORMAL);
                        GameState.player.disablePower(PlayerPower.LOW_GRAV);
                        GameState.player.usePower(PlayerPower.MASSIVE_GRAV);
                        break;
                    case PlayerPower.REV_GRAVITY:
                        /*GameState.player.GRAVITY = -0.08f;
                        GameState.player.JUMP = 3.0f;*/
                        prevLogEntries += (hackString + "\n");
                        GameState.player.usePower(PlayerPower.REV_GRAVITY);
                        break;

                    case PlayerPower.SUPER_JUMP:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.JUMP = -1.8f;
                        GameState.player.usePower(PlayerPower.SUPER_JUMP);
                        GameState.player.disablePower(PlayerPower.NORMAL_JUMP);
                        GameState.player.disablePower(PlayerPower.WEAK_JUMP);
                        break;
                    case PlayerPower.NORMAL_JUMP:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.JUMP = -1.1f;
                        GameState.player.disablePower(PlayerPower.SUPER_JUMP);
                        GameState.player.usePower(PlayerPower.NORMAL_JUMP);
                        GameState.player.disablePower(PlayerPower.WEAK_JUMP);
                        break;
                    case PlayerPower.WEAK_JUMP:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.JUMP = -0.6f;
                        GameState.player.disablePower(PlayerPower.SUPER_JUMP);
                        GameState.player.disablePower(PlayerPower.NORMAL_JUMP);
                        GameState.player.usePower(PlayerPower.WEAK_JUMP);
                        break;

                    case PlayerPower.OVERCLOCK4:
                        prevLogEntries += (hackString + "\n");
                        Game1.CLOCKSPEED = 4;
                        GameState.player.usePower(PlayerPower.OVERCLOCK4);
                        GameState.player.disablePower(PlayerPower.OVERCLOCK2);
                        GameState.player.disablePower(PlayerPower.CLOCK1);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK2);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK4);
                        break;
                    case PlayerPower.OVERCLOCK2:
                        prevLogEntries += (hackString + "\n");
                        Game1.CLOCKSPEED = 2;
                        GameState.player.disablePower(PlayerPower.OVERCLOCK4);
                        GameState.player.usePower(PlayerPower.OVERCLOCK2);
                        GameState.player.disablePower(PlayerPower.CLOCK1);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK2);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK4);
                        break;
                    case PlayerPower.CLOCK1:
                        prevLogEntries += (hackString + "\n");
                        Game1.CLOCKSPEED = 1.0f;
                        GameState.player.disablePower(PlayerPower.OVERCLOCK4);
                        GameState.player.disablePower(PlayerPower.OVERCLOCK2);
                        GameState.player.usePower(PlayerPower.CLOCK1);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK2);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK4);
                        break;
                    case PlayerPower.UNDERCLOCK4:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.disablePower(PlayerPower.OVERCLOCK4);
                        GameState.player.disablePower(PlayerPower.OVERCLOCK2);
                        GameState.player.disablePower(PlayerPower.CLOCK1);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK2);
                        GameState.player.usePower(PlayerPower.UNDERCLOCK4);
                        Game1.CLOCKSPEED = 0.25f;
                        break;
                    case PlayerPower.UNDERCLOCK2:
                        prevLogEntries += (hackString + "\n");
                        Game1.CLOCKSPEED = 0.5f;
                        GameState.player.disablePower(PlayerPower.OVERCLOCK4);
                        GameState.player.disablePower(PlayerPower.OVERCLOCK2);
                        GameState.player.disablePower(PlayerPower.CLOCK1);
                        GameState.player.usePower(PlayerPower.UNDERCLOCK2);
                        GameState.player.disablePower(PlayerPower.UNDERCLOCK4);
                        break;

                    case PlayerPower.MOVEMENT_FAST:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.MOVEMENTSPEED = 0.56f;
                        GameState.player.disablePower(PlayerPower.MOVEMENT_SLOW);
                        GameState.player.disablePower(PlayerPower.MOVEMENT_NORMAL);
                        GameState.player.usePower(PlayerPower.MOVEMENT_FAST);
                        break;
                    case PlayerPower.MOVEMENT_NORMAL:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.MOVEMENTSPEED = 0.28f;
                        GameState.player.disablePower(PlayerPower.MOVEMENT_SLOW);
                        GameState.player.usePower(PlayerPower.MOVEMENT_NORMAL);
                        GameState.player.disablePower(PlayerPower.MOVEMENT_FAST);
                        break;
                    case PlayerPower.MOVEMENT_SLOW:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.MOVEMENTSPEED = 0.14f;
                        GameState.player.usePower(PlayerPower.MOVEMENT_SLOW);
                        GameState.player.disablePower(PlayerPower.MOVEMENT_NORMAL);
                        GameState.player.disablePower(PlayerPower.MOVEMENT_FAST);
                        break;


                    case PlayerPower.BULLET1:
                        prevLogEntries += (hackString + "\n");
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-2f, 0f) : new Vector2(2f, 0f), Bullets.P_SMALL, GameState.player);
                        GameState.player.detection -= 0.1f;
                        break;

                    case PlayerPower.BULLET2:
                        prevLogEntries += (hackString + "\n");
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-1.2f, 0f) : new Vector2(1.2f, 0f), Bullets.P_SMALL, GameState.player);
                        GameState.player.detection -= 0.1f;
                        break;
                    case PlayerPower.BULLET_SPREAD:
                        prevLogEntries += (hackString + "\n");
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-0.8f, 0.4f) : new Vector2(0.8f, 0.4f), Bullets.P_SMALL, GameState.player);
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-0.9f, 0.2f) : new Vector2(0.9f, 0.2f), Bullets.P_SMALL, GameState.player);
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-1.0f, 0f) : new Vector2(1.0f, 0f), Bullets.P_SMALL, GameState.player);
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-0.9f, -0.2f) : new Vector2(0.9f, -0.2f), Bullets.P_SMALL, GameState.player);
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-0.8f, -0.4f) : new Vector2(0.8f, -0.4f), Bullets.P_SMALL, GameState.player);
                        GameState.player.detection -= 0.4f;
                        break;
                    case PlayerPower.BULLET_DIAGONAL:
                        prevLogEntries += (hackString + "\n");
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-0.8f, 0.5f) : new Vector2(0.8f, 0.5f), Bullets.P_SMALL, GameState.player);
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-0.8f, -0.5f) : new Vector2(0.8f, -0.5f), Bullets.P_SMALL, GameState.player);
                        GameState.player.detection -= 0.2f;
                        break;
                    case PlayerPower.BULLET_TRIPLE:
                        prevLogEntries += (hackString + "\n");
                        Bullets.instance.addNewBullet(GameState.player.position + new Vector2(0, -20),
                            GameState.player.facingLeft ? new Vector2(-1.2f, 0f) : new Vector2(1.2f, 0f), Bullets.P_SMALL, GameState.player);
                        Bullets.instance.addNewBullet(GameState.player.position,
                            GameState.player.facingLeft ? new Vector2(-1.2f, 0f) : new Vector2(1.2f, 0f), Bullets.P_SMALL, GameState.player);
                        Bullets.instance.addNewBullet(GameState.player.position + new Vector2(0, 20),
                            GameState.player.facingLeft ? new Vector2(-1.2f, 0f) : new Vector2(1.2f, 0f), Bullets.P_SMALL, GameState.player);
                        GameState.player.detection -= 0.25f;
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

                    case PlayerPower.RESET:
                        prevLogEntries += (hackString + "\n");
                        GameState.player.hardReset();
                        break;

                    default:
                        if (hackString.Length < 14)
                            hackString = new String(("System Runtime Exception: \n   Unknown Command: " + hackString + "\n").ToCharArray());
                        else
                            hackString = new String(("System Runtime Exception: \n   Unknown Command: " + hackString.Substring(0, 10) + "...\n").ToCharArray());
                        prevLogEntries += (hackString + "\n");
                        break;
                }
            }
            else
            {
                prevLogEntries += (hackString + " not available yet\n");
            }
        }

        public void clearInput()
        {
            promptString = new String(
                 (promptDefault + "Previous Log Entries:\n" + prevLogEntries + "Enter Power:\n").ToCharArray());

            if (hackString.Length > 0)
                hackString = hackString.Remove(0);
        }

        internal void catIntoAvailable(string newPower)
        {
            promptDefault += newPower + "\n";
        }
    }
}
