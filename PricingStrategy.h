#ifndef PRICINGSTRATEGY_H
#define PRICINGSTRATEGY_H
#include "Interfaces.h"
#include "Exceptions.h"
using namespace std;
class StandardPricing : public IPricingStrategy {
public:
    double calculateCost(double baseCost) const override;
};
class FixedDiscountPricing : public IPricingStrategy {
private:
    double discountAmount;
public:
    FixedDiscountPricing(double discount);
    double calculateCost(double baseCost) const override;
};
class PercentageDiscountPricing : public IPricingStrategy {
private:
    double discountPercent;
public:
    PercentageDiscountPricing(double percent);
    double calculateCost(double baseCost) const override;
};
#endif