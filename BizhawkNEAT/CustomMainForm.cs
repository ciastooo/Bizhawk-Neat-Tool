using BizHawk.Client.ApiHawk;
using BizhawkNEAT;
using BizhawkNEAT.Neat;
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

        private GameInformationHandler gameInformationHandler { get; set; }

        private Network network { get; set; }

        public CustomMainForm()
        {
            InitializeComponent();
            gameInformationHandler = new GameInformationHandler();
            network = new Network(gameInformationHandler);
            network.Init(13 * 13 + 1, 6);
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
            if(type == ToolFormUpdateType.PostFrame)
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
    }
}
