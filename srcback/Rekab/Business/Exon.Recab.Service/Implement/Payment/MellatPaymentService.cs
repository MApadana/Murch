using System;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Domain.SqlServer;
using log4net;
using Exon.Recab.Service.Model.PaymentModel;
using Exon.Recab.Service.ir.shaparak.bpm;
using Exon.Recab.Domain.Constant.Transaction;
using Exon.Recab.Service.Constant;

namespace Exon.Recab.Service.Implement.Payment
{
    public class MellatPaymentService
    {
        readonly long TerminalId;
        readonly string UserName;
        readonly string Password;
        readonly string CallBackUrl;
        readonly string UrlCall;
        readonly SdbContext _sdb;
        readonly static ILog Log = LogManager.GetLogger(typeof(MellatPaymentService));
        public MellatPaymentService(string terminalId, string username, string password, string callbackurl, string urlcall)
        {
            this.TerminalId = Convert.ToInt64(terminalId);
            this.UserName = username;
            this.Password = password;
            this.CallBackUrl = callbackurl;
            this.UrlCall = urlcall;
            _sdb = new SdbContext();

        }

        public GatewayInitializeModel Initialze(long amount, long userId)
        {
            string payDate = DateTime.Now.Year.ToString() +
                             DateTime.Now.Month.ToString().PadLeft(2, '0') +
                             DateTime.Now.Day.ToString().PadLeft(2, '0');

            string PayTime = DateTime.Now.Hour.ToString().PadLeft(2, '0') +
                             DateTime.Now.Minute.ToString().PadLeft(2, '0') +
                             DateTime.Now.Second.ToString().PadLeft(2, '0');

            Transaction transaction = new Transaction
            {
                Amount = amount,
                Status = BankStatus.Init,
                UserId = userId,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddHours(24),
                BankResponse = "",
                RfId = "",
                BankType = BankType.ملت,
                Description = "پرداخت با درگاه بانک ملت خرید"

            };

            _sdb.Transactions.Add(transaction);

            _sdb.SaveChanges();

            Log.Info(string.Format("Initate add credit to user {0} , amount {1} , date {2}",
                                    userId.ToString(),
                                    amount,
                                    DateTime.UtcNow.ToLongDateString()
                                    ));

            PaymentGatewayImplService bpService = new PaymentGatewayImplService();

            string result = bpService.bpPayRequest(
                                                terminalId: TerminalId,
                                                userName: this.UserName,
                                                userPassword: Password,
                                                orderId: transaction.Id,
                                                amount: amount,
                                                localDate: payDate,
                                                localTime: PayTime,
                                                additionalData: " ",
                                                callBackUrl: CallBackUrl,
                                                payerId: (long)0);


            Log.Info(result);

            String[] resultArray = result.Split(',');
            MellatPaymentResult resCode = (MellatPaymentResult)int.Parse(resultArray[0]);

            GatewayInitializeModel model = new GatewayInitializeModel();

            if (resCode == MellatPaymentResult.TransactionApproved)
            {
                transaction.RfId = resultArray[1];
                transaction.Status = BankStatus.WaitForPayment;

                Log.Info(string.Format("call banck rfId # {0} , userId #{1}", resultArray[1], userId.ToString()));

                model.rfid = resultArray[1];
                model.bank = (int)BankType.ملت;
                model.returnUrl = CallBackUrl;
                model.urlCall = this.UrlCall;
                model.message = "در حال اتصال به درگاه پرداخت بانک ملت";
                model.success = true;
            }

            else
            {
                _sdb.Transactions.Remove(transaction);

                model.success = false;
                model.message = "خطا در برقراری ارتباط با سرور بانک ملت";

            }

            _sdb.SaveChanges();

            return model;

        }

        public long ConfirmPayment(string Rfid, long OrderId, long bankResponseCode)
        {

            Transaction transaction = _sdb.Transactions.FirstOrDefault(t => t.BankResponse == Rfid);

            if (transaction != null)
            {
                transaction.BankResponse = bankResponseCode.ToString();

                PaymentGatewayImplService bpService = new PaymentGatewayImplService();

                string result = bpService.bpVerifyRequest(
                                                        terminalId: TerminalId,
                                                        userName: UserName,
                                                        userPassword: Password,
                                                        orderId: transaction.Id,
                                                        saleOrderId: OrderId,
                                                        saleReferenceId: bankResponseCode
                                                        );
                Log.Info("result for bpVerifyRequest #" + result + transaction.Description);

                int resultstatus = 999;

                int.TryParse(result, out resultstatus);

                MellatPaymentResult verifyRequest = (MellatPaymentResult)resultstatus;

                if (verifyRequest != MellatPaymentResult.TransactionApproved)
                {
                    transaction.Description = result;
                    transaction.Status = BankStatus.Error;
                    return 0;

                }

                result = bpService.bpSettleRequest(terminalId: TerminalId,
                                                    userName: UserName,
                                                    userPassword: Password,
                                                    orderId: transaction.Id,
                                                    saleOrderId: OrderId,
                                                    saleReferenceId: bankResponseCode);

                Log.Info("result for SattlRequest #" + result + transaction.Description);

                int.TryParse(result, out resultstatus);
                MellatPaymentResult SattlRequest = (MellatPaymentResult)resultstatus;

                if (SattlRequest != MellatPaymentResult.TransactionApproved)
                {
                    transaction.Description = result;
                    transaction.Status = BankStatus.Error;
                    return 0;
                }

                transaction.Status = BankStatus.OK;
              

                _sdb.SaveChanges();

                return transaction.Id;

            }

            Log.Error(string.Format("RFID #{} , ORDERID #{1} ,BANKRESPONCECODE #{2}",Rfid , OrderId ,bankResponseCode));

            return 0;

        }
    }
}
