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
        public const int RUNCYCLE = 20;

        public float health;
        private Texture2D guardbase, guardgun;
        private float moveTimer;

        public GuardEnemy(Vector2 pos)
            : base(pos)
        {
            health = 15.0f;
            moveTimer = 0;
        }

        public override void LoadContent(Game game)
        {
            guardbase = game.Content.Load<Texture2D>(@"images/guardbase");
            guardgun = game.Content.Load<Texture2D>(@"images/guardgun");
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
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (velocity.X < 0)
            {
                spriteBatch.Draw(guardbase, position - GameState.player.position, null, Color.White, 0f, new Vector2(20, 20), 1f, SpriteEffects.FlipHorizontally, 0.9f);
                spriteBatch.Draw(guardgun, position - GameState.player.position, null, Color.White, 0f, new Vector2(20, 20), 1f, SpriteEffects.FlipHorizontally, 0.9f);
            }
            else
            {
                spriteBatch.Draw(guardbase, position - GameState.player.position, null, Color.White, 0f, new Vector2(20, 20), 1f, SpriteEffects.None, 0.9f);
                spriteBatch.Draw(guardgun, position - GameState.player.position, null, Color.White, 0f, new Vector2(20, 20), 1f, SpriteEffects.None, 0.9f);
            }
        }
    }
}
