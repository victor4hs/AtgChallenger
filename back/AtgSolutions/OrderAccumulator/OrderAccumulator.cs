using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using QuickFix;
using QuickFix.Fields;
using System.Threading;
using Microsoft.VisualBasic;

namespace OrderAccumulator
{
    public class OrderAccumulator : MessageCracker, IApplication
    {
        int orderID = 0;
        int execID = 0;

        decimal limitDefault = 1.000m;
        decimal valueExec = 0;

        private string GenOrderID()
        {
            return (++orderID).ToString();
        }
        private string GenExecID()
        {
            return (++execID).ToString("D8");
        }

        private decimal GetLimit()
        {
            return limitDefault - valueExec;
        }

        private decimal AddValueExec(decimal value)
        {
            return value + valueExec;
        }

        public void FromApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("IN:  " + message);
            try
            {
                Crack(message, sessionID);
            }
            catch
            {
                Debug.WriteLine(message);
            }
        }

        public void ToApp(Message message, SessionID sessionID)
        {
            Console.WriteLine("OUT: " + message);
        }

        public void FromAdmin(Message message, SessionID sessionID) { }
        public void OnCreate(SessionID sessionID) { }
        public void OnLogout(SessionID sessionID) { }
        public void OnLogon(SessionID sessionID) { }
        public void ToAdmin(Message message, SessionID sessionID) { }

        public void OnMessage(QuickFix.FIX44.NewOrderSingle message, SessionID sessionId)
        {
            Price price = message.Price;

            SendExecution(sessionId, OrdStatus.NEW, ExecType.NEW, message, message.OrderQty.getValue(), 0, 0, 0, 0);
            Thread.Sleep(1000);

            if (price.getValue() >= GetLimit())
            {
                SendExecution(sessionId, OrdStatus.REJECTED, ExecType.REJECTED, message, 0, 0, price.getValue(), message.OrderQty.getValue(), price.getValue());
            }
            else
            {
                AddValueExec(price.getValue() * message.OrderQty.getValue());
                SendExecution(sessionId, OrdStatus.FILLED, ExecType.FILL, message, 0, 0, price.getValue(), message.OrderQty.getValue(), price.getValue());
            }
        }

        private void SendExecution(SessionID s, char ordStatus, char execType, QuickFix.FIX44.NewOrderSingle n, decimal leavesQty, decimal cumQty, decimal avgPx, decimal lastQty, decimal lastPrice)
        {
            QuickFix.FIX44.ExecutionReport exReport = new QuickFix.FIX44.ExecutionReport(
                new OrderID(GenOrderID()),
                new ExecID(GenExecID()),
                new ExecType(execType),
                new OrdStatus(ordStatus),
                n.Symbol,
                n.Side,
                new LeavesQty(leavesQty),
                new CumQty(cumQty),
                new AvgPx(avgPx));

            exReport.ClOrdID = new ClOrdID(n.ClOrdID.getValue());
            exReport.Set(new LastQty(lastQty));
            exReport.Set(new LastPx(lastPrice));

            try
            {
                Session.SendToTarget(exReport, s);
            }
            catch (SessionNotFound ex)
            {
                Console.WriteLine("==session not found exception!==");
                Console.WriteLine(ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public void OnMessage(QuickFix.FIX44.OrderCancelReplaceRequest msg, SessionID s)
        {
            string orderid = (msg.IsSetOrderID()) ? msg.OrderID.Obj : "unknown orderID";
            QuickFix.FIX44.OrderCancelReject ocj = new QuickFix.FIX44.OrderCancelReject(
                new OrderID(orderid), msg.ClOrdID, msg.OrigClOrdID, new OrdStatus(OrdStatus.REJECTED), new CxlRejResponseTo(CxlRejResponseTo.ORDER_CANCEL_REPLACE_REQUEST));

            ocj.CxlRejReason = new CxlRejReason(CxlRejReason.OTHER);
            ocj.Text = new Text("Sua ordem foi rejeitada!");

            try
            {
                Session.SendToTarget(ocj, s);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void OnMessage(QuickFix.FIX44.BusinessMessageReject n, SessionID s) { }
    }
}
