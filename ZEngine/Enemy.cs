using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace ZEngine;

using Microsoft.Xna.Framework;

public class Enemy : Character {
    private int respawnTimer;
    private const int maxRespawnTime = 60;
    Random random = new Random();
    SoundEffect explosion;

    public Enemy() {
        
    }

    public Enemy(Vector2 inputPosition) {
        position = inputPosition;
    }

    public override void Initialize() {
        active = true;
        collidable = false;
        position.X = random.Next(0, 1100);
        base.Initialize();
    }

    public override void Load(ContentManager content) {
        image = TextureLoader.Load("enemy", content);
        explosion = content.Load<SoundEffect>("Audio\\explosion");
        base.Load(content);
    }

    public override void Update(List<GameObject> objects, Map map) {
        if (respawnTimer > 0) {
            respawnTimer--;
            if (respawnTimer <= 0) {
                Initialize();
            }
        }
        base.Update(objects, map);
    }

    public override void BulletResponse() {
        active = false;
        respawnTimer = maxRespawnTime;
        Player.score++;
        explosion.Play();
        base.BulletResponse();
    }
}