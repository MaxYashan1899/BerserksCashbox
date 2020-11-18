////var path = "C:\\JoulupukkiList.txt";
////var nameList = File.ReadAllLines(path).ToList();

////var cashBoxOperation = new CashBoxOperation();
////cashBoxOperation.TotalSumInCashBox(generalCost, generalIncome, baseCashBoxSum)

////List<BerserkMembers> berserkMembers = new List<BerserkMembers>()
////{
////    foreach (var item in nameList)
////{
////    new BerserkMembers { BerserksName = item, CurrentDebt = 150 };
////}
////var newList = berserkMembers.BerserksName.OrderBy(x => x.).ToList();
////var contains = berserkMembers.BerserksName.Contains("Ragnar");
////Console.WriteLine(contains);


//public int TotalSumInCashBox(DatabaseMonthPayment databaseMonthPayment, CashBoxOperation cashBoxOperation)
//{
//    //int totalSumInCashBox = BaseCashBoxSum;
//    var currentSumInCashBox = 0;
//    using (var db = new CashBoxDatabase())
//    {
//        //var currentSumInCashBox =    BaseCashBoxSumOperation(totalSumInCashBox) 
//        currentSumInCashBox = cashBoxOperation.BaseCashBoxSum
//                                  + db.CashBoxOperations.Sum(s => s.OtherIncomes)
//                                  + databaseMonthPayment.LastMonthPaymentSum(databaseMonthPayment)
//                                  - db.CashBoxOperations.Sum(s => s.OtherExpenses)
//                                  - db.CashBoxOperations.Sum(s => s.WorkshopRental)
//                                  - db.CashBoxOperations.Sum(s => s.CommunityHouseRental);
//        //totalSumInCashBox = currentSumInCashBox;
//        //Console.WriteLine($"Баланс по кассе: {totalSumInCashBox} грн.");
//        Console.WriteLine($"Баланс по кассе: {currentSumInCashBox} грн.");
//        Console.WriteLine($"\tКасса на начало месяца: {cashBoxOperation.BaseCashBoxSum} грн.  \tРасходы на мастерскую: {db.CashBoxOperations.Sum(s => s.WorkshopRental)} грн.");
//        Console.WriteLine($"\tОбщая сумма взносов: {databaseMonthPayment.LastMonthPaymentSum(databaseMonthPayment)} грн. \t\tРасходы на ангар: {db.CashBoxOperations.Sum(s => s.CommunityHouseRental)} грн.");
//        Console.WriteLine($"\tСумма доходов: {db.CashBoxOperations.Sum(s => s.OtherIncomes)} грн. \t\t\tСумма расходов: {db.CashBoxOperations.Sum(s => s.OtherExpenses)} грн.");
//        Console.WriteLine();

//    }
//    //return totalSumInCashBox;
//    return currentSumInCashBox;
//}


////public int BaseCashBoxSumOperation(int totalSumInCashBox)
////{
////    int baseCashBoxSum = totalSumInCashBox;
////    return baseCashBoxSum;
////}