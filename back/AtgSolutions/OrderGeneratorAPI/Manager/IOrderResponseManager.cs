using OrderGeneratorAPI.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace OrderGeneratorAPI.Manager
{
    public interface IOrderResponseManager
    {
        /// <summary>
        /// Process the order
        /// </summary>
        /// <param name="status"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task ResponseOrderStatusAsync(string status, string message);
    }
}
