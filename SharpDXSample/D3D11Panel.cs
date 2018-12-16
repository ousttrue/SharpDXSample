using System;
using System.Windows.Forms;


namespace SharpDXSample
{
    public partial class D3D11Panel : UserControl
    {
        D3D11Renderer m_renderer = new D3D11Renderer();
        public D3D11Renderer Renderer
        {
            get { return m_renderer; }
        }

        D3D11Shader m_shader = new D3D11Shader();
        public D3D11Shader Shader
        {
            get { return m_shader; }
        }

        D3D11VertexBuffer m_buffer = new D3D11VertexBuffer();
        public D3D11VertexBuffer Buffer
        {
            get { return m_buffer; }
        }

        public D3D11Panel()
        {
            InitializeComponent();
            m_renderer.ClearColor =
            new SharpDX.Mathematics.Interop.RawColor4(0, 0, 0.5f, 1.0f);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            m_renderer.BeginRendering(Handle);

            m_shader.SetContext(m_renderer.Device, m_renderer.Context);
            m_buffer.Draw(m_renderer.Device, m_renderer.Context);

            m_renderer.EndRendering();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            m_renderer.Resize(ClientSize.Width, ClientSize.Height);
            Invalidate(); // 再描画
        }

        public void Close()
        {
            if (m_buffer != null)
            {
                m_buffer.Dispose();
                m_buffer = null;
            }
            if (m_shader != null)
            {
                m_shader.Dispose();
                m_shader = null;
            }
            if (m_renderer != null)
            {
                m_renderer.Dispose();
                m_renderer = null;
            }
        }
    }
}
