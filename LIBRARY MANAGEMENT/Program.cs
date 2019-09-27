using LIBRARY_MANAGEMENT.Classes;
using System;


namespace LIBRARY_MANAGEMENT
{
    class Program
    {
        static void Main(string[] args)
        {
            
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
            

            
        }
    }
}
