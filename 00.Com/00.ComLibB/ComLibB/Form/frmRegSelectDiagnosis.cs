using FarPoint.Win;
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{

    /// <summary>
    /// Class Name : frmRegSelectDiagnosis
    /// File Name : frmRegSelectDiagnosis.cs
    /// Title or Description : 선택진료 신청서 등록관리(개인별)
    /// Author : 박성완
    /// Create Date : 2017-06-15
    /// <history> 
    /// 2017-06-28 이벤트 및 메소드 정리 작업 - 박성완
    /// </history>
    /// </summary>
    public partial class frmRegSelectDiagnosis : Form
    {
        #region Field 및 Enum
        string FstrPano    = "";
        string FstrRowid   = "";
        string FstrFDate   = "";
        string FstrEDate   = "";
        string FstrEntDate = "";

        //스프레드의 선 외곽선 필드
        ComplexBorderSide top    = new ComplexBorderSide(ComplexBorderSideStyle.MediumLine, Color.Black);
        ComplexBorderSide bottom = new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black);
        ComplexBorderSide left   = new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black);
        ComplexBorderSide right  = new ComplexBorderSide(ComplexBorderSideStyle.ThinLine, Color.Black);
        //스프레드들의 칼럼 열거
        enum ColssInfo
        {
            Pano, Sname, Jumin, Juso, TelNo
        }
        enum ColssDept
        {
        DeptCode, DeptName, DrCode, DrName, SDate = 5, EDate, Sel, NewChk, RowID, EntDate, Set1 = 12, Set2, Set3
        , Set4, Set5, Set6, Set7, Set8, Set9, SetC1, SetC2, SetC3, SetC4, SetC5, SetC6, SetC7, SetC8, SetC9, Work
        } 
        enum ColssSel
        {
         DeptCode, DrCode, DrName,SDate,EDate,DelDate,EntDate,EntName,NewChk,DeptCode2,DrCode2,DrName2,RowID,EntDate2
        ,Set1, Set2, Set3, Set4, Set5, Set6, Set7, Set8, Set9, SetC1, SetC2, SetC3, SetC4, SetC5, SetC6, SetC7, SetC8, SetC9, Work
        }


        #endregion

        #region 메소드 모음
        void Screen_Clear()
        {
            ssInfo_Sheet1.ClearRange(0, 0, 0, ssInfo_Sheet1.Columns.Count, true);

            txtPano.Text             = "";

            txtDeptCode.Text         = "";
            txtDrCode.Text           = "";
            dtpSDate.Text            = "";
            dtpEDate.Text            = "";

            lblDept.Text             = "";
            lblDrName.Text           = "";

            FstrPano                 = "";
            FstrRowid                = "";
            FstrFDate                = "";
            FstrEDate                = "";
            FstrEntDate              = "";

            ssDept_Sheet1.Rows.Count = 0;
            ssSel_Sheet1.Rows.Count  = 0;

            lblSTS.Text              = "";
            lblSTS.BackColor         = Color.LightGray;

            chkAgree.Checked         = false;

            Support_Clear();
            SelectTxt_Lock_Check("신규");

            btnDelete.Enabled        = false;
            
        }

        void Support_Clear()
        {
            chkSet3.Checked = true;
            chkSet5.Checked = true;
            chkSet6.Checked = true;

            txtSetC3.Text = "";
            txtSetC5.Text = "";
            txtSetC6.Text = "";

            grbSet.Enabled = false;
        }

        void SelectTxt_Lock_Check(string argGubun)
        {
            dtpSDate.Enabled = true;

            if (argGubun == "신규")
            {
                txtDeptCode.Enabled = false;
                txtDrCode.Enabled = false;
                grbSet.Enabled = true;
            }
            else
            {
                txtDeptCode.Enabled = false;
                txtDrCode.Enabled = false;
                grbSet.Enabled = false;
            }
        }

        void SelectTxt_Clear_SUB()
        {
            txtDeptCode.Text = "";
            txtDrCode.Text   = "";
            lblDept.Text     = "";
            lblDrName.Text   = "";
            dtpSDate.Text    = "";
            dtpEDate.Text    = "";

            FstrPano         = "";
            FstrRowid        = "";
            FstrFDate        = "";
            FstrEDate        = "";
        }

        /// <summary>
        /// 자료삭제 메솓,
        /// </summary>
        /// <returns></returns>
        bool DeleteData()
        {
            if (ComQuery.IsJobAuth(this, "D", clsDB.DbCon) == false) return false; //권한 확인

            ComFunc.ReadSysDate(clsDB.DbCon);

            if (optIO1.Checked == true) { MessageBox.Show("외래만 선택신청서를 당일 삭제 가능합니다.."); return false; }
            if (FstrEntDate != clsPublic.GstrSysDate) { MessageBox.Show("선택신청서는 당일등록건에서만 삭제 가능합니다.."); return false; }
            if (FstrRowid == "" || FstrEntDate == "") { MessageBox.Show("삭제할 등록번호를 먼저 선택하십시오.."); return false; }
            if (MessageBox.Show("정말로 선택한 선택진료 신청건을 삭제하시겠습니까?" + "\n삭제되면 선택진료대상에서 제외됩니다..", "삭제확인", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return false;
            }

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            ComFunc.ReadSysDate(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //이전 내용 백업한다
                SQL = " INSERT INTO KOSMOS_PMPA.BAS_SELECT_MST_HIS  ";
                SQL = SQL + ComNum.VBLF + " ( PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,WORK,ENTDATE2,BIGO,";
                SQL = SQL + ComNum.VBLF + "   Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9 )   ";
                SQL = SQL + ComNum.VBLF + "   SELECT  PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,WORK,ENTDATE,BIGO, ";
                SQL = SQL + ComNum.VBLF + "           Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9 ";
                SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.BAS_SELECT_MST ";
                SQL = SQL + ComNum.VBLF + "     WHERE ROWID =//" + FstrRowid + "// ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;

                }

                SQL = " UPDATE KOSMOS_PMPA.BAS_SELECT_MST SET ";
                SQL = SQL + ComNum.VBLF + " DELDATE =TO_DATE(//" + clsPublic.GstrSysDate + "//,//YYYY-MM-DD//) ,  ";
                SQL = SQL + ComNum.VBLF + " ENTSABUN =//" + clsPublic.GstrJobPart + "//, ";
                SQL = SQL + ComNum.VBLF + " ENTDATE2 = SYSDATE ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID =//" + FstrRowid + "// ";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return false;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("선택한것을 삭제처리 하였습니다..");
                Screen_Clear();
                txtPano.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// 자료저장 메소드
        /// </summary>
        /// <returns></returns>
        bool SaveData()
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return false; //권한 확인

            //기본 마스터
            string strPano     = "";
            string strSname    = "";
            string strGubun    = "";         //입원,외래구분
            string strDeptCode = "";         //선택과
            string strDrCode   = "";         //선택의사
            string strSDate    = "";         //선택시작일
            string strEDate    = "";         //선택종료일
            string strBigo     = "";

            string strDelDate  = "";         //선택제외일자

            string strChk      = "";         //저장점검
            string strWork     = "";         //0:신규, 1.동의해지
            string strTempPano = "";         //임시 등록번호보관

            string strSet1     = "";         //진료지원 진찰
            string strSet2     = "";         //진료지원 검사
            string strSet3     = "";         //진료지원 영상진단
            string strSet4     = "";         //진료지원 방사선치료
            string strSet5     = "";         //진료지원 방사선혈관촬영
            string strSet6     = "";         //진료지원 마취
            string strSet7     = "";         //진료지원 정신요법
            string strSet8     = "";         //진료지원 처치.수술
            string strSet9     = "";         //진료지원 침.구 및 부항

            string strSetC1    = "";         //진료지원 진찰
            string strSetC2    = "";         //진료지원 검사
            string strSetC3    = "";         //진료지원 영상진단
            string strSetC4    = "";         //진료지원 방사선치료
            string strSetC5    = "";         //진료지원 방사선혈관촬영
            string strSetC6    = "";         //진료지원 마취
            string strSetC7    = "";         //진료지원 정신요법
            string strSetC8    = "";         //진료지원 처치.수술
            string strSetC9    = "";         //진료지원 침.구 및 부항

            string SQL         = "";         //Query문
            string SqlErr      = "";         //에러문 받는 변수
            int intRowAffected = 0;          //변경된 Row 받는 변수
            DataTable dt       = null;

            ComFunc.ReadSysDate(clsDB.DbCon);

            strPano = ssInfo_Sheet1.Cells[0, 0].Text.Trim();
            strSname = ssInfo_Sheet1.Cells[0, 1].Text.Trim();
            strTempPano = strPano;

            if (strPano == "" || strSname == "")
            {
                MessageBox.Show("환자를 선택후 저장하십시오");
                return false;
            }
            
            if (clsPublic.GnJobSabun == 111 || clsPublic.GnJobSabun == 222)
            {
                MessageBox.Show("선택진료 신청서를 저장할수 없는 권한입니다..원무과에 문의하세요!!");
                return false;
            }

            strChk = "";
            strBigo = "";

            //외래,입원
            if (optIO0.Checked == true)
            {
                strGubun = "O";
            }
            else
            {
                strGubun = "I";
            }

            strDeptCode = txtDeptCode.Text.Trim();
            strDrCode = txtDrCode.Text.Trim();
            strSDate = dtpSDate.Text.Trim();
            strEDate = dtpEDate.Text;

            strWork = "0";
            if (chkAgree.Checked == true) { strWork = "1"; }

            strSet1 = "N"; strSet2 = "N"; strSet3 = "N";
            strSet4 = "N"; strSet5 = "N"; strSet6 = "N";
            strSet7 = "N"; strSet8 = "N"; strSet9 = "N";

            strSetC1 = ""; strSetC2 = ""; strSetC3 = "";
            strSetC4 = ""; strSetC5 = ""; strSetC6 = "";
            strSetC7 = ""; strSetC8 = ""; strSetC9 = "";

            //1~9 까지 있지만 이 세개 제외하고는 VB 디자인에 없음
            if (chkSet3.Checked == true) { strSet3 = "Y"; }
            if (chkSet5.Checked == true) { strSet5 = "Y"; }
            if (chkSet6.Checked == true) { strSet6 = "Y"; }

            strSetC3 = txtSetC3.Text.Trim();
            strSetC5 = txtSetC5.Text.Trim();
            strSetC6 = txtSetC6.Text.Trim();

            if (strSetC3 != "" && strSetC3 == "N") { MessageBox.Show("위임의사 있는데 선택항목 공란임..", "확인요망"); }
            if (strSetC5 != "" && strSetC5 == "N") { MessageBox.Show("위임의사 있는데 선택항목 공란임..", "확인요망"); }
            if (strSetC6 != "" && strSetC6 == "N") { MessageBox.Show("위임의사 있는데 선택항목 공란임..", "확인요망"); }

            if (strGubun == "I" && (strDeptCode == "PD" || strDeptCode == "NP"))
            {
                MessageBox.Show("입원시 소아과 및 정신과 는 등록안됨!!", "확인요망");
                return false;
            }

            if (strGubun == "O" && strDeptCode == "MG")
            {
                MessageBox.Show("외래는 등록안됨!!", "확인요망");
                return false;
            }

            if ( strGubun == "O" && strDeptCode == "EN")
            {
                MessageBox.Show("외래는 등록안됨!!", "확인요망");
                return false;
            }

            if(strSDate.Length != 10) { MessageBox.Show("날짜형식은 YYYY-MM-DD 입니다....", "확인요망"); return false; }
            if (strEDate != "" && strEDate.Length != 10) { MessageBox.Show("날짜형식은 YYYY-MM-DD 입니다....", "확인요망"); return false; }
            if (DateTime.Parse(strSDate) < DateTime.Parse("2011-06-01")) { MessageBox.Show("선택진료 시작일자는 2011-06-01 부터 가능합니다..", "확인요망"); return false; }
            if (strEDate != "")
            {
                if (DateTime.Parse(strEDate) < DateTime.Parse("2011-06-01")) { MessageBox.Show("선택진료 종료일자는 2011-06-01 부터 가능합니다..", "확인요망"); return false; }
            }

            if (strSDate != clsPublic.GstrSysDate)
            {
                strBigo = "강제일자 조정건";
            }
            if (strEDate != "" && DateTime.Parse(strEDate) < DateTime.Parse(clsPublic.GstrSysDate)) { MessageBox.Show("종료일자는 오늘보다 작을수 없습니다..", "확인요망"); return false; }
            if (strEDate != "" && DateTime.Parse(strSDate) > DateTime.Parse(strEDate)) { MessageBox.Show("시작일자는 종료일자보다 작을수 없습니다..", "확인요망"); return false; }

            //선택진료 종료일 체크
            if(FstrRowid == "" && DateTime.Parse(FstrEDate) >= DateTime.Parse(strSDate))
            {
                if (strGubun == "I")
                {
                    if(MessageBox.Show("선택신청서 신규저장시 시작일자는 마지막 종료일자보다 커야합니다..!!"+"\n\n"+ "당일 재입원일경우만 오늘 날짜로 선택신청을 하시겠습니까?","선택신청확인",MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("선택진료 종료일자는 2011-06-01 부터 가능합니다..", "확인요망");
                    return false;
                }
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                if (FstrRowid == "")
                {
                    strChk = "OK";
                    strDelDate = "";

                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_SELECT_MST ( PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,WORK,ENTDATE2,Bigo, ";
                    SQL = SQL + ComNum.VBLF + "   Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9  ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " //" + strPano + "//,//" + strSname + "//,//" + strGubun + "//,//" + strDeptCode + "//,//" + strDrCode + "//,";
                    SQL = SQL + ComNum.VBLF + " TO_DATE(//" + strSDate + "//,//YYYY-MM-DD//),TO_DATE(//" + strEDate + "//,//YYYY-MM-DD//),TO_DATE(//" + strDelDate + "//,//YYYY-MM-DD//), ";
                    SQL = SQL + ComNum.VBLF + " //" + clsPublic.GstrJobPart + "//,SYSDATE, //0//,SYSDATE,//" + strBigo + "//, ";
                    SQL = SQL + ComNum.VBLF + " //" + strSet1 + "//,//" + strSet2 + "//,//" + strSet3 + "//,//" + strSet4 + "//,//" + strSet5 + "//,//" + strSet6 + "//,//" + strSet7 + "//,//" + strSet8 + "//,//" + strSet9 + "//, ";
                    SQL = SQL + ComNum.VBLF + " //" + strSetC1 + "//,//" + strSetC2 + "//,//" + strSetC3 + "//,//" + strSetC4 + "//,//" + strSetC5 + "//,//" + strSetC6 + "//,//" + strSetC7 + "//,//" + strSetC8 + "//,//" + strSetC9 + "//  ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_SELECT_MST_HIS  ";
                    SQL = SQL + ComNum.VBLF + " ( PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,WORK,ENTDATE2,Bigo,";
                    SQL = SQL + ComNum.VBLF + "   Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9 ) VALUES ( ";
                    SQL = SQL + ComNum.VBLF + " //" + strPano + "//,//" + strSname + "//,//" + strGubun + "//,//" + strDeptCode + "//,//" + strDrCode + "//,";
                    SQL = SQL + ComNum.VBLF + " TO_DATE(//" + strSDate + "//,//YYYY-MM-DD//),TO_DATE(//" + strEDate + "//,//YYYY-MM-DD//),TO_DATE(//" + strDelDate + "//,//YYYY-MM-DD//), ";
                    SQL = SQL + ComNum.VBLF + " //" + clsPublic.GstrJobPart + "//,SYSDATE, //0//,SYSDATE,//" + strBigo + "//, ";
                    SQL = SQL + ComNum.VBLF + " //" + strSet1 + "//,//" + strSet2 + "//,//" + strSet3 + "//,//" + strSet4 + "//,//" + strSet5 + "//,//" + strSet6 + "//,//" + strSet7 + "//,//" + strSet8 + "//,//" + strSet9 + "//, ";
                    SQL = SQL + ComNum.VBLF + " //" + strSetC1 + "//,//" + strSetC2 + "//,//" + strSetC3 + "//,//" + strSetC4 + "//,//" + strSetC5 + "//,//" + strSetC6 + "//,//" + strSetC7 + "//,//" + strSetC8 + "//,//" + strSetC9 + "//  ) ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }
                else if (FstrRowid != "")
                {
                    strChk = "OK";
                    //이전내역 백업
                    SQL = " INSERT INTO KOSMOS_PMPA.BAS_SELECT_MST_HIS  ";
                    SQL = SQL + ComNum.VBLF + " ( PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,WORK,ENTDATE2,BIGO,";
                    SQL = SQL + ComNum.VBLF + "   Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9 )   ";
                    SQL = SQL + ComNum.VBLF + "   SELECT  PANO,SNAME,GUBUN,DEPTCODE,DRCODE,SDATE,EDATE,DELDATE,ENTSABUN,ENTDATE,//0//,ENTDATE2,BIGO, ";
                    SQL = SQL + ComNum.VBLF + "           Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9,Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9 ";
                    SQL = SQL + ComNum.VBLF + "    FROM KOSMOS_PMPA.BAS_SELECT_MST ";
                    SQL = SQL + ComNum.VBLF + "     WHERE ROWID =//" + FstrRowid + "// ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }

                    SQL = " UPDATE KOSMOS_PMPA.BAS_SELECT_MST SET ";
                    //2014-04-17 시작일자 조정가능하게 변경 원무과 의뢰서 요청
                    SQL = SQL + ComNum.VBLF + " SDate =TO_DATE(//" + strSDate + "//,//YYYY-MM-DD//) ,  ";
                    SQL = SQL + ComNum.VBLF + "  Work =//" + strWork + "//, ";
                    SQL = SQL + ComNum.VBLF + " EDATE =TO_DATE(//" + strEDate + "//,//YYYY-MM-DD//) ,  ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN =//" + clsPublic.GstrJobPart + "//, ";
                    SQL = SQL + ComNum.VBLF + " ENTDATE2 = SYSDATE ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID =//" + FstrRowid + "// ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return false;
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Screen_Clear();

                if( strChk == "OK")
                {
                    ComFunc.MsgBox("선택한 자료가 정상 저장되었습니다..");
                    if (optIO1.Checked == true)
                    {
                        SQL = " SELECT Pano FROM " + ComNum.VBLF + "IPD_NEW_MASTER ";
                        SQL = SQL + ComNum.VBLF + "  WHERE PANO =//" + strTempPano + "// ";
                        SQL = SQL + ComNum.VBLF + "   AND ACTDATE IS NULL ";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                        if (dt.Rows.Count > 0) 
                        {
                            //TODO:GstrSelTemp2의 위치 및 용도 확인 필요
                            //clsPublic.GstrSelTemp2 = "OK";
                        }
                        dt.Dispose();
                        dt = null;
                    }
                }
                else
                {
                    MessageBox.Show("변경된 자료나, 신규등록자료가 없습니다..");
                }
                txtPano.Focus();
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        /// <summary>
        /// ssInfo 스프레드에 특정 환자번호로 그 외의 정보를 찾아 출력한다.
        /// </summary>
        /// <param name="argPano">환자번호</param>
        void Pano_Info_Display(string argPano)
        {
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;

            Cursor.Current = Cursors.WaitCursor;

            SQL = " SELECT a.Pano,a.SName,a.Jumin1,a.Jumin2,a.Jumin3, ";
            SQL = SQL + ComNum.VBLF + "  a.Juso,a.ZipCode1 || a.ZipCode1 AS ZipCode,a.Tel,a.Hphone, "  ;
            SQL = SQL + ComNum.VBLF + "  b.DelDate,b.ROWID, a.ZIPCODE3, a.ROADDETAIL, a.BUILDNO "  ;
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_PATIENT a, KOSMOS_PMPA.BAS_SELECT_MST b "  ;
            SQL = SQL + ComNum.VBLF + "   WHERE a.PANO =b.Pano(+) "  ;
            SQL = SQL + ComNum.VBLF + "    AND a.PANO =//" + argPano + "// "  ;

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count > 0)
            {
                FstrPano = dt.Rows[0]["Pano"].ToString().Trim();
                ssInfo_Sheet1.Cells[0, (int)ColssInfo.Pano].Text = FstrPano;
                ssInfo_Sheet1.Cells[0, (int)ColssInfo.Sname].Text = dt.Rows[0]["SName"].ToString().Trim();
                ssInfo_Sheet1.Cells[0, (int)ColssInfo.Jumin].Text = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                if (dt.Rows[0]["BUILDNO"].ToString().Trim() != "")
                {
                    ssInfo_Sheet1.Cells[0, (int)ColssInfo.Juso].Text =
                        //TODO: VbFunction 함수 필요-READ_ROAD_JUSO(dt.Rows[0]["BUILDNO"].ToString().Trim() +
                        " " + dt.Rows[0]["ROADDETAIL"].ToString().Trim();
                }
                else
                {                    
                    ssInfo_Sheet1.Cells[0, (int)ColssInfo.Juso].Text =
                         //TODO: VbFunction 함수 필요-READ_BAS_Mail(dt.Rows[0]["ZipCode"].ToString().Trim() +
                         " " + dt.Rows[0]["Juso"].ToString().Trim();
                }
                ssInfo_Sheet1.Cells[0, (int)ColssInfo.TelNo].Text = dt.Rows[0]["Tel"].ToString().Trim() + "\n" + dt.Rows[0]["Hphone"].ToString().Trim();
                btnDelete.Enabled = true;
            }
            else
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                ssInfo_Sheet1.ClearRange(0, 0, 0, ssInfo_Sheet1.Columns.Count, true);
                return;
            }            

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;
        }

        /// <summary>
        /// (ssDept)각 정보들을 매개로 다른 정보들을 스프레드에 출력한다.
        /// </summary>
        /// <param name="argSpread">출력할 스프레드</param>
        /// <param name="argDept">진료과</param>
        /// <param name="argPano">환자번호</param>
        /// <param name="argIO">입원외래구분</param>
        /// <param name="argTour"></param>
        void Read_BasicSelectDeptDoctor_Display(FpSpread argSpread, string argDept, string argPano, string argIO, string argTour)
        {
            string strDrCode = "";
            string strDeptCode = "";
            string strDeptName = "";
            string strDrName = "";

            string strOK = "";
            string strSel = "";
            string strSDate = "";
            string strEdate = "";
            string strEntDate = "";
            string strROWID = "";

            string strWork = "";

            string strSet1 = ""; //진료지원 진찰
            string strSet2 = ""; //진료지원 검사
            string strSet3 = ""; //진료지원 영상진단
            string strSet4 = ""; //진료지원 방사선치료
            string strSet5 = ""; //진료지원 방사선혈관촬영
            string strSet6 = ""; //진료지원 마취
            string strSet7 = ""; //진료지원 정신요법
            string strSet8 = ""; //진료지원 처치.수술
            string strSet9 = ""; //진료지원 침.구 및 부항

            string strSetC1 = ""; //진료지원 진찰
            string strSetC2 = ""; //진료지원 검사
            string strSetC3 = ""; //진료지원 영상진단
            string strSetC4 = ""; //진료지원 방사선치료
            string strSetC5 = ""; //진료지원 방사선혈관촬영
            string strSetC6 = ""; //진료지원 마취
            string strSetC7 = ""; //진료지원 정신요법
            string strSetC8 = ""; //진료지원 처치.수술
            string strSetC9 = ""; //진료지원 침.구 및 부항

            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            DataTable dt1 = null;

            argSpread.Sheets[0].Rows.Count = 0;

            //의사
            SQL = " SELECT a.DrCode,a.DrName,a.DrDept1,a.GbChoice,b.DeptNamek ";
            SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_DOCTOR a, KOSMOS_PMPA.BAS_CLINICDEPT b ";
            SQL = SQL + ComNum.VBLF + " WHERE a.DrDept1=b.DeptCode(+) ";
            if (argDept != "전체") { SQL = SQL + ComNum.VBLF + "  AND a.DrDept1 IN ( " + argDept + " )"; }
            SQL = SQL + ComNum.VBLF + "  AND a.GbChoice='Y' ";
            if (argTour != "ALL") { SQL = SQL + ComNum.VBLF + "  AND (a.TOUR ='N' OR a.TOUR IS NULL)  "; }
            SQL = SQL + ComNum.VBLF + "  AND SUBSTR(a.DrCode,3,2) <>'99' ";
            if (chkRD.Checked == true) { SQL = SQL + ComNum.VBLF + " AND a.DrDept1 NOT IN ('RD') "; }
            SQL = SQL + ComNum.VBLF + "  ORDER BY b.PrintRanking,a.DrCode  ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            argSpread.Sheets[0].RowCount = dt.Rows.Count;
            argSpread.Sheets[0].SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                strOK = "OK";
                strDrCode = dt.Rows[i]["DrCode"].ToString().Trim();
                strDeptCode = dt.Rows[i]["DrDept1"].ToString().Trim();
                strDrName = dt.Rows[i]["DrName"].ToString().Trim();
                strDeptName = dt.Rows[i]["DeptNamek"].ToString().Trim();

                SQL = " SELECT DEPTCODE,Work,";
                SQL = SQL + ComNum.VBLF + " Set1,Set2,Set3,Set4,Set5,Set6,Set7,Set8,Set9, ";
                SQL = SQL + ComNum.VBLF + " Setc1,Setc2,Setc3,Setc4,Setc5,Setc6,Setc7,Setc8,Setc9, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(SDATE,'YYYY-MM-DD') SDATE, ";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EDATE,'YYYY-MM-DD') EDATE,";
                SQL = SQL + ComNum.VBLF + " TO_CHAR(EntDate,'YYYY-MM-DD') EntDate,ENTSABUN,ROWID";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SELECT_MST ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano ='" + argPano + "' ";
                SQL = SQL + ComNum.VBLF + "  AND DrCode ='" + strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GUBUN ='" + argIO + "' ";
                SQL = SQL + ComNum.VBLF + "  AND (DELDATE IS NULL OR DELDATE ='') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY SDATE DESC ";

                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                strOK = ""; strSel = "";
                strSDate = ""; strEdate = "";
                strROWID = "";
                strWork = "";

                strSet1 = ""; strSet2 = ""; strSet3 = "";
                strSet4 = ""; strSet5 = ""; strSet6 = "";
                strSet7 = ""; strSet8 = ""; strSet9 = "";

                strSetC1 = ""; strSetC2 = ""; strSetC3 = "";
                strSetC4 = ""; strSetC5 = ""; strSetC6 = "";
                strSetC7 = ""; strSetC8 = ""; strSetC9 = "";

                strOK = "OK";
                if (dt1.Rows.Count > 0)
                {
                    strOK = "OK";

                    strSDate = dt1.Rows[i]["SDate"].ToString().Trim();
                    strEdate = dt1.Rows[i]["EDate"].ToString().Trim();
                    strEntDate = dt1.Rows[i]["EntDate"].ToString().Trim();
                    strROWID = dt1.Rows[i]["ROWID"].ToString().Trim();  //최종ROWID

                    strSet1 = dt1.Rows[i]["Set1"].ToString().Trim();
                    strSet2 = dt1.Rows[i]["Set2"].ToString().Trim();
                    strSet3 = dt1.Rows[i]["Set3"].ToString().Trim();
                    strSet4 = dt1.Rows[i]["Set4"].ToString().Trim();
                    strSet5 = dt1.Rows[i]["Set5"].ToString().Trim();
                    strSet6 = dt1.Rows[i]["Set6"].ToString().Trim();
                    strSet7 = dt1.Rows[i]["Set7"].ToString().Trim();
                    strSet8 = dt1.Rows[i]["Set8"].ToString().Trim();
                    strSet9 = dt1.Rows[i]["Set9"].ToString().Trim();

                    strSetC1 = dt1.Rows[i]["Setc1"].ToString().Trim();
                    strSetC2 = dt1.Rows[i]["Setc2"].ToString().Trim();
                    strSetC3 = dt1.Rows[i]["Setc3"].ToString().Trim();
                    strSetC4 = dt1.Rows[i]["Setc4"].ToString().Trim();
                    strSetC5 = dt1.Rows[i]["Setc5"].ToString().Trim();
                    strSetC6 = dt1.Rows[i]["Setc6"].ToString().Trim();
                    strSetC7 = dt1.Rows[i]["Setc7"].ToString().Trim();
                    strSetC8 = dt1.Rows[i]["Setc8"].ToString().Trim();
                    strSetC9 = dt1.Rows[i]["Setc9"].ToString().Trim();

                    strWork = dt1.Rows[i]["Work"].ToString().Trim();

                    strSel = "Y";
                }
                dt1.Dispose();
                dt1 = null;

                if(strOK == "OK")
                {
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.DeptCode].Text = strDeptCode;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.DeptName].Text = strDeptName;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.DrCode].Text = strDrCode;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.DrName].Text = strDrName;

                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SDate].Text = strSDate;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.EDate].Text = strEdate;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Sel].Text = strSel;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.NewChk].Text = ""; //TODO: Read_SELECT_INSERT_CHK2(argSpread, argIO, strDrCode); 함수 필요함
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.RowID].Text = strROWID;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.EntDate].Text = strEntDate;

                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set1].Text = strSet1;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set2].Text = strSet2;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set3].Text = strSet3;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set4].Text = strSet4;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set5].Text = strSet5;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set6].Text = strSet6;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set7].Text = strSet7;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set8].Text = strSet8;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Set9].Text = strSet9;

                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC1].Text = strSetC1;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC2].Text = strSetC2;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC3].Text = strSetC3;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC4].Text = strSetC4;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC5].Text = strSetC5;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC6].Text = strSetC6;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC7].Text = strSetC7;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC8].Text = strSetC8;
                    argSpread.Sheets[0].Cells[i, (int)ColssDept.SetC9].Text = strSetC9;

                    argSpread.Sheets[0].Cells[i, (int)ColssDept.Work].Text = strWork;

                    if (strSel == "Y")
                    {
                        argSpread.Sheets[0].Cells[i, 1, i, 8].ForeColor = Color.FromArgb(0, 0, 0);
                        argSpread.Sheets[0].Cells[i, 1, i, 8].BackColor = Color.FromArgb(210, 210, 255);
                    }
                }
            }

            dt.Dispose();
            dt = null;
        }

        /// <summary>
        /// (ssSel)각 정보들을 매개로 다른 정보들을 스프레드에 출력한다.
        /// </summary>
        /// <param name="argSpread">스프레드</param>
        /// <param name="argDept">진료과</param>
        /// <param name="argPano">환자번호</param>
        /// <param name="argIO">입원외래구분</param>
        void Read_RegSelectDeptDoctor_Display(FpSpread argSpread, string argDept, string argPano, string argIO)
        {
            string strNew = "";
            string strOLD = "";

            string SQL    = "";       //Query문
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt  = null;

            argSpread.Sheets[0].Rows.Count = 0;

            if (argDept == "") { return; }

            //의사
            SQL = " SELECT a.DEPTCODE,a.DrCode,a.Work,";
            SQL = ComNum.VBLF + " a.Set1,a.Set2,a.Set3,a.Set4,a.Set5,a.Set6,a.Set7,a.Set8,a.Set9, ";
            SQL = ComNum.VBLF + " a.Setc1,a.Setc2,a.Setc3,a.Setc4,a.Setc5,a.Setc6,a.Setc7,a.Setc8,a.Setc9, ";
            SQL = ComNum.VBLF + " TO_CHAR(a.SDATE,//YYYY-MM-DD//) SDATE, ";
            SQL = ComNum.VBLF + " TO_CHAR(a.EDATE,//YYYY-MM-DD//) EDATE, ";
            SQL = ComNum.VBLF + " TO_CHAR(a.DELDATE,//YYYY-MM-DD//) DELDATE, ";
            SQL = ComNum.VBLF + " TO_CHAR(a.EntDate,//YYYY-MM-DD HH24:MI//) EntDate,";
            SQL = ComNum.VBLF + " TO_CHAR(a.EntDate2,//YYYY-MM-DD HH24:MI//) EntDate2,";
            SQL = ComNum.VBLF + " a.ENTSABUN,a.ROWID ";
            SQL = ComNum.VBLF + "  FROM KOSMOS_PMPA.BAS_SELECT_MST a, KOSMOS_PMPA.BAS_CLINICDEPT b ";
            SQL = ComNum.VBLF + " WHERE a.DeptCode=b.DeptCode(+)";
            SQL = ComNum.VBLF + "  AND a.Pano =//" + argPano + "// " ;

            if (argDept != "전체" ) { SQL = ComNum.VBLF + "  AND b.DeptCode IN ( " + argDept + " )"; }
            SQL = ComNum.VBLF + "  AND a.GUBUN =//" + argIO + "// ";
            SQL = ComNum.VBLF + "  AND (a.DELDATE IS NULL OR a.DELDATE =////) ";
            SQL = ComNum.VBLF + " ORDER BY b.PrintRanking,a.DrCode,a.SDATE DESC ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                return;
            }
            if (dt.Rows.Count == 0)
            {
                dt.Dispose();
                dt = null;
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                return;
            }

            argSpread.Sheets[0].Rows.Count = dt.Rows.Count;
            argSpread.Sheets[0].SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < dt.Rows.Count; i++) 
            {
                strNew = dt.Rows[i]["DrCode"].ToString().Trim();

                if (strNew != strOLD)
                {
                    argSpread.Sheets[0].Cells[i, (int)ColssSel.DeptCode].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    argSpread.Sheets[0].Cells[i, (int)ColssSel.DrCode].Text = strNew;
                    argSpread.Sheets[0].Cells[i, (int)ColssSel.DrName].Text = ""; //TODO:READ_BAS_Doctor(strNew) 함수필요
                    argSpread.Sheets[0].Rows[i].Border = new ComplexBorder(left, top, right, bottom);
                }

                argSpread.Sheets[0].Cells[i, (int)ColssSel.SDate].Text = dt.Rows[i]["SDate"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.EDate].Text = dt.Rows[i]["EDate"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.DelDate].Text = dt.Rows[i]["DELDATE"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.EntDate].Text = dt.Rows[i]["EntDate"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.EntName].Text = dt.Rows[i]["EntSabun"].ToString().Trim() + "("; //TODO: + READ_INSA_Name(dt.Rows[i]["EntSabun"].ToString().Trim()) +")";
                argSpread.Sheets[0].Cells[i, (int)ColssSel.NewChk].Text = ""; //TODO:Read_SELECT_INSERT_CHK2(ArgPano, ArgIO, Trim(AdoGetString(AdoRes, "DrCode", i))) 함수필요

                argSpread.Sheets[0].Cells[i, (int)ColssSel.DeptCode2].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.DrCode2].Text = dt.Rows[i]["DrCode"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.DrName2].Text = //TODO:READ_BAS_Doctor(strNew) 함수필요;

                argSpread.Sheets[0].Cells[i, (int)ColssSel.RowID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.EntDate2].Text = dt.Rows[i]["EntDate2"].ToString().Trim();

                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set1].Text = dt.Rows[i]["Set1"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set2].Text = dt.Rows[i]["Set2"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set3].Text = dt.Rows[i]["Set3"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set4].Text = dt.Rows[i]["Set4"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set5].Text = dt.Rows[i]["Set5"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set6].Text = dt.Rows[i]["Set6"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set7].Text = dt.Rows[i]["Set7"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set8].Text = dt.Rows[i]["Set8"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.Set9].Text = dt.Rows[i]["Set9"].ToString().Trim();

                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC1].Text = dt.Rows[i]["Setc1"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC2].Text = dt.Rows[i]["Setc2"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC3].Text = dt.Rows[i]["Setc3"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC4].Text = dt.Rows[i]["Setc4"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC5].Text = dt.Rows[i]["Setc5"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC6].Text = dt.Rows[i]["Setc6"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC7].Text = dt.Rows[i]["Setc7"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC8].Text = dt.Rows[i]["Setc8"].ToString().Trim();
                argSpread.Sheets[0].Cells[i, (int)ColssSel.SetC9].Text = dt.Rows[i]["Setc9"].ToString().Trim();

                argSpread.Sheets[0].Cells[i, (int)ColssSel.Work].Text = dt.Rows[i]["Work"].ToString().Trim();

                strOLD = dt.Rows[i]["DrCode"].ToString().Trim();
            }
            dt.Dispose();
            dt = null;
        }

        bool ViewData()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return false; //권한 확인
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                string strIO = "";

                SelectTxt_Clear_SUB();

                if (optIO0.Checked == true)
                {
                    strIO = "O";

                    chkSet5.Visible = false; txtSetC5.Visible = false;
                    chkSet6.Visible = false; txtSetC6.Visible = false;
                }
                else if (optIO1.Checked == true)
                {
                    strIO = "I";

                    chkSet5.Visible = true; txtSetC5.Visible = true;
                    chkSet6.Visible = true; txtSetC6.Visible = true;
                }

                txtPano.Text = String.Format("{0:00000000}", txtPano.Text);

                if (txtPano.Text != "")
                {
                    Pano_Info_Display(txtPano.Text);
                    Read_BasicSelectDeptDoctor_Display(ssDept, "전체", txtPano.Text, strIO, "");
                    Read_RegSelectDeptDoctor_Display(ssSel, "전체", txtPano.Text, strIO);
                }
                Cursor.Current = Cursors.Default;
                return true;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
                return false;
            }
        }

        #endregion

        #region 이벤트 처리

        void setEvent()
        {
            //폼 로드 이벤트
            this.Load += (sender, e) =>
            {
                if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
                ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

                ComFunc.ReadSysDate(clsDB.DbCon);

                ssDept_Sheet1.Columns[(int)ColssDept.NewChk].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.RowID].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.EntDate].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.Set1].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.Set2].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.Set4].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.Set7].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.Set8].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.Set9].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.SetC1].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.SetC2].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.SetC4].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.SetC7].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.SetC8].Visible = false;
                ssDept_Sheet1.Columns[(int)ColssDept.SetC9].Visible = false;

                ssSel_Sheet1.Columns[(int)ColssSel.DeptCode2].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.DrCode2].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.DrName2].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.RowID].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.Set1].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.Set2].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.Set4].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.Set7].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.Set8].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.Set9].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.SetC1].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.SetC2].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.SetC4].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.SetC7].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.SetC8].Visible = false;
                ssSel_Sheet1.Columns[(int)ColssSel.SetC9].Visible = false;

                lblIO.Text = "";

                Screen_Clear();

                lblSabun.Text = "작업자 : " + clsPublic.GstrJobName;
                //TODO:GstrSelTemp 값에 따라 체크박스 숨김 여부 확인. VB 디자인에는 숨겨진 체크박스가 없어서 어떤식으로 구현할지 확인필요
                //        //기본외래 - 혈관,마취 안보임
                //ChkSet5.Visible = False: TxtSetC5.Visible = False
                //ChkSet6.Visible = False: TxtSetC6.Visible = False


                //If GnJobSabun <> 4349 Then
                //    test.Visible = False
                //    //////Frame_Set.Visible = False
                //End If


                //P_Sabun.Caption = "작업자 : " & GstrJobName


                ////////GstrSelTemp = "I^^81000004^^"
                //If l(GstrSelTemp, "^^") > 1 Then
                //    If Trim(P(GstrSelTemp, "^^", 1)) = "I" Then
                //        TxtPano.Text = Trim(P(GstrSelTemp, "^^", 2))
                //        OptIO(1).Value = True


                //        ChkSet5.Visible = True: TxtSetC5.Visible = True
                //        ChkSet6.Visible = True: TxtSetC6.Visible = True
                //    ElseIf Trim(P(GstrSelTemp, "^^", 1)) = "O" Then
                //        TxtPano.Text = Trim(P(GstrSelTemp, "^^", 2))
                //        OptIO(0).Value = True


                //        ChkSet5.Visible = False: TxtSetC5.Visible = False
                //        ChkSet6.Visible = False: TxtSetC6.Visible = False
                //    End If
                //End If };
            };

            //txtPano - Leave
            txtPano.Leave += (sender, e) => 
            {
                string strIO = "";

                if (optIO0.Checked == true)
                {
                    strIO = "O";
                }
                else if (optIO1.Checked == true)
                {
                    strIO = "I";
                }

                //TODO: READ_BARCODE 함수 필요
                //if (Read_Barcode(txtPano.Text.Trim()) == true)
                //{
                //    txtPano.Text = clsPublic.GstrBarPano;
                //}
                //else
                //{
                //    txtPano.Text = String.Format("{0:00000000}", txtPano.Text);
                //}

                txtPano.Text = String.Format("{0:00000000}", txtPano.Text);

                Pano_Info_Display(txtPano.Text);
                Read_BasicSelectDeptDoctor_Display(ssDept, "전체", txtPano.Text, strIO, "");
                Read_RegSelectDeptDoctor_Display(ssSel, "전체", txtPano.Text, strIO);
            };

            //optIO0 - 입원,외래 라디오박스
            optIO0.CheckedChanged += (sender, e) => 
            {
                if (optIO0.Checked == true)
                {
                    lblIO.Text = "외래 등록";
                    lblIO.BackColor = Color.Blue;

                    chkSet5.Visible = false; txtSetC5.Visible = false;
                    chkSet6.Visible = false; txtSetC6.Visible = false;

                    ssDept_Sheet1.Columns[(int)ColssDept.Set5].Visible = false;
                    ssDept_Sheet1.Columns[(int)ColssDept.Set6].Visible = false;

                    ssDept_Sheet1.Columns[(int)ColssDept.SetC5].Visible = false;
                    ssDept_Sheet1.Columns[(int)ColssDept.SetC6].Visible = false;

                    ssSel_Sheet1.Columns[(int)ColssSel.Set5].Visible = false;
                    ssSel_Sheet1.Columns[(int)ColssSel.Set6].Visible = false;

                    ssSel_Sheet1.Columns[(int)ColssSel.SetC5].Visible = false;
                    ssSel_Sheet1.Columns[(int)ColssSel.SetC6].Visible = false;
                }
                else
                {
                    lblIO.Text = "입원 등록";
                    lblIO.BackColor = Color.Red;

                    chkSet5.Visible = true; txtSetC5.Visible = true;
                    chkSet6.Visible = true; txtSetC6.Visible = true;

                    ssDept_Sheet1.Columns[(int)ColssDept.Set5].Visible = true;
                    ssDept_Sheet1.Columns[(int)ColssDept.Set6].Visible = true;

                    ssDept_Sheet1.Columns[(int)ColssDept.SetC5].Visible = true;
                    ssDept_Sheet1.Columns[(int)ColssDept.SetC6].Visible = true;

                    ssSel_Sheet1.Columns[(int)ColssSel.Set5].Visible = true;
                    ssSel_Sheet1.Columns[(int)ColssSel.Set6].Visible = true;

                    ssSel_Sheet1.Columns[(int)ColssSel.SetC5].Visible = true;
                    ssSel_Sheet1.Columns[(int)ColssSel.SetC6].Visible = true;
                }

                if (txtPano.Text != "")
                {
                    SelectTxt_Clear_SUB();
                    btnView.PerformClick();
                }
            };

            #region Click 이벤트
            btnView.Click += (sender, e) => { if (ViewData() == false) return; };
            btnSave.Click += (sender, e) => { if (SaveData() == false) return; };
            btnDelete.Click += (sender, e) => { if (DeleteData() == false) return; };
            btnCancel.Click += (sender, e) => { Screen_Clear(); txtPano.Focus(); };
            btnExit.Click += (sender, e) => { this.Close(); };
            #endregion

            #region CellDoubleClick 이벤트
            ssSel.CellDoubleClick += (sender, e) => 
            {
                string strNewChk = "";

                string strWork   = "";
                string strSet1   = ""; //진료지원 진찰
                string strSet2   = ""; //진료지원 검사
                string strSet3   = ""; //진료지원 영상진단
                string strSet4   = ""; //진료지원 방사선치료
                string strSet5   = ""; //진료지원 방사선혈관촬영
                string strSet6   = ""; //진료지원 마취
                string strSet7   = ""; //진료지원 정신요법
                string strSet8   = ""; //진료지원 처치.수술
                string strSet9   = ""; //진료지원 침.구 및 부항

                string strSetC1  = ""; //진료지원 진찰
                string strSetC2  = ""; //진료지원 검사
                string strSetC3  = ""; //진료지원 영상진단
                string strSetC4  = ""; //진료지원 방사선치료
                string strSetC5  = ""; //진료지원 방사선혈관촬영
                string strSetC6  = ""; //진료지원 마취
                string strSetC7  = ""; //진료지원 정신요법
                string strSetC8  = ""; //진료지원 처치.수술
                string strSetC9  = ""; //진료지원 침.구 및 부항

                if (e.Row < 0) { return; }
                if (e.Column < 0) { return; }

                strWork = "";
                chkAgree.Checked = false;

                strSet1  = ""; strSet2 = ""; strSet3 = "";
                strSet4  = ""; strSet5 = ""; strSet6 = "";
                strSet7  = ""; strSet8 = ""; strSet9 = "";

                strSetC1 = ""; strSetC2 = ""; strSetC3 = "";
                strSetC4 = ""; strSetC5 = ""; strSetC6 = "";
                strSetC7 = ""; strSetC8 = ""; strSetC9 = "";

                FstrRowid = "";

                SelectTxt_Clear_SUB();

                FstrFDate   = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SDate].Text.Trim();
                FstrEDate   = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.EDate].Text.Trim();
                FstrEntDate = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.EntDate].Text.Trim().Substring(0, 10);
                FstrRowid   = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.RowID].Text.Trim();

                strSet1     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set1].Text.Trim();
                strSet2     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set2].Text.Trim();
                strSet3     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set3].Text.Trim();
                strSet4     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set4].Text.Trim();
                strSet5     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set5].Text.Trim();
                strSet6     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set6].Text.Trim();
                strSet7     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set7].Text.Trim();
                strSet8     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set8].Text.Trim();
                strSet9     = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Set9].Text.Trim();

                strSetC1    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC1].Text.Trim();
                strSetC2    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC2].Text.Trim();
                strSetC3    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC3].Text.Trim();
                strSetC4    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC4].Text.Trim();
                strSetC5    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC5].Text.Trim();
                strSetC6    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC6].Text.Trim();
                strSetC7    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC7].Text.Trim();
                strSetC8    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC8].Text.Trim();
                strSetC9    = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SetC9].Text.Trim();

                strWork = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.Work].Text.Trim();

                strNewChk = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.NewChk].Text.Trim();

                if (ssSel_Sheet1.Cells[e.Row, (int)ColssSel.EDate].Text.Trim() != "")
                {
                    if (strNewChk == "OK")
                    {
                        if (MessageBox.Show("신규로 작업가능한 선택의사입니다..신규작업을 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            FstrRowid = "";
                        }
                        else
                        {
                            FstrRowid = "";
                            return;
                        }
                    }
                    else
                    {
                        FstrRowid = "";
                        MessageBox.Show("종료일자 없는것만 가능합니다..!!" + "\n" + "해당의사로 선택등록이 되어있습니다.. 수정작업은 불가합니다..", "확인");
                        return;
                    }
                }

                if (FstrRowid != "")
                {
                    btnDelete.Enabled = true;
                    lblSTS.Text = "수정작업->종료일자만 저장가능!!";
                    lblSTS.BackColor = Color.Purple;
                    dtpSDate.Text = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.SDate].ToString().Trim();

                    SelectTxt_Lock_Check("수정");

                    Support_Clear();

                    if (strWork == "1") { chkAgree.Checked = true; }

                    //if (strSet1 == "Y") { ch.Checked = true; }
                    //if (strSet2 == "Y") { chkAgree.Checked = true; }
                    if (strSet3 == "Y") { chkSet3.Checked = true; }
                    //if (strSet4 == "Y") { ch.Checked = true; }
                    if (strSet5 == "Y") { chkSet5.Checked = true; }
                    if (strSet6 == "Y") { chkSet6.Checked = true; }
                    //if (strSet7 == "Y") { chkAgree.Checked = true; }
                    //if (strSet8 == "Y") { chkAgree.Checked = true; }
                    //if (strSet9 == "Y") { chkAgree.Checked = true; }

                    txtSetC3.Text = strSet3;
                    txtSetC5.Text = strSet5;
                    txtSetC6.Text = strSet6;

                    dtpEDate.Focus();
                }
                else
                {
                    lblSTS.Text = "신규등록";
                    lblSTS.BackColor = Color.PaleGreen;
                    dtpSDate.Text = "";
                    SelectTxt_Lock_Check("신규");

                    Support_Clear();
                    grbSet.Enabled = true;

                    dtpSDate.Focus();
                }

                txtDeptCode.Text = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.DeptCode2].ToString().Trim();
                lblDept.Text = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.DrCode2].ToString().Trim(); //TODO:함수 필요 VbFunction-GetDeptName(ssSel_Sheet1.Cells[e.Row, (int)ColssSel.DrCode2].ToString().Trim());
                txtDrCode.Text = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.DrCode2].ToString().Trim();
                lblDrName.Text = ssSel_Sheet1.Cells[e.Row, (int)ColssSel.DrName2].ToString().Trim();

            };
            ssDept.CellDoubleClick += (sender, e) => 
            {
                string strNewChk = "";

                string strWork = "";

                string strSet1 = ""; //진료지원 진찰
                string strSet2 = ""; //진료지원 검사
                string strSet3 = ""; //진료지원 영상진단
                string strSet4 = ""; //진료지원 방사선치료
                string strSet5 = ""; //진료지원 방사선혈관촬영
                string strSet6 = ""; //진료지원 마취
                string strSet7 = ""; //진료지원 정신요법
                string strSet8 = ""; //진료지원 처치.수술
                string strSet9 = ""; //진료지원 침.구 및 부항

                string strSetC1 = ""; //진료지원 진찰
                string strSetC2 = ""; //진료지원 검사
                string strSetC3 = ""; //진료지원 영상진단
                string strSetC4 = ""; //진료지원 방사선치료
                string strSetC5 = ""; //진료지원 방사선혈관촬영
                string strSetC6 = ""; //진료지원 마취
                string strSetC7 = ""; //진료지원 정신요법
                string strSetC8 = ""; //진료지원 처치.수술
                string strSetC9 = ""; //진료지원 침.구 및 부항

                if (e.Row < 0) { return; }
                if (e.Column < 0) { return; }

                strWork = "";
                chkAgree.Checked = false;

                strSet1 = ""; strSet2 = ""; strSet3 = "";
                strSet4 = ""; strSet5 = ""; strSet6 = "";
                strSet7 = ""; strSet8 = ""; strSet9 = "";

                strSetC1 = ""; strSetC2 = ""; strSetC3 = "";
                strSetC4 = ""; strSetC5 = ""; strSetC6 = "";
                strSetC7 = ""; strSetC8 = ""; strSetC9 = "";

                FstrRowid = "";

                SelectTxt_Clear_SUB();

                FstrFDate = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SDate].ToString().Trim();
                FstrEDate = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.EDate].ToString().Trim();
                FstrRowid = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.RowID].ToString().Trim();
                FstrEntDate = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.EntDate].ToString().Trim();

                strSet1 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set1].ToString().Trim();
                strSet2 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set2].ToString().Trim();
                strSet3 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set3].ToString().Trim();
                strSet4 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set4].ToString().Trim();
                strSet5 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set5].ToString().Trim();
                strSet6 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set6].ToString().Trim();
                strSet7 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set7].ToString().Trim();
                strSet8 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set8].ToString().Trim();
                strSet9 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Set9].ToString().Trim();

                strSetC1 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC1].ToString().Trim();
                strSetC2 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC2].ToString().Trim();
                strSetC3 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC3].ToString().Trim();
                strSetC4 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC4].ToString().Trim();
                strSetC5 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC5].ToString().Trim();
                strSetC6 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC6].ToString().Trim();
                strSetC7 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC7].ToString().Trim();
                strSetC8 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC8].ToString().Trim();
                strSetC9 = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SetC9].ToString().Trim();

                strWork = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.Work].ToString().Trim();

                strNewChk = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.NewChk].ToString().Trim();

                if (ssDept_Sheet1.Cells[e.Row, (int)ColssDept.EDate].ToString().Trim() != "")
                {
                    if (strNewChk == "OK")
                    {
                        if (MessageBox.Show("신규로 작업가능한 선택의사입니다..신규작업을 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            FstrRowid = "";
                        }
                        else
                        {
                            FstrRowid = "";
                            return;
                        }
                    }
                    else
                    {
                        FstrRowid = "";
                        MessageBox.Show("신규등록만 가능합니다..!!" + "\n\n" + "해당의사로 선택등록이 되어있습니다.. 등록한내용에서 수정을 하십시오", "확인");
                        return;
                    }
                }

                if (FstrRowid != "")
                {
                    btnDelete.Enabled = true;
                    lblSTS.Text = "수정작업->종료일자만 저장가능!!";
                    lblSTS.BackColor = Color.Purple;
                    dtpSDate.Text = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.SDate].ToString().Trim();
                    SelectTxt_Lock_Check("수정");

                    Support_Clear();

                    if (strWork == "1") { chkAgree.Checked = true; }

                    //if (strSet1 == "Y") { ch.Checked = true; }
                    //if (strSet2 == "Y") { chkAgree.Checked = true; }
                    if (strSet3 == "Y") { chkSet3.Checked = true; }
                    //if (strSet4 == "Y") { ch.Checked = true; }
                    if (strSet5 == "Y") { chkSet5.Checked = true; }
                    if (strSet6 == "Y") { chkSet6.Checked = true; }
                    //if (strSet7 == "Y") { chkAgree.Checked = true; }
                    //if (strSet8 == "Y") { chkAgree.Checked = true; }
                    //if (strSet9 == "Y") { chkAgree.Checked = true; }

                    txtSetC3.Text = strSet3;
                    txtSetC5.Text = strSet5;
                    txtSetC6.Text = strSet6;

                    dtpEDate.Focus();
                }
                else
                {
                    lblSTS.Text = "신규등록";
                    lblSTS.BackColor = Color.PaleGreen;
                    dtpSDate.Text = "";
                    SelectTxt_Lock_Check("신규");

                    Support_Clear();
                    grbSet.Enabled = true;

                    if (dtpSDate.Text.Trim() == "")
                    {
                        dtpSDate.Text = clsPublic.GstrSysDate;
                    }
                    dtpSDate.Focus();
                }

                txtDeptCode.Text = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.DeptCode].ToString().Trim();
                lblDept.Text = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.DeptName].ToString().Trim();
                txtDrCode.Text = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.DrCode].ToString().Trim();
                lblDrName.Text = ssDept_Sheet1.Cells[e.Row, (int)ColssDept.DrName].ToString().Trim();
            };
            #endregion


        }
        private void btnPanoSel_Click(object sender, EventArgs e)
        {
            //TODO:Frm개인선택진료정보.Show 1
        }
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                SendKeys.Send("{Tab}");
            }
        }
      

        #endregion

        /// <summary>
        /// 생성자
        /// </summary>
        public frmRegSelectDiagnosis()
        {
            InitializeComponent();

            setEvent();
        }

    }
}
