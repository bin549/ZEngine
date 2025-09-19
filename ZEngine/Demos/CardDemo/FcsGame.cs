using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace ZEngine.CardDemo;

public class FcsGame : Microsoft.Xna.Framework.Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    private IFcsState currentState;
    private SpriteFont? overlayFont;
    private string overlayText = string.Empty;
    private double overlayTimer;

    public FcsGame() {
        graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Resolution.Init(ref graphics);
        Resolution.SetVirtualResolution(512, 480);
        Resolution.SetResolution(512, 480, false);
        IsMouseVisible = true;
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (_, __) => {
            graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            Resolution.SetResolution(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight, false);
        };
    }

    protected override void Initialize() {
        base.Initialize();
        Camera.Initialize();
    }

    protected override void LoadContent() {
        spriteBatch = new SpriteBatch(GraphicsDevice);
        overlayFont = Content.Load<SpriteFont>("Fonts\\Arial");
        GoToMenu();
    }

    protected override void Update(GameTime gameTime) {
        currentState.Update(gameTime);
        if (overlayTimer > 0) {
            overlayTimer -= gameTime.ElapsedGameTime.TotalSeconds;
            if (overlayTimer < 0) overlayTimer = 0;
        }
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.DarkGreen);
        Resolution.BeginDraw();
        currentState.Draw(spriteBatch);
        if (overlayFont != null && overlayTimer > 0 && !string.IsNullOrEmpty(overlayText)) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp,
                DepthStencilState.Default, RasterizerState.CullNone, null, Resolution.getTransformationMatrix());
            spriteBatch.DrawString(overlayFont, overlayText, new Vector2(10, 10), Color.White);
            spriteBatch.End();
        }
        base.Draw(gameTime);
    }

    private void GoToMenu() {
        MusicPlayer.Stop();
        currentState = new MenuState(() => GoToGame());
        currentState.Load(Content, GraphicsDevice);
        ShowOverlay("Assets OK (Menu)");
    }

    private void GoToGame() {
        MusicPlayer.Stop();
        currentState = new GameState();
        currentState.Load(Content, GraphicsDevice);
        ShowOverlay("Assets OK (Game)");
    }

    private void ShowOverlay(string text) {
        overlayText = text;
        overlayTimer = 2.0;
        System.Console.WriteLine(text);
    }
}


