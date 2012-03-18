using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RGJgame
{
    class Collisions
    {
        public static void check(Entity ent, Player player)
        {
            if (ent.position.X + ent.texture.Width < player.position.X + Player.PLAYERDRAWPOS.X ||
                ent.position.Y + ent.texture.Height < player.position.Y + Player.PLAYERDRAWPOS.Y ||
                ent.position.X > player.position.X + player.imageDimension().X + Player.PLAYERDRAWPOS.X ||
                ent.position.Y > player.position.Y + player.imageDimension().Y + Player.PLAYERDRAWPOS.Y)
            {
                // No Collision
            }
            else
            {
                ent.doCollision(player);
                player.doCollision(ent);
            }
        }
    }
}
