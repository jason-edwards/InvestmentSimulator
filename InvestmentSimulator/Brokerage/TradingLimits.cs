using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Text;

namespace InvestmentSimulator.Brokerage
{
    public class TradingLimits
    {
        public DepositLimit DepositLimit { get; set; }
        public WithdrawalLimit WithdrawalLimit { get; set; }
    }
}
