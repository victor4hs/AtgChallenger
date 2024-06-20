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
    public class OrderResponseViewModel
    {
        public required string Message { get; set; }
        public required string Status { get; set; }
    }

}
