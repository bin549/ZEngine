using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine.CardDemo;

public class GameState : IFcsState {
    private Texture2D? background;
    private readonly List<CardSprite> dealtCards = new List<CardSprite>();
    private Deck deck = new Deck();
    private Vector2[] targetPositions = new Vector2[5];
    private Vector2[] currentPositions = new Vector2[5];
    private float dealT;
    private const float dealDuration = 0.35f;
    private int dealingIndex;

    public void Load(ContentManager content, GraphicsDevice graphicsDevice) {
        background = TextureLoader.Load("Card/Sprites/Environments/Game_BG", content);
        deck.Initialize(content);
        MusicPlayer.PlayLoop(content, "Card/Sounds/Musics/Time Passes");
        var start = new Vector2(512/2f - 2 * 60, 360f);
        int gap = 60;
        for (int i = 0; i < 5; i++) {
            targetPositions[i] = new Vector2(start.X + i * gap, start.Y);
            currentPositions[i] = new Vector2(1280/2, 720 + 200);
        }
        DealFive();
    }

    public void Update(GameTime gameTime) {
        Input.Update();
        UpdateDealing((float)gameTime.ElapsedGameTime.TotalSeconds);
        for (int i = 0; i < dealtCards.Count; i++) {
            dealtCards[i].UpdateDrag();
        }
        if (Input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.Space)) {
            DealFive();
        }
        if (Input.KeyPressed(Microsoft.Xna.Framework.Input.Keys.R)) {
            deck.Shuffle();
            DealFive();
        }
    }

    public void Draw(SpriteBatch spriteBatch) {
        spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.LinearClamp,
            DepthStencilState.Default, RasterizerState.CullNone, null, Resolution.getTransformationMatrix());
        if (background != null) {
            float sx = 512f / background.Width;
            float sy = 480f / background.Height;
            float s = MathF.Min(sx, sy);
            var size = new Vector2(background.Width * s, background.Height * s);
            var pos = new Vector2((512 - size.X) / 2f, (480 - size.Y) / 2f);
            spriteBatch.Draw(background, pos, null, Color.White, 0f, Vector2.Zero, s,
                SpriteEffects.None, 0.9f);
        }
        for (int i = 0; i < dealtCards.Count; i++) {
            var card = dealtCards[i];
            var pos = currentPositions[i];
            card.position = pos;
            card.Draw(spriteBatch);
        }
        spriteBatch.End();
    }

    private void DealFive() {
        dealtCards.Clear();
        dealingIndex = 0;
        dealT = 0f;
        for (int i = 0; i < 5; i++) {
            var card = deck.Draw();
            if (card == null) {
                deck.Shuffle();
                card = deck.Draw();
            }
            currentPositions[i] = targetPositions[i];
            card!.position = currentPositions[i];
            dealtCards.Add(card);
        }
    }

    private void UpdateDealing(float dt) {
        if (dealingIndex >= dealtCards.Count) return;
        dealT += dt;
        float t = MathF.Min(1f, dealT / dealDuration);
        currentPositions[dealingIndex] = Vector2.Lerp(currentPositions[dealingIndex], targetPositions[dealingIndex], t);
        if (t >= 1f) {
            dealT = 0f;
            dealingIndex++;
        }
    }
}


