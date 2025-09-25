using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ZEngine.SnakeDemo;

public class SnakeGame : Microsoft.Xna.Framework.Game {
	private GraphicsDeviceManager graphics;
	private SpriteBatch spriteBatch;
	private Texture2D pixel;

	private const int CellSize = 16;
	private int columns;
	private int rows;

	private List<Point> snake = new List<Point>();
	private Direction currentDirection = Direction.Right;
	private Direction nextDirection = Direction.Right;
	private double stepIntervalSeconds = 0.12;
	private double stepAccumulatorSeconds = 0.0f;
	private bool isAlive = true;

	private Point food;

	private int score = 0;
	private int best = 0;

	public SnakeGame() {
		graphics = new GraphicsDeviceManager(this);
		Content.RootDirectory = "Content";
		Resolution.Init(ref graphics);
		Resolution.SetVirtualResolution(1280, 720);
		Resolution.SetResolution(1280, 720, false);
	}

	protected override void Initialize() {
		base.Initialize();
		Camera.Initialize();
		columns = Resolution.VirtualWidth / CellSize;
		rows = Resolution.VirtualHeight / CellSize;
		ResetGame();
	}

	protected override void LoadContent() {
		spriteBatch = new SpriteBatch(GraphicsDevice);
		pixel = new Texture2D(GraphicsDevice, 1, 1);
		pixel.SetData(new[] { Color.White });
	}

	protected override void Update(GameTime gameTime) {
		Input.Update();
		HandleInput();
		if (isAlive) {
			stepAccumulatorSeconds += gameTime.ElapsedGameTime.TotalSeconds;
			while (stepAccumulatorSeconds >= stepIntervalSeconds) {
				Step();
				stepAccumulatorSeconds -= stepIntervalSeconds;
			}
		} else {
			if (Input.KeyPressed(Keys.Space)) {
				ResetGame();
			}
		}
		base.Update(gameTime);
	}

	protected override void Draw(GameTime gameTime) {
		GraphicsDevice.Clear(Color.Black);
		Resolution.BeginDraw();
		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone, null, Resolution.getTransformationMatrix());
		var fullWidth = columns * CellSize;
		var fullHeight = rows * CellSize;
		spriteBatch.Draw(pixel, new Rectangle(0, 0, fullWidth, CellSize), Color.DimGray);
		spriteBatch.Draw(pixel, new Rectangle(0, (rows - 1) * CellSize, fullWidth, CellSize), Color.DimGray);
		spriteBatch.Draw(pixel, new Rectangle(0, 0, CellSize, fullHeight), Color.DimGray);
		spriteBatch.Draw(pixel, new Rectangle((columns - 1) * CellSize, 0, CellSize, fullHeight), Color.DimGray);
		DrawCell(food, Color.Red);
		for (int i = 0; i < snake.Count; i++) {
			var color = i == snake.Count - 1 ? new Color(40, 220, 40) : new Color(20, 160, 20);
			DrawCell(snake[i], color);
		}
		spriteBatch.End();
		base.Draw(gameTime);
	}

	private void HandleInput() {
		if (Input.KeyPressed(Keys.Up) || Input.KeyPressed(Keys.W)) TrySetDirection(Direction.Up);
		if (Input.KeyPressed(Keys.Down) || Input.KeyPressed(Keys.S)) TrySetDirection(Direction.Down);
		if (Input.KeyPressed(Keys.Left) || Input.KeyPressed(Keys.A)) TrySetDirection(Direction.Left);
		if (Input.KeyPressed(Keys.Right) || Input.KeyPressed(Keys.D)) TrySetDirection(Direction.Right);
	}

	private void TrySetDirection(Direction dir) {
		if (snake.Count > 1 && IsOpposite(dir, currentDirection)) return;
		nextDirection = dir;
	}

	private bool IsOpposite(Direction a, Direction b) {
		return (a == Direction.Up && b == Direction.Down)
			|| (a == Direction.Down && b == Direction.Up)
			|| (a == Direction.Left && b == Direction.Right)
			|| (a == Direction.Right && b == Direction.Left);
	}

	private void Step() {
		currentDirection = nextDirection;
		var head = snake[^1];
		var delta = DirectionToDelta(currentDirection);
		var newHead = new Point(head.X + delta.X, head.Y + delta.Y);
		if (newHead.X < 0 || newHead.Y < 0 || newHead.X >= columns || newHead.Y >= rows) {
			Die();
			return;
		}
		for (int i = 0; i < snake.Count; i++) {
			if (snake[i] == newHead) {
				Die();
				return;
			}
		}
		snake.Add(newHead);
		if (newHead == food) {
			score += 10;
			SpawnFood();
			stepIntervalSeconds = Math.Max(0.06, stepIntervalSeconds - 0.002);
		} else {
			snake.RemoveAt(0);
		}
	}

	private static Point DirectionToDelta(Direction dir) {
		return dir switch {
			Direction.Up => new Point(0, -1),
			Direction.Down => new Point(0, 1),
			Direction.Left => new Point(-1, 0),
			_ => new Point(1, 0)
		};
	}

	private void ResetGame() {
		best = Math.Max(best, score);
		score = 0;
		stepIntervalSeconds = 0.12;
		isAlive = true;

		snake.Clear();
		var start = new Point(columns / 2, rows / 2);
		snake.Add(new Point(start.X - 2, start.Y));
		snake.Add(new Point(start.X - 1, start.Y));
		snake.Add(new Point(start.X, start.Y));
		currentDirection = Direction.Right;
		nextDirection = Direction.Right;
		SpawnFood();
	}

	private void Die() {
		isAlive = false;
	}

	private void SpawnFood() {
		while (true) {
			var p = new Point(Global.random.Next(1, columns - 1), Global.random.Next(1, rows - 1));
			bool onSnake = false;
			for (int i = 0; i < snake.Count; i++) if (snake[i] == p) { onSnake = true; break; }
			if (!onSnake) { food = p; return; }
		}
	}

	private void DrawCell(in Point cell, in Color color) {
		var rect = new Rectangle(cell.X * CellSize, cell.Y * CellSize, CellSize, CellSize);
		spriteBatch.Draw(pixel, rect, color);
	}

	private enum Direction {
		Up,
		Down,
		Left,
		Right
	}
}

