#ifndef INTERFACES_H
#define INTERFACES_H
#include <string>
using namespace std;
class IPricingStrategy {
public:
    virtual double calculateCost(double baseCost) const = 0;
    virtual ~IPricingStrategy() = default;
};
class IClientInfo {
public:
    virtual wstring getInfo() const = 0;
    virtual wstring getName() const = 0;
    virtual ~IClientInfo() = default;
};
class IClientFinancial {
public:
    virtual double calculateTotalCost() const = 0;
    virtual double getBaseServiceCost() const = 0;
    virtual ~IClientFinancial() = default;
};
#endif