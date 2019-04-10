using BizHawk.Client.ApiHawk;
using System.Windows.Forms;

[assembly:BizHawkExternalTool("NEAT", "test")]
namespace BizHawk.Client.EmuHawk
{
	public partial class CustomMainForm : Form, IExternalToolForm
	{
		public CustomMainForm()
		{
			InitializeComponent();
		}

		public bool UpdateBefore => throw new System.NotImplementedException();

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
			throw new System.NotImplementedException();
		}

		public void Restart()
		{
			throw new System.NotImplementedException();
		}

		public void UpdateValues()
		{
			throw new System.NotImplementedException();
		}
	}
}
