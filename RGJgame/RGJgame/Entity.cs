using System;
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
    public abstract class Entity
    {
        public Vector2 position, velocity, acceleration;
        public Texture2D texture;
        public int health;

        public Entity(Vector2 pos)
        {
            position = pos;
            velocity = new Vector2(0.0f, 0.0f);
            acceleration = new Vector2(0.0f, 0.0f);
        }

        public abstract void LoadContent(Game game);
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(SpriteBatch spriteBatch);
        public abstract void doCollision(Player player);
    }
}
