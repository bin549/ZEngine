using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GameEngine;

public class GameObject {
    protected Texture2D image;
    public Vector2 position;
    public Color drawColor = Color.White;
    public float scale = 1f, rotation = 0f;
    protected float layerDepth = .5f;
    public bool active = true;
    protected Vector2 center;
    public bool collidable = true;
    protected int boundingBoxWidth, boundingBoxHeight;
    protected Vector2 boundingBoxOffset;
    private Texture2D boundingBoxImage;
    const bool drawBoundingBoxes = true;
    protected Vector2 direction = new Vector2(1, 0);

    public Rectangle BoundingBox {
        get {
            return new Rectangle((int)(this.position.X + this.boundingBoxOffset.X),
                (int)(this.position.Y + this.boundingBoxOffset.Y), boundingBoxWidth, boundingBoxHeight);
        }
    }

    public GameObject() {
    }

    public virtual void Initialize() {
    }

    public virtual void Load(ContentManager content) {
        this.boundingBoxImage = TextureLoader.Load("pixel", content);
        this.CalculateCenter();
        if (this.image != null) {
            this.boundingBoxWidth = this.image.Width;
            this.boundingBoxHeight = this.image.Height;
        }
    }

    public virtual void Update(List<GameObject> objects, Map map) {
        
    }

    public virtual bool CheckCollision(Rectangle input) {
        return BoundingBox.Intersects(input);
    }

    public virtual void Draw(SpriteBatch spriteBatch) {
        if (this.boundingBoxImage != null && drawBoundingBoxes == true && this.active == true)
            spriteBatch.Draw(boundingBoxImage, new Vector2(BoundingBox.X, BoundingBox.Y), BoundingBox,
                new Color(120, 120, 120, 120), 0f, Vector2.Zero, 1f, SpriteEffects.None, .1f);

        if (this.image != null && this.active)
            spriteBatch.Draw(image, position, null, drawColor, rotation, Vector2.Zero, scale, SpriteEffects.None,
                layerDepth);
    }

    public virtual void BulletResponse() {
        
    }

    private void CalculateCenter() {
        if (image == null)
            return;
        center.X = image.Width / 2;
        center.Y = image.Height / 2;
    }
}