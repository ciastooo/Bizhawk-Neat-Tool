using BizHawk.Client.ApiHawk;
using System.Windows.Forms;

[assembly:BizHawkExternalTool("NEAT", "Bizhawk tool for neat")]
namespace BizHawk.Client.EmuHawk
{
	public partial class CustomMainForm : Form, IExternalToolForm
	{
        [RequiredApi]
        private JoypadApi Joypad { get; set; }

		public CustomMainForm()
		{
			InitializeComponent();
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
		
		}

		public void Restart()
		{
            InitializeComponent();
        }

        public void UpdateValues()
		{
			
		}

        private void Sel_Click(object sender, System.EventArgs e)
        {
            var a = Joypad.Get();
            var b = Joypad.Get(1);
            var c = Joypad.Get(2);
            Joypad.Set("A", true, 1);
        }

        private void Lewo_Click(object sender, System.EventArgs e)
        {
            Joypad.Set("Left", true, 1);
        }

        private void Prawo_Click(object sender, System.EventArgs e)
        {
            Joypad.Set("Right", true, 1);
        }

        private void Góra_Click(object sender, System.EventArgs e)
        {
            Joypad.Set("Start", true, 1);
        }

        private (int, int) GetPlayerPosition()
        {
        }

        private int GetTile(int x, int y)
        {

        }
    }
}
