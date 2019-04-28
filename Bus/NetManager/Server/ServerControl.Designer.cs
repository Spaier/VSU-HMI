namespace NetManager.Server
{
    partial class ServerControl
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.nLiveTime = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nPort = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nLiveTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.SuspendLayout();
            // 
            // nTestLive
            // 
            this.nLiveTime.Location = new System.Drawing.Point(8, 70);
            this.nLiveTime.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nLiveTime.Name = "nTestLive";
            this.nLiveTime.Size = new System.Drawing.Size(120, 20);
            this.nLiveTime.TabIndex = 6;
            this.nLiveTime.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nLiveTime.ValueChanged += new System.EventHandler(this.nTestLive_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(117, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Время проверки (мс):";
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(8, 25);
            this.nPort.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.nPort.Minimum = new decimal(new int[] {
            1023,
            0,
            0,
            0});
            this.nPort.Name = "nPort";
            this.nPort.Size = new System.Drawing.Size(120, 20);
            this.nPort.TabIndex = 1;
            this.nPort.Value = new decimal(new int[] {
            8000,
            0,
            0,
            0});
            this.nPort.ValueChanged += new System.EventHandler(this.nPort_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Номер порта:";
            // 
            // ServerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.nLiveTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nPort);
            this.Controls.Add(this.label1);
            this.Name = "ServerControl";
            this.Size = new System.Drawing.Size(136, 98);
            ((System.ComponentModel.ISupportInitialize)(this.nLiveTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.NumericUpDown nLiveTime;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.NumericUpDown nPort;
        public System.Windows.Forms.Label label1;
    }
}
