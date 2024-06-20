using Microsoft.AspNetCore.Mvc;
using OrderGeneratorAPI.Manager;
using OrderGeneratorAPI.ViewModel;

namespace OrderGeneratorAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderGeneratorController : ControllerBase
    {
        private readonly IOrderProcessManager _orderGeneratorManager;

        public OrderGeneratorController(IOrderProcessManager orderGeneratorManager)
        {
            _orderGeneratorManager = orderGeneratorManager;
        }

        [HttpPost]
        public async Task<ActionResult> ProcessOrder(OrderViewModel order)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    success = false,
                    data = new object(),
                    errors = ModelState.ValidationState
                });

            await _orderGeneratorManager.ProcessOrderAsync(order);

            return Ok(new
            {
                success = true,
                data = "Sua ordem está sendo processada, aguarde para novos status.",
                errors = new List<object>()
            });
        }
    }
}
