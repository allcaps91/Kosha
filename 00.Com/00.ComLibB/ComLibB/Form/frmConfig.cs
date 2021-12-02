using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        void frmConfig_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strEmrViewPos = "";

            //TODO
            //strEmrViewPos = gGetSection("SCREEN", "DATECHK", "", "C:\CMC\MTSCONFIG.INI");

            if (strEmrViewPos == "")
            {
                optM2.Checked = true;
                return;
            }

            if (Convert.ToInt16(strEmrViewPos) == 1)
            {
                optM1.Checked = true;
            }
            else
                optM2.Checked = true;
        }

        void btnSave_Click(object sender, EventArgs e)
        {
            string strEmrViewPos = "";

            if(optM1.Checked == true)
            {
                strEmrViewPos = "1";
            }
            else if(optM2.Checked == true)
            {
                strEmrViewPos = "2";
            }

            //TODO
            //Call gPutSection("SCREEN", "DATECHK", strEmrViewPos, "C:\CMC\MTSCONFIG.INI")
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
