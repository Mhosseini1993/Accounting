using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accounting.DataLayer.UnitOfWork;
using Accounting.ViewModel.AccountViewModel;

namespace Accounting.BusinessLayer
{
    public class CalculateAccountBalance
    {
        public static AccountBalanceViewModel Calculate()
        {
            DateTime start = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime End = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 30);

            AccountBalanceViewModel Result = new AccountBalanceViewModel();

            using (UnitOfwork unit = new UnitOfwork())
            {
                Result.Get = unit.AccountRepository.GetAll(t => t.TypeID == 1 && (t.DateTime >= start && t.DateTime <= End)).Select(t => t.Amount).Sum();
                Result.Pay = unit.AccountRepository.GetAll(t => t.TypeID == 2 && (t.DateTime >= start && t.DateTime <= End)).Select(t => t.Amount).Sum();
                Result.Remain = Result.Get - Result.Pay;

            }

            return Result;
        }
    }
}
