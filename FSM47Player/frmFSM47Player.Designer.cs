namespace FSM47Player
{
    partial class frmFSM47Player
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
			this.pnlControls = new System.Windows.Forms.GroupBox();
			this.txtState = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnNext = new System.Windows.Forms.Button();
			this.btnLast = new System.Windows.Forms.Button();
			this.btnPause = new System.Windows.Forms.Button();
			this.btnPlay = new System.Windows.Forms.Button();
			this.btnStop = new System.Windows.Forms.Button();
			this.btnVolume0 = new System.Windows.Forms.Button();
			this.btnVolume33 = new System.Windows.Forms.Button();
			this.btnVolume66 = new System.Windows.Forms.Button();
			this.btnVolume100 = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.lstTracks = new System.Windows.Forms.ListBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.btnClearConsole = new System.Windows.Forms.Button();
			this.lstConsole = new System.Windows.Forms.ListBox();
			this.pnlControls.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// pnlControls
			// 
			this.pnlControls.Controls.Add(this.txtState);
			this.pnlControls.Controls.Add(this.label1);
			this.pnlControls.Controls.Add(this.btnNext);
			this.pnlControls.Controls.Add(this.btnLast);
			this.pnlControls.Controls.Add(this.btnPause);
			this.pnlControls.Controls.Add(this.btnPlay);
			this.pnlControls.Controls.Add(this.btnStop);
			this.pnlControls.Controls.Add(this.btnVolume0);
			this.pnlControls.Controls.Add(this.btnVolume33);
			this.pnlControls.Controls.Add(this.btnVolume66);
			this.pnlControls.Controls.Add(this.btnVolume100);
			this.pnlControls.Enabled = false;
			this.pnlControls.Location = new System.Drawing.Point(17, 16);
			this.pnlControls.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.pnlControls.Name = "pnlControls";
			this.pnlControls.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.pnlControls.Size = new System.Drawing.Size(123, 495);
			this.pnlControls.TabIndex = 0;
			this.pnlControls.TabStop = false;
			this.pnlControls.Text = "Controls";
			// 
			// txtState
			// 
			this.txtState.Enabled = false;
			this.txtState.Location = new System.Drawing.Point(12, 220);
			this.txtState.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.txtState.Name = "txtState";
			this.txtState.ReadOnly = true;
			this.txtState.Size = new System.Drawing.Size(95, 22);
			this.txtState.TabIndex = 6;
			this.txtState.TabStop = false;
			this.txtState.WordWrap = false;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(8, 199);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(92, 17);
			this.label1.TabIndex = 5;
			this.label1.Text = "Current State";
			// 
			// btnNext
			// 
			this.btnNext.Location = new System.Drawing.Point(9, 167);
			this.btnNext.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnNext.Name = "btnNext";
			this.btnNext.Size = new System.Drawing.Size(100, 28);
			this.btnNext.TabIndex = 4;
			this.btnNext.Text = "Next >>>";
			this.btnNext.UseVisualStyleBackColor = true;
			this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
			// 
			// btnLast
			// 
			this.btnLast.Location = new System.Drawing.Point(9, 132);
			this.btnLast.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnLast.Name = "btnLast";
			this.btnLast.Size = new System.Drawing.Size(100, 28);
			this.btnLast.TabIndex = 3;
			this.btnLast.Text = "<<< Last";
			this.btnLast.UseVisualStyleBackColor = true;
			this.btnLast.Click += new System.EventHandler(this.btnLast_Click);
			// 
			// btnPause
			// 
			this.btnPause.Location = new System.Drawing.Point(8, 96);
			this.btnPause.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnPause.Name = "btnPause";
			this.btnPause.Size = new System.Drawing.Size(100, 28);
			this.btnPause.TabIndex = 2;
			this.btnPause.Text = "Pause ║";
			this.btnPause.UseVisualStyleBackColor = true;
			this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
			// 
			// btnPlay
			// 
			this.btnPlay.Location = new System.Drawing.Point(9, 60);
			this.btnPlay.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnPlay.Name = "btnPlay";
			this.btnPlay.Size = new System.Drawing.Size(100, 28);
			this.btnPlay.TabIndex = 1;
			this.btnPlay.Text = "Play >";
			this.btnPlay.UseVisualStyleBackColor = true;
			this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
			// 
			// btnVolume0
			// 
			this.btnVolume0.Location = new System.Drawing.Point(9, 250);
			this.btnVolume0.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnVolume0.Name = "btnVolume0";
			this.btnVolume0.Size = new System.Drawing.Size(100, 28);
			this.btnVolume0.TabIndex = 10;
			this.btnVolume0.Text = "Volume 0";
			this.btnVolume0.UseVisualStyleBackColor = true;
			this.btnVolume0.Click += new System.EventHandler(this.btnVolume0_Click);
			// 
			// btnVolume33
			// 
			this.btnVolume33.Location = new System.Drawing.Point(9, 280);
			this.btnVolume33.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnVolume33.Name = "btnVolume33";
			this.btnVolume33.Size = new System.Drawing.Size(100, 28);
			this.btnVolume33.TabIndex = 10;
			this.btnVolume33.Text = "Volume 33";
			this.btnVolume33.UseVisualStyleBackColor = true;
			this.btnVolume33.Click += new System.EventHandler(this.btnVolume33_Click);
			// 
			// btnVolume66
			// 
			this.btnVolume66.Location = new System.Drawing.Point(9, 310);
			this.btnVolume66.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnVolume66.Name = "btnVolume66";
			this.btnVolume66.Size = new System.Drawing.Size(100, 28);
			this.btnVolume66.TabIndex = 10;
			this.btnVolume66.Text = "Volume 66";
			this.btnVolume66.UseVisualStyleBackColor = true;
			this.btnVolume66.Click += new System.EventHandler(this.btnVolume66_Click);
			// 
			// btnVolume100
			// 
			this.btnVolume100.Location = new System.Drawing.Point(9, 340);
			this.btnVolume100.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnVolume100.Name = "btnVolume100";
			this.btnVolume100.Size = new System.Drawing.Size(100, 28);
			this.btnVolume100.TabIndex = 13;
			this.btnVolume100.Text = "Volume 100";
			this.btnVolume100.UseVisualStyleBackColor = true;
			this.btnVolume100.Click += new System.EventHandler(this.btnVolume100_Click);
			// 
			// btnStop
			// 
			this.btnStop.Location = new System.Drawing.Point(9, 25);
			this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnStop.Name = "btnStop";
			this.btnStop.Size = new System.Drawing.Size(100, 28);
			this.btnStop.TabIndex = 0;
			this.btnStop.Text = "Stop ■";
			this.btnStop.UseVisualStyleBackColor = true;
			this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.lstTracks);
			this.groupBox2.Location = new System.Drawing.Point(139, 16);
			this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox2.Size = new System.Drawing.Size(200, 495);
			this.groupBox2.TabIndex = 1;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Tracks";
			// 
			// lstTracks
			// 
			this.lstTracks.FormattingEnabled = true;
			this.lstTracks.ItemHeight = 16;
			this.lstTracks.Location = new System.Drawing.Point(8, 23);
			this.lstTracks.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lstTracks.Name = "lstTracks";
			this.lstTracks.Size = new System.Drawing.Size(183, 452);
			this.lstTracks.TabIndex = 0;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.btnClearConsole);
			this.groupBox3.Controls.Add(this.lstConsole);
			this.groupBox3.Location = new System.Drawing.Point(337, 16);
			this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.groupBox3.Size = new System.Drawing.Size(671, 495);
			this.groupBox3.TabIndex = 2;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Console";
			// 
			// btnClearConsole
			// 
			this.btnClearConsole.Location = new System.Drawing.Point(11, 23);
			this.btnClearConsole.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.btnClearConsole.Name = "btnClearConsole";
			this.btnClearConsole.Size = new System.Drawing.Size(188, 28);
			this.btnClearConsole.TabIndex = 1;
			this.btnClearConsole.Text = "Clear Console";
			this.btnClearConsole.UseVisualStyleBackColor = true;
			this.btnClearConsole.Click += new System.EventHandler(this.btnClearConsole_Click);
			// 
			// lstConsole
			// 
			this.lstConsole.FormattingEnabled = true;
			this.lstConsole.ItemHeight = 16;
			this.lstConsole.Location = new System.Drawing.Point(9, 55);
			this.lstConsole.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.lstConsole.Name = "lstConsole";
			this.lstConsole.Size = new System.Drawing.Size(652, 420);
			this.lstConsole.TabIndex = 0;
			// 
			// frmFSM47Player
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1067, 554);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.pnlControls);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "frmFSM47Player";
			this.Text = "FSM47 Player";
			this.Load += new System.EventHandler(this.frmFSM47Player_Load);
			this.pnlControls.ResumeLayout(false);
			this.pnlControls.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox pnlControls;
        private System.Windows.Forms.TextBox txtState;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnLast;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnVolume0;
        private System.Windows.Forms.Button btnVolume33;
        private System.Windows.Forms.Button btnVolume66;
        private System.Windows.Forms.Button btnVolume100;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox lstTracks;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnClearConsole;
        private System.Windows.Forms.ListBox lstConsole;
    }
}

