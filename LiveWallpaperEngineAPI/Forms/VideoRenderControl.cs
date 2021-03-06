﻿using DZY.Util.Winform.Extensions;
using Giantapp.LiveWallpaper.Engine.Renders;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Giantapp.LiveWallpaper.Engine.Forms
{
    //显示视频的控件
    public partial class VideoRenderControl : UserControl, IRenderControl
    {
        private Mpv.NET.Player.MpvPlayer _player;
        //private string _lastPath;
        private int _volume;
        public VideoRenderControl()
        {
            InitializeComponent();
            //UI
            BackColor = Color.Magenta;
        }

        public void InitRender()
        {

        }

        public new void Load(string path)
        {
            if (_player == null)
            {
                var assembly = Assembly.GetEntryAssembly();
                string appDir = System.IO.Path.GetDirectoryName(assembly.Location);
                string dllPath = $@"{appDir}\lib\mpv-1.dll";
                if (IntPtr.Size == 4)
                {
                    // 32-bit
                }
                else if (IntPtr.Size == 8)
                {
                    // 64-bit
                    dllPath = $@"{appDir}\lib\mpv-1-x64.dll";
                }
                this.InvokeIfRequired(() =>
                {                    
                    //单元测试
                    _player = new Mpv.NET.Player.MpvPlayer(Handle, dllPath)
                    {
                        Loop = true,
                        Volume = 0
                    };                    
                    //防止视频黑边
                    _player.API.SetPropertyString("panscan", "1.0");
                    _player.AutoPlay = true;
                    _player.Volume = _volume;
                    //Load(_lastPath);
                });
            }

            if (string.IsNullOrEmpty(path))
                return;
            try
            {

                System.Diagnostics.Debug.WriteLine("load video 0");
                //_lastPath = path;
                // 设置解码模式为自动，如果条件允许，MPV会启动硬件解码
                _player?.API.SetPropertyString("hwdec", "auto");
                //_player.API.SetProperty("hwdec",Encoding.Default.GetBytes("auto"));
                _player?.Pause();
                _player?.Load(path);
                _player?.Resume();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                System.Diagnostics.Debug.WriteLine("load video 1");
            }
        }

        public void Stop()
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("stop video 0");
                _player?.Stop();
            }
            catch (Exception)
            {
            }
            finally
            {
                System.Diagnostics.Debug.WriteLine("stop video 1");
            }
        }

        public void DisposeRender()
        {
            // 不能释放，否则重新开启视频会崩溃
            //_player?.Dispose();
            _player = null;
        }

        public void Pause()
        {
            _player?.Pause();
        }

        public void Resum()
        {
            _player?.Resume();
        }

        public void SetVolume(int v)
        {
            if (v < 0)
                v = 0;
            if (v > 100)
                v = 100;

            _volume = v;

            if (_player != null)
                _player.Volume = v;
        }
    }
}
