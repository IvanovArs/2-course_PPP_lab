#ifndef INTERNETPROVIDER_H
#define INTERNETPROVIDER_H
#include <vector>
#include <memory>
#include <string>
#include "Client.h"
using namespace std;
class InternetProvider {
private:
    vector<unique_ptr<Client>> clients;
    wstring providerName;
public:
    InternetProvider(const wstring& name);
    void addClient(unique_ptr<Client> client);
    bool removeClient(const Client& clientToRemove);
    void applyToAllClients(void (*function)(Client&));
    double calculateTotalRevenue() const;
    void displayAllClients() const;
    size_t getClientCount() const;
    InternetProvider& operator+=(unique_ptr<Client> client);
    Client* operator[](const wstring& clientName);
    const Client* operator[](const wstring& clientName) const;
};
#endif