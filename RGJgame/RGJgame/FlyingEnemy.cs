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
    class FlyingEnemy : Entity
    {
        public float MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f;
        public static Vector2 FLYERDRAWPOS = new Vector2(300, 300);
        public const int RUNCYCLE = 40;

        private Texture2D flyer1, flyer2, flyer3, flyer4;
        private int runtimer;

        public FlyingEnemy(Vector2 pos)
            : base(pos)
        {
            health = 6;
            runtimer = 0;

            velocity.X = MOVEMENTSPEED;
            velocity.Y = MOVEMENTSPEED;
        }

        public override void LoadContent(Game game)
        {
            flyer1 = game.Content.Load<Texture2D>(@"images/flyer1");
            flyer2 = game.Content.Load<Texture2D>(@"images/flyer2");
            flyer3 = game.Content.Load<Texture2D>(@"images/flyer3");
            flyer4 = game.Content.Load<Texture2D>(@"images/flyer4");

            texture = flyer1;
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds * Game1.CLOCKSPEED;

            if (runtimer > 500.0f)
                runtimer = 0;

            Vector2 dirToPlayer = GameState.player.position - position + Player.PLAYERDRAWPOS;
            dirToPlayer.Normalize();

            position += dirToPlayer * velocity * elapsedTime;
            runtimer++;

            Collisions.check(this, GameState.player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D toDraw;
            if (velocity.X == 0)
                toDraw = flyer1;
            else if (runtimer % RUNCYCLE < RUNCYCLE / 4)
                toDraw = flyer1;
            else if (runtimer % RUNCYCLE < RUNCYCLE / 3)
                toDraw = flyer2;
            else if (runtimer % RUNCYCLE < RUNCYCLE / 2)
                toDraw = flyer3;
            else
                toDraw = flyer4;

            SpriteEffects flyerDir = new SpriteEffects();

            if (velocity.X < 0)
                flyerDir = SpriteEffects.FlipHorizontally;


            spriteBatch.Draw(toDraw, position - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, 0f, 
                new Vector2(toDraw.Width / 2, toDraw.Height / 2), 1f, flyerDir, 0.9f);
        }

        public override void doCollision(Player player)
        {

        }
    }
}
