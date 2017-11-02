namespace NabtoTunnelManager
{
	partial class MainView
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainView));
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.duplicateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabelMessage = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelToServer = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabelFromServer = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonAdd = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonDuplicate = new System.Windows.Forms.ToolStripButton();
			this.toolStripButtonRemove = new System.Windows.Forms.ToolStripButton();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.listView1 = new System.Windows.Forms.ListView();
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
			this.contextMenuStrip1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.toolStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.Panel2.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.closeToolStripMenuItem,
            this.newToolStripMenuItem,
            this.duplicateToolStripMenuItem,
            this.removeToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(125, 114);
			// 
			// openToolStripMenuItem
			// 
			this.openToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
			this.openToolStripMenuItem.Name = "openToolStripMenuItem";
			this.openToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.openToolStripMenuItem.Text = "Open";
			this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
			// 
			// closeToolStripMenuItem
			// 
			this.closeToolStripMenuItem.Enabled = false;
			this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
			this.closeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.closeToolStripMenuItem.Text = "Close";
			this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
			// 
			// newToolStripMenuItem
			// 
			this.newToolStripMenuItem.Name = "newToolStripMenuItem";
			this.newToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.newToolStripMenuItem.Text = "New";
			this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
			// 
			// duplicateToolStripMenuItem
			// 
			this.duplicateToolStripMenuItem.Name = "duplicateToolStripMenuItem";
			this.duplicateToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.duplicateToolStripMenuItem.Text = "Duplicate";
			this.duplicateToolStripMenuItem.Click += new System.EventHandler(this.duplicateToolStripMenuItem_Click);
			// 
			// removeToolStripMenuItem
			// 
			this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
			this.removeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
			this.removeToolStripMenuItem.Text = "Remove";
			this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelMessage,
            this.toolStripStatusLabelToServer,
            this.toolStripStatusLabelFromServer});
			this.statusStrip1.Location = new System.Drawing.Point(0, 502);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(690, 22);
			this.statusStrip1.TabIndex = 4;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabelMessage
			// 
			this.toolStripStatusLabelMessage.Name = "toolStripStatusLabelMessage";
			this.toolStripStatusLabelMessage.Size = new System.Drawing.Size(675, 17);
			this.toolStripStatusLabelMessage.Spring = true;
			this.toolStripStatusLabelMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// toolStripStatusLabelToServer
			// 
			this.toolStripStatusLabelToServer.AutoSize = false;
			this.toolStripStatusLabelToServer.Name = "toolStripStatusLabelToServer";
			this.toolStripStatusLabelToServer.Size = new System.Drawing.Size(120, 17);
			this.toolStripStatusLabelToServer.Text = "To server: 0";
			this.toolStripStatusLabelToServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripStatusLabelToServer.Visible = false;
			// 
			// toolStripStatusLabelFromServer
			// 
			this.toolStripStatusLabelFromServer.AutoSize = false;
			this.toolStripStatusLabelFromServer.Name = "toolStripStatusLabelFromServer";
			this.toolStripStatusLabelFromServer.Size = new System.Drawing.Size(120, 17);
			this.toolStripStatusLabelFromServer.Text = "From server: 0";
			this.toolStripStatusLabelFromServer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.toolStripStatusLabelFromServer.Visible = false;
			// 
			// toolStrip1
			// 
			this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
			this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButtonOpen,
            this.toolStripButtonClose,
            this.toolStripButtonAdd,
            this.toolStripButtonDuplicate,
            this.toolStripButtonRemove});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(690, 39);
			this.toolStrip1.TabIndex = 5;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripButtonOpen
			// 
			this.toolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonOpen.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonOpen.Image")));
			this.toolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonOpen.Name = "toolStripButtonOpen";
			this.toolStripButtonOpen.Size = new System.Drawing.Size(36, 36);
			this.toolStripButtonOpen.Text = "toolStripButton1";
			this.toolStripButtonOpen.ToolTipText = "Open selected tunnel";
			this.toolStripButtonOpen.Click += new System.EventHandler(this.toolStripButtonOpen_Click);
			// 
			// toolStripButtonClose
			// 
			this.toolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClose.Image")));
			this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonClose.Name = "toolStripButtonClose";
			this.toolStripButtonClose.Size = new System.Drawing.Size(36, 36);
			this.toolStripButtonClose.Text = "toolStripButton1";
			this.toolStripButtonClose.ToolTipText = "Close current tunnel";
			this.toolStripButtonClose.Visible = false;
			this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
			// 
			// toolStripButtonAdd
			// 
			this.toolStripButtonAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonAdd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAdd.Image")));
			this.toolStripButtonAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonAdd.Name = "toolStripButtonAdd";
			this.toolStripButtonAdd.Size = new System.Drawing.Size(36, 36);
			this.toolStripButtonAdd.Text = "toolStripButton2";
			this.toolStripButtonAdd.ToolTipText = "Add new tunnel";
			this.toolStripButtonAdd.Click += new System.EventHandler(this.toolStripButtonAdd_Click);
			// 
			// toolStripButtonDuplicate
			// 
			this.toolStripButtonDuplicate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonDuplicate.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonDuplicate.Image")));
			this.toolStripButtonDuplicate.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonDuplicate.Name = "toolStripButtonDuplicate";
			this.toolStripButtonDuplicate.Size = new System.Drawing.Size(36, 36);
			this.toolStripButtonDuplicate.Text = "toolStripButton1";
			this.toolStripButtonDuplicate.ToolTipText = "Duplicate selected tunnel";
			this.toolStripButtonDuplicate.Click += new System.EventHandler(this.toolStripButtonDuplicate_Click);
			// 
			// toolStripButtonRemove
			// 
			this.toolStripButtonRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButtonRemove.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRemove.Image")));
			this.toolStripButtonRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripButtonRemove.Name = "toolStripButtonRemove";
			this.toolStripButtonRemove.Size = new System.Drawing.Size(36, 36);
			this.toolStripButtonRemove.Text = "toolStripButton3";
			this.toolStripButtonRemove.ToolTipText = "Remove selected tunnel(s)";
			this.toolStripButtonRemove.Click += new System.EventHandler(this.toolStripButtonRemove_Click);
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 39);
			this.splitContainer1.Name = "splitContainer1";
			this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.listView1);
			// 
			// splitContainer1.Panel2
			// 
			this.splitContainer1.Panel2.Controls.Add(this.propertyGrid1);
			this.splitContainer1.Size = new System.Drawing.Size(690, 463);
			this.splitContainer1.SplitterDistance = 135;
			this.splitContainer1.TabIndex = 6;
			// 
			// listView1
			// 
			this.listView1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader2,
            this.columnHeader1,
            this.columnHeader3});
			this.listView1.ContextMenuStrip = this.contextMenuStrip1;
			this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.listView1.FullRowSelect = true;
			this.listView1.HideSelection = false;
			this.listView1.Location = new System.Drawing.Point(0, 0);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(690, 135);
			this.listView1.TabIndex = 2;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
			this.listView1.DoubleClick += new System.EventHandler(this.listView1_DoubleClick);
			// 
			// columnHeader4
			// 
			this.columnHeader4.Text = "Name";
			this.columnHeader4.Width = 200;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Server";
			this.columnHeader2.Width = 150;
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Local endpoint";
			this.columnHeader1.Width = 150;
			// 
			// columnHeader3
			// 
			this.columnHeader3.Text = "Remote endpoint";
			this.columnHeader3.Width = 150;
			// 
			// propertyGrid1
			// 
			this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.propertyGrid1.Location = new System.Drawing.Point(0, 0);
			this.propertyGrid1.Name = "propertyGrid1";
			this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.NoSort;
			this.propertyGrid1.Size = new System.Drawing.Size(690, 324);
			this.propertyGrid1.TabIndex = 0;
			this.propertyGrid1.ToolbarVisible = false;
			this.propertyGrid1.SelectedGridItemChanged += new System.Windows.Forms.SelectedGridItemChangedEventHandler(this.propertyGrid1_SelectedGridItemChanged);
			this.propertyGrid1.Leave += new System.EventHandler(this.propertyGrid1_Leave);
			// 
			// MainView
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(690, 524);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.statusStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainView";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Nabto Tunnel Manager";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.contextMenuStrip1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
			this.splitContainer1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem duplicateToolStripMenuItem;
		private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMessage;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelToServer;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelFromServer;
		private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
		private System.Windows.Forms.ToolStrip toolStrip1;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.PropertyGrid propertyGrid1;
		private System.Windows.Forms.ToolStripButton toolStripButtonOpen;
		private System.Windows.Forms.ToolStripButton toolStripButtonAdd;
		private System.Windows.Forms.ToolStripButton toolStripButtonRemove;
		private System.Windows.Forms.ToolStripButton toolStripButtonDuplicate;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
		private System.Windows.Forms.ToolStripButton toolStripButtonClose;
		private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
		private System.Windows.Forms.ColumnHeader columnHeader4;
	}
}

