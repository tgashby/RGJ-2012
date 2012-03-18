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
    class SpawnerEnemy : Entity
    {
        public float MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f;
        public static Vector2 SPAWNERDRAWPOS = new Vector2(300, 300);
        public const int RUNCYCLE = 20;

        public float health;
        private Texture2D spawner1, spawner2, spawner3;
        private int runtimer;

        public SpawnerEnemy(Vector2 pos)
            : base(pos)
        {
            health = 20.0f;
        }

        public override void LoadContent(Game game)
        {
            spawner1 = game.Content.Load<Texture2D>(@"images/spawner1");
            spawner2 = game.Content.Load<Texture2D>(@"images/spawner2");
            spawner3 = game.Content.Load<Texture2D>(@"images/spawner3");
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds * Game1.CLOCKSPEED;

            if (runtimer > 1000.0f)
                runtimer = 0;

            runtimer++;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D toDraw;
            if (runtimer % RUNCYCLE < RUNCYCLE / 3)
                toDraw = spawner1;
            else if (runtimer % RUNCYCLE < RUNCYCLE / 2)
                toDraw = spawner2;
            else
                toDraw = spawner3;

            SpriteEffects spawnerDir = new SpriteEffects();

            if (velocity.X < 0)
                spawnerDir = SpriteEffects.FlipHorizontally;


            spriteBatch.Draw(toDraw, position - GameState.player.position, null, Color.White, 0f, new Vector2(20, 20), 1f, spawnerDir, 0.9f);
        }
    }
}
