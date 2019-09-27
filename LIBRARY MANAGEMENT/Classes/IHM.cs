using System;
using System.Collections.Generic;
using System.Text;

namespace LIBRARY_MANAGEMENT.Classes
{
    public class IHM
    {

        private static IHM instance = null;
        private static object _lock = new object();
        private static Personne pLogged = new Personne();
        public static IHM Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                        instance = new IHM();
                    return instance;
                }
            }
        }


        public static Personne Connection()
        {
            bool isLogged = false;
            do
            {
                Console.Clear();
                Console.WriteLine("------------BIENVENUE A LA BIB'INN-------");
                Console.Write(" Login : ");
                string l = Console.ReadLine();
                Console.Write(" Password : ");
                string p = Console.ReadLine();
                (Personne pers, bool isLoggedd) = Personne.Login(l, p);
                pLogged = pers;
                isLogged = isLoggedd;
            } while (isLogged == false);
            return pLogged;
        }
        public static void MenuPrincipalAdmin()
        {
            int choix;
            do
            {
                Console.Clear();
                Console.WriteLine("Bievenue dans votre menu libraire zbrrrrrr");
                Console.WriteLine("1- Menu User");
                Console.WriteLine("2- Menu book");
                Console.WriteLine("0- Quitter");

                Int32.TryParse(Console.ReadLine(), out choix);

                switch (choix)
                {
                    case 1:
                        Console.Clear();
                        MenuUser();
                        break;

                    case 2:
                        Console.Clear();
                        MenuBook();
                        break;


                    default:
                        break;
                }

            } while (choix != 0);

        }

        public static void MenuBookOther()
        {
            int choixb;
            int idB;
            int card;
            Book b = new Book();
            Library l = new Library();
            List<Book> ListeB = new List<Book>();
            Console.Clear();
            do
            {

                Console.WriteLine("1- Lister les livres");
                Console.WriteLine("2- Rechercher un livre / titre");
                Console.WriteLine("3- Emprunter un livre");
                Console.WriteLine("4- Retour d'un livre");
                Console.WriteLine("0- Quitter");
                Int32.TryParse(Console.ReadLine(), out choixb);

                switch (choixb)
                {
                    case 1:
                        Console.Clear();
                        ListeB = Book.GetAllBooks();
                        Console.WriteLine("-------LISTE DES BOUQUINS--------");
                        foreach (Book tin in ListeB)
                        {
                            Console.WriteLine(" ID : {0} , Auteur : {1}, Livre : {2}, Status : {3}", tin.Id, tin.Auteur, tin.Titre, tin.Status);
                        }
                        Console.WriteLine("---------------------------------");
                        break;

                    case 2:
                        Console.Clear();
                        Console.WriteLine("Veuillez entrer votre recherche");
                        string recherche = Console.ReadLine();
                        ListeB = Book.GetBook(recherche);

                        Console.WriteLine("-----Resultat de votre recherche-----");
                        foreach (Book agnie in ListeB)
                        {
                            Console.WriteLine(" ID : {0} , Auteur : {1}, Livre {2}", agnie.Id, agnie.Auteur, agnie.Titre);
                        }
                        Console.WriteLine("-------Fin de votre recherche-------");
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Saisir n° de carte de membre ");

                        Int32.TryParse(Console.ReadLine(), out card);
                        Console.WriteLine("Saisir n° du livre ");

                        Int32.TryParse(Console.ReadLine(), out idB);
                        l.BorrowBook(card, idB);
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Saisir n° du livre retourné ");
                        Int32.TryParse(Console.ReadLine(), out idB);
                        l.ReturnBook(idB);
                        break;
                    default:
                        break;
                }

            } while (choixb != 0);
        }
        public static void MenuBook()
        {
            int choixb;
            int idB;
            int card;
            Book b = new Book();
            Library l = new Library();
            List<Book> ListeB = new List<Book>();
            Console.Clear();
            do
            {

                Console.WriteLine("1- Lister les livres");
                Console.WriteLine("2- Ajouter un livre");
                Console.WriteLine("3- Rechercher un livre / titre");
                Console.WriteLine("4- Emprunter un livre");
                Console.WriteLine("5- Retour d'un livre");
                Console.WriteLine("6- Lister les livres empruntés");
                Console.WriteLine("7- Menu principal");
                Int32.TryParse(Console.ReadLine(), out choixb);

                switch (choixb)
                {
                    case 1:
                        Console.Clear();

                        ListeB = Book.GetAllBooks();
                        Console.WriteLine("-------LISTE DES BOUQUINS--------");
                        foreach (Book tin in ListeB)
                        {
                            Console.WriteLine(" ID : {0} , Auteur : {1}, Livre : {2}, Status : {3}", tin.Id, tin.Auteur, tin.Titre, tin.Status);
                        }
                        Console.WriteLine("---------------------------------");
                        break;

                    case 2:
                        Console.Clear();
                        Console.Write("L'auteur du bouquin : ");
                        b.Auteur = Console.ReadLine();
                        Console.Write("Le titre du bouquin : ");
                        b.Titre = Console.ReadLine();

                        b.Add();
                        break;
                    case 3:
                        Console.Clear();
                        Console.WriteLine("Veuillez entrer votre recherche");
                        string recherche = Console.ReadLine();
                        ListeB = Book.GetBook(recherche);

                        Console.WriteLine("-----Resultat de votre recherche-----");
                        foreach (Book agnie in ListeB)
                        {
                            Console.WriteLine(" ID : {0} , Auteur : {1}, Livre {2}", agnie.Id, agnie.Auteur, agnie.Titre);
                        }
                        Console.WriteLine("-------Fin de votre recherche-------");
                        break;
                    case 4:
                        Console.Clear();
                        Console.WriteLine("Saisir n° de carte de membre ");

                        Int32.TryParse(Console.ReadLine(), out card);
                        Console.WriteLine("Saisir n° du livre ");

                        Int32.TryParse(Console.ReadLine(), out idB);
                        l.BorrowBook(card, idB);
                        break;
                    case 5:
                        Console.Clear();
                        Console.WriteLine("Saisir n° du livre retourné ");
                        Int32.TryParse(Console.ReadLine(), out idB);
                        l.ReturnBook(idB);
                        break;
                    case 6:
                        Console.Clear();
                        List<string> ListBB = l.GetBorrowedBooks();
                        foreach (string borrowed in ListBB)
                        {
                            Console.WriteLine(borrowed);
                        }
                        break;

                    default:

                        break;
                }

            } while (choixb != 7);



        }
        public static void MenuUser()
        {
            int choixu;
            do
            {
                Console.WriteLine("1- Ajouter un user");
                Console.WriteLine("2- Supprimer un user");
                Console.WriteLine("3- Lister les users");
                Console.WriteLine("4- Menu principal");

                Int32.TryParse(Console.ReadLine(), out choixu);

                switch (choixu)
                {
                    case 1:
                        int gen;
                        int compteur;
                        int numRole;
                        Personne p = new Personne();
                        
                            compteur = 0;
                            Console.Clear();
                            Console.Write("Nom :  ");
                            p.Nom = Console.ReadLine();
                            Console.Write("Prenom :  ");
                            p.Prenom = Console.ReadLine();
                            Console.Write("L'identifiant :  ");
                            string l = Console.ReadLine();
                            Console.Write("le mot de passe :  ");
                            string pwd = Console.ReadLine();

                            Array roles = Enum.GetValues(typeof(UserTypes_Enum));
                            foreach (UserTypes_Enum u in roles)
                            {
                                Console.WriteLine(compteur + " : " + u);
                                compteur++;
                            }

                            Console.Write("Numéro de rôle :  ");
                            numRole = Convert.ToInt32(Console.ReadLine());
                            p.UserType = (UserTypes_Enum)numRole;

                            gen = p.Add(l, pwd);
                        if (gen == default)
                        {
                            Console.Clear();
                            Console.WriteLine("Erreur login existant !");
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Voici votre n° de carte : " + gen);
                        }
                        break;

                    case 2:
                        Console.Clear();
                        Console.Write("Merci de saisir l'id à supprimer :");
                        int idUserDelete;
                        Int32.TryParse(Console.ReadLine(), out idUserDelete);

                        try
                        {
                            bool del = Personne.DeleteUser(idUserDelete);
                            if (del)
                            {
                                Console.WriteLine($"Utilisateur {idUserDelete} supprimé");
                            }
                            else
                            {
                                Console.WriteLine($"Utilisateur {idUserDelete} introuvable");
                            }

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }


                        break;
                    case 3:
                        Console.Clear();
                        List<Personne> listU = Personne.GetAllUsers();
                        Console.WriteLine("Liste des Utilisateurs");
                        foreach (Personne pers in listU)
                        {

                            Console.WriteLine("Id : " + pers.Id + ", Nom : " + pers.Nom + ", Prenom : " + pers.Prenom + ", N° Carte : " + pers.NumUser + ", Role : " + pers.UserType);
                        }
                        break;

                    default:
                        break;
                }

            } while (choixu != 4);
        }
    }
}
