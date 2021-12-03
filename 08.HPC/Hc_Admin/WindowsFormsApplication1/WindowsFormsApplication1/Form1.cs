using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            {
                //Enter your ADB's user id, password, and net service name
                string conString = "User Id=ADMIN;Password=QjelQjel!@12;Data Source=kosha_low;Connection Timeout=90;";

                //Enter directory where you unzipped your cloud credentials
                if (OracleConfiguration.TnsAdmin == null)
                {
                    OracleConfiguration.TnsAdmin = @"C:\Oracle\network\admin";
                    OracleConfiguration.WalletLocation = OracleConfiguration.TnsAdmin;
                }

                using (OracleConnection con = new OracleConnection(conString))
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        try
                        {
                            con.Open();

                            cmd.CommandText = "select GRPCDB,GRPCD,BASCD,APLFRDATE,APLENDDATE,BASNAME,BASNAME1,VFLAG1,VFLAG2, " +
                                            "VFLAG3,VFLAG4,NFLAG1,NFLAG2,NFLAG3,NFLAG4,REMARK,REMARK1,INPDATE,INPTIME,USECLS," +
                                            "DISPSEQ " +
                                "from BAS_BASCD ";
                            OracleDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                                Console.WriteLine(reader.GetString(0) + " " + reader.GetString(1) + " ");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
        }

    }
}

