﻿namespace Pizzaria.Dtos
{
    public class PizzaCriacaoDto
    {
        public string Capa { get; set; } = string.Empty;
        public string Sabor { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public double Valor { get; set; }
    }
}
