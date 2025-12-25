#pragma once
#include "Header1.h"
#include "Header2.h"
namespace InternetProviderApp {
	using namespace System;
	using namespace System::ComponentModel;
	using namespace System::Collections;
	using namespace System::Windows::Forms;
	using namespace System::Data;
	using namespace System::Drawing;
	using namespace System::IO;
	public ref class MainForm : public System::Windows::Forms::Form
	{
	public:
		MainForm(void)
		{
			InitializeComponent();
			provider = std::make_unique<InternetProvider>(L"Мегафон");
			UpdateClientList();
			UpdateStats();
		}
	protected:
		~MainForm()
		{
			if (components)
			{
				delete components;
			}
		}
	private:
		std::unique_ptr<InternetProvider> provider;
		#pragma region Windows Form Designer generated code
		System::ComponentModel::Container^ components;
		System::Windows::Forms::DataGridView^ dataGridViewClients;
		System::Windows::Forms::Button^ buttonAddClient;
		System::Windows::Forms::Button^ buttonEditClient;
		System::Windows::Forms::Button^ buttonDeleteClient;
		System::Windows::Forms::Button^ buttonSortByName;
		System::Windows::Forms::Button^ buttonSortByCost;
		System::Windows::Forms::Button^ buttonSaveToFile;
		System::Windows::Forms::Button^ buttonLoadFromFile;
		System::Windows::Forms::Label^ labelTotalRevenue;
		System::Windows::Forms::Label^ labelClientCount;
		System::Windows::Forms::OpenFileDialog^ openFileDialog;
		System::Windows::Forms::SaveFileDialog^ saveFileDialog;
		#pragma endregion
		void InitializeComponent(void)
		{
			this->components = gcnew System::ComponentModel::Container();
			this->dataGridViewClients = (gcnew System::Windows::Forms::DataGridView());
			this->buttonAddClient = (gcnew System::Windows::Forms::Button());
			this->buttonEditClient = (gcnew System::Windows::Forms::Button());
			this->buttonDeleteClient = (gcnew System::Windows::Forms::Button());
			this->buttonSortByName = (gcnew System::Windows::Forms::Button());
			this->buttonSortByCost = (gcnew System::Windows::Forms::Button());
			this->buttonSaveToFile = (gcnew System::Windows::Forms::Button());
			this->buttonLoadFromFile = (gcnew System::Windows::Forms::Button());
			this->labelTotalRevenue = (gcnew System::Windows::Forms::Label());
			this->labelClientCount = (gcnew System::Windows::Forms::Label());
			this->openFileDialog = (gcnew System::Windows::Forms::OpenFileDialog());
			this->saveFileDialog = (gcnew System::Windows::Forms::SaveFileDialog());
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->dataGridViewClients))->BeginInit();
			this->SuspendLayout();
			this->dataGridViewClients->AllowUserToAddRows = false;
			this->dataGridViewClients->AllowUserToDeleteRows = false;
			this->dataGridViewClients->ColumnHeadersHeightSizeMode = System::Windows::Forms::DataGridViewColumnHeadersHeightSizeMode::AutoSize;
			this->dataGridViewClients->Dock = System::Windows::Forms::DockStyle::Top;
			this->dataGridViewClients->Location = System::Drawing::Point(0, 0);
			this->dataGridViewClients->Name = L"dataGridViewClients";
			this->dataGridViewClients->ReadOnly = true;
			this->dataGridViewClients->Size = System::Drawing::Size(784, 300);
			this->dataGridViewClients->TabIndex = 0;
			this->buttonAddClient->Location = System::Drawing::Point(12, 315);
			this->buttonAddClient->Name = L"buttonAddClient";
			this->buttonAddClient->Size = System::Drawing::Size(100, 30);
			this->buttonAddClient->TabIndex = 1;
			this->buttonAddClient->Text = L"Добавить";
			this->buttonAddClient->UseVisualStyleBackColor = true;
			this->buttonAddClient->Click += gcnew System::EventHandler(this, &MainForm::buttonAddClient_Click);
			this->buttonEditClient->Location = System::Drawing::Point(118, 315);
			this->buttonEditClient->Name = L"buttonEditClient";
			this->buttonEditClient->Size = System::Drawing::Size(100, 30);
			this->buttonEditClient->TabIndex = 2;
			this->buttonEditClient->Text = L"Изменить";
			this->buttonEditClient->UseVisualStyleBackColor = true;
			this->buttonEditClient->Click += gcnew System::EventHandler(this, &MainForm::buttonEditClient_Click);
			this->buttonDeleteClient->Location = System::Drawing::Point(224, 315);
			this->buttonDeleteClient->Name = L"buttonDeleteClient";
			this->buttonDeleteClient->Size = System::Drawing::Size(100, 30);
			this->buttonDeleteClient->TabIndex = 3;
			this->buttonDeleteClient->Text = L"Удалить";
			this->buttonDeleteClient->UseVisualStyleBackColor = true;
			this->buttonDeleteClient->Click += gcnew System::EventHandler(this, &MainForm::buttonDeleteClient_Click);
			this->buttonSortByName->Location = System::Drawing::Point(330, 315);
			this->buttonSortByName->Name = L"buttonSortByName";
			this->buttonSortByName->Size = System::Drawing::Size(100, 30);
			this->buttonSortByName->TabIndex = 4;
			this->buttonSortByName->Text = L"Сорт. по имени";
			this->buttonSortByName->UseVisualStyleBackColor = true;
			this->buttonSortByName->Click += gcnew System::EventHandler(this, &MainForm::buttonSortByName_Click); 
			this->buttonSortByCost->Location = System::Drawing::Point(436, 315);
			this->buttonSortByCost->Name = L"buttonSortByCost";
			this->buttonSortByCost->Size = System::Drawing::Size(100, 30);
			this->buttonSortByCost->TabIndex = 5;
			this->buttonSortByCost->Text = L"Сорт. по стоимости";
			this->buttonSortByCost->UseVisualStyleBackColor = true;
			this->buttonSortByCost->Click += gcnew System::EventHandler(this, &MainForm::buttonSortByCost_Click);
			this->buttonSaveToFile->Location = System::Drawing::Point(542, 315);
			this->buttonSaveToFile->Name = L"buttonSaveToFile";
			this->buttonSaveToFile->Size = System::Drawing::Size(100, 30);
			this->buttonSaveToFile->TabIndex = 6;
			this->buttonSaveToFile->Text = L"Сохранить";
			this->buttonSaveToFile->UseVisualStyleBackColor = true;
			this->buttonSaveToFile->Click += gcnew System::EventHandler(this, &MainForm::buttonSaveToFile_Click);
			this->buttonLoadFromFile->Location = System::Drawing::Point(648, 315);
			this->buttonLoadFromFile->Name = L"buttonLoadFromFile";
			this->buttonLoadFromFile->Size = System::Drawing::Size(100, 30);
			this->buttonLoadFromFile->TabIndex = 7;
			this->buttonLoadFromFile->Text = L"Загрузить";
			this->buttonLoadFromFile->UseVisualStyleBackColor = true;
			this->buttonLoadFromFile->Click += gcnew System::EventHandler(this, &MainForm::buttonLoadFromFile_Click); 
			this->labelTotalRevenue->AutoSize = true;
			this->labelTotalRevenue->Location = System::Drawing::Point(12, 360);
			this->labelTotalRevenue->Name = L"labelTotalRevenue";
			this->labelTotalRevenue->Size = System::Drawing::Size(143, 13);
			this->labelTotalRevenue->TabIndex = 8;
			this->labelTotalRevenue->Text = L"Общая выручка: 0 руб.";
			this->labelClientCount->AutoSize = true;
			this->labelClientCount->Location = System::Drawing::Point(12, 385);
			this->labelClientCount->Name = L"labelClientCount";
			this->labelClientCount->Size = System::Drawing::Size(128, 13);
			this->labelClientCount->TabIndex = 9;
			this->labelClientCount->Text = L"Количество клиентов: 0";
			this->openFileDialog->FileName = L"openFileDialog1";
			this->openFileDialog->Filter = L"Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*"; 
			this->saveFileDialog->Filter = L"Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*";
			this->AutoScaleDimensions = System::Drawing::SizeF(6, 13);
			this->AutoScaleMode = System::Windows::Forms::AutoScaleMode::Font;
			this->ClientSize = System::Drawing::Size(784, 411);
			this->Controls->Add(this->labelClientCount);
			this->Controls->Add(this->labelTotalRevenue);
			this->Controls->Add(this->buttonLoadFromFile);
			this->Controls->Add(this->buttonSaveToFile);
			this->Controls->Add(this->buttonSortByCost);
			this->Controls->Add(this->buttonSortByName);
			this->Controls->Add(this->buttonDeleteClient);
			this->Controls->Add(this->buttonEditClient);
			this->Controls->Add(this->buttonAddClient);
			this->Controls->Add(this->dataGridViewClients);
			this->MaximumSize = System::Drawing::Size(800, 450);
			this->MinimumSize = System::Drawing::Size(800, 450);
			this->Name = L"MainForm";
			this->Text = L"Система управления клиентами интернет-провайдера";
			(cli::safe_cast<System::ComponentModel::ISupportInitialize^>(this->dataGridViewClients))->EndInit();
			this->ResumeLayout(false);
			this->PerformLayout();
		}
