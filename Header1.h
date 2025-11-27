#pragma once
#include <string>
#include <memory>
#include <vector>
#include <algorithm>
#include <stdexcept>
#include <limits>
using namespace std;
using namespace System;
class ProviderException : public runtime_error {
public:
    ProviderException(const string& message) : runtime_error(message) {}
};
class ClientException : public ProviderException {
public:
    ClientException(conststring& message) : ProviderException(message) {}
};
class IPricingStrategy {
public:
    virtual double calculateCost(double baseCost) const = 0;
    virtual wstring getDescription() const = 0;
    virtual ~IPricingStrategy() = default;
};
class StandardPricing : public IPricingStrategy {
public:
    double calculateCost(double baseCost) const override {
        if (baseCost < 0) {
            throw ProviderException("Базовая стоимость не может быть отрицательной");
        }
        return baseCost;
    }
    wstring getDescription() const override {
        return L"Стандартная цена";
    }
};
class FixedDiscountPricing : public IPricingStrategy {
private:
    double discountAmount;
public:
    FixedDiscountPricing(double discount) : discountAmount(discount) {
        if (discount < 0) {
            throw ProviderException("Размер скидки не может быть отрицательным");
        }
    }
    double calculateCost(double baseCost) const override {
        if (baseCost < 0) {
            throw ProviderException("Базовая стоимость не может быть отрицательной");
        }
        double result = baseCost - discountAmount;
        return result > 0 ? result : 0;
    }
    wstring getDescription() const override {
        return L"Фиксированная скидка: " + to_wstring(discountAmount) + L" руб.";
    }
};
class PercentageDiscountPricing : public IPricingStrategy {
private:
    double discountPercent;
public:
    PercentageDiscountPricing(double percent) : discountPercent(percent) {
        if (percent < 0 || percent > 100) {
            throw ProviderException("Процент скидки должен быть от 0 до 100");
        }
    }
    double calculateCost(double baseCost) const override {
        if (baseCost < 0) {
            throw ProviderException("Базовая стоимость не может быть отрицательной");
        }
        return baseCost * (1 - discountPercent / 100);
    }
    wstring getDescription() const override {
        return L"Процентная скидка: " + to_wstring(discountPercent) + L"%";
    }
};
class IClient {
protected:
    wstring name;
    double baseServiceCost;
    unique_ptr<IPricingStrategy> pricingStrategy;
public:
    IClient(const wstring& clientName, double cost, unique_ptr<IPricingStrategy> strategy)
        : name(clientName), baseServiceCost(cost), pricingStrategy(move(strategy)) {
        if (clientName.empty()) {
            throw ClientException("Имя клиента не может быть пустым");
        }
        if (cost < 0 || cost > numeric_limits<double>::max()) {
            throw ClientException("Базовая стоимость услуг должна быть в допустимом диапазоне");
        }
    }
    virtual double calculateTotalCost() const {
        return pricingStrategy->calculateCost(baseServiceCost);
    }
    virtual wstring getType() const = 0;
    virtual wstring getAdditionalInfo() const = 0;
    virtual wstring getDisplayInfo() const = 0;
    virtual ~IClient() = default;
    wstring getName() const { return name; }
    double getBaseCost() const { return baseServiceCost; }
    wstring getPricingStrategyDescription() const {
        return pricingStrategy->getDescription();
    }
};
class RegularClient : public IClient {
public:
    RegularClient(const wstring& name, double cost,
        unique_ptr<IPricingStrategy> strategy = make_unique<StandardPricing>())
        : IClient(name, cost, move(strategy)) {}
    wstring getType() const override {
        return L"Обычный клиент";
    }
    wstring getAdditionalInfo() const override {
        return L"Нет дополнительной информации";
    }
    wstring getDisplayInfo() const override {
        return L"Обычный клиент: " + name + L", базовая стоимость: " + to_wstring(baseServiceCost) + L", итого: " + to_wstring(calculateTotalCost());
    }
};
class VIPClient : public IClient {
private:
    wstring vipLevel;
public:
    VIPClient(const wstring& name, double cost, const wstring& level,
        unique_ptr<IPricingStrategy> strategy = make_unique<StandardPricing>())
        : IClient(name, cost, move(strategy)), vipLevel(level) {
        if (level.empty()) {
            throw ClientException("Уровень VIP не может быть пустым");
        }
    }
    wstring getType() const override {
        return L"VIP клиент";
    }
    wstring getAdditionalInfo() const override {
        return L"Уровень VIP: " + vipLevel;
    }
    wstring getDisplayInfo() const override {
        return L"VIP клиент: " + name + L" (уровень: " + vipLevel + L")" + L", базовая стоимость: " + to_wstring(baseServiceCost) + L", итого: " + to_wstring(calculateTotalCost());
    }
    wstring getVipLevel() const { return vipLevel; }
};
class CorporateClient : public IClient {
private:
    wstring companyName;
public:
    CorporateClient(const wstring& name, double cost, const wstring& company,
        unique_ptr<IPricingStrategy> strategy = make_unique<StandardPricing>())
        : IClient(name, cost, move(strategy)), companyName(company) {
        if (company.empty()) {
            throw ClientException("Название компании не может быть пустым");
        }
    }
    wstring getType() const override {
        return L"Корпоративный клиент";
    }
    wstring getAdditionalInfo() const override {
        return L"Компания: " + companyName;
    }
    wstring getDisplayInfo() const override {
        return L"Корпоративный клиент: " + name + L" (компания: " + companyName + L")" + L", базовая стоимость: " + to_wstring(baseServiceCost) + L", итого: " + to_wstring(calculateTotalCost());
    }
    wstring getCompanyName() const { return companyName; }
};
class InternetProvider {
private:
    vector<unique_ptr<IClient>> clients;
    wstring providerName;
public:
    InternetProvider(const wstring& name) : providerName(name) {
        if (name.empty()) {
            throw ProviderException("Название провайдера не может быть пустым");
        }
    }
    void addClient(unique_ptr<IClient> client) {
        if (!client) {
            throw ProviderException("Нельзя добавить пустого клиента");
        }
        auto it = find_if(clients.begin(), clients.end(),
            [&](const auto& existingClient) {
                return existingClient->getName() == client->getName();
            });
        if (it != clients.end()) {
            throw ClientException("Клиент с таким именем уже существует");
        }
        clients.push_back(move(client));
    }
    bool removeClient(const wstring& name) {
        auto it = find_if(clients.begin(), clients.end(),
            [&](const auto& client) {
                return client->getName() == name;
            });
        if (it != clients.end()) {
            clients.erase(it);
            return true;
        }
        return false;
    }
    IClient* getClient(const wstring& name) {
        auto it = find_if(clients.begin(), clients.end(),
            [&](const auto& client) {
                return client->getName() == name;
            });
        if (it != clients.end()) {
            return it->get();
        }
        return nullptr;
    }
    double calculateTotalRevenue() const {
        double total = 0;
        for (const auto& client : clients) {
            total += client->calculateTotalCost();
        }
        return total;
    }
    size_t getClientCount() const {
        return clients.size();
    }
    const vector<unique_ptr<IClient>>& getClients() const {
        return clients;
    }
    wstring getProviderName() const {
        return providerName;
    }
    void sortClientsByName() {
        sort(clients.begin(), clients.end(),
            [](const auto& a, const auto& b) {
                return a->getName() < b->getName();
            });
    }
    void sortClientsByCost() {
        sort(clients.begin(), clients.end(),
            [](const auto& a, const auto& b) {
                return a->calculateTotalCost() < b->calculateTotalCost();
            });
    }
};