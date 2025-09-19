using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;

namespace ZEngine.CardDemo;

public static class MusicPlayer {
    private static SoundEffectInstance? loopInstance;

    public static void PlayLoop(ContentManager content, string assetPath, float volume = 0.25f) {
        try {
            var se = content.Load<SoundEffect>(assetPath);
            loopInstance?.Stop();
            loopInstance?.Dispose();
            loopInstance = se.CreateInstance();
            loopInstance.IsLooped = true;
            loopInstance.Volume = volume;
            loopInstance.Play();
        } catch (Exception) {
            // ignore if missing
        }
    }

    public static void Stop() {
        try { loopInstance?.Stop(); } catch { }
    }
}
