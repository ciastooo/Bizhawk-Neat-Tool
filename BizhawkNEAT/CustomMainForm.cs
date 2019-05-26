using BizHawk.Client.ApiHawk;
using BizhawkNEAT;
using BizhawkNEAT.Neat;
using BizhawkNEAT.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

[assembly: BizHawkExternalTool("NEAT", "Bizhawk tool for neat")]
namespace BizHawk.Client.EmuHawk
{
    public partial class CustomMainForm : Form, IExternalToolForm
    {
        [RequiredApi]
        private JoypadApi Joypad
        {
            set
            {
                gameInformationHandler.SetJoypadApi(value);
            }
        }
        [RequiredApi]
        private IMem Memory
        {
            set
            {
                gameInformationHandler.SetMemoryApi(value);
            }
        }
        [RequiredApi]
        private MemorySaveStateApi SaveState
        {
            set
            {
                gameInformationHandler.SetSaveStateApi(value);
            }
        }
        [RequiredApi]
        private EmuApi EmuApi
        {
            set
            {
                gameInformationHandler.SetEmuApi(value);
            }
        }

        private GameInformationHandler gameInformationHandler { get; set; }
        private Network network { get; set; }

        public CustomMainForm()
        {
            gameInformationHandler = new GameInformationHandler();
        }

        public bool UpdateBefore => true;

        public bool AskSaveChanges()
        {
            throw new System.NotImplementedException();
        }

        public void FastUpdate()
        {
            throw new System.NotImplementedException();
        }

        public void NewUpdate(ToolFormUpdateType type)
        {
            if (network != null)
            {
                if (type == ToolFormUpdateType.PreFrame)
                {
                    gameInformationHandler.Pause();
                }
                if (type == ToolFormUpdateType.PostFrame)
                {
                    infoLabel.Text = $"Generation: {network.Generation}; MaxFitness: {network.TopFitnessInGeneration}; Specie: {network.CurrentSpecie.Name}; Genome: {network.CurrentSpecie.Genomes.IndexOf(network.CurrentPlayer)}/{network.CurrentSpecie.Genomes.Count}";
                    network.Train();
                    if (fitnessChart.Series["Fitness"].Points.Count < network.Generation - 1)
                    {
                        fitnessChart.Series["Fitness"].Points.AddXY(network.Generation - 1, network.TopFitnessInPreviousGeneration);
                        fitnessChart.Series["AverageFitness"].Points.AddXY(network.Generation - 1, network.AverageFitnessInPreviousGeneration);
                    }
                    gameInformationHandler.Unpause();
                }
            }
        }

        public void Restart()
        {
            InitializeComponent();
        }

        public void UpdateValues()
        {

        }

        private void Start_Click(object sender, System.EventArgs e)
        {
            start.Enabled = false;
            var bitmap = new Bitmap(networkGraph.Width, networkGraph.Height);
            networkGraph.Image = bitmap;
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.UserPaint, true);
            gameInformationHandler.SaveGameState();
            network = new Network(gameInformationHandler, networkGraph.CreateGraphics());
            network.Init(Config.InputNodesCount, Config.ButtonNames.Length);
            fitnessChart.Series["Fitness"].Points.AddXY(0, 0);
            fitnessChart.Series["AverageFitness"].Points.AddXY(0, 0);
        }

        private void ShowGenome_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.DrawGenome = showGenome.Checked;
        }

        private void SaveButton_Click(object sender, System.EventArgs e)
        {
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                var json = new JObject();

                var chartJson = new JObject();
                chartJson.Add("Fitness", JToken.FromObject(fitnessChart.Series["Fitness"].Points.Select(p => new { x = p.XValue, y = p.YValues[0] }).ToList()));
                chartJson.Add("AverageFitness", JToken.FromObject(fitnessChart.Series["AverageFitness"].Points.Select(p => new { x = p.XValue, y = p.YValues[0] }).ToList()));
                json.Add("Chart", chartJson);

                json.Add("ConnectionCounter", IdGenerator.ConnectionCounter);
                json.Add("NodeCounter", IdGenerator.NodeCounter);

                json.Add("Network", network.GetNetworkJson());

                var path = Path.Combine(Directory.GetCurrentDirectory(), "neat");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                using (var fileStream = saveFileDialog.OpenFile())
                {
                    using (var sw = new StreamWriter(fileStream))
                    {
                        sw.Write(json.ToString());
                    }
                }
                //File.WriteAllText(path + $"{DateTime.Now.ToString("dd.MM.yyyy.HH.mm.ss")}.neat.json", json.ToString());
            }
        }

        private void LoadButton_Click(object sender, System.EventArgs e)
        {

        }
    }
}
