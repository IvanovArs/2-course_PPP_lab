using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
namespace Лаба_4
{
    public interface IPricingStrategy{
        double CalculateCost(double baseCost);
        string GetDescription();
        string GetStrategyType();
        double GetDiscountValue();
    }
    public interface IClient{
        int Id { get; set; }
        string Name { get; set; }
        string ClientType { get; set; }
        double BaseCost { get; set; }
        string PricingStrategyType { get; set; }
        double DiscountValue { get; set; }
        string AdditionalInfo { get; set; }
        double CalculateTotalCost();
        string GetPricingStrategyDescription();
        string GetDisplayInfo();
    }
    public class StandardPricing : IPricingStrategy{
        public double CalculateCost(double baseCost) => baseCost;
        public string GetDescription() => "Стандартная цена";
        public string GetStrategyType() => "Standard";
        public double GetDiscountValue() => 0;
    }
    public class FixedDiscountPricing : IPricingStrategy{
        private double discount;
        public FixedDiscountPricing(double discount) => this.discount = discount;
        public double CalculateCost(double baseCost) => Math.Max(0, baseCost - discount);
        public string GetDescription() => $"Фиксированная скидка: {discount} руб.";
        public string GetStrategyType() => "FixedDiscount";
        public double GetDiscountValue() => discount;
    }
    public class PercentageDiscountPricing : IPricingStrategy{
        private double percent;
        public PercentageDiscountPricing(double percent) => this.percent = percent;
        public double CalculateCost(double baseCost) => baseCost * (1 - percent / 100);
        public string GetDescription() => $"Процентная скидка: {percent}%";
        public string GetStrategyType() => "PercentageDiscount";
        public double GetDiscountValue() => percent;
    }
    public abstract class Client : IClient{
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientType { get; set; }
        public double BaseCost { get; set; }
        public string PricingStrategyType { get; set; }
        public double DiscountValue { get; set; }
        public string AdditionalInfo { get; set; }
        protected IPricingStrategy PricingStrategy;
        public Client(){
            Name = "";
            ClientType = "";
            PricingStrategyType = "Standard";
            AdditionalInfo = "";
            PricingStrategy = CreatePricingStrategy();
        }
        public double CalculateTotalCost(){
            return PricingStrategy?.CalculateCost(BaseCost) ?? BaseCost;
        }
        public string GetPricingStrategyDescription(){
            return PricingStrategy?.GetDescription() ?? "Стандартная цена";
        }
        public abstract string GetDisplayInfo();
        private IPricingStrategy CreatePricingStrategy(){
            switch (PricingStrategyType){
                case "FixedDiscount":
                    return new FixedDiscountPricing(DiscountValue);
                case "PercentageDiscount":
                    return new PercentageDiscountPricing(DiscountValue);
                default:
                    return new StandardPricing();
            }
        }
    }
    public class RegularClient : Client{
        public RegularClient(){
            ClientType = "Regular";
        }
        public override string GetDisplayInfo(){
            return $"Обычный клиент: {Name}, базовая стоимость: {BaseCost:F2}, итого: {CalculateTotalCost():F2}";
        }
    }
    public class VIPClient : Client{
        public VIPClient(){
            ClientType = "VIP";
        }
        public override string GetDisplayInfo(){
            return $"VIP клиент: {Name} (уровень: {AdditionalInfo}), базовая стоимость: {BaseCost:F2}, итого: {CalculateTotalCost():F2}";
        }
    }
    public class CorporateClient : Client{
        public CorporateClient(){
            ClientType = "Corporate";
        }
        public override string GetDisplayInfo(){
            return $"Корпоративный клиент: {Name} (компания: {AdditionalInfo}), базовая стоимость: {BaseCost:F2}, итого: {CalculateTotalCost():F2}";
        }
    }
}