using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace Лаба_4
{
    public partial class Form1 : Form
    {
        private class InternetProvider{
            private const double MAX_SERVICE_COST = 1000000000;
            private List<IClient> clients = new List<IClient>();
            private string providerName;
            public InternetProvider(string name) { providerName = name; }
            public void AddClient(IClient client) { clients.Add(client); }
            public bool RemoveClient(string name){
                var client = clients.FirstOrDefault(c => c.GetName() == name);
                if (client != null) { clients.Remove(client); return true; }
                return false;
            }
            public IClient GetClient(string name){
                return clients.FirstOrDefault(c => c.GetName() == name);
            }
            public void SortClientsByName(){
                clients = clients.OrderBy(c => c.GetName()).ToList();
            }
            public void SortClientsByCost(){
                clients = clients.OrderBy(c => c.CalculateTotalCost()).ToList();
            }
            public double CalculateTotalRevenue(){
                return clients.Sum(c => c.CalculateTotalCost());
            }
            public int GetClientCount() { return clients.Count; }
            public string GetProviderName() { return providerName; }
            public List<IClient> GetClients() { return clients; }
        }
        private interface IPricingStrategy{
            double CalculateCost(double baseCost);
            string GetDescription();
        }
        private interface IClient{
            string GetName();
            string GetType();
            double GetBaseCost();
            double CalculateTotalCost();
            string GetPricingStrategyDescription();
            string GetAdditionalInfo();
        }
        private class StandardPricing : IPricingStrategy{
            public double CalculateCost(double baseCost) { return baseCost; }
            public string GetDescription() { return "Стандартная цена"; }
        }
        private class FixedDiscountPricing : IPricingStrategy{
            private double discount;
            public FixedDiscountPricing(double discount) { this.discount = discount; }
            public double CalculateCost(double baseCost) { return Math.Max(0, baseCost - discount); }
            public string GetDescription() { return $"Фиксированная скидка: {discount} руб."; }
        }
        private class PercentageDiscountPricing : IPricingStrategy{
            private double percent;
            public PercentageDiscountPricing(double percent) { this.percent = percent; }
            public double CalculateCost(double baseCost) { return baseCost * (1 - percent / 100); }
            public string GetDescription() { return $"Процентная скидка: {percent}%"; }
        }
        private abstract class Client : IClient{
            protected string name;
            protected double baseCost;
            protected IPricingStrategy strategy;
            public Client(string name, double cost, IPricingStrategy strategy){
                this.name = name;
                this.baseCost = cost;
                this.strategy = strategy;
            }
            public string GetName() { return name; }
            public double GetBaseCost() { return baseCost; }
            public double CalculateTotalCost() { return strategy.CalculateCost(baseCost); }
            public string GetPricingStrategyDescription() { return strategy.GetDescription(); }
            public abstract string GetType();
            public abstract string GetAdditionalInfo();
        }
        private class RegularClient : Client{
            public RegularClient(string name, double cost, IPricingStrategy strategy)
                : base(name, cost, strategy) { }
            public override string GetType() { return "Обычный клиент"; }
            public override string GetAdditionalInfo() { return "Нет дополнительной информации"; }
        }
        private class VIPClient : Client{
            private string vipLevel;
            public VIPClient(string name, double cost, string vipLevel, IPricingStrategy strategy)
                : base(name, cost, strategy) { this.vipLevel = vipLevel; }
            public override string GetType() { return "VIP клиент"; }
            public override string GetAdditionalInfo() { return $"Уровень VIP: {vipLevel}"; }
            public string GetVipLevel() { return vipLevel; }
        }
        private class CorporateClient : Client{
            private string companyName;
            public CorporateClient(string name, double cost, string company, IPricingStrategy strategy)
                : base(name, cost, strategy) { this.companyName = company; }
            public override string GetType() { return "Корпоративный клиент"; }
            public override string GetAdditionalInfo() { return $"Компания: {companyName}"; }
            public string GetCompanyName() { return companyName; }
        }
        private InternetProvider provider;
        public Form1(){
            InitializeComponent();
            provider = new InternetProvider("Мегафон");
            UpdateClientList();
            UpdateStats();
        }
        private DataGridView GetDataGridView(){
            var dgv = this.Controls.Find("dataGridView1", true).FirstOrDefault() as DataGridView;
            if (dgv == null){
                dgv = this.Controls.OfType<DataGridView>().FirstOrDefault();
            }
            return dgv;
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
        private void UpdateClientList(){
            var dataGridView = GetDataGridView();
            if (dataGridView == null){
                MessageBox.Show("DataGridView не найден на форме!", "Ошибка");
                return;
            }
            dataGridView.Rows.Clear();
            dataGridView.Columns.Clear();
            dataGridView.Columns.Add("Name", "Имя");
            dataGridView.Columns.Add("Type", "Тип");
            dataGridView.Columns.Add("BaseCost", "Базовая стоимость");
            dataGridView.Columns.Add("TotalCost", "Итоговая стоимость");
            dataGridView.Columns.Add("Pricing", "Стратегия ценообразования");
            dataGridView.Columns.Add("AdditionalInfo", "Дополнительная информация");
            foreach (var client in provider.GetClients()){
                dataGridView.Rows.Add(
                    client.GetName(),
                    client.GetType(),
                    client.GetBaseCost().ToString("F2"),
                    client.CalculateTotalCost().ToString("F2"),
                    client.GetPricingStrategyDescription(),
                    client.GetAdditionalInfo()
                );
            }
            UpdateStats();
        }
        private void UpdateStats(){
            try{
                double revenue = provider.CalculateTotalRevenue();
                int count = provider.GetClientCount();
                var labelRevenue = this.Controls.Find("labelTotalRevenue", true).FirstOrDefault() as Label;
                if (labelRevenue != null)
                    labelRevenue.Text = $"Общая выручка: {revenue:F2} руб.";
                var labelCount = this.Controls.Find("labelClientCount", true).FirstOrDefault() as Label;
                if (labelCount != null)
                    labelCount.Text = $"Количество клиентов: {count}";
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при обновлении статистики: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private IClient CreateClientFromForm(AddClientForm form){
            IPricingStrategy strategy;
            switch (form.PricingStrategy){
                case "Фиксированная скидка":
                    strategy = new FixedDiscountPricing(form.DiscountValue);
                    break;
                case "Процентная скидка":
                    strategy = new PercentageDiscountPricing(form.DiscountValue);
                    break;
                default:
                    strategy = new StandardPricing();
                    break;
            }
            IClient client;
            switch (form.ClientType){
                case "VIP клиент":
                    client = new VIPClient(form.ClientName, form.BaseCost, form.AdditionalInfo, strategy);
                    break;
                case "Корпоративный клиент":
                    client = new CorporateClient(form.ClientName, form.BaseCost, form.AdditionalInfo, strategy);
                    break;
                default:
                    client = new RegularClient(form.ClientName, form.BaseCost, strategy);
                    break;
            }
            return client;
        }
        private void buttonAddClient_Click(object sender, EventArgs e){
            try{
                using (AddClientForm addForm = new AddClientForm()){
                    if (addForm.ShowDialog() == DialogResult.OK){
                        IClient client = CreateClientFromForm(addForm);
                        provider.AddClient(client);
                        UpdateClientList();
                        MessageBox.Show($"Клиент {addForm.ClientName} успешно добавлен!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при добавлении клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonEditClient_Click(object sender, EventArgs e){
            var dataGridView = GetDataGridView();
            if (dataGridView == null || dataGridView.SelectedRows.Count == 0){
                MessageBox.Show("Выберите клиента для редактирования", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try{
                string clientName = dataGridView.SelectedRows[0].Cells["Name"].Value?.ToString();
                if (string.IsNullOrEmpty(clientName))
                    return;
                IClient existingClient = provider.GetClient(clientName);
                if (existingClient != null){
                    string currentType = existingClient.GetType();
                    double currentCost = existingClient.GetBaseCost();
                    string currentAdditionalInfo = existingClient.GetAdditionalInfo();
                    string currentPricingStrategy = existingClient.GetPricingStrategyDescription();
                    using (AddClientForm editForm = new AddClientForm(
                        clientName, currentType, currentCost, currentAdditionalInfo, currentPricingStrategy))
                    {
                        if (editForm.ShowDialog() == DialogResult.OK){
                            provider.RemoveClient(clientName);
                            IClient updatedClient = CreateClientFromForm(editForm);
                            provider.AddClient(updatedClient);
                            UpdateClientList();
                            MessageBox.Show($"Клиент {editForm.ClientName} успешно обновлен!", "OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при редактировании клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonDeleteClient_Click(object sender, EventArgs e){
            var dataGridView = GetDataGridView();
            if (dataGridView == null || dataGridView.SelectedRows.Count == 0){
                MessageBox.Show("Выберите клиента для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            try{
                string clientName = dataGridView.SelectedRows[0].Cells["Name"].Value?.ToString();
                if (string.IsNullOrEmpty(clientName))
                    return;
                if (MessageBox.Show($"Вы уверены, что хотите удалить клиента {clientName}?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes){
                    if (provider.RemoveClient(clientName)){
                        UpdateClientList();
                        MessageBox.Show($"Клиент {clientName} успешно удален!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при удалении клиента: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonSortByName_Click(object sender, EventArgs e){
            try{
                provider.SortClientsByName();
                UpdateClientList();
                MessageBox.Show("Клиенты отсортированы по имени", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonSortByCost_Click(object sender, EventArgs e){
            try{
                provider.SortClientsByCost();
                UpdateClientList();
                MessageBox.Show("Клиенты отсортированы по стоимости", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при сортировке: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void buttonSaveToFile_Click(object sender, EventArgs e){
            try{
                SaveFileDialog saveDialog = new SaveFileDialog();
                saveDialog.Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
                saveDialog.Title = "Сохранить данные клиентов";
                if (saveDialog.ShowDialog() == DialogResult.OK){
                    using (StreamWriter writer = new StreamWriter(saveDialog.FileName)){
                        writer.WriteLine("Internet Provider Client Data");
                        writer.WriteLine("==============================");
                        writer.WriteLine($"Провайдер: {provider.GetProviderName()}");
                        writer.WriteLine();
                        foreach (var client in provider.GetClients()){
                            writer.WriteLine($"Имя: {client.GetName()}");
                            writer.WriteLine($"Тип: {client.GetType()}");
                            writer.WriteLine($"Базовая стоимость: {client.GetBaseCost():F2} руб.");
                            writer.WriteLine($"Итоговая стоимость: {client.CalculateTotalCost():F2} руб.");
                            writer.WriteLine($"Стратегия: {client.GetPricingStrategyDescription()}");
                            writer.WriteLine($"Доп. информация: {client.GetAdditionalInfo()}");
                            writer.WriteLine("---");
                        }
                        writer.WriteLine();
                        writer.WriteLine("ОБЩАЯ СТАТИСТИКА:");
                        writer.WriteLine($"Общая выручка: {provider.CalculateTotalRevenue():F2} руб.");
                        writer.WriteLine($"Всего клиентов: {provider.GetClientCount()}");
                    }
                    MessageBox.Show($"Данные успешно сохранены в файл: {saveDialog.FileName}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex){
                MessageBox.Show($"Ошибка при сохранении файла: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button7_Click(object sender, EventArgs e){
            MessageBox.Show("Функция загрузки из файла будет реализована в следующей версии", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void labelTotalRevenue_Click(object sender, EventArgs e){}
        private IPricingStrategy CreatePricingStrategy(IClient client){
            string description = client.GetPricingStrategyDescription();
            if (description.Contains("Фиксированная скидка")){
                string discountStr = description.Split(':')[1].Trim().Split(' ')[0];
                if (double.TryParse(discountStr, out double discount))
                    return new FixedDiscountPricing(discount);
            }else if (description.Contains("Процентная скидка")){
                string discountStr = description.Split(':')[1].Trim().Split('%')[0];
                if (double.TryParse(discountStr, out double discount))
                    return new PercentageDiscountPricing(discount);
            }
            return new StandardPricing();
        }
    }
}