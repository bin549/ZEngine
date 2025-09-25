using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine.CardDemo;

public class MenuState : IFcsState {
    private Texture2D? title;
    private Texture2D? pixel;
    private UIButton startButton = new UIButton();
    private Action? onStart;

    public MenuState(Action onStart) {
        this.onStart = onStart;
    }

    public void Load(ContentManager content, GraphicsDevice graphicsDevice) {
        title = TextureLoader.Load("Sprites/UI/title_screen", content);
        pixel = TextureLoader.Load("pixel", content);
        startButton.bounds = new Rectangle(512/2 - 120, 480 - 120, 240, 64);
        startButton.text = "Start (Enter)";
        startButton.Load(content);
        startButton.Clicked += () => onStart?.Invoke();
        MusicPlayer.PlayLoop(content, "Sounds/Musics/Home");
    }

    public void Update(GameTime gameTime) {
        Input.Update();
        if (Input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Enter)) {
            onStart?.Invoke();
        }
        startButton.Update();
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
            DepthStencilState.Default, RasterizerState.CullNone, null, Resolution.getTransformationMatrix());
        if (title != null) {
            float sx = 512f / title.Width;
            float sy = 480f / title.Height;
            float s = MathF.Min(sx, sy);
            var size = new Vector2(title.Width * s, title.Height * s);
            var pos = new Vector2((512 - size.X) / 2f, (480 - size.Y) / 2f);
            spriteBatch.Draw(title, pos, null, Color.White, 0f, Vector2.Zero, s, SpriteEffects.None, 0.8f);
        } else if (pixel != null) {
            var rect = new Rectangle(0, 0, 1280, 720);
            spriteBatch.Draw(pixel, rect, Color.DarkBlue);
        }
        startButton.Draw(spriteBatch);
        spriteBatch.End();
    }
}


