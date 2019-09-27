using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LIBRARY_MANAGEMENT.Classes
{
    class Library
    {
        private int idUser;
        private int idBook;
        public static event Action<string> livreNonRendu;


        private SqlCommand command;
        private SqlDataReader reader;

        public int IdUser { get => idUser; set => idUser = value; }
        public int IdBook { get => idBook; set => idBook = value; }

        public bool BorrowBook(int carte, int idBook)
        {
            BookStatus_Enum statusBook = BookStatus_Enum.disponible;
            bool bookExist = true;
            bool empruntOk = false;

            command = new SqlCommand("SELECT id FROM personne WHERE num_user = @carte", Database.Instance);
            command.Parameters.Add(new SqlParameter("@carte", carte));

            Database.Instance.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                IdUser = reader.GetInt32(0);
            }
            command.Dispose();
            reader.Close();
            command = new SqlCommand("SELECT id, status FROM book WHERE id = @id", Database.Instance);
            command.Parameters.Add(new SqlParameter("@id", idBook));

            reader = command.ExecuteReader();
            if (reader.Read())
            {
                IdBook = reader.GetInt32(0);
                statusBook = (BookStatus_Enum)(int)reader.GetByte(1);
            } else
            {
                Console.WriteLine($"ERREUR :: Aucun livre trouvé avec cet ID : {idBook}");
                bookExist = false;
            }
            command.Dispose();
            reader.Close();

            if (statusBook != BookStatus_Enum.indisponible && bookExist) {
                command = new SqlCommand("INSERT INTO library (id_personne, id_book, date_emprunt) VALUES (@idp, @idb, @de)", Database.Instance);
                command.Parameters.Add(new SqlParameter("@idp", IdUser));
                command.Parameters.Add(new SqlParameter("@idb", IdBook));
                command.Parameters.Add(new SqlParameter("@de", DateTime.Now));

                command.ExecuteNonQuery();
                command.Dispose();

                command = new SqlCommand("UPDATE book SET status = @status WHERE id=@id", Database.Instance);
                command.Parameters.Add(new SqlParameter("@id", IdBook));
                command.Parameters.Add(new SqlParameter("@status", BookStatus_Enum.indisponible));

                command.ExecuteNonQuery();
                command.Dispose();
                empruntOk = true;
                
            }
            Database.Instance.Close();
            return empruntOk;
        }

        public void ReturnBook(int id)
        {
            command = new SqlCommand("UPDATE book SET status = @status WHERE id=@id", Database.Instance);
            command.Parameters.Add(new SqlParameter("@id", id));
            command.Parameters.Add(new SqlParameter("@status", BookStatus_Enum.disponible));
            Database.Instance.Open();
            command.ExecuteNonQuery();
            command.Dispose();
            Database.Instance.Close();
        }

        public List<string> GetBorrowedBooks()
        {
            List<string> ListOfBorrewedB = new List<string>();
            string books = "";

            command = new SqlCommand("SELECT nom,prenom,num_user,titre,auteur,date_emprunt, p.id, b.id,l.id_personne, l.id_book FROM personne as p,book AS b, library as l WHERE p.id=l.id_personne AND b.id=l.id_book ", Database.Instance);
            Database.Instance.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                books = $"{reader.GetString(0)} {reader.GetString(1)} ({reader.GetInt32(2)} ) : \" {reader.GetString(3)} \" - {reader.GetString(4)} (le {reader.GetDateTime(5)} )";

                TimeSpan ts = DateTime.Now - reader.GetDateTime(5);

                if (ts.Days > 7)
                {
                    if (livreNonRendu != null)
                        livreNonRendu(books);
                } else
                {
                    ListOfBorrewedB.Add(books);
                }
                
            }

            command.Dispose();
            Database.Instance.Close();

            return ListOfBorrewedB;


        }


    }
}
