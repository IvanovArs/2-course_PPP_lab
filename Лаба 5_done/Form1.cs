using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace Лаба_4
{
    public partial class Form1 : Form{
        private DatabaseManager dbManager;
        private List<IClient> clients;
        public Form1(){
            InitializeComponent();
            labelTotalRevenue.Visible = false;
            labelClientCount.Visible = false;
            clients = new List<IClient>();
            ConfigureDataGridView();
            labelStats.Text = "Нажмите 'Подключить БД' для загрузки данных";
            dbManager = null;
            buttonAddClient.Enabled = false;     
            buttonEditClient.Enabled = false;     
            buttonDeleteClient.Enabled = false;  
            buttonConnectDB.Enabled = true;    
            buttonConnectDB.BackColor = System.Drawing.Color.LightBlue;
            buttonConnectDB.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Bold);
        }
        private void InitializeDatabaseManually(){
            try{
                MessageBox.Show("База данных инициализирована успешно!", "OK");
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка инициализации БД: {ex.Message}", "Ошибка");
            }
        }
        protected override void OnFormClosed(FormClosedEventArgs e){
            dbManager?.Dispose(); 
            base.OnFormClosed(e);
        }
        private void LoadClientsFromDatabase(){
            if (dbManager == null) return;
            try{
                clients = dbManager.GetAllClients();
                UpdateDataGridView();
                UpdateStats();
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateClientList(){
            var dataGridView = GetDataGridView();
            if (dataGridView == null) return;
            clients = dbManager.GetAllClients();
            if (clients == null || clients.Count == 0){
                dataGridView.Rows.Clear();
                UpdateStats();
                return;
            }
            dataGridView.Rows.Clear();
            foreach (var client in clients){
                dataGridView.Rows.Add(
                    client.Id,
                    client.Name,
                    GetClientTypeDisplayName(client.ClientType),
                    client.BaseCost.ToString("F2"),
                    client.CalculateTotalCost().ToString("F2"),
                    client.GetPricingStrategyDescription(),
                    client.AdditionalInfo
                );
            }
            UpdateStats();
        }
        private string GetClientTypeDisplayName(string clientType){
            switch (clientType){
                case "Regular": return "Обычный клиент";
                case "VIP": return "VIP клиент";
                case "Corporate": return "Корпоративный клиент";
                default: return clientType;
            }
        }
        private void UpdateStats(){
            if (clients == null || clients.Count == 0){
                labelStats.Text = "Клиентов нет";
                return;
            }
            int count = clients.Count;
            double revenue = clients.Sum(c => c.CalculateTotalCost());
            labelStats.Text = $"Всего клиентов: {count} | Общая выручка: {revenue:F2} руб.";
        }
        private DataGridView GetDataGridView(){
            return this.Controls.Find("dataGridView1", true).FirstOrDefault() as DataGridView;
        }
        private void buttonAddClient_Click(object sender, EventArgs e){
            if (dbManager == null){
                MessageBox.Show("Сначала подключитесь к базе данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var addForm = new AddClientForm()){
                if (addForm.ShowDialog() == DialogResult.OK){
                    var client = addForm.GetClient();
                    dbManager.AddClient(client);
                    LoadClientsFromDatabase();
                    MessageBox.Show($"Клиент {client.Name} успешно добавлен!", "OK");
                }
            }
        }
        private void buttonEditClient_Click(object sender, EventArgs e){
            if (dbManager == null){
                MessageBox.Show("Сначала подключитесь к базе данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dataGridViewClients.SelectedRows.Count == 0){
                MessageBox.Show("Выберите клиента для редактирования", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try{
                DataGridViewRow selectedRow = dataGridViewClients.SelectedRows[0];
                int clientId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                IClient clientToEdit = dbManager.GetClientById(clientId);
                if (clientToEdit == null){
                    MessageBox.Show("Клиент не найден в базе данных", "Ошибка");
                    return;
                }
                using (AddClientForm editForm = new AddClientForm(clientToEdit)){
                    if (editForm.ShowDialog() == DialogResult.OK){
                        IClient updatedClient = editForm.GetClient();
                        updatedClient.Id = clientId; 
                        dbManager.UpdateClient(updatedClient);
                        LoadClientsFromDatabase();
                        MessageBox.Show($"Клиент '{updatedClient.Name}' успешно обновлен!", "ОК", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при редактировании: {ex.Message}", "Ошибка");
            }
        }
        private void buttonDeleteClient_Click(object sender, EventArgs e){
            if (dbManager == null){
                MessageBox.Show("Сначала подключитесь к базе данных!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (dataGridViewClients.SelectedRows.Count == 0){
                MessageBox.Show("Выберите клиента для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try{
                DataGridViewRow selectedRow = dataGridViewClients.SelectedRows[0];
                int clientId = Convert.ToInt32(selectedRow.Cells["Id"].Value);
                string clientName = selectedRow.Cells["Name"].Value?.ToString() ?? "Неизвестный клиент";
                string clientType = GetClientTypeDisplayName(selectedRow.Cells["ClientType"].Value?.ToString() ?? "");
                string confirmationMessage =
                    $"ВЫ УВЕРЕНЫ, ЧТО ХОТИТЕ УДАЛИТЬ КЛИЕНТА?\n\n" +
                    $"▸ Имя: {clientName}\n" +
                    $"▸ Тип: {clientType}\n" +
                    $"▸ ID: {clientId}\n\n" +
                    $"Это действие НЕЛЬЗЯ отменить!";
                DialogResult result = MessageBox.Show(
                    confirmationMessage,
                    "ПОДТВЕРЖДЕНИЕ УДАЛЕНИЯ",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2 
                );
                if (result == DialogResult.Yes){
                    dbManager.DeleteClient(clientId);
                    LoadClientsFromDatabase();
                    MessageBox.Show($"Клиент '{clientName}' успешно удален!", "ОК", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }else{
                    MessageBox.Show("Удаление отменено", "Отмена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка");
            }
        }
        private void buttonSortByName_Click(object sender, EventArgs e){
            clients = clients.OrderBy(c => c.Name).ToList();
            UpdateClientList();
            MessageBox.Show("Клиенты отсортированы по имени", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void buttonSortByCost_Click(object sender, EventArgs e){
            clients = clients.OrderBy(c => c.CalculateTotalCost()).ToList();
            UpdateClientList();
            MessageBox.Show("Клиенты отсортированы по стоимости", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void buttonSaveToFile_Click(object sender, EventArgs e){
            try{
                using (SaveFileDialog saveDialog = new SaveFileDialog()){
                    saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt|CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                    saveDialog.Title = "Экспорт данных клиентов";
                    if (saveDialog.ShowDialog() == DialogResult.OK){
                        ExportDataToFile(saveDialog.FileName);
                        MessageBox.Show($"Данные успешно экспортированы в файл: {saveDialog.FileName}", "ОК", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при экспорте данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonLoadFromFile_Click(object sender, EventArgs e){
            try{
                using (OpenFileDialog openDialog = new OpenFileDialog()){
                    openDialog.Filter = "Текстовые файлы (*.txt)|*.txt|CSV файлы (*.csv)|*.csv|Все файлы (*.*)|*.*";
                    openDialog.Title = "Импорт данных клиентов";
                    if (openDialog.ShowDialog() == DialogResult.OK){
                        MessageBox.Show("Функция импорта будет реализована в следующей версии", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при импорте данных: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button7_Click(object sender, EventArgs e){
            buttonLoadFromFile_Click(sender, e);
        }
        private void ExportDataToFile(string filePath){
            using (StreamWriter writer = new StreamWriter(filePath)){
                writer.WriteLine("Internet Provider Client Data Export");
                writer.WriteLine("=====================================");
                writer.WriteLine($"Дата экспорта: {DateTime.Now}");
                writer.WriteLine($"Всего клиентов: {clients.Count}");
                writer.WriteLine();
                foreach (var client in clients){
                    writer.WriteLine($"ID: {client.Id}");
                    writer.WriteLine($"Имя: {client.Name}");
                    writer.WriteLine($"Тип: {GetClientTypeDisplayName(client.ClientType)}");
                    writer.WriteLine($"Базовая стоимость: {client.BaseCost:F2} руб.");
                    writer.WriteLine($"Итоговая стоимость: {client.CalculateTotalCost():F2} руб.");
                    writer.WriteLine($"Стратегия: {client.GetPricingStrategyDescription()}");
                    writer.WriteLine($"Доп. информация: {client.AdditionalInfo}");
                    writer.WriteLine("---");
                }
                double totalRevenue = clients.Sum(c => c.CalculateTotalCost());
                writer.WriteLine();
                writer.WriteLine("ОБЩАЯ СТАТИСТИКА:");
                writer.WriteLine($"Общая выручка: {totalRevenue:F2} руб.");
                writer.WriteLine($"Средняя стоимость: {clients.Average(c => c.CalculateTotalCost()):F2} руб.");
            }
        }
        private void ConfigureDataGridView(){
            dataGridViewClients.Columns.Clear();
            dataGridViewClients.Columns.Add("Id", "ID");
            dataGridViewClients.Columns.Add("Name", "Имя");
            dataGridViewClients.Columns.Add("ClientType", "Тип клиента");
            dataGridViewClients.Columns.Add("BaseCost", "Базовая стоимость");
            dataGridViewClients.Columns.Add("TotalCost", "Итоговая стоимость");
            dataGridViewClients.Columns.Add("PricingStrategy", "Стратегия ценообразования");
            dataGridViewClients.Columns.Add("AdditionalInfo", "Дополнительная информация");
            dataGridViewClients.Columns["Id"].Visible = false;
            dataGridViewClients.Columns["Name"].Width = 150;
            dataGridViewClients.Columns["ClientType"].Width = 120;
            dataGridViewClients.Columns["BaseCost"].Width = 120;
            dataGridViewClients.Columns["TotalCost"].Width = 120;
            dataGridViewClients.Columns["PricingStrategy"].Width = 180;
            dataGridViewClients.Columns["AdditionalInfo"].Width = 200;
        }
        private void buttonConnectDB_Click(object sender, EventArgs e){
            try
            {
                if (dbManager == null){
                    dbManager = new DatabaseManager();
                    MessageBox.Show("База данных подключена успешно!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    buttonConnectDB.Text = "Обновить данные";
                    buttonConnectDB.BackColor = System.Drawing.Color.LightGreen;
                    buttonAddClient.Enabled = true;
                    buttonEditClient.Enabled = true;
                    buttonDeleteClient.Enabled = true;
                    buttonSortByName.Enabled = true;
                    buttonSortByCost.Enabled = true;
                    buttonSaveToFile.Enabled = true;
                    buttonLoadFromFile.Enabled = true;
                    buttonAddClient.BackColor = System.Drawing.Color.LightGreen;
                    buttonEditClient.BackColor = System.Drawing.Color.LightYellow;
                    buttonDeleteClient.BackColor = System.Drawing.Color.LightCoral;
                }
                LoadClientsFromDatabase();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка подключения к БД: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dbManager = null;
                buttonAddClient.Enabled = false;
                buttonEditClient.Enabled = false;
                buttonDeleteClient.Enabled = false;
            }
        }
        private void UpdateDataGridView(){
            dataGridViewClients.Rows.Clear();
            if (clients == null || clients.Count == 0){
                labelStats.Text = "В базе данных нет клиентов. Добавьте первого клиента.";
                return;
            }
            foreach (var client in clients){
                dataGridViewClients.Rows.Add(
                    client.Id,
                    client.Name,
                    GetClientTypeDisplayName(client.ClientType),
                    client.BaseCost.ToString("F2"),
                    client.CalculateTotalCost().ToString("F2"),
                    client.GetPricingStrategyDescription(),
                    client.AdditionalInfo
                );
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e){
        }
        private void labelTotalRevenue_Click(object sender, EventArgs e){
        }
        private void CheckDatabaseConnection(){
            try{
                using (var context = new AppDbContext()){
                    context.Database.EnsureCreated();
                    var tableExists = context.Database.ExecuteSqlRaw(
                        "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Clients'");
                    Console.WriteLine($"Таблица Clients существует: {tableExists > 0}");
                    var count = context.Clients.Count();
                    Console.WriteLine($"Записей в базе: {count}");
                }
            }
            catch (Exception ex){
                Console.WriteLine($"Ошибка проверки базы: {ex.Message}");
            }
        }
        private void buttonCalculateStats_Click(object sender, EventArgs e){
            CalculateAndShowStatistics();
        }
        private void CalculateAndShowStatistics(){
            if (clients == null || clients.Count == 0){
                MessageBox.Show("Нет клиентов для подсчета статистики", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try{
                double totalRevenue = clients.Sum(c => c.CalculateTotalCost());
                int regularCount = clients.Count(c => c.ClientType == "Regular");
                int vipCount = clients.Count(c => c.ClientType == "VIP");
                int corporateCount = clients.Count(c => c.ClientType == "Corporate");
                double averageCost = clients.Average(c => c.CalculateTotalCost());
                string message = $"СТАТИСТИКА ПРОВАЙДЕРА:\n\n" +
                                $"Всего клиентов: {clients.Count}\n" +
                                $"• Обычные: {regularCount}\n" +
                                $"• VIP: {vipCount}\n" +
                                $"• Корпоративные: {corporateCount}\n\n" +
                                $"Общая выручка: {totalRevenue:F2} руб.\n" +
                                $"Средняя стоимость: {averageCost:F2} руб.\n\n" +
                                $"Данные актуальны на: {DateTime.Now:dd.MM.yyyy HH:mm}";
                MessageBox.Show(message, "Статистика провайдера", MessageBoxButtons.OK, MessageBoxIcon.Information);
                UpdateStats();
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при подсчете статистики: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}