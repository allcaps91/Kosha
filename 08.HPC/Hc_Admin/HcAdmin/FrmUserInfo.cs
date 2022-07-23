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

namespace HcAdmin
{
    public partial class FrmUserInfo : Form
    {
        public FrmUserInfo()
        {
            InitializeComponent();
        }

        private void FrmUserInfo_Load(object sender, EventArgs e)
        {
            Data_Select_all();
        }

        //전체 자료 검색
        private void Data_Select_all()
        {
            string SQL = "";
            DataTable dt = null;
            string strNEW = "";
            string strOLD = "";
            string strNewVer = "";
            string strToday = DateTime.Now.ToString("yyyy-MM-dd");

            int nCnt1 = 0;
            int nCnt2 = 0;
            int nCnt3 = 0;
            int nCnt4 = 0;
            int nRow = 0;

            //서버에서 현재 버전정보를 읽음
            strNewVer = GET_NewVersion();

            SS1_Sheet1.RowCount = 0;
            SS2_Sheet1.RowCount = 0;
            SS3_Sheet1.RowCount = 0;

            strOLD = "";
            strNEW = "";
            nCnt1 = 0;
            nCnt2 = 0;
            nCnt3 = 0;
            nCnt4 = 0;

            SS1_Sheet1.RowCount = 0;
            SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            SQL = "SELECT * FROM pc_master ";
            SQL = SQL + "ORDER BY LICNO,LASTDATE DESC ";
            dt = clsDbMySql.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                SS2_Sheet1.RowCount = dt.Rows.Count;
                SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SS2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MAC"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["LICNO"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["VER"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IP"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["STARTDATE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["LASTDATE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WINVER"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                    strNEW = dt.Rows[i]["LICNO"].ToString().Trim();
                    if (strOLD == "") strOLD = strNEW;
                    if (strOLD != strNEW)
                    {
                        nRow++;
                        SS1_Sheet1.RowCount = nRow;
                        SS1_Sheet1.Cells[nRow - 1, 0].Text = strOLD;
                        SS1_Sheet1.Cells[nRow - 1, 1].Text = READ_LicName(strOLD);
                        SS1_Sheet1.Cells[nRow - 1, 2].Text = nCnt1.ToString();
                        SS1_Sheet1.Cells[nRow - 1, 3].Text = nCnt2.ToString();
                        SS1_Sheet1.Cells[nRow - 1, 4].Text = nCnt3.ToString();
                        SS1_Sheet1.Cells[nRow - 1, 5].Text = nCnt4.ToString();

                        strOLD = strNEW;
                        nCnt1 = 0;
                        nCnt2 = 0;
                        nCnt3 = 0;
                        nCnt4 = 0;
                    }
                    nCnt1++;
                    if (VB.Left(dt.Rows[i]["LASTDATE"].ToString().Trim(), 10) == strToday)
                    {
                        nCnt2++;
                    }
                    else
                    {
                        nCnt3++;
                    }
                    if (dt.Rows[i]["VER"].ToString().Trim()!=strNewVer) nCnt4++;
                }

                nRow++;
                SS1_Sheet1.RowCount = nRow;
                SS1_Sheet1.Cells[nRow - 1, 0].Text = strOLD;
                SS1_Sheet1.Cells[nRow - 1, 1].Text = READ_LicName(strOLD);
                SS1_Sheet1.Cells[nRow - 1, 2].Text = nCnt1.ToString();
                SS1_Sheet1.Cells[nRow - 1, 3].Text = nCnt2.ToString();
                SS1_Sheet1.Cells[nRow - 1, 4].Text = nCnt3.ToString();
                SS1_Sheet1.Cells[nRow - 1, 5].Text = nCnt4.ToString();
            }

            dt.Dispose();
            dt = null;

            //
            // 5일의 사용자 로그를 보여 줌
            //
            string strGDate = DateTime.Now.AddDays(-5).ToString("yyyy-MM-dd");

            SQL = "SELECT * FROM pc_log ";
            SQL = SQL + "WHERE SENDTIME>='" + strGDate + "' ";
            SQL = SQL + "ORDER BY SENDTIME DESC ";
            dt = clsDbMySql.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                SS3_Sheet1.RowCount = dt.Rows.Count;
                SS3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SS3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["LICNO"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MAC"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IP"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SENDLOG"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;

        }

