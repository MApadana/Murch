using Exon.Recab.Domain.Entity.PackageModule;
using Exon.Recab.Domain.SqlServer;
using Exon.Recab.Infrastructure.Exception;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Exon.Recab.Infrastructure.Utility.Extension;
using Exon.Recab.Domain.Constant.Voucher;
using Exon.Recab.Infrastructure.Utility.Security;
using System.IO;
using Exon.Recab.Service.Resource;
using Exon.Recab.Service.Model.PackageModel;
using Exon.Recab.Domain.Constant.CS.Exception;

namespace Exon.Recab.Service.Implement.Package
{
    public class VoucherService
    {
        private readonly SdbContext _sdb;

        public VoucherService()
        {
            _sdb = new SdbContext();
        }

        public bool AddVoucherConfigAndVocher(long userId,
                                              string title,
                                              string description,
                                              string fromTime,
                                              string fromDate,
                                              string toTime,
                                              string toDate,
                                              long value,
                                              long count,
                                              int type)
        {
            Domain.Entity.User user = _sdb.Users.Find(userId);

            if (user == null)
                throw new RecabException((int)ExceptionType.UserNotFound);

            VoucherConfig voucherConfig = new VoucherConfig
            {
                UserId = user.Id,
                Title = title ?? "",
                Description = description ?? "",
                FromDate = fromDate.PersianToGregorianWithDate(fromTime),
                ToDate = toDate.PersianToGregorianWithDate(toTime),
                Count = count,
                Status = (VoucherStatus)type,
                CreateStatus = VoucherCreationStatus.درحال_تولید,
                Value = value,
                CreatDate = DateTime.UtcNow
            };


            _sdb.VoucherConfig.Add(voucherConfig);

                _sdb.SaveChanges();
     
            for (int i = 0; i < voucherConfig.Count; i++)
            {

                voucherConfig.Vouchers.Add(new Voucher
                {
                    Code = CodeHelper.NewVoucher(),
                    FromDate = voucherConfig.FromDate,
                    ToDate = voucherConfig.ToDate,
                    ResponseCode = CodeHelper.NewKey()
                });

            }

            voucherConfig.CreateStatus = VoucherCreationStatus.باموفقییت;
            _sdb.SaveChanges();


            return true;


        }


        public List<VoucherConfigViewModel> ListVocherConfig(int? status,
                                                            string fromDate,
                                                            string toDate,
                                                            ref long count,
                                                            int size = 1,
                                                            int skip = 0)
        {

            List<VoucherConfig> VoucherConfig = new List<VoucherConfig>();
            if (status.HasValue)
            {
                VoucherConfig = _sdb.VoucherConfig.Where(vc => vc.CreateStatus == (VoucherCreationStatus)status.Value).ToList();
            }
            else
            {
                VoucherConfig = _sdb.VoucherConfig.ToList();
            }


            if (fromDate != null && fromDate != "")
            {
                DateTime time = fromDate.PersianToGregorian();

                VoucherConfig = VoucherConfig.Where(vc => vc.FromDate > time).ToList();
            }

            if (fromDate != null && toDate != "")
            {
                DateTime time = toDate.PersianToGregorian();

                VoucherConfig = VoucherConfig.Where(vc => vc.FromDate < time).ToList();
            }

            count = VoucherConfig.Count;


            return count == 0 ? new List<VoucherConfigViewModel>() :
                    VoucherConfig.Select(vc => new VoucherConfigViewModel
                    {
                        voucherConfigId = vc.Id,
                        count = vc.Count,
                        creatDate = vc.CreatDate.UTCToPersianDateLong(),
                        fromDate = vc.FromDate.LocalToPersianDateLong(),
                        toDate = vc.ToDate.LocalToPersianDateLong(),
                        userCreator = vc.User.FirstName + " " + vc.User.LastName,
                        type = vc.Status.ToString(),
                        title = vc.Title,
                        description = vc.Description,
                        value = vc.Value.ToString("##,###")

                    }).OrderBy(c => c.voucherConfigId).Skip(size * skip).Take(size).ToList();


        }       

