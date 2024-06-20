using OrderGeneratorAPI.ClientApp;
using OrderGeneratorAPI.ViewModel;
using QuickFix.Fields;

namespace OrderGeneratorAPI.Manager
{
    public class OrderProcessManager : IOrderProcessManager
    {
        private readonly OrderGeneratorClientApp _orderGeneratorClientApp;
        public OrderProcessManager(OrderGeneratorClientApp orderGeneratorClientApp)
        {
            _orderGeneratorClientApp = orderGeneratorClientApp;
        }

        /// <summary> Implemented in the interface </summary>
        public async Task<bool> ProcessOrderAsync(OrderViewModel order)
        {
            var symbol = new QuickFix.Fields.Symbol(order.Symbol);
            var orderType = new Side(order.OrderType.Equals(OrderType.BUY.ToString()) ? Side.BUY : Side.SELL);
            var orderQtd = new OrderQty(order.Quantity);
            var price = new Price(order.Price);

            var message = _orderGeneratorClientApp.NewOrderSingle(symbol, orderType, orderQtd, price);
            _orderGeneratorClientApp.SendMessage(message);
            
            return await Task.FromResult(true);
        }

    }
}
