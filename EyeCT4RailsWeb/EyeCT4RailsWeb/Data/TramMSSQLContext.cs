using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyeCT4RailsWeb.Models;
using System.Data.SqlClient;

namespace EyeCT4RailsWeb.Data
{
    public class TramMSSQLContext : ITramRepo
    {
        string connString = "Data Source=mssql.fhict.local;Initial Catalog=dbi344145;Integrated Security=False;User ID=dbi344145;Password=rails;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// Deze methode veranderd de tram van sector
        /// </summary>
        /// <param name="sector"></param>
        /// <param name="tram"></param>
        /// <returns>true als het gelukt is anders false</returns>
        public bool ChangeTramSector(Sector sector, Tram tram)
        {
            string query = "UPDATE SECTOR SET Tram_ID = null, Blokkade = 0 WHERE Tram_ID = @TramId";
            string query2 = "UPDATE SECTOR SET Tram_ID = @TramId WHERE Nummer = @Sectornummer AND Spoor_Nummer = @Sectorspoor";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TramId", tram.ID);

                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(query2, conn))
                {
                    cmd.Parameters.AddWithValue("@Sectorspoor", sector.Spoor);
                    cmd.Parameters.AddWithValue("@Sectornummer", sector.Nummer);
                    cmd.Parameters.AddWithValue("@TramId", tram.ID);


                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// deze methode geeft alle trams terug
        /// PRIORITEIT VERWIJDERD MOET NOG TERUG!
        /// </summary>
        /// <returns></returns>
        public List<Tram> GetTrams()
        {
            string query = "SELECT t.ID, tt.Omschrijving, t.Nummer, l.Nummer, s.Spoor_Nummer, s.Nummer, t.Status, t.Defect, t.Vervuild FROM TRAM t left join SECTOR s ON s.Tram_ID = t.ID, TRAMTYPE tt, TRAM_LIJN tl, LIJN l WHERE t.Tramtype_ID = tt.ID AND t.ID = tl.Tram_ID AND tl.Lijn_ID = l.ID";
            List<Tram> tramList = new List<Tram>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(4))
                            {
                                tramList.Add(new Tram(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                (Status)Enum.Parse(typeof(Status),
                                reader.GetString(6)),
                                reader.GetInt32(7),
                                reader.GetInt32(8)
                                ));
                            }
                            else
                            {
                                tramList.Add(new Tram(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                reader.GetInt32(5),
                                (Status)Enum.Parse(typeof(Status),
                                reader.GetString(6)),
                                reader.GetInt32(7),
                                reader.GetInt32(8)
                                ));
                            }

                            //int r4;
                            //int r5;
                            //if (reader.IsDBNull(4))
                            //{
                            //    r4 = 0;
                            //        }
                            //else
                            //{
                            //    r4 = reader.GetInt32(4);
                            //}
                            //if (reader.IsDBNull(5))
                            //{
                            //    r5 = 0;
                            //        }
                            //else
                            //{
                            //    r5 = reader.GetInt32(5);
                            //}
                            //tramList.Add(new Tram(reader.GetInt32(0),
                            //    reader.GetString(1),
                            //    reader.GetInt32(2),
                            //    (Status)Enum.Parse(typeof(Status),
                            //    reader.GetString(3)),
                            //    r4,
                            //    r5
                            //    ));                       
                        }
                        return tramList;
                    }
                }
            }
        }

        /// <summary>
        /// deze methode past de status van een tram aan en update ook "StatusAangepast" in de database
        /// </summary>
        /// <param name="status">status van de tram</param>
        /// <param name="tram"></param>
        /// <returns>als het lukt returned het true anders false</returns>
        public bool ChangeStatusTrams(string status, Tram tram)
        {
            string query = "UPDATE TRAM SET Status = @status WHERE ID = @tramId;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@tramId", tram.ID);

                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Deze methode past de schoonmaak prioriteit aan van een tram
        /// </summary>
        /// <param name="prioriteit">gewenste prioriteit</param>
        /// <param name="tram">tram die je wilt updaten</param>
        /// <returns>returned true als het lukt, false als het niet lukt</returns>
        public bool ChangeSchoonmaakPrioriteitTram(int prioriteit, Tram tram)
        {
            string query = "UPDATE TRAM SET Vervuild = @prioriteit WHERE ID = @tramId;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@prioriteit", prioriteit);
                    cmd.Parameters.AddWithValue("@tramId", tram.ID);

                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }

                }
            }

            return false;
        }

        /// <summary>
        /// Deze methode past de reparatie prioriteit aan van een tram
        /// </summary>
        /// <param name="prioriteit">gewenste prioriteit</param>
        /// <param name="tram">tram die je wilt updaten</param>
        /// <returns></returns>
        public bool ChangeReparatiePrioriteitTram(int prioriteit, Tram tram)
        {
            string query = "UPDATE TRAM SET Defect = @prioriteit WHERE ID = @tramId;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@prioriteit", prioriteit);
                    cmd.Parameters.AddWithValue("@tramId", tram.ID);

                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Deze methode update de laatste GROTE schoonmaak datum van een tram aan naar de datum van nu
        /// </summary>
        /// <param name="tram">tram die je wilt updaten</param>
        /// <returns></returns>
        public bool UpdateLaatsteGroteSchoonmaak(Tram tram)
        {
            string query = "UPDATE TRAM SET LaatsteGroteSchoonmaak = GETDATE() WHERE ID = @tramId;";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tramId", tram.ID);

                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Deze methode update de laatste KLEINE schoonmaak datum van een tram aan naar de datum van nu
        /// </summary>
        /// <param name="tram">tram die je wilt updaten</param>
        /// <returns></returns>
        public bool UpdateLaatsteKleineSchoonmaak(Tram tram)
        {
            string query = "UPDATE TRAM SET LaatsteKleineSchoonmaak = GETDATE() WHERE ID = @tramId";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tramId", tram.ID);

                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public List<Tram> GetSchoonmaakTrams()
        {
            string query = "SELECT t.ID, t.Nummer, tt.Omschrijving FROM Tram t JOIN Tramtype tt ON tt.ID = t.Tramtype_ID where t.Status = 'SCHOONMAAK';";
            List<Tram> tramList = new List<Tram>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tramList.Add(new Tram(reader.GetInt32(0),
                                                  reader.GetInt32(1),
                                                  reader.GetString(2)));
                        }
                        return tramList;
                    }
                }
            }
        }

        public List<Tram> GetReparartieTrams()
        {
            string query = "SELECT t.ID, t.Nummer, tt.Omschrijving FROM Tram t JOIN Tramtype tt ON tt.ID = t.Tramtype_ID where t.Status = 'DEFECT'";
            List<Tram> tramList = new List<Tram>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tramList.Add(new Tram(reader.GetInt32(0),
                                                  reader.GetInt32(1),
                                                  reader.GetString(2)));
                        }
                        return tramList;
                    }
                }
            }
        }

        /// <summary>
        /// Haalt de tram op met de gegeven nummer
        /// </summary>
        /// <param name="ID">tram nummer</param>
        /// <returns></returns>
        public Tram GetTramByID(int nummer)
        {
            string query = "SELECT t.ID, tt.Omschrijving, t.Nummer, l.Nummer, s.Spoor_Nummer, s.Nummer, t.Status, t.Defect, t.Vervuild FROM TRAM t left join SECTOR s ON s.Tram_ID = t.ID, TRAMTYPE tt, TRAM_LIJN tl, LIJN l WHERE t.Tramtype_ID = tt.ID AND t.ID = tl.Tram_ID AND tl.Lijn_ID = l.ID AND t.Nummer = @nummer";
            Tram tram = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nummer", nummer);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(4))
                            {
                                tram = new Tram(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                (Status)Enum.Parse(typeof(Status),
                                reader.GetString(6)),
                                reader.GetInt32(7),
                                reader.GetInt32(8)
                                );
                            }
                            else
                            {
                                tram = new Tram(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                reader.GetInt32(4),
                                reader.GetInt32(5),
                                (Status)Enum.Parse(typeof(Status),
                                reader.GetString(6)),
                                reader.GetInt32(7),
                                reader.GetInt32(8)
                                );
                            }
                        }
                        return tram;
                    }
                }
            }
        }

        /// <summary>
        /// verwijdert een tram van een sector met de gegeven tramnummer
        /// </summary>
        /// <param name="TramID">tramnummer</param>
        /// <returns></returns>
        public bool LeaveRemise(int Tramnummer)
        {
            string query = "UPDATE SECTOR SET Tram_ID = null WHERE Tram_ID = (select t.ID from TRAM t where t.Nummer = @tramnummer)";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@tramnummer", Tramnummer);

                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }
                    return false;
                }
            }
        }
    }
}