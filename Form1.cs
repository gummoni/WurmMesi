using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace WurmMesi
{
    public partial class Form1 : Form
    {
        string eventFile => $"_Event.{DateTime.Now.ToString("yyyy-MM")}.txt";
        FoodEffects foods { get; set; }
        Reader reader;

        public Form1()
        {
            InitializeComponent();
            Console.WriteLine(eventFile);

            //設定データ読み込み
            if (File.Exists("foods.txt"))
            {
                foods = Json.Parse<FoodEffects>(File.ReadAllText("foods.txt"));
            }
            if(null == foods)
            {
                foods = new FoodEffects();
            }

            if (File.Exists("setting.txt"))
            {
                txDir.Text = File.ReadAllText("setting.txt");
                btStart_Click(null, null);
            }
            dataGridView1.DataSource = foods;
        }

        private void btStart_Click(object sender, EventArgs e)
        {
            if (btStart.Text == "開始")
            {
                var dir = txDir.Text;
                if (!Directory.Exists(dir))
                {
                    MessageBox.Show($"{dir} ディレクトリが見つかりません");
                    return;
                }

                var fullPath = Path.Combine(dir, eventFile);
                if (!File.Exists(fullPath))
                {
                    MessageBox.Show($"{dir} ファイルが見つかりません");
                    return;
                }

                reader = new WurmMesi.Reader(fullPath, Encoding.GetEncoding(932));
                reader.OnReadLineRecieved += Reader_OnReadLineRecieved;
                btStart.Text = "停止";
            }
            else
            {
                btStart.Text = "開始";
                reader.OnReadLineRecieved -= Reader_OnReadLineRecieved;
                reader.Dispose();
            }

        }

        public delegate void UpdateView();

        void Reader_OnReadLineRecieved(object sender, ReadLineEventArgs e)
        {
            try
            {
                if (foods.Parse(e.Line))
                {
                    try
                    {
                        dataGridView1.Invoke(new UpdateView(() =>
                        {
                            try
                            {
                                dataGridView1.DataSource = null;
                                dataGridView1.DataSource = foods;
                            }
                            catch
                            {
                            }
                        }));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch
            {
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            File.WriteAllText("foods.txt", Json.ToString(foods));
            File.WriteAllText("setting.txt", txDir.Text);
            MessageBox.Show("保存しました。");
        }
    }
}
