using DeepTownCalculator.Domain.Entities;

namespace DeepTownCalculator.Domain.Repositories
{
    public interface IRecipesRepository
    {
        IEnumerable<RecipeEntity> Get(int recipeId);
        IEnumerable<RecipeEntity> Get(IEnumerable<int> recipeId);
    }
}
