using System;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace _03232020_REQ
{
    public partial class FormSinhVien : Form
    {

        public FormSinhVien()
        {
            InitializeComponent();
            LoadSinhVienToControl();
        }

        private void Themsinhvienbtn_Click(object sender, EventArgs e)
        {
            SinhvienDBMng.Themsinhvien(GetXDoc());
            LoadSinhVienToControl();
        }

        private void Suasinhvienbtn_Click(object sender, EventArgs e)
        {
            SinhvienDBMng.Suasinhvien(GetXDoc());
            LoadSinhVienToControl();
        }

        private void Xoasinhvienbtn_Click(object sender, EventArgs e)
        {
            SinhvienDBMng.Xoasinhvien(GetXDoc());
            LoadSinhVienToControl();
        }

        private void LoadSinhVienToControl()
        {
            SinhvienView.Rows.Clear();
            XmlDocument XmlSinhVien_View = SinhvienDBMng.GetDanhsachSinvien();
            XPathNavigator nav = XmlSinhVien_View.CreateNavigator();
            XPathNodeIterator nodes = nav.Select("//LINE");
            int r = 0;
            foreach (XPathNavigator v in nodes)
            {
                SinhvienView.Rows.Add();
                SinhvienView.Rows[r].Cells["SinhvienID"].Value = v.SelectSingleNode("@SinhvienID").Value.ToString();
                SinhvienView.Rows[r].Cells["SinhvienPrkID"].Value = v.SelectSingleNode("@SinhvienPrkID").Value.ToString();
                SinhvienView.Rows[r].Cells["SinhvienName"].Value = v.SelectSingleNode("@SinhvienName").Value.ToString();
                SinhvienView.Rows[r].Cells["SinhvienAddr"].Value = v.SelectSingleNode("@SinhvienAddr").Value.ToString();
                ++r;
            }
        }

        private void SinhvienView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SinhvienIDtb.Text = SinhvienView.Rows[e.RowIndex].Cells["SinhvienID"].Value.ToString();
            SinhvienNametb.Text = SinhvienView.Rows[e.RowIndex].Cells["SinhvienName"].Value.ToString();
            SinhvienPrkIDtb.Text = SinhvienView.Rows[e.RowIndex].Cells["SinhvienPrkID"].Value.ToString();
            SinhvienAddrtb.Text = SinhvienView.Rows[e.RowIndex].Cells["SinhvienAddr"].Value.ToString();
        }

        private XmlDocument GetXDoc()
        {
            XDocument xDoc = new XDocument(new XElement("BIZREQUEST",
                                    new XElement("DATAAREA",
                                        new XElement("VOUCHERS"))));
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xDoc.CreateReader());
            XPathNavigator nav = xmlDoc.CreateNavigator();
            XElement header = new XElement("HEADER",
                 new XAttribute("SinhvienPrkID", SinhvienPrkIDtb.Text),
                 new XAttribute("SinhvienID", SinhvienPrkIDtb.Text),
                 new XAttribute("SinhvienName", SinhvienNametb.Text),
                 new XAttribute("SinhvienAddr", SinhvienAddrtb.Text),
                 new XAttribute("SinhvienEmail", SinhvienEmailtb.Text),
                 new XAttribute("SinhvienPhone", SinhvienPhonetb.Text));
            nav.SelectSingleNode("//VOUCHERS").AppendChild(header.CreateReader());

            return xmlDoc;
        }
    }
}
