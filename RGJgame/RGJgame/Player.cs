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
    class Player
    {
        public float JUMP = -1.1f, MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f;
        public static Vector2 PLAYERDRAWPOS = new Vector2(300, 300);
        public const int RUNCYCLE = 20;

        public float health;
        public float detection;
        private bool jump = false, hack = false, shielding = false;
        private Texture2D standing, running1, running2, jumping, hacking, jumphacking, shield;
        private int runtimer;

        public Vector2 position, velocity;

        public Player(Vector2 pos)
        {
            health = 1.0f;
            detection = 1.0f;

            position = pos;
        }

        public void LoadContent(Game game)
        {
            standing = game.Content.Load<Texture2D>(@"images/guy_standing");
            running1 = game.Content.Load<Texture2D>(@"images/guy_walking1");
            running2 = game.Content.Load<Texture2D>(@"images/guy_walking2");
            jumping = game.Content.Load<Texture2D>(@"images/guy_jumping");
            hacking = game.Content.Load<Texture2D>(@"images/guy_hacking_standing");
            jumphacking = game.Content.Load<Texture2D>(@"images/guy_hacking_jumping");
            shield = game.Content.Load<Texture2D>(@"images/guy_shield");
        }

        public void update(float dtime)
        {
            if (!jump)
                shielding = KeyHandler.keyDown(Keys.S);
            else
                shielding = false;

            if (!shielding)
            {
                if (KeyHandler.keyDown(Keys.W))
                {
                    if (!jump)
                    {
                        jump = true;
                        velocity.Y = JUMP;
                    }
                }
                if (KeyHandler.keyDown(Keys.A))
                {
                    velocity.X += -MOVEMENTSPEED;
                    if (velocity.X < -MOVEMENTSPEED)
                        velocity.X = -MOVEMENTSPEED;
                    runtimer++;
                }
                if (KeyHandler.keyDown(Keys.D))
                {
                    velocity.X += MOVEMENTSPEED;
                    if (velocity.X > MOVEMENTSPEED)
                        velocity.X = MOVEMENTSPEED;
                    runtimer++;
                }

                if (!KeyHandler.keyDown(Keys.D) && !KeyHandler.keyDown(Keys.A))
                {
                    velocity.X = 0;
                    runtimer = 0;
                }

                if (jump)
                    velocity.Y += GRAVITY;

                if (position.Y > 500)
                {
                    position.Y = 500;
                    jump = false;
                }

                position += velocity * dtime;
            }
            
            /*detection -= 0.01f;
            if (detection < 0) detection = 1;*/
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Texture2D toDraw;
            if (jump)
            {
                if (hack)
                    toDraw = jumphacking;
                else
                    toDraw = jumping;
            }
            else if (hack)
            {
                toDraw = hacking;
            }
            else if (shielding)
            {
                toDraw = shield;
            }
            else
            {
                if (velocity.X == 0)
                    toDraw = standing;
                else if (runtimer % RUNCYCLE < RUNCYCLE / 2)
                    toDraw = running1;
                else
                    toDraw = running2;
            }

            SpriteEffects playerDir = new SpriteEffects();

            if (velocity.X < 0)
                playerDir = SpriteEffects.FlipHorizontally;
                
                
            spriteBatch.Draw(toDraw, PLAYERDRAWPOS, null, Color.White, 0f, new Vector2(20, 20), 1f, playerDir, 0.8f);
        }

    }

    public sealed class PlayerPower
    {
        private PlayerPower() {}

        // Add powers and their string values down here, then go to LogState and add them in parseInput
        public const string GRAVITY_OFF = "GRAVITY OFF";
        public const string SUPER_JUMP = "SUPER JUMP";
        public const string LOW_GRAV = "LOW GRAVITY";
        public const string MASSIVE_GRAV = "MASSIVE GRAVITY";
        public const string REV_GRAVITY = "REVERSE GRAVITY";
        public const string SUPERSPEED = "OVERCLOCK";
        public const string BULLET = "FIRE BULLET";
        public const string BULLET_SPREAD = "FIRE SPREAD";
        public const string AUTO_FIRE = "AUTO FIRE";
        public const string STRONG_BULLETS = "STRONG BULLETS";
        public const string FREEZE_ENEMIES = "FREEZE";
        public const string BURN_ENEMIES = "FLAMES";
        public const string THROW_ENEMY = "THROW NEAREST";
        public const string NUKE = "WE ARE DOOMED";
        public const string TELEPORT = "TELEPORT"; // Special Case
        public const string LAZER = "LAZAR";
        public const string DECREASE_ENEMY_SPEED = "SLOW EM DOWN";
        public const string GET_ENEMY_ID = "PID NEAREST";
        public const string KILL_ID = "KILL"; // Special Case
        public const string ROOT_PRIV = "LOLZROOT";
    }
}
