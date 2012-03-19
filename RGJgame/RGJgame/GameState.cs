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
    class GameState : State
    {
        private SpriteFont gameFont;
        private Texture2D background;
        public static Player player;
        public const int PARALAX = 10;
        public static Map gameMap;
        public Dictionary<Color, Texture2D[]> tileTextures;
        public Dictionary<Color, Entity> entities;
        public Bus bus;
        public Texture2D level;
        public Texture2D texture_infoPad;
        List<Entity> enemies;
        public Bullets bullets;

        public GameState(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            gameFont = Game.Content.Load<SpriteFont>(@"logtext");
            background = Game.Content.Load<Texture2D>(@"backgrounds/chipbg");

            level = Game.Content.Load<Texture2D>(@"maps/HugeLevel");

            tileTextures = new Dictionary<Color, Texture2D[]>();
            tileTextures.Add(Color.Black, new Texture2D[]{
               Game.Content.Load<Texture2D>(@"images/tile1"),
               Game.Content.Load<Texture2D>(@"images/tile2"),
               Game.Content.Load<Texture2D>(@"images/tile3"),
            });
            tileTextures.Add(Color.Red, new Texture2D[]{
                Game.Content.Load<Texture2D>(@"images/spike1"),
                Game.Content.Load<Texture2D>(@"images/spike2"),
                Game.Content.Load<Texture2D>(@"images/spike3"),
            });

            entities = new Dictionary<Color, Entity>();
            entities.Add(new Color(0, 100, 100), new InfoPad(Vector2.One, PlayerPower.LOW_GRAV));                   // 0
            entities.Add(new Color(5, 100, 100), new InfoPad(Vector2.One, PlayerPower.MASSIVE_GRAV));               // 5
            entities.Add(new Color(10, 100, 100), new InfoPad(Vector2.One, PlayerPower.GRAVITY_NORMAL));            // 10
            entities.Add(new Color(15, 100, 100), new InfoPad(Vector2.One, PlayerPower.GRAVITY_OFF));               // 15
            entities.Add(new Color(20, 100, 100), new InfoPad(Vector2.One, PlayerPower.REV_GRAVITY));               // 20
            entities.Add(new Color(25, 100, 100), new InfoPad(Vector2.One, PlayerPower.WEAK_JUMP));                 // 25
            entities.Add(new Color(30, 100, 100), new InfoPad(Vector2.One, PlayerPower.SUPER_JUMP));                // 30
            entities.Add(new Color(35, 100, 100), new InfoPad(Vector2.One, PlayerPower.NORMAL_JUMP));               // 35
            entities.Add(new Color(40, 100, 100), new InfoPad(Vector2.One, PlayerPower.MOVEMENT_FAST));             // 40
            entities.Add(new Color(45, 100, 100), new InfoPad(Vector2.One, PlayerPower.MOVEMENT_SLOW));             // 45
            entities.Add(new Color(50, 100, 100), new InfoPad(Vector2.One, PlayerPower.MOVEMENT_NORMAL));           // 50
            entities.Add(new Color(55, 100, 100), new InfoPad(Vector2.One, PlayerPower.BULLET1));                   // 55
            entities.Add(new Color(60, 100, 100), new InfoPad(Vector2.One, PlayerPower.BULLET2));                   // 60
            entities.Add(new Color(65, 100, 100), new InfoPad(Vector2.One, PlayerPower.BULLET_SPREAD));             // 65
            entities.Add(new Color(70, 100, 100), new InfoPad(Vector2.One, PlayerPower.BULLET_DIAGONAL));           // 70
            entities.Add(new Color(75, 100, 100), new InfoPad(Vector2.One, PlayerPower.BULLET_TRIPLE));             // 75
            entities.Add(new Color(80, 100, 100), new InfoPad(Vector2.One, PlayerPower.STRONG_BULLETS));            // 80
            entities.Add(new Color(85, 100, 100), new InfoPad(Vector2.One, PlayerPower.FREEZE_ENEMIES));            // 85
            entities.Add(new Color(90, 100, 100), new InfoPad(Vector2.One, PlayerPower.BURN_ENEMIES));              // 90
            entities.Add(new Color(95, 100, 100), new InfoPad(Vector2.One, PlayerPower.THROW_ENEMY));               // 95
            entities.Add(new Color(100, 100, 100), new InfoPad(Vector2.One, PlayerPower.SUPERLAZER));                     // 100
            entities.Add(new Color(105, 100, 100), new InfoPad(Vector2.One, PlayerPower.TELEPORT));                 // 105
            entities.Add(new Color(110, 100, 100), new InfoPad(Vector2.One, PlayerPower.LAZER));                    // 110
            entities.Add(new Color(115, 100, 100), new InfoPad(Vector2.One, PlayerPower.DECREASE_ENEMY_SPEED));     // 115
            entities.Add(new Color(120, 100, 100), new InfoPad(Vector2.One, PlayerPower.GET_ENEMY_ID));             // 120
            entities.Add(new Color(125, 100, 100), new InfoPad(Vector2.One, PlayerPower.KILL_ID));                  // 125
            entities.Add(new Color(130, 100, 100), new InfoPad(Vector2.One, PlayerPower.ROOT_PRIV));                // 130
            entities.Add(new Color(135, 100, 100), new InfoPad(Vector2.One, PlayerPower.OVERCLOCK4));               // 135
            entities.Add(new Color(140, 100, 100), new InfoPad(Vector2.One, PlayerPower.OVERCLOCK2));               // 140
            entities.Add(new Color(145, 100, 100), new InfoPad(Vector2.One, PlayerPower.CLOCK1));                   // 145
            entities.Add(new Color(150, 100, 100), new InfoPad(Vector2.One, PlayerPower.UNDERCLOCK2));              // 150
            entities.Add(new Color(155, 100, 100), new InfoPad(Vector2.One, PlayerPower.UNDERCLOCK4));              // 155
            entities.Add(new Color(160, 100, 100), new InfoPad(Vector2.One, PlayerPower.RESET));                    // 160
            
           
            entities.Add(new Color(50, 255, 255), new GuardEnemy(Vector2.One));     // 50
            entities.Add(new Color(60, 255, 255), new SpawnerEnemy(Vector2.One));   // 60
            entities.Add(new Color(70, 255, 255), new FlyingEnemy(Vector2.One));    // 70
            gameMap = new Map(Game, level, tileTextures, entities);

            enemies = gameMap.makeTileMap();

            player = new Player(gameMap.getPlayerSpawn());
            player.LoadContent(Game);

          /*  enemies = new List<Entity>();

            enemies.Add(new GuardEnemy(gameMap.getPlayerSpawn() + new Vector2(600, 0)));
            enemies.Add(new SpawnerEnemy(gameMap.getPlayerSpawn() + new Vector2(-300, 0)));
            enemies.Add(new InfoPad(gameMap.getPlayerSpawn() + new Vector2(150, 0), PlayerPower.GRAVITY_OFF));*/

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
                gameMap.checkEnemyCollision(ent);
            }

            bullets.checkEnemyCollisions(enemies);
            bullets.checkPlayerCollisions(player);
            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].health <= 0)
                {
                    enemies.RemoveAt(i);
                    i--;
                }
            }

            bullets.cullDeadBullets(gameMap);
        }
    }
}
