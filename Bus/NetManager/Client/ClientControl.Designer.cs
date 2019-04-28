namespace NetManager.Client
{
    partial class ClientControl
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
            this.pNetworkSettings = new System.Windows.Forms.Panel();
            this.gbClients = new System.Windows.Forms.GroupBox();
            this.chClients = new System.Windows.Forms.CheckedListBox();
            this.pNetworkSettings.SuspendLayout();
            this.gbClients.SuspendLayout();
            this.SuspendLayout();
            // 
            // pNetworkSettings
            // 
            this.pNetworkSettings.Controls.Add(this.label2);
            this.pNetworkSettings.Controls.Add(this.tbIP);
            this.pNetworkSettings.Controls.Add(this.lPort);
            this.pNetworkSettings.Controls.Add(this.lIP);
            this.pNetworkSettings.Dock = System.Windows.Forms.DockStyle.Top;
            this.pNetworkSettings.Location = new System.Drawing.Point(0, 0);
            this.pNetworkSettings.Name = "pNetworkSettings";
            this.pNetworkSettings.Size = new System.Drawing.Size(174, 98);
            this.pNetworkSettings.TabIndex = 20;
            // 
            // gbClients
            // 
            this.gbClients.Controls.Add(this.chClients);
            this.gbClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbClients.Location = new System.Drawing.Point(0, 98);
            this.gbClients.Name = "gbClients";
            this.gbClients.Size = new System.Drawing.Size(174, 115);
            this.gbClients.TabIndex = 21;
            this.gbClients.TabStop = false;
            this.gbClients.Text = "Подключенные клиенты";
            // 
            // chClients
            // 
            this.chClients.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chClients.FormattingEnabled = true;
            this.chClients.Location = new System.Drawing.Point(3, 16);
            this.chClients.Name = "chClients";
            this.chClients.Size = new System.Drawing.Size(168, 96);
            this.chClients.TabIndex = 0;
            // 
            // ClientControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.Controls.Add(this.gbClients);
            this.Controls.Add(this.pNetworkSettings);
            this.Name = "ClientControl";
            this.Size = new System.Drawing.Size(174, 213);
            this.Controls.SetChildIndex(this.pNetworkSettings, 0);
            this.Controls.SetChildIndex(this.tbPort, 0);
            this.Controls.SetChildIndex(this.tbName, 0);
            this.Controls.SetChildIndex(this.gbClients, 0);
            this.pNetworkSettings.ResumeLayout(false);
            this.pNetworkSettings.PerformLayout();
            this.gbClients.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
        public System.Windows.Forms.Panel pNetworkSettings;
        public System.Windows.Forms.GroupBox gbClients;
        public System.Windows.Forms.CheckedListBox chClients;
#pragma warning restore CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
    }
}
