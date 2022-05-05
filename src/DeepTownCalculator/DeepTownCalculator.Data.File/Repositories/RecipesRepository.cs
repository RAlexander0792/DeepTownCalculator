using DeepTownCalculator.Domain.Entities;
using DeepTownCalculator.Domain.Repositories;

namespace DeepTownCalculator.Data.File.Repositories
{
    public class RecipesRepository : IRecipesRepository
    {
        public IEnumerable<RecipeEntity> Get(int recipeId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<RecipeEntity> Get(IEnumerable<int> recipeId)
        {
            throw new NotImplementedException();
        }
    }
}
