using ComBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class frmCertView : Form
    {
        ComFunc CF = new ComFunc();
        clsPublic cpublic = new clsPublic();

        string SQL = "";
        string SqlErr = "";
        int intRowAffected = 0;

        public frmCertView()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string strPano = "";
            string strJong = "";
            string strData = "";
            string strCertData = "";
            string strUseId = "";
            string strDay = "";
            string strBigo = "";

            Cursor.Current = Cursors.WaitCursor;

            SQL = "  WITH CERT_DATA AS  " + ComNum.VBLF;
            SQL += " ( " + ComNum.VBLF;
            SQL += "  SELECT CERTNO, PANO, EMRDATA, CERTDATA " + ComNum.VBLF;
            SQL += "  FROM KOSMOS_PMPA.BAS_CERTPOOL_" + VB.Replace(cpublic.strSysDate, "-", "") + " " + ComNum.VBLF;
            SQL += "  WHERE TABLE_NAME = 'AEMRCHARTMST'" + ComNum.VBLF;
            SQL += " ) " + ComNum.VBLF;
            SQL += " ,CHART_DATA AS " + ComNum.VBLF;
            SQL += " ( " + ComNum.VBLF;
            SQL += "  SELECT /*+ INDEX(AEMRCHARTMST AEMRCHARTMSTHIS_IDX_MIBI)*/  " + ComNum.VBLF;
            SQL += "    A.FORMNO, A.UPDATENO, EMRNO, F.FORMNAME, A.PTNO, A.CERTDATE, A.CHARTUSEID, A.CERTNO   " + ComNum.VBLF;
            SQL += "  , (SELECT EMP_NM FROM KOSMOS_ERP.HR_EMP_BASIS WHERE EMP_ID = A.CHARTUSEID) AS USER_NAME " + ComNum.VBLF;
            SQL += "  , C.CERTDATA AS CERT_DATA                 " + ComNum.VBLF;
            SQL += "  , C.EMRDATA  AS EMR_DATA                  " + ComNum.VBLF;
            SQL += "   FROM KOSMOS_EMR.AEMRCHARTMST A " + ComNum.VBLF;
            SQL += "   LEFT OUTER JOIN KOSMOS_EMR.AEMRFORM F " + ComNum.VBLF;
            SQL += "     ON A.FORMNO = F.FORMNO " + ComNum.VBLF;
            SQL += "    AND A.UPDATENO = F.UPDATENO " + ComNum.VBLF;
            SQL += "   LEFT OUTER JOIN CERT_DATA C " + ComNum.VBLF;
            SQL += "     ON A.CERTNO = C.CERTNO " + ComNum.VBLF;
            SQL += "    AND C.PANO = A.PTNO" + ComNum.VBLF;
            SQL += "    AND C.CERTDATA IS NOT NULL  " + ComNum.VBLF;
            SQL += "  WHERE A.WRITEDATE >= '20200422' " + ComNum.VBLF;
            SQL += "    AND A.WRITEDATE <= TO_CHAR(SYSDATE, 'YYYYMMDD') " + ComNum.VBLF;
            SQL += "    AND A.CERTDATE = TO_CHAR(SYSDATE, 'YYYYMMDD') " + ComNum.VBLF;
            SQL += " )" + ComNum.VBLF;
            SQL += " SELECT A.PTNO,A.FORMNAME,A.EMR_DATA,A.CERT_DATA,A.CHARTUSEID,A.CERTDATE,' ' " + ComNum.VBLF;
            SQL += "   FROM CHART_DATA A" + ComNum.VBLF;

            SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                ssList_Sheet1.DataSource = dt;

                ssList_Sheet1.Columns[0].Width = 78;
                ssList_Sheet1.Columns[1].Width = 160;
                ssList_Sheet1.Columns[2].Width = 457;
                ssList_Sheet1.Columns[3].Width = 331;
                ssList_Sheet1.Columns[4].Width = 74;
                ssList_Sheet1.Columns[5].Width = 107;
                ssList_Sheet1.Columns[6].Width = 89;
                ssList_Sheet1.Rows[-1].Height = 25;                
            }

            dt.Dispose();
            dt = null;

            ComFunc.MsgBox("조회 완료", "확인");

            Cursor.Current = Cursors.Default;
        }

        private void frmCertView_Load(object sender, EventArgs e)
        {
            read_sysdate();

            lblDate.Text = "조회일자 : " + cpublic.strSysDate;

            ssList_Sheet1.Rows.Count = 0;
        }

        void read_sysdate()
        {
            //
            cpublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cpublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }
    }
}
