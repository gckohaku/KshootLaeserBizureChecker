namespace KshootLaeserBizureChecker
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();

			ofd.Title = "�`�F�b�N����t�@�C����I�����Ă�������";
			ofd.InitialDirectory = XmlDataManager.GetFileName();
			ofd.Filter = "KSH�t�@�C��(*.ksh)|*.ksh";
			ofd.RestoreDirectory = true;

			if (ofd.ShowDialog() == DialogResult.OK)
			{
				string fileName = ofd.FileName;
				textBox1.Text = fileName;
				XmlDataManager.Store(fileName);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			LaeserCheck.Check(textBox1.Text, ref textBox2);
		}
	}
}