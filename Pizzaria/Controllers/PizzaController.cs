using Microsoft.AspNetCore.Mvc;
using Pizzaria.Dtos;
using Pizzaria.Models;
using Pizzaria.Services.Pizza;

namespace Pizzaria.Controllers
{
    public class PizzaController : Controller
    {
        private readonly IPizzaInterface pizzaInterface;

        public PizzaController(IPizzaInterface pizzaInterface) 
        {
            this.pizzaInterface = pizzaInterface;
        }

        public async Task<IActionResult> Index()
        {
            var pizzas = await pizzaInterface.BuscarPizzas();
            return View(pizzas);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Cadastrar(
            PizzaCriacaoDto pizzaDto,
            IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                await pizzaInterface.CriarPizza(pizzaDto, foto);
                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzaDto);
            }
        }

        public async Task<IActionResult> Editar(int id)
        {
            var pizza = await pizzaInterface.BuscarPizzaPorId(id);
            return View(pizza);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(PizzaModel pizzaModel, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                await pizzaInterface.EditarPizza(pizzaModel, foto);
                return RedirectToAction("Index", "Pizza");
            }
            else
            {
                return View(pizzaModel);
            }
        }

        public async Task<IActionResult> Remover(int id)
        {
            await pizzaInterface.RemoverPizza(id);
            return RedirectToAction("Index", "Pizza");
        }
    }
}
