using OrderGeneratorAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OrderGeneratorAPI.Manager
{
    public interface IOrderProcessManager
    {
        /// <summary>
        /// Process the order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        Task<bool> ProcessOrderAsync(OrderViewModel order);
    }
}