#pragma endregion
	private:
		void UpdateClientList(){
			dataGridViewClients->Rows->Clear();
			dataGridViewClients->Columns->Clear();
			dataGridViewClients->Columns->Add("Name", "Имя");
			dataGridViewClients->Columns->Add("Type", "Тип");
			dataGridViewClients->Columns->Add("BaseCost", "Базовая стоимость");
			dataGridViewClients->Columns->Add("TotalCost", "Итоговая стоимость");
			dataGridViewClients->Columns->Add("Pricing", "Стратегия ценообразования");
			dataGridViewClients->Columns->Add("AdditionalInfo", "Дополнительная информация");
			dataGridViewClients->Columns["BaseCost"]->DefaultCellStyle->Format = "F2";
			dataGridViewClients->Columns["TotalCost"]->DefaultCellStyle->Format = "F2";
			for (const auto& client : provider->getClients()) {
				dataGridViewClients->Rows->Add(
					gcnew String(client->getName().c_str()),
					gcnew String(client->getType().c_str()),
					client->getBaseCost(),
					client->calculateTotalCost(),
					gcnew String(client->getPricingStrategyDescription().c_str()),
					gcnew String(client->getAdditionalInfo().c_str())
				);
			}
			UpdateStats();
		}
		void UpdateStats(){
			try {
				double revenue = provider->calculateTotalRevenue();
				int count = static_cast<int>(provider->getClientCount());
				labelTotalRevenue->Text = String::Format("Общая выручка: {0:F2} руб.", revenue);
				labelClientCount->Text = String::Format("Количество клиентов: {0}", count);
			}
			catch (const std::exception& ex) {
				MessageBox::Show(gcnew String(ex.what()), "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
		}
		void buttonAddClient_Click(System::Object^ sender, System::EventArgs^ e){
			try {
				MessageBox::Show("Функция добавления клиента будет реализована в форме AddClientForm", "Информация", MessageBoxButtons::OK, MessageBoxIcon::Information);
			}
			catch (const std::exception& ex) {
				MessageBox::Show(gcnew String(ex.what()), "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
		}
		void buttonEditClient_Click(System::Object^ sender, System::EventArgs^ e){
			if (dataGridViewClients->SelectedRows->Count == 0) {
				MessageBox::Show("Выберите клиента для редактирования", "Информация", MessageBoxButtons::OK, MessageBoxIcon::Information);
				return;
			}
			try {
				String^ clientName = safe_cast<String^>(dataGridViewClients->SelectedRows[0]->Cells["Name"]->Value);
				MessageBox::Show("Редактирование клиента: " + clientName, "Информация", MessageBoxButtons::OK, MessageBoxIcon::Information);
			}
			catch (Exception^ ex) {
				MessageBox::Show("Ошибка при выборе клиента: " + ex->Message, "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
		}
		void buttonDeleteClient_Click(System::Object^ sender, System::EventArgs^ e){
			if (dataGridViewClients->SelectedRows->Count == 0) {
				MessageBox::Show("Выберите клиента для удаления", "Информация", MessageBoxButtons::OK, MessageBoxIcon::Information);
				return;
			}
			try {
				String^ clientName = safe_cast<String^>(dataGridViewClients->SelectedRows[0]->Cells["Name"]->Value);
				if (MessageBox::Show("Вы уверены, что хотите удалить клиента " + clientName + "?", "Подтверждение удаления", MessageBoxButtons::YesNo, MessageBoxIcon::Question) == DialogResult::Yes) { 
					std::wstring name = msclr::interop::marshal_as<std::wstring>(clientName);
					if (provider->removeClient(name)) {
						UpdateClientList();
						MessageBox::Show("Клиент удален успешно", "ОК", MessageBoxButtons::OK, MessageBoxIcon::Information);
					}else {
						MessageBox::Show("Клиент не найден", "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
					}
				}
			}
			catch (const std::exception& ex) {
				MessageBox::Show(gcnew String(ex.what()), "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
			catch (Exception^ ex) {
				MessageBox::Show("Ошибка при удалении клиента: " + ex->Message, "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
		}
		void buttonSortByName_Click(System::Object^ sender, System::EventArgs^ e){
			try {
				provider->sortClientsByName();
				UpdateClientList();
				MessageBox::Show("Клиенты отсортированы по имени", "Информация", MessageBoxButtons::OK, MessageBoxIcon::Information);
			}
			catch (const std::exception& ex) {
				MessageBox::Show(gcnew String(ex.what()), "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
		}
		void buttonSortByCost_Click(System::Object^ sender, System::EventArgs^ e){
			try {
				provider->sortClientsByCost();
				UpdateClientList();
				MessageBox::Show("Клиенты отсортированы по стоимости", "Информация", MessageBoxButtons::OK, MessageBoxIcon::Information);
			}
			catch (const std::exception& ex) {
				MessageBox::Show(gcnew String(ex.what()), "Ошибка", MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
		}
		void buttonSaveToFile_Click(System::Object^ sender, System::EventArgs^ e){
			try {
				if (saveFileDialog->ShowDialog() == DialogResult::OK) {
					StreamWriter^ writer = gcnew StreamWriter(saveFileDialog->FileName);
					writer->WriteLine("Internet Provider Client Data");
					writer->WriteLine("==============================");
					writer->WriteLine("Провайдер: " + gcnew String(provider->getProviderName().c_str()));
					writer->WriteLine("");
					for (const auto& client : provider->getClients()) {
						writer->WriteLine("Имя: " + gcnew String(client->getName().c_str()));
						writer->WriteLine("Тип: " + gcnew String(client->getType().c_str()));
						writer->WriteLine("Базовая стоимость: " + client->getBaseCost().ToString("F2") + " руб.");
						writer->WriteLine("Итоговая стоимость: " + client->calculateTotalCost().ToString("F2") + " руб.");
						writer->WriteLine("Стратегия: " + gcnew String(client->getPricingStrategyDescription().c_str()));
						writer->WriteLine("Доп. информация: " + gcnew String(client->getAdditionalInfo().c_str()));
						writer->WriteLine("---");
					}
					writer->WriteLine("");
					writer->WriteLine("ОБЩАЯ СТАТИСТИКА:");
					writer->WriteLine("Общая выручка: " + provider->calculateTotalRevenue().ToString("F2") + " руб.");
					writer->WriteLine("Всего клиентов: " + provider->getClientCount().ToString());
					writer->Close();
					MessageBox::Show("Данные успешно сохранены в файл: " + saveFileDialog->FileName, "ОК", MessageBoxButtons::OK, MessageBoxIcon::Information);
				}
			}
			catch (Exception^ ex) {
				MessageBox::Show("Ошибка при сохранении файла: " + ex->Message, "Ошибка",
					MessageBoxButtons::OK, MessageBoxIcon::Error);
			}
		}
		void buttonLoadFromFile_Click(System::Object^ sender, System::EventArgs^ e){
			MessageBox::Show("Функция загрузки из файла будет реализована в следующей версии", "Информация", MessageBoxButtons::OK, MessageBoxIcon::Information);
		}
	};
}