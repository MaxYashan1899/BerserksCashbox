using System;
using System.Collections.Generic;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var cashBox = new CashBox() { BaseCashBoxSum = 2500 };
            var cashBoxReport = new CashBoxReport();
            var cashBoxPaymentsOperation = new CashBoxPaymentsOperation();
            var berserkMembersMonthReport = new BerserkMembersMonthReport();
            var berserkMembers = new List<BerserkMembers>();

            var monthPaymentOperations = new BerserkMembersMonthPaymentOperations();
            var add_RemoveBerserksMember = new Add_RemoveBerserkMembers();

            #region Инициализация объектами
            //BerserkMembers berserk1 = new BerserkMembers { BerserksName = "Ragnar", StartDebt = 250, CurrentDate = DateTime.Now, StartDate = DateTime.Now};
            //BerserkMembers berserk2 = new BerserkMembers { BerserksName = "Ottar", StartDebt = 250, CurrentDate = DateTime.Now, StartDate = DateTime.Now };
            //BerserkMembers berserk3 = new BerserkMembers { BerserksName = "Torbiorn", StartDebt = 250, CurrentDate = DateTime.Now, StartDate = DateTime.Now };
            //BerserkMembers berserk4 = new BerserkMembers { BerserksName = "Eivar", StartDebt = 150, CurrentDate = DateTime.Now, StartDate = DateTime.Now };
            //berserkMembers.Add(berserk1);
            //berserkMembers.Add(berserk2);
            //berserkMembers.Add(berserk3);
            //berserkMembers.Add(berserk4);

            berserkMembers.Add(new BerserkMembers { BerserksName = "Ragnar", StartDebt = 250, CurrentDate = DateTime.Now, StartDate = DateTime.Now });
            berserkMembers.Add(new BerserkMembers { BerserksName = "Ottar", StartDebt = 250, CurrentDate = DateTime.Now, StartDate = DateTime.Now });
            berserkMembers.Add(new BerserkMembers { BerserksName = "Torbiorn", StartDebt = 250, CurrentDate = DateTime.Now, StartDate = DateTime.Now });
            berserkMembers.Add(new BerserkMembers { BerserksName = "Eivar", StartDebt = 150, CurrentDate = DateTime.Now, StartDate = DateTime.Now });
            using (var db = new BerserkMembersDatabase())
            {
                if (db.BerserkMembers.Count() == 0)
                {
                    db.BerserkMembers.AddRange(berserkMembers);
                    db.SaveChanges();
                }
            }
            #endregion

            bool flag = true;
            while (flag)
            {
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.WriteLine("1.Ежемесячный взнос  \t 2. Оплата мастерской  \t 3. Оплата общинного дома");
                Console.WriteLine("4. Другие расходы \t 5. Другие доходы \t 6. Баланс по людям");
                Console.WriteLine("7. Баланс по кассе \t 8. Добавить/удалить члена клуба \t 9. Выйти из программы");
                Console.WriteLine("--------------------------------");
                Console.WriteLine("Введите номер пункта:");
                Console.ForegroundColor = color;
                try
                {
                    int number = Convert.ToInt32(Console.ReadLine());

                    switch (number)
                    {
                        case 1:
                            monthPaymentOperations.GetMonthPayment(berserkMembers);
                            berserkMembersMonthReport.MembersPaymentsMonthReport(berserkMembers);
                            break;
                        case 2:
                            cashBoxPaymentsOperation.WorkshopRentalPayment(cashBox);
                            break;
                        case 3:
                            cashBoxPaymentsOperation.CommunityHouseRentalPayment(cashBox);
                            break;
                        case 4:
                            cashBoxPaymentsOperation.GetOtherExpenses(cashBox);
                            break;
                        case 5:
                            cashBoxPaymentsOperation.GetOtherIncomes(cashBox);
                            break;
                        case 6:
                            berserkMembersMonthReport.MembersPaymentsMonthReport(berserkMembers);
                            break;
                        case 7:
                            cashBoxReport.TotalSumInCashBox(monthPaymentOperations, cashBox, cashBoxPaymentsOperation);
                            break;
                        case 8:
                            add_RemoveBerserksMember.AddAndRemoveMembers(berserkMembers);
                            break;
                        case 9:
                            flag = false;
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
        }
    }
}
