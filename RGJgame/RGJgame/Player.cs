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
        public const float JUMP = -1.1f, MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f;
        public static Vector2 PLAYERDRAWPOS = new Vector2(300, 300);
        public const int RUNCYCLE = 20;

        public float health;
        public float detection;
        private bool jump = false, hack = false;
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
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                if (!jump)
                {
                    jump = true;
                    velocity.Y = JUMP;
                }
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X += -MOVEMENTSPEED;
                if (velocity.X < -MOVEMENTSPEED)
                    velocity.X = -MOVEMENTSPEED;
                runtimer++;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity.X += MOVEMENTSPEED;
                if (velocity.X > MOVEMENTSPEED)
                    velocity.X = MOVEMENTSPEED;
                runtimer++;
            }
            if (!Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.A))
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
            else
            {
                if (hack)
                    toDraw = hacking;
                else
                {
                    if (velocity.X == 0)
                        toDraw = standing;
                    else if (runtimer % RUNCYCLE < RUNCYCLE / 2)
                        toDraw = running1;
                    else
                        toDraw = running2;
                }
            }
            

            spriteBatch.Draw(toDraw, PLAYERDRAWPOS, null, Color.White, 0f, new Vector2(20, 20), 1f, SpriteEffects.None, 0.8f);
        }

    }
}
