using SharpDX.Direct3D11;
using System.Runtime.InteropServices;
using SharpDX;
using System;
using SharpDX.Direct3D;

namespace SharpDXSample
{
    public struct Vertex
    {
        public Vector4 Position;
        public Vector4 Color;

        public Vertex(Vector4 pos, Vector4 col)
        {
            Position = pos;
            Color = col;
        }

        public override string ToString()
        {
            return $"({Position.X}, {Position.Y}, {Position.Z})({Color.X}, {Color.Y}, {Color.Z}, {Color.W})";
        }
    }

    public class D3D11VertexBuffer : System.IDisposable
    {
        public void Dispose()
        {
            if (m_buffer != null)
            {
                m_buffer.Dispose();
                m_buffer = null;
            }
        }

        Vertex[] m_vertices;
        public void SetVertices(Vertex[] vertices)
        {
            Dispose();
            m_vertices = vertices;
        }

        SharpDX.Direct3D11.Buffer m_buffer;
        void CreateResource(Device device)
        {
            if (m_buffer == null)
            {
                if (m_vertices != null)
                {
                    Console.WriteLine("create bufer");
                    m_buffer = SharpDX.Direct3D11.Buffer.Create(device, BindFlags.VertexBuffer, m_vertices);
                }
            }
        }

        static int s_stride = Marshal.SizeOf<Vertex>();

        public void Draw(Device device, DeviceContext context)
        {
            CreateResource(device);
            if (m_buffer != null)
            {
                context.InputAssembler.PrimitiveTopology = PrimitiveTopology.TriangleList;
                context.InputAssembler.SetVertexBuffers(0, new VertexBufferBinding(m_buffer, s_stride, 0));
                context.Draw(m_vertices.Length, 0);
            }
        }
    }
}
