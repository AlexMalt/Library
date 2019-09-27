using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LIBRARY_MANAGEMENT.Classes
{
    class Book
    {
        private int id;
        private string titre;
        private string auteur;
        private BookStatus_Enum status;
        private static SqlCommand command;
        private static SqlDataReader reader;

        public int Id { get => id; set => id = value; }
        public string Titre { get => titre; set => titre = value; }
        public string Auteur { get => auteur; set => auteur = value; }
        internal BookStatus_Enum Status { get => status; set => status = value; }

        public Book()
        {
        }

        public Book(int id)
        {
            command = new SqlCommand("SELECT titre, auteur FROM books WHERE id = @id", Database.Instance);
            command.Parameters.Add(new SqlParameter("@id", id));

            Database.Instance.Open();
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                Titre = reader.GetString(0);
                Auteur = reader.GetString(1);
                this.Id = id;
            }
            command.Dispose();
            Database.Instance.Close();
        }

        public void Add()
        {
            
            command = new SqlCommand("INSERT INTO book (titre, auteur,status) OUTPUT INSERTED.ID values(@titre,@auteur,@status)", Database.Instance);
            command.Parameters.Add(new SqlParameter("@titre", Titre));
            command.Parameters.Add(new SqlParameter("@auteur", Auteur));
            Status = BookStatus_Enum.disponible;
            command.Parameters.Add(new SqlParameter("@status", Status));
            
            Database.Instance.Open();
            Id = (int)command.ExecuteScalar();
            command.Dispose();
            Database.Instance.Close();
        }

        public static List<Book> GetAllBooks()
        {
            List<Book> listeB = new List<Book>();

            command = new SqlCommand("SELECT id, titre, auteur,status FROM book", Database.Instance);
            Database.Instance.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Book b = new Book
                {
                    Id = reader.GetInt32(0),
                    Titre = reader.GetString(1),
                    Auteur = reader.GetString(2),
                    Status = (BookStatus_Enum)reader.GetByte(3)
                };

                listeB.Add(b);

            }
            command.Dispose();
            Database.Instance.Close();
            return listeB;
        }

        public static  List<Book> GetBook(string research)
        {
            List<Book> ListeBR = new List<Book>();

            command = new SqlCommand("SELECT id, titre, auteur,status FROM book WHERE titre LIKE @research", Database.Instance);
            research = "%" + research + "%";
            command.Parameters.Add(new SqlParameter("@research", research ));

            Database.Instance.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Book b = new Book
                {
                    Id = reader.GetInt32(0),
                    Titre = reader.GetString(1),
                    Auteur = reader.GetString(2),
                    Status = (BookStatus_Enum)reader.GetByte(3)
                };

                ListeBR.Add(b);
            }
            command.Dispose();
            Database.Instance.Close();
            return ListeBR;
        }

    }
}
