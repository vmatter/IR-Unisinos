using System;
using System.Windows.Forms;

namespace PDF_Viewer
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        public string documentName;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = @"D:\",
                Title = "Browse Text Files",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "pdf",
                Filter = "Pdf files (*.pdf)|*.pdf",
                FilterIndex = 2,
                RestoreDirectory = true,

                ReadOnlyChecked = true,
                ShowReadOnly = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                axAcroPDF1.src = openFileDialog.FileName;
                documentName = openFileDialog.FileName;
            }
            else
            {
                //MessageBox.Show("Select the File");
            }

        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // TODO: Pegar isso e exibir quando o botão search for clicado.
            int queryNumber = 10;
            string stringDeBusca = "desenvolvimento AND aplicação";
            // TODO: APlicar regex ou outra função.
            string[] stringBuscaPalavra = new string[2];
            stringBuscaPalavra[0] = "desenvolvimento";
            int qtdStringA = 1;
            int qtdStringB = 3;
            stringBuscaPalavra[1] = "aplicação";

            string documentName = "Enunciado.pdf";

            TextToChange = "teste";

            this.textBox2.Text = "***************************************** \r\n \r\n " +
                $"Número da consulta: {queryNumber}\r\n \r\n " +
                $"Nome do documento: {documentName}\r\n \r\n " +
                $"String de busca: {stringDeBusca} \r\n \r\n " +
                $"Ocorrências: {stringBuscaPalavra[0]} ({qtdStringA}), {stringBuscaPalavra[1]} ({qtdStringB}) \r\n \r\n" +
                "*****************************************";
        }

        public string TextToChange
        {
            get { return textBox2.Text; }
            set { textBox2.Text = "teste"; }
        }
    }
}
