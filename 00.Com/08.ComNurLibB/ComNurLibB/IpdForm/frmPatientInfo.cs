using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    public partial class frmPatientInfo : Form
    {
        string strIpdNo = "";
        string strPaNo = "";

        public frmPatientInfo()
        {
            InitializeComponent();
        }

        private void frmPatientInfo_Load(object sender, EventArgs e)
        {
            
        }

        public void rGetDate(string strPano, string strIpdno)
        {
            strIpdNo = strIpdno;
            strPaNo = strPano;

            //통증
            if (clsNurse.READ_PAIN_RESTART(clsDB.DbCon, strIpdno, strPano) == "OK")
            {
                lblPW3.BackColor = Color.PaleGreen;
            }
            else
            {
                lblPW3.BackColor = SystemColors.Control;
            }

            //항암제
            if(clsNurse.READ_JUSAMIX(clsDB.DbCon, strPano) == "OK")
            {
                lblPW5.BackColor = Color.PaleGreen;
            }
            else
            {
                lblPW5.BackColor = SystemColors.Control;
            }

            //중심정맥관
            if (clsNurse.READ_CENTRAL_CATH(clsDB.DbCon, strIpdno) == "OK")
            {
                lblPW6.BackColor = Color.PaleGreen;
            }
            else
            {
                lblPW6.BackColor = SystemColors.Control;
            }
                
            //ADR
            if(clsNurse.READ_ADR(clsDB.DbCon, strIpdno) != "")
            {
                lblPW7.BackColor = Color.PaleGreen;
            }
            else
            {
                lblPW7.BackColor = SystemColors.Control;
            }

            //DNR
            if (clsNurse.READ_DNR(clsDB.DbCon, strIpdno) != "")
            {
                lblPW9.BackColor = Color.PaleGreen;
            }
            else
            {
                lblPW9.BackColor = SystemColors.Control;
            }

        }

        private void lblPW3_DoubleClick(object sender, EventArgs e)
        {
            if (strIpdNo == "" || strPaNo == "") return;

            ComFunc.MsgBox(READ_DETAIL_PAIN());
        }

        private string READ_DETAIL_PAIN()
        {
            string strTemp = "";
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            try
            {                
                SQL = "";
                SQL = " SELECT CYCLE, REGION, ASPECT, DETERIORATION, ";
                SQL = SQL + ComNum.VBLF + "  MITIGATION, SCORE, TOOLS, DURATION, ";
                SQL = SQL + ComNum.VBLF + "  DRUG, NODRUG, TIMES";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_PAIN_SCALE";
                SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + strIpdNo;
                SQL = SQL + ComNum.VBLF + "   AND PANO = '" + strPaNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE = TRUNC(SYSDATE)    ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY ACTDATE DESC, ACTTIME DESC";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return strTemp;
                }
                
                if (dt.Rows.Count > 0)
                {
                    strTemp = strTemp + " => 평가주기 : " + dt.Rows[0]["CYCLE"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 통증위치 : " + dt.Rows[0]["REGION"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 통증양상 : " + dt.Rows[0]["ASPECT"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 악화요인 : " + dt.Rows[0]["DETERIORATION"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 완화요인 : " + dt.Rows[0]["MITIGATION"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 통증강도 : " + dt.Rows[0]["SCORE"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 평가도구 : " + dt.Rows[0]["TOOLS"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 통증빈도 : " + dt.Rows[0]["DURATION"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 지속시간 : " + dt.Rows[0]["TIMES"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 중재-약물 : " + dt.Rows[0]["DRUG"].ToString().Trim() + ComNum.VBLF;
                    strTemp = strTemp + " => 중재-비약물 : " + dt.Rows[0]["NODRUG"].ToString().Trim() + ComNum.VBLF;                    
                }

                dt.Dispose();
                dt = null;
                return strTemp;
            }
            catch (Exception ex)
            {
                dt.Dispose();
                dt = null;
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return strTemp;
            }
        }

        private void lblPW9_DoubleClick(object sender, EventArgs e)
        {
            frmDNR frmDNRX = new frmDNR(strIpdNo, "", strPaNo);
            frmDNRX.StartPosition = FormStartPosition.CenterParent;
            frmDNRX.ShowDialog();
            clsNurse.setClearMemory(frmDNRX);
        }
    }
}
