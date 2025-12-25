using System.Windows.Forms;

namespace Лаба_4
{
    partial class Form1
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
            this.dataGridViewClients = new System.Windows.Forms.DataGridView();
            this.buttonAddClient = new System.Windows.Forms.Button();
            this.buttonEditClient = new System.Windows.Forms.Button();
            this.buttonDeleteClient = new System.Windows.Forms.Button();
            this.buttonSortByName = new System.Windows.Forms.Button();
            this.buttonSortByCost = new System.Windows.Forms.Button();
            this.buttonSaveToFile = new System.Windows.Forms.Button();
            this.buttonLoadFromFile = new System.Windows.Forms.Button();
            this.labelTotalRevenue = new System.Windows.Forms.Label();
            this.labelClientCount = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClients)).BeginInit();
            this.SuspendLayout();
            this.dataGridViewClients.AllowUserToAddRows = false;
            this.dataGridViewClients.AllowUserToDeleteRows = false;
            this.dataGridViewClients.BackgroundColor = System.Drawing.SystemColors.ActiveCaption;
            this.dataGridViewClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewClients.Location = new System.Drawing.Point(400, 120);
            this.dataGridViewClients.Name = "dataGridViewClients";
            this.dataGridViewClients.ReadOnly = true;
            this.dataGridViewClients.RowHeadersWidth = 51;
            this.dataGridViewClients.RowTemplate.Height = 24;
            this.dataGridViewClients.Size = new System.Drawing.Size(900, 500);
            this.dataGridViewClients.TabIndex = 0;
            this.dataGridViewClients.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.buttonAddClient.Location = new System.Drawing.Point(1390, 120);
            this.buttonAddClient.Name = "buttonAddClient";
            this.buttonAddClient.Size = new System.Drawing.Size(120, 25);
            this.buttonAddClient.TabIndex = 1;
            this.buttonAddClient.Text = "Добавить\r\n ";
            this.buttonAddClient.UseVisualStyleBackColor = true;
            this.buttonAddClient.Click += new System.EventHandler(this.buttonAddClient_Click); 
            this.buttonEditClient.Location = new System.Drawing.Point(1390, 165);
            this.buttonEditClient.Name = "buttonEditClient";
            this.buttonEditClient.Size = new System.Drawing.Size(120, 25);
            this.buttonEditClient.TabIndex = 2;
            this.buttonEditClient.Text = "Изменить\r\n";
            this.buttonEditClient.UseVisualStyleBackColor = true;
            this.buttonEditClient.Click += new System.EventHandler(this.buttonEditClient_Click);
            this.buttonDeleteClient.Location = new System.Drawing.Point(1390, 210);
            this.buttonDeleteClient.Name = "buttonDeleteClient";
            this.buttonDeleteClient.Size = new System.Drawing.Size(120, 25);
            this.buttonDeleteClient.TabIndex = 3;
            this.buttonDeleteClient.Text = "Удалить";
            this.buttonDeleteClient.UseVisualStyleBackColor = true;
            this.buttonDeleteClient.Click += new System.EventHandler(this.buttonDeleteClient_Click);
            this.buttonSortByName.Location = new System.Drawing.Point(477, 64);
            this.buttonSortByName.Name = "buttonSortByName";
            this.buttonSortByName.Size = new System.Drawing.Size(120, 50);
            this.buttonSortByName.TabIndex = 4;
            this.buttonSortByName.Text = "Сортировать по имени\r\n";
            this.buttonSortByName.UseVisualStyleBackColor = true;
            this.buttonSortByName.Click += new System.EventHandler(this.buttonSortByName_Click);
            this.buttonSortByCost.Location = new System.Drawing.Point(875, 64);
            this.buttonSortByCost.Name = "buttonSortByCost";
            this.buttonSortByCost.Size = new System.Drawing.Size(120, 50);
            this.buttonSortByCost.TabIndex = 5;
            this.buttonSortByCost.Text = "Сортировать по стоимости";
            this.buttonSortByCost.UseVisualStyleBackColor = true;
            this.buttonSortByCost.Click += new System.EventHandler(this.buttonSortByCost_Click);
            this.buttonSaveToFile.Location = new System.Drawing.Point(1390, 255);
            this.buttonSaveToFile.Name = "buttonSaveToFile";
            this.buttonSaveToFile.Size = new System.Drawing.Size(120, 25);
            this.buttonSaveToFile.TabIndex = 6;
            this.buttonSaveToFile.Text = "Сохранить";
            this.buttonSaveToFile.UseVisualStyleBackColor = true;
            this.buttonSaveToFile.Click += new System.EventHandler(this.buttonSaveToFile_Click);
            this.buttonLoadFromFile.Location = new System.Drawing.Point(1390, 300);
            this.buttonLoadFromFile.Name = "buttonLoadFromFile";
            this.buttonLoadFromFile.Size = new System.Drawing.Size(120, 25);
            this.buttonLoadFromFile.TabIndex = 7;
            this.buttonLoadFromFile.Text = "Загрузить";
            this.buttonLoadFromFile.UseVisualStyleBackColor = true;
            this.buttonLoadFromFile.Click += new System.EventHandler(this.button7_Click);
            this.labelTotalRevenue.AccessibleRole = System.Windows.Forms.AccessibleRole.IpAddress;
            this.labelTotalRevenue.AutoSize = true;
            this.labelTotalRevenue.Location = new System.Drawing.Point(600, 700);
            this.labelTotalRevenue.Name = "labelTotalRevenue";
            this.labelTotalRevenue.Size = new System.Drawing.Size(151, 16);
            this.labelTotalRevenue.TabIndex = 8;
            this.labelTotalRevenue.Text = "Общая выручка: 0 руб.";
            this.labelTotalRevenue.Click += new System.EventHandler(this.labelTotalRevenue_Click);
            this.labelClientCount.AutoSize = true;
            this.labelClientCount.Location = new System.Drawing.Point(850, 700);
            this.labelClientCount.Name = "labelClientCount";
            this.labelClientCount.Size = new System.Drawing.Size(163, 16);
            this.labelClientCount.TabIndex = 9;
            this.labelClientCount.Text = "Количество клинетов: 0";
            this.openFileDialog1.FileName = "openFileDialog1";
            this.buttonCalculateStats = new System.Windows.Forms.Button();
            this.buttonCalculateStats.Location = new System.Drawing.Point(1250, 64);
            this.buttonCalculateStats.Name = "buttonCalculateStats";
            this.buttonCalculateStats.Size = new System.Drawing.Size(150, 50);
            this.buttonCalculateStats.TabIndex = 10;
            this.buttonCalculateStats.Text = "Подсчитать статистику";
            this.buttonCalculateStats.UseVisualStyleBackColor = true;
            this.buttonCalculateStats.Click += new System.EventHandler(this.buttonCalculateStats_Click);
            this.buttonConnectDB = new System.Windows.Forms.Button();
            this.buttonConnectDB.Location = new System.Drawing.Point(1390, 345);
            this.buttonConnectDB.Name = "buttonConnectDB";
            this.buttonConnectDB.Size = new System.Drawing.Size(120, 25);
            this.buttonConnectDB.TabIndex = 11;
            this.buttonConnectDB.Text = "Подключить БД";
            this.buttonConnectDB.UseVisualStyleBackColor = true;
            this.buttonConnectDB.Click += new System.EventHandler(this.buttonConnectDB_Click);
            this.labelStats = new System.Windows.Forms.Label();
            this.labelStats.AutoSize = true;
            this.labelStats.Location = new System.Drawing.Point(600, 700); 
            this.labelStats.Name = "labelStats";
            this.labelStats.Size = new System.Drawing.Size(300, 16);
            this.labelStats.TabIndex = 12;
            this.labelStats.Text = "Подключитесь к БД для отображения статистики";
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1864, 1003);
            this.Controls.Add(this.labelStats);
            this.Controls.Add(this.buttonConnectDB);
            this.Controls.Add(this.labelClientCount);
            this.Controls.Add(this.labelTotalRevenue);
            this.Controls.Add(this.buttonLoadFromFile);
            this.Controls.Add(this.buttonSaveToFile);
            this.Controls.Add(this.buttonSortByCost);
            this.Controls.Add(this.buttonSortByName);
            this.Controls.Add(this.buttonDeleteClient);
            this.Controls.Add(this.buttonEditClient);
            this.Controls.Add(this.buttonAddClient);
            this.Controls.Add(this.dataGridViewClients);
            this.Controls.Add(this.buttonCalculateStats);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Система управления клиентами интернет-провайдера";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewClients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        private System.Windows.Forms.DataGridView dataGridViewClients;
        private System.Windows.Forms.Button buttonAddClient;
        private System.Windows.Forms.Button buttonEditClient;
        private System.Windows.Forms.Button buttonDeleteClient;
        private System.Windows.Forms.Button buttonSortByName;
        private System.Windows.Forms.Button buttonSortByCost;
        private System.Windows.Forms.Button buttonSaveToFile;
        private System.Windows.Forms.Button buttonLoadFromFile;
        private System.Windows.Forms.Label labelTotalRevenue;
        private System.Windows.Forms.Label labelClientCount;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button buttonCalculateStats;
        private System.Windows.Forms.Button buttonConnectDB;
        private System.Windows.Forms.Label labelStats;
    }
}

