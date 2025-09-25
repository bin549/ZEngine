using System;

namespace PlatformerDemoApp;

public static class Program {
	[STAThread]
	static void Main() {
		using (var game = new ZEngine.Game())
			game.Run();
	}
}
