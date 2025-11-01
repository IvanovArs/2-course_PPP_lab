#include "Client.h"
#include <iostream>
#include <algorithm>
using namespace std;
ClientBase::ClientBase(const wstring& clientName, double cost, unique_ptr<IPricingStrategy> strategy)
    : name(clientName), baseServiceCost(cost), pricingStrategy(move(strategy)) {
    if (clientName.empty()) {
        throw ClientException("Имя клиента не может быть пустым");
    }
    if (cost < 0 || cost > numeric_limits<double>::max()) {
        throw ClientException("Базовая стоимость услуг должна быть в допустимом диапазоне");
    }
}
Client::Client(const wstring& clientName, double cost, unique_ptr<IPricingStrategy> strategy)
    : ClientBase(clientName, cost, move(strategy)) {}
Client::Client(const Client& other)
    : ClientBase(other.name, other.baseServiceCost, make_unique<StandardPricing>()) {
}
wstring Client::getInfo() const {
    return L"Клиент: " + name + L", базовая стоимость: " + to_wstring(baseServiceCost);
}
wstring Client::getName() const {
    return name;
}
double Client::calculateTotalCost() const {
    return pricingStrategy->calculateCost(baseServiceCost);
}
double Client::getBaseServiceCost() const {
    return baseServiceCost;
}
bool Client::operator==(const Client& other) const {
    return name == other.name && baseServiceCost == other.baseServiceCost;
}
bool Client::operator!=(const Client& other) const {
    return !(*this == other);
}
bool Client::operator<(const Client& other) const {
    return calculateTotalCost() < other.calculateTotalCost();
}
Client Client::operator+(double additionalCost) const {
    Client newClient(name, baseServiceCost + additionalCost,
        make_unique<StandardPricing>());
    return newClient;
}
Client& Client::operator+=(double additionalCost) {
    baseServiceCost += additionalCost;
    return *this;
}
wostream& operator<<(wostream& os, const Client& client) {
    os << client.getInfo() << L", итого: " << client.calculateTotalCost();
    return os;
}
RegularClient::RegularClient(const wstring& name, double cost, unique_ptr<IPricingStrategy> strategy)
    : Client(name, cost, move(strategy)) {}
wstring RegularClient::getInfo() const {
    return L"Обычный клиент: " + getName() + L", базовая стоимость: " + to_wstring(baseServiceCost);
}
VIPClient::VIPClient(const wstring& name, double cost, const wstring& level, unique_ptr<IPricingStrategy> strategy)
    : Client(name, cost, move(strategy)), vipLevel(level) {
    if (level.empty()) {
        throw ClientException("Уровень VIP не может быть пустым");
    }
}
wstring VIPClient::getInfo() const {
    return L"VIP клиент: " + getName() + L" (уровень: " + vipLevel + L"), базовая стоимость: " + to_wstring(baseServiceCost);
}
CorporateClient::CorporateClient(const wstring& name, double cost, const wstring& company, unique_ptr<IPricingStrategy> strategy)
    : Client(name, cost, move(strategy)), companyName(company) {
    if (company.empty()) {
        throw ClientException("Название компании не может быть пустым");
    }
}
wstring CorporateClient::getInfo() const {
    return L"Корпоративный клиент: " + getName() + L" (компания: " + companyName + L"), базовая стоимость: " + to_wstring(baseServiceCost);
}