using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyeCT4RailsWeb.Models;
using System.Data.SqlClient;

namespace EyeCT4RailsWeb.Data
{
    public class GebruikerMSSQLContext : IGebruiker
    {
        string connstring = "Data Source=mssql.fhict.local;Initial Catalog=dbi344145;Integrated Security=False;User ID=dbi344145;Password=rails;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        /// <summary>
        /// This method sends the username and password to the database. When the database finds
        /// the corresponding user, it returns its data. When no user is found, it returns null.
        /// </summary>
        /// <param name="gebruikersnaam">The username of the user.</param>
        /// <param name="wachtwoord">The user's password.</param>
        /// <returns>When user is found, it returrns its data, when no user is found it returns null.</returns>
        public Gebruiker Verificatie(string gebruikersnaam, string wachtwoord)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();

                string query = "SELECT m.Naam, f.Naam FROM MEDEWERKER m, FUNCTIE f WHERE m.Functie_ID = f.ID AND m.Naam = @Gebruikersnaam AND m.Wachtwoord = @Wachtwoord";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Gebruikersnaam", gebruikersnaam);
                    command.Parameters.AddWithValue("@Wachtwoord", wachtwoord);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            return CreateGebruikerFromReader(reader);
                        }
                    }
                }
            }
            return null;
        }

        public Gebruiker CreateGebruikerFromReader(SqlDataReader reader)
        {
            return new Gebruiker(
                Convert.ToString(reader["Gebruikersnaam"]),
                Convert.ToString(reader["Naam"]),
                (Functie)Enum.Parse(typeof(Functie), (Convert.ToString(reader["Functie"]))));
        }

        public bool GebruikerToevoegen(string gebruikersnaam, string wachtwoord, string functie)
        {
            using (SqlConnection connection = new SqlConnection(connstring))
            {
                connection.Open();

                string query = "INSERT INTO MEDEWERKER(Naam, Wachtwoord, Functie_ID) VALUES('@Gebruikersnaam', '@Wachtwoord', (SELECT f.ID from FUNCTIE f where f.Naam = '@Functie'))";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Gebruikersnaam", gebruikersnaam);
                    command.Parameters.AddWithValue("@Wachtwoord", wachtwoord);
                    command.Parameters.AddWithValue("@Functie", functie);

                    try
                    {
                        command.ExecuteNonQuery();
                        return true;
                    }
                    catch (SqlException e)
                    {
                        if (e.Number == 2627)
                        {
                            throw;
                        }

                        throw;
                    }
                }
            }
        }
    }
}