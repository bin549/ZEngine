using System;

namespace CardDemoApp;

public static class Program {
	[STAThread]
	static void Main() {
		using (var game = new ZEngine.CardDemo.FcsGame())
			game.Run();
	}
}


