using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class Bullets
    {
        public static Bullets instance;
        public static Texture2D P_SMALL, PURPLE, RED, YELLOW;
        private List<Bullet> bullets;
        private Random rand;


        public Bullets()
        {
            Bullets.instance = this;
            bullets = new List<Bullet>();
            rand = new Random();
        }

        public void LoadContent(Game game)
        {
            P_SMALL = game.Content.Load<Texture2D>(@"images/playersmallbullet");
            PURPLE = game.Content.Load<Texture2D>(@"images/enemypurplebullet");
            RED = game.Content.Load<Texture2D>(@"images/enemyredbullet");
            YELLOW = game.Content.Load<Texture2D>(@"images/enemyyellowbullet");
        }

        public void addNewBullet(Vector2 position, Vector2 velocity, Texture2D image, Object shotBy, bool passthrough)
        {
            bullets.Add(new Bullet(position, velocity, image, shotBy, passthrough));
        }

        public void removeAll(Object objWhoShotThem)
        {
            if (objWhoShotThem == null)
            {
                bullets = new List<Bullet>();
            }
            else
            {
                for (int i = 0; i < bullets.Count; i++)
                {
                    if (bullets[i].shotBy == objWhoShotThem)
                    {
                        bullets.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public void checkEnemyCollisions(List<Entity> enemies)
        {
            foreach (Entity e in enemies)
            {
                foreach (Bullet b in bullets)
                {
                    if (!(b.shotBy is Entity))
                    {
                        if (b.position.X > e.position.X - e.texture.Width / 2 &&
                            b.position.X < e.position.X + e.texture.Width / 2 &&
                            b.position.Y > e.position.Y - e.texture.Height / 2 &&
                            b.position.Y < e.position.Y + e.texture.Height / 2)
                        {
                            if (b.image == Bullets.P_SMALL)
                            {
                                e.health -= 1;
                                b.alive = false;
                            }
                        }
                    }
                }
            }
        }

        public void checkPlayerCollisions(Player e)
        {
            foreach (Bullet b in bullets)
            {
                if ((b.shotBy is Entity))
                {
                    if (b.position.X > e.position.X - e.imageDimension().X / 2 &&
                        b.position.X < e.position.X + e.imageDimension().X / 2 &&
                        b.position.Y > e.position.Y - e.imageDimension().Y / 2 &&
                        b.position.Y < e.position.Y + e.imageDimension().Y / 2)
                    {
                        if (!e.shielding)
                            e.health -= 1;
                        b.alive = false;
                    }
                }
            }
        }

        public void cullDeadBullets(Map m)
        {
            foreach (Bullet b in bullets)
            {
                if (m.checkBulletCollision(b.position) && !b.pass && b.image != Bullets.RED)
                    b.alive = false;
                else if (m.checkBulletCollision(b.position) && !b.pass)
                {
                    if (rand.Next(2) == 0)
                    {
                        b.alive = false;
                    }
                    else
                    {
                        b.velocity.X = -b.velocity.X;
                        b.velocity.Y = -b.velocity.Y;
                    }
                }
            }

            for (int i = 0; i < bullets.Count; i++)
            {
                if (!bullets[i].alive)
                {
                    bullets.RemoveAt(i);
                    i--;
                }
            }
        }

        public void update(float gameTime)
        {
            foreach (Bullet b in bullets)
            {
                b.position += b.velocity * gameTime;
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            Vector2 pos = GameState.player.position;

            foreach (Bullet b in bullets)
            {
                float rot;
                if (b.position.Y - pos.Y > 0)
                    rot = (float)Math.Atan((b.position.X - pos.X) / (b.position.Y - pos.Y));
                else
                    rot = (float)Math.Atan((b.position.X - pos.X) / (b.position.Y - pos.Y - 0.0001)) + (float)MathHelper.Pi;

                spriteBatch.Draw(b.image, b.position - pos + Player.PLAYERDRAWPOS, null, Color.White, rot, 
                    new Vector2(b.image.Width / 2, b.image.Height / 2), 1f, SpriteEffects.None, 0.85f);
            }
        }

        private class Bullet
        {
            public Vector2 position, velocity;
            public Texture2D image;
            public Object shotBy;
            public bool alive = true;
            public bool pass = false;

            public Bullet(Vector2 position, Vector2 velocity, Texture2D image, Object shotBy, bool passthrough)
            {
                this.position = position;
                this.velocity = velocity;
                this.image = image;
                this.shotBy = shotBy;
                this.pass = passthrough;
            }
        }
    }
}
