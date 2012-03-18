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
    class GuardEnemy : Entity
    {
        public const float MOVEMENTSPEED = 0.28f, BULLETSPEED = 2f;
        public const int SHOOTTIME = 100, MINDISTANCE = 400, MAXDISTANCE = 900, NUMSHOTS = 12;


        private Texture2D guardbase, guardgun;
        private float shotTimer;
        private int numshots;

        public GuardEnemy(Vector2 pos)
            : base(pos)
        {
            health = 9;
            numshots = NUMSHOTS;
            shotTimer = 0;
        }

        public override void LoadContent(Game game)
        {
            guardbase = game.Content.Load<Texture2D>(@"images/guardbase");
            guardgun = game.Content.Load<Texture2D>(@"images/guardgun");

            texture = guardbase;
        }

        public override void Update(GameTime gameTime)
        {
            if (velocity.Y != 0)
                velocity.Y += GameState.player.GRAVITY;

            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds * Game1.CLOCKSPEED;
            Vector2 toPlayer = -(position - GameState.player.position);

            if (toPlayer.Length() > MINDISTANCE && toPlayer.Length() < MAXDISTANCE)
            {
                if (toPlayer.X > 0)
                {
                    velocity.X = MOVEMENTSPEED;
                }
                else
                {
                    velocity.X = -MOVEMENTSPEED;
                }
            }
            else
            {
                velocity.X = 0;
            }

            position += velocity * elapsedTime;
            
            if (toPlayer.Length() < MINDISTANCE)
            {
                shotTimer -= elapsedTime;
                if (shotTimer <= 0)
                {
                    toPlayer.Normalize();
                    Bullets.instance.addNewBullet((position - new Vector2(0, 20)), toPlayer * BULLETSPEED, Bullets.P_SMALL, this);

                    shotTimer = SHOOTTIME;
                    numshots--;
                }
                if (numshots == 0)
                {
                    numshots = NUMSHOTS;
                    shotTimer = SHOOTTIME * 20;
                }
            }

            // Collision checking
            Collisions.check(this, GameState.player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Vector2 toPlayer = position - GameState.player.position;
            float rot;

            if (toPlayer.Y < 0)
            {
                rot = (float)Math.Atan(toPlayer.X / toPlayer.Y) + (float)MathHelper.Pi;
            }
            else
            {
                rot = (float)Math.Atan(toPlayer.X / toPlayer.Y + 0.001);
            }

            spriteBatch.Draw(guardbase, position - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, 0f,
                new Vector2(guardbase.Width / 2, guardbase.Height / 2), 1f, SpriteEffects.None, 0.8f);
            spriteBatch.Draw(guardgun, (position - new Vector2(0, 20)) - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, -rot,
                new Vector2(guardgun.Width / 2, guardgun.Height / 2), 1f, SpriteEffects.None, 0.7f);
        }

        public override void doCollision(Player player)
        {

        }
    }
}
