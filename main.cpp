#include <iostream>
#include <vector>
#include <memory>
#include <string>
#include <stdexcept>
#include <algorithm>
#include <limits>
#include <cwchar>
#include <locale>
#include "Client.h"
#include "InternetProvider.h"
#include "PricingStrategy.h"
using namespace std;
void increaseServiceCost(Client& client) {
    client += 100.0;
    wcout << L"Обновленная стоимость для " << client.getName() << endl;
}
double safeInputDouble(const wstring& prompt) {
    double value;
    while (true) {
        wcout << prompt;
        cin >> value;
        if (cin.fail()) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            wcout << L"Ошибка ввода! Пожалуйста, введите число: ";
            continue;
        }
        if (value < 0) {
            wcout << L"Ошибка: стоимость не может быть отрицательной. Попробуйте снова: ";
            continue;
        }
        cin.ignore(numeric_limits<streamsize>::max(), '\n');
        break;
    }
    return value;
}
void showMenu() {
    wcout << L"\n=== Меню ===" << endl;
    wcout << L"1. Добавить клиента" << endl;
    wcout << L"2. Рассчитать общую выручку" << endl;
    wcout << L"3. Показать всех клиентов" << endl;
    wcout << L"4. Увеличить стоимость всем клиентам" << endl;
    wcout << L"5. Демонстрация перегрузки операторов" << endl;
    wcout << L"6. Выход" << endl;
    wcout << L"Выберите команду: ";
}
void addClientInteractive(InternetProvider& provider) {
    try {
        int clientType;
        wstring name, vipLevel, companyName;
        double cost;
        int strategyType;
        double discountValue;
        wcout << L"\n=== Добавление нового клиента ===" << endl;
        wcout << L"Выберите тип клиента:" << endl;
        wcout << L"1. Обычный клиент" << endl;
        wcout << L"2. VIP клиент" << endl;
        wcout << L"3. Корпоративный клиент" << endl;
        wcout << L"Ваш выбор: ";
        cin >> clientType;
        if (cin.fail()) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            throw ClientException("Ошибка ввода: ожидалось числовое значение");
        }
        cin.ignore();
        wcout << L"Введите имя клиента: ";
        getline(wcin, name);
        cost = safeInputDouble(L"Введите базовую стоимость услуг (в рублях): ");
        wcout << L"Выберите стратегию ценообразования:" << endl;
        wcout << L"1. Стандартная цена" << endl;
        wcout << L"2. Фиксированная скидка" << endl;
        wcout << L"3. Процентная скидка" << endl;
        wcout << L"Ваш выбор: ";
        cin >> strategyType;
        if (cin.fail()) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            throw ClientException("Ошибка ввода: ожидалось числовое значение");
        }
        unique_ptr<IPricingStrategy> strategy;
        switch (strategyType) {
        case 1:
            strategy = make_unique<StandardPricing>();
            break;
        case 2:
            discountValue = safeInputDouble(L"Введите размер фиксированной скидки (в рублях): ");
            strategy = make_unique<FixedDiscountPricing>(discountValue);
            break;
        case 3:
            discountValue = safeInputDouble(L"Введите размер процентной скидки (в %): ");
            strategy = make_unique<PercentageDiscountPricing>(discountValue);
            break;
        default:
            throw ClientException("Неверный выбор стратегии");
        }
        cin.ignore();
        switch (clientType) {
        case 1:
            provider += make_unique<RegularClient>(name, cost, move(strategy));
            break;
        case 2:
            wcout << L"Введите уровень VIP: ";
            getline(wcin, vipLevel);
            provider += make_unique<VIPClient>(name, cost, vipLevel, move(strategy));
            break;
        case 3:
            wcout << L"Введите название компании: ";
            getline(wcin, companyName);
            provider += make_unique<CorporateClient>(name, cost, companyName, move(strategy));
            break;
        default:
            throw ClientException("Неверный тип клиента");
        }
    }
    catch (const std::exception& e) {
        wcerr << L"Ошибка при добавлении клиента: " << e.what() << endl;
    }
}
void demonstrateOperators(InternetProvider& provider) {
    wcout << L"\n=== Демонстрация перегрузки операторов ===" << endl;
    auto client1 = make_unique<RegularClient>(L"Иван", 1000.0);
    auto client2 = make_unique<RegularClient>(L"Петр", 1500.0);
    wcout << L"Сравнение клиентов: " << endl;
    wcout << *client1 << endl;
    wcout << *client2 << endl;
    wcout << L"Клиент 1 < Клиент 2: " << (*client1 < *client2) << endl;
    auto client3 = *client1 + 500.0;
    wcout << L"После client1 + 500: " << client3 << endl;
    *client1 += 300.0;
    wcout << L"После client1 += 300: " << *client1 << endl;
}
int main() {
    setlocale(LC_ALL, "Russian");
    InternetProvider provider(L"Мегафон");
    int choice;
    wcout << L"=== Система управления клиентами интернет-провайдера ===" << endl;
    do {
        showMenu();
        cin >> choice;
        if (cin.fail()) {
            cin.clear();
            cin.ignore(numeric_limits<streamsize>::max(), '\n');
            wcout << L"Ошибка ввода! Пожалуйста, введите число от 1 до 6." << endl;
            continue;
        }
        cin.ignore();
        switch (choice) {
        case 1:
            addClientInteractive(provider);
            break;
        case 2:
            if (provider.getClientCount() == 0) {
                wcout << L"Нет клиентов для расчета выручки." << endl;
            }
            else {
                wcout << L"\n=== Расчет общей выручки ===" << endl;
                provider.displayAllClients();
                wcout << L"\nОбщая выручка провайдера: " << provider.calculateTotalRevenue()
                    << L" рублей" << endl;
                wcout << L"Общее количество клиентов: " << provider.getClientCount() << endl;
            }
            break;
        case 3:
            provider.displayAllClients();
            break;
        case 4:
            provider.applyToAllClients(increaseServiceCost);
            wcout << L"Стоимость услуг увеличена для всех клиентов!" << endl;
            break;
        case 5:
            demonstrateOperators(provider);
            break;
        case 6:
            wcout << L"Выход из программы..." << endl;
            break;
        default:
            wcout << L"Неверная команда! Пожалуйста, выберите от 1 до 6." << endl;
        }
    } while (choice != 6);
    return 0;
}