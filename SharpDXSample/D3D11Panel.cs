using System;
using System.Drawing;
using System.Windows.Forms;


namespace SharpDXSample
{
    public partial class D3D11Panel : UserControl
    {
        D3D11Renderer m_renderer = new D3D11Renderer
        {
            ClearColor = new SharpDX.Mathematics.Interop.RawColor4(0, 0, 0.5f, 1.0f)
        };

        const float ToF = 1.0f / 255;
        public Color ClearColor
        {
            get
            {
                return Color.FromArgb(
                    (byte)(m_renderer.ClearColor.A * 255),
                    (byte)(m_renderer.ClearColor.R * 255),
                    (byte)(m_renderer.ClearColor.G * 255),
                    (byte)(m_renderer.ClearColor.B * 255)
                    );
            }
            set
            {
                m_renderer.ClearColor = new SharpDX.Mathematics.Interop.RawColor4(
                    value.R * ToF,
                    value.G * ToF,
                    value.B * ToF,
                    value.A * ToF
                    );
                Invalidate();
            }
        }

        public D3D11Panel()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            m_renderer.Paint(Handle);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            m_renderer.Resize(ClientSize.Width, ClientSize.Height);
            Invalidate(); // 再描画
        }

        public void Close()
        {
            if (m_renderer != null)
            {
                m_renderer.Dispose();
                m_renderer = null;
            }
        }
    }
}
