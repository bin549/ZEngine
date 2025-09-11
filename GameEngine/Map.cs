using Microsoft.Xna.Framework.Content;

namespace GameEngine;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Map {
    public List<Wall> walls = new List<Wall>();
    private Texture2D wallImage;

    public int mapWidth = 15;
    public int mapHeight = 9;
    public int tileSize = 128;

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