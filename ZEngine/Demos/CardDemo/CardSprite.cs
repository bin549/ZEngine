using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine.CardDemo;

public class CardSprite : GameObject {
    public string name = string.Empty;
    private bool dragging;
    private Vector2 dragOffset;
    private const int targetCardHeight = 96; // 512x480 下的目标牌高度

    public void Load(ContentManager content, string nameWithoutFolder) {
        name = nameWithoutFolder;
        image = TextureLoader.Load($"Card/Sprites/cards/{name}", content);
        // 按目标高度等比缩放
        if (image != null && image.Height > 0) {
            scale = (float)targetCardHeight / image.Height;
        }
        base.Load(content);
        layerDepth = 0.5f;
        // 同步缩放后的碰撞框尺寸
        if (image != null) {
            boundingBoxWidth = (int)(image.Width * scale);
            boundingBoxHeight = (int)(image.Height * scale);
        }
        collidable = false;
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
    }

    public void UpdateDrag() {
        var mouse = Microsoft.Xna.Framework.Input.Mouse.GetState();
        // 屏幕坐标 -> 虚拟坐标（与绘制一致）
        var mousePos = new Vector2(mouse.X - Resolution.VirtualViewportX, mouse.Y - Resolution.VirtualViewportY);
        mousePos = Vector2.Transform(mousePos, Matrix.Invert(Resolution.getTransformationMatrix()));
        var leftDown = mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed;
        if (!dragging && leftDown && BoundingBox.Contains((int)mousePos.X, (int)mousePos.Y)) {
            dragging = true;
            dragOffset = mousePos - position;
        }
        if (dragging) {
            if (leftDown) {
                position = mousePos - dragOffset;
            } else {
                dragging = false;
            }
        }
    }
}


