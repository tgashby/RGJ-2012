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
        public float MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f, BULLETSPEED = 0.8f;
        public static Vector2 SPAWNERDRAWPOS = new Vector2(300, 300);
        public const int RUNCYCLE = 30;
        public const int SHOOTTIME = 50, MINDISTANCE = 400, MAXDISTANCE = 900, NUMSHOTS = 10;

        private Texture2D[] spawner;
        private int runtimer;
        private Random rand;
        private float shotTimer;
        private int numshots = NUMSHOTS;

        public SpawnerEnemy(Vector2 pos)
            : base(pos - new Vector2(30, 30))
        {
            health = 10;
            rand = new Random();
        }

        public override void LoadContent(Game game)
        {
            spawner = new Texture2D[3];
            spawner[0] = game.Content.Load<Texture2D>(@"images/spawner1");
            spawner[1] = game.Content.Load<Texture2D>(@"images/spawner2");
            spawner[2] = game.Content.Load<Texture2D>(@"images/spawner3");

            texture = spawner[0];
        }

        public override void Update(GameTime gameTime)
        {
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds * Game1.CLOCKSPEED;

            runtimer+=gameTime.ElapsedGameTime.Milliseconds;

            if (runtimer > 50.0f)
                runtimer = 0;

            Vector2 toPlayer = -(position - GameState.player.position);

            if (toPlayer.Length() < MINDISTANCE)
            {
                shotTimer -= elapsedTime;
                if (shotTimer <= 0)
                {
                    toPlayer.Normalize();
                    Vector2 r = new Vector2((float)rand.NextDouble() - 0.5f, (float)rand.NextDouble() - 0.5f);
                    r /= 2;
                    Bullets.instance.addNewBullet((position + new Vector2(0, 40)), toPlayer * BULLETSPEED + r, Bullets.PURPLE, this, true);

                    shotTimer = SHOOTTIME;
                    numshots--;
                }
                if (numshots == 0)
                {
                    numshots = NUMSHOTS;
                    shotTimer = SHOOTTIME * 50;
                }
            }
            
            Collisions.check(this, GameState.player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Texture2D toDraw = texture;
            if (runtimer == 0)
                texture = spawner[rand.Next(3)];

            SpriteEffects spawnerDir = new SpriteEffects();

            if (velocity.X < 0)
                spawnerDir = SpriteEffects.FlipHorizontally;


            spriteBatch.Draw(toDraw, position - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, 0f, 
                new Vector2(toDraw.Width / 2, toDraw.Height / 2), 1f, spawnerDir, 0.8f);
        }

        public override void doCollision(Player player)
        {

        }
    }
}
