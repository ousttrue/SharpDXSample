using System.Windows.Forms;


namespace SharpDXSample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            button1.BackColor = d3D11Panel1.ClearColor;
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
                d3D11Panel1.ClearColor = colorDialog1.Color;
            }
        }
    }
}
