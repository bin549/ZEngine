using System;

namespace SnakeDemoApp;

public static class Program {
	[STAThread]
	static void Main() {
		using (var game = new ZEngine.SnakeDemo.SnakeGame())
			game.Run();
	}
}


