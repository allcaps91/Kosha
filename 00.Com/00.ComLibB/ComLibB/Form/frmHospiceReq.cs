using ComBase;
using System;
using System.Data;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmHospiceReq
    /// File Name : frmHospiceReq.cs
    /// Title or Description : 호스피스 의뢰 서식 
    /// Author : 안정수
    /// Create Date : 2021-01-26
    /// Update Histroy : 
    /// </summary>
    /// <history>  
    /// </history>
    /// <seealso> 
    /// </seealso>
    /// <vbp>
    /// </vbp>
    public partial class frmHospiceReq : Form
    {
        clsPublic cPublic = new clsPublic();
        string GstrEntDate = "";
        string GstrCDate = "";
        string GstrPtno = "";

        public delegate void EventExit();
        public event EventExit rEventExit;

        public frmHospiceReq()
        {
            InitializeComponent();
        }

        public frmHospiceReq(string argPtno, string argCDATE, string argENTDATE)
        {
            InitializeComponent();
            GstrCDate = argCDATE;
            GstrEntDate = argENTDATE;
            GstrPtno = argPtno;
        }

        private void frmHospiceReq_Load(object sender, EventArgs e)
        {
            read_sysdate();

            //if(clsType.User.IdNumber == "46037")
            //{
            //    btnSave.Visible = false;
            //}
            //else
            //{
                btnSave.Visible = true; 
            //}

            if (GstrEntDate != "" && GstrCDate != "")
            {
                Get_Data(GstrEntDate);
            }
            else
            {
                Get_Data();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            return;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(Chk_Value() == true)
            {
                if(Save_Data() == true)
                {
                    Close();
                }
            }
        }

        void read_sysdate()
        {
            cPublic.strSysDate = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
            cPublic.strSysTime = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T");
        }

        void Get_Data(string argEntDate = "")
        { 
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strDate = "";

            //strDate = (VB.DateAdd("D", -7, ComFunc.FormatStrToDate(cPublic.strSysDate, "D"))).ToString("yyyy-MM-dd");            
            strDate = VB.DateAdd("D", -7, cPublic.strSysDate).ToString();

            SQL = "";
            SQL += ComNum.VBLF + "SELECT *";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_HOSPICE_REQ";
            SQL += ComNum.VBLF + "WHERE 1=1";            

            if (argEntDate == "")
            {
                SQL += ComNum.VBLF + "  AND PANO = '" + clsOrdFunction.Pat.PtNo + "'";
                SQL += ComNum.VBLF + "  AND ENTDATE >= TO_DATE('" + VB.Left(strDate, 10) + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "  AND ENTDATE <= TO_DATE('" + cPublic.strSysDate + "','YYYY-MM-DD')   ";
            }
            else
            {
                SQL += ComNum.VBLF + "  AND PANO = '" + GstrPtno + "'";
                SQL += ComNum.VBLF + "  AND (";
                SQL += ComNum.VBLF + "          (ENTDATE >= TO_DATE('" + VB.Left(argEntDate, 10) + "','YYYY-MM-DD')";
                SQL += ComNum.VBLF + "       AND ENTDATE <= TO_DATE('" + cPublic.strSysDate + "','YYYY-MM-DD'))     ";
                SQL += ComNum.VBLF + "       OR                                                                     ";
                SQL += ComNum.VBLF + "           (JINDATE >= TO_DATE('" + VB.Left(GstrCDate, 10) + "','YYYY-MM-DD') ";
                SQL += ComNum.VBLF + "       AND JINDATE <= TO_DATE('" + cPublic.strSysDate + "','YYYY-MM-DD'))     ";
                SQL += ComNum.VBLF + "      )";
            }

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }

            if (dt.Rows.Count > 0)
            {
                ssView.ActiveSheet.Cells[0, 1].Text = dt.Rows[0]["ILLNAME"].ToString().Trim();

                ssView.ActiveSheet.Cells[1, 1].Text = dt.Rows[0]["JINDATE"].ToString().Trim(); 
                
                if(dt.Rows[0]["GBNOTICE"].ToString().Trim() == "Y")
                {
                    ssView.ActiveSheet.Cells[1, 6].Text = "True";
                }
                else
                {
                    ssView.ActiveSheet.Cells[1, 7].Text = "True";
                }              

                if(dt.Rows[0]["GBPATIENT1"].ToString().Trim() == "Y")
                {
                    ssView.ActiveSheet.Cells[2, 3].Text = "True";
                }
                else
                {
                    ssView.ActiveSheet.Cells[2, 2].Text = "True";
                }

                if (dt.Rows[0]["GBFAMILY1"].ToString().Trim() == "Y")
                {
                    ssView.ActiveSheet.Cells[3, 3].Text = "True";
                }
                else
                {
                    ssView.ActiveSheet.Cells[3, 2].Text = "True";
                }

                if (dt.Rows[0]["GBPATIENT2"].ToString().Trim() == "Y")
                {
                    ssView.ActiveSheet.Cells[2, 8].Text = "True";
                }
                else
                {
                    ssView.ActiveSheet.Cells[2, 7].Text = "True";
                }

                if (dt.Rows[0]["GBFAMILY2"].ToString().Trim() == "Y")
                {
                    ssView.ActiveSheet.Cells[3, 8].Text = "True";
                }
                else
                {
                    ssView.ActiveSheet.Cells[3, 7].Text = "True";
                }

                if(dt.Rows[0]["GBSAYU"].ToString().Trim() == "1")
                {
                    ssView.ActiveSheet.Cells[4, 1].Text = "True";
                }
                else if (dt.Rows[0]["GBSAYU"].ToString().Trim() == "2")
                {
                    ssView.ActiveSheet.Cells[6, 1].Text = "True";
                }
                else if (dt.Rows[0]["GBSAYU"].ToString().Trim() == "3")
                {
                    ssView.ActiveSheet.Cells[4, 1].Text = "True";
                    ssView.ActiveSheet.Cells[6, 1].Text = "True";
                }

                if(dt.Rows[0]["GBSTATUS"].ToString().Trim() == "1")
                {
                    ssView.ActiveSheet.Cells[8, 1].Text = "True";
                }
                else if (dt.Rows[0]["GBSTATUS"].ToString().Trim() == "2")
                {
                    ssView.ActiveSheet.Cells[8, 2].Text = "True";
                }
                else if (dt.Rows[0]["GBSTATUS"].ToString().Trim() == "3")
                {
                    ssView.ActiveSheet.Cells[8, 3].Text = "True";
                }
                else if (dt.Rows[0]["GBSTATUS"].ToString().Trim() == "4")
                {
                    ssView.ActiveSheet.Cells[8, 4].Text = "True";
                }
                else if (dt.Rows[0]["GBSTATUS"].ToString().Trim() == "5")
                {
                    ssView.ActiveSheet.Cells[8, 5].Text = "True";
                    ssView.ActiveSheet.Cells[8, 6].Text = dt.Rows[0]["STATUSETC"].ToString().Trim();                    
                }
            }

            if (dt.Rows.Count == 0)
            {
                ssView.ActiveSheet.Cells[0, 1].Text = READ_ILLS();
                ssView.ActiveSheet.Cells[1, 1].Text = cPublic.strSysDate;                
            }

            dt.Dispose();
            dt = null;
        }

        public string READ_ILLS()
        {
            string rtnVal = "";
            string SQL = "";
            string SqlErr = "";
            DataTable dt2 = null;

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                                ";
            SQL += ComNum.VBLF + "  A.ILLCODE                                                                           ";
            SQL += ComNum.VBLF + ",(SELECT ILLNAMEE                                                                     ";
            SQL += ComNum.VBLF + "    FROM ADMIN.BAS_ILLS                                                         ";
            SQL += ComNum.VBLF + "   WHERE 1=1                                                                          ";
            SQL += ComNum.VBLF + "     AND ILLCODE = A.ILLCODE                                                          ";
            SQL += ComNum.VBLF + "     AND ILLCLASS = '1'                                                               ";
            SQL += ComNum.VBLF + "     AND ROWNUM = 1                                                                   ";
            SQL += ComNum.VBLF + " ) AS ILLNAME                                                                         ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "OCS_IILLS A                                                 ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                             ";
            SQL += ComNum.VBLF + "  AND PTNO = '" + clsOrdFunction.Pat.PtNo + "'                                        ";
            SQL += ComNum.VBLF + "  AND ILLCODE LIKE 'C%'                                                               ";
            SQL += ComNum.VBLF + "  AND ROWNUM = 1                                                                      ";
            SQL += ComNum.VBLF + "ORDER BY A.ENTDATE DESC                                                               ";
            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt2.Rows.Count > 0)
            {
                rtnVal = dt2.Rows[0]["ILLNAME"].ToString().Trim();
            }

            dt2.Dispose();
            dt2 = null;

            return rtnVal;
        }

        bool Chk_Value()
        {
            bool rtnVal = false;

            if(ssView.ActiveSheet.Cells[0, 1].Text == "")
            {
                ComFunc.MsgBox("진단명을 입력해주세요.");
                return rtnVal;
            }

            if(ssView.ActiveSheet.Cells[1, 1].Text == "")
            {
                ComFunc.MsgBox("진단일을 입력해주세요.");
                return rtnVal;
            }

            if (ssView.ActiveSheet.Cells[1, 6].Text == "" && ssView.ActiveSheet.Cells[1, 7].Text == "")
            {
                ComFunc.MsgBox("말기 통보 유무를 체크해주세요.");
                return rtnVal;
            }

            if (ssView.ActiveSheet.Cells[2, 2].Text == "" && ssView.ActiveSheet.Cells[2, 3].Text == "")
            {
                ComFunc.MsgBox("(환자)말기 병식 유무를 체크해주세요.");
                return rtnVal;
            }

            if (ssView.ActiveSheet.Cells[3, 2].Text == "" && ssView.ActiveSheet.Cells[3, 3].Text == "")
            {
                ComFunc.MsgBox("(가족)말기 병식 유무를 체크해주세요.");
                return rtnVal;
            }

            if (ssView.ActiveSheet.Cells[2, 7].Text == "" && ssView.ActiveSheet.Cells[2, 8].Text == "")
            {
                ComFunc.MsgBox("(환자)말기 예후의 인식을 체크해주세요.");
                return rtnVal;
            }

            if (ssView.ActiveSheet.Cells[3, 7].Text == "" && ssView.ActiveSheet.Cells[3, 8].Text == "")
            {
                ComFunc.MsgBox("(가족)말기 예후의 인식을 체크해주세요.");
                return rtnVal;
            }

            if ((ssView.ActiveSheet.Cells[4, 1].Text == "False" || ssView.ActiveSheet.Cells[4, 1].Text == "")
                && (ssView.ActiveSheet.Cells[6, 1].Text == "False" || ssView.ActiveSheet.Cells[6, 1].Text == ""))
            {
                ComFunc.MsgBox("말기암 진단 근거를 체크해주세요.");
                return rtnVal;
            }

            if (ssView.ActiveSheet.Cells[8, 1].Text == "" && ssView.ActiveSheet.Cells[8, 2].Text == ""
                && ssView.ActiveSheet.Cells[8, 3].Text == "" && ssView.ActiveSheet.Cells[8, 4].Text == ""
                && ssView.ActiveSheet.Cells[8, 5].Text == "")
            {
                ComFunc.MsgBox("의식 상태를 체크해주세요.");
                return rtnVal;
            }

            if (ssView.ActiveSheet.Cells[8, 5].Text == "True" && ssView.ActiveSheet.Cells[8, 6].Text == "")
            {
                ComFunc.MsgBox("의식상태 기타사유를 입력해주세요.");
                return rtnVal;
            }

            rtnVal = true;
            return rtnVal;
        }

        bool Save_Data()
        {            
            int intRowAffected = 0; //변경된 Row 받는 변수

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            string strROWID = "";

            DataTable dt = null;

            bool rtnVal = false;

            string strILLNAME = "";
            string strJINDATE = "";
            string strGBNOTICE = "";
            string strGBFAMILY1 = "";
            string strGBFAMILY2 = "";
            string strGBPATIENT1 = "";
            string strGBPATIENT2 = "";
            string strGBSAYU = "";
            string strGBSTATUS = "";
            string strSTATUSETC = "";
            string strPANO = "";
            string strENTDATE = "";

            #region 변수 내용 

            strILLNAME = ssView.ActiveSheet.Cells[0, 1].Text;
            strJINDATE = ssView.ActiveSheet.Cells[1, 1].Text;

            if (ssView.ActiveSheet.Cells[1, 6].Text == "True")
            {
                strGBNOTICE = "Y";
            }
            else if (ssView.ActiveSheet.Cells[1, 7].Text == "True")
            {
                strGBNOTICE = "N";
            }

            if (ssView.ActiveSheet.Cells[2, 2].Text == "True")
            {
                strGBPATIENT1 = "N";
            }
            else
            {
                strGBPATIENT1 = "Y";
            }

            if (ssView.ActiveSheet.Cells[3, 2].Text == "True")
            {
                strGBFAMILY1 = "N";
            }
            else
            {
                strGBFAMILY1 = "Y";
            }

            if (ssView.ActiveSheet.Cells[2, 7].Text == "True")
            {
                strGBPATIENT2 = "N";
            }
            else
            {
                strGBPATIENT2 = "Y";
            }

            if (ssView.ActiveSheet.Cells[3, 7].Text == "True")
            {
                strGBFAMILY2 = "N";
            }
            else
            {
                strGBFAMILY2 = "Y";
            }

            if (ssView.ActiveSheet.Cells[4, 1].Text == "True" && ssView.ActiveSheet.Cells[6, 1].Text == "False")
            {
                strGBSAYU = "1";
            }
            else if ((ssView.ActiveSheet.Cells[4, 1].Text == "False" || ssView.ActiveSheet.Cells[4, 1].Text == "") && ssView.ActiveSheet.Cells[6, 1].Text == "True")
            {
                strGBSAYU = "2";
            }
            else if (ssView.ActiveSheet.Cells[4, 1].Text == "True" && ssView.ActiveSheet.Cells[6, 1].Text == "True")
            {
                strGBSAYU = "3";
            }

            if (ssView.ActiveSheet.Cells[8, 1].Text == "True")
            {
                strGBSTATUS = "1";
            }
            else if (ssView.ActiveSheet.Cells[8, 2].Text == "True")
            {
                strGBSTATUS = "2";
            }
            else if (ssView.ActiveSheet.Cells[8, 3].Text == "True")
            {
                strGBSTATUS = "3";
            }
            else if (ssView.ActiveSheet.Cells[8, 4].Text == "True")
            {
                strGBSTATUS = "4";
            }
            else if (ssView.ActiveSheet.Cells[8, 5].Text == "True")
            {
                strGBSTATUS = "5";
                strSTATUSETC = ssView.ActiveSheet.Cells[8, 6].Text.Trim();
            }

            strPANO = clsOrdFunction.Pat.PtNo;
            strENTDATE = cPublic.strSysDate;

            #endregion

            SQL = "";
            SQL += ComNum.VBLF + "SELECT ROWID";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_MED + "ETC_HOSPICE_REQ";
            SQL += ComNum.VBLF + "WHERE 1=1";
            SQL += ComNum.VBLF + "  AND PANO = '" + clsOrdFunction.Pat.PtNo + "'";
            SQL += ComNum.VBLF + "  AND ENTDATE = TO_DATE('" + strENTDATE + "', 'YYYY-MM-DD')";
            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }

            if (dt.Rows.Count > 0)
            {
                strROWID = dt.Rows[0]["ROWID"].ToString().Trim();
            }

            dt.Dispose();
            dt = null;

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            if (strROWID == "")
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO ADMIN.ETC_HOSPICE_REQ";
                SQL += ComNum.VBLF + "(ILLNAME, JINDATE, GBNOTICE, GBPATIENT1, GBFAMILY1";
                SQL += ComNum.VBLF + ",GBPATIENT2, GBFAMILY2, GBSAYU, GBSTATUS, STATUSETC";
                SQL += ComNum.VBLF + ",PANO, ENTDATE)";
                SQL += ComNum.VBLF + "VALUES(";
                SQL += ComNum.VBLF + " '" + strILLNAME + "'";
                SQL += ComNum.VBLF + ",'" + strJINDATE + "'";
                SQL += ComNum.VBLF + ",'" + strGBNOTICE + "'";
                SQL += ComNum.VBLF + ",'" + strGBPATIENT1 + "'";
                SQL += ComNum.VBLF + ",'" + strGBFAMILY1 + "'";
                SQL += ComNum.VBLF + ",'" + strGBPATIENT2 + "'";
                SQL += ComNum.VBLF + ",'" + strGBFAMILY2 + "'";
                SQL += ComNum.VBLF + ",'" + strGBSAYU + "'";
                SQL += ComNum.VBLF + ",'" + strGBSTATUS + "'";
                SQL += ComNum.VBLF + ",'" + strSTATUSETC + "'";
                SQL += ComNum.VBLF + ",'" + strPANO + "'";
                SQL += ComNum.VBLF + ",'" + strENTDATE + "'";
                SQL += ComNum.VBLF + ")";
            }
            else
            {
                SQL = "";
                SQL += ComNum.VBLF + "UPDATE ADMIN.ETC_HOSPICE_REQ";                
                SQL += ComNum.VBLF + "SET ";
                SQL += ComNum.VBLF + "  ILLNAME = '" + strILLNAME + "'";
                SQL += ComNum.VBLF + ", JINDATE = '" + strJINDATE + "'";
                SQL += ComNum.VBLF + ", GBNOTICE = '" + strGBNOTICE + "'";
                SQL += ComNum.VBLF + ", GBPATIENT1 = '" + strGBPATIENT1 + "'";
                SQL += ComNum.VBLF + ", GBFAMILY1 = '" + strGBFAMILY1 + "'";
                SQL += ComNum.VBLF + ", GBPATIENT2 = '" + strGBPATIENT2 + "'";
                SQL += ComNum.VBLF + ", GBFAMILY2 = '" + strGBFAMILY2 + "'";
                SQL += ComNum.VBLF + ", GBSAYU = '" + strGBSAYU + "'";
                SQL += ComNum.VBLF + ", GBSTATUS = '" + strGBSTATUS + "'";
                SQL += ComNum.VBLF + ", STATUSETC = '" + strSTATUSETC + "'";
                SQL += ComNum.VBLF + ", PANO = '" + strPANO + "'";
                SQL += ComNum.VBLF + ", ENTDATE = '" + strENTDATE + "'";
                SQL += ComNum.VBLF + "WHERE ROWID = '" + strROWID + "'";
            }

            try
            {
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                rtnVal = true;
                return rtnVal;
            }

            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }

        }

    }
}
