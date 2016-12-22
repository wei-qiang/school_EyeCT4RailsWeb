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
            string query = "UPDATE TRAM SET Sector_ID = @SectorId WHERE ID = @TramId";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@SectorId", sector.ID);
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
            string query = "SELECT t.ID, tt.Omschrijving, t.nummer, t.Status, s.nummer, s.Spoor_Nummer FROM TRAM t LEFT JOIN Sector s ON s.Tram_ID = t.ID JOIN TRAMTYPE tt ON t.Tramtype_ID = tt.ID;";
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
                            int r4;
                            int r5;
                            if (reader.IsDBNull(4))
                            {
                                r4 = 0;
                                    }
                            else
                            {
                                r4 = reader.GetInt32(4);
                            }
                            if (reader.IsDBNull(5))
                            {
                                r5 = 0;
                                    }
                            else
                            {
                                r5 = reader.GetInt32(5);
                            }
                            tramList.Add(new Tram(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                (Status)Enum.Parse(typeof(Status),
                                reader.GetString(3)),
                                r4,
                                r5
                                ));                       
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
            string query = "SELECT t.ID, tt.Naam FROM Tram t JOIN Tramtype tt ON tt.ID = t.Tramtype_ID WHERE t.Status = 'SCHOONMAAK';";
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
                                                  reader.GetString(1)));
                        }
                        return tramList;
                    }
                }
            }
        }

        public List<Tram> GetReparartieTrams()
        {
            string query = "SELECT t.ID, tt.Naam FROM TRAM t JOIN tramtype tt ON tt.ID = tramtype_ID  WHERE t.status = 'DEFECT';";
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
                                                  reader.GetString(1)));
                        }
                        return tramList;
                    }
                }
            }
        }

        public Tram GetTramByID(int ID)
        {
            string query = "SELECT t.ID, tt.Naam, tl.Lijn_ID, t.Sector_ID, t.Status, t.PrioriteitReparatie, t.PrioriteitSchoonmaak FROM TRAM t, TRAMTYPE tt, TRAM_LIJN tl WHERE t.Tramtype_ID = tt.ID AND t.ID = tl.Tram_ID AND t.ID = @tramID;";
            Tram tram = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@tramID", ID);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            if (reader.IsDBNull(3))
                            {
                                tram = new Tram(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                (Status)Enum.Parse(typeof(Status),
                                reader.GetString(4))//,
                                //reader.GetInt32(5),
                                //reader.GetInt32(6)
                                );
                            }
                            else
                            {
                                tram = new Tram(reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetInt32(2),
                                reader.GetInt32(3),
                                (Status)Enum.Parse(typeof(Status),
                                reader.GetString(4))//,
                                //reader.GetInt32(5),
                                //reader.GetInt32(6)
                                );
                            }
                        }
                        return tram;
                    }
                }
            }
        }

        public bool LeaveRemise(int TramID)
        {
            string query = "UPDATE TRAM SET Sector_ID = null WHERE ID = @TramId";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@TramId", TramID);

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