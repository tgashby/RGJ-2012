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
        private Dictionary<Color, Entity> m_entities;
        private const int tileWidth = 60;

        public Map(Game game, Texture2D tileMap, Dictionary<Color, Texture2D[]> tileTextures,
            Dictionary<Color, Entity> entities)
        {
            m_game = game;
            m_tileMap = tileMap;
            m_textures = tileTextures;
            m_entities = entities;
            m_tiles = new Tile[tileMap.Width, tileMap.Height];
            m_pixels = new Color[tileMap.Width * tileMap.Height];
            m_tileMap.GetData(m_pixels);
        }

        public Vector2 getPlayerSpawn()
        {
            return m_playerSpawn;
        }

        public void checkPlayerCollision(Player p)
        {
            int xmin = (int)(p.position.X - p.imageDimension().X / 2) / tileWidth;
            int xmax = (int)(p.position.X + p.imageDimension().X / 2) / tileWidth;
            int ymin = (int)(p.position.Y - p.imageDimension().Y / 2) / tileWidth;
            int ymax = (int)(p.position.Y + p.imageDimension().Y / 2) / tileWidth;

            int x = (int)(p.position.X) / tileWidth;

            if (m_tiles[x, ymin] != null)
            {
                p.position.Y = ymax * tileWidth + p.imageDimension().Y / 2;
                p.velocity.Y = 0;
                if (m_tiles[x, ymin].getType() == 1)
                {
                    p.health = 0;
                }
            }

            if (m_tiles[x, ymax] != null)
            {
                p.position.Y = ymax * tileWidth - p.imageDimension().Y / 2;
                p.jump = false;
                if (m_tiles[x, ymax].getType() == 1)
                {
                    p.health = 0;
                }
            }
            else
            {
                p.jump = true;
            }

            int y = (int)(p.position.Y) / tileWidth;

            if (m_tiles[xmin, y] != null)
            {
                p.position.X = (xmax) * tileWidth + p.imageDimension().X / 2;
                if (m_tiles[xmin, y].getType() == 1)
                {
                    p.health = 0;
                }
            }
            if (m_tiles[xmax, y] != null)
            {
                p.position.X = xmax * tileWidth - p.imageDimension().X / 2;
                if (m_tiles[xmax, y].getType() == 1)
                {
                    p.health = 0;
                }
            }
            
        }

        public void checkEnemyCollision(Entity ent)
        {
            int xmin = (int)(ent.position.X - ent.texture.Width / 2) / tileWidth;
            int xmax = (int)(ent.position.X + ent.texture.Width / 2) / tileWidth;
            int ymin = (int)(ent.position.Y - ent.texture.Height / 2) / tileWidth;
            int ymax = (int)(ent.position.Y + ent.texture.Height / 2) / tileWidth;

            int x = (int)(ent.position.X) / tileWidth;

            if (m_tiles[x, ymin] != null)
            {
                ent.position.Y = ymax * tileWidth + ent.texture.Height / 2;
                ent.velocity.Y = 0;
            }

            if (m_tiles[x, ymax] != null)
            {
                ent.position.Y = ymax * tileWidth - ent.texture.Height / 2;
            }

            int y = (int)(ent.position.Y) / tileWidth;
            
            if (m_tiles[xmin, y] != null)
            {
                ent.position.X = (xmin + 1) * tileWidth + ent.texture.Width / 2;
            }
            if (m_tiles[xmax, y] != null)
            {
                ent.position.X = xmax * tileWidth - ent.texture.Width / 2;
            }
        }

        public Vector2 teleportCheck(Player p, Vector2 displaced)
        {
            Vector2 result = p.position + displaced;
            int xmin = (int)(result.X - p.imageDimension().X / 2) / tileWidth;
            int xmax = (int)(result.X + p.imageDimension().X / 2) / tileWidth;
            int ymin = (int)(result.Y - p.imageDimension().Y / 2) / tileWidth;
            int ymax = (int)(result.Y + p.imageDimension().Y / 2) / tileWidth;

            int x = (int)(result.X) / tileWidth;

            if (m_tiles[x, ymin] != null)
            {
                result.Y = ymax * tileWidth + p.imageDimension().Y / 2;
                //p.velocity.Y = 0;
            }

            if (m_tiles[x, ymax] != null)
            {
                result.Y = ymax * tileWidth - p.imageDimension().Y / 2;
                //p.jump = false;
            }
            else
            {
                //p.jump = true;
            }

            int y = (int)(result.Y) / tileWidth;

            if (m_tiles[xmin, y] != null)
            {
                result.X = (xmax) * tileWidth + p.imageDimension().X / 2;
            }
            if (m_tiles[xmax, y] != null)
            {
                result.X = xmax * tileWidth - p.imageDimension().X / 2;
            }
            if (teleportCheck2(p, result))
            {
                return result;
            }
            else
            {
                p.health = 0;
                return result;
            }
        }

        public bool teleportCheck2(Player p, Vector2 result)
        {
            int xmin = (int)(result.X + 1 - p.imageDimension().X / 2) / tileWidth;
            int xmax = (int)(result.X - 1 + p.imageDimension().X / 2) / tileWidth;
            int ymin = (int)(result.Y + 1 - p.imageDimension().Y / 2) / tileWidth;
            int ymax = (int)(result.Y - 1 + p.imageDimension().Y / 2) / tileWidth;

            int x = (int)(result.X) / tileWidth;
            int y = (int)(result.Y) / tileWidth;

            if (m_tiles[x, ymin] != null)
            {
                return false;
                //p.velocity.Y = 0;
            }

            if (m_tiles[x, ymax] != null)
            {
                return false;
                //p.jump = false;
            }
            else
            {
                //p.jump = true;
            }

            if (m_tiles[xmin, y] != null)
            {
                return false;
            }
            if (m_tiles[xmax, y] != null)
            {
                return false;
            }
            return true;
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
                    if (tempPos.X > playerPos.X - 500 && tempPos.X < playerPos.X + 500
                        && tempPos.Y > playerPos.Y - 500 && tempPos.Y < playerPos.Y + 500)
                    {
                        t.Draw(batch, playerPos);
                    }
                }
            }
        }

        public List<Entity> makeTileMap()
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
                    else if (curColor.Equals(Color.Red))
                    {
                        m_tiles[x, y] = new Tile(new Vector2(x * tileWidth, y * tileWidth),
                            m_textures[curColor]);
                        m_tiles[x, y].setType(1);
                    }
                    else if (m_entities.ContainsKey(curColor))
                    {
                        m_tiles[x, y] = null;
                        Entity temp = m_entities[curColor];
                        Entities.instance().addEntity(temp, new Vector2(x * tileWidth, y * tileWidth), m_game);
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

            return Entities.instance().list();
        }

    }
}
