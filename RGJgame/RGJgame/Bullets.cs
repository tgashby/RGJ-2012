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
        public static Texture2D P_SMALL;
        private List<Bullet> bullets;


        public Bullets()
        {
            Bullets.instance = this;
            bullets = new List<Bullet>();
        }

        public void LoadContent(Game game)
        {
            P_SMALL = game.Content.Load<Texture2D>(@"images/playersmallbullet");
        }

        public void addNewBullet(Vector2 position, Vector2 velocity, Texture2D image, Object shotBy)
        {
            bullets.Add(new Bullet(position, velocity, image, shotBy));
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

            public Bullet(Vector2 position, Vector2 velocity, Texture2D image, Object shotBy)
            {
                this.position = position;
                this.velocity = velocity;
                this.image = image;
                this.shotBy = shotBy;
            }
        }
    }
}
