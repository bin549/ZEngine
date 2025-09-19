using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine;

public class Bullet : GameObject {
    private const float speed = 12f;
    private Character owner;
    private int destoryTimer;
    private const int maxTimer = 100;

    public Bullet() {
        active = false;
    }

    public override void Load(ContentManager content) {
        this.image = TextureLoader.Load("bullet", content);
        base.Load(content);
    }

    public override void Update(List<GameObject> objects, Map map) {
        if (!active) {
            return;
        }
        position += direction * speed;
        CheckCollision(objects, map);
        destoryTimer--;
        if (destoryTimer <= 0 && active) {
            Destory();
        }
        base.Update(objects, map);
    }

    private void CheckCollision(List<GameObject> objects, Map map) {
        for (var i = 0; i < objects.Count; i++) {
            if (objects[i].active && objects[i] != owner && objects[i].CheckCollision(BoundingBox)) {
                Destory();
                objects[i].BulletResponse();
                return;
            }
        }
        if (map.CheckCollision(BoundingBox) != Rectangle.Empty) {
            Destory();
        }
    }

    public void Destory() {
        active = false;
    }

    public void Fire(Character character, Vector2 inputPosition, Vector2 inputDirection) {
        owner = character;
        position = inputPosition;
        direction = inputDirection;
        active = true;
        destoryTimer = maxTimer;
    }
}


