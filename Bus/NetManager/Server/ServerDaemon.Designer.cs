namespace NetManager.Server
{
    partial class ServerDaemon
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
            this.tbClients = new System.Windows.Forms.TextBox();
            this.pClientCount = new System.Windows.Forms.Panel();
            this.lClients = new System.Windows.Forms.Label();
            this.gbNetworkSettings = new System.Windows.Forms.GroupBox();
            this.pIPAddress = new System.Windows.Forms.Panel();
            this.lIPAddress = new System.Windows.Forms.Label();
            this.tbIPAddress = new System.Windows.Forms.TextBox();
            this.pNetworkSettings = new System.Windows.Forms.Panel();
            this.btnStart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nLiveTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).BeginInit();
            this.pClientCount.SuspendLayout();
            this.gbNetworkSettings.SuspendLayout();
            this.pIPAddress.SuspendLayout();
            this.pNetworkSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // nLiveTime
            // 
            this.nLiveTime.Location = new System.Drawing.Point(10, 81);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(10, 64);
            // 
            // nPort
            // 
            this.nPort.Location = new System.Drawing.Point(10, 36);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(10, 19);
            // 
            // tbClients
            // 
            this.tbClients.BackColor = System.Drawing.SystemColors.Window;
            this.tbClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbClients.Location = new System.Drawing.Point(0, 160);
            this.tbClients.Multiline = true;
            this.tbClients.Name = "tbClients";
            this.tbClients.ReadOnly = true;
            this.tbClients.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbClients.Size = new System.Drawing.Size(370, 130);
            this.tbClients.TabIndex = 7;
            // 
            // pClientCount
            // 
            this.pClientCount.AutoSize = true;
            this.pClientCount.Controls.Add(this.lClients);
            this.pClientCount.Dock = System.Windows.Forms.DockStyle.Top;
            this.pClientCount.Location = new System.Drawing.Point(0, 147);
            this.pClientCount.Name = "pClientCount";
            this.pClientCount.Size = new System.Drawing.Size(370, 13);
            this.pClientCount.TabIndex = 8;
            // 
            // lClients
            // 
            this.lClients.AutoSize = true;
            this.lClients.Location = new System.Drawing.Point(0, 0);
            this.lClients.Name = "lClients";
            this.lClients.Size = new System.Drawing.Size(104, 13);
            this.lClients.TabIndex = 4;
            this.lClients.Text = "Число клиентов - 0";
            // 
            // gbNetworkSettings
            // 
            this.gbNetworkSettings.Controls.Add(this.pIPAddress);
            this.gbNetworkSettings.Controls.Add(this.pNetworkSettings);
            this.gbNetworkSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbNetworkSettings.Location = new System.Drawing.Point(0, 0);
            this.gbNetworkSettings.Name = "gbNetworkSettings";
            this.gbNetworkSettings.Size = new System.Drawing.Size(370, 147);
            this.gbNetworkSettings.TabIndex = 6;
            this.gbNetworkSettings.TabStop = false;
            this.gbNetworkSettings.Text = " Сетевые настройки ";
            // 
            // pIPAddress
            // 
            this.pIPAddress.Controls.Add(this.lIPAddress);
            this.pIPAddress.Controls.Add(this.tbIPAddress);
            this.pIPAddress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pIPAddress.Location = new System.Drawing.Point(143, 16);
            this.pIPAddress.Name = "pIPAddress";
            this.pIPAddress.Size = new System.Drawing.Size(224, 128);
            this.pIPAddress.TabIndex = 6;
            // 
            // lIPAddress
            // 
            this.lIPAddress.AutoSize = true;
            this.lIPAddress.Location = new System.Drawing.Point(8, 8);
            this.lIPAddress.Name = "lIPAddress";
            this.lIPAddress.Size = new System.Drawing.Size(60, 13);
            this.lIPAddress.TabIndex = 4;
            this.lIPAddress.Text = "IP Адреса:";
            // 
            // tbIPAddress
            // 
            this.tbIPAddress.BackColor = System.Drawing.SystemColors.Window;
            this.tbIPAddress.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbIPAddress.Location = new System.Drawing.Point(0, 24);
            this.tbIPAddress.Multiline = true;
            this.tbIPAddress.Name = "tbIPAddress";
            this.tbIPAddress.ReadOnly = true;
            this.tbIPAddress.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbIPAddress.Size = new System.Drawing.Size(224, 104);
            this.tbIPAddress.TabIndex = 3;
            // 
            // pNetworkSettings
            // 
            this.pNetworkSettings.Controls.Add(this.btnStart);
            this.pNetworkSettings.Dock = System.Windows.Forms.DockStyle.Left;
            this.pNetworkSettings.Location = new System.Drawing.Point(3, 16);
            this.pNetworkSettings.Name = "pNetworkSettings";
            this.pNetworkSettings.Size = new System.Drawing.Size(140, 128);
            this.pNetworkSettings.TabIndex = 5;
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(26, 92);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 4;
            this.btnStart.Text = "Старт";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.button1_Click);
            // 
            // ServerDaemon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbClients);
            this.Controls.Add(this.pClientCount);
            this.Controls.Add(this.gbNetworkSettings);
            this.Name = "ServerDaemon";
            this.Size = new System.Drawing.Size(370, 290);
            this.Controls.SetChildIndex(this.gbNetworkSettings, 0);
            this.Controls.SetChildIndex(this.pClientCount, 0);
            this.Controls.SetChildIndex(this.tbClients, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.Controls.SetChildIndex(this.nPort, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.nLiveTime, 0);
            ((System.ComponentModel.ISupportInitialize)(this.nLiveTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nPort)).EndInit();
            this.pClientCount.ResumeLayout(false);
            this.pClientCount.PerformLayout();
            this.gbNetworkSettings.ResumeLayout(false);
            this.pIPAddress.ResumeLayout(false);
            this.pIPAddress.PerformLayout();
            this.pNetworkSettings.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
        public System.Windows.Forms.GroupBox gbNetworkSettings;
        public System.Windows.Forms.TextBox tbIPAddress;
        public System.Windows.Forms.Panel pNetworkSettings;
        public System.Windows.Forms.Label lIPAddress;
        public System.Windows.Forms.Label lClients;
        public System.Windows.Forms.TextBox tbClients;
        public System.Windows.Forms.Panel pClientCount;
        public System.Windows.Forms.Button btnStart;
        public System.Windows.Forms.Panel pIPAddress;
#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    }
}
