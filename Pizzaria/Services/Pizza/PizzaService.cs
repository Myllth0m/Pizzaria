using Microsoft.EntityFrameworkCore;
using Pizzaria.Data;
using Pizzaria.Dtos;
using Pizzaria.Models;

namespace Pizzaria.Services.Pizza
{
    public class PizzaService : IPizzaInterface
    {
        private readonly AppDbContext context;
        private readonly string sistema;

        public PizzaService(
            AppDbContext context,
            IWebHostEnvironment sistema) 
        {
            this.context = context;
            this.sistema = sistema.WebRootPath;
        }

        public string GerarCaminhoDoArquivo(IFormFile foto)
        {
            var codigoUnico = Guid.NewGuid().ToString();
            var caminhoDaImagem = foto.FileName.Replace(" ", "").ToLower() + codigoUnico + ".png";
            var caminhoParaSalvarAImagem = sistema + "\\assets\\";

            if (!Directory.Exists(caminhoParaSalvarAImagem))
            {
                Directory.CreateDirectory(caminhoParaSalvarAImagem);
            }

            using (var stream = File.Create(caminhoParaSalvarAImagem + caminhoDaImagem))
            {
                foto.CopyToAsync(stream).Wait();
            }

            return caminhoDaImagem;
        }

        public async Task<PizzaModel> CriarPizza(PizzaCriacaoDto pizzaDto, IFormFile foto)
        {
            try
            {
                var caminhoDaImagem = GerarCaminhoDoArquivo(foto);
                var pizza = new PizzaModel
                {
                    Sabor = pizzaDto.Sabor,
                    Descricao = pizzaDto.Descricao,
                    Valor = pizzaDto.Valor,
                    Capa = caminhoDaImagem,
                };

                context.Add(pizza);
                await context.SaveChangesAsync();

                return pizza;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<PizzaModel>> BuscarPizzas()
        {
            try
            {
                return await context.Pizzas.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> BuscarPizzaPorId(int id)
        {
            try
            {
                return await context.Pizzas.FirstAsync(pizza => pizza.Id == id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PizzaModel> EditarPizza(PizzaModel pizzaModel, IFormFile foto)
        {
            try
            {
                var pizza = await context.Pizzas.AsNoTracking().FirstAsync(pizza => pizza.Id == pizzaModel.Id);
                var caminhoDaImagem = string.Empty;

                if (foto != null)
                {
                    string capaExistente = sistema + "\\assets\\" + pizza.Capa;

                    if (File.Exists(capaExistente))
                    {
                        File.Delete(capaExistente);
                    }

                    caminhoDaImagem = GerarCaminhoDoArquivo(foto);
                }

                pizza.Sabor = pizzaModel.Sabor;
                pizza.Descricao = pizzaModel.Descricao;
                pizza.Valor = pizzaModel.Valor;

                if (caminhoDaImagem != string.Empty)
                {
                    pizza.Capa = caminhoDaImagem;
                }
                else
                {
                    pizza.Capa = pizza.Capa;
                }

                context.Update(pizza);
                await context.SaveChangesAsync();

                return pizzaModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task RemoverPizza(int id)
        {
            try
            {
                var pizza = await context.Pizzas.FirstAsync(pizza => pizza.Id == id);

                context.Remove(pizza);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
