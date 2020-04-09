using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace _03232020_REQ
{
    class SinhvienDBMng
    {
        static string connetionString = "Data Source=.;Initial Catalog=03232020_REQ;User ID=sa;Password=123";
        public static void Themsinhvien(XmlDocument xmlDoc)
        {
            XPathNavigator nav = xmlDoc.CreateNavigator();
            XPathNavigator headers = nav.SelectSingleNode("//HEADER");
            connetionString = "Data Source=.;Initial Catalog=03232020_REQ;User ID=sa;Password=123";
            using (SqlConnection cnn = new SqlConnection(connetionString))
            {
                cnn.Open();

                    string SinhvienPrkID = headers.SelectSingleNode("//@SinhvienPrkID").Value;
                    string SinhvienID = headers.SelectSingleNode("//@SinhvienID").Value;
                    string SinhvienName = headers.SelectSingleNode("//@SinhvienName").Value;
                    string SinhvienAddr = headers.SelectSingleNode("//@SinhvienAddr").Value;
                    string SinhvienEmail = headers.SelectSingleNode("//@SinhvienEmail").Value;
                    string SinhvienPhone = headers.SelectSingleNode("//@SinhvienPhone").Value;
                    string oString = @"insert into SinhVien 
                                    values(@SinhvienPrkID,@SinhvienID,@SinhvienName,@SinhvienAddr,@SinhvienEmail,@SinhvienPhone)";
                    SqlCommand oCmd = new SqlCommand(oString, cnn);
                    oCmd.Parameters.AddWithValue("@SinhvienPrkID", SinhvienPrkID);
                    oCmd.Parameters.AddWithValue("@SinhvienID", SinhvienID);
                    oCmd.Parameters.AddWithValue("@SinhvienName", SinhvienName);
                    oCmd.Parameters.AddWithValue("@SinhvienAddr", SinhvienAddr);
                    oCmd.Parameters.AddWithValue("@SinhvienEmail", SinhvienEmail);
                    oCmd.Parameters.AddWithValue("@SinhvienPhone", SinhvienPhone);

                oCmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public static void Suasinhvien(XmlDocument xmlDoc)
        {
            XPathNavigator nav = xmlDoc.CreateNavigator();
            XPathNavigator headers = nav.SelectSingleNode("//HEADER");
            string connetionString = null;
            connetionString = "Data Source=.;Initial Catalog=03232020_REQ;User ID=sa;Password=123";
            using (SqlConnection cnn = new SqlConnection(connetionString))
            {
                string SinhvienPrkID = headers.SelectSingleNode("//@SinhvienPrkID").Value;
                string SinhvienID = headers.SelectSingleNode("//@SinhvienID").Value;
                string SinhvienName = headers.SelectSingleNode("//@SinhvienName").Value;
                string SinhvienAddr = headers.SelectSingleNode("//@SinhvienAddr").Value;
                string SinhvienEmail = headers.SelectSingleNode("//@SinhvienEmail").Value;
                string SinhvienPhone = headers.SelectSingleNode("//@SinhvienPhone").Value;
                string oString = @"Update SinhVien set 
                                        SinhvienName = @SinhvienName, SinhvienAddr = @SinhvienAddr, SinhvienEmail = @SinhvienEmail
                                        ,SinhvienPhone = @SinhvienPhone
                                        where SinhvienID = @SinhvienID and SinhvienPrkID = @SinhvienPrkID";
                SqlCommand oCmd = new SqlCommand(oString, cnn);
                oCmd.Parameters.AddWithValue("@SinhvienPrkID", SinhvienPrkID);
                oCmd.Parameters.AddWithValue("@SinhvienID", SinhvienID);
                oCmd.Parameters.AddWithValue("@SinhvienName", SinhvienName);
                oCmd.Parameters.AddWithValue("@SinhvienAddr", SinhvienAddr);
                oCmd.Parameters.AddWithValue("@SinhvienEmail", SinhvienEmail);
                oCmd.Parameters.AddWithValue("@SinhvienPhone", SinhvienPhone);
                cnn.Open();
                oCmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public static void Xoasinhvien(XmlDocument xmlDoc)
        {
            XPathNavigator nav = xmlDoc.CreateNavigator();
            XPathNavigator headers = nav.SelectSingleNode("//HEADER");

            using (SqlConnection cnn = new SqlConnection(connetionString))
            {
                string SinhvienPrkID = headers.SelectSingleNode("//@SinhvienPrkID").Value;
                SqlCommand oCmd = new SqlCommand();
                oCmd.Connection = cnn;
                oCmd.CommandType = System.Data.CommandType.Text;
                oCmd.CommandTimeout = 300;
                string oString = String.Format("DELETE FROM SinhVien Where SinhvienPrkID = @SinhvienPrkID");
                oCmd.Parameters.AddWithValue("@SinhvienPrkID", SinhvienPrkID);
                oCmd.CommandText = oString;
                cnn.Open();
                oCmd.ExecuteNonQuery();
                cnn.Close();
            }
        }

        public static XmlDocument GetDanhsachSinvien()
        {
            XDocument xDoc = new XDocument(new XElement("BIZREQUEST",
                                                new XElement("DATAAREA",
                                                    new XElement("VOUCHERS",
                                                        new XElement("VLINES")))));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xDoc.CreateReader());
            XPathNavigator nav = xmlDoc.CreateNavigator();
            string connetionString = null;
            connetionString = "Data Source=.;Initial Catalog=03232020_REQ;User ID=sa;Password=123";
            using (SqlConnection cnn = new SqlConnection(connetionString))
            {
                string oString = @"select * from SinhVien";
                SqlCommand oCmd = new SqlCommand(oString, cnn);
                cnn.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        XElement line = new XElement("LINE", 
                                                        new XAttribute("SinhvienID", oReader["SinhvienID"]),
                                                        new XAttribute("SinhvienPrkID", oReader["SinhvienPrkID"]),
                                                        new XAttribute("SinhvienName", oReader["SinhvienName"]),
                                                        new XAttribute("SinhvienAddr", oReader["SinhvienAddr"])
                                                        );
                        nav.SelectSingleNode("//VLINES").AppendChild(line.CreateReader());
                    }
                    cnn.Close();
                }
            }
            return xmlDoc;
        }
    }
}
