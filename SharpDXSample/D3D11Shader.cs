using SharpDX.D3DCompiler;
using SharpDX.Direct3D11;
using System;

namespace SharpDXSample
{
    public class D3D11Shader : IDisposable
    {
        CompilationResult m_vsCompiled;
        CompilationResult m_psCompiled;

        string m_source;
        public string Source
        {
            get { return m_source; }
            set
            {
                if (m_source == value) return;
                m_source = value;

                Dispose();

                // compile
                m_vsCompiled = ShaderBytecode.Compile(m_source, "VS", "vs_4_0", ShaderFlags.None, EffectFlags.None);
                m_psCompiled = ShaderBytecode.Compile(m_source, "PS", "ps_4_0", ShaderFlags.None, EffectFlags.None);

                Console.WriteLine("compiled");
            }
        }

        InputLayout m_layout;
        VertexShader m_vs;
        PixelShader m_ps;

        public void Dispose()
        {
            if (m_layout != null)
            {
                m_layout.Dispose();
                m_layout = null;
            }

            if (m_vs != null)
            {
                m_vs.Dispose();
                m_vs = null;
            }

            if (m_ps != null)
            {
                m_ps.Dispose();
                m_ps = null;
            }
        }

        void CreateResources(Device device)
        {
            if (m_vs == null)
            {
                if (m_vsCompiled != null)
                {
                    m_vs = new VertexShader(device, m_vsCompiled);
                }
            }

            if (m_layout == null)
            {
                if (m_vsCompiled != null)
                {
                    // Layout from VertexShader input signature
                    m_layout = new InputLayout(
                    device,
                    ShaderSignature.GetInputSignature(m_vsCompiled),
                    new[]
                        {
                        new InputElement("POSITION", 0, SharpDX.DXGI.Format.R32G32B32A32_Float, 0, 0),
                        new InputElement("COLOR", 0, SharpDX.DXGI.Format.R32G32B32A32_Float, 16, 0)
                        });
                }
            }

            if (m_ps == null)
            {
                if (m_psCompiled != null)
                {
                    m_ps = new PixelShader(device, m_psCompiled);
                }
            }
        }

        public void SetContext(Device device, DeviceContext context)
        {
            CreateResources(device);
            if (m_vs != null)
            {
                context.VertexShader.Set(m_vs);
                context.PixelShader.Set(m_ps);
                context.InputAssembler.InputLayout = m_layout;
            }
        }
    }
}
