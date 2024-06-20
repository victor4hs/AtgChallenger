using Microsoft.AspNet.SignalR.Messaging;
using Microsoft.AspNetCore.Http;
using OrderGeneratorAPI.Manager;
using OrderGeneratorAPI.ViewModel;
using QuickFix;
using QuickFix.Fields;
using QuickFix.FIX44;
using QuickFix.Transport;

namespace OrderGeneratorAPI.ClientApp
{
    public class OrderGeneratorClientApp : MessageCracker, IApplication
    {
        Session _session = null;
        SocketInitiator _socketInitiator = null;
        private readonly IOrderResponseManager _orderResponseManager;
        public OrderGeneratorClientApp(IOrderResponseManager orderResponseManager)
        {
            _orderResponseManager = orderResponseManager;
        }

        public void ApplicationStop()
        {
            _socketInitiator.Stop();
        }

        public void OnCreate(SessionID sessionID)
        {
            _session = Session.LookupSession(sessionID);
        }

        public void OnLogon(SessionID sessionID) { Console.WriteLine("Logon - " + sessionID.ToString()); }
        public void OnLogout(SessionID sessionID) { Console.WriteLine("Logout - " + sessionID.ToString()); }

        public void FromAdmin(QuickFix.Message message, SessionID sessionID) { }
        public void ToAdmin(QuickFix.Message message, SessionID sessionID) { }

        public void FromApp(QuickFix.Message message, SessionID sessionID)
        {
            Console.WriteLine("IN:  " + message.ToString());
            try
            {
                Crack(message, sessionID);
            }
            catch (Exception ex)
            {
                Console.WriteLine("==Cracker exception==");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void ToApp(QuickFix.Message message, SessionID sessionID){ }

        public void OnMessage(ExecutionReport messageReport, SessionID sessionId)
        {
            switch (messageReport.OrdStatus.getValue())
            {
                case OrdStatus.NEW:
                    _orderResponseManager.ResponseOrderStatusAsync("Ordem está sendo processada!", "success");
                    break;
                case OrdStatus.FILLED:
                    _orderResponseManager.ResponseOrderStatusAsync("Ordem processada com sucesso!", "success");
                    break;
            }
        }

        public void OnMessage(OrderCancelReject messageOrderReject, SessionID sessionId)
        {
            _orderResponseManager.ResponseOrderStatusAsync(messageOrderReject.Text.toStringField(), "error");
        }

        public void SendMessage(NewOrderSingle m)
        {
            if (_session != null)
                _session.Send(m);
            else
            {
                Console.WriteLine("Can't send message: session not created.");
            }
        }

        public NewOrderSingle NewOrderSingle(QuickFix.Fields.Symbol symbol, Side side, OrderQty orderQty, Price price)
        {
            var newOrderSingle = new NewOrderSingle();

            newOrderSingle.ClOrdID = new ClOrdID();
            newOrderSingle.Symbol = symbol;
            newOrderSingle.Side = side;
            newOrderSingle.TransactTime = new TransactTime(DateTime.Now);
            newOrderSingle.OrdType = new OrdType(OrdType.LIMIT);

            newOrderSingle.Set(new HandlInst('1'));
            newOrderSingle.Set(orderQty);
            newOrderSingle.Set(price);

            return newOrderSingle;
        }
    }
}
