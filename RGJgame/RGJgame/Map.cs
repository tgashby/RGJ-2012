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
    public class Map
    {
        private Texture2D m_tileMap;
        private Game m_game;
        private Color[] m_pixels;
        private Tile[,] m_tiles;
        private Vector2 m_playerSpawn;
        private Dictionary<Color, Texture2D[]> m_textures;
        private const int tileWidth = 20;

        public Map(Game game, Texture2D tileMap, Dictionary<Color, Texture2D[]> tileTextures)
        {
            m_game = game;
            m_tileMap = tileMap;
            m_textures = tileTextures;
            m_tiles = new Tile[tileMap.Width, tileMap.Height];
            m_pixels = new Color[tileMap.Width * tileMap.Height];
            m_tileMap.GetData(m_pixels);
        }

        public void Update(float gameTime)
        {
            foreach (Tile t in m_tiles)
            {
                if (t != null)
                    t.Update(gameTime);
            }
        }

        public void Draw(SpriteBatch batch, Vector2 playerPos)
        {
            foreach (Tile t in m_tiles)
            {
                if (t != null)
                {
                    Vector2 tempPos = t.getPosition();
                    if (tempPos.X > playerPos.X - 400 && tempPos.X < playerPos.X + 400
                        && tempPos.Y > playerPos.Y - 400 && tempPos.Y < playerPos.Y + 400)
                    {
                        t.Draw(batch, playerPos);
                    }
                }
            }
        }

        public Bus makeTileMap()
        {
            Bus bus = new Bus();
            Color curColor;
            for (int y = 0; y < m_tileMap.Height; y++)
            {
                for (int x = 0; x < m_tileMap.Width; x++)
                {
                    curColor = m_pixels[(y * m_tileMap.Width) + x];
                    if (curColor.Equals(Color.White))
                    {
                        m_tiles[x, y] = null;
                    }
                    else if (curColor.Equals(Color.Cyan))
                    {
                        m_tiles[x, y] = null;
                        m_playerSpawn = new Vector2(x * tileWidth, y * tileWidth);
                    }
                    else if (m_textures.ContainsKey(curColor))
                    {
                        m_tiles[x, y] = new Tile(new Vector2(x * tileWidth, y * tileWidth),
                            m_textures[curColor]);
                    }
                    else
                    {
                        m_tiles[x, y] = null;
                    }
                }
            }
            return bus;
        }

    }
}
