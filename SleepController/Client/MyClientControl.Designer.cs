namespace ClientExample
{
    partial class MyClientControl
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
            this.button1 = new System.Windows.Forms.Button();
            this.pNetworkSettings.SuspendLayout();
            this.gbClients.SuspendLayout();
            this.SuspendLayout();
            // 
            // pNetworkSettings
            // 
            this.pNetworkSettings.Controls.Add(this.button1);
            this.pNetworkSettings.Controls.SetChildIndex(this.lIP, 0);
            this.pNetworkSettings.Controls.SetChildIndex(this.lPort, 0);
            this.pNetworkSettings.Controls.SetChildIndex(this.tbIP, 0);
            this.pNetworkSettings.Controls.SetChildIndex(this.label2, 0);
            this.pNetworkSettings.Controls.SetChildIndex(this.button1, 0);
            // 
            // chClients
            // 
            this.chClients.Size = new System.Drawing.Size(194, 94);
            // 
            // tbName
            // 
            this.tbName.Size = new System.Drawing.Size(88, 20);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(107, 67);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 19;
            this.button1.Text = "Старт";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // MyClientControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Name = "MyClientControl";
            this.pNetworkSettings.ResumeLayout(false);
            this.pNetworkSettings.PerformLayout();
            this.gbClients.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
    }
}
