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
    class GameState : State
    {
        private SpriteFont gameFont;
        private Texture2D background;
        public static Player player;
        public const int PARALAX = 10;
        public Map gameMap;
        public Dictionary<Color, Texture2D[]> tileTextures;
        public Bus bus;
        public Texture2D level;
        List<Entity> enemies;
        public Bullets bullets;

        public GameState(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>(@"logtext");
            background = Game.Content.Load<Texture2D>(@"backgrounds/bg");

            level = Game.Content.Load<Texture2D>(@"maps/testmap");

            tileTextures = new Dictionary<Color, Texture2D[]>();
            tileTextures.Add(Color.Black, new Texture2D[]{
               Game.Content.Load<Texture2D>(@"images/tile1"),
               Game.Content.Load<Texture2D>(@"images/tile2"),
               Game.Content.Load<Texture2D>(@"images/tile3"),
            });
            gameMap = new Map(Game, level, tileTextures);
            gameMap.makeTileMap();

            player = new Player(gameMap.getPlayerSpawn());
            player.LoadContent(Game);

            enemies = new List<Entity>();

            //enemies[0] = new FlyingEnemy(gameMap.getPlayerSpawn() + new Vector2(100, 30));
            //enemies[1] = new FlyingEnemy(gameMap.getPlayerSpawn() + new Vector2(50, 0));
            enemies.Add(new GuardEnemy(gameMap.getPlayerSpawn() + new Vector2(600, 0)));
            enemies.Add(new SpawnerEnemy(gameMap.getPlayerSpawn() + new Vector2(-300, 0)));

            foreach (Entity ent in enemies)
            {
                ent.LoadContent(Game);
            }

            bullets = new Bullets();
            bullets.LoadContent(Game);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, - player.position / PARALAX, null, Color.White, 0f, Vector2.Zero, 1, SpriteEffects.None, 0f);

            player.draw(spriteBatch);
            gameMap.Draw(spriteBatch, player.position);
            bullets.draw(spriteBatch);

            foreach (Entity ent in enemies)
            {
                ent.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            float gametime = gameTime.ElapsedGameTime.Milliseconds * Game1.CLOCKSPEED;

            player.update(gametime);
            gameMap.Update(gametime);
            bullets.update(gametime);

            gameMap.checkPlayerCollision(player);

            foreach (Entity ent in enemies)
            {
                ent.Update(gameTime);
            }

            bullets.checkEnemyCollisions(enemies);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].health <= 0)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            bullets.cullDeadBullets();
        }
    }
}
