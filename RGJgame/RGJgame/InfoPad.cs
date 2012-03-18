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
    class InfoPad : Entity
    {
        public const int ANIMTIME = 100; 

        public String power;
        Texture2D[] anim;
        int totaltime;


        public InfoPad(Vector2 pos, String power)
            : base(pos)
        {
            this.power = power;
            health = 9001;
        }

        public override void LoadContent(Game game)
        {
            anim = new Texture2D[4];
            anim[0] = game.Content.Load<Texture2D>(@"images/infopad1");
            anim[1] = game.Content.Load<Texture2D>(@"images/infopad2");
            anim[2] = game.Content.Load<Texture2D>(@"images/infopad3");
            anim[3] = game.Content.Load<Texture2D>(@"images/infopad2");

            texture = game.Content.Load<Texture2D>(@"images/infoPanel");
        }

        public override void Update(GameTime gameTime)
        {
            Collisions.check(this, GameState.player);

            totaltime += gameTime.ElapsedGameTime.Milliseconds;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim[(totaltime / ANIMTIME) % 4], position - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, 0f,
                    new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0.9f);
        }

        public override void doCollision(Player player)
        {

        }
    }
}