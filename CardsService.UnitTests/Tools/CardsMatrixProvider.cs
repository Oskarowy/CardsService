using CardsService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardsService.Tests.Tools
{
    public static class CardsMatrixProvider
    {
        public static TheoryData<CardType, CardStatus, bool> AllCardsCollection => new TheoryData<CardType, CardStatus, bool>
        {
            {CardType.Prepaid, CardStatus.Active, true},
            {CardType.Prepaid, CardStatus.Ordered, true },
            {CardType.Prepaid, CardStatus.Inactive, true },
            {CardType.Prepaid, CardStatus.Restricted, true },
            {CardType.Prepaid, CardStatus.Blocked, true },
            {CardType.Prepaid, CardStatus.Expired, true },
            {CardType.Prepaid, CardStatus.Closed, true },
            {CardType.Prepaid, CardStatus.Active, false },
            {CardType.Prepaid, CardStatus.Ordered, false },
            {CardType.Prepaid, CardStatus.Inactive, false },
            {CardType.Prepaid, CardStatus.Restricted, false },
            {CardType.Prepaid, CardStatus.Blocked, false },
            {CardType.Prepaid, CardStatus.Expired, false },
            {CardType.Prepaid, CardStatus.Closed, false },
            {CardType.Debit, CardStatus.Active, true },
            {CardType.Debit, CardStatus.Ordered, true },
            {CardType.Debit, CardStatus.Inactive, true },
            {CardType.Debit, CardStatus.Restricted, true },
            {CardType.Debit, CardStatus.Blocked, true },
            {CardType.Debit, CardStatus.Expired, true },
            {CardType.Debit, CardStatus.Closed, true },
            {CardType.Debit, CardStatus.Active, false },
            {CardType.Debit, CardStatus.Ordered, false },
            {CardType.Debit, CardStatus.Inactive, false },
            {CardType.Debit, CardStatus.Restricted, false },
            {CardType.Debit, CardStatus.Blocked, false },
            {CardType.Debit, CardStatus.Expired, false },
            {CardType.Debit, CardStatus.Closed, false },
            {CardType.Credit, CardStatus.Active, true },
            {CardType.Credit, CardStatus.Ordered, true },
            {CardType.Credit, CardStatus.Inactive, true },
            {CardType.Credit, CardStatus.Restricted, true },
            {CardType.Credit, CardStatus.Blocked, true },
            {CardType.Credit, CardStatus.Expired, true },
            {CardType.Credit, CardStatus.Closed, true },
            {CardType.Credit, CardStatus.Active, false },
            {CardType.Credit, CardStatus.Ordered, false },
            {CardType.Credit, CardStatus.Inactive, false },
            {CardType.Credit, CardStatus.Restricted, false },
            {CardType.Credit, CardStatus.Blocked, false },
            {CardType.Credit, CardStatus.Expired, false },
            {CardType.Credit, CardStatus.Closed, false }
        };
    }

    public record CardMatrixRecord
    {
        public CardMatrixRecord(CardType cardType, CardStatus cardStatus, bool isPinSet)
        {
            CardType = cardType;
            CardStatus = cardStatus;
            IsPinSet = isPinSet;
        }

        public CardType CardType { get; set; }
        public CardStatus CardStatus { get; set; }
        public bool IsPinSet { get; set; }
    }
}
