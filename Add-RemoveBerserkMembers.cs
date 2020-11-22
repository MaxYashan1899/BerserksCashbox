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
                            RemoveMember(berserkMembers);
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
                        int monthPaymentSum = CashBoxDatabaseOperation.ParseInt("Введите сумму ежемесячного взноса:");

                        BerserkMembers newMember = new BerserkMembers { BerserksName = name, StartDebt = monthPaymentSum, CurrentDate = DateTime.Now };
                        berserkMembers.Add(newMember);
                        Console.WriteLine($"{name} добавлен в члены клуба");

                        db.BerserkMembers.Add(newMember);
                        db.SaveChanges();
                     
                        flag = false;
                    }
                }
           }
        }
        public void RemoveMember(List<BerserkMembers> berserkMembers)
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
                        var memberForRemove = berserkMembers.First(n => n.BerserksName == name);
                        berserkMembers.Remove(memberForRemove);
                        Console.WriteLine($"{name} удален из членов клуба");

                        db.BerserkMembers.Remove(memberForRemove);
                        db.SaveChanges();
                      
                        flag = false;
                    }
                }
            }
        }
    }
}

// удаление, добавление (обработать ошибки вывода) 
// Ввести СтартДату (чтоб вычеслять месячный долг добавленным людям)

