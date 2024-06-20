using OrderGeneratorAPI.ClientApp;
using OrderGeneratorAPI.ViewModel;
using QuickFix.Fields;

namespace OrderGeneratorAPI.Manager
{
    public class OrderResponseManager : IOrderResponseManager
    {
        private readonly ChatHub _chatHub;
        public OrderResponseManager(ChatHub chatHub)
        {
            _chatHub = chatHub;
        }

        /// <summary> Implemented in the interface </summary>
        public async Task ResponseOrderStatusAsync(string message, string status)
        {
            await _chatHub.SendMessage(message, status);
        }
    }
}
