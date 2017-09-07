using SFML.Graphics;
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace SFML.WinForms
{
    public class RenderSurface : Control
    {
        [Browsable(false)]
        public new string Text;
        private Color _clearColor;
        private bool _designMode;
        private RenderWindow _surface;
        private Graphics.View _view;
        public Graphics.View View = null;

        private Timer _autoDraw = new Timer();
        public bool AutoDraw { get { return _autoDraw.Enabled; } set { _autoDraw.Enabled = value; } }
        public int AutoDrawInterval { get { return _autoDraw.Interval; } set { _autoDraw.Interval = value; } }
        private void CheckUpdate(object sender, EventArgs e) { Invalidate(); }

        public override global::System.Drawing.Color BackColor
        {
            get { return base.BackColor; }
            set
            {
                base.BackColor = value;
                _clearColor = new Color(value.R, value.G, value.B, value.A);
            }
        }

        protected override void OnCreateControl()
        {
            _designMode =
                (LicenseManager.UsageMode == LicenseUsageMode.Designtime) ||
                (global::System.Diagnostics.Process.GetCurrentProcess().ProcessName == "devenv");
            if (!_designMode)
            {
                base.OnCreateControl();

                _clearColor = new Color(base.BackColor.R, base.BackColor.G, base.BackColor.B, base.BackColor.A);
                _surface = new RenderWindow(Handle);
                BackColor = base.BackColor;
                Render += OnRender;
                _autoDraw.Tick += CheckUpdate;
                
                Resize += GenerateView;
                GenerateView(null, null);
            }
            else base.OnCreateControl();

        }

        private void GenerateView(object sender, EventArgs e)
        {
            _surface.Size = new System.Vector2u((uint)Width, (uint)Height);
            _view = new Graphics.View(new FloatRect(0, 0, Width, Height));
        }

        protected override void Dispose(bool disposing)
        {
            if (_surface != null)
            {
                _surface.Dispose();
                _surface = null;
            }
            base.Dispose(disposing);
        }

        protected sealed override void OnPaint(PaintEventArgs e)
        {
            if (_designMode) return;
            
            _surface.Clear(_clearColor);
            if (View != null) _surface.SetView(View);
            else _surface.SetView(_view);
            if (Render != null) Render.Invoke(null, EventArgs.Empty);
            _surface.Display();
        }
        protected sealed override void OnPaintBackground(PaintEventArgs e) { if (_designMode) base.OnPaintBackground(e); }

        /// <summary> Draws input to the surface. </summary>
        public void Draw(Drawable drawable)
        {
            _surface.Draw(drawable);
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw(Drawable drawable, RenderStates states)
        {
            _surface.Draw(drawable, states);
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw(Vertex[] vertices, PrimitiveType type)
        {
            _surface.Draw(vertices, type);
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw(Vertex[] vertices, PrimitiveType type, RenderStates states)
        {
            _surface.Draw(vertices, type, states);
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw(Vertex[] vertices, uint start, uint count, PrimitiveType type)
        {
            _surface.Draw(vertices, start, count, type);
        }
        /// <summary> Draws input to the surface. </summary>
        public void Draw(Vertex[] vertices, uint start, uint count, PrimitiveType type, RenderStates states)
        {
            _surface.Draw(vertices, start, count, type, states);
        }

        public event EventHandler Render;
        public virtual void OnRender(object sender, EventArgs e) { }
    }
}
