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
            if (ent.position.X + ent.texture.Width / 2  < player.position.X - player.imageDimension().X / 2 ||
                ent.position.Y + ent.texture.Height / 2 < player.position.Y - player.imageDimension().Y / 2 ||
                ent.position.X - ent.texture.Width / 2  > player.position.X + player.imageDimension().X / 2 ||
                ent.position.Y - ent.texture.Height / 2 > player.position.Y + player.imageDimension().Y / 2)
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
