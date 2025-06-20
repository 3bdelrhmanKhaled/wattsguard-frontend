using System;
using System.Collections.Generic;
using System.Linq;

namespace Grad_Project.Services
{
    public class ElectricityBillCalculator
    {
        private static readonly List<(double Limit, double Price)> Tariffs = new List<(double, double)>
        {
            (50, 68),
            (100, 78),
            (200, 95),
            (350, 155),
            (650, 195),
            (1000, 210),
            (double.PositiveInfinity, 230)
        };

        public static double CalculateConsumption(double amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Amount cannot be negative.", nameof(amount));
            }

            double remainingAmount = amount * 100;
            double totalConsumption = 0;
            double previousLimit = 0;

            foreach (var (limit, price) in Tariffs)
            {
                if (remainingAmount <= 0)
                {
                    break;
                }

                double slabCapacity = limit - previousLimit;
                double costOfSlab = slabCapacity * price;

                if (remainingAmount >= costOfSlab)
                {
                    totalConsumption += slabCapacity;
                    remainingAmount -= costOfSlab;
                }
                else
                {
                    totalConsumption += remainingAmount / price;
                    remainingAmount = 0;
                }

                previousLimit = limit;
            }

            if (previousLimit == 1000 && remainingAmount > 0)
            {
                double excessConsumption = remainingAmount / 230;
                totalConsumption += excessConsumption;
            }

            return Math.Round(totalConsumption, 2);
        }

        public static string FormatHours(double hoursDecimal)
        {
            int hours = (int)hoursDecimal;
            int minutes = (int)Math.Round((hoursDecimal - hours) * 60);
            return $"{hours}h {minutes}m";
        }
    }
}