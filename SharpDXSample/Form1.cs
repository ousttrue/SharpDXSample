using System.Windows.Forms;


namespace SharpDXSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            d3D11Panel1.Close();
        }
    }
}
