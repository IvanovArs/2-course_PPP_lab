using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
namespace Лаба_4
{
    public partial class DatabaseViewerForm : Form{
        private DatabaseManager dbManager;
        private Label labelTotalClients;
        private Label labelTotalRevenue;
        private Label labelDatabaseSize;
        private Label labelLastUpdate;
        private Button buttonOpenInBrowser;
        private Button buttonBackup;
        private Button buttonClose;
        public DatabaseViewerForm(DatabaseManager manager){
            InitializeComponent();
            dbManager = manager;
            LoadDatabaseInfo();
        }
        private void LoadDatabaseInfo(){
            try{
                var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.db");
                if (File.Exists(databasePath)){
                    var clients = dbManager.GetAllClients();
                    double totalRevenue = clients.Sum(c => c.CalculateTotalCost());
                    var fileInfo = new FileInfo(databasePath);
                    labelTotalClients.Text = $"Всего клиентов: {clients.Count}";
                    labelTotalRevenue.Text = $"Общая выручка: {totalRevenue:F2} руб.";
                    labelDatabaseSize.Text = $"Размер БД: {fileInfo.Length} байт";
                    labelLastUpdate.Text = $"Последнее обновление: {fileInfo.LastWriteTime:dd.MM.yyyy HH:mm}";
                }else{
                    labelTotalClients.Text = "Всего клиентов: 0";
                    labelTotalRevenue.Text = "Общая выручка: 0 руб.";
                    labelDatabaseSize.Text = "Размер БД: файл не найден";
                    labelLastUpdate.Text = "Последнее обновление: нет данных";
                }
            }
            catch (Exception ex){
                labelTotalClients.Text = "Ошибка загрузки данных";
                labelTotalRevenue.Text = $"Ошибка: {ex.Message}";
            }
        }
        private void buttonOpenInBrowser_Click(object sender, EventArgs e){
            try{
                var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.db");
                if (File.Exists(databasePath)){
                    System.Diagnostics.Process.Start("explorer.exe", $"/select,\"{databasePath}\"");
                }else{
                    MessageBox.Show("Файл базы данных не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonBackup_Click(object sender, EventArgs e){
            try{
                var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "clients.db");
                if (!File.Exists(databasePath)){
                    MessageBox.Show("Файл базы данных не найден!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                using (SaveFileDialog saveDialog = new SaveFileDialog()){
                    saveDialog.Filter = "SQLite Database (*.db)|*.db|Все файлы (*.*)|*.*";
                    saveDialog.FileName = $"clients_backup_{DateTime.Now:yyyyMMdd_HHmmss}.db";
                    saveDialog.Title = "Создание резервной копии базы данных";
                    if (saveDialog.ShowDialog() == DialogResult.OK){
                        File.Copy(databasePath, saveDialog.FileName, true);
                        MessageBox.Show($"Резервная копия создана успешно!\n\nПуть: {saveDialog.FileName}", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при создании резервной копии: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonClose_Click(object sender, EventArgs e){
            this.Close();
        }
    }
}