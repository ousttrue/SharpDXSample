using SharpDX.Direct3D11;
using SharpDX.DXGI;
using System;


namespace SharpDXSample
{
    class DXGISwapChain : IDisposable
    {
        SwapChain m_swapChain;
        public void Present()
        {
            m_swapChain.Present(0, PresentFlags.None);
        }
        public void Resize(int w, int h)
        {
            ReleaseRTV();
            var desc = m_swapChain.Description;
            m_swapChain.ResizeBuffers(desc.BufferCount, w, h, desc.ModeDescription.Format, desc.Flags);
        }

        RenderTargetView m_rtv;
        public void ReleaseRTV()
        {
            if (m_rtv != null)
            {
                m_rtv.Dispose();
                m_rtv = null;
            }
        }
        public RenderTargetView GetRenderTargetView(SharpDX.Direct3D11.Device device)
        {
            if (m_rtv == null)
            {
                // New RenderTargetView from the backbuffer
                using (var m_backBuffer = Texture2D.FromSwapChain<Texture2D>(m_swapChain, 0))
                {
                    m_rtv = new RenderTargetView(device, m_backBuffer);
                }
            }
            return m_rtv;
        }

        public void Dispose()
        {
            ReleaseRTV();
            if (m_swapChain != null)
            {
                m_swapChain.Dispose();
                m_swapChain = null;
            }
        }

        public DXGISwapChain(SwapChain swapchain)
        {
            m_swapChain = swapchain;
        }
    }
}