        public List<VoucherViewModel> ListVoucher(long voucherConfigId, string responseCode, ref long count, int size = 1, int skip = 0)
        {
            VoucherConfig voucherConfig = _sdb.VoucherConfig.Find(voucherConfigId);

            if (voucherConfig == null)
                throw new RecabException((int)ExceptionType.VocherConfigNotFound);

            if (responseCode != "" && responseCode != null)
            {

                count = voucherConfig.Vouchers.Count;

                return voucherConfig.Vouchers.OrderBy(v => v.Id).Skip(size * skip).Take(size).Select(v => new VoucherViewModel
                {
                    voucherCode = v.Code,
                    voucherResponceCode = v.ResponseCode,
                    userName = v.CreditId.HasValue ? v.Credit.User.FirstName + " " + v.Credit.User.LastName : ""
                }).ToList();



            }
            else {
                count = voucherConfig.Vouchers.Where(v => v.ResponseCode.Contains(responseCode)).Count();

                return voucherConfig.Vouchers.Where(v => v.ResponseCode.Contains(responseCode))
                                             .OrderBy(v => v.Id).Skip(size * skip).Take(size)
                                             .Select(v => new VoucherViewModel
                                             {
                                                 voucherCode = v.Code,
                                                 voucherResponceCode = v.ResponseCode,
                                                 userName = v.CreditId.HasValue ? v.Credit.User.FirstName + " " + v.Credit.User.LastName : ""
                                             }).ToList();
            }

        }

        public List<VoucherWithVoucherConfigViewModel> VoucherSearch(string voucherConfigTitle,
                                                                     string fromPersianDate,
                                                                     string toPersianDate,
                                                                     string responseCode,
                                                                     int status,
                                                                     ref long count,
                                                                     int size = 1,
                                                                     int skip = 0)
        {

            List<Voucher> vouchers = _sdb.Voucher.ToList();


            if (voucherConfigTitle != "" && voucherConfigTitle != null)
                vouchers = vouchers.Where(v => v.VoucherConfig.Title.Contains(voucherConfigTitle)).ToList();

            if (responseCode != "" && responseCode != null)
                vouchers = vouchers.Where(v => v.ResponseCode.Contains(responseCode)).ToList();

            if (fromPersianDate != "" && fromPersianDate != null)
            {
                DateTime FromDate = fromPersianDate.PersianToGregorian();

                if (FromDate != DateTime.Now)
                    vouchers = vouchers.Where(v => v.FromDate > FromDate).ToList();
            }

            if (toPersianDate != "" && toPersianDate != null)
            {
                DateTime FromDate = toPersianDate.PersianToGregorian();

                if (FromDate != DateTime.Now)
                    vouchers = vouchers.Where(v => v.ToDate < FromDate).ToList();
            }


            switch (status)
            {
                case 1:
                    vouchers = vouchers.Where(v => v.CreditId.HasValue).ToList();
                    break;
                case 2:
                    vouchers = vouchers.Where(v => !v.CreditId.HasValue).ToList();
                    break;

                default:
                    break;


            }

            count = vouchers.Count;


            return vouchers.OrderBy(v => v.Id).Skip(size * skip).Take(size).
                Select(v => new VoucherWithVoucherConfigViewModel
                {
                    responseCode = v.ResponseCode,
                    voucherCode = v.Code,
                    voucherConfigTitle = v.VoucherConfig.Title,
                    fromPersianDate = v.FromDate.LocalToPersianDateLong(),
                    toPersianDate = v.ToDate.LocalToPersianDateLong(),
                    username = v.CreditId.HasValue ? v.Credit.User.FirstName + " " + v.Credit.User.LastName : "",
                    userId = v.CreditId.HasValue ? v.Credit.UserId : 0,

                }).ToList();
        }

        public ValidateVoucherViewModel ValidateVoucher(string code, long cpptId)
        {

            CategoryPurchasePackageType CPPT = _sdb.CategoryPurchasePackageTypes.Find(cpptId);

            if (CPPT == null)
                throw new RecabException((int)ExceptionType.CpptIdNotFound);

            Voucher voucher = _sdb.Voucher.FirstOrDefault(v => v.Code == code);

            if (voucher == null)
                return new ValidateVoucherViewModel
                {
                    fromDate = "",
                    message = "کد تخفیف وارد شده یافت نشد",
                    toDate = "",
                    validate = false,
                };


            DateTime DateTime = DateTime.Now;

            if (voucher.FromDate > DateTime || voucher.ToDate < DateTime)
                return new ValidateVoucherViewModel
                {
                    fromDate = "",
                    message = "تاریخ کد تخفیف وارد شده معتبر نمی باشد",
                    toDate = "",
                    validate = false,
                };

            if (voucher.CreditId.HasValue)

                return new ValidateVoucherViewModel
                {
                    fromDate = "",
                    message = "کد تخفیف وارد شده قبلاً استفاده شده است",
                    toDate = "",
                    validate = false,
                };


            string basePrice = CPPT.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == PackageConfig.Price) != null ?
                                CPPT.PurchaseConfig.FirstOrDefault(pc => pc.PackageBaseConfig.Title == PackageConfig.Price).Value : "";

            if (basePrice == "")
                throw new RecabException((int)ExceptionType.PriceNotFoundForCpptId);

            long price = 0;

             if(!Int64.TryParse(basePrice, out price))
                throw new RecabException((int)ExceptionType.PriceForCpptIdIncorrectFormat);

            return new ValidateVoucherViewModel
            {
                fromDate = voucher.FromDate.LocalToPersianDateLong(),
                message = voucher.VoucherConfig.Title,
                toDate = voucher.ToDate.LocalToPersianDateLong(),
                validate = true,
                type = (voucher.VoucherConfig.Status == VoucherStatus.درصدی?"%":"ریال") + " " + voucher.VoucherConfig.Value,
                amountWithoutDiscount = price.ToStringRial(),
                amountWithDisCount = (voucher.VoucherConfig.Status == VoucherStatus.درصدی ? (price -(( price * voucher.VoucherConfig.Value)/100)): (price- voucher.VoucherConfig.Value)).ToString("##,###")
            };


        }

        public CreditValidateVoucherViewModel ValidateVoucherForCredit(string code)
        {
           
            Voucher voucher = _sdb.Voucher.FirstOrDefault(v => v.Code == code);

            if (voucher == null)
                return new CreditValidateVoucherViewModel
                {
                    fromDate = "",
                    message = "کد تخفیف وارد شده یافت نشد",
                    toDate = "",
                    validate = false,
                };


            DateTime DateTime = DateTime.Now;

            if (voucher.FromDate > DateTime || voucher.ToDate < DateTime)
                return new CreditValidateVoucherViewModel
                {
                    fromDate = "",
                    message = "تاریخ کد تخفیف وارد شده معتبر نمی باشد",
                    toDate = "",
                    validate = false,
                };

            if (voucher.CreditId.HasValue)
                return new CreditValidateVoucherViewModel
                {
                    fromDate = "",
                    message = "کد تخفیف وارد شده قبلاً استفاده شده است",
                    toDate = "",
                    validate = false,
                };


            if (voucher.VoucherConfig.Status == VoucherStatus.درصدی)
                return new CreditValidateVoucherViewModel
                {
                    fromDate = "",
                    message = "کد تخفیف وارد شده مناسب برای افزایش اعتبار نمی باشد ",
                    toDate = "",
                    validate = false,
                };


            return new CreditValidateVoucherViewModel
            {
                fromDate = voucher.FromDate.LocalToPersianDateLong(),
                message = voucher.VoucherConfig.Title,
                toDate = voucher.ToDate.LocalToPersianDateLong(),
                validate = true,
                amount =voucher.VoucherConfig.Value.ToStringRial()
            };

        }
        public MemoryStream CreateVoucherFile(long voucherConfigId, ref string fileName)
        {

            VoucherConfig VoucherConfig = _sdb.VoucherConfig.Find(voucherConfigId);

            if (VoucherConfig == null)
                throw new RecabException((int)ExceptionType.VocherConfigNotFound);


            List<Voucher> Vouchers = _sdb.Voucher.Where(v => v.VoucherConfigId == VoucherConfig.Id).ToList();

            string name = string.Format("{0}.{1}",
                                          voucherConfigId.ToString(),
                                          DateTime.UtcNow.ToString()
                                          .Replace("/", "")
                                          .Replace(":", "")
                                          .Replace(" ", "")
                                          .Replace("-", ""));

            string extion = ".csv";

            fileName = name + extion;


            MemoryStream ms = new MemoryStream();

            var writer = new StreamWriter(ms);

            writer.WriteLine(string.Format("{0},{1},{2},{3},{4}",
                                             "ردیف",
                                            "کد تخفیف",
                                            "کد پیگیری",
                                            "تاریخ شروع تخفیف",
                                            "تاریخ پایان تخفیف"
                                             ));

            foreach (var item in Vouchers)
            {

                writer.WriteLine("{0},{1},{2},{3},{4}",
                                 item.Id.ToString(),
                                 item.Code,
                                 item.ResponseCode,
                                 item.FromDate.LocalToPersianDateLong().Replace(",", ""),
                                 item.ToDate.LocalToPersianDateLong().Replace(",", "")
                                 );

            }

            writer.Flush();

            ms.Position = 0;

            return ms;
        }


    }
}
