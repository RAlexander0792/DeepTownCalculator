using DeepTownCalculator.Domain.Entities;
using System;
using System.Collections.Generic;

namespace DeepTownCalculator.UnitTests.EntityGenerators
{
    public static class ItemGen
    {
        public static List<ItemEntity> Generate(int quantity)
        {
            var itemList = new List<ItemEntity>();
            for(int i = 0; i < quantity; i++)
            {
                itemList.Add(New());
            }
            return itemList;
        }
        public static ItemEntity New()
        {
            return new ItemEntity() { Id = Guid.NewGuid(), Name = Helpers.GenAlphStr(5, 50), Price = Helpers.GenPrice()};
        }
    }
}