        //현재 서버의 버전정보를 읽음
        private string GET_NewVersion()
        {
            string SQL = "";
            DataTable dt1 = null;
            string strNewVer = "";

            try
            {
                SQL = "";
                SQL = "SELECT * FROM LICMSG ";

                dt1 = clsDbMySql.GetDataTable(SQL);
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i]["Gubun"].ToString() == "1") strNewVer = dt1.Rows[i]["Remark"].ToString();
                }
                dt1.Dispose();
                dt1 = null;
                return strNewVer;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                ComFunc.MsgBox(ex.Message);
                return "";
            }

        }

        //라이선스 회사명을 읽음
        private string READ_LicName(string argCode)
        {
            string SQL = "";
            DataTable dt1 = null;
            string strReturn = "";

            SQL = "SELECT Sangho FROM LICMST ";
            SQL = SQL + "WHERE LicNo='" + argCode + "' ";
            dt1 = clsDbMySql.GetDataTable(SQL);
            if (dt1.Rows.Count>0) strReturn = dt1.Rows[0]["Sangho"].ToString();
            dt1.Dispose();
            dt1 = null;
            return strReturn;
        }

        private void SS1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string strToday = DateTime.Now.ToString("yyyy-MM-dd");
            string strLicno = SS1.ActiveSheet.Cells[e.Row, 0].Value.ToString();

            SS2_Sheet1.RowCount = 0;
            SS3_Sheet1.RowCount = 0;

            SQL = "SELECT * FROM pc_master ";
            SQL = SQL + "WHERE LICNO='" + strLicno + "' ";
            SQL = SQL + "ORDER BY LASTDATE DESC ";
            dt = clsDbMySql.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                SS2_Sheet1.RowCount = dt.Rows.Count;
                SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SS2_Sheet1.Cells[i, 0].Text = dt.Rows[i]["MAC"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 1].Text = dt.Rows[i]["LICNO"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 2].Text = dt.Rows[i]["VER"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IP"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 4].Text = dt.Rows[i]["STARTDATE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 5].Text = dt.Rows[i]["LASTDATE"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 6].Text = dt.Rows[i]["WINVER"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    SS2_Sheet1.Cells[i, 8].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                }
            }

            dt.Dispose();
            dt = null;

            //
            // 20일의 사용자 로그를 보여 줌
            //
            string strGDate = DateTime.Now.AddDays(-20).ToString("yyyy-MM-dd");

            SQL = "SELECT * FROM pc_log ";
            SQL = SQL + "WHERE SENDTIME>='" + strGDate + "' ";
            SQL = SQL + "  AND LICNO='" + strLicno + "' ";
            SQL = SQL + "ORDER BY SENDTIME DESC ";
            dt = clsDbMySql.GetDataTable(SQL);
            if (dt.Rows.Count > 0)
            {
                SS3_Sheet1.RowCount = dt.Rows.Count;
                SS3_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SS3_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SENDTIME"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 1].Text = dt.Rows[i]["LICNO"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 2].Text = dt.Rows[i]["MAC"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 3].Text = dt.Rows[i]["IP"].ToString().Trim();
                    SS3_Sheet1.Cells[i, 4].Text = dt.Rows[i]["SENDLOG"].ToString().Trim();
                }
            }
            dt.Dispose();
            dt = null;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            Data_Select_all();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string SQL = string.Empty;
            bool SqlErr;

            string strOld = "";
            string strNew = "";
            string strMac = "";
            int nCnt = 0;

            for (int i=0; i<SS2_Sheet1.RowCount; i++)
            {
                strMac = SS2_Sheet1.Cells[i, 0].Text.Trim();
                strNew = SS2_Sheet1.Cells[i, 7].Text.Trim();
                strOld = SS2_Sheet1.Cells[i, 8].Text.Trim();
                if (strNew != strOld)
                {
                    nCnt++;
                    SS2_Sheet1.Cells[i, 8].Text = strNew;

                    SQL = "UPDATE pc_master SET ";
                    SQL += "      Remark='" + strNew + "' ";
                    SQL += "WHERE MAC = '" + strMac + "'";
                    SqlErr = clsDbMySql.ExecuteNonQuery(SQL);
                    if (SqlErr == false)
                    {
                        ComFunc.MsgBox("참고사항 변경 실패", "알림");
                        return;
                    }
                }
            }
            if (nCnt>0) ComFunc.MsgBox("변경 완료", "알림");
        }

        private void SS2_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            bool SqlErr;
            DataTable dt = null;
            string strMac = "";

            if (e.Column != 0) return;

            strMac = SS2.ActiveSheet.Cells[e.Row, 0].Value.ToString();
            if (ComFunc.MsgBoxQ(strMac + " PC정보를 삭제하시겠습니까") == DialogResult.No) return;

            SQL = "DELETE FROM pc_master ";
            SQL += "WHERE MAC = '" + strMac + "'";
            SqlErr = clsDbMySql.ExecuteNonQuery(SQL);
            if (SqlErr == false)
            {
                ComFunc.MsgBox("PC정보 삭제 실패", "알림");
                return;
            }

            Data_Select_all();
            ComFunc.MsgBox("PC정보 삭제 완료", "알림");
        }
    }
}
