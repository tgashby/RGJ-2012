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
        public float MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f;
        public static Vector2 GUARDDRAWPOS = new Vector2(300, 300);

        private Texture2D guardbase, guardgun;
        private float moveTimer, shotTimer;

        public GuardEnemy(Vector2 pos)
            : base(pos)
        {
            health = 9;
            moveTimer = 0;
            shotTimer = 0;
            velocity.Y = GRAVITY;
        }

        public override void LoadContent(Game game)
        {
            guardbase = game.Content.Load<Texture2D>(@"images/guardbase");
            guardgun = game.Content.Load<Texture2D>(@"images/guardgun");

            texture = guardbase;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds * Game1.CLOCKSPEED;

            moveTimer += elapsedTime;

            if (moveTimer < 1000)
            {
                velocity.X = -MOVEMENTSPEED;
            }
            else if (moveTimer < 2000)
            {
                velocity.X = MOVEMENTSPEED;
            }
            else
                moveTimer = 0;

            position += velocity * elapsedTime;

            shotTimer += elapsedTime;
            if (shotTimer >= 1000.0f)
            {
                if (velocity.X < 0)
                    Bullets.instance.addNewBullet((position - new Vector2(10, 20)), new Vector2(-1.0f, 0.0f), Bullets.P_SMALL, this);
                else
                    Bullets.instance.addNewBullet((position - new Vector2(-10, 20)), new Vector2(1.0f, 0.0f), Bullets.P_SMALL, this);

                shotTimer = 0.0f;
            }

            // Collision checking
            Collisions.check(this, GameState.player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (velocity.X < 0)
            {
                spriteBatch.Draw(guardbase, position - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, 0f,
                    new Vector2(guardbase.Width / 2, guardbase.Height / 2), 1f, SpriteEffects.FlipHorizontally, 0.8f);
                spriteBatch.Draw(guardgun, (position - new Vector2(10, 20)) - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, -(float)Math.PI / 2.0f,
                    new Vector2(guardgun.Width / 2, guardgun.Height / 2), 1f, SpriteEffects.FlipHorizontally, 0.9f);
            }
            else
            {
                spriteBatch.Draw(guardbase, position - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, 0f, 
                    new Vector2(guardbase.Width / 2, guardbase.Height / 2), 1f, SpriteEffects.None, 0.8f);
                spriteBatch.Draw(guardgun, (position - new Vector2(-10, 20)) - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, (float)Math.PI / 2.0f,
                    new Vector2(guardgun.Width / 2, guardgun.Height / 2), 1f, SpriteEffects.None, 0.9f);
            }
        }

        public override void doCollision(Player player)
        {

        }
    }
}
