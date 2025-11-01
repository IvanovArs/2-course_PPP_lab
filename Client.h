#ifndef CLIENT_H
#define CLIENT_H
#include <memory>
#include <string>
#include "Interfaces.h"
#include "Exceptions.h"
#include "PricingStrategy.h"
using namespace std;
class ClientBase {
protected:
    wstring name;
    double baseServiceCost;
    unique_ptr<IPricingStrategy> pricingStrategy;
public:
    ClientBase(const std::wstring& clientName, double cost,
        unique_ptr<IPricingStrategy> strategy);
    virtual ~ClientBase() = default;
};
class Client : public ClientBase, public IClientInfo, public IClientFinancial {
public:
    Client(const wstring& clientName, double cost, unique_ptr<IPricingStrategy> strategy = make_unique<StandardPricing>());
    Client(const Client& other);
    wstring getInfo() const override;
    wstring getName() const override;
    double calculateTotalCost() const override;
    double getBaseServiceCost() const override;
    bool operator==(const Client& other) const;
    bool operator!=(const Client& other) const;
    bool operator<(const Client& other) const;
    Client operator+(double additionalCost) const;
    Client& operator+=(double additionalCost);
    friend wostream& operator<<(wostream& os, const Client& client);
};
class RegularClient : public Client {
public:
    RegularClient(const wstring& name, double cost,
        unique_ptr<IPricingStrategy> strategy = make_unique<StandardPricing>());
    wstring getInfo() const override;
};
class VIPClient : public Client {
private:
    wstring vipLevel;
public:
    VIPClient(const wstring& name, double cost, const wstring& level,
        unique_ptr<IPricingStrategy> strategy = make_unique<StandardPricing>());
    wstring getInfo() const override;
};
class CorporateClient : public Client {
private:
    wstring companyName;
public:
    CorporateClient(const wstring& name, double cost, const wstring& company,
        unique_ptr<IPricingStrategy> strategy = make_unique<StandardPricing>());
    wstring getInfo() const override;
};
#endif