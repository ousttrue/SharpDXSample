using SharpDX;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SharpDXSample
{
    public partial class Form1 : Form
    {
        string m_sourcePath;

        ShaderSource m_source;

        BindingList<Vertex> m_vertices = new BindingList<Vertex>();

        public Form1()
        {
            InitializeComponent();

            button1.BackColor = d3D11Panel1.Renderer.ClearColor.ToColor();

            // shader source
            m_sourcePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "shader.fx");
            m_source = new ShaderSource();
            richTextBox1.DataBindings.Add(new Binding("Text", m_source, "Source"));
            m_source.PropertyChanged += M_source_PropertyChanged;
                
            m_source.Source = File.ReadAllText(m_sourcePath, Encoding.UTF8);

            // watch
            var watcher = new System.IO.FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(m_sourcePath);
            watcher.NotifyFilter = System.IO.NotifyFilters.LastWrite;
            watcher.Filter = Path.GetFileName(m_sourcePath);
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;

            // vertex
            listBox1.DataSource = m_vertices;

            m_vertices.ListChanged += (o, e) =>
              {
                  d3D11Panel1.Buffer.SetVertices(m_vertices.ToArray());
                  d3D11Panel1.Invalidate();
              };

            m_vertices.Add(new Vertex(new Vector4(0.0f, 0.5f, 0.5f, 1.0f), new Vector4(1.0f, 0.0f, 0.0f, 1.0f)));
            m_vertices.Add(new Vertex(new Vector4(0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 1.0f, 0.0f, 1.0f)));
            m_vertices.Add(new Vertex(new Vector4(-0.5f, -0.5f, 0.5f, 1.0f), new Vector4(0.0f, 0.0f, 1.0f, 1.0f)));
        }

        private async void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            await Task.Delay(100);

            if (InvokeRequired)
            {
                Action callback = () =>
                {
                    Watcher_Changed(sender, e);
                };
                Invoke(callback);
                return;
            }
            m_source.Source=File.ReadAllText(m_sourcePath, Encoding.UTF8);
        }

        private void M_source_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Action callback = () =>
                {
                    M_source_PropertyChanged(sender, e);
                };
                Invoke(callback);
                return;
            }

            d3D11Panel1.Shader.Source = m_source.Source;
            d3D11Panel1.Invalidate();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            d3D11Panel1.Close();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            DialogResult result = colorDialog1.ShowDialog();
            // See if user pressed ok.
            if (result == DialogResult.OK)
            {
                // Set form background to the selected color.
                button1.BackColor = colorDialog1.Color;
                d3D11Panel1.Renderer.ClearColor = colorDialog1.Color.ToSharpDX();
                d3D11Panel1.Invalidate();
            }
        }
    }

    class ShaderSource : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void RaisePropertyChanged(string prop)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(prop));
            }
        }

        string m_source;
        public string Source
        {
            get { return m_source; }
            set
            {
                if (m_source == value) return;
                m_source = value;
                RaisePropertyChanged(nameof(Source));
            }
        }
    }

    static class ColorExtensions
    {
        public static System.Drawing.Color ToColor(this Color4 src)
        {
            return System.Drawing.Color.FromArgb(
                (byte)(src.Alpha * 255),
                (byte)(src.Red * 255),
                (byte)(src.Green * 255),
                (byte)(src.Blue * 255)
                );
        }

        const float ToF = 1.0f / 255;
        public static Color4 ToSharpDX(this System.Drawing.Color value)
        {
            return new Color4(
                value.R * ToF,
                value.G * ToF,
                value.B * ToF,
                value.A * ToF
                );
        }
    }
}
