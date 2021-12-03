using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2017-10-27
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref=  D:\psmh\IPD\iument\iument.vbp\frm전실전과조회.frm" >> frmPmpaViewViewTrans.cs 폼이름 재정의" />

    public partial class frmPmpaViewViewTrans : Form
    {

        clsSpread.SpdPrint_Margin setMargin;
        clsSpread.SpdPrint_Option setOption;
        clsSpread CS = new clsSpread();
        ComFunc CF = new ComFunc();
        clsIument CIT = new clsIument();
        string strdtP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewViewTrans()
        {
            InitializeComponent();
        }

        public frmPmpaViewViewTrans(string strPano)
        {
            InitializeComponent();

            if (strPano.Trim() != "")
            {
                txtPano.Text = strPano;
            }
        }

        private void frmPmpaViewViewTrans_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            Screen_Clear();
            txtPano.Text = "";
        }

        private void Screen_Clear()
        {
            SSTrans_Sheet1.RowCount = 0;
            SSTrans_Sheet1.RowCount = 12;

            //SSInfo_Sheet1.Cells [0 , 0 , SSInfo_Sheet1.RowCount - 1 , SSInfo_Sheet1.ColumnCount - 1].Text = "";

            SS1_Sheet1.RowCount = 5;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;


            txtPano.Text = txtPano.Text.Trim();

            if (rdoOptTewon2.Checked == true && txtPano.Text == "")
            {
                ComFunc.MsgBox("퇴원환자는 반드시 등록번호 또는 성명을 입력하셔야 됩니다.", "확인");
                return;
            }

            if (rdoOptJob0.Checked == true)
            {
                if (txtPano.Text != "")
                {
                    txtPano.Text = VB.Val(txtPano.Text).ToString("00000000").Trim();
                }
            }

            try
            {

                //'환자명단을 SELECT
                SQL = "";
                SQL = "SELECT IPDNO,Pano,SName,DeptCode,RoomCode,GbSTS,Bi,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(InDate,'YYYY-MM-DD') InDate ";
                SQL = SQL + ComNum.VBLF + " FROM IPD_NEW_MASTER ";

                if (rdoOptTewon0.Checked == true)
                {
                    if (rdoOptJob0.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE Pano='" + txtPano.Text + "' ";
                    }
                    if (rdoOptJob1.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " WHERE SName LIKE '%" + txtPano.Text + "%' ";
                    }
                }
                else if (rdoOptTewon1.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "WHERE ActDate IS NULL ";
                    SQL = SQL + ComNum.VBLF + "  AND GbSTS = '1' ";
                    if (rdoOptJob0.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND Pano='" + txtPano.Text + "' ";
                    }

                    if (rdoOptJob1.Checked == true && txtPano.Text != "")
                    {
                        SQL = SQL + ComNum.VBLF + " AND SName LIKE '%" + txtPano.Text + "%' ";
                    }
                }
                else
                {
                    if (rdoOptJob0.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + txtPano.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND ActDate IS NOT NULL ";
                    }
                    else if (rdoOptJob1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE SName = '" + txtPano.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND ActDate IS NOT NULL ";
                    }
                    else
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE ActDate = TO_DATE('" + txtPano.Text + "','YYYY-MM-DD') ";
                    }
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY  Pano, INDATE DESC  ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //스프레드 출력문
                SSListJewon_Sheet1.RowCount = dt.Rows.Count;
                SSListJewon_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    SSListJewon_Sheet1.Cells[i, 0].Text = dt.Rows[i]["Pano"].ToString().Trim();
                    SSListJewon_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SName"].ToString().Trim();
                    SSListJewon_Sheet1.Cells[i, 2].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    SSListJewon_Sheet1.Cells[i, 3].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    SSListJewon_Sheet1.Cells[i, 4].Text = dt.Rows[i]["IPDNO"].ToString().Trim();
                }
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region 함수 모음 (SSInfo_Display , READ_HISTORY)
        private void SSInfo_Display()
        {
            int i = 0;
            int nRow = 0;
            string strJumin = "";
            string strZipCode = "";
            string strJuso = "";
            string strTel = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;


            try
            {
                //'환자명단을 SELECT
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT Jumin1,      Jumin2,      Jumin3,                  ";
                SQL = SQL + ComNum.VBLF + "        ZipCode1,    ZipCode2,    Juso,  Tel,    HPhone    ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_PATIENT                    ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                  ";
                SQL = SQL + ComNum.VBLF + "    AND Pano='" + clsPmpaType.IMST.Pano + "'               ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                strJumin = "";
                strZipCode = "";
                strJuso = "";
                strTel = "";

                if (dt.Rows.Count > 0)
                {

                    //'주민암호화
                    if (dt.Rows[0]["Jumin3"].ToString().Trim() != "")
                        strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                    else
                        strJumin = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + dt.Rows[0]["Jumin2"].ToString().Trim();

                    strZipCode = dt.Rows[0]["ZipCode1"].ToString().Trim() + "-" + dt.Rows[0]["ZipCode2"].ToString().Trim();

                    strJuso = CF.READ_BAS_Mail(clsDB.DbCon, strZipCode) + " " + dt.Rows[0]["Juso"].ToString().Trim();
                    strTel = dt.Rows[0]["Tel"].ToString().Trim();
                    strTel = strTel + " " + dt.Rows[0]["HPhone"].ToString().Trim();
                    strTel = (strTel).Trim();
                }

                dt.Dispose();
                dt = null;

                //SSInfo.Col = 2
                SSInfo_Sheet1.Cells[0, 1].Text = " " + clsPmpaType.IMST.Sname + "(" + clsPmpaType.IMST.Age + "/" + clsPmpaType.IMST.Sex + ")";
                SSInfo_Sheet1.Cells[1, 1].Text = " " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", clsPmpaType.IMST.Bi);
                SSInfo_Sheet1.Cells[2, 1].Text = " " + VB.Left(clsPmpaType.IMST.InDate, 10);
                SSInfo_Sheet1.Cells[3, 1].Text = " " + clsPmpaType.IMST.InTime;
                SSInfo_Sheet1.Cells[4, 1].Text = " " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원경로", clsPmpaType.IMST.AmSet7);
                SSInfo_Sheet1.Cells[5, 1].Text = " " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_입원상태", clsPmpaType.IMST.GbSTS);
                SSInfo_Sheet1.Cells[6, 1].Text = " " + strTel;
                SSInfo_Sheet1.Cells[7, 1].Text = " " + strJuso;
                //SSInfo.Col = 4
                SSInfo_Sheet1.Cells[0, 3].Text = " " + strJumin;
                SSInfo_Sheet1.Cells[1, 3].Text = " " + clsPmpaType.IMST.DeptCode + "/" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, clsPmpaType.IMST.DrCode);
                SSInfo_Sheet1.Cells[2, 3].Text = " " + clsPmpaType.IMST.OutDate;
                SSInfo_Sheet1.Cells[3, 3].Text = " " + clsPmpaType.IMST.ActDate;
                SSInfo_Sheet1.Cells[4, 3].Text = " " + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "IPD_퇴원종류", clsPmpaType.IMST.GbTewon);

                switch (SSInfo_Sheet1.Cells[4, 3].Text)
                {
                    case "0":
                        SSInfo_Sheet1.Cells[4, 3].Text = "완쾌";
                        break;
                    case "1":
                        SSInfo_Sheet1.Cells[4, 3].Text = "빈사";
                        break;
                    case "2":
                        SSInfo_Sheet1.Cells[4, 3].Text = "자의";
                        break;
                    case "3":
                        SSInfo_Sheet1.Cells[4, 3].Text = "이송";
                        break;
                    case "4":
                        SSInfo_Sheet1.Cells[4, 3].Text = "사망";
                        break;
                    case "5":
                        SSInfo_Sheet1.Cells[4, 3].Text = "도주";
                        break;
                    case "6":
                        SSInfo_Sheet1.Cells[4, 3].Text = "보관금퇴원";
                        break;
                    default:
                        SSInfo_Sheet1.Cells[4, 3].Text = "";
                        break;
                }

                SS1_Sheet1.RowCount = 0;

                //'IPD_TRANS를 읽어 Sheet에 표시함
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT TRSNO,TO_CHAR(InDate,'YYYY-MM-DD') InDate,                 ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(OutDate,'YYYY-MM-DD') OutDate,                     ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,Ilsu,Bi,             ";
                SQL = SQL + ComNum.VBLF + "        DeptCode,DrCode,GbIPD,SangAmt,OgPdBun,AmSet3,Amt50,Amt53   ";
                SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS                                ";
                SQL = SQL + ComNum.VBLF + "  WHERE 1=1                                                      ";
                SQL = SQL + ComNum.VBLF + "      AND IPDNO=" + clsPmpaType.IMST.IPDNO + "                      ";
                SQL = SQL + ComNum.VBLF + "    AND GbIPD <> 'D'                                               "; //'삭제는 제외
                SQL = SQL + ComNum.VBLF + "  ORDER BY GbIPD,InDate DESC                                       ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count == 0)
                {

                    dt.Dispose();
                    dt = null;
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                nRow = 0;

                SS1_Sheet1.RowCount = dt.Rows.Count;
                SS1_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;
                    if (nRow > dt.Rows.Count)
                    {
                        SS1_Sheet1.RowCount = nRow;
                    }
                    SS1_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["ActDate"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["InDate"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["OutDate"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["Ilsu"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 6].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DrCode"].ToString().Trim());
                    SS1_Sheet1.Cells[nRow - 1, 7].Text = "";
                    if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                    {
                        SS1_Sheet1.Cells[nRow - 1, 7].Text = "지병";
                    }
                    SS1_Sheet1.Cells[nRow - 1, 8].Text = "";
                    if (VB.Val(dt.Rows[i]["SangAmt"].ToString().Trim()) > 0)
                    {
                        SS1_Sheet1.Cells[nRow - 1, 8].Text = "상한";
                    }
                    SS1_Sheet1.Cells[nRow - 1, 9].Text = dt.Rows[i]["OgPdBun"].ToString().Trim();
                    SS1_Sheet1.Cells[nRow - 1, 10].Text = "";
                    if (dt.Rows[i]["AmSet3"].ToString().Trim() == "9")
                    {
                        SS1_Sheet1.Cells[nRow - 1, 10].Text = "완료";
                    }
                    SS1_Sheet1.Cells[nRow - 1, 11].Text = Convert.ToDouble(dt.Rows[i]["Amt50"].ToString().Trim()).ToString("#,##0");
                    SS1_Sheet1.Cells[nRow - 1, 12].Text = Convert.ToDouble(dt.Rows[i]["Amt53"].ToString().Trim()).ToString("#,##0");
                    SS1_Sheet1.Cells[nRow - 1, 13].Text = clsPmpaType.IMST.IPDNO.ToString();
                    SS1_Sheet1.Cells[nRow - 1, 14].Text = dt.Rows[i]["TRSNO"].ToString().Trim();
                }



                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void READ_HISTORY()
        {
            int i = 0;
            int nRow = 0;
            string strOldData = "";
            string strNewData = "";
            string strViewData = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            SSTrans_Sheet1.RowCount = 0;

            //try
            //{
            #region READ_BM

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT TO_CHAR(JobDate,'YYYY-MM-DD') JobDate, Bi ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_BM                  ";
            SQL = SQL + ComNum.VBLF + "  WHERE 1=1 ";
            SQL = SQL + ComNum.VBLF + "      AND IPDNO = " + clsPmpaType.IMST.IPDNO + "    ";
            SQL = SQL + ComNum.VBLF + "      AND GbBackUp = 'J'                            ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY JobDate,Bi                             ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            strOldData = dt.Rows[0]["Bi"].ToString().Trim();
            nRow = 0;

            for (i = 0; i < dt.Rows.Count; i++)
            {
                strNewData = dt.Rows[i]["Bi"].ToString().Trim();

                if (strOldData != strNewData)
                {
                    nRow = nRow + 1;

                    if (nRow > SSTrans_Sheet1.RowCount)
                    {
                        SSTrans_Sheet1.RowCount = nRow;
                        SSTrans_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    }
                    SSTrans_Sheet1.Cells[nRow - 1, 0].Text = nRow.ToString().Trim();
                    SSTrans_Sheet1.Cells[nRow - 1, 1].Text = dt.Rows[i]["JobDate"].ToString().Trim();
                    strViewData = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strOldData ) + "->";
                    strViewData = strViewData + clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_환자종류", strNewData);
                    SSTrans_Sheet1.Cells[nRow - 1, 2].Text = strViewData;
                    strOldData = strNewData;
                }
            }

            dt.Dispose();
            dt = null;
            #endregion

            #region READ_TRANSFOR_Dept


            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT FrDept, FRDOCTOR,  FrRoom, ToDept, TODOCTOR, ToRoom,        ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(TrsDate,'YYYY-MM-DD') TrsDate1 ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR  ";
            SQL = SQL + ComNum.VBLF + "  WHERE Pano       = '" + clsPmpaType.IMST.Pano + "' ";
            SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') >= '" + VB.Left(clsPmpaType.IMST.InDate, 10) + "' ";

            if (clsPmpaType.IMST.OutDate != "")
                SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') <= '" + clsPmpaType.IMST.OutDate + "' ";

            SQL = SQL + ComNum.VBLF + "    AND FrDept <> ToDept ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY TrsDate ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {

                    nRow = nRow + 1;

                    if (nRow > SSTrans_Sheet1.RowCount)
                    {
                        SSTrans_Sheet1.RowCount = nRow;
                        SSTrans_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    }
                    SSTrans_Sheet1.Cells[nRow - 1, 3].Text = nRow.ToString().Trim();
                    SSTrans_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["TrsDate1"].ToString().Trim();
                    SSTrans_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["FrDept"].ToString().Trim() + "(" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["FRDOCTOR"].ToString().Trim()) + ")->" + dt.Rows[i]["ToDept"].ToString().Trim() + "(" + clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["TODOCTOR"].ToString().Trim()) + ")";
                }
            }
            dt.Dispose();
            dt = null;
            #endregion

            #region READ_TRANSFOR_Room

            SQL = "";
            SQL = SQL + ComNum.VBLF + " SELECT FrDept, FrRoom, ToDept, ToRoom,        ";
            SQL = SQL + ComNum.VBLF + "        TO_CHAR(TrsDate,'YYYY-MM-DD') TrsDate1 ";
            SQL = SQL + ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANSFOR  ";
            SQL = SQL + ComNum.VBLF + "  WHERE Pano       = '" + clsPmpaType.IMST.Pano + "' ";
            SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') >= '" + VB.Left(clsPmpaType.IMST.InDate, 10) + "' ";

            if (clsPmpaType.IMST.OutDate != "")
                SQL = SQL + ComNum.VBLF + "    AND TO_CHAR(TrsDate,'YYYY-MM-DD') <= '" + clsPmpaType.IMST.OutDate + "' ";

            SQL = SQL + ComNum.VBLF + "    AND FrRoom <> ToRoom ";
            SQL = SQL + ComNum.VBLF + "  ORDER BY TrsDate ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            if (dt.Rows.Count > 0)
            {
                nRow = 0;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    nRow = nRow + 1;

                    if (nRow > SSTrans_Sheet1.RowCount)
                    {
                        SSTrans_Sheet1.RowCount = nRow;
                        SSTrans_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    }
                    SSTrans_Sheet1.Cells[nRow - 1, 6].Text = nRow.ToString();
                    SSTrans_Sheet1.Cells[nRow - 1, 7].Text = dt.Rows[i]["TrsDate1"].ToString().Trim();
                    SSTrans_Sheet1.Cells[nRow - 1, 8].Text = dt.Rows[i]["FrRoom"].ToString().Trim() + " -> " + dt.Rows[i]["ToRoom"].ToString().Trim();

                }
            }
            dt.Dispose();
            dt = null;
            #endregion

            Cursor.Current = Cursors.Default;
            btnSearch.Enabled = true;


            //}
            //catch (Exception ex)
            //{
            //    btnSearch.Enabled = true;
            //    ComFunc.MsgBox (ex.Message);
            //    clsDB.SaveSqlErrLog (ex.Message , SQL, clsDB.DbCon); //에러로그 저장
            //    Cursor.Current = Cursors.Default;
            //}

        }
        #endregion


        private void SSListJewon_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            double nIPDNO = 0;

            nIPDNO = VB.Val(SSListJewon_Sheet1.Cells[e.Row, 4].Text);
            CIT.Read_Ipd_Master(clsDB.DbCon, "", (long)nIPDNO);
            if (clsPmpaType.IMST.IPDNO == 0)
            {
                ComFunc.MsgBox("입원마스타가 없습니다.", "오류");
                return;
            }

            SSInfo_Display();
            READ_HISTORY();

            if (nIPDNO > 335523)
            {
                Read_Ipd_Doctor_Change(nIPDNO);
            }
        }

        private void Read_Ipd_Doctor_Change(double ArgIpdNo)
        {
            int i = 0;
            int nRow = 0;
            int nReadCnt = 0;
            string strTemp = "";
            string strPano = "";
            string strSname = "";
            string strList = "";
            string strFrRoom = "";
            string strToRoom = "";
            string strDrcode = "";
            string strOK = "";
            string strSel = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strVal = "";

            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;
            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SUBSTR(REMARK,1,8) PANO,'' FrWard,'' FrRoom,'' FrDept,'' ToWard,'' ToRoom,'' ToDept, IPDNO,  ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(JobTime,'YYYY-MM-DD HH24:MI') TrsDate,Remark,'의사변경' AS Gubun                     ";
                SQL = SQL + ComNum.VBLF + "  FROM " + ComNum.DB_PMPA + "ETC_JOBHISTORY  ";
                SQL = SQL + ComNum.VBLF + "   WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "     AND IPDNO =" + ArgIpdNo + " ";
                SQL = SQL + ComNum.VBLF + "   ORDER BY JobTime DESC ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    btnSearch.Enabled = true;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SS2_Sheet1.RowCount = 0;
                    SS2_Sheet1.RowCount = dt.Rows.Count;


                    //스프레드 출력문
                    SS2_Sheet1.RowCount = dt.Rows.Count;
                    SS2_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strTemp = dt.Rows[i]["Remark"].ToString().Trim();
                        strPano = VB.Pstr(strTemp, " ", 1);
                        strSname = VB.Pstr(strTemp, " ", 2);
                        strList = "(" + VB.Pstr(strTemp, "(", 2);
                        strFrRoom = VB.Mid((strList).Trim(), 2, 4);
                        strToRoom = VB.Mid((strList).Trim(), 8, 4);

                        strOK = "OK";
                        strSel = "";

                        SQL = "";
                        SQL = " SELECT DrCode ";
                        SQL = SQL + ComNum.VBLF + "FROM ADMIN.BAS_DOCTOR ";
                        SQL = SQL + ComNum.VBLF + " WHERE ( DRCODE ='" + strFrRoom + "' OR DRCODE ='" + strToRoom + "' ) ";
                        SQL = SQL + ComNum.VBLF + " AND GBCHOICE='Y' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            btnSearch.Enabled = true;
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                            strSel = "OK";
                        dt1.Dispose();
                        dt1 = null;

                        if (strOK == "OK" && strPano != "81000004")
                        {
                            nRow = nRow + 1;

                            SS2_Sheet1.Cells[nRow - 1, 0].Text = strPano.Trim();
                            SS2_Sheet1.Cells[nRow - 1, 1].Text = strSname.Trim();

                            if (strSel == "OK")
                            {
                                SS2_Sheet1.Cells[nRow - 1, 2].Text = "Y";
                            }

                            SS2_Sheet1.Cells[nRow - 1, 3].Text = strList + "(" + strFrRoom + ") " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strFrRoom);
                            SS2_Sheet1.Cells[nRow - 1, 4].Text = strList + "(" + strToRoom + ") " + clsVbfunc.GetBASDoctorName(clsDB.DbCon, strToRoom);
                            SS2_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["TrsDate"].ToString().Trim();
                            SS2_Sheet1.Cells[nRow - 1, 6].Text = dt.Rows[i]["Gubun"].ToString().Trim();
                        }
                    }
                }
                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void rdoOptJob_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOptJob0.Checked == true)
            {
                dtpFdate.Visible = false;
                txtPano.Visible = true;
                lblpano.Text = "등록번호";
            }
            else if (rdoOptJob1.Checked == true)
            {
                dtpFdate.Visible = false;
                txtPano.Visible = true;
                lblpano.Text = "환자성명";
            }
            else
            {
                dtpFdate.Visible = true;
                txtPano.Visible = false;
                lblpano.Text = "퇴원일자";
            }

        }

        private void rdoOptTewon_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOptTewon2.Checked == true)
            {
                rdoOptJob2.Checked = true;
            }
            else
            {
                if (rdoOptJob2.Checked == true)
                {
                    rdoOptJob0.Visible = true;
                }
                rdoOptJob2.Enabled = false;
            }
        }
    }
}
