using Microsoft.Xna.Framework.Content;

namespace ZEngine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Map {
    public List<Decor> decor = new List<Decor>();
    public List<Wall> walls = new List<Wall>();
    private Texture2D wallImage;

    public int mapWidth = 15;
    public int mapHeight = 9;
    public int tileSize = 128;

    public void LoadMap(ContentManager content) {
        for (var i = 0; i < decor.Count; i++) {
            decor[i].Load(content, decor[i].imagePath);
        }
    }

    public void Load(ContentManager content) {
        this.wallImage = TextureLoader.Load("pixel", content);
    }

    public Rectangle CheckCollision(Rectangle input) {
        for (int i = 0; i < this.walls.Count; i++) {
            if (this.walls[i] != null && this.walls[i].wall.Intersects(input) == true)
                return this.walls[i].wall;
        }
        return Rectangle.Empty;
    }

    public void Update(List<GameObject> objects) {
        for (var i = 0; i < decor.Count; i++) {
            decor[i].Update(objects, this);
        }
    }
    
    public void DrawWalls(SpriteBatch spriteBatch) {
        for (int i = 0; i < walls.Count; i++) {
            if (this.walls[i] != null && this.walls[i].active == true)
                spriteBatch.Draw(wallImage, new Vector2((int)walls[i].wall.X, (int)walls[i].wall.Y), walls[i].wall,
                    Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, .7f);
        }
    }
}

public class Wall {
    public Rectangle wall;
    public bool active = true;

    public Wall() {
    }

    public Wall(Rectangle inputRectangle) {
        this.wall = inputRectangle;
    }
}

public class Decor : GameObject {
    public string imagePath;
    public Rectangle sourceRect;

    public string Name {
        get { return imagePath; }
    }

    public Decor() {
        collidable = false;
    }

    public Decor(Vector2 inputPosition, string inputImagePath, float inputDepth) {
        position = inputPosition;
        imagePath = inputImagePath;
        layerDepth = inputDepth;
        active = true;
        collidable = false;
    }

    public virtual void Load(ContentManager content, string asset) {
        image = TextureLoader.Load(asset, content);
        image.Name = asset;
        boundingBoxWidth = image.Width;
        boundingBoxHeight = image.Height;
        if (sourceRect == Rectangle.Empty) {
            sourceRect = new Rectangle(0, 0, image.Width, image.Height);
        }
    }

    public void SetImage(Texture2D input, string newPath) {
        image = input;
        imagePath = newPath;
        boundingBoxWidth = sourceRect.Width = image.Width;
        boundingBoxHeight = sourceRect.Height = image.Height;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        if (image != null && active) {
            spriteBatch.Draw(image, position, sourceRect, drawColor, rotation, Vector2.Zero, scale, SpriteEffects.None,
                layerDepth);
        }
    }
}


