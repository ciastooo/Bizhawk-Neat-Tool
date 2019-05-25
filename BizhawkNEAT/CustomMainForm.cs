using BizHawk.Client.ApiHawk;
using BizhawkNEAT;
using BizhawkNEAT.Neat;
using BizhawkNEAT.Utils;
using System.Drawing;
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
            if (network != null && type == ToolFormUpdateType.PostFrame)
            {
                network.Train();
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
            var bitmap = new Bitmap(networkGraph.Width, networkGraph.Height);
            networkGraph.Image = bitmap;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            gameInformationHandler.SaveGameState();
            network = new Network(gameInformationHandler, networkGraph.CreateGraphics());
            network.Init(13 * 13 + 1, Config.ButtonNames.Length);
        }

        private void ShowGenome_CheckedChanged(object sender, System.EventArgs e)
        {
            Config.DrawGenome = showGenome.Checked;
        }
    }
}
