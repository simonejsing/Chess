using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess2
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }

        public Sprite(Texture2D texture)
        {
            this.Texture = texture;
        }
    }

    public class SpriteLoader
    {
        private ContentManager manager;

        public SpriteLoader(ContentManager manager)
        {
            this.manager = manager;
        }

        public Sprite Load(string filename)
        {
            return new Sprite(this.manager.Load<Texture2D>(@"sprites\" + filename));
        }
    }
}
