using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OrderGeneratorAPI.ViewModel
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Símbolo")]
        [EnumDataType(typeof(Symbol), ErrorMessage = "Só existem as opções PETR4, VALE3 ou VIIA4 disponíveis")]
        public required string Symbol { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Tipo da Ordem")]
        [EnumDataType(typeof(OrderType), ErrorMessage = "Só existem as opções de compra ou venda disponíveis")]
        public required string OrderType { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Quantidade")]
        public required int Quantity { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Price")]
        public required decimal Price { get; set; }
    }

    public enum OrderType
    {
        [EnumMember(Value = "SELL")]
        SELL,

        [EnumMember(Value = "BUY")]
        BUY
    }

    public enum Symbol
    {
        [EnumMember(Value = "PETR4")]
        PETR4,

        [EnumMember(Value = "VALE3")]
        VALE3,

        [EnumMember(Value = "VIIA4")]
        VIIA4
    }

}
