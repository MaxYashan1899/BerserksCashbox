using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BerserksCashbox
{
    public class Add_RemoveBerserksMember
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
                            AddNewMember();
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

       
                void AddNewMember()
            {
                var flag = true;
                while (flag)
                {
                    Console.WriteLine("Введите имя нового члена клуба:");
                    string name = Console.ReadLine();
                    if ( berserkMembers.Any(n => n.BerserksName == name))
                        Console.WriteLine("Такое имя уже существует");
                    else
                    {
                        Console.WriteLine("Введите сумму ежемесячного взноса:");
                        int monthPaymentSum = int.Parse(Console.ReadLine());
                        var newMember = new BerserkMembers { BerserksName = name, StartDebt = monthPaymentSum, StartData = DateTime.Now };
                        berserkMembers.Add(newMember);
                        Console.WriteLine($"{name} добавлен в члены клуба");
                        using (var db = new BerserkMembersDatabase())
                        {
                            db.BerserkMembers.Add(newMember);
                            db.SaveChanges();
                        }
                        flag = false;
                    }
                }

            }
            void RemoveMember()
            {
                var flag = true;
                while (flag)
                {
                    Console.WriteLine("Введите имя члена клуба для удаления:");
                    string name = Console.ReadLine();
                    if (berserkMembers.All(n => n.BerserksName != name))
                    {
                        Console.WriteLine("Такого имени не существует");
                        
                    }
                    else
                    {
                        var memberForRemove = berserkMembers.First(n => n.BerserksName == name);
                        berserkMembers.Remove(memberForRemove);
                        Console.WriteLine($"{name} удален из членов клуба");
                        using (var db = new BerserkMembersDatabase())
                        {
                            db.BerserkMembers.Remove(memberForRemove);
                            db.SaveChanges();
                        }
                        flag = false;
                    }
                }
            }
        }
    }
}

// парсинг, проверки
// упорядочить методы

