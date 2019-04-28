namespace ClientExample
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.myClientControl = new ClientExample.MyClientControl();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tbReseive = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // myClientControl
            // 
            this.myClientControl.ClientName = "ChatClient";
            this.myClientControl.Dock = System.Windows.Forms.DockStyle.Left;
            this.myClientControl.IPServer = ((System.Net.IPAddress)(resources.GetObject("myClientControl.IPServer")));
            this.myClientControl.IsSyncronized = true;
            this.myClientControl.Location = new System.Drawing.Point(0, 0);
            this.myClientControl.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.myClientControl.Name = "myClientControl";
            this.myClientControl.Port = 8000;
            this.myClientControl.Size = new System.Drawing.Size(267, 326);
            this.myClientControl.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tbReseive);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(267, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.groupBox2.Size = new System.Drawing.Size(494, 326);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Принятые строки";
            // 
            // tbReseive
            // 
            this.tbReseive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbReseive.Location = new System.Drawing.Point(4, 19);
            this.tbReseive.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbReseive.Multiline = true;
            this.tbReseive.Name = "tbReseive";
            this.tbReseive.ReadOnly = true;
            this.tbReseive.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbReseive.Size = new System.Drawing.Size(486, 303);
            this.tbReseive.TabIndex = 0;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 326);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.myClientControl);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "FormMain";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormMain_FormClosed);
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MyClientControl myClientControl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox tbReseive;
    }
}

