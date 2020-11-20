using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace BerserksCashbox
{
    class Program
    {
       
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            var cashBoxOperation = new CashBoxOperation() { BaseCashBoxSum = 2500 };
            var databaseCashBoxOperation = new DatabaseCashBoxOperation();
            var databaseMonthPayment = new DatabaseMonthPayment();
            var berserkMembers = new List<BerserkMembers>();

            var monthPaymentOperations = new MonthPaymentOperations();
            var add_RemoveBerserksMember = new Add_RemoveBerserksMember();
    
            #region Инициализация объектами
            BerserkMembers berserk1 = new BerserkMembers { BerserksName = "Ragnar", StartDebt = 250, StartData = DateTime.Now};
            BerserkMembers berserk2 = new BerserkMembers { BerserksName = "Ottar", StartDebt = 250, StartData = DateTime.Now};
            BerserkMembers berserk3 = new BerserkMembers { BerserksName = "Torbiorn", StartDebt = 250, StartData = DateTime.Now};
            BerserkMembers berserk4 = new BerserkMembers { BerserksName = "Eivar", StartDebt = 150, StartData = DateTime.Now };
            berserkMembers.Add(berserk1);
            berserkMembers.Add(berserk2);
            berserkMembers.Add(berserk3);
            berserkMembers.Add(berserk4);
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
                            databaseMonthPayment.DatabaseInitialization(berserk1, berserk2, berserk3, berserk4);
                            monthPaymentOperations.GetMonthPayment(berserkMembers);
                            databaseMonthPayment.DatabaseInfo(berserkMembers);
                            break;
                        case 2:
                            databaseCashBoxOperation.WorkshopRentalPayment(cashBoxOperation);
                            break;
                        case 3:
                            databaseCashBoxOperation.CommunityHouseRentalPayment(cashBoxOperation);
                            break;
                        case 4:
                            databaseCashBoxOperation.GetOtherExpenses(cashBoxOperation);
                            break;
                        case 5:
                            databaseCashBoxOperation.GetOtherIncomes(cashBoxOperation);
                            break;
                        case 6:
                            databaseMonthPayment.DatabaseInitialization(berserk1, berserk2, berserk3, berserk4);
                            databaseMonthPayment.DatabaseInfo(berserkMembers);
                            break;
                        case 7:
                            databaseCashBoxOperation.TotalSumInCashBox(databaseMonthPayment, cashBoxOperation);
                            break;
                        case 8:
                            databaseMonthPayment.DatabaseInitialization(berserk1, berserk2, berserk3, berserk4);
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

// парсинг, проверка

// добавить \удалить нового члена клуба  -  ошибка при новом запуске консоли
// редактирование (код и вывод на консоль)

// чтение имен с файла 