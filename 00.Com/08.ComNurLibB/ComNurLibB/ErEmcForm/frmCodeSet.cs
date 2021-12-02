using System;
using System.Data;
using System.Windows.Forms;
using ComBase;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : EctEmcSend
    /// File Name       : frmCodeSet
    /// Description     : 응급자원정보 병상 분류 선택
    /// Author          : 전상원
    /// Create Date     : 2018-05-08
    /// Update History  : 
    /// </summary>
    /// <history>      
    /// </history>
    /// <seealso cref= )\OPD\emc\dgemc.vbp(FrmCodeSet.frm)" >> frmCodeSet.cs 폼이름 재정의" />
    public partial class frmCodeSet : Form
    {
        public frmCodeSet()
        {
            InitializeComponent();
        }

        private void frmCodeSet_Load(object sender, EventArgs e)
        {
            cboWard.Items.Add("전체");
            cboWard.Items.Add("ER");
            cboWard.Items.Add("ICU");
            cboWard.Items.Add("병동");
            cboWard.SelectedIndex = 0;

            cboGubun.Items.Add("****.전체"); 
            cboGubun.Items.Add("O001.응급실 일반병상"); 
            cboGubun.Items.Add("O002.응급실 소아 병상"); 
            cboGubun.Items.Add("O003.응급실 읍압 격리 병상"); 
            cboGubun.Items.Add("O004.응급실 일반 격리 병상"); 
            cboGubun.Items.Add("O005.응급전용 중환자실"); 
            cboGubun.Items.Add("O006.내과중환자실"); 
            cboGubun.Items.Add("O007.외과중환자실"); 
            cboGubun.Items.Add("O008.신생아중환자실"); 
            cboGubun.Items.Add("O009.소아중환자실"); 
            cboGubun.Items.Add("O010.소아응급전용 중환자실 병상"); 
            cboGubun.Items.Add("O011.신경과중환자실"); 
            cboGubun.Items.Add("O012.신경외과중환자실"); 
            cboGubun.Items.Add("O013.화상중환자실"); 
            cboGubun.Items.Add("O014.외상중환자실"); 
            cboGubun.Items.Add("O015.심장내과 중환자실"); 
            cboGubun.Items.Add("O016.흉부외과 중환자실"); 
            cboGubun.Items.Add("O017.일반 중환자실"); 
            cboGubun.Items.Add("O018.중환자실 내 음압 격리 병상"); 
            cboGubun.Items.Add("O019.응급전용 입원실"); 
            cboGubun.Items.Add("O020.소아응급전용 입원 병상"); 
            cboGubun.Items.Add("O021.외상전용 입원실"); 
            cboGubun.Items.Add("O022.수술실"); 
            cboGubun.Items.Add("O023.외상전용 수술실"); 
            cboGubun.Items.Add("O024.정신과 폐쇄 병상"); 
            cboGubun.Items.Add("O025.음압 격리 병상"); 
            cboGubun.Items.Add("O026.분만실"); 
            cboGubun.Items.Add("O027.CT"); 
            cboGubun.Items.Add("O028.MRI"); 
            cboGubun.Items.Add("O029.혈관촬영기"); 
            cboGubun.Items.Add("O030.인공호흡기"); 
            cboGubun.Items.Add("O031.인공호흡기(소아)"); 
            cboGubun.Items.Add("O032.인큐베이터"); 
            cboGubun.Items.Add("O033.CRRT"); 
            cboGubun.Items.Add("O034.ECMO"); 
            cboGubun.Items.Add("O035.치료적 저체온 요법"); 
            cboGubun.Items.Add("O036.화상전용 처치실");
            cboGubun.SelectedIndex = 0;

            GetData();
        }

        private void GetData()
        {            
            int i = 0;
            int nREAD = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.RowCount = 0;

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT A.GUBUN, A.WARDCODE, A.ROOMCODE, A.NAME, A.CODE, A.TBED, B.ERCODE, B.ROWID";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "VIEW_ER_EMC_ROOM A, " + ComNum.DB_PMPA + "NUR_ER_EMC_ROOM B";
                if (chkAll.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE A.WARDCODE = B.WARDCODE";
                    SQL = SQL + ComNum.VBLF + "    AND A.ROOMCODE = B.ROOMCODE";
                    SQL = SQL + ComNum.VBLF + "    AND A.CODE = B.CODE";
                    SQL = SQL + ComNum.VBLF + "    AND B.ERCODE IS NOT NULL ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  WHERE A.WARDCODE = B.WARDCODE(+)";
                    SQL = SQL + ComNum.VBLF + "    AND A.ROOMCODE = B.ROOMCODE(+)";
                    SQL = SQL + ComNum.VBLF + "    AND A.CODE = B.CODE(+)";
                }

                if (VB.Trim(cboWard.Text) == "전체")
                {

                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND A.GUBUN = '" + VB.Trim(cboWard.Text) + "' ";
                }

                if (VB.Left(cboGubun.Text, 4) == "****")
                {

                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "    AND B.ERCODE = '" + VB.Left(VB.Trim(cboGubun.Text), 4) + "' ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nREAD = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["GUBUN"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 2].Text = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["CODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["TBED"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["NAME"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 6].Text = READ_CODE2NAME(dt.Rows[i]["ERCODE"].ToString().Trim());
                        ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["ERCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {            
            int i = 0;
            string strOK = "";
            string strROWID = "";
            string strGUBUN = "";
            string strWARDCODE = "";
            string strROOMCODE = "";
            string strCODE = "";
            string strNAME = "";
            string strERCODE = "";

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            strOK = "OK";
            
            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strGUBUN = VB.Trim(ssView_Sheet1.Cells[i, 0].Text);
                    strWARDCODE = VB.Trim(ssView_Sheet1.Cells[i, 1].Text);
                    strROOMCODE = VB.Trim(ssView_Sheet1.Cells[i, 2].Text);
                    strCODE = VB.Trim(ssView_Sheet1.Cells[i, 3].Text);
                    strNAME = VB.Trim(ssView_Sheet1.Cells[i, 5].Text);
                    strERCODE = VB.Trim(ssView_Sheet1.Cells[i, 7].Text);
                    strROWID = VB.Trim(ssView_Sheet1.Cells[i, 9].Text);

                    if (strROWID == "")
                    {
                        SQL = "";
                        SQL = " INSERT INTO " + ComNum.DB_PMPA + "NUR_ER_EMC_ROOM ";
                        SQL = SQL + ComNum.VBLF + "( GUBUN, WARDCODE, ROOMCODE, CODE,";
                        SQL = SQL + ComNum.VBLF + "  NAME, ERCODE) VALUES ( ";
                        SQL = SQL + ComNum.VBLF + "'" + strGUBUN + "','" + strWARDCODE + "','" + strROOMCODE + "','" + strCODE + "', ";
                        SQL = SQL + ComNum.VBLF + "'" + strNAME + "','" + strERCODE + "') ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            strOK = "NO";
                            return;
                        }
                    }
                    else
                    {
                        SQL = "";
                        SQL = " UPDATE " + ComNum.DB_PMPA + "NUR_ER_EMC_ROOM SET ";
                        SQL = SQL + ComNum.VBLF + "  ERCODE = '" + strERCODE + "' ";
                        SQL = SQL + ComNum.VBLF + "  WHERE ROWID = '" + strROWID + "' ";

                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox(SqlErr);
                            Cursor.Current = Cursors.Default;
                            strOK = "NO";
                            return;
                        }
                    }
                }

                if (strOK == "NO")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("저장 중 에러 발생");
                }
                else
                {
                    clsDB.setCommitTran(clsDB.DbCon);
                    GetData();
                }

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private string READ_NAME2CODE(string argDATA)
        {
            string rtnVal = "";

            switch (argDATA)
            {
                case "응급실 일반병상":
                    rtnVal = "O001";
                    break;
                case "응급실 소아 병상":
                    rtnVal = "O002";
                    break;
                case "응급실 읍압 격리 병상":
                    rtnVal = "O003";
                    break;
                case "응급실 일반 격리 병상":
                    rtnVal = "O004";
                    break;
                case "응급전용 중환자실":
                    rtnVal = "O005";
                    break;
                case "내과중환자실":
                    rtnVal = "O006";
                    break;
                case "외과중환자실":
                    rtnVal = "O007";
                    break;
                case "신생아중환자실":
                    rtnVal = "O008";
                    break;
                case "소아중환자실":
                    rtnVal = "O009";
                    break;
                case "소아응급전용 중환자실 병상":
                    rtnVal = "O010";
                    break;
                case "신경과중환자실":
                    rtnVal = "O011";
                    break;
                case "신경외과중환자실":
                    rtnVal = "O012";
                    break;
                case "화상중환자실":
                    rtnVal = "O013";
                    break;
                case "외상중환자실":
                    rtnVal = "O014";
                    break;
                case "심장내과 중환자실":
                    rtnVal = "O015";
                    break;
                case "흉부외과 중환자실":
                    rtnVal = "O016";
                    break;
                case "일반 중환자실":
                    rtnVal = "O017";
                    break;
                case "중환자실 내 음압 격리 병상":
                    rtnVal = "O018";
                    break;
                case "응급전용 입원실":
                    rtnVal = "O019";
                    break;
                case "소아응급전용 입원 병상":
                    rtnVal = "O020";
                    break;
                case "외상전용 입원실":
                    rtnVal = "O021";
                    break;
                case "수술실":
                    rtnVal = "O022";
                    break;
                case "외상전용 수술실":
                    rtnVal = "O023";
                    break;
                case "정신과 폐쇄 병상":
                    rtnVal = "O024";
                    break;
                case "음압 격리 병상":
                    rtnVal = "O025";
                    break;
                case "분만실":
                    rtnVal = "O026";
                    break;
                case "CT":
                    rtnVal = "O027";
                    break;
                case "MRI":
                    rtnVal = "O028";
                    break;
                case "혈관촬영기":
                    rtnVal = "O029";
                    break;
                case "인공호흡기":
                    rtnVal = "O030";
                    break;
                case "인공호흡기(소아)":
                    rtnVal = "O031";
                    break;
                case "인큐베이터":
                    rtnVal = "O032";
                    break;
                case "CRRT":
                    rtnVal = "O033";
                    break;
                case "ECMO":
                    rtnVal = "O034";
                    break;
                case "치료적 저체온 요법":
                    rtnVal = "O035";
                    break;
                case "화상전용 처치실":
                    rtnVal = "O036";
                    break;
                default:                    
                    //READ_GUBUN = "";
                    break;
            }

            return rtnVal;
        }

        private string READ_CODE2NAME(string argDATA)
        {
            string rtnVal = "";

            switch (argDATA)
            {
                case "O001":
                    rtnVal = "응급실 일반병상";
                    break;
                case "O002":
                    rtnVal = "응급실 소아 병상";
                    break;
                case "O003":
                    rtnVal = "응급실 읍압 격리 병상";
                    break;
                case "O004":
                    rtnVal = "응급실 일반 격리 병상";
                    break;
                case "O005":
                    rtnVal = "응급전용 중환자실";
                    break;
                case "O006":
                    rtnVal = "내과중환자실";
                    break;
                case "O007":
                    rtnVal = "외과중환자실";
                    break;
                case "O008":
                    rtnVal = "신생아중환자실";
                    break;
                case "O009":
                    rtnVal = "소아중환자실";
                    break;
                case "O010":
                    rtnVal = "소아응급전용 중환자실 병상";
                    break;
                case "O011":
                    rtnVal = "신경과중환자실";
                    break;
                case "O012":
                    rtnVal = "신경외과중환자실";
                    break;
                case "O013":
                    rtnVal = "화상중환자실";
                    break;
                case "O014":
                    rtnVal = "외상중환자실";
                    break;
                case "O015":
                    rtnVal = "심장내과 중환자실";
                    break;
                case "O016":
                    rtnVal = "흉부외과 중환자실";
                    break;
                case "O017":
                    rtnVal = "일반 중환자실";
                    break;
                case "O018":
                    rtnVal = "중환자실 내 음압 격리 병상";
                    break;
                case "O019":
                    rtnVal = "응급전용 입원실";
                    break;
                case "O020":
                    rtnVal = "소아응급전용 입원 병상";
                    break;
                case "O021":
                    rtnVal = "외상전용 입원실";
                    break;
                case "O022":
                    rtnVal = "수술실";
                    break;
                case "O023":
                    rtnVal = "외상전용 수술실";
                    break;
                case "O024":
                    rtnVal = "정신과 폐쇄 병상";
                    break;
                case "O025":
                    rtnVal = "음압 격리 병상";
                    break;
                case "O026":
                    rtnVal = "분만실";
                    break;
                case "O027":
                    rtnVal = "CT";
                    break;
                case "O028":
                    rtnVal = "MRI";
                    break;
                case "O029":
                    rtnVal = "혈관촬영기";
                    break;
                case "O030":
                    rtnVal = "인공호흡기";
                    break;
                case "O031":
                    rtnVal = "인공호흡기(소아)";
                    break;
                case "O032":
                    rtnVal = "인큐베이터";
                    break;
                case "O033":
                    rtnVal = "CRRT";
                    break;
                case "O034":
                    rtnVal = "ECMO";
                    break;
                case "O035":
                    rtnVal = "치료적 저체온 요법";
                    break;
                case "O036":
                    rtnVal = "화상전용 처치실";
                    break;
                default:
                    rtnVal = "";
                    break;
            }

            return rtnVal;
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            string strDATA = "";
            string strNAME = "";

            if (e.Column != 6)
            {
                return;
            }

            strDATA = VB.Trim(ssView_Sheet1.Cells[e.Row, 6].Text);

            strNAME = READ_NAME2CODE(strDATA);

            ssView_Sheet1.Cells[e.Row, 7].Text = strNAME;
        }
    }
}
