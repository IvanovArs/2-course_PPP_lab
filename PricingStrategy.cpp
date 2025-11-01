#include "PricingStrategy.h"
using namespace std;
double StandardPricing::calculateCost(double baseCost) const {
    if (baseCost < 0) {
        throw ProviderException("������� ��������� �� ����� ���� �������������");
    }
    return baseCost;
}
FixedDiscountPricing::FixedDiscountPricing(double discount) : discountAmount(discount) {
    if (discount < 0) {
        throw ProviderException("������ ������ �� ����� ���� �������������");
    }
}
double FixedDiscountPricing::calculateCost(double baseCost) const {
    if (baseCost < 0) {
        throw ProviderException("������� ��������� �� ����� ���� �������������");
    }
    double result = baseCost - discountAmount;
    return result > 0 ? result : 0;
}
PercentageDiscountPricing::PercentageDiscountPricing(double percent) : discountPercent(percent) {
    if (percent < 0 || percent > 100) {
        throw ProviderException("������� ������ ������ ���� �� 0 �� 100");
    }
}
double PercentageDiscountPricing::calculateCost(double baseCost) const {
    if (baseCost < 0) {
        throw ProviderException("������� ��������� �� ����� ���� �������������");
    }
    return baseCost * (1 - discountPercent / 100);
}