using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine.CardDemo;

public interface IFcsState {
    void Load(ContentManager content, GraphicsDevice graphicsDevice);
    void Update(GameTime gameTime);
    void Draw(SpriteBatch spriteBatch);
}


