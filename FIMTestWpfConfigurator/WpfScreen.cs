using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;
//_______________________________________________________________________________________________________________________
//https://stackoverflow.com/questions/1927540/how-to-get-the-size-of-the-current-screen-in-wpf
//_______________________________________________________________________________________________________________________
namespace FIMTestWpfConfigurator {
    //_______________________________________________________________________________________________________________________
    public class WpfScreen {
        //_______________________________________________________________________________________________________________________
        public static IEnumerable<WpfScreen> AllScreens() {
            foreach (Screen screen in Screen.AllScreens) {
                yield return new WpfScreen(screen);
                }
            }
        //_______________________________________________________________________________________________________________________
        public static WpfScreen GetScreenFrom(Window window) {
            WindowInteropHelper windowInteropHelper = new WindowInteropHelper(window);
            Screen screen = System.Windows.Forms.Screen.FromHandle(windowInteropHelper.Handle);
            WpfScreen wpfScreen = new WpfScreen(screen);
            return wpfScreen;
            }
        //_______________________________________________________________________________________________________________________
        public static WpfScreen GetScreenFrom(System.Windows.Point point) {
            int x = (int)Math.Round(point.X);
            int y = (int)Math.Round(point.Y);

            // are x,y device-independent-pixels ??
            System.Drawing.Point drawingPoint = new System.Drawing.Point(x, y);
            Screen screen = Screen.FromPoint(drawingPoint);
            WpfScreen wpfScreen = new WpfScreen(screen);
            return wpfScreen;
            }
        //_______________________________________________________________________________________________________________________
        public static double FrameHeight = SystemInformation.FrameBorderSize.Height;
        public static double FrameWidth = SystemInformation.FrameBorderSize.Width;
        public static double FrameWidth2 = SystemParameters.ResizeFrameVerticalBorderWidth;
        public static double MaximizedPrimaryScreenHeight = SystemParameters.MaximizedPrimaryScreenHeight - FrameHeight;
        public static double MaximizedPrimaryScreenWidth = SystemParameters.MaximizedPrimaryScreenWidth - FrameWidth;
        public double MaximizedCurrentScreenHeight => WorkingArea.Height + WpfScreen.FrameHeight * 2;
        public double MaximizedCurrentScreenWidth => WorkingArea.Width + WpfScreen.FrameWidth * 2;
        //_______________________________________________________________________________________________________________________
        public static WpfScreen Primary {
            get { return new WpfScreen(Screen.PrimaryScreen); }
            }
        //_______________________________________________________________________________________________________________________
        private readonly Screen screen;
        //_______________________________________________________________________________________________________________________
        internal WpfScreen(Screen screen) {
            this.screen = screen;
            }
        //_______________________________________________________________________________________________________________________
        public Rect DeviceBounds {
            get { return this.GetRect(this.screen.Bounds); }
            }
        //_______________________________________________________________________________________________________________________
        public Rect WorkingArea {
            get { return this.GetRect(this.screen.WorkingArea); }
            }
        //_______________________________________________________________________________________________________________________
        private Rect GetRect(Rectangle value) {
            // should x, y, width, height be device-independent-pixels ??
            return new Rect {
                X = value.X,
                Y = value.Y,
                Width = value.Width,
                Height = value.Height
                };
            }
        //_______________________________________________________________________________________________________________________
        public bool IsPrimary {
            get { return this.screen.Primary; }
            }
        //_______________________________________________________________________________________________________________________
        public string DeviceName {
            get { return this.screen.DeviceName; }
            }
        }
    }
