using FlatPanelTest.Controls;
using FlatPanelTest;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Controls
{
    [ToolboxBitmap(typeof(Panel))]
    public class FlatPanel : Panel, ITheme
    {
        #region Private Members

        private UITheme _theme = UITheme.VS2019LightBlue;
        private FlatScrollBar? _vScrollBar = null;
        private FlatScrollBar? _hScrollBar = null;

        #endregion

        #region Constructor

        public FlatPanel()
        {
            BorderStyle = BorderStyle.None;
        }

        #endregion

        #region Public Properties

        [Category("Appearance")]
        [Description("The theme to apply to the Flat Panel control.")]
        [DefaultValue(UITheme.VS2019LightBlue)]
        public virtual UITheme Theme
        {
            get => _theme;
            set
            {
                _theme = value;
                if (_vScrollBar != null) _vScrollBar.Theme = _theme;
                if (_hScrollBar != null) _hScrollBar.Theme = _theme;
                Invalidate(true);
            }
        }

        [Category("Appearance")]
        [Description("True to allow the control to inherit the parent control style.")]
        [DefaultValue(true)]
        public bool ParentTheme { get; set; } = true;

        #endregion

        #region Overridden Methods

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_vScrollBar != null)
            {
                if (VerticalScroll.Visible)
                {
                    _vScrollBar.Minimum = VerticalScroll.Minimum;
                    _vScrollBar.Maximum = VerticalScroll.Maximum;
                    _vScrollBar.LargeChange = VerticalScroll.LargeChange;
                    _vScrollBar.SmallChange = VerticalScroll.SmallChange;
                    _vScrollBar.Value = VerticalScroll.Value;

                    AdjustVScrollBarSize();
                    AdjustVScrollBarLocation();

                    _vScrollBar.Visible = true;
                }
                else
                {
                    _vScrollBar.Visible = false;
                }
            }

            if (_hScrollBar != null)
            {
                if (HorizontalScroll.Visible)
                {
                    _hScrollBar.Minimum = HorizontalScroll.Minimum;
                    _hScrollBar.Maximum = HorizontalScroll.Maximum;
                    _hScrollBar.LargeChange = HorizontalScroll.LargeChange;
                    _hScrollBar.SmallChange = HorizontalScroll.SmallChange;
                    _hScrollBar.Value = HorizontalScroll.Value;

                    AdjustHScrollBarSize();
                    AdjustHScrollBarLocation();

                    _hScrollBar.Visible = true;
                }
                else
                {
                    _hScrollBar.Visible = false;
                }
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            Form? form = FindForm();
            if (form != null)
            {
                _vScrollBar = new FlatScrollBar
                {
                    Orientation = ScrollBarOrientation.Vertical,
                    Theme = _theme,
                    Value = 0
                };
                form.Controls.Add(_vScrollBar);
                _vScrollBar.BringToFront();
                _vScrollBar.Visible = false;

                _vScrollBar.Scroll += VerticalScrollBar_Scroll;

                _hScrollBar = new FlatScrollBar
                {
                    Orientation = ScrollBarOrientation.Horizontal,
                    Theme = _theme,
                    Height = SystemInformation.HorizontalScrollBarHeight
                };
                form.Controls.Add(_hScrollBar);
                _hScrollBar.BringToFront();
                _hScrollBar.Visible = false;

                _hScrollBar.Scroll += HorizontalScrollBar_Scroll;
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            base.OnHandleDestroyed(e);

            Form? form = FindForm();
            if (form != null)
            {
                try
                {
                    if (_vScrollBar != null)
                    {
                        form.Controls.Remove(_vScrollBar);
                        _vScrollBar.Dispose();
                    }

                    if (_hScrollBar != null)
                    {
                        form.Controls.Remove(_hScrollBar);
                        _hScrollBar.Dispose();
                    }
                }
                catch
                {
                    // Do nothing
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            AdjustVScrollBarSize();
            AdjustVScrollBarLocation();

            AdjustHScrollBarSize();
            AdjustHScrollBarLocation();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            if (_vScrollBar != null)
                _vScrollBar.Value = Math.Abs(AutoScrollPosition.Y);
        }

        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);

            AdjustVScrollBarLocation();
            AdjustHScrollBarLocation();
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);

            if (se.ScrollOrientation == ScrollOrientation.VerticalScroll)
            {
                if (_vScrollBar != null)
                    _vScrollBar.Value = VerticalScroll.Value;
            }
            else
            {
                if (_hScrollBar != null)
                    _hScrollBar.Value = HorizontalScroll.Value;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            if (_vScrollBar != null)
                _vScrollBar.Visible = Visible;
            if (_hScrollBar != null)
                _hScrollBar.Visible = Visible;

            if (Visible)
                AutoScrollPosition = new Point(0, 0);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Form? form = FindForm();
                if (form != null)
                {
                    try
                    {
                        if (_vScrollBar != null)
                        {
                            form.Controls.Remove(_vScrollBar);
                            _vScrollBar.Dispose();
                        }

                        if (_hScrollBar != null)
                        {
                            form.Controls.Remove(_hScrollBar);
                            _hScrollBar.Dispose();
                        }
                    }
                    catch
                    {
                        // Do nothing
                    }
                }
            }

            base.Dispose(disposing);
        }

        #endregion

        #region Private Methods

        private void VerticalScrollBar_Scroll(object? sender, EventArgs e)
        {
            if (_vScrollBar != null)
                AutoScrollPosition = new Point(HorizontalScroll.Value, _vScrollBar.Value);
        }

        private void HorizontalScrollBar_Scroll(object? sender, EventArgs e)
        {
            if (_hScrollBar != null)
                AutoScrollPosition = new Point(_hScrollBar.Value, VerticalScroll.Value);
        }

        private void AdjustVScrollBarLocation()
        {
            if (_vScrollBar != null)
            {
                _vScrollBar.Location = BorderStyle == BorderStyle.None
                    ? new Point(Location.X + Width - SystemInformation.VerticalScrollBarWidth, Location.Y)
                    : new Point(Location.X + Width - (SystemInformation.VerticalScrollBarWidth + 1), Location.Y + 1);
            }
        }

        private void AdjustVScrollBarSize()
        {
            if (_vScrollBar != null)
            {
                int h = Height;
                if (BorderStyle != BorderStyle.None) h -= 2;
                if (HorizontalScroll.Visible) h -= SystemInformation.HorizontalScrollBarHeight;
                _vScrollBar.Height = h;
            }
        }

        private void AdjustHScrollBarLocation()
        {
            if (_hScrollBar != null)
            {
                _hScrollBar.Location = BorderStyle == BorderStyle.None
                    ? new Point(Location.X, Location.Y + Height - SystemInformation.HorizontalScrollBarHeight)
                    : new Point(Location.X + 1, Location.Y + Height - (SystemInformation.HorizontalScrollBarHeight + 1));
            }
        }

        private void AdjustHScrollBarSize()
        {
            if (_hScrollBar != null)
            {
                int w = Width;
                if (BorderStyle != BorderStyle.None) w -= 2;
                if (VerticalScroll.Visible) w -= SystemInformation.VerticalScrollBarWidth;
                _hScrollBar.Width = w;
            }
        }

        #endregion
    }
}
