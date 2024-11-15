﻿
using SharedLibrary.Models;

namespace NoPersa.Tests.DatabaseMemory
{
    public class StaticCustomersLightDiets
    {
        public static List<CustomersLightDiet> GetCustomersLightDiets() =>
        [
            new() { CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!, LightDietId = 1, LightDiet = StaticLightDiets.GetLightDiets().FirstOrDefault(c => c.Id == 1)!, Selected = false},
            new() { CustomerId = 1, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 1)!, LightDietId = 2, LightDiet = StaticLightDiets.GetLightDiets().FirstOrDefault(c => c.Id == 2)!, Selected = false},
            new() { CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!, LightDietId = 1, LightDiet = StaticLightDiets.GetLightDiets().FirstOrDefault(c => c.Id == 1)!, Selected = false},
            new() { CustomerId = 2, Customer = StaticCustomers.GetCustomers().FirstOrDefault(c => c.Id == 2)!, LightDietId = 2, LightDiet = StaticLightDiets.GetLightDiets().FirstOrDefault(c => c.Id == 2)!, Selected = false},
        ];
    }
}