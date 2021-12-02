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
using ComDbB;

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmAppointR
    /// Description     : 예약현황
    /// Author          : 전상원
    /// Create Date     : 2018-05-24
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\ipdocs\iorder\mtsiorder.vbp(FrmAppointR) >> frmAppointR.cs 폼이름 재정의" />
    public partial class frmAppointR : Form
    {
        string strPrtGb = "";
        string prtTitle = "";
        string prtSubTitle = "";

        public frmAppointR()
        {
            InitializeComponent();
        }

        private void frmAppointR_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close();
            //    return;
            //} //폼 권한 조회
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            ComFunc CF = new ComFunc();

            Get_Ward();
            Get_Dept();
            Get_SlipNo();

            cboGb.Items.Add("1.병    동");
            cboGb.Items.Add("2.진료과목");
            cboGb.Items.Add("3.환자번호");

            dtpFrDate.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, strSysDate, -1));
            dtpToDate.Value = Convert.ToDateTime(CF.DATE_ADD(clsDB.DbCon, strSysDate, 7));

            strPrtGb = "NO";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            //if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}

            

            if (cboGb.SelectedIndex == -1)
            {
                return;
            }

            if (cboGb.SelectedIndex == 2)
            {
                return;
            }

            if (txtPtno.Text == "")
            {
                return;
            }

            if (dtpFrDate.Text == "")
            {
                return;
            }

            if (dtpToDate.Text == "")
            {
                return;
            }

            if (VB.Trim(VB.Left(cboDept.Text, 4)) == "ALL" && cboGb.SelectedIndex == 1)
            {
                return;
            }

            ssView_Sheet1.RowCount = 2;
            strPrtGb = "NO";

            #region Print_Title
            Print_Title();
            #endregion

            Cursor.Current = Cursors.WaitCursor;

            btnPrint.Enabled = false;

            if (chkXray.Checked == true)
            {
                #region Xray_Disp
                Xray_Disp();
                #endregion
            }

            if (chkEndo.Checked == true)
            {
                #region Endo_Disp
                Endo_Disp();
                #endregion
            }

            if (chkEkg.Checked == true)
            {
                #region Ekg_Disp
                Ekg_Disp();
                #endregion
            }

            if (chkPT.Checked == true)
            {
                #region PT_Disp()
                PT_Disp();
                #endregion
            }

            Cursor.Current = Cursors.Default;

            btnPrint.Enabled = true;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            //if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            //{
            //    return; //권한 확인
            //}
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Get_Dept()
        {
            int i = 0;
            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT DeptCode, DeptNameK ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + " WHERE  DeptCode  NOT IN ('II','RT','TO','AN','HR','PT','PC' )  "; //~1
                SQL = SQL + ComNum.VBLF + " ORDER  BY PrintRanking ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    for (i = 0; i < dt1.Rows.Count; i++)
                    {
                        cboDept.Items.Add(dt1.Rows[i]["DeptCode"].ToString().Trim());
                    }
                }

                dt1.Dispose();
                dt1 = null;

                cboDept.SelectedIndex = 0;

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string Get_SysDateTm()
        {
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            string strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");

            string rtnVal = "";

            rtnVal = strSysDate + " " + strSysTime;

            return rtnVal;
        }

        private string Get_SlipName(string SlipNo)
        {
            string rtnVal = "";

            int j = 0;

            for (j = 0; j < cboSlipNo.Items.Count; j++)
            {
                if (VB.Trim(SlipNo) == VB.Trim(VB.Left(cboSlipNo.Items[j].ToString().Trim(), 4)))
                {
                    rtnVal = VB.Trim(VB.Mid(cboSlipNo.Items[j].ToString().Trim(), 5, 50));
                    break;
                }
            }

            return rtnVal;
        }

        private void Get_SlipNo()
        {
            string strSlipNo = "";

            int i = 0;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT SlipNo, OrderName FROM OCS_ORDERCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE  OrderCode = ' ' ";
                SQL = SQL + ComNum.VBLF + " AND    Seqno     = 0   ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SlipNo        ";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    for (i = 0; i < dt2.Rows.Count; i++)
                    {
                        strSlipNo = dt2.Rows[i]["SlipNo"].ToString().Trim();
                        cboSlipNo.Items.Add(strSlipNo + dt2.Rows[i]["OrderName"].ToString().Trim());
                    }
                }

                dt2.Dispose();
                dt2 = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Get_Ward()
        {
            int i = 0;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";
                SQL = SQL + ComNum.VBLF + "";

                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt2.Rows.Count > 0)
                {
                    for (i = 0; i < dt2.Rows.Count; i++)
                    {
                        cboWard.Items.Add(dt2.Rows[i]["WardCode"].ToString().Trim());
                    }
                }

                dt2.Dispose();
                dt2 = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt2 != null)
                {
                    dt2.Dispose();
                    dt2 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string Get_FunName(string GbJong)
        {
            string rtnVal = "";

            switch (GbJong)
            {
                case "1":
                    rtnVal = "EKG";
                    break;
                case "2":
                    rtnVal = "뇌파";
                    break;
                case "3":
                    rtnVal = "Echo";
                    break;
                case "4":
                    rtnVal = "Pft";
                    break;
                case "5":
                    rtnVal = "Mct";
                    break;
                case "6":
                    rtnVal = "청력";
                    break;
                default:
                    rtnVal = "???";
                    break;
            }

            return rtnVal;
        }

        private string Get_XJongName(string XJong)
        {
            string rtnVal = "";

            switch (XJong)
            {
                case "1":
                    rtnVal = "X선  일반 촬영";
                    break;
                case "2":
                    rtnVal = "X선  특수 촬영";
                    break;
                case "3":
                    rtnVal = "초    음    파";
                    break;
                case "4":
                    rtnVal = "C           T";
                    break;
                case "5":
                    rtnVal = "M     R     T";
                    break;
                case "6":
                    rtnVal = "R           I";
                    break;
                case "7":
                    rtnVal = "B     M      D";
                    break;
            }

            return rtnVal;
        }

        private void Print_Title()
        {
            if (cboGb.SelectedIndex == 0)
            {
                prtSubTitle = " 병    동 : " + VB.Trim(VB.Left(cboWard.Text, 4));
            }
            else if (cboGb.SelectedIndex == 1)
            {
                prtSubTitle = " 진료과목 : " + VB.Trim(VB.Left(cboDept.Text, 4));
            }
            else if (cboGb.SelectedIndex == 2)
            {
                prtSubTitle = " 환자번호 : " + VB.Trim(txtPtno.Text);
            }
        }

        private void PT_Disp()
        {
            string strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

            int i = 0;
            int nRead = 0;
            string oldSlipNo = "";
            string newSlipNo = "";

            DataTable dt1 = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(B.ACTDATE, 'YYYY-MM-DD') ACTDATE, TO_CHAR( B.CDATE, 'YYYY-MM-DD') CDATE,";
                SQL = SQL + ComNum.VBLF + " A.PANO ,A.Sname,A.RoomCode, A.SEX, A.AGE, A.DEPTCODE, B.SUCODE , C.SUNAMEK ";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.ETC_PTORDER B, KOSMOS_PMPA.BAS_SUN C ";
                SQL = SQL + ComNum.VBLF + "  WHERE  GBSTS IN ('0') ";

                if (cboGb.SelectedIndex == 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.WardCode = '" + cboWard + "' ";
                }
                else if (cboGb.SelectedIndex == 1)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.DeptCode = '" + cboDept + "' ";
                }
                else if (cboGb.SelectedIndex == 2)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.Pano = '" + txtPtno + "' ";
                }

                SQL = SQL + ComNum.VBLF + " AND A.PANO = B.PANO    ";
                SQL = SQL + ComNum.VBLF + " AND B.GBIO='I'";
                SQL = SQL + ComNum.VBLF + " AND B.ACTDATE>=TRUNC(SYSDATE-2) ";
                SQL = SQL + ComNum.VBLF + " AND B.ACTDATE<=TRUNC(SYSDATE-1) ";
                SQL = SQL + ComNum.VBLF + "  AND B.SUCODE = C.SUNEXT(+)";
                SQL = SQL + ComNum.VBLF + " GROUP BY B.ACTDATE, B.CDATE,  A.PANO, A.Sname,A.RoomCode, A.SEX, A.AGE, A.DEPTCODE, B.SUCODE, C.SUNAMEK  ";

                SQL = SQL + ComNum.VBLF + " Union All ";

                SQL = SQL + ComNum.VBLF + "  SELECT TO_CHAR(SYSDATE,'YYYY-MM-DD') ACTDATE,TO_CHAR(SYSDATE,'YYYY-MM-DD') CDATE,";
                SQL = SQL + ComNum.VBLF + " A.PANO ,A.Sname,A.RoomCode, A.SEX, A.AGE, A.DEPTCODE,  'PT######' SUCODE, '물리치료의뢰' SUNAMEK";
                SQL = SQL + ComNum.VBLF + " FROM  KOSMOS_PMPA.IPD_NEW_MASTER A ";
                SQL = SQL + ComNum.VBLF + " WHERE A.GBSTS ='0' ";


                if (cboGb.SelectedIndex == 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.WardCode = '" + cboWard + "' ";
                }

                else if (cboGb.SelectedIndex == 1)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.DeptCode = '" + cboDept + "' ";
                }

                else if (cboGb.SelectedIndex == 2)
                {
                    SQL = SQL + ComNum.VBLF + " AND A.Pano = '" + txtPtno + "' ";
                }

                SQL = SQL + ComNum.VBLF + " AND A.PANO IN (";
                SQL = SQL + ComNum.VBLF + "                SELECT Ptno FROM KOSMOS_OCS.OCS_IORDER ";
                SQL = SQL + ComNum.VBLF + "                 WHERE BDate >= TRUNC(SYSDATE ) ";
                SQL = SQL + ComNum.VBLF + "                   AND OrderCode = 'PT######'";
                SQL = SQL + ComNum.VBLF + "                 GROUP BY Ptno)";
                SQL = SQL + ComNum.VBLF + " GROUP BY   A.PANO, A.Sname,A.RoomCode, A.SEX, A.AGE, A.DEPTCODE";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                nRead = dt1.Rows.Count;

                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    newSlipNo = "0102";

                    if (oldSlipNo != newSlipNo)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "▶";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Get_SlipName("0102");
                        oldSlipNo = newSlipNo;

                        #region Border_Disp
                        Border_Disp();
                        #endregion
                    }

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt1.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt1.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt1.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt1.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt1.Rows[i]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt1.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = "물리치료: " + dt1.Rows[i]["SUCODE"].ToString().Trim() + "-> " + dt1.Rows[i]["SUNAMEK"].ToString().Trim();

                    SQL = "";
                    SQL = "SELECT Pano FROM KOSMOS_PMPA.ETC_PTORDER ";
                    SQL = SQL + ComNum.VBLF + " WHERE CDATE =TRUNC(SYSDATE) ";
                    SQL = SQL + ComNum.VBLF + "    AND PANO ='" + dt1.Rows[i]["PANO"].ToString().Trim() + "'";
                    SQL = SQL + ComNum.VBLF + "    AND GbIO = 'I' ";

                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt2.Rows.Count == 0)
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "미치료";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = "";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "치료";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Mid(strSysDate, 6, 2) + "/" + VB.Right(strSysDate, 2);
                    }

                    dt2.Dispose();
                    dt2 = null;

                    #region Border1_Disp
                    Border1_Disp();
                    #endregion
                }

                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Xray_Disp()
        {
            int i = 0;
            string oldSlipNo = "";
            string newSlipNo = "";

            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT D.XJong,    M.RoomCode,    M.SName,       M.Pano,         ";
                SQL = SQL + ComNum.VBLF + "        M.Sex,      M.Age,         M.DeptCode,    C.XName,        ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(D.SeekDate,'YYYY-MM-DD') SeekDate,  TO_CHAR(D.SeekDate,'hh24:mi') SeekTime, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(D.EnterDate,'YYYY-MM-DD') EnterDate,  D.GbEnd     ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_PMPA.XRAY_DETAIL   D,  ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.XRAY_CODE     C,  ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.IPD_NEW_MASTER    M   ";
                SQL = SQL + ComNum.VBLF + " WHERE  D.SeekDate >= TO_DATE('" + dtpFrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND    D.SeekDate <= TO_DATE('" + dtpToDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND    D.GbReserved <>  '9'  ";
                SQL = SQL + ComNum.VBLF + " AND    D.XJong IN ('2', '3', '4', '5', '6','7')  ";
                SQL = SQL + ComNum.VBLF + " AND    D.Pano      =  M.Pano  ";

                if (cboGb.SelectedIndex == 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.WardCode = '" + cboWard + "' ";
                }
                else if (cboGb.SelectedIndex == 1)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode = '" + cboDept + "' ";
                }
                else if (cboGb.SelectedIndex == 2)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.Pano = '" + txtPtno + "' ";
                }
                    
                SQL = SQL + ComNum.VBLF + " AND    M.GBSTS ='0' ";
                SQL = SQL + ComNum.VBLF + " AND    D.XCode    =  C.XCode  ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY  D.XJong, D.SeekDate, M.RoomCode, M.SName  ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    newSlipNo = dt1.Rows[i]["XJong"].ToString().Trim();

                    if (oldSlipNo != newSlipNo)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "▶";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount, 2].Text = Get_XJongName(dt1.Rows[i]["XJong"].ToString().Trim());

                        oldSlipNo = newSlipNo;

                        #region Border_Disp
                        Border_Disp();
                        #endregion
                    }

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt1.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt1.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt1.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt1.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt1.Rows[i]["Age"].ToString().Trim() + " ";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt1.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt1.Rows[i]["XName"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Mid(dt1.Rows[i]["SeekDate"].ToString().Trim(), 6, 2) + "/" + VB.Mid(dt1.Rows[i]["SeekDate"].ToString().Trim(), 9, 2);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt1.Rows[i]["SeekTime"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Mid(dt1.Rows[i]["EnterDate"].ToString().Trim(), 6, 2) + "/" + VB.Mid(dt1.Rows[i]["EnterDate"].ToString().Trim(), 9, 2);

                    if (dt1.Rows[i]["SeekTime"].ToString().Trim() == "")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "미접수";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "접수";
                    }

                    switch (dt1.Rows[i]["GbEnd"].ToString().Trim())
                    {
                        case "1":
                            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "촬영";
                            break;
                    }

                    #region Border1_Disp
                    Border1_Disp();
                    #endregion
                }

                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Endo_Disp()
        {
            int i = 0;
            string oldSlipNo = "";
            string newSlipNo = "";

            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT M.RoomCode,  M.SName,     M.Pano,      M.Sex,      ";
                SQL = SQL + ComNum.VBLF + "        M.Age,       M.DeptCode,  C.OrderName,             ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(E.RDate,'YYYY-MM-DD') RDate, E.GbSunap,    ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(E.JDate,'YYYY-MM-DD') JDate, E.ResultDate  ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.ENDO_JUPMST   E,  ";
                SQL = SQL + ComNum.VBLF + "        OCS_ORDERCODE            C,  ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.IPD_NEW_MASTER   M   ";
                SQL = SQL + ComNum.VBLF + " WHERE  E.RDate    >=  TO_DATE('" + dtpFrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND    E.RDate    <=  TO_DATE('" + dtpToDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + " AND    E.GbSunap  <>  '*'  ";
                SQL = SQL + ComNum.VBLF + " AND    E.Gbio      =  'I'  ";
                SQL = SQL + ComNum.VBLF + " AND    E.Ptno      =  M.Pano  ";

                if (cboGb.SelectedIndex == 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.WardCode = '" + cboWard + "' ";
                }
                else if (cboGb.SelectedIndex == 1)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode = '" + cboDept + "' ";
                }
                else if (cboGb.SelectedIndex == 2)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.Pano = '" + txtPtno + "' ";
                }
                    
                SQL = SQL + ComNum.VBLF + " AND    M.GBSTS ='0' ";
                SQL = SQL + ComNum.VBLF + " AND    C.SlipNo    =  '0044'  ";
                SQL = SQL + ComNum.VBLF + " AND    C.OrderCode =  E.OrderCode  ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY  E.RDate, M.RoomCode, M.SName  ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    newSlipNo = "0044";

                    if (oldSlipNo != newSlipNo)
                    {
                        ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "▶";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Get_SlipName(newSlipNo);
                        oldSlipNo = newSlipNo;
                    }

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt1.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt1.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt1.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt1.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt1.Rows[i]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt1.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt1.Rows[i]["OrderName"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Mid(dt1.Rows[i]["RDate"].ToString().Trim(), 6, 2) + "/" + VB.Mid(dt1.Rows[i]["RDate"].ToString().Trim(), 9, 2);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "00:00";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Mid(dt1.Rows[i]["JDate"].ToString().Trim(), 6, 2) + "/" + VB.Mid(dt1.Rows[i]["JDate"].ToString().Trim(), 9, 2);

                    if (dt1.Rows[i]["GbSunap"].ToString().Trim() == "2")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "미접수";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "";
                    }
                    else if (dt1.Rows[i]["GbSunap"].ToString().Trim() == "1")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "접수";
                    }

                    if (dt1.Rows[i]["ResultDate"].ToString().Trim() != "")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "결과";
                    }

                    #region Border1_Disp
                    Border1_Disp();
                    #endregion
                }

                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Ekg_Disp()
        {
            int i = 0;
            string oldSlipNo = "";
            string newSlipNo = "";

            DataTable dt1 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + " SELECT M.RoomCode,  M.SName,     M.Pano,      M.Sex,       ";
                SQL = SQL + ComNum.VBLF + "        M.Age,       M.DeptCode,  E.OrderCode, C.OrderName, ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(E.RDate,'YYYY-MM-DD HH24:MI') ResDate,    ";
                SQL = SQL + ComNum.VBLF + "        TO_CHAR(E.EntDate,'YYYY-MM-DD') EntDate,  E.GbJob, E.GuBun   ";
                SQL = SQL + ComNum.VBLF + " FROM   KOSMOS_OCS.ETC_JUPMST     E,  ";
                SQL = SQL + ComNum.VBLF + "        OCS_ORDERCODE             C,  ";
                SQL = SQL + ComNum.VBLF + "        KOSMOS_PMPA.IPD_NEW_MASTER    M   ";
                SQL = SQL + ComNum.VBLF + " WHERE  TO_CHAR(E.RDate,'YYYY-MM-DD') >= '" + dtpFrDate + "' ";
                SQL = SQL + ComNum.VBLF + " AND    TO_CHAR(E.RDate,'YYYY-MM-DD') <= '" + dtpToDate + "' ";
                SQL = SQL + ComNum.VBLF + " AND    E.GbJob     <> '9' ";
                SQL = SQL + ComNum.VBLF + " AND    E.GbIO       = 'I' ";
                SQL = SQL + ComNum.VBLF + " AND    E.Ptno       =  M.Pano ";

                if (cboGb.SelectedIndex == 0)
                {
                    SQL = SQL + ComNum.VBLF + " AND M.WardCode = '" + cboWard + "' ";
                }
                else if (cboGb.SelectedIndex == 1)
                    SQL = SQL + ComNum.VBLF + " AND M.DeptCode = '" + cboDept + "' ";
                else if (cboGb.SelectedIndex == 2)
                    SQL = SQL + ComNum.VBLF + " AND M.Ptno = '" + txtPtno + "' ";

                SQL = SQL + ComNum.VBLF + " AND    M.GBSTS ='0' ";
                SQL = SQL + ComNum.VBLF + " AND    C.OrderCode =  E.OrderCode  ";
                SQL = SQL + ComNum.VBLF + " ORDER  BY  E.RDate, M.RoomCode, M.SName  ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count == 0)
                {
                    dt1.Dispose();
                    dt1 = null;
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                
                for (i = 0; i < dt1.Rows.Count; i++)
                {
                    newSlipNo = dt1.Rows[i]["GuBun"].ToString().Trim();

                    if (oldSlipNo != newSlipNo)
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = "▶";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = Get_FunName(newSlipNo);
                        oldSlipNo = newSlipNo;

                        #region Border_Disp
                        Border_Disp();
                        #endregion
                    }

                    ssView_Sheet1.RowCount = ssView_Sheet1.RowCount + 1;

                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1].Text = dt1.Rows[i]["RoomCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 2].Text = dt1.Rows[i]["SName"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 3].Text = dt1.Rows[i]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 4].Text = dt1.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 5].Text = dt1.Rows[i]["Age"].ToString().Trim() + " ";
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 6].Text = dt1.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 7].Text = dt1.Rows[i]["XName"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = VB.Mid(dt1.Rows[i]["ResDate"].ToString().Trim(), 6, 2) + "/" + VB.Mid(dt1.Rows[i]["ResDate"].ToString().Trim(), 9, 2);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = dt1.Rows[i]["SeekTime"].ToString().Trim();
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 10].Text = VB.Mid(dt1.Rows[i]["EntDate"].ToString().Trim(), 6, 2) + "/" + VB.Mid(dt1.Rows[i]["EntDate"].ToString().Trim(), 9, 2);
                    ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = dt1.Rows[i]["GbJob"].ToString().Trim();

                    if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text == "1")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "미접수";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 8].Text = "";
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 9].Text = "";
                    }
                    else if (ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text == "2")
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "예약";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 11].Text = "접수";
                    }

                    #region Border1_Disp
                    Border1_Disp();
                    #endregion
                }

                dt1.Dispose();
                dt1 = null;
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                Cursor.Current = Cursors.Default;
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Border_Disp()
        {
            strPrtGb = "YES";

            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 255, 255);
            //.CellBorderColor = RGB(192, 192, 192)
            //.CellBorderType = SS_BORDER_TYPE_OUTLINE
            //.CellBorderStyle = SS_BORDER_STYLE_SOLID
            //.Action = SS_ACTION_SET_CELL_BORDER
        }

        private void Border1_Disp()
        {
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(0, 0, 0);
            ssView_Sheet1.Cells[ssView_Sheet1.RowCount - 1, 1, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 255, 255);
            //.CellBorderColor = RGB(192, 192, 192)
            //.CellBorderType = SS_BORDER_TYPE_LEFT Or SS_BORDER_TYPE_RIGHT Or SS_BORDER_TYPE_TOP Or SS_BORDER_TYPE_BOTTOM
            //.CellBorderStyle = SS_BORDER_STYLE_SOLID
            //.Action = SS_ACTION_SET_CELL_BORDER
        }
    }
}
