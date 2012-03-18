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
    public class Entities
    {
        private static Entities m_instance;
        public static Texture2D P_SMALL;
        private List<Entity> entities;

        public static Entities instance()
        {
            if (m_instance == null)
            {
                m_instance = new Entities();
            }

            return m_instance;
        }

        private Entities()
        {
            entities = new List<Entity>();
        }

        public void addEntity(Entity type, Vector2 position, Game game)
        {
            Type t = type.GetType();

            if (t == typeof(InfoPad))
            {
                entities.Add(new InfoPad(position, ((InfoPad)type).power));
            }
            else if (t == typeof(FlyingEnemy))
            {
                entities.Add(new FlyingEnemy(position));
            }
            else if (t == typeof(SpawnerEnemy))
            {
                entities.Add(new SpawnerEnemy(position));
            }
            else if (t == typeof(GuardEnemy))
            {
                entities.Add(new GuardEnemy(position));
            }
            else
            { }

            entities.Last<Entity>().LoadContent(game);
        }

        public void update(GameTime gameTime)
        {
            foreach (Entity ent in entities)
            {
                ent.Update(gameTime);
            }
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (Entity ent in entities)
            {
                ent.Draw(spriteBatch);    
            }
        }

        public List<Entity> list()
        {
            return entities;
        }
    }
}
