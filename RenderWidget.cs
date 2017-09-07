using SFML.Graphics;
using System;
using System.ComponentModel;
using System.Security;
using System.Runtime.InteropServices;
using SFML;

namespace SFML.GtkSharp
{
    [ToolboxItem (true)]
    public class RenderWidget : Gtk.DrawingArea, IDisposable
    {
        private Color _clearColor = new Color { R = 50, B = 160, G = 160 };
        private bool _needUpdate;
        private RenderWindow _surface;
		private bool autoDraw = true;
		public bool AutoDraw { get { return autoDraw; } set { autoDraw = value; } }

        public byte ColorBlue
        {
            get { return _clearColor.B; }
            set { _clearColor.B = value; }
        }
        public byte ColorGreen
        {
            get { return _clearColor.G; }
            set { _clearColor.G = value; }
        }
        public byte ColorRed
        {
            get { return _clearColor.R; }
            set { _clearColor.R = value; }
        }
        private View _view;
        [Browsable (false)]
        public View View
        {
            get { return _view; }
            set { _view = value; _needUpdate = true; }
        }
        [Browsable (false)]
        public FloatRect ViewPort
        {
            get { return _view.Viewport; }
            set { _view = new View (value); _needUpdate = true; }
        }
        public RenderWidget ()
        {
            DoubleBuffered = false;
            Render += OnRender;
            _surface = new RenderWindow (Handle);
            Gtk.Application.Invoke (delegate
                {
                    if (CSFML.WINDOWS)
                    {
                        try { _surface = new RenderWindow (gdk_win32_drawable_get_handle (GdkWindow.Handle)); }
                        catch { throw new Exception ("Incorrect or missing installation of GTK# or CSFML library."); }
                    }
                    else if (CSFML.LINUX)
                    {
                        try { _surface = new RenderWindow (gdk_x11_drawable_get_xid (GdkWindow.Handle)); }
                        catch { throw new Exception ("Incompatible version of Linux or missing installation of gtk or CSFML library."); }
                    }
                    else if (CSFML.MAC)
                    {
                        try { _surface = new RenderWindow (gdk_quartz_window_get_nswindow (GdkWindow.Handle)); }
                        catch { throw new Exception ("Incompatible version of Mac or missing installation of gtk or CSFML library."); }
                    }
                    else
                        throw new Exception ("Operating system could not be detected. Possible error due "
                                             + "to not using CSFML.Activate() at start of program or OS incompatible!");
                    _needUpdate = true;
                });

            GLib.Idle.Add (new GLib.IdleHandler (delegate
            {
                if (_needUpdate && AutoDraw) return OnExposeEvent (null);
                return true;
            }));
        }
        protected override bool OnExposeEvent (Gdk.EventExpose ev)
        {
            _surface.Clear (_clearColor);
            if (_view != null) _surface.SetView (_view);
			if (Render != null) Render.Invoke (null, EventArgs.Empty);
            _surface.Display ();
            _needUpdate = false;
            return true;
        }
        public override void Dispose ()
        {
            if (_surface != null)
            {
                _surface.Dispose ();
                _surface = null;
            }
            base.Dispose ();
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw (Drawable drawable)
        {
            _surface.Draw (drawable);
            _needUpdate = true;
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw (Drawable drawable, RenderStates states)
        {
            _surface.Draw (drawable, states);
            _needUpdate = true;
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw (Vertex [] vertices, PrimitiveType type)
        {
            _surface.Draw (vertices, type);
            _needUpdate = true;
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw (Vertex [] vertices, PrimitiveType type, RenderStates states)
        {
            _surface.Draw (vertices, type, states);
            _needUpdate = true;
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw (Vertex [] vertices, uint start, uint count, PrimitiveType type)
        {
            _surface.Draw (vertices, start, count, type);
            _needUpdate = true;
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw (Vertex [] vertices, uint start, uint count, PrimitiveType type, RenderStates states)
        {
            _surface.Draw (vertices, start, count, type, states);
            _needUpdate = true;
        }

        public event EventHandler Render;
        public virtual void OnRender (object sender, EventArgs e) { }

        [SuppressUnmanagedCodeSecurity, DllImport ("libgdk-win32-2.0-0.dll")]
        public static extern IntPtr gdk_win32_drawable_get_handle (IntPtr handle);

        [SuppressUnmanagedCodeSecurity, DllImport ("gdk-x11-2.0")]
        static extern IntPtr gdk_x11_drawable_get_xid (IntPtr handle);

        [SuppressUnmanagedCodeSecurity, DllImport ("libgdk-quartz-2.0.0.dylib")]
        static extern IntPtr gdk_quartz_window_get_nswindow (IntPtr handle);
    }
}
