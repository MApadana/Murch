using Exon.Recab.Domain.SqlServer;
using System;
using System.Collections.Generic;
using System.Linq;
using Exon.Recab.Domain.Entity;
using Exon.Recab.Infrastructure.Exception;
using System.Net;
using Exon.Recab.Domain.Constant.Transaction;
using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Service.Resource;
using Exon.Recab.Domain.Constant.Voucher;
using Exon.Recab.Service.Model.PaymentModel;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Domain.Constant.User;
using Exon.Recab.Domain.Constant.CS.Exception;
using Exon.Recab.Service.Implement.Email;
using Exon.Recab.Domain.MongoDb;

namespace Exon.Recab.Service.Implement.Payment
{
    public class PaymentService
    {
        private SdbContext _sdb;
        private MdbContext _mdb;
        //private readonly string PaymetCallUrl;
        public PaymentService()
        {

            _sdb = new SdbContext();
            _mdb = new MdbContext();
            //PaymetCallUrl = "test";
        }

        public bool InitPayment(long cpptId, long? productId, int bancType, long userId, string voucherCode)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);


            Product product = new Product();

            if (productId.HasValue)
            {
                product = _sdb.Product.FirstOrDefault(p => p.UserId == userId && p.Id == productId);
                if (product == null)
                    throw new RecabException((int)ExceptionType.ProductNotFound);
            }
            UserPackageCredit newPackageCredit = PaymentBuyPackage(cpptId: cpptId,
                                                                       bancType: bancType,
                                                                       userId: userId,
                                                                       voucherCode: voucherCode);


            if (newPackageCredit != null && productId.HasValue)
            {
                newPackageCredit.UsedQuota = newPackageCredit.UsedQuota - 1;
                newPackageCredit.Prodouct.Add(product);
                product.UserPackageCreditId = newPackageCredit.Id;
                _sdb.SaveChanges();
            }

            EmailService _emailService = new EmailService(ref _sdb, ref _mdb);

            _emailService.SendPurchaseDetail(newPackageCredit);

