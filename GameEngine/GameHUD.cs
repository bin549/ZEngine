using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine;

public class GameHUD {
    SpriteFont font;

    public void Load(ContentManager content) {
        font = content.Load<SpriteFont>("Fonts\\Arial");
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Begin();
        spriteBatch.DrawString(font, "Score: "+Player.score.ToString(), Vector2.Zero, Color.White);
        spriteBatch.End();
    }
}