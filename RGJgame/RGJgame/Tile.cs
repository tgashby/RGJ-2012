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
    class Tile
    {
        private Vector2 m_position;
        private Texture2D[] m_texture;
        private Texture2D m_currentTexture;
        private int m_type;
        private float m_textureTimer;

        public Tile(Vector2 position, Texture2D[] texture)
        {
            m_position = position;
            m_texture = texture;
            m_currentTexture = m_texture[0];
        }

        public Vector2 getPosition()
        {
            return m_position;
        }

        public void setPosition(Vector2 spawn)
        {
            m_position = spawn;
        }

        public void Update(float gameTime)
        {
            Random rand = new Random();
            int index = rand.Next(m_texture.Length);
            m_textureTimer += gameTime;
            if (m_textureTimer > 0.35) {
                m_textureTimer = 0;
                m_currentTexture = m_texture[index];
            }
        }

        public void Draw(SpriteBatch batch, Vector2 pos)
        {
            batch.Draw(m_currentTexture, m_position - pos + new Vector2(300, 300), null, Color.White,
                0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0.3f);
        }

    }
}
