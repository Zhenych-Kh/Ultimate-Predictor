using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Newtonsoft.Json;

namespace UltimatePredictor
{
    public partial class Form1 : Form
    {
        private const string APP_NAME = "ULTIMATE_PREDICTOR";
        private readonly string PREDICTION_CONFIG_PASS = $"{Environment.CurrentDirectory}\\PredictionsConfig.json";
        private string[] _predictions;
        private Random _random = new Random();

        public Form1()
        {
            InitializeComponent();
            Text = APP_NAME;
        }

        private async void bPredict_Click(object sender, EventArgs e)
        {
            bPredict.Enabled = false;
            await Task.Run(() =>
            {
                for (int i = 1; i <= 100; i++)
                {
                    this.Invoke(new Action(() =>
                    {
                        UpdatePrograssBar(i);
                    }));
                    Text = $"Predict: {i}%";
                    Thread.Sleep(10);
                }
            });
            var index = _random.Next(_predictions.Length);
            var prediction = _predictions[index];
            MessageBox.Show($"{prediction}!");

            bPredict.Enabled = true;
            progressBar1.Value = 0;
            Text = APP_NAME;
        }

        private void UpdatePrograssBar(int i)
        {
            if (i == progressBar1.Maximum)
            {
                progressBar1.Maximum = i + 1;
                progressBar1.Value = i + 1;
                progressBar1.Maximum = i;
            }
            else
            {
                progressBar1.Value = i + 1;
            }
            progressBar1.Value = i;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                var data = File.ReadAllText(PREDICTION_CONFIG_PASS);
                _predictions = JsonConvert.DeserializeObject<string[]>(data);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                throw;
            }
            finally
            {
                if(_predictions == null)
                {
                    Close();
                }
                else if(_predictions.Length == 0)
                {
                    MessageBox.Show("Предсказания закончились");
                    Close();
                }    
            }
        }
    }
}
