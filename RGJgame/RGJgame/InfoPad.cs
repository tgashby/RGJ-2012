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
        public String power;

        public InfoPad(Vector2 pos, String power)
            : base(pos)
        {
            this.power = power;
            health = 9001;
        }

        public override void LoadContent(Game game)
        {
            texture = game.Content.Load<Texture2D>(@"images/infoPanel");
        }

        public override void Update(GameTime gameTime)
        {
            Collisions.check(this, GameState.player);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position - GameState.player.position + Player.PLAYERDRAWPOS, null, Color.White, 0f,
                    new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0.9f);
        }

        public override void doCollision(Player player)
        {

        }
    }
}