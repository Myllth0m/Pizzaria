using Pizzaria.Dtos;
using Pizzaria.Models;

namespace Pizzaria.Services.Pizza
{
    public interface IPizzaInterface
    {
        Task<PizzaModel> CriarPizza(PizzaCriacaoDto pizzaDto, IFormFile foto);
        Task<List<PizzaModel>> BuscarPizzas();
        Task<PizzaModel> BuscarPizzaPorId(int id);
        Task<PizzaModel> EditarPizza(PizzaModel pizzaModel, IFormFile foto);
        Task RemoverPizza(int id);
    }
}
