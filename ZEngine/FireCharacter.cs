using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine;

public class FireCharacter : Character {
    List<Bullet> bullets = new List<Bullet>();
    const int numOfBullets = 3;
    
    public FireCharacter() {}

    public override void Initialize() {
        if (bullets.Count == 0) {
            for (var i = 0; i < numOfBullets; i++) {
                bullets.Add(new Bullet());
            }
        }
        base.Initialize();
    }

    public override void Load(ContentManager content) {
        for (int i = 0; i < numOfBullets; i++) {
            bullets[i].Load(content);
        }
        base.Load(content);
    }
    
    public override void Update(List<GameObject> objects, Map map) {
        for (int i = 0; i < numOfBullets; i++) {
            bullets[i].Update(objects, map);
        }
        base.Update(objects, map);
    }

    public void Fire() {
        for (int i = 0; i < numOfBullets; i++) {
            if (!bullets[i].active) {
                bullets[i].Fire(this, position, direction);
                break;
            }
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        for (int i = 0; i < numOfBullets; i++) {
            bullets[i].Draw(spriteBatch);
        }
        base.Draw(spriteBatch);
    }
}