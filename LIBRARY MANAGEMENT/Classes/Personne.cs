using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace LIBRARY_MANAGEMENT.Classes
{
    public class Personne
    {
        private int id;
        private string nom;
        private string prenom;
        private int numUser;
        private int idP;
        private UserTypes_Enum userType;
        private static bool isLogged = false;
        private static SqlCommand command;
        private static SqlDataReader reader;
        public string Nom { get => nom; set => nom = value; }
        public string Prenom { get => prenom; set => prenom = value; }
        public int NumUser { get => numUser; set => numUser = value; }
        public UserTypes_Enum UserType { get => userType; set => userType = value; }
        public int Id { get => id; set => id = value; }
        public int IdP { get => idP; set => idP = value; }
       

        public Personne()
        {

        }

        public Personne(int id)
        {
            command = new SqlCommand("select nom, prenom,num_user,role FROM personne WHERE id=@id", Database.Instance);
            command.Parameters.Add(new SqlParameter("@id", id));
            Database.Instance.Open();
            // recuperer les resuletat de la requete
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                Nom = reader.GetString(0);
                Prenom = reader.GetString(1);
                NumUser = reader.GetInt32(2);
                UserType = (UserTypes_Enum)reader.GetInt32(3);
                this.Id = id;
            }
            command.Dispose();
            Database.Instance.Close();
        }

        public int Add(string log,string pwd)
        {
            bool randoKey = false;
            command = new SqlCommand("SELECT num_user from personne where num_user = @randnum", Database.Instance);
            while (!randoKey)
            {
                Random randnum = new Random();
                int randouille = randnum.Next(0, 9999);

                command.Parameters.Add(new SqlParameter("@randnum", randouille));
                Database.Instance.Open();
                reader = command.ExecuteReader();
                if (!reader.Read())
                {
                    NumUser = randouille;
                    randoKey = true;
                }
                reader.Close();
                command.Dispose();
                command = new SqlCommand("SELECT login FROM login WHERE login=@login  ", Database.Instance);
                command.Parameters.Add(new SqlParameter("@login", log));
                reader = command.ExecuteReader();
                if (reader.Read())
                {

                    NumUser = default;
                    reader.Close();
                    command.Dispose();
                }
                else
                {
                    reader.Close();
                    command.Dispose();
                    command = new SqlCommand("INSERT INTO personne (nom,prenom,num_user,role) OUTPUT INSERTED.ID values(@nom,@prenom,@numUser,@role)", Database.Instance);
                    command.Parameters.Add(new SqlParameter("@nom", Nom));
                    command.Parameters.Add(new SqlParameter("@prenom", Prenom));
                    command.Parameters.Add(new SqlParameter("@numUser", NumUser));
                    command.Parameters.Add(new SqlParameter("@role", UserType));
                    
                    Id = (int)command.ExecuteScalar();
                    command.Dispose();
                    command = new SqlCommand("INSERT INTO login (login,password,id_personne) VALUES (@login,@pwd,@idP)", Database.Instance);
                    command.Parameters.Add(new SqlParameter("@login", log));
                    command.Parameters.Add(new SqlParameter("@pwd", pwd));
                    command.Parameters.Add(new SqlParameter("@idP", Id));
                    command.ExecuteNonQuery();
                    reader.Close();
                    command.Dispose();
                }
                
                Database.Instance.Close();
            }

            
            Database.Instance.Close();

            return NumUser;

        }

        public static List<Personne> GetAllUsers()
        {
            List<Personne> listeU = new List<Personne>();

            command = new SqlCommand("SELECT id, nom,prenom,num_user,role FROM personne", Database.Instance);
            Database.Instance.Open();
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                Personne u = new Personne
                {
                    Id = reader.GetInt32(0),
                    Nom = reader.GetString(1),
                    Prenom = reader.GetString(2),
                    NumUser = reader.GetInt32(3),
                    UserType = (UserTypes_Enum)reader.GetInt16(4)
                };

                listeU.Add(u);

            }
            command.Dispose();
            Database.Instance.Close();
            return listeU;
        }
        public static bool DeleteUser(int id)
        {
            bool etBill = false;

            command = new SqlCommand("select id FROM personne WHERE id=@id", Database.Instance);
            command.Parameters.Add(new SqlParameter("@id", id));
            Database.Instance.Open();
            // recuperer les resuletat de la requete
            reader = command.ExecuteReader();
            if (reader.Read())
            {
                etBill = true;
            }
            command.Dispose();
            Database.Instance.Close();

            if (etBill)
            {
                Database.Instance.Open();
                command = new SqlCommand("DELETE FROM login WHERE id_personne=@id", Database.Instance);
                command.Parameters.Add(new SqlParameter("@id", id));
                command.ExecuteNonQuery();
                command.Dispose();

                command = new SqlCommand("DELETE FROM personne WHERE id=@id", Database.Instance);
                command.Parameters.Add(new SqlParameter("@id", id));
                
                // permet d'executer la requete et retourne le nombre de row affectées
                command.ExecuteNonQuery();
                command.Dispose();
                Database.Instance.Close();
            }

            return etBill;
        }
       

        public static (Personne,bool) Login(string login, string mdp) 
        {
            command = new SqlCommand("SELECT login, password,id_personne,nom,prenom,num_user,role FROM login,personne WHERE login=@login AND password=@mdp AND id_personne=personne.id", Database.Instance);
            command.Parameters.Add(new SqlParameter("@login", login));
            command.Parameters.Add(new SqlParameter("@mdp", mdp));
            Database.Instance.Open();
            reader = command.ExecuteReader();
            Personne u = new Personne();
            if (reader.Read())
            {
                u = new Personne
                {
                    Id = reader.GetInt32(2),
                    Nom = reader.GetString(3),
                    Prenom = reader.GetString(4),
                    NumUser = reader.GetInt32(5),
                    UserType = (UserTypes_Enum)reader.GetInt16(6)
                };
                isLogged = true;

            }
            command.Dispose();
            Database.Instance.Close();
            return (u, isLogged);
            
        }
    }
}
