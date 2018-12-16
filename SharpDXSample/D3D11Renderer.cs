using SharpDX;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;


namespace SharpDXSample
{
    public class D3D11Renderer : IDisposable
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        Viewport Viewport
        {
            get
            {
                return new Viewport(0, 0, Width, Height, 0.0f, 1.0f);
            }
        }

        public SharpDX.Color4 ClearColor { get; set; }

        SharpDX.Direct3D11.Device m_device;
        public SharpDX.Direct3D11.Device Device
        {
            get { return m_device; }
        }

        DeviceContext m_context;
        public DeviceContext Context
        {
            get { return m_context; }
        }

        DXGISwapChain m_swapChain;

        public void Dispose()
        {
            if (m_swapChain != null)
            {
                m_swapChain.Dispose();
                m_swapChain = null;
            }
            if (m_context != null)
            {
                m_context.Dispose();
                m_context = null;
            }
            if (m_device != null)
            {
                m_device.Dispose();
                m_device = null;
            }
        }

        void CreateDevice(IntPtr hWnd)
        {
            if (m_device != null)
            {
                return;
            }

            SharpDX.Configuration.EnableObjectTracking = true;

            // SwapChain description
            var desc = new SwapChainDescription()
            {
                BufferCount = 1,
                ModeDescription = new ModeDescription(Width, Height,
                    new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = hWnd,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            // Create Device and SwapChain
            SwapChain swapChain;
            SharpDX.Direct3D11.Device.CreateWithSwapChain(DriverType.Hardware,
                DeviceCreationFlags.Debug,
                desc,
                out m_device, out swapChain);
            m_context = m_device.ImmediateContext;

            // Ignore all windows events
            using (var factory = swapChain.GetParent<Factory>())
            {
                factory.MakeWindowAssociation(hWnd, WindowAssociationFlags.IgnoreAll);
            }

            m_swapChain = new DXGISwapChain(swapChain);
        }

        public void Resize(int w, int h)
        {
            if (w == Width && h == Height)
            {
                return;
            }
            Width = w;
            Height = h;
            if (m_swapChain != null)
            {
                m_swapChain.Resize(w, h);
            }
        }

        public void BeginRendering(IntPtr hWnd)
        {
            CreateDevice(hWnd);

            var rtv = m_swapChain.GetRenderTargetView(m_device);
            m_context.ClearRenderTargetView(rtv, ClearColor);

            m_context.OutputMerger.SetTargets(rtv);
            m_context.Rasterizer.SetViewport(Viewport);
        }

        public void EndRendering()
        { 
            m_swapChain.Present();
        }
    }
}
