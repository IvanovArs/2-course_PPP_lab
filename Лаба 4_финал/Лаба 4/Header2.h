#pragma once
#include "Header1.h"
using namespace std;
namespace InternetProviderApp {
    using namespace System;
    using namespace System::ComponentModel;
    using namespace System::Collections;
    using namespace System::Windows::Forms;
    using namespace System::Data;
    using namespace System::Drawing;
    public ref class AddClientForm : public Form{
    private:
        InternetProvider* provider;
        IClient* clientToEdit;
        bool isEditMode;
    public:
        AddClientForm(InternetProvider* prov, IClient* client = nullptr, bool editMode = false)
            : provider(prov), clientToEdit(client), isEditMode(editMode){
            InitializeComponent();
            InitializeForm();
        }
    protected:
        ~AddClientForm(){
            if (components){
                delete components;
            }
        }
    private:
        System::Windows::Forms::ComboBox^ comboBoxClientType;
        System::Windows::Forms::TextBox^ textBoxName;
        System::Windows::Forms::TextBox^ textBoxBaseCost;
        System::Windows::Forms::ComboBox^ comboBoxPricingStrategy;
        System::Windows::Forms::TextBox^ textBoxAdditionalInfo;
        System::Windows::Forms::Label^ labelAdditionalInfo;
        System::Windows::Forms::TextBox^ textBoxDiscount;
        System::Windows::Forms::Label^ labelDiscount;
        System::Windows::Forms::Button^ buttonOK;
        System::Windows::Forms::Button^ buttonCancel;
        System::Windows::Forms::Label^ labelTotalCost;
        System::ComponentModel::Container^ components;
        void InitializeComponent(void){
            this->comboBoxClientType = gcnew ComboBox();
            this->textBoxName = gcnew TextBox();
            this->textBoxBaseCost = gcnew TextBox();
            this->comboBoxPricingStrategy = gcnew ComboBox();
            this->textBoxAdditionalInfo = gcnew TextBox();
            this->labelAdditionalInfo = gcnew Label();
            this->textBoxDiscount = gcnew TextBox();
            this->labelDiscount = gcnew Label();
            this->buttonOK = gcnew Button();
            this->buttonCancel = gcnew Button();
            this->labelTotalCost = gcnew Label();
            this->SuspendLayout();
            this->comboBoxClientType->Location = Point(120, 20);
            this->comboBoxClientType->Size = Drawing::Size(200, 23);
            this->comboBoxClientType->DropDownStyle = ComboBoxStyle::DropDownList;
            this->comboBoxClientType->Items->AddRange(gcnew array<String^> {
                "Обычный клиент", "VIP клиент", "Корпоративный клиент"
            });
            this->comboBoxClientType->SelectedIndex = 0;
            this->comboBoxClientType->SelectedIndexChanged +=
                gcnew EventHandler(this, &AddClientForm::comboBoxClientType_SelectedIndexChanged);
            this->textBoxName->Location = Point(120, 50);
            this->textBoxName->Size = Drawing::Size(200, 23);
            this->textBoxBaseCost->Location = Point(120, 80);
            this->textBoxBaseCost->Size = Drawing::Size(200, 23);
            this->textBoxBaseCost->TextChanged +=
                gcnew EventHandler(this, &AddClientForm::textBoxBaseCost_TextChanged);
            this->comboBoxPricingStrategy->Location = Point(120, 110);
            this->comboBoxPricingStrategy->Size = Drawing::Size(200, 23);
            this->comboBoxPricingStrategy->DropDownStyle = ComboBoxStyle::DropDownList;
            this->comboBoxPricingStrategy->Items->AddRange(gcnew array<String^> {
                "Стандартная цена", "Фиксированная скидка", "Процентная скидка"
            });
            this->comboBoxPricingStrategy->SelectedIndex = 0;
            this->comboBoxPricingStrategy->SelectedIndexChanged +=
                gcnew EventHandler(this, &AddClientForm::comboBoxPricingStrategy_SelectedIndexChanged);
            this->textBoxAdditionalInfo->Location = Point(120, 140);
            this->textBoxAdditionalInfo->Size = Drawing::Size(200, 23);
            this->textBoxAdditionalInfo->Visible = false;
            this->labelAdditionalInfo->Location = Point(20, 143);
            this->labelAdditionalInfo->Size = Drawing::Size(90, 20);
            this->labelAdditionalInfo->Text = "Уровень VIP:";
            this->labelAdditionalInfo->Visible = false;
            this->textBoxDiscount->Location = Point(120, 170);
            this->textBoxDiscount->Size = Drawing::Size(200, 23);
            this->textBoxDiscount->Visible = false;
            this->labelDiscount->Location = Point(20, 173);
            this->labelDiscount->Size = Drawing::Size(90, 20);
            this->labelDiscount->Text = "Размер скидки:";
            this->labelDiscount->Visible = false;
            this->buttonOK->Location = Point(120, 220);
            this->buttonOK->Size = Drawing::Size(90, 30);
            this->buttonOK->Text = "OK";
            this->buttonOK->DialogResult = DialogResult::OK;
            this->buttonOK->Click += gcnew EventHandler(this, &AddClientForm::buttonOK_Click);
            this->buttonCancel->Location = Point(230, 220);
            this->buttonCancel->Size = Drawing::Size(90, 30);
            this->buttonCancel->Text = "Отмена";
            this->buttonCancel->DialogResult = DialogResult::Cancel;
            this->labelTotalCost->Location = Point(20, 200);
            this->labelTotalCost->Size = Drawing::Size(300, 20);
            this->labelTotalCost->Text = "Итоговая стоимость: 0.00 руб.";
            array<Label^>^ labels = gcnew array<Label^> {
                gcnew Label(), gcnew Label(), gcnew Label(), gcnew Label()
            };
            labels[0]->Location = Point(20, 23);
            labels[0]->Size = Drawing::Size(90, 20);
            labels[0]->Text = "Тип клиента:";
            labels[1]->Location = Point(20, 53);
            labels[1]->Size = Drawing::Size(90, 20);
            labels[1]->Text = "Имя:";
            labels[2]->Location = Point(20, 83);
            labels[2]->Size = Drawing::Size(90, 20);
            labels[2]->Text = "Базовая стоимость:";
            labels[3]->Location = Point(20, 113);
            labels[3]->Size = Drawing::Size(90, 20);
            labels[3]->Text = "Стратегия цены:";
            this->ClientSize = Drawing::Size(350, 270);
            this->Text = isEditMode ? "Редактирование клиента" : "Добавление клиента";
            this->FormBorderStyle = FormBorderStyle::FixedDialog;
            this->MaximizeBox = false;
            this->MinimizeBox = false;
            this->StartPosition = FormStartPosition::CenterParent;
            this->Controls->AddRange(gcnew array<Control^> {
                this->comboBoxClientType,
                    this->textBoxName,
                    this->textBoxBaseCost,
                    this->comboBoxPricingStrategy,
                    this->textBoxAdditionalInfo,
                    this->labelAdditionalInfo,
                    this->textBoxDiscount,
                    this->labelDiscount,
                    this->buttonOK,
                    this->buttonCancel,
                    this->labelTotalCost
            });
            this->Controls->AddRange(labels);
            this->ResumeLayout(false);
            this->PerformLayout();
        }
        void InitializeForm(){
            if (isEditMode && clientToEdit) {
                textBoxName->Text = gcnew String(clientToEdit->getName().c_str());
                textBoxBaseCost->Text = clientToEdit->getBaseCost().ToString("F2");
                std::wstring type = clientToEdit->getType();
                if (type == L"Обычный клиент") comboBoxClientType->SelectedIndex = 0;
                else if (type == L"VIP клиент") comboBoxClientType->SelectedIndex = 1;
                else if (type == L"Корпоративный клиент") comboBoxClientType->SelectedIndex = 2;
                UpdateTotalCost();
            }
        }
        void UpdateTotalCost(){
            try {
                double baseCost = 0;
                if (Double::TryParse(textBoxBaseCost->Text, baseCost)) {
                    labelTotalCost->Text = String::Format("Итоговая стоимость: {0:F2} руб.", baseCost);
                }
            }
            catch (Exception^) {
                labelTotalCost->Text = "Итоговая стоимость: 0.00 руб.";
            }
        }
    private:
        void comboBoxClientType_SelectedIndexChanged(Object^ sender, EventArgs^ e)
        {
            int selectedIndex = comboBoxClientType->SelectedIndex;

            textBoxAdditionalInfo->Visible = (selectedIndex != 0);
            labelAdditionalInfo->Visible = (selectedIndex != 0);

            if (selectedIndex == 1) {
                labelAdditionalInfo->Text = "Уровень VIP:";
                textBoxAdditionalInfo->Text = "Gold";
            }
            else if (selectedIndex == 2) {
                labelAdditionalInfo->Text = "Компания:";
                textBoxAdditionalInfo->Text = "ООО Рога и копыта";
            }
        }

        void comboBoxPricingStrategy_SelectedIndexChanged(Object^ sender, EventArgs^ e)
        {
            int selectedIndex = comboBoxPricingStrategy->SelectedIndex;

            textBoxDiscount->Visible = (selectedIndex != 0);
            labelDiscount->Visible = (selectedIndex != 0);

            if (selectedIndex == 1) {
                labelDiscount->Text = "Скидка (руб.):";
                textBoxDiscount->Text = "100";
            }
            else if (selectedIndex == 2) {
                labelDiscount->Text = "Скидка (%):";
                textBoxDiscount->Text = "10";
            }
        }
        void textBoxBaseCost_TextChanged(Object^ sender, EventArgs^ e){
            UpdateTotalCost();
        }
        void buttonOK_Click(Object^ sender, EventArgs^ e){
            if (!ValidateInput()) {
                return;
            }
            try {
                wstring name = msclr::interop::marshal_as<std::wstring>(textBoxName->Text);
                double baseCost = Double::Parse(textBoxBaseCost->Text);
                int clientType = comboBoxClientType->SelectedIndex;
                int pricingType = comboBoxPricingStrategy->SelectedIndex;
                unique_ptr<IPricingStrategy> strategy;
                switch (pricingType) {
                case 0:
                    strategy = make_unique<StandardPricing>();
                    break;
                case 1:
                {
                    double discount = Double::Parse(textBoxDiscount->Text);
                    strategy = make_unique<FixedDiscountPricing>(discount);
                }
                break;
                case 2:
                {
                    double discount = Double::Parse(textBoxDiscount->Text);
                    strategy =make_unique<PercentageDiscountPricing>(discount);
                }
                break;
                }
                unique_ptr<IClient> client;
                switch (clientType) {
                case 0:
                    client = make_unique<RegularClient>(name, baseCost, move(strategy));
                    break;
                case 1:
                {
                    wstring vipLevel = msclr::interop::marshal_as<wstring>(textBoxAdditionalInfo->Text);
                    client = make_unique<VIPClient>(name, baseCost, vipLevel, move(strategy));
                }
                break;
                case 2:
                {
                    wstring company = msclr::interop::marshal_as<wstring>(textBoxAdditionalInfo->Text);
                    client = make_unique<CorporateClient>(name, baseCost, company, move(strategy));
                }
                break;
                }
                if (isEditMode && clientToEdit) {
                    provider->removeClient(clientToEdit->getName());
                }
                provider->addClient(move(client));
                DialogResult = DialogResult::OK;
            }
            catch (const exception& ex) {
                MessageBox::Show(gcnew String(ex.what()), "Ошибка",
                    MessageBoxButtons::OK, MessageBoxIcon::Error);
            }
            catch (Exception^ ex) {
                MessageBox::Show("Ошибка: " + ex->Message, "Ошибка",
                    MessageBoxButtons