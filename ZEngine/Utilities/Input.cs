using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace ZEngine;

public static class Input {
    private static KeyboardState keyboardState = Keyboard.GetState();
    private static KeyboardState lastKeyboardState;

    private static MouseState mouseState;
    private static MouseState lastMouseState;

    public static void Update() {
        lastKeyboardState = keyboardState;
        keyboardState = Keyboard.GetState();

        lastMouseState = mouseState;
        mouseState = Mouse.GetState();
    }

    public static bool IsKeyDown(Keys input) {
        return keyboardState.IsKeyDown(input);
    }

    public static bool IsKeyUp(Keys input) {
        return keyboardState.IsKeyUp(input);
    }

    public static bool KeyPressed(Keys input) {
        if (keyboardState.IsKeyDown(input) == true && lastKeyboardState.IsKeyDown(input) == false)
            return true;
        else
            return false;
    }

    public static bool MouseLeftDown() {
        if (mouseState.LeftButton == ButtonState.Pressed)
            return true;
        else
            return false;
    }

    public static bool MouseRightDown() {
        if (mouseState.RightButton == ButtonState.Pressed)
            return true;
        else
            return false;
    }

    public static bool MouseLeftClicked() {
        if (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released)
            return true;
        else
            return false;
    }

    public static bool MouseRightClicked() {
        if (mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released)
            return true;
        else
            return false;
    }

    public static Vector2 MousePositionCamera() {
        Vector2 mousePosition = Vector2.Zero;
        mousePosition.X = mouseState.X;
        mousePosition.Y = mouseState.Y;
        return ScreenToWorld(mousePosition);
    }

    public static Vector2 LastMousePositionCamera() {
        Vector2 mousePosition = Vector2.Zero;
        mousePosition.X = lastMouseState.X;
        mousePosition.Y = lastMouseState.Y;

        return ScreenToWorld(mousePosition);
    }

    private static Vector2 ScreenToWorld(Vector2 input) {
        input.X -= Resolution.VirtualViewportX;
        input.Y -= Resolution.VirtualViewportY;

        return Vector2.Transform(input, Matrix.Invert(Camera.GetTransformMatrix()));
    }
}