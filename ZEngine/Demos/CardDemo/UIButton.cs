using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZEngine.CardDemo;

public class UIButton {
    public Rectangle bounds;
    public string text = string.Empty;
    private SpriteFont? font;
    public event Action? Clicked;

    public void Load(ContentManager content) {
        font = content.Load<SpriteFont>("Fonts/Arial");
    }

    public void Update() {
        var mouse = Mouse.GetState();
        if (mouse.LeftButton == ButtonState.Pressed && bounds.Contains(mouse.X, mouse.Y)) {
            Clicked?.Invoke();
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        if (font == null) return;
        var size = font.MeasureString(text);
        var pos = new Vector2(bounds.X + (bounds.Width - size.X) / 2, bounds.Y + (bounds.Height - size.Y) / 2);
        spriteBatch.DrawString(font, text, pos, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.6f);
    }
}


