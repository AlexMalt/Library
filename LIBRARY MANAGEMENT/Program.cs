using LIBRARY_MANAGEMENT.Classes;
using System;


namespace LIBRARY_MANAGEMENT
{
    class Program
    {
        static void Main(string[] args)
        {
            int choixPrinc;
            do
            {
                Console.Clear();
                Console.WriteLine("Bievenue dans votre menu libraire zbrrrrrr");
                Console.WriteLine("1- Se connecter");
                Console.WriteLine("0- Quitter");

                Int32.TryParse(Console.ReadLine(), out choixPrinc);

                switch (choixPrinc)
                {
                    case 1:
                        Personne p = IHM.Connection();
                        Console.Clear();
                        switch (p.UserType)
                        {
                            case UserTypes_Enum.admin:
                                IHM.MenuPrincipalAdmin();
                                break;

                            default:
                                IHM.MenuBookOther();
                                break;
                        }
                        break;
                    default:
                        break;
                }

            } while (choixPrinc != 0);
            

        }
    }
}
