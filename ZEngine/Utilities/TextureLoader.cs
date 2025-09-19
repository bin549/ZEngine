using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace ZEngine;

public static class TextureLoader {
    const bool usingPipeline = false;
    
    public static Texture2D Load(string filePath, ContentManager content) {
        try {
            Texture2D image = content.Load<Texture2D>(filePath);
            if (usingPipeline == false)
                PremultiplyTexture(image);
            return image;
        } catch (ContentLoadException) {
            // fallthrough to raw file loading
        } catch (FileNotFoundException) {
            // fallthrough to raw file loading
        }

        string[] candidates = new string[] {
            filePath + ".png",
            filePath + ".jpg",
            filePath + ".jpeg",
            filePath + ".bmp"
        };

        foreach (var relPath in candidates) {
            // try without prefix via TitleContainer
            if (TryLoadViaTitleContainer(content, relPath, out var tex1)) return tex1;
            // try with Content/ prefix via TitleContainer
            var withContent = Path.Combine("Content", relPath);
            if (TryLoadViaTitleContainer(content, withContent, out var tex2)) return tex2;
            // try absolute path from base directory
            var abs1 = Path.Combine(AppContext.BaseDirectory ?? string.Empty, relPath);
            if (TryLoadViaAbsolute(content, abs1, out var tex3)) return tex3;
            var abs2 = Path.Combine(AppContext.BaseDirectory ?? string.Empty, withContent);
            if (TryLoadViaAbsolute(content, abs2, out var tex4)) return tex4;
        }

        // If all attempts failed, rethrow a descriptive error
        throw new ContentLoadException("Unable to load texture: " + filePath + " (.xnb or image file not found)");
    }

    private static void PremultiplyTexture(Texture2D texture) {
        Color[] buffer = new Color[texture.Width * texture.Height];
        texture.GetData(buffer);
        for (int i = 0; i < buffer.Length; i++) {
            buffer[i] = Color.FromNonPremultiplied(buffer[i].R, buffer[i].G, buffer[i].B, buffer[i].A);
        }
        texture.SetData(buffer);
    }

    private static bool TryLoadViaTitleContainer(ContentManager content, string path, out Texture2D texture) {
        texture = null!;
        try {
            using (var stream = TitleContainer.OpenStream(path)) {
                var service = (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
                if (service == null || service.GraphicsDevice == null) return false;
                var tex = Texture2D.FromStream(service.GraphicsDevice, stream);
                if (usingPipeline == false)
                    PremultiplyTexture(tex);
                texture = tex;
                return true;
            }
        } catch {
            return false;
        }
    }

    private static bool TryLoadViaAbsolute(ContentManager content, string absolutePath, out Texture2D texture) {
        texture = null!;
        try {
            if (!File.Exists(absolutePath)) return false;
            using (var stream = File.OpenRead(absolutePath)) {
                var service = (IGraphicsDeviceService)content.ServiceProvider.GetService(typeof(IGraphicsDeviceService));
                if (service == null || service.GraphicsDevice == null) return false;
                var tex = Texture2D.FromStream(service.GraphicsDevice, stream);
                if (usingPipeline == false)
                    PremultiplyTexture(tex);
                texture = tex;
                return true;
            }
        } catch {
            return false;
        }
    }
}