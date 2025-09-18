using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine;

public class Game : Microsoft.Xna.Framework.Game {
    private GraphicsDeviceManager graphics;
    private SpriteBatch spriteBatch;
    public List<GameObject> objects = new List<GameObject>();
    public Map map = new Map();
    GameHUD gameHud = new GameHUD();

    public Game() {
        this.graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        Resolution.Init(ref graphics);
        Resolution.SetVirtualResolution(1280, 720);
        Resolution.SetResolution(1280, 720, false);
    }

    protected override void Initialize() {
        base.Initialize();
        Camera.Initialize();
    }

    protected override void LoadContent() {
        this.spriteBatch = new SpriteBatch(GraphicsDevice);
        this.map.Load(Content);
        gameHud.Load(Content);
        this.LoadLevel();
    }

    protected override void Update(GameTime gameTime) {
        Input.Update();
        this.UpdateObjects();
        map.Update(objects);
        this.UpdateCamera();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime) {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        Resolution.BeginDraw();
        this.spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, 
            SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Camera.GetTransformMatrix());
        this.DrawObjects();
        this.map.DrawWalls(spriteBatch);
        this.spriteBatch.End();
        gameHud.Draw(spriteBatch);
        base.Draw(gameTime);
    }

    public void LoadLevel() { 
        this.objects.Add(new Player(new Vector2(640, 360)));
        this.objects.Add(new Enemy(new Vector2(300, 522)));
        this.map.walls.Add(new Wall(new Rectangle(256, 256, 256, 256)));
        this.map.walls.Add(new Wall(new Rectangle(0, 650, 1280, 128)));
        this.map.decor.Add(new Decor(Vector2.Zero, "background", 1f));
        this.map.LoadMap(Content);
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
        for (var i = 0; i < map.decor.Count; i++) {
            map.decor[i].Draw(spriteBatch);
        }
    }

    private void UpdateCamera() {
        if (objects.Count == 0) {
            return;
        }
        Camera.Update(objects[0].position);
    }
}

public static class Program {
    [STAThread]
    static void Main() {
        using (var game = new Game())
            game.Run();
    }
}