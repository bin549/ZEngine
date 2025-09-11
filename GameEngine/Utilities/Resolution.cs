using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameEngine;

static class Resolution {
    static private GraphicsDeviceManager _Device = null;
    static private int _Width = 800;
    static private int _Height = 600;
    static private int _VWidth = 1024;
    static private int _VHeight = 768;
    static private Matrix _ScaleMatrix;
    static private bool _FullScreen = false;
    static private bool _dirtyMatrix = true;
    static private int virtualViewportX;
    static private int virtualViewportY;

    public static int VirtualViewportX {
        get { return virtualViewportX; }
    }

    public static int VirtualViewportY {
        get { return virtualViewportY; }
    }

    public static int VirtualWidth {
        get { return _VWidth; }
    }

    public static int VirtualHeight {
        get { return _VHeight; }
    }

    static public void Init(ref GraphicsDeviceManager device) {
        _Width = device.PreferredBackBufferWidth;
        _Height = device.PreferredBackBufferHeight;
        _Device = device;
        _dirtyMatrix = true;
        ApplyResolutionSettings();
    }

    static public Matrix getTransformationMatrix() {
        if (_dirtyMatrix) RecreateScaleMatrix();
        return _ScaleMatrix;
    }

    static public void SetResolution(int Width, int Height, bool FullScreen) {
        _Width = Width;
        _Height = Height;
        _FullScreen = FullScreen;
        ApplyResolutionSettings();
    }

    static public void SetVirtualResolution(int Width, int Height) {
        _VWidth = Width;
        _VHeight = Height;
        _dirtyMatrix = true;
    }

    static private void ApplyResolutionSettings() {
#if XBOX360
           _FullScreen = true;
#endif

        if (_FullScreen == false) {
            if ((_Width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width)
                && (_Height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)) {
                _Device.PreferredBackBufferWidth = _Width;
                _Device.PreferredBackBufferHeight = _Height;
                _Device.IsFullScreen = _FullScreen;
                _Device.PreferMultiSampling = true;
                _Device.ApplyChanges();
            }
        } else {
            foreach (DisplayMode dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes) {
                if ((dm.Width == _Width) && (dm.Height == _Height)) {
                    _Device.PreferredBackBufferWidth = _Width;
                    _Device.PreferredBackBufferHeight = _Height;
                    _Device.IsFullScreen = _FullScreen;
                    _Device.PreferMultiSampling = true;
                    _Device.ApplyChanges();
                }
            }
        }
        _dirtyMatrix = true;
        _Width = _Device.PreferredBackBufferWidth;
        _Height = _Device.PreferredBackBufferHeight;
    }

    static public void BeginDraw() {
        FullViewport();
        _Device.GraphicsDevice.Clear(Color.Black);
        ResetViewport();
        _Device.GraphicsDevice.Clear(Color.Black);
    }

    static private void RecreateScaleMatrix() {
        _dirtyMatrix = false;
        _ScaleMatrix = Matrix.CreateScale(
            (float)_Device.GraphicsDevice.Viewport.Width / _VWidth,
            (float)_Device.GraphicsDevice.Viewport.Width / _VWidth,
            1f);
    }


    static public void FullViewport() {
        Viewport vp = new Viewport();
        vp.X = vp.Y = 0;
        vp.Width = _Width;
        vp.Height = _Height;
        _Device.GraphicsDevice.Viewport = vp;
    }

    static public float getVirtualAspectRatio() {
        return (float)_VWidth / (float)_VHeight;
    }

    static public void ResetViewport() {
        float targetAspectRatio = getVirtualAspectRatio();
        int width = _Device.PreferredBackBufferWidth;
        int height = (int)(width / targetAspectRatio + .5f);
        bool changed = false;
        if (height > _Device.PreferredBackBufferHeight) {
            height = _Device.PreferredBackBufferHeight;
            width = (int)(height * targetAspectRatio + .5f);
            changed = true;
        }
        Viewport viewport = new Viewport();
        viewport.X = (_Device.PreferredBackBufferWidth / 2) - (width / 2);
        viewport.Y = (_Device.PreferredBackBufferHeight / 2) - (height / 2);
        virtualViewportX = viewport.X;
        virtualViewportY = viewport.Y;
        viewport.Width = width;
        viewport.Height = height;
        viewport.MinDepth = 0;
        viewport.MaxDepth = 1;
        if (changed) {
            _dirtyMatrix = true;
        }
        _Device.GraphicsDevice.Viewport = viewport;
    }
}