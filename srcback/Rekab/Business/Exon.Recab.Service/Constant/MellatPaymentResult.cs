using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exon.Recab.Service.Constant
{
    public enum MellatPaymentResult
    {
        TransactionApproved = 0,
        InvalidCardNumber = 11,
        NoSufficientFunds = 12,
        IncorrectPin = 13,
        AllowableNumberOfPinTriesExceeded = 14,
        CardNotEffective = 15,
        ExceedsWithdrawalFrequencyLimit = 16,
        CustomerCancellation = 17,
        ExpiredCard = 18,
        ExceedsWithdrawalAmountLimit = 19,
        NoSuchIssuer = 111,
        CardSwitchInternalError = 112,
        IssuerOrSwitchIsInoperative = 113,
        TransactionNotPermittedToCardHolder = 114,
        InvalidMerchant = 21,
        SecurityViolation = 23,
        InvalidUserOrPassword = 24,
        InvalidAmount = 25,
        InvalidResponse = 31,
        FormatError = 32,
        NoInvestmentAccount = 33,
        SystemInternalError = 34,
        InvalidBusinessDate = 35,
        DuplicateOrderId = 41,
        SaleTransactionNotFound = 42,
        DuplicateVerify = 43,
        VerifyTransactionNotFound = 44,
        TransactionHasBeenSettled = 45,
        TransactionHasNotBeenSettled = 46,
        SettleTransactionNotFound = 47,
        TransactionHasBeenReversed = 48,
        RefundTransactionNotFound = 49,
        BillDigitIncorrect = 412,
        PaymentDigitIncorrect = 413,
        BillOrganizationNotValid = 414,
        SessionTimeout = 415,
        DataAccessException = 416,
        PayerIdIsInvalid = 417,
        CustomerNotFound = 418,
        TryCountExceeded = 419,
        InvalidIP = 421,
        DuplicateTransmission = 51,
        OriginalTransactionNotFound = 54,
        InvalidTransaction = 55,
        ErrorInSettle = 61,
        undefind = 999
    }
}
