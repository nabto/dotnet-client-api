using Nabto.Client;
using Nabto.Client.Tunneling;
using NabtoTunnelManager.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace NabtoTunnelManager
{
	public partial class MainView : Form
	{
		const string TunnelConfigurationFile = "TunnelConfigurations.xml";

		string applicationText;

		List<TunnelConfiguration> tunnelConfigurations;

		NabtoClient nabto;

		enum TunnelManagerState
		{
			Stopped,
			Starting,
			Started,
			StopSignaled
		};
		TunnelManagerState tunnelState = TunnelManagerState.Stopped;
		TunnelConfiguration tunnelConfiguration;
		Task tunnelTask;

		public MainView()
		{
			InitializeComponent();
		}

		#region GUI event handlers

		private void Form1_Load(object sender, EventArgs e)
		{
			Size = Settings.Default.MainViewSize;
			WindowState = Settings.Default.MainViewWindowState;

			if (Settings.Default.MainViewLocation.X <= -32000 || Settings.Default.MainViewLocation.Y <= -32000)
			{
				CenterToScreen();
			}
			else
			{
				Location = Settings.Default.MainViewLocation;
			}

			try
			{
				nabto = new NabtoClient();
			}
			catch (Exception exc)
			{
				MessageBox.Show("Unable to load Nabto client API!");
				return;
			}

			var applicationVersion = Assembly.GetEntryAssembly().GetName().Version;
			applicationText = string.Format("{0} - {1} ({2})", Text, applicationVersion.ToString(2), nabto.NativeClientLibraryVersion);
			Text = applicationText;

			LoadTunnelConfigurations();

			foreach (var tc in tunnelConfigurations)
			{
				var lvi = CreateListViewItem(tc);
				UpdateListViewItem(lvi);
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (tunnelState != TunnelManagerState.Stopped)
			{
				if (MessageBox.Show("Closing Nabto Tunnel Manager will close the currently open tunnel. Do you want to close proceed?", "Nabto Tunnel Manager", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.No)
				{
					e.Cancel = true;
					return;
				}
			}

			switch (WindowState)
			{
				case FormWindowState.Maximized:
					Settings.Default.MainViewWindowState = FormWindowState.Maximized;
					break;
				case FormWindowState.Minimized:
					Settings.Default.MainViewWindowState = FormWindowState.Normal;
					break;
				case FormWindowState.Normal:
					Settings.Default.MainViewSize = Size;
					Settings.Default.MainViewLocation = Location;
					Settings.Default.MainViewWindowState = WindowState;
					break;
			}
			Settings.Default.Save();

			if (tunnelState != TunnelManagerState.Stopped)
			{
				CloseTunnel();
			}

			if (tunnelTask != null)
			{
				tunnelTask.Wait(2000);
			}

			if (nabto != null)
			{
				nabto.Shutdown();
			}
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			propertyGrid1.SelectedObject = GetCurrentTunnelConfiguration();
		}

		private void propertyGrid1_Leave(object sender, EventArgs e)
		{
			// trim Nabto specific fields to avoid crash in client API.
			var tunnelConfiguration = propertyGrid1.SelectedObject as TunnelConfiguration;
			tunnelConfiguration.Email = tunnelConfiguration.Email.Trim();
			tunnelConfiguration.LocalEndpoint = tunnelConfiguration.LocalEndpoint.Trim();
			tunnelConfiguration.Name = tunnelConfiguration.Name.Trim();
			tunnelConfiguration.Password = tunnelConfiguration.Password.Trim();
			tunnelConfiguration.RemoteEndpoint = tunnelConfiguration.RemoteEndpoint.Trim();
			tunnelConfiguration.Server = tunnelConfiguration.Server.Trim();

			SaveTunnelConfigurations();
			UpdateListViewItem();
		}

		private void propertyGrid1_SelectedGridItemChanged(object sender, SelectedGridItemChangedEventArgs e)
		{
			SaveTunnelConfigurations();
			UpdateListViewItem();
		}

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			OpenSelectedTunnel();
		}

		#region Tool bar

		private void toolStripButtonOpen_Click(object sender, EventArgs e)
		{
			OpenSelectedTunnel();
		}

		private void toolStripButtonClose_Click(object sender, EventArgs e)
		{
			CloseTunnel();
		}

		private void toolStripButtonAdd_Click(object sender, EventArgs e)
		{
			CreateNewTunnelConfiguration();
		}

		private void toolStripButtonDuplicate_Click(object sender, EventArgs e)
		{
			DuplicateSelectedTunnelConfiguration();
		}

		private void toolStripButtonRemove_Click(object sender, EventArgs e)
		{
			RemoveSelectedTunnelConfiguration();
		}

		#endregion

		#region Tunnel list context menu

		private void openToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OpenSelectedTunnel();
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseTunnel();
		}

		private void newToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CreateNewTunnelConfiguration();
		}

		private void duplicateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			DuplicateSelectedTunnelConfiguration();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RemoveSelectedTunnelConfiguration();
		}

		#endregion

		#endregion

		ListViewItem CreateListViewItem(TunnelConfiguration tunnelConfiguration)
		{
			var lvi = new ListViewItem();
			foreach (var item in listView1.Columns)
			{
				lvi.SubItems.Add(""); // add a field for each column
			}
			lvi.Tag = tunnelConfiguration;
			listView1.Items.Add(lvi);

			return lvi;
		}

		void UpdateListViewItem(ListViewItem listViewItem)
		{
			if (listViewItem != null)
			{
				var tc = (TunnelConfiguration)listViewItem.Tag;
				listViewItem.SubItems[0].Text = tc.Name;
				listViewItem.SubItems[1].Text = tc.Server;
				listViewItem.SubItems[2].Text = tc.LocalEndpoint;
				listViewItem.SubItems[3].Text = tc.RemoteEndpoint;
			}
		}

		void UpdateListViewItem()
		{
			UpdateListViewItem(GetCurrentListViewItem());
		}

		void LoadTunnelConfigurations()
		{
			if (File.Exists(TunnelConfigurationFile))
			{
				var xs = new XmlSerializer(typeof(List<TunnelConfiguration>));

				using (var fs = File.OpenRead(TunnelConfigurationFile))
				{
					tunnelConfigurations = (List<TunnelConfiguration>)xs.Deserialize(fs);
				}
			}
			else
			{
				tunnelConfigurations = new List<TunnelConfiguration>();
			}
		}

		void SaveTunnelConfigurations()
		{
			var xs = new XmlSerializer(typeof(List<TunnelConfiguration>));

			using (var fs = File.Create(TunnelConfigurationFile))
			{
				xs.Serialize(fs, tunnelConfigurations);
			}
		}

		ListViewItem GetCurrentListViewItem()
		{
			if (listView1.SelectedItems.Count == 1)
			{
				return listView1.SelectedItems[0];
			}
			else
			{
				return null;
			}
		}

		TunnelConfiguration GetCurrentTunnelConfiguration()
		{
			if (listView1.SelectedItems.Count == 1)
			{
				return (TunnelConfiguration)listView1.SelectedItems[0].Tag;
			}
			else
			{
				return null;
			}
		}

		void DeselectAllListViewItems()
		{
			foreach (ListViewItem item in listView1.Items)
			{
				item.Focused = false;
				item.Selected = false;
			}
		}

		#region High level operations

		void SetMessage(string message)
		{
			BeginInvoke(new Action(() =>
			{
				toolStripStatusLabelMessage.Text = message;
			}));
		}

		void SetCaption(string text)
		{
			BeginInvoke(new Action(() =>
			{
				Text = text;
			}));
		}

		void SetGuiLockedState(bool locked)
		{
			BeginInvoke(new Action(() =>
			{
				if (locked)
				{
					propertyGrid1.Enabled = false;

					toolStripButtonOpen.Visible = false;
					toolStripButtonClose.Visible = true;

					openToolStripMenuItem.Enabled = false;
					closeToolStripMenuItem.Enabled = true;
				}
				else
				{
					propertyGrid1.Enabled = true;

					toolStripButtonOpen.Visible = true;
					toolStripButtonClose.Visible = false;

					openToolStripMenuItem.Enabled = true;
					closeToolStripMenuItem.Enabled = false;
				}
			}));
		}

		void TunnelWorkerMethod()
		{
			tunnelState = TunnelManagerState.Starting;

			SetGuiLockedState(true);

			if (tunnelConfiguration != null)
			{
				SetMessage("Initializing...");

				// parse local endpoint
				string localIp;
				int localPort;
				var localEndpointParts = tunnelConfiguration.LocalEndpoint.Split(':');

				if (localEndpointParts.Length != 2)
				{
					SetMessage("Invalid local endpoint format!");
					return;
				}

				localIp = localEndpointParts[0];

				if (string.Compare(localIp, "127.0.0.1") != 0)
				{
					SetMessage("Only local IP 127.0.0.1 is supported!");
					return;
				}

				if (int.TryParse(localEndpointParts[1], out localPort) == false)
				{
					SetMessage("Invalid local endpoint format!");
					return;
				}

				// parse remote endpoint
				string remoteIp;
				int remotePort;
				var remoteEndpointParts = tunnelConfiguration.RemoteEndpoint.Split(':');

				if (remoteEndpointParts.Length != 2)
				{
					SetMessage("Invalid remote endpoint format!");
					return;
				}

				remoteIp = remoteEndpointParts[0];

				if (int.TryParse(remoteEndpointParts[1], out remotePort) == false)
				{
					SetMessage("Invalid remote endpoint format!");
					return;
				}

				SetMessage("Creating session...");
				using (var session = nabto.CreateSession(tunnelConfiguration.Email, tunnelConfiguration.Password))
				{
					if (session == null)
					{
						SetMessage("Failed creating session!");
						return;
					}

					SetMessage("Creating tunnel...");
					using (var tunnel = session.CreateTunnel(tunnelConfiguration.Server, localPort, remoteIp, remotePort))
					{
						if (tunnel == null)
						{
							SetMessage("Failed creating tunnel!");
							return;
						}

						SetCaption(tunnelConfiguration.Name + " - " + applicationText);

						tunnelState = TunnelManagerState.Started;

						var version = tunnel.Version;

						while (tunnelState == TunnelManagerState.Started)
						{
							var state = tunnel.State;

							SetMessage(string.Format("Tunnel state: {0}", state.ToString()));

							if (state == TunnelState.Closed)
							{
								tunnelState = TunnelManagerState.StopSignaled;
							}

							Thread.Sleep(100);
						}
					}

					SetMessage("Tunnel closed.");
				}

				SetMessage("Session closed.");
			}
		}

		void OpenSelectedTunnel()
		{
			// if no tunnel is open start one
			if (tunnelState == TunnelManagerState.Stopped)
			{
				tunnelConfiguration = GetCurrentTunnelConfiguration(); // extract tunnel configuration while on the UI thread
				tunnelTask = Task.Factory.StartNew(TunnelWorkerMethod).ContinueWith((t) =>
				{
					SetCaption(applicationText);
					SetGuiLockedState(false);
					tunnelState = TunnelManagerState.Stopped;
				});
			}
		}

		void CloseTunnel()
		{
			// if a tunnel is opened signal it to stop
			if (tunnelState == TunnelManagerState.Started)
			{
				tunnelState = TunnelManagerState.StopSignaled;
			}
		}

		void CreateNewTunnelConfiguration()
		{
			var tc = new TunnelConfiguration();
			tunnelConfigurations.Add(tc);

			SaveTunnelConfigurations(); // save to disc

			var lvi = CreateListViewItem(tc);
			UpdateListViewItem(lvi);

			DeselectAllListViewItems();

			lvi.Focused = true;
			lvi.Selected = true;
		}

		void DuplicateSelectedTunnelConfiguration()
		{
			if (GetCurrentListViewItem() != null)
			{
				var tc = new TunnelConfiguration(GetCurrentTunnelConfiguration()); // create new configuration based on the selected one
				tunnelConfigurations.Add(tc); // add to collection

				SaveTunnelConfigurations(); // save to disc

				var lvi = CreateListViewItem(tc);
				UpdateListViewItem(lvi);

				DeselectAllListViewItems();

				lvi.Focused = true;
				lvi.Selected = true;
			}
		}

		void RemoveSelectedTunnelConfiguration()
		{
			while (listView1.SelectedItems.Count > 0)
			{
				var lvi = listView1.SelectedItems[0];
				tunnelConfigurations.Remove((TunnelConfiguration)lvi.Tag);
				listView1.Items.Remove(lvi);
			}

			SaveTunnelConfigurations();
		}

		#endregion
	}
}
