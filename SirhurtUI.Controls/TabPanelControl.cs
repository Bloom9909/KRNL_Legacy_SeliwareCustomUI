using System;
using System.Windows.Forms;

namespace SirhurtUI.Controls;

public class TabPanelControl : UserControl
{
	public TabPanelControl()
	{
		InitializeComponent();
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.Name = "TabPanelControl";
		base.Load += new System.EventHandler(TabPanelControl_Load);
		base.ResumeLayout(false);
	}

	private void TabPanelControl_Load(object sender, EventArgs e)
	{
	}
}
