using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine;

public class Player : FireCharacter {
    public static int score;

    public Player() {
    }

    public Player(Vector2 inputPosition) {
        this.position = inputPosition;
    }

    public override void Initialize() {
        score = 0;
        base.Initialize();
    }

    public override void Load(ContentManager content) {
        this.image = TextureLoader.Load("sprite", content);
        base.Load(content);
    }

    public override void Update(List<GameObject> objects, Map map) {
        this.CheckInput(objects, map);
        base.Update(objects, map);
    }

    private void CheckInput(List<GameObject> objects, Map map) {
        if (Input.IsKeyDown(Keys.A)) {
            base.MoveLeft();
        }
        if (Input.IsKeyDown(Keys.D)) {
            base.MoveRight();
        }
        if (Character.applyGravity) {
            if (Input.IsKeyDown(Keys.W)) {
                Jump(map);
            }
        } else {
            if (Input.IsKeyDown(Keys.W)) {
                base.MoveUp();
            }
            if (Input.IsKeyDown(Keys.S)) {
                base.MoveDown();
            }
        }
        if (Input.IsKeyDown(Keys.Space)) {
            Fire();
        }
    }

    public override void Draw(SpriteBatch spriteBatch) {
        base.Draw(spriteBatch);
    }
}