using System;
using System.Windows.Forms;


namespace SharpDXSample
{
    public partial class D3D11Panel : UserControl
    {
        D3D11Renderer m_renderer = new D3D11Renderer();

        public D3D11Panel()
        {
            InitializeComponent();

            m_renderer.ClearColor = new SharpDX.Mathematics.Interop.RawColor4(0, 0, 0.5f, 0);
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
