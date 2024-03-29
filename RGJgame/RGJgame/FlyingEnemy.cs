﻿using System;
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
        public float MOVEMENTSPEED = 0.28f, GRAVITY = 0.08f, VELOCITY = 0.25f, BULLETSPEED = 0.6f;
        public static Vector2 FLYERDRAWPOS = new Vector2(300, 300);
        public const int RUNCYCLE = 40;
        public const int SHOOTTIME = 50, MINDISTANCE = 300, MAXDISTANCE = 900, NUMSHOTS = 3;

        private Texture2D flyer1, flyer2, flyer3, flyer4;
        private int runtimer;
        private bool moving;
        private float shotTimer;
        private Random rand;
        private int numshots = NUMSHOTS;

        public FlyingEnemy(Vector2 pos)
            : base(pos)
        {
            health = 2;
            runtimer = 0;
            rand = new Random();

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

            Vector2 toPlayer = GameState.player.position - position;

            if (toPlayer.Length() > MAXDISTANCE)
            {
                toPlayer.X = 0;
                toPlayer.Y = 0;
                moving = false;
            }
            else if (toPlayer.Length() > MINDISTANCE && moving)
            {
                toPlayer.Normalize();
                toPlayer.Y -= GameState.player.GRAVITY - 0.08f;
                moving = true;
            }
            else if (toPlayer.Length() > MINDISTANCE)
            {
                toPlayer.Normalize();
                toPlayer.Y -= GameState.player.GRAVITY - 0.08f;
                moving = true;

                //shoot
                shotTimer -= elapsedTime;
                if (shotTimer <= 0)
                {
                    toPlayer.Normalize();
                    Bullets.instance.addNewBullet((position), toPlayer * BULLETSPEED, Bullets.RED, this, false);

                    shotTimer = SHOOTTIME;
                    numshots--;
                }
                if (numshots == 0)
                {
                    numshots = NUMSHOTS;
                    shotTimer = SHOOTTIME * 40;
                }
            }
            else
            {
                // shoot
                shotTimer -= elapsedTime;
                if (shotTimer <= 0)
                {
                    toPlayer.Normalize();
                    Bullets.instance.addNewBullet((position), toPlayer * BULLETSPEED, Bullets.RED, this, false);

                    shotTimer = SHOOTTIME;
                    numshots--;
                }
                if (numshots == 0)
                {
                    numshots = NUMSHOTS;
                    shotTimer = SHOOTTIME * 40;
                }


                toPlayer.X = 0;
                toPlayer.Y = 0;
                toPlayer.Y -= GameState.player.GRAVITY - 0.08f;
                moving = false;
            }


            position += toPlayer * VELOCITY * elapsedTime;
            
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
