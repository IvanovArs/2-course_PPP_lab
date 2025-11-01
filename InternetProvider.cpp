#include "InternetProvider.h"
#include <iostream>
#include <algorithm>
using namespace std;
InternetProvider::InternetProvider(const wstring& name) : providerName(name) {
    if (name.empty()) {
        throw ProviderException("�������� ���������� �� ����� ���� ������");
    }
}
void InternetProvider::addClient(unique_ptr<Client> client) {
    if (!client) {
        throw ProviderException("������ �������� ������� �������");
    }
    auto it = find_if(clients.begin(), clients.end(),
        [&](const auto& existingClient) {
            return existingClient->getName() == client->getName();
        });
    if (it != clients.end()) {
        throw ClientException("������ � ����� ������ ��� ����������");
    }
    clients.push_back(move(client));
    wcout << L"������ ������� ��������!" << std::endl;
}
bool InternetProvider::removeClient(const Client& clientToRemove) {
    auto it = find_if(clients.begin(), clients.end(),
        [&](const auto& client) {
            return client->getName() == clientToRemove.getName();
        });
    if (it != clients.end()) {
        clients.erase(it);
        wcout << L"������ ������!" << endl;
        return true;
    }
    return false;
}
void InternetProvider::applyToAllClients(void (*function)(Client&)) {
    for (auto& client : clients) {
        function(*client);
    }
}
double InternetProvider::calculateTotalRevenue() const {
    double total = 0;
    for (const auto& client : clients) {
        total += client->calculateTotalCost();
    }
    return total;
}
void InternetProvider::displayAllClients() const {
    if (clients.empty()) {
        wcout << L"��� ������������������ ��������." << endl;
        return;
    }
    wcout << L"\n=== ������ �������� ���������� " << providerName << L" ===" << endl;
    for (const auto& client : clients) {
        wcout << *client << endl;
    }
}
size_t InternetProvider::getClientCount() const {
    return clients.size();
}
InternetProvider& InternetProvider::operator+=(unique_ptr<Client> client) {
    addClient(move(client));
    return *this;
}
Client* InternetProvider::operator[](const wstring& clientName) {
    auto it = find_if(clients.begin(), clients.end(),
        [&](const auto& client) {
            return client->getName() == clientName;
        });
    return (it != clients.end()) ? it->get() : nullptr;
}
const Client* InternetProvider::operator[](const wstring& clientName) const {
    auto it = find_if(clients.begin(), clients.end(),
        [&](const auto& client) {
            return client->getName() == clientName;
        });
    return (it != clients.end()) ? it->get() : nullptr;
}