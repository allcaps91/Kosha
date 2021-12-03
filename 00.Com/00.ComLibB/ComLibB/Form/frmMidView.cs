using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmMidView.cs
    /// Description     : 의무기록 조건검색
    /// Author          : 박창욱
    /// Create Date     : 2017-07-14
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "\Ocs\OpdOcs\ojumst\CsInfo53.frm(FrmMidView.frm) >> frmMidView.cs 폼이름 재정의" />	
    public partial class frmMidView : Form
    {
        int nRow = 0;
        int FnInx = 0;

        string GstrPANO = "";
        public delegate void SetPano(string strPano);
        public event SetPano rSetPano;

        public delegate void EventClose();
        public event EventClose rEventClose;

        public frmMidView()
        {
            InitializeComponent();
        }

        void Display_Diagnosis()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            int i = 0;
            int nREAD = 0;
            string SQLSub = "";
            string strPano = "";
            string strOutDate = "";
            string strDeptCode = "";
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (txtCode1.Text.Trim() == "")
            {
                ComFunc.MsgBox("조회하실 상병, 수술코드가 공란입니다");
                return;
            }

            if (txtCode2.Text.Trim() != "" && txtCode3.Text.Trim() != "")
            {
                SQLSub = " AND Diagnosis1 IN ('" + txtCode1.Text.Trim() + "','";
                SQLSub = SQLSub + txtCode2.Text.Trim() + "','" + txtCode3.Text.Trim() + "') ";
            }
            else if (txtCode2.Text.Trim() != "")
            {
                SQLSub = " AND Diagnosis1 IN ('" + txtCode1.Text.Trim() + "','";
                SQLSub = SQLSub + txtCode2.Text.Trim() + "') ";
            }
            else
            {
                SQLSub = " AND Diagnosis1 = '" + txtCode1.Text.Trim() + "' ";
            }

            strDeptCode = VB.Left(cboDept.Text, 2);

            try
            {
                SQL = "";
                SQL = "SELECT Pano cPano,TO_CHAR(OUTDATE,'YYYY-MM-DD') cOutDate ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.MID_DIAGNOSIS ";
                SQL = SQL + ComNum.VBLF + " WHERE OutDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND OutDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (chkMsym.Checked == true)
                {
                    SQL = SQL + " AND SeqNo = 1 ";
                }
                SQL = SQL + ComNum.VBLF + SQLSub;
                SQL = SQL + ComNum.VBLF + "   AND ROWNUM < 5001 ";
                SQL = SQL + ComNum.VBLF + " GROUP BY Pano,OutDate ";
                SQL = SQL + ComNum.VBLF + " ORDER BY Pano,OutDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["cPano"].ToString().Trim();
                    strOutDate = dt.Rows[i]["cOutDate"].ToString().Trim();

                    SQL = "";
                    SQL = "SELECT Pano,TO_CHAR(OUTDATE,'YYYY-MM-DD') OutDate,";
                    SQL = SQL + ComNum.VBLF + "    Sname,Idept,Tdept,TDoctor,TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                    SQL = SQL + ComNum.VBLF + "    Jilsu,Jumin2,Age,Bi,CResult,TModel,GbDie,Kukso,Kukso1,";
                    SQL = SQL + ComNum.VBLF + "    Kukso2,Kukso3,Sgun1,SGun1_B, sancd,NbGb,BabyType, ";
                    SQL = SQL + ComNum.VBLF + "    ADMIN.FC_BAS_DOCTOR_DRNAME(TDoctor) As TDoctor_Name";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.MID_SUMMARY ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND OutDate = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                    if (strDeptCode != "**")
                    {
                        SQL = SQL + ComNum.VBLF + " AND TDept = '" + strDeptCode + "' ";
                    }
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                    if (dt2.Rows.Count == 1)
                    {
                        FnInx = 0;
                    }
                    Display_One_Pano(dt2);
                    dt2.Dispose();
                    dt2 = null;

                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        void Display_Etc()
        {
            int i = 0;
            int nREAD = 0;
            string strPano = "";
            string strOutDate = "";
            string strDeptCode = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            strDeptCode = VB.Left(cboDept.Text, 2);

            if (VB.Left(cboJong.Text, 2) == "11")
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 26].Text = "암종";
            }
            else
            {
                ssView_Sheet1.ColumnHeader.Cells[0, 26].Text = "산모";
            }

            try
            {
                SQL = "";
                SQL = "SELECT    Pano,TO_CHAR(OUTDATE,'YYYY-MM-DD') OutDate,";
                SQL = SQL + ComNum.VBLF + "    Sname,Idept,Tdept,TDoctor,TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                SQL = SQL + ComNum.VBLF + "    Jilsu,Jumin2,Age,Bi,CResult,TModel,GbDie,Kukso,Kukso1,";
                SQL = SQL + ComNum.VBLF + "    Kukso2,Kukso3,Sgun1,SGun1_B,Sancd,NbGb,BabyType,Canc, ";
                SQL = SQL + ComNum.VBLF + "    ADMIN.FC_BAS_DOCTOR_DRNAME(TDoctor) As TDoctor_Name";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.MID_SUMMARY ";
                SQL = SQL + ComNum.VBLF + " WHERE OutDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND OutDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (strDeptCode != "**")
                {
                    SQL = SQL + " AND Tdept = '" + strDeptCode + "' ";
                }
                switch (VB.Left(cboJong.Text, 2))
                {
                    case "03":
                        SQL = SQL + ComNum.VBLF + " AND (Kukso > 0 OR Kukso1 > 0 OR Kukso2 > 0 Or Kukso3 > 0) ";
                        break;
                    case "04":
                        SQL = SQL + ComNum.VBLF + " AND (SGUN1 > 0 OR SGUN1_B > 0) ";
                        break;
                    case "05":
                        SQL = SQL + ComNum.VBLF + " AND (GbDie IN ('1','2','3','4') OR Tmodel='5') ";
                        break;
                    case "06":
                        SQL = SQL + ComNum.VBLF + " AND Tmodel = '4' ";
                        break;
                    case "07":
                        SQL = SQL + ComNum.VBLF + " AND Sancd IN ('2','5','6') ";
                        break;
                    case "08":
                        SQL = SQL + ComNum.VBLF + " AND Sancd = '7' ";
                        break;
                    case "10":
                        SQL = SQL + ComNum.VBLF + " AND IpKyng = '1' ";
                        break;
                }
                if (VB.Left(cboJong.Text, 2) == "09")
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY OutDate,Pano ";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY Pano,OutDate ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    FnInx = i;
                    Display_One_Pano(dt);
                }
                dt.Dispose();
                dt = null;

                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

        }

        void Display_One_Pano(DataTable argDt)
        {
            int i = 0;
            int Inx = 0;
            string Chk = "";
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            try
            {
                if (VB.Left(cboJong.Text, 2) == "11")
                {
                    //상병을 READ
                    SQL = "";
                    SQL = "SELECT Diagnosis1 FROM ADMIN.MID_DIAGNOSIS ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + argDt.Rows[FnInx]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND OUTDATE = TO_DATE('" + argDt.Rows[FnInx]["OutDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND ROWNUM <  5 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo,Diagnosis1 ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            if (VB.Left(dt.Rows[i]["Diagnosis1"].ToString().Trim(), 2) == "M8" && VB.Mid(dt.Rows[i]["Diagnosis1"].ToString().Trim(), 6, 1) == "3"
                                || VB.Mid(dt.Rows[i]["Diagnosis1"].ToString().Trim(), 6, 1) == "6")
                            {
                                Chk = "*";
                            }
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    if (Chk == "*")
                    {
                        nRow += 1;
                        if (nRow > ssView_Sheet1.RowCount)
                        {
                            ssView_Sheet1.RowCount = nRow;
                            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                        }

                        ssView_Sheet1.Cells[nRow - 1, 0].Text = argDt.Rows[FnInx]["Pano"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 1].Text = argDt.Rows[FnInx]["OutDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 2].Text = argDt.Rows[FnInx]["Sname"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 3].Text = argDt.Rows[FnInx]["Idept"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 4].Text = argDt.Rows[FnInx]["Tdept"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 5].Text = argDt.Rows[FnInx]["TDoctor_Name"].ToString().Trim();  //READ_DrName_CSINFO(TDoctor)
                        ssView_Sheet1.Cells[nRow - 1, 6].Text = argDt.Rows[FnInx]["InDate"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 7].Text = argDt.Rows[FnInx]["Jilsu"].ToString().Trim();
                        if (VB.Left(argDt.Rows[FnInx]["Jumin2"].ToString().Trim(), 1) == "1" || VB.Left(argDt.Rows[FnInx]["Jumin2"].ToString().Trim(), 1) == "3")
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "남";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[nRow - 1, 8].Text = "여";
                        }
                        ssView_Sheet1.Cells[nRow - 1, 9].Text = argDt.Rows[FnInx]["Age"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 10].Text = READ_Bi_Name(argDt.Rows[FnInx]["Bi"].ToString().Trim());
                        ssView_Sheet1.Cells[nRow - 1, 11].Text = argDt.Rows[FnInx]["TModel"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 12].Text = argDt.Rows[FnInx]["CResult"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 20].Text = argDt.Rows[FnInx]["GbDie"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 21].Text = argDt.Rows[FnInx]["Kukso"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 22].Text = argDt.Rows[FnInx]["Kukso1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 23].Text = argDt.Rows[FnInx]["Kukso2"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 24].Text = argDt.Rows[FnInx]["NbGb"].ToString().Trim() + "/" + argDt.Rows[FnInx]["BabyType"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 25].Text = argDt.Rows[FnInx]["SGun1"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 26].Text = argDt.Rows[FnInx]["SGun1_B"].ToString().Trim();
                        ssView_Sheet1.Cells[nRow - 1, 27].Text = argDt.Rows[FnInx]["Canc"].ToString().Trim();

                        //상병을 READ
                        SQL = "";
                        SQL = "SELECT Diagnosis1 FROM ADMIN.MID_DIAGNOSIS ";
                        SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + argDt.Rows[FnInx]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND OUTDATE = TO_DATE('" + argDt.Rows[FnInx]["OutDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND ROWNUM  <  5 ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo,Diagnosis1 ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssView_Sheet1.Cells[nRow - 1, i + 13].Text = dt.Rows[i]["Diagnosis1"].ToString().Trim();
                            }
                        }

                        dt.Dispose();
                        dt = null;

                        //수술코드 Display
                        SQL = "";
                        SQL = "SELECT Operation FROM ADMIN.MID_OP ";
                        SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + argDt.Rows[FnInx]["Pano"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND OUTDATE = TO_DATE('" + argDt.Rows[FnInx]["OutDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND ROWNUM  < 4 ";
                        SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo,Operation ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }
                        if (dt.Rows.Count > 0)
                        {
                            for (i = 0; i < dt.Rows.Count; i++)
                            {
                                ssView_Sheet1.Cells[nRow - 1, i + 17].Text = dt.Rows[i]["Operation"].ToString().Trim();
                            }
                        }

                        dt.Dispose();
                        dt = null;
                    }
                }
                else
                {
                    nRow += 1;
                    if (nRow > ssView_Sheet1.RowCount)
                    {
                        ssView_Sheet1.RowCount = nRow;
                        ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
                    }
                    ssView_Sheet1.Cells[nRow - 1, 0].Text = argDt.Rows[FnInx]["Pano"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 1].Text = argDt.Rows[FnInx]["OutDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 2].Text = argDt.Rows[FnInx]["Sname"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 3].Text = argDt.Rows[FnInx]["Idept"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 4].Text = argDt.Rows[FnInx]["Tdept"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 5].Text = argDt.Rows[FnInx]["TDoctor_Name"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 6].Text = argDt.Rows[FnInx]["InDate"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 7].Text = argDt.Rows[FnInx]["Jilsu"].ToString().Trim();
                    if (VB.Left(argDt.Rows[FnInx]["Jumin2"].ToString().Trim(), 1) == "1" || VB.Left(argDt.Rows[FnInx]["Jumin2"].ToString().Trim(), 1) == "3")
                    {
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = "남";
                    }
                    else
                    {
                        ssView_Sheet1.Cells[nRow - 1, 8].Text = "여";
                    }
                    ssView_Sheet1.Cells[nRow - 1, 9].Text = argDt.Rows[FnInx]["Age"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 10].Text = READ_Bi_Name(argDt.Rows[FnInx]["Bi"].ToString().Trim());
                    ssView_Sheet1.Cells[nRow - 1, 11].Text = argDt.Rows[FnInx]["TModel"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 12].Text = argDt.Rows[FnInx]["CResult"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 20].Text = argDt.Rows[FnInx]["GbDie"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 21].Text = argDt.Rows[FnInx]["Kukso"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 22].Text = argDt.Rows[FnInx]["Kukso1"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 23].Text = argDt.Rows[FnInx]["Kukso2"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 24].Text = argDt.Rows[FnInx]["NbGb"].ToString().Trim() + "/" + argDt.Rows[FnInx]["BabyType"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 25].Text = argDt.Rows[FnInx]["SGun1"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 26].Text = argDt.Rows[FnInx]["SGun1_B"].ToString().Trim();
                    ssView_Sheet1.Cells[nRow - 1, 27].Text = argDt.Rows[FnInx]["Sancd"].ToString().Trim();

                    //상병을 Display
                    SQL = "";
                    SQL = "SELECT Diagnosis1 FROM ADMIN.MID_DIAGNOSIS ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + argDt.Rows[FnInx]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND OUTDATE = TO_DATE('" + argDt.Rows[FnInx]["OutDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND Diagnosis1 > ' ' ";
                    SQL = SQL + ComNum.VBLF + "  AND ROWNUM  < 5 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo,Diagnosis1 ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssView_Sheet1.Cells[nRow - 1, i + 13].Text = dt.Rows[i]["Diagnosis1"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;

                    //수술코드 Display
                    SQL = "";
                    SQL = "SELECT Operation FROM ADMIN.MID_OP ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + argDt.Rows[FnInx]["Pano"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND OUTDATE = TO_DATE('" + argDt.Rows[FnInx]["OutDate"].ToString().Trim() + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "  AND Operation > ' ' ";
                    SQL = SQL + ComNum.VBLF + "  AND ROWNUM  < 4 ";
                    SQL = SQL + ComNum.VBLF + "ORDER BY SeqNo,Operation ";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }
                    if (dt.Rows.Count > 0)
                    {
                        for (i = 0; i < dt.Rows.Count; i++)
                        {
                            ssView_Sheet1.Cells[nRow - 1, i + 17].Text = dt.Rows[i]["Operation"].ToString().Trim();
                        }
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        string READ_Bi_Name(string argBi)
        {
            string rtn = "";

            switch (argBi)
            {
                case "11":
                    rtn = "건강보험";
                    break;
                case "12":
                    rtn = "건강보험";
                    break;
                case "13":
                    rtn = "건강보험";
                    break;
                case "21":
                    rtn = "의료급여1종";
                    break;
                case "22":
                    rtn = "의료급여2종";
                    break;
                case "23":
                    rtn = "의료급여3종";
                    break;
                case "24":
                    rtn = "행려환자";
                    break;
                case "31":
                    rtn = "산재";
                    break;
                case "32":
                    rtn = "공상";
                    break;
                case "33":
                    rtn = "산재공상";
                    break;
                case "41":
                    rtn = "공단100%";
                    break;
                case "42":
                    rtn = "직장100%";
                    break;
                case "43":
                    rtn = "지역100%";
                    break;
                case "44":
                    rtn = "가족계획";
                    break;
                case "51":
                    rtn = "일반";
                    break;
                case "52":
                    rtn = "TA보험";
                    break;
                case "53":
                    rtn = "계약처";
                    break;
                case "54":
                    rtn = "미확인";
                    break;
                case "55":
                    rtn = "TA일반";
                    break;
                default:
                    rtn = "";
                    break;
            }
            return rtn;
        }

        void Display_OpCode()
        {
            int i = 0;
            int nREAD = 0;
            string SQLSub = "";
            string strPano = "";
            string strOutDate = "";
            string strDeptCode = "";
            DataTable dt = null;
            DataTable dt2 = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            if (txtCode1.Text.Trim() == "")
            {
                ComFunc.MsgBox("조회하실 수술코드가 공란입니다.");
                return;
            }


            if (txtCode2.Text.Trim() != "" && txtCode3.Text.Trim() != "")
            {
                SQLSub = " AND Operation IN ('" + txtCode1.Text.Trim() + "','";
                SQLSub = SQLSub + txtCode2.Text.Trim() + "','" + txtCode3.Text.Trim() + "')";
            }
            else if (txtCode2.Text.Trim() != "")
            {
                SQLSub = " AND Operation IN ('" + txtCode1.Text.Trim() + "','";
                SQLSub = SQLSub + txtCode2.Text.Trim() + "')";
            }
            else
            {
                SQLSub = " AND Operation = '" + txtCode1.Text.Trim() + "' ";
            }

            strDeptCode = VB.Left(cboDept.Text, 2);

            try
            {
                SQL = "";
                SQL = "SELECT Pano cPano,TO_CHAR(OUTDATE,'YYYY-MM-DD') cOutDate ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.MID_OP ";
                SQL = SQL + ComNum.VBLF + " WHERE OutDate >= TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND OutDate <= TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                if (chkMsym.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + " AND SeqNo = 1 ";
                }
                SQL = SQL + ComNum.VBLF + SQLSub;
                SQL = SQL + ComNum.VBLF + " GROUP BY Pano,OutDate ";
                SQL = SQL + ComNum.VBLF + " ORDER BY Pano,OutDate ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                nREAD = dt.Rows.Count;

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    strPano = dt.Rows[i]["cPano"].ToString().Trim();
                    strOutDate = dt.Rows[i]["cOutDate"].ToString().Trim();

                    SQL = "";
                    SQL = "SELECT Pano,TO_CHAR(OUTDATE,'YYYY-MM-DD') OutDate,";
                    SQL = SQL + ComNum.VBLF + "    Sname,Idept,Tdept,TDoctor,TO_CHAR(InDate,'YYYY-MM-DD') InDate,";
                    SQL = SQL + ComNum.VBLF + "    Jilsu,Jumin2,Age,Bi,CResult,TModel,GbDie,Kukso,Kukso1,";
                    SQL = SQL + ComNum.VBLF + "    Kukso2,Kukso3,Sgun1,SGun1_B,Sancd,NbGb,BabyType, ";
                    SQL = SQL + ComNum.VBLF + "    ADMIN.FC_BAS_DOCTOR_DRNAME(TDoctor) As TDoctor_Name";
                    SQL = SQL + ComNum.VBLF + " FROM ADMIN.MID_SUMMARY ";
                    SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + strPano + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND OutDate = TO_DATE('" + strOutDate + "','YYYY-MM-DD') ";
                    if (strDeptCode != "**")
                    {
                        SQL = SQL + ComNum.VBLF + " AND TDept = '" + strDeptCode + "' ";
                    }
                    SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        return;
                    }

                    if (dt2.Rows.Count == 1)
                    {
                        FnInx = 0;
                        Display_One_Pano(dt2);
                    }
                    dt2.Dispose();
                    dt2 = null;
                }
                dt.Dispose();
                dt = null;
                ssView_Sheet1.RowCount = nRow;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                return; //권한 확인

            string strJong = "";

            btnSearch.Enabled = false;
            strJong = VB.Left(cboJong.Text, 2);

            nRow = 0;
            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);
            ssView_Sheet1.Cells[0, 0, 49, ssView_Sheet1.ColumnCount - 1].Text = "";

            if (strJong == "01")
            {
                Display_Diagnosis();
            }
            else if (strJong == "02")
            {
                Display_OpCode();
            }
            else
            {
                Display_Etc();
            }

            btnSearch.Enabled = true;
        }

        private void frmMidView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
                this.Close(); //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddDays(-60);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            txtCode1.Text = "";
            txtCode2.Text = "";
            txtCode3.Text = "";

            //진료과 Combo Set
            try
            {
                SQL = "";
                SQL = "SELECT DeptCode,DeptNameK ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.BAS_CLINICDEPT ";
                SQL = SQL + ComNum.VBLF + "WHERE DeptCode NOT IN ('II','R6','TO','AN','HR','PT','CS','ER') ";
                SQL = SQL + ComNum.VBLF + "ORDER BY PrintRanking,DeptCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                cboDept.Items.Clear();
                cboDept.Items.Add("**.전체과");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboDept.Items.Add(dt.Rows[i]["DeptCode"].ToString().Trim() + "." + dt.Rows[i]["DeptNameK"].ToString().Trim());
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            cboDept.SelectedIndex = 0;

            //작업종류 ComboSet
            cboJong.Items.Clear();
            cboJong.Items.Add("01.상병코드별");
            cboJong.Items.Add("02.수술코드별");
            cboJong.Items.Add("03.마취자명단");
            cboJong.Items.Add("04.생검자명단");
            cboJong.Items.Add("05.사망자명단");
            cboJong.Items.Add("07.분만자명단");
            cboJong.Items.Add("08.사산자명단");
            cboJong.Items.Add("09.퇴원자전체");
            cboJong.Items.Add("10.응급실경유");
            cboJong.Items.Add("11.암환자명단");
            cboJong.SelectedIndex = 0;
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssView_Sheet1.RowCount == 0)
            {
                return;
            }

            if (e.ColumnHeader == true)
            {
                clsSpread.gSpdSortRow(ssView, e.Column);
                return;
            }

            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
            ssView_Sheet1.Cells[e.Row, 0, e.Row, ssView_Sheet1.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;

            GstrPANO = ssView_Sheet1.Cells[e.Row, 0].Text;
            rSetPano(GstrPANO);
            //FrmCSInfoView.Show
        }

        private void ssView_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strCode = "";
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            switch (e.NewColumn)
            {
                case 11:
                    lblMsg.Text = "퇴원형태 : 1.퇴원지시후 2.자의퇴원 3.전송 4.탈원 5.사망";
                    break;
                case 12:
                    lblMsg.Text = "퇴원결과: 1.완쾌 2.호전 3.호전안됨 4.치료못함 5.진단뿐 6.가망없는 퇴원 7.48이전사망, 8.48이후사망";
                    break;
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                    //상병, 수술명칭 Display
                    strCode = ssView_Sheet1.Cells[e.NewRow, e.NewColumn].Text;
                    if (strCode == "")
                    {
                        lblMsg.Text = "";
                        return;
                    }

                    //상병, 수술명을 READ
                    try
                    {
                        SQL = "";
                        SQL = "SELECT IllNameE, IllNameK FROM ADMIN.BAS_ILLS ";
                        SQL = SQL + ComNum.VBLF + "WHERE IllCode = '" + strCode + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            lblMsg.Text = dt.Rows[0]["IllNameE"].ToString().Trim() + "(" + dt.Rows[0]["IllNameK"].ToString().Trim() + ")";
                        }
                        dt.Dispose();
                        dt = null;
                    }
                    catch (Exception ex)
                    {
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                        ComFunc.MsgBox(ex.Message);
                    }
                    break;
                case 20:
                    lblMsg.Text = "사망구분: 1.수술중 2.마취사망 3.수술후 10일이내 4.신생아 사망";
                    break;
                default:
                    lblMsg.Text = "";
                    break;
            }
        }

        private void txtCode1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCode2.Focus();
                txtCode1.Text = txtCode1.Text.ToUpper();
            }
        }

        private void txtCode2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtCode3.Focus();
                txtCode2.Text = txtCode2.Text.ToUpper();
            }
        }

        private void txtCode3_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btnSearch.Focus();
                txtCode3.Text = txtCode3.Text.ToUpper();
            }
        }

        private void dtpFDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dtpTDate.Focus();
            }
        }

        private void dtpTDate_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                cboDept.Focus();
            }
        }

        private void cboJong_SelectedIndexChanged(object sender, EventArgs e)
        {
            string strJong = "";

            txtCode1.Text = "";
            txtCode2.Text = "";
            txtCode3.Text = "";
            strJong = VB.Left(cboJong.Text, 2);

            if (strJong == "01")
            {
                grbMsym.Enabled = true;
                grbMsym.Text = "찾으실 상병코드는?";
                chkMsym.Text = "주상병만 찾기";
            }
            else if (strJong == "02")
            {
                grbMsym.Enabled = true;
                grbMsym.Text = "찾으실 수술코드는?";
                chkMsym.Text = "주수술만 찾기";
            }
            else
            {
                grbMsym.Enabled = false;
                chkMsym.Checked = false;
            }
        }

        private void txtCode1_Leave(object sender, EventArgs e)
        {
            txtCode1.Text = txtCode1.Text.ToUpper();
        }

        private void txtCode2_Leave(object sender, EventArgs e)
        {
            txtCode2.Text = txtCode2.Text.ToUpper();
        }

        private void txtCode3_Leave(object sender, EventArgs e)
        {
            txtCode3.Text = txtCode3.Text.ToUpper();
        }
    }
}
