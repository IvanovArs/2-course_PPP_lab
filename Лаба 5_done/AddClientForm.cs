using System;
using System.Windows.Forms;
namespace Лаба_4
{
    public partial class AddClientForm : Form{
        private IClient client;
        private bool isEditMode;
        public AddClientForm(){
            InitializeComponent();
            isEditMode = false;
            this.Text = "Добавление нового клиента";
            InitializeFormControls();
        }
        public AddClientForm(IClient existingClient) : this() {
            isEditMode = true;
            client = existingClient;
            LoadClientData(); 
            this.Text = $"Редактирование клиента: {client.Name}";
            this.BackColor = System.Drawing.Color.LightYellow; 
            buttonOK.Text = "Сохранить";
        }
        private void InitializeFormControls(){
            comboBoxClientType.SelectedIndex = 0;
            comboBoxPricingStrategy.SelectedIndex = 0;
            UpdateAdditionalInfoLabel();
            UpdateDiscountVisibility();
        }
        public IClient GetClient(){
            return client;
        }
        private void LoadClientData(){
            if (client == null) return;
            textBoxName.Text = client.Name;
            textBoxBaseCost.Text = client.BaseCost.ToString("F2");
            switch (client.ClientType){
                case "Regular":
                    comboBoxClientType.SelectedIndex = 0;
                    break;
                case "VIP":
                    comboBoxClientType.SelectedIndex = 1;
                    break;
                case "Corporate":
                    comboBoxClientType.SelectedIndex = 2;
                    break;
            }
            textBoxAdditionalInfo.Text = client.AdditionalInfo;
            switch (client.PricingStrategyType){
                case "FixedDiscount":
                    comboBoxPricingStrategy.SelectedIndex = 1;
                    textBoxDiscount.Text = client.DiscountValue.ToString("F2");
                    break;
                case "PercentageDiscount":
                    comboBoxPricingStrategy.SelectedIndex = 2;
                    textBoxDiscount.Text = client.DiscountValue.ToString("F2");
                    break;
                default:
                    comboBoxPricingStrategy.SelectedIndex = 0;
                    break;
            }
        }
        private void comboBoxClientType_SelectedIndexChanged(object sender, EventArgs e){
            UpdateAdditionalInfoLabel();
        }
        private void comboBoxPricingStrategy_SelectedIndexChanged(object sender, EventArgs e){
            UpdateDiscountVisibility();
        }
        private void UpdateAdditionalInfoLabel(){
            switch (comboBoxClientType.SelectedIndex)
            {
                case 0: 
                    labelAdditionalInfo.Visible = false;
                    textBoxAdditionalInfo.Visible = false;
                    textBoxAdditionalInfo.Text = "";
                    break;
                case 1: 
                    labelAdditionalInfo.Visible = true;
                    textBoxAdditionalInfo.Visible = true;
                    labelAdditionalInfo.Text = "Уровень VIP:";
                    if (string.IsNullOrEmpty(textBoxAdditionalInfo.Text))
                        textBoxAdditionalInfo.Text = "Gold";
                    break;
                case 2:
                    labelAdditionalInfo.Visible = true;
                    textBoxAdditionalInfo.Visible = true;
                    labelAdditionalInfo.Text = "Компания:";
                    if (string.IsNullOrEmpty(textBoxAdditionalInfo.Text))
                        textBoxAdditionalInfo.Text = "ООО Рога и копыта";
                    break;
            }
        }
        private void UpdateDiscountVisibility(){
            bool showDiscount = comboBoxPricingStrategy.SelectedIndex != 0;
            labelDiscount.Visible = showDiscount;
            textBoxDiscount.Visible = showDiscount;
            if (comboBoxPricingStrategy.SelectedIndex == 1)
                labelDiscount.Text = "Скидка (руб.):";
            else if (comboBoxPricingStrategy.SelectedIndex == 2)
                labelDiscount.Text = "Скидка (%):";
            if (showDiscount && string.IsNullOrEmpty(textBoxDiscount.Text))
                textBoxDiscount.Text = "0";
        }
        private void buttonOK_Click(object sender, EventArgs e){
            if (ValidateInput()){
                CreateOrUpdateClient();
                DialogResult = DialogResult.OK;
                Close();
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e){
            DialogResult = DialogResult.Cancel;
            Close();
        }
        private bool ValidateInput(){
            if (string.IsNullOrWhiteSpace(textBoxName.Text)){
                MessageBox.Show("Введите имя клиента", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxName.Focus();
                return false;
            }
            if (!double.TryParse(textBoxBaseCost.Text, out double cost) || cost < 0){
                MessageBox.Show("Введите корректную базовую стоимость (положительное число)", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxBaseCost.Focus();
                return false;
            }
            if (cost > 1000000000){
                MessageBox.Show("Стоимость услуг не может превышать 1,000,000,000 руб.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxBaseCost.Focus();
                return false;
            }
            if ((comboBoxClientType.SelectedIndex == 1 || comboBoxClientType.SelectedIndex == 2) && string.IsNullOrWhiteSpace(textBoxAdditionalInfo.Text)){
                MessageBox.Show("Заполните дополнительную информацию", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxAdditionalInfo.Focus();
                return false;
            }
            if (textBoxDiscount.Visible){
                if (!double.TryParse(textBoxDiscount.Text, out double discount)){
                    MessageBox.Show("Введите корректное значение скидки", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxDiscount.Focus();
                    return false;
                }
                if (comboBoxPricingStrategy.SelectedIndex == 1 && discount < 0){
                    MessageBox.Show("Размер скидки не может быть отрицательным", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxDiscount.Focus();
                    return false;
                }
                if (comboBoxPricingStrategy.SelectedIndex == 2 && (discount < 0 || discount > 100)){
                    MessageBox.Show("Процент скидки должен быть от 0 до 100", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBoxDiscount.Focus();
                    return false;
                }
            }
            return true;
        }
        private void CreateOrUpdateClient(){
            string clientType = "";
            switch (comboBoxClientType.SelectedIndex){
                case 0: clientType = "Regular"; break;
                case 1: clientType = "VIP"; break;
                case 2: clientType = "Corporate"; break;
            }
            string strategyType = "";
            switch (comboBoxPricingStrategy.SelectedIndex){
                case 0: strategyType = "Standard"; break;
                case 1: strategyType = "FixedDiscount"; break;
                case 2: strategyType = "PercentageDiscount"; break;
            }
            double discountValue = textBoxDiscount.Visible ? double.Parse(textBoxDiscount.Text) : 0;
            switch (clientType){
                case "VIP":
                    client = new VIPClient();
                    break;
                case "Corporate":
                    client = new CorporateClient();
                    break;
                default:
                    client = new RegularClient();
                    break;
            }
            client.Name = textBoxName.Text.Trim();
            client.ClientType = clientType;
            client.BaseCost = double.Parse(textBoxBaseCost.Text);
            client.PricingStrategyType = strategyType;
            client.DiscountValue = discountValue;
            client.AdditionalInfo = textBoxAdditionalInfo.Text.Trim();
        }
    }
}