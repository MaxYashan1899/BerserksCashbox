using System;
using System.Collections.Generic;
using System.Linq;

namespace CHRBerserk.BerserksCashbox
{
    public class Add_RemoveBerserkMembers
    {
        public void AddAndRemoveMembers(List<BerserkMembers> berserkMembers)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine("Введите вид операции: 1.Добавить нового члена клуба  2.Удалить члена клуба 3. Выйти из данной операции");

                try
                {
                    int number = Convert.ToInt32(Console.ReadLine());

                    switch (number)
                    {
                        case 1:
                            AddNewMember(berserkMembers);
                            break;
                        case 2:
                            RemoveMember();
                            break;
                        case 3:
                            flag = false;
                            continue;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public void AddNewMember(List<BerserkMembers> berserkMembers)
            {
                var flag = true;
                while (flag)
                {
                    Console.WriteLine("Введите имя нового члена клуба:");
                    string name = Console.ReadLine();

                using (var db = new BerserkMembersDatabase())
                {
                    if (db.BerserkMembers.Any(n => n.BerserksName == name))
                        Console.WriteLine("Такое имя уже существует");
                    else if (String.IsNullOrEmpty(name))
                        throw new ArgumentNullException("Имя не может быть пустым", nameof(name));
                    else
                    {
                        int monthPaymentSum = CashBoxDatabaseOperation.ParseInt("Введите сумму ежемесячного взноса");

                        BerserkMembers newMember = new BerserkMembers { BerserksName = name, StartDebt = monthPaymentSum, StartDate = DateTime.Now, CurrentDate = DateTime.Now };
                        berserkMembers.Add(newMember);
                        Console.WriteLine($"{name} добавлен в члены клуба");

                        db.BerserkMembers.Add(newMember);
                        db.SaveChanges();
                     
                        flag = false;
                    }
                }
           }
        }
        public void RemoveMember()
            {
            var flag = true;

            while (flag)
            {
                Console.WriteLine("Введите имя члена клуба для удаления:");
                string name = Console.ReadLine();

                using (var db = new BerserkMembersDatabase())
                {
                    if (db.BerserkMembers.All(n => n.BerserksName != name))
                        Console.WriteLine("Такого имени не существует");
                    else if (String.IsNullOrEmpty(name))
                        throw new ArgumentNullException("Имя не может быть пустым", nameof(name));
                    else
                    {
                        var memberForRemove = db.BerserkMembers.Where(n => n.BerserksName == name);
                        PaymentsSumOfRemovedMember(name);

                        Console.WriteLine($"{name} удален из членов клуба");
                        db.BerserkMembers.RemoveRange(memberForRemove);
                        db.SaveChanges();
                      
                        flag = false;
                    }
                }
            }
         }
        public int PaymentsSumOfRemovedMember(string name)
        {
            var paymentsSumOfRemovedMember = 0;
            using (var db = new BerserkMembersDatabase())
            {
                paymentsSumOfRemovedMember = db.BerserkMembers.Where(n => n.BerserksName == name).Sum(n => n.CurrentPayment);
                if (paymentsSumOfRemovedMember > 0)
                {
                    BerserkMembers noNameMember = new BerserkMembers { BerserksName = "NoName", StartDebt = 0, CurrentPayment = paymentsSumOfRemovedMember, CurrentDate = DateTime.Now, StartDate = DateTime.Now };

                    db.BerserkMembers.Add(noNameMember);
                    db.SaveChanges();
                }
            }
            return paymentsSumOfRemovedMember;
        }
    }
}



