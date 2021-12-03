using ComBase;
using FarPoint.Win.Spread;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ComEmrBase
{
    /// <summary>
    /// Class Name      : ComEmrBase
    /// File Name       : frmERPatient
    /// Description     : 응급실 내원환자 조회
    /// Author          : 이현종
    /// Create Date     : 2019-08-28
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso cref= " PSMH\mtsEmr(frmERPatient.frm) >> frmERPatient.cs 폼이름 재정의" />
    /// 
    public partial class frmERPatient : Form
    {
        //환자정보 전달
        public delegate void SendPatNo(string strPtNo);
        public event SendPatNo rSendPatInfo;

        public delegate void CloseEvent();
        public event CloseEvent rClosed;

        public frmERPatient()
        {
            InitializeComponent();
        }

        private void FrmERPatient_Load(object sender, EventArgs e)
        {
            dtpTDate.Value = Convert.ToDateTime(ComQuery.CurrentDateTime(clsDB.DbCon, "S"));
            dtpFDate.Value = dtpTDate.Value.AddDays(-1);


            cboPart.Items.Clear();

            cboPart.Items.Add("0.전체");
            cboPart.Items.Add("1.Day");
            cboPart.Items.Add("2.Night");
            cboPart.SelectedIndex = 0;

            btnSearch.PerformClick();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            GetSearhData();
        }


        void GetSearhData()
        {

            string SQL    = string.Empty;
            string SqlErr = string.Empty;
            DataTable dt = null;

            SSList_Sheet1.RowCount = 0;


            Cursor.Current = Cursors.WaitCursor;
            try
            {
                SQL = " SELECT Pano, SName, TO_CHAR(JTime,'YYYY-MM-DD') JDATE,         ";
                SQL += ComNum.VBLF + "Jin, Bunup, PNEUMONIA, ERPATIENT   ";
                SQL += ComNum.VBLF + "FROM KOSMOS_PMPA.OPD_MASTER K  ";
                SQL += ComNum.VBLF + "WHERE BDate BETWEEN TO_DATE('" + dtpFDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')  AND TO_DATE('" + dtpTDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                SQL += ComNum.VBLF + "AND DeptCode IN ('EM' ,'ER')      ";
                SQL += ComNum.VBLF + "AND Jin IN ('0','1','2','3','4','5','6','F','R','S')  ";
                SQL += ComNum.VBLF + "AND EXISTS (  ";
                SQL += ComNum.VBLF + "              SELECT a.Pano,b.SName,TO_CHAR(a.JDate,'YYYY-MM-DD') JDate,  ";
                SQL += ComNum.VBLF + "               TO_CHAR(a.EntDate,'YYYY-MM-DD HH24:MI') EntDate,a.EntSabun  ";
                SQL += ComNum.VBLF + "               FROM KOSMOS_PMPA.NUR_ER_PATIENT a, KOSMOS_PMPA.BAS_PATIENT b   ";

                if(chkCheck.Checked)
                {

                    SQL += ComNum.VBLF + "              WHERE a.JDate >= TO_DATE('" + dtpFDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')  AND a.JDATE <= TO_DATE('" + dtpTDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                    switch (VB.Left(cboPart.Text, 1))
                    {
                        case "1":
                            SQL += ComNum.VBLF + " AND TO_CHAR(INTIME,'HH24:MI:SS') >= '08:00:00'";
                            SQL += ComNum.VBLF + " AND TO_CHAR(INTIME,'HH24:MI:SS') < '20:00:00'";
                            break;
                        case "2":
                            SQL += ComNum.VBLF + " AND (TO_CHAR(INTIME,'HH24:MI:SS') >= '20:00:00'";
                            SQL += ComNum.VBLF + " OR TO_CHAR(INTIME,'HH24:MI:SS') < '08:00:00') ";
                            break;
                    }
                }
                else
                {
                    SQL += ComNum.VBLF + "              WHERE a.JDate >= TO_DATE('" + dtpFDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')  AND a.JDATE <= TO_DATE('" + dtpTDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')";
                }

                if (chkYeyak.Checked)
                {
                    if (rdoYeyak0.Checked)
                    {
                       SQL += ComNum.VBLF + "                AND A.STUDY = '예약자'";
                    }
                    else
                    {
                        SQL += ComNum.VBLF + "                AND (A.STUDY <> '예약자' OR A.STUDY IS NULL) ";
                    }
                }

                SQL += ComNum.VBLF + "                AND a.Pano=b.Pano(+)   ";
                if(chkEnd.Checked == false)
                {
                    SQL += ComNum.VBLF + "                AND A.OutTime IS NULL ";
                }
                SQL += ComNum.VBLF + "                AND A.PANO = K.Pano )  ";

                //'조회기간중 입원인 환자
                if( ComboJob.Checked)
                {
                    SQL += ComNum.VBLF + " AND OCSJIN = '#' ";
                    SQL += ComNum.VBLF + " AND PANO IN ( SELECT PANO FROM  KOSMOS_PMPA.IPD_NEW_MASTER ";
                    SQL += ComNum.VBLF + "                WHERE GBSTS = '0' ";
                    SQL += ComNum.VBLF + "                AND INDATE BETWEEN TO_DATE('" + dtpFDate.Value.ToShortDateString() + "', 'YYYY-MM-DD')  AND TO_DATE('" + dtpTDate.Value.ToShortDateString() + "', 'YYYY-MM-DD') ) ";
                }

                if (rdoSORT0.Checked)
                {

                    SQL += ComNum.VBLF + "ORDER BY SName, Pano, JDATE ";
                }
                else if (rdoSORT1.Checked)
                {
                    SQL += ComNum.VBLF + "ORDER BY PANO, JDATE ";
                }
                else
                {

                    SQL += ComNum.VBLF + "ORDER BY JDATE DESC, SNAME, PANO ";
                }

                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    ComFunc.MsgBoxEx(this, "조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (dt.Rows.Count > 0)
                {

                    SSList_Sheet1.RowCount = dt.Rows.Count;
                    SSList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    string strPtNo = string.Empty;
                    string strTime = string.Empty;
                    string strFormNo = string.Empty;

                    string strHidden1 = "OK";
                    string strHidden2 = "OK";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        SSList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                        strPtNo = SSList_Sheet1.Cells[i, 0].Text.Trim();
                        SSList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                        SSList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["JDate"].ToString().Trim();
                        strTime = SSList_Sheet1.Cells[i, 2].Text.Trim();

                        strFormNo = READ_TEMR(strPtNo, strTime.Replace("-", ""));


                        strHidden1 = "OK";
                        strHidden2 = "OK";
                        //'2048:  ER 경과기록
                        //'1836:  응급의뢰센 '터 간호정보조사지
                        //'2005:  응급센터진료기록


                        #region 서식지 여러개 점검
                        if (strFormNo.IndexOf("2005") != -1 || strFormNo.IndexOf("1828") != -1 || strFormNo.IndexOf("1831") != -1 ||
                           strFormNo.IndexOf("1841") != -1 || strFormNo.IndexOf("2074") != -1 || strFormNo.IndexOf("2605") != -1)
                            {
                                SSList_Sheet1.Cells[i, 4].Text = "T";
                        }
                        else
                        {
                            SSList_Sheet1.Cells[i, 4].Text = "";
                        }

                        if(SSList_Sheet1.Cells[i, 4].Text.Trim().Equals("T"))
                        {
                            SSList_Sheet1.Cells[i, 4].BackColor = Color.FromArgb(255, 200, 200);
                        }
                        else
                        {
                            SSList_Sheet1.Cells[i, 4].BackColor = Color.FromArgb(255, 255, 255);
                            strHidden1 = "NO";
                        }

                        #endregion

                        #region 1836 ㅈ머검
                        if (strFormNo.IndexOf("1836") != -1)
                        {
                            SSList_Sheet1.Cells[i, 5].Text = "T";
                        }
                        else
                        {
                            SSList_Sheet1.Cells[i, 5].Text = "";
                        }

                        if (SSList_Sheet1.Cells[i, 5].Text.Trim().Equals("T"))
                        {
                            SSList_Sheet1.Cells[i, 5].BackColor = Color.FromArgb(200, 200, 255);
                        }
                        else
                        {
                            SSList_Sheet1.Cells[i, 5].BackColor = Color.FromArgb(255, 255, 255);
                        }
                        #endregion


                        #region 1836 ㅈ머검
                        if (strFormNo.IndexOf("2048") != -1)
                        {
                            SSList_Sheet1.Cells[i, 6].Text = "T";
                        }
                        else
                        {
                            SSList_Sheet1.Cells[i, 6].Text = "";
                        }

                        if (SSList_Sheet1.Cells[i, 6].Text.Trim().Equals("T"))
                        {
                            SSList_Sheet1.Cells[i, 6].BackColor = Color.FromArgb(200, 255, 200);
                        }
                        else
                        {
                            SSList_Sheet1.Cells[i, 6].BackColor = Color.FromArgb(255, 255, 255);
                            strHidden2 = "NO";
                        }
                        #endregion

                        if(chkCheck.Checked)
                        {
                            SSList_Sheet1.Rows[i].Visible = strHidden1.Equals("OK") || strHidden2.Equals("OK") ? false : true;
                        }
                        else
                        {
                            SSList_Sheet1.Rows[i].Visible = true;
                        }
                    }
                }

                Cursor.Current = Cursors.Default;
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBoxEx(this, ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        string READ_TEMR(string ArgPano, string ArgInDate)
        {
            StringBuilder rtnVal = new StringBuilder();

            string strSql = string.Empty;
            OracleDataReader reader = null;

            strSql = " SELECT FORMNO";
            strSql = strSql + ComNum.VBLF + "FROM " + ComNum.DB_EMR + "EMRXML";
            strSql = strSql + ComNum.VBLF + "  WHERE MEDDEPTCD = 'ER'";
            strSql = strSql + ComNum.VBLF + "    AND MEDFRDATE = '" + ArgInDate.Replace("-", "") + "'";
            strSql = strSql + ComNum.VBLF + "    AND PTNO = '" + ArgPano + "'";
            strSql = strSql + ComNum.VBLF + "    AND FORMNO IN (2005, 1836, 2048) ";

            strSql = strSql + ComNum.VBLF + "UNION ALL";
            strSql = strSql + ComNum.VBLF + "SELECT FORMNO";
            strSql = strSql + ComNum.VBLF + "  FROM " + ComNum.DB_EMR + "AEMRCHARTMST";
            strSql = strSql + ComNum.VBLF + " WHERE MEDDEPTCD = 'ER'";
            strSql = strSql + ComNum.VBLF + "   AND MEDFRDATE = '" + ArgInDate.Replace("-", "") + "'";
            strSql = strSql + ComNum.VBLF + "   AND PTNO = '" + ArgPano + "'";
            strSql = strSql + ComNum.VBLF + "   AND FORMNO IN (2005, 1836, 2048) ";

            string sqlErr = clsDB.GetAdoRs(ref reader, strSql, clsDB.DbCon);
            if (sqlErr.Length > 0)
            {
                clsDB.SaveSqlErrLog(sqlErr, sqlErr, clsDB.DbCon);
                ComFunc.MsgBox(sqlErr);
                return rtnVal.ToString().Trim();
            }
            

            if (reader.HasRows)
            {
                while(reader.Read())
                {
                    rtnVal.Append("|" + reader.GetValue(0).ToString().Trim());
                }
            }

            reader.Dispose();


            return rtnVal.ToString().Trim();
        }


        private void BtnPrint_Click(object sender, EventArgs e)
        {
            using (clsSpread CS = new clsSpread())
            {
                string strTitle  = string.Empty;
                string strHeader = string.Empty;
                string strFooter = string.Empty;
                bool PrePrint = false;

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 30, 30, 50, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                CS.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }


        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ChkCheck_CheckedChanged(object sender, EventArgs e)
        {
            cboPart.Visible = chkCheck.Checked;
        }

        private void ChkYeyak_CheckedChanged(object sender, EventArgs e)
        {
            if(chkYeyak.Checked)
            {
                rdoYeyak0.Visible = true;
                rdoYeyak1.Visible = true;
            }
            else
            {
                rdoYeyak0.Visible = false;
                rdoYeyak1.Visible = false;
            }
        }

        private void SSList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (SSList_Sheet1.RowCount == 0)
                return;

            if (SSList_Sheet1.Cells[e.Row, 0].Text.Trim().Length == 0)
                return;

            rSendPatInfo(SSList_Sheet1.Cells[e.Row, 0].Text.Trim());

            if (chkClose.Checked)
                Close();
        }

        private void FrmERPatient_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(rClosed == null)
            {
                Close();
            }
            else
            {
                rClosed();
            }
        }
    }
}