            return true;
        }

        public bool InitPayment(long? Amount, long userId, string voucherCode)
        {

            if (Amount.HasValue && (voucherCode != "" && voucherCode != null))
                throw new RecabException("amount and voucher code both defind", HttpStatusCode.BadRequest);

            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);


            long price = 0;

            long? voucherId = new long?();

            if (Amount.HasValue && Amount.Value > 0)
                price = Amount.Value;

            if (voucherCode != null && voucherCode != "")
            {
                Voucher voucher = _sdb.Voucher.FirstOrDefault(v => v.Code == voucherCode);

                if (voucher == null)
                    throw new RecabException("voucher  not found", HttpStatusCode.BadRequest);

                voucherId = voucher.Id;
                switch (voucher.VoucherConfig.Status)
                {
                    case VoucherStatus.درصدی:
                        throw new RecabException("not compatiable voucher", HttpStatusCode.BadRequest);


                    case VoucherStatus.مقداری:
                        price = voucher.VoucherConfig.Value;
                        break;
                }


            }


            Transaction transaction = new Transaction
            {
                UserId = userId,
                Status = BankStatus.OK,
                Amount = price,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(50),
                BankType = BankType.ملت,
                Description = "افزایش اعتبار کیف پول",
                RfId = "",
                BankResponse = "",
            };


            _sdb.Transactions.Add(transaction);
            _sdb.SaveChanges();


            Credit creditADD = new Credit
            {
                TransactionId = transaction.Id,
                UserId = user.Id,
                Amount = transaction.Amount,
                Description = "افزایش اعتبار کیف پول",
                InsertTime = DateTime.UtcNow,
                VoucherId = voucherId

            };

            _sdb.Credits.Add(creditADD);
            _sdb.SaveChanges();

            return true;

        }

        private UserPackageCredit PaymentBuyPackage(long cpptId, int bancType, long userId, string voucherCode)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            CategoryPurchasePackageType cppt = _sdb.CategoryPurchasePackageTypes.Find(cpptId);

            if (cppt == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFound);

            PurchaseConfig amountconfig = cppt.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == PackageConfig.Price);

            if (amountconfig == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFoundMissdItemForConfig);

            PurchaseConfig quotaConfig = cppt.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == PackageConfig.Quota);

            if (quotaConfig == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFoundMissdItemForConfig);

            PurchaseConfig expiryDate = cppt.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == PackageConfig.ExpiryDate);

            if (expiryDate == null)
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFoundMissdItemForConfig);

            long PaymentAmount = 0;

            if (!long.TryParse(amountconfig.Value, out PaymentAmount))
                throw new RecabException((int)ExceptionType.CategoryPurchasePackageTypeNotFoundMissdItemForConfig);


            long? voucherId = new long?();
            if (voucherCode != null && voucherCode != "")
            {
                Voucher voucher = _sdb.Voucher.FirstOrDefault(v => v.Code == voucherCode);

                if (voucher == null)
                    throw new RecabException((int)ExceptionType.VoucherInvalid);

                switch (voucher.VoucherConfig.Status)
                {
                    case VoucherStatus.درصدی:
                        PaymentAmount = PaymentAmount - ((PaymentAmount * voucher.VoucherConfig.Value) / 100);
                        break;

                    case VoucherStatus.مقداری:
                        PaymentAmount = PaymentAmount - voucher.VoucherConfig.Value;
                        break;

                }

                voucherId = voucher.Id;
            }


            if ((BankType)bancType == BankType.کیف_پول)
            {
                long count = _sdb.Credits.Where(c => c.UserId == user.Id).Sum(c => c.Amount);


                if (count < PaymentAmount)
                    throw new RecabException((int)ExceptionType.CreditNotEnough);
            }


            int quota = 0;
            UserPackageCredit UserPackageCredit = new UserPackageCredit
            {
                UserId = userId,
                ExpireDate = DateTime.UtcNow.AddDays(30),
                InsertTime = DateTime.UtcNow,
                CategoryPurchasePackageTypeId = cppt.Id,
                BaseQuota = int.TryParse(quotaConfig.Value, out quota) ? quota : 1,
                Status = UserCreditStatus.فعال
            };

            UserPackageCredit.UsedQuota = UserPackageCredit.BaseQuota;

            _sdb.UserPackageCredits.Add(UserPackageCredit);

            Transaction transaction = new Transaction
            {
                UserId = userId,
                Status = BankStatus.OK,
                Amount = PaymentAmount,
                StartTime = DateTime.UtcNow,
                EndTime = DateTime.UtcNow.AddDays(50),
                BankType = BankType.ملت,
                Description = "پرداخت بسته برای کاربر",
                RfId = "",
                BankResponse = "",
            };


            _sdb.Transactions.Add(transaction);

            _sdb.SaveChanges();

            if ((BankType)bancType != BankType.کیف_پول)
            {
                Credit creditADD = new Credit
                {
                    TransactionId = transaction.Id,
                    UserId = user.Id,
                    Amount = transaction.Amount,
                    Description = "افزایش اعتبار کاربر توسط بانک",
                    InsertTime = DateTime.UtcNow,

                };
                _sdb.Credits.Add(creditADD);
                _sdb.SaveChanges();

                Credit creditPay = new Credit
                {
                    UserPackageCreditId = UserPackageCredit.Id,
                    UserId = user.Id,
                    VoucherId = voucherId,
                    Amount = -transaction.Amount,
                    Description = "خرید بسته",
                    InsertTime = DateTime.UtcNow,
                    ParentCreditId = creditADD.Id

                };
                _sdb.Credits.Add(creditPay);
                _sdb.SaveChanges();
            }

            else
            {
                Credit creditPay = new Credit
                {
                    UserPackageCreditId = UserPackageCredit.Id,
                    UserId = user.Id,
                    VoucherId = voucherId,
                    Amount = -transaction.Amount,
                    Description = " خرید بسته برای کاربر از طریق کیف پول",
                    InsertTime = DateTime.UtcNow

                };
                _sdb.Credits.Add(creditPay);
                _sdb.SaveChanges();

            }

            EmailService _emailService = new EmailService(ref _sdb, ref _mdb);

            _emailService.SendPurchaseDetail(UserPackageCredit);

            return UserPackageCredit;
        }

        public UserCreditDetailViewModel UserCreditDetail(string toPersianDate,
                                                          string fromPersianDate, 
                                                          long userId, 
                                                          ref long count, 
                                                          int size = 1,
                                                          int skip = 0)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);
            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            List<Credit> credits = _sdb.Credits.Where(c => c.UserId == userId).ToList();

            UserCreditDetailViewModel model = new UserCreditDetailViewModel
            {
                totalAmount = credits.Sum(c => c.Amount).ToStringRial()
            };

            if (fromPersianDate != "" && fromPersianDate != null)
            {
                DateTime from = fromPersianDate.PersianToGregorianUTC();
                credits = credits.Where(c => c.InsertTime > from).ToList();
            }

            if (toPersianDate != "" && toPersianDate != null)
            {
                DateTime to = toPersianDate.PersianToGregorianUTC();
                credits = credits.Where(c => c.InsertTime < to).ToList();
            }


           

            credits = credits.Where(c => c.Amount < 0 || (c.Amount >= 0 && !credits.Any(cr => cr.ParentCreditId == c.Id))).ToList();

            foreach (var item in credits)
            {

                if (item.ParentCreditId.HasValue)
                {

                    Voucher voucher = !item.VoucherId.HasValue ? new Voucher() : _sdb.Voucher.Find(item.VoucherId.Value);

                    model.items.Add(new CreditDetailItemViewModel
                    {
                        id = item.Id,
                        date = item.InsertTime.UTCToPersianDateLong(),
                        amount = (-item.Amount).ToStringRial(),
                        description = item.Description,
                        bank = item.ParentCredit.Transaction.BankType.ToString(),
                        voucher = item.VoucherId.HasValue ? voucher.Code : ""
                    });
                }

                else
                {
                    if (item.Amount >= 0)
                    {
                        Voucher voucher = !item.VoucherId.HasValue ? new Voucher() : _sdb.Voucher.Find(item.VoucherId.Value);

                        model.items.Add(new CreditDetailItemViewModel
                        {
                            id = item.Id,
                            date = item.InsertTime.UTCToPersianDateLong(),
                            amount = (item.Amount).ToStringRial(),
                            description = item.Description,
                            bank = item.TransactionId.HasValue ? item.Transaction.BankType.ToString() : "",
                            voucher = item.VoucherId.HasValue ? voucher.VoucherConfig.Title : ""
                        });

                    }

                }

            }



            count = model.items.Count();
            model.items = model.items.OrderBy(c => c.id).Skip(size * skip).Take(size).ToList();


            return model;
        }


    }
}
