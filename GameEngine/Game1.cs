using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine;

public class Game1 : Microsoft.Xna.Framework.Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    public List<GameObject> objects = new List<GameObject>();
    public Map map = new Map();

    public Game1() {
        this.graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        this.graphics.PreferredBackBufferWidth = 1280;
        this.graphics.PreferredBackBufferHeight = 720;
        this.graphics.IsFullScreen = true;
        this.graphics.ApplyChanges();
    }

    protected override void Initialize() {
        base.Initialize();
    }

    protected override void LoadContent() {
        this.spriteBatch = new SpriteBatch(GraphicsDevice);
        this.map.Load(Content);
        this.LoadLevel();
    }

    protected override void Update(GameTime gameTime) {
        Input.Update();
        this.UpdateObjects();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
        this.DrawObjects();
        this.map.DrawWalls(spriteBatch);
        this.spriteBatch.End();
        base.Draw(gameTime);
    }

    public void LoadLevel() {
        this.objects.Add(new Player(new Vector2(640, 360)));
        this.map.walls.Add(new Wall(new Rectangle(256, 256, 256, 256)));
        this.map.walls.Add(new Wall(new Rectangle(0, 650, 1280, 128)));
        this.LoadObjects();
    }

    public void LoadObjects() {
        for (var i = 0; i < objects.Count; i++) {
            objects[i].Initialize();
            objects[i].Load(Content);
        }
    }

    public void UpdateObjects() {
        for (var i = 0; i < objects.Count; i++) {
            objects[i].Update(objects, map);
        }
    }

    public void DrawObjects() {
        for (var i = 0; i < objects.Count; i++) {
            objects[i].Draw(spriteBatch);
        }
    }
}

public static class Program {
    [STAThread]
    static void Main() {
        using (var game = new Game1())
            game.Run();
    }
}