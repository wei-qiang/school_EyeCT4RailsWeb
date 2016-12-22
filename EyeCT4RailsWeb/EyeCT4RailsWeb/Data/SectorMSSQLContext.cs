using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EyeCT4RailsWeb.Models;
using System.Data.SqlClient;

namespace EyeCT4RailsWeb.Data
{
    public class SectorMSSQLContext : ISectorRepo
    {
        string connString = "Data Source=mssql.fhict.local;Initial Catalog=dbi344145;Integrated Security=False;User ID=dbi344145;Password=rails;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public bool BlockSector(int spoorid, int sectornummer, int blokkeren)
        {
            string query = "UPDATE SECTOR SET Blokkade = @blokkeren WHERE Spoor_ID = @spoorid AND Nummer = @sectornummer";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@blokkeren", blokkeren);
                    cmd.Parameters.AddWithValue("@spoorid", spoorid);
                    cmd.Parameters.AddWithValue("@sectornummer", sectornummer);

                    if (Convert.ToInt32(cmd.ExecuteNonQuery()) > 0)
                    {
                        return true;
                    }

                }
            }
            return false;
        }

        public List<Sector> GetAllSectors()
        {

            List<Sector> SectorList = new List<Sector>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                string query = "SELECT s.ID, s.Spoor_Nummer, s.Nummer, s.Blokkeren " +
                    "FROM SECTOR s, SPOOR sp WHERE s.Spoor_ID = sp.ID;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SectorList.Add(new Sector(reader.GetInt32(0),
                                                      reader.GetInt32(1),
                                                      reader.GetInt32(2),
                                                      Convert.ToBoolean(reader.GetInt32(3))));
                        }
                        return SectorList;
                    }
                }
            }
        }

        public Sector GetSectorByID(int spoornummer, int sectornummer)
        {
            string query = "SELECT s.ID, s.Spoor_Nummer, s.Nummer, s.Blokkeren FROM SECTOR s WHERE s.Spoor_Nummer = @spoornummer and s.Nummer = @sectornummer;";
            Sector sector = null;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@spoornummer", spoornummer);
                    cmd.Parameters.AddWithValue("@sectornummer", sectornummer);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sector = new Sector(reader.GetInt32(0),
                                                 reader.GetInt32(1),
                                                 reader.GetInt32(2),
                                                 Convert.ToBoolean(reader.GetInt32(3)));
                        }
                    }
                }
            }
            return sector;
        }

        public Sector GetSectorByLijn(int Lijn)
        {
            string query = "SELECT s.ID, s.Spoor_Nummer, s.Nummer, s.Blokkade FROM SECTOR s, SPOOR sp, LIJN l WHERE s.Spoor_Nummer = sp.Nummer AND sp.Lijn_ID = l.ID AND s.Blokkade = 0 AND l.Nummer = @lijn";
            Sector sector = null;

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@lijn", Lijn);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            sector = new Sector(reader.GetInt32(0),
                                                      reader.GetInt32(1),
                                                      reader.GetInt32(2),
                                                      Convert.ToBoolean(reader.GetInt32(3)));
                            return sector;
                        }
                        return sector;
                    }
                }
            }
        }

        public List<Sector> GetSectorBySpoor(int spoor)
        {
            string query = "SELECT s.ID, s.Spoor_Nummer, s.Nummer, s.Blokkade FROM SECTOR s, SPOOR sp WHERE s.Spoor_Nummer = sp.Nummer AND sp.Nummer = @spoor;";
            List<Sector> SectorList = new List<Sector>();

            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@spoor", spoor);
                    conn.Open();

                    cmd.Parameters.AddWithValue("@spoorId", spoor);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SectorList.Add(new Sector(reader.GetInt32(0),
                                                      reader.GetInt32(1),
                                                      reader.GetInt32(2),
                                                      Convert.ToBoolean(reader.GetInt32(3))));
                        }
                        return SectorList;
                    }
                }
            }
        }

        public int GetSpoorLijn(int spoor)
        {
            string query = "SELECT sl.Lijn_ID FROM SPOOR_LIJN sl WHERE sl.Spoor_ID = @spoorID;";
            int lijn = 0;
            using (SqlConnection conn = new SqlConnection(connString))
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@spoorID", spoor);
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lijn = (reader.GetInt32(0));
                        }
                    }
                }
            }
            return lijn;
        }
    }
}