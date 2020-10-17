using System;
using System.Windows.Forms;
using Wallter.Properties;

namespace Wallter
{
    /// <summary>
    /// Very basic tray app with system timer and simple menu
    /// </summary>
    class WallterApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;
        internal Timer changeTimer;

        public WallterApplicationContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                ContextMenu = new ContextMenu(new MenuItem[] {
                    new MenuItem("Change now", ChangeNow),
                    new MenuItem("-"), 
                    new MenuItem("Exit", Exit)
                }),
                Visible = true
            };

            // Change image on start
            doChangeWallpaper();

            // Change interval here. Default is one day after launch
            changeTimer = new Timer();
            changeTimer.Interval = (int) new TimeSpan(1, 0, 0, 0).TotalMilliseconds;
            changeTimer.Enabled = true;
            changeTimer.Tick += (sender, args) =>
            {
                changeTimer.Enabled = false;
                doChangeWallpaper();
                changeTimer.Enabled = true;
            };

            changeTimer.Start();
        }

        /// <summary>
        /// Trigger change wallpaper. Search terms are last part of URI
        /// </summary>
        void doChangeWallpaper()
        {
            var uri = new Uri("https://source.unsplash.com/1920x1080/?wallpaper,dark,nature");
            WallterChanger.Set(uri, WallterChanger.Style.Stretched);
        }

        /// <summary>
        /// Change wallpaper on demand
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChangeNow(object sender, EventArgs e)
        {
            doChangeWallpaper();
        }

        // Exit app
        void Exit(object sender, EventArgs e)
        {
            // Hide tray icon, otherwise it will remain shown until user mouses over it
            trayIcon.Visible = false;
            changeTimer.Stop();
            Application.Exit();
        }
    }
}
