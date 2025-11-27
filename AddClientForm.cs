using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace Лаба_4
{
    public partial class AddClientForm : Form{
        public string ClientName { get; private set; }
        public string ClientType { get; private set; }
        public double BaseCost { get; private set; }
        public string AdditionalInfo { get; private set; }
        public string PricingStrategy { get; private set; }
        public double DiscountValue { get; private set; }
        public AddClientForm(){
            InitializeComponent();
            InitializeForm();
        }
        public AddClientForm(string name, string type, double cost, string additionalInfo, string pricingStrategy){
            InitializeComponent();
            InitializeForm();
            textBoxName.Text = name;
            textBoxBaseCost.Text = cost.ToString("F2");
            textBoxAdditionalInfo.Text = additionalInfo;
            switch (type){
                case "Обычный клиент":
                    comboBoxClientType.SelectedIndex = 0;
                    break;
                case "VIP клиент":
                    comboBoxClientType.SelectedIndex = 1;
                    labelAdditionalInfo.Text = "Уровень VIP:";
                    break;
                case "Корпоративный клиент":
                    comboBoxClientType.SelectedIndex = 2;
                    labelAdditionalInfo.Text = "Компания:";
                    break;
            }
            if (pricingStrategy.Contains("Стандартная цена"))
                comboBoxPricingStrategy.SelectedIndex = 0;
            else if (pricingStrategy.Contains("Фиксированная скидка")){
                comboBoxPricingStrategy.SelectedIndex = 1;
                string discountStr = pricingStrategy.Split(':')[1].Trim().Split(' ')[0];
                textBoxDiscount.Text = discountStr;
            }
            else if (pricingStrategy.Contains("Процентная скидка")){
                comboBoxPricingStrategy.SelectedIndex = 2;
                string discountStr = pricingStrategy.Split(':')[1].Trim().Split('%')[0];
                textBoxDiscount.Text = discountStr;
            }
        }
        private void InitializeForm(){
            comboBoxClientType.SelectedIndex = 0;
            comboBoxPricingStrategy.SelectedIndex = 0;
            UpdateAdditionalInfoLabel();
            UpdateDiscountVisibility();
        }
        private void comboBoxClientType_SelectedIndexChanged(object sender, EventArgs e){
            UpdateAdditionalInfoLabel();
        }
        private void comboBoxPricingStrategy_SelectedIndexChanged(object sender, EventArgs e){
            UpdateDiscountVisibility();
        }
        private void UpdateAdditionalInfoLabel(){
            switch (comboBoxClientType.SelectedIndex){
                case 0: 
                    labelAdditionalInfo.Visible = false;
                    textBoxAdditionalInfo.Visible = false;
                    break;
                case 1: 
                    labelAdditionalInfo.Visible = true;
                    textBoxAdditionalInfo.Visible = true;
                    labelAdditionalInfo.Text = "Уровень VIP:";
                    textBoxAdditionalInfo.Text = "Gold";
                    break;
                case 2:
                    labelAdditionalInfo.Visible = true;
                    textBoxAdditionalInfo.Visible = true;
                    labelAdditionalInfo.Text = "Компания:";
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
        }
        private void buttonOK_Click(object sender, EventArgs e){
            if (ValidateInput()){
                ClientName = textBoxName.Text.Trim();
                BaseCost = double.Parse(textBoxBaseCost.Text);
                AdditionalInfo = textBoxAdditionalInfo.Text.Trim();
                DiscountValue = textBoxDiscount.Visible ? double.Parse(textBoxDiscount.Text) : 0;
                switch (comboBoxClientType.SelectedIndex){
                    case 0:
                        ClientType = "Обычный клиент";
                        break;
                    case 1:
                        ClientType = "VIP клиент";
                        break;
                    case 2:
                        ClientType = "Корпоративный клиент";
                        break;
                }
                switch (comboBoxPricingStrategy.SelectedIndex){
                    case 0:
                        PricingStrategy = "Стандартная цена";
                        break;
                    case 1:
                        PricingStrategy = "Фиксированная скидка";
                        break;
                    case 2:
                        PricingStrategy = "Процентная скидка";
                        break;
                }
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
    }
}