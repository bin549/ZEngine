using System;

namespace ZEngine;

public static class Program {
    [STAThread]
    static void Main() {
        var demo = Environment.GetEnvironmentVariable("DEMO")?.ToLowerInvariant();
        switch (demo) {
            case "fcs":
            case "card":
                RunCard();
                break;
            case "platformer":
            default:
                RunPlatformer();
                break;
        }
    }

    static void RunPlatformer() {
        using (var game = new Game())
            game.Run();
    }

    static void RunCard() {
        using (var game = new ZEngine.CardDemo.FcsGame())
            game.Run();
    }
}


