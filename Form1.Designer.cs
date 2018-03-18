namespace LectureScheduler
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FileContent = new System.Windows.Forms.RichTextBox();
            this.GraphPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.AnimGraphNext = new System.Windows.Forms.Button();
            this.AnimGraphPrev = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.GraphPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(167, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load File";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog1_FileOk);
            // 
            // FileContent
            // 
            this.FileContent.Location = new System.Drawing.Point(12, 62);
            this.FileContent.Name = "FileContent";
            this.FileContent.Size = new System.Drawing.Size(387, 376);
            this.FileContent.TabIndex = 2;
            this.FileContent.Text = "Result will be displayed here.\nA new window will open to display the graph.";
            // 
            // GraphPanel
            // 
            this.GraphPanel.Controls.Add(this.label3);
            this.GraphPanel.Location = new System.Drawing.Point(411, 62);
            this.GraphPanel.Name = "GraphPanel";
            this.GraphPanel.Size = new System.Drawing.Size(511, 376);
            this.GraphPanel.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(407, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Graph :";
            // 
            // AnimGraphNext
            // 
            this.AnimGraphNext.Enabled = false;
            this.AnimGraphNext.Location = new System.Drawing.Point(819, 36);
            this.AnimGraphNext.Name = "AnimGraphNext";
            this.AnimGraphNext.Size = new System.Drawing.Size(102, 21);
            this.AnimGraphNext.TabIndex = 5;
            this.AnimGraphNext.Text = "Next >";
            this.AnimGraphNext.UseVisualStyleBackColor = true;
            this.AnimGraphNext.Click += new System.EventHandler(this.AnimGraphNext_Click);
            // 
            // AnimGraphPrev
            // 
            this.AnimGraphPrev.Enabled = false;
            this.AnimGraphPrev.Location = new System.Drawing.Point(713, 36);
            this.AnimGraphPrev.Name = "AnimGraphPrev";
            this.AnimGraphPrev.Size = new System.Drawing.Size(100, 20);
            this.AnimGraphPrev.TabIndex = 6;
            this.AnimGraphPrev.Text = "< Previous";
            this.AnimGraphPrev.UseVisualStyleBackColor = true;
            this.AnimGraphPrev.Click += new System.EventHandler(this.AnimGraphPrev_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(710, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Animate graph here! :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(109, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Please load a file first!";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(931, 450);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AnimGraphPrev);
            this.Controls.Add(this.AnimGraphNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GraphPanel);
            this.Controls.Add(this.FileContent);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Butterfly\'s Lecture Scheduler";
            this.GraphPanel.ResumeLayout(false);
            this.GraphPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox FileContent;
        private System.Windows.Forms.Panel GraphPanel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button AnimGraphNext;
        private System.Windows.Forms.Button AnimGraphPrev;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}

