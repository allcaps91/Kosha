using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;

namespace ComNurLibB
{
    /// <summary>
    /// Class Name      : MedOprNr
    /// File Name       : 
    /// Description     : 
    /// Author          : 김효성
    /// Create Date     : 2018-01-02
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <seealso cref= "d:\psmh\nurse\nrstd\Frm사망발생보고서.frm >> frmMortalityReturns.cs 폼이름 재정의" />


    public partial class frmMortalityReturns : Form
    {
        string FstrROWID = "";
        //string FstrFlag = "";
        string GstrHelpCode = "";
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");
        ComFunc CF = new ComFunc();
        
        public frmMortalityReturns(string strHlepcode)
        {
            GstrHelpCode = strHlepcode;

            InitializeComponent();
        }

        public frmMortalityReturns()
        {
            InitializeComponent();
        }

        private void SCREEN_CLEAR()
        {
            int i = 0;

            //'인쇄용 sheet
            for (i = 4; i <= 9; i++)
            {
                SS2_Sheet1.Cells[3, 3].Text = "";
                SS2_Sheet1.Cells[3, 5].Text = "";
                SS2_Sheet1.Cells[3, 7].Text = "";
            }

            SS2_Sheet1.Cells[10, 7].Text = "";
            SS2_Sheet1.Cells[12, 6].Text = "";
            SS2_Sheet1.Cells[13, 7].Text = "";
            SS2_Sheet1.Cells[14, 6].Text = "";

            for (i = 17; i <= 19; i++)
            {
                SS2_Sheet1.Cells[i - 1, 6].Text = "";
            }
            for (i = 21; i <= 23; i++)
            {
                SS2_Sheet1.Cells[i - 1, 4].Text = "";
            }

            SS2_Sheet1.Cells[24, 7].Text = "";
            SS2_Sheet1.Cells[25, 2].Text = "";
            SS2_Sheet1.Cells[26, 7].Text = "";
            SS2_Sheet1.Cells[27, 2].Text = "";

            //'진단명..
            SS2_Sheet1.Cells[11, 2].Text = "";
            SS2_Sheet1.Cells[26, 2].Text = "";

            TxtPano.Text = "";
            TxtSex.Text = "";
            TxtAge.Text = "";
            TxtDept.Text = "";
            TxtRoom.Text = "";
            TxtWard.Text = "";
            TxtSName.Text = "";
            TxtDoct1.Text = "";
            TxtDoct2.Text = "";
            dtpIDate.Text = "";
            TxtITime.Text = "";
            TxtDDate.Text = "";
            TxtDTime.Text = "";
            TxtActDate.Text = "";
            TxtSabun.Text = "";

            TxtIDiag.Text = "";
            TxtDDiag.Text = "";
            TxtSisul.Text = "";
            TxtSusul.Text = "";
            TxtHap.Text = "";
            TxtSusulDate2.Text = "";
            TxtSusulName2.Text = "";
            TxtExamRemark.Text = "";
            TxtP_REMARK.Text = "";

            OptDNR1.Checked = true;
            OptCPCR1.Checked = true;
            OptBugum1.Checked = true;
            OptSisul1.Checked = true;
            OptSusul1.Checked = true;
            OptTrans1.Checked = true;
            OptDead1.Checked = true;
            OptDead21.Checked = true;
            OptAdd1.Checked = true;
            OptHap1.Checked = true;
            OptPro1.Checked = true;

            //Control[] controls = ComFunc.GetAllControls(this);    //모든 control을 받아온다

            //foreach (Control ctl in controls)   //foreach문을 사용하여 control을 하나씩 ctl에 넣는다.
            //{
            //    if (ctl is RadioButton)     //한 control이 Radio버튼이라면 ?
            //    {
            //        if (VB.Left(((RadioButton)ctl).Name, 3) == "Opt") //라디오버튼의 이름을 10글자 잘라서 rdoZipName인지 확인
            //        {
            //            ((RadioButton)ctl).Checked = false;
            //            break;  //foreach문 나가기
            //        }
            //    }
            //}

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            TxtIPDNO.Text = "";
            SCREEN_CLEAR();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private bool Save()
        {
            //int j = 0;
            double nIpdNo = 0;
            string strPano = "";
            string strName = "";
            string strAge = "";
            string strSex = "";
            string strIpDiag = "";
            string strDDiag = "";
            string strIpDate = "";
            string strACTDATE = "";
            string strRoom = "";
            string strWARD = "";
            string strDeptCode = "";
            string strSisul = "";
            string strSisulRemark = "";
            string strHap = "";
            string strSusul = "";
            string strSusulRemark = "";
            string strSusulDate2 = "";
            string strSusulName2 = "";
            string strDNR = "";
            string strCPCR = "";
            string strBugum = "";
            string strTrans = "";
            string strDead1 = "";
            string strDead2 = "";
            string strAdd = "";
            string strAdd_Remark = "";
            string strHapbung = "";
            string strPro = "";
            string strPro_Remark = "";
            string strDoct1 = "";
            string strDoct2 = "";
            string strDDate = "";
            string strDTime = "";
            string strIp48 = "";
            string strDJong = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;
            bool rtnVal = false;
            //int i = 0;

            strPano = "";
            strName = "";
            strAge = "";
            strSex = "";
            strIpDiag = "";
            strDDiag = "";
            strIpDate = "";
            strACTDATE = "";
            strRoom = "";
            strWARD = "";
            strDeptCode = "";
            nIpdNo = 0;
            strSisul = "";
            strSisulRemark = "";
            strHap = "";
            strSusul = "";
            strSusulRemark = "";
            strSusulDate2 = "";
            strSusulName2 = "";
            strIp48 = "";
            strDJong = "";
            strAdd_Remark = "";
            strPro_Remark = "";
            strDoct1 = "";
            strDoct2 = "";
            strDDate = "";
            strDTime = "";

            if (GstrHelpCode == "")
            {
                ComFunc.MsgBox("저장실패, 해당환자를 먼저 선택하세요", "확인");
                return false;
            }

            //'기본정보 세팅
            nIpdNo = VB.Val(TxtIPDNO.Text);
            strIpDate = (dtpIDate.Text) + " " + (TxtITime.Text);
            strACTDATE = (TxtActDate.Text);
            strPano = (TxtPano.Text);
            strName = (TxtSName.Text);
            strSex = (TxtSex.Text);
            strAge = (TxtAge.Text);
            strDeptCode = (TxtDept.Text);
            strWARD = (TxtWard.Text);
            strRoom = (TxtRoom.Text);
            strDoct1 = (TxtDoct1.Text);
            strDoct2 = (TxtDoct2.Text);
            strDDate = (TxtDDate.Text);
            strDTime = (TxtDTime.Text);
            strIpDiag = (TxtIDiag.Text);
            strDDiag = (TxtDDiag.Text);
            strSisulRemark = (TxtSisul.Text);
            strHap = (TxtHap.Text);
            strSusulRemark = (TxtSusul.Text);
            strSusulDate2 = (TxtSusulDate2.Text);
            strSusulName2 = (TxtSusulName2.Text);
            strAdd_Remark = (TxtExamRemark.Text);
            strPro_Remark = (TxtP_REMARK.Text);

            if (OptDNR0.Checked == true)
                strDNR = "0";
            else
                strDNR = "1";

            if (OptCPCR0.Checked == true)
                strCPCR = "0";
            else
                strCPCR = "1";
            if (OptBugum0.Checked == true)
                strBugum = "0";
            else
                strBugum = "1";
            if (OptSisul0.Checked == true)
                strSisul = "0";
            else
                strSisul = "1";
            if (OptSusul0.Checked == true)
                strSusul = "0";
            else
                strSusul = "1";
            if (OptTrans0.Checked == true)
                strTrans = "0";
            else
                strTrans = "1";
            if (OptDead0.Checked == true)
                strDead1 = "0";
            else
                strDead1 = "1";
            if (OptDead20.Checked == true)
                strDead2 = "0";
            else
                strDead2 = "1";
            if (OptAdd0.Checked == true)
                strAdd = "0";
            else
                strAdd = "1";
            if (OptHap0.Checked == true)
                strHapbung = "0";
            else
                strHapbung = "1";
            if (OptPro0.Checked == true)
                strPro = "0";
            else
                strPro = "1";
            if (Opt480.Checked == true)
                strIp48 = "0";
            else
                strIp48 = "1";
            if (OptDJong0.Checked == true)
                strDJong = "0";
            else
                strDJong = "1";

            if (strPano == "")
            {
                ComFunc.MsgBox("등록번호가 공백입니다.", "확인");
                return false;
            }
            if (strName == "")
            {
                ComFunc.MsgBox("성명이 공백입니다.", "확인");
                return false;
            }

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL = " SELECT ROWID FROM " + ComNum.DB_PMPA + "NUR_STD_DEATH WHERE PANO = '" + strPano + "'";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }

                if (dt.Rows.Count > 0)
                {
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                }
                else
                {
                    FstrROWID = "";
                }

                dt.Dispose();
                dt = null;

                if (FstrROWID == "")
                {
                    SQL = "";
                    SQL = "INSERT INTO NUR_STD_DEATH ( ";
                    SQL = SQL + ComNum.VBLF + " PANO,SNAME,SEX,AGE,ACTDATE,DDATE,IpDate,DTime,DeptCode,RoomCode,WardCode, ";
                    SQL = SQL + ComNum.VBLF + " Doct1,Doct2,IpDiag,DDiag,DNR,CPCR,Bugum,Trans,IpSuggest,SuggestD,Add_Exam,Add_Exam_Remark, ";
                    SQL = SQL + ComNum.VBLF + " Sisul,Sisul_Remark,HapBung,HapBung_Remark,IpSusul,IpSusul_Remark,";
                    SQL = SQL + ComNum.VBLF + " ReSusul,ReSusul_Remark,Problem,Problem_Remark,Ip48,DeathJong,Ipdno, ";
                    SQL = SQL + ComNum.VBLF + " ENTSABUN ) VALUES ('" + strPano + "','" + strName + "' , ";
                    SQL = SQL + ComNum.VBLF + " '" + strSex + "' , " + VB.Val(strAge) + ", ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strACTDATE + "','YYYY-MM-DD'),TO_DATE('" + strDDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " TO_DATE('" + strIpDate + "','YYYY-MM-DD HH24:MI'),  ";
                    SQL = SQL + ComNum.VBLF + " '" + strDTime + "' ,'" + strDeptCode + "', " + VB.Val(strRoom) + ",'" + strWARD + "' , '" + strDoct1 + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strDoct2 + "','" + strIpDiag + "', '" + strDDiag + "','" + strDNR + "','" + strCPCR + "' ,";
                    SQL = SQL + ComNum.VBLF + "  '" + strBugum + "','" + strTrans + "','" + strDead1 + "', '" + strDead2 + "','" + strAdd + "','" + strAdd_Remark + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSisul + "','" + strSisulRemark + "','" + strHapbung + "','" + strHap + "', ";
                    SQL = SQL + ComNum.VBLF + " '" + strSusul + "','" + strSusulRemark + "',TO_DATE('" + strSusulDate2 + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " '" + strSusulName2 + "','" + strPro + "','" + strPro_Remark + "','" + strIp48 + "','" + strDJong + "', " + nIpdNo + ", ";
                    SQL = SQL + ComNum.VBLF + "  " + clsPublic.GnJobSabun + " ) ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }
                else
                {
                    SQL = "";
                    SQL = " UPDATE NUR_STD_DEATH  SET ";
                    SQL = SQL + ComNum.VBLF + " PANO ='" + strPano + "' , ";
                    SQL = SQL + ComNum.VBLF + " SNAME ='" + strName + "' , ";
                    SQL = SQL + ComNum.VBLF + " SEX ='" + strSex + "' , ";
                    SQL = SQL + ComNum.VBLF + " AGE =" + VB.Val(strAge) + " , ";
                    SQL = SQL + ComNum.VBLF + " ACTDATE =TO_DATE('" + strACTDATE + "','YYYY-MM-DD') , ";
                    SQL = SQL + ComNum.VBLF + " DDATE =TO_DATE('" + strDDate + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " IpDate =TO_DATE('" + strIpDate + "','YYYY-MM-DD HH24:MI'), ";
                    SQL = SQL + ComNum.VBLF + " DTime ='" + strDTime + "', ";
                    SQL = SQL + ComNum.VBLF + " DeptCode ='" + strDeptCode + "' , ";
                    SQL = SQL + ComNum.VBLF + " ROOMCODE =" + VB.Val(strRoom) + " , ";
                    SQL = SQL + ComNum.VBLF + " WardCode ='" + strWARD + "' , ";
                    SQL = SQL + ComNum.VBLF + " Doct1 ='" + strDoct1 + "' , ";
                    SQL = SQL + ComNum.VBLF + " Doct2 ='" + strDoct2 + "' , ";
                    SQL = SQL + ComNum.VBLF + " IpDiag ='" + strIpDiag + "' , ";
                    SQL = SQL + ComNum.VBLF + " DDiag ='" + strDDiag + "' , ";
                    SQL = SQL + ComNum.VBLF + " DNR ='" + strDNR + "' , ";
                    SQL = SQL + ComNum.VBLF + " CPCR ='" + strCPCR + "' , ";
                    SQL = SQL + ComNum.VBLF + " Bugum ='" + strBugum + "' , ";
                    SQL = SQL + ComNum.VBLF + " Trans ='" + strTrans + "' , ";
                    SQL = SQL + ComNum.VBLF + " IpSuggest ='" + strDead1 + "' , ";
                    SQL = SQL + ComNum.VBLF + " SuggestD ='" + strDead2 + "' , ";
                    SQL = SQL + ComNum.VBLF + " Add_Exam ='" + strAdd + "' , ";
                    SQL = SQL + ComNum.VBLF + " Add_Exam_Remark ='" + strAdd_Remark + "' , ";
                    SQL = SQL + ComNum.VBLF + " Sisul ='" + strSisul + "' , ";
                    SQL = SQL + ComNum.VBLF + " Sisul_Remark ='" + strSisulRemark + "' , ";
                    SQL = SQL + ComNum.VBLF + " HapBung ='" + strHapbung + "' , ";
                    SQL = SQL + ComNum.VBLF + " HapBung_Remark ='" + strHap + "' , ";
                    SQL = SQL + ComNum.VBLF + " IpSusul ='" + strSusul + "' , ";
                    SQL = SQL + ComNum.VBLF + " IpSusul_Remark ='" + strSusulRemark + "' , ";
                    SQL = SQL + ComNum.VBLF + " ReSusul= TO_DATE('" + strSusulDate2 + "','YYYY-MM-DD'), ";
                    SQL = SQL + ComNum.VBLF + " ReSusul_Remark ='" + strSusulName2 + "' , ";
                    SQL = SQL + ComNum.VBLF + " Problem ='" + strPro + "' , ";
                    SQL = SQL + ComNum.VBLF + " Problem_Remark ='" + strPro_Remark + "' , ";
                    SQL = SQL + ComNum.VBLF + " Ip48 ='" + strIp48 + "' , ";
                    SQL = SQL + ComNum.VBLF + " DeathJong ='" + strDJong + "' , ";
                    SQL = SQL + ComNum.VBLF + " IpdNo=" + nIpdNo + " ";
                    SQL = SQL + ComNum.VBLF + " WHERE ROWID ='" + FstrROWID + "' ";

                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        Cursor.Current = Cursors.Default;
                        return rtnVal;
                    }
                }

                //clsDB.setRollbackTran(clsDB.DbCon);
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            Search();
            SCREEN_CLEAR();
        }

        private void Search()
        {
            string strWARD = "";
            string strToDate = "";
            string strNextDate = "";
            //string strRemark = "";
            //string strOK = "";
            int nRow = 0;
            //string strFlag = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";

            SSList_Sheet1.Rows.Count = 1;
            SSList_Sheet1.Rows.Count = 0;

            strToDate = dtpDate.Value.ToString("yyy-MM-dd");
            strNextDate = CF.DATE_ADD(clsDB.DbCon, strToDate, 1);

            strWARD = (ComboWard.Text).Trim();

            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (strWARD == "ER")
                {

                    SQL = "";
                    SQL = " SELECT A.PANO, A.JDATE INDATE, A.DEPTCODE, 'ER' WARDCODE, '100' ROOMCODE";
                    SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.NUR_ER_PATIENT a, KOSMOS_PMPA.NUR_STD_DEATH B ";// 'a,  MID_SUMMARY b ";
                    if (OptGbn1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE A.JDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND A.JDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO(+)";
                    }
                    else if (OptGbn2.Checked == true)
                    {

                        SQL = SQL + ComNum.VBLF + "  WHERE B.ACTDATE >= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND B.ACTDATE <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND A.JDATE >= TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "     AND A.JDATE <= TO_DATE('" + dtpEDate.Text + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.PANO = B.PANO";
                    }
                    SQL = SQL + ComNum.VBLF + "  ORDER BY A.JDATE, B.SNAME   ";
                }
                else
                {
                    SQL = "";
                    SQL = " SELECT a.IPDNO, a.Pano, a.SName, a.Sex, a.Age, a.DeptCode,a.DrCode,a.ILSU, ";
                    SQL = SQL + ComNum.VBLF + "  a.Bi, a.ReliGion, a.GbSpc, a.WardCode, a.RoomCode, ";
                    SQL = SQL + ComNum.VBLF + " TO_CHAR(A.INDATE,'YYYY-MM-DD')  INDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.ACTDATE,'YYYY-MM-DD') ACTDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.OUTDATE,'YYYY-MM-DD') OUTDATE , ";
                    SQL = SQL + ComNum.VBLF + "  TO_CHAR(a.InDate,'YYYY-MM-DD') InDate, a.JiYuk  ";
                    SQL = SQL + ComNum.VBLF + "  FROM IPD_NEW_MASTER a, KOSMOS_PMPA.NUR_STD_DEATH B ";// 'a,  MID_SUMMARY b ";
                    if (OptGbn1.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE a.IpwonTime >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + " 00:01','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "  AND a.IpwonTime <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + " 23:59','YYYY-MM-DD HH24:MI') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO(+)";


                    }
                    else if (OptGbn2.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "  WHERE B.ACTDATE >= TO_DATE('" + dtpDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "  AND B.ACTDATE <= TO_DATE('" + dtpEDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND A.IPDNO = B.IPDNO";
                    }
                    SQL = SQL + ComNum.VBLF + "  AND a.Amset4 <> '3' ";

                    switch (ComboWard.Text)
                    {
                        case "전체":
                            break;
                        case "SICU":
                            SQL = SQL + ComNum.VBLF + " AND a.RoomCode = '233' ";
                            break;
                        case "MICU":
                            SQL = SQL + ComNum.VBLF + " AND a.RoomCode = '234' ";
                            break;
                        case "ND":
                        case "NR":
                            SQL = SQL + ComNum.VBLF + " AND a.WARDCODE IN ('ND','NR') ";
                            break;
                        default:
                            SQL = SQL + ComNum.VBLF + " AND a.WardCode IN (" + ReadInWard(ComboWard.Text) + ") ";
                            break;
                    }

                    SQL = SQL + ComNum.VBLF + "  ORDER BY a.RoomCode,a.SName,a.Indate DESC   ";
                }

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    SSList_Sheet1.RowCount = dt.Rows.Count;
                    SSList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        nRow = nRow + 1;

                        SSList_Sheet1.Cells[nRow - 1, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 1].Text = clsVbfunc.GetPatientName(clsDB.DbCon, dt.Rows[i]["PANO"].ToString().Trim());
                        SSList_Sheet1.Cells[nRow - 1, 2].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 3].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                        SSList_Sheet1.Cells[nRow - 1, 4].Text = dt.Rows[i]["Indate"].ToString().Trim();

                        if (ComboWard.Text == "ER")
                        {
                            SSList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        }
                        else
                        {
                            SSList_Sheet1.Cells[nRow - 1, 5].Text = dt.Rows[i]["Ipdno"].ToString().Trim();
                        }
                        SSList_Sheet1.Cells[nRow - 1, 6].Text = "OK";

                        SQL = "";
                        SQL = " SELECT ROWID ";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_STD_DEATH WHERE PANO = '" + dt.Rows[i]["PANO"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTableEx(ref dt1, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            SSList_Sheet1.Cells[nRow - 1, 0, nRow - 1, 1].ForeColor = Color.FromArgb(0, 0, 0);
                            SSList_Sheet1.Cells[nRow - 1, 0, nRow - 1, 1].BackColor = Color.FromArgb(128, 255, 128);
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }

                    dt.Dispose();
                    dt = null;

                    SSList_Sheet1.RowCount = nRow;

                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        /// <summary>
        /// 과거 병동 데이터 조회 되도록 프로그램
        /// 쿼리 사용시 IN으로 조회해야함.
        /// </summary>
        /// <param name="argWard"></param>
        /// <returns></returns>
        private string ReadInWard(string argWard)
        {
            string rtnval = "";
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";


            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT CODE ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_BCODE ";
                SQL = SQL + ComNum.VBLF + " WHERE GUBUN = 'NUR_과거병동조회' ";
                SQL = SQL + ComNum.VBLF + "    AND NAME = '" + argWard + "' ";
                SQL = SQL + ComNum.VBLF + "    AND DELDATE IS NULL ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnval;
                }
                if (dt.Rows.Count == 0)
                {
                    rtnval = "'" + argWard + "'";
                }
                if (dt.Rows.Count > 0)
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        rtnval = rtnval + dt.Rows[i]["CODE"].ToString().Trim() + "','";
                    }
                    rtnval = "'" + rtnval;
                    rtnval = VB.Mid(rtnval, 1, VB.Len(rtnval) - 2);

                }
                dt.Dispose();
                dt = null;

                return rtnval;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                    return rtnval;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            return rtnval;
        }

        private void btnTongbo_2_Click(object sender, EventArgs e)
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            if (ComFunc.MsgBoxQ("사망자발생 서식지를 발행하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();

            SS5_Sheet1.Cells[3, 1].Text = "성명 : " + (TxtSName.Text).Trim() + VB.Space(5) + "등록번호 : " + (TxtPano.Text).Trim() + VB.Space(5) + " 성별 : " + (TxtSex.Text).Trim() + " 나이 : " + (TxtAge.Text).Trim() + VB.Space(5) + (TxtDept.Text).Trim() + " 과";

            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)

                strHeader = CS.setSpdPrint_String(strTitle, new Font("굴림체", 20, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(SS5, PrePrint, setMargin, setOption, strHeader, strFooter);



        }

        private void btnTongbo_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false)
            {
                return;
            }

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;
            clsSpread CS = new clsSpread();
            string strSex = "";
            //string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            switch (TxtSex.Text)
            {
                case "M":
                    strSex = "남자 (M)";
                    break;
                case "F":
                    strSex = "여자 (F)";
                    break;
                default:
                    strSex = TxtSex.Text;
                    break;
            }

            if (ComFunc.MsgBoxQ("사망자발생 통지서를 발행하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }
            //'사망자발생 통지서 인쇄용
            SS3_Sheet1.Cells[3, 1].Text = CF.READ_DEPTNAMEK(clsDB.DbCon, TxtDept.Text) + "(" + TxtDept.Text + ") 과";
            SS3_Sheet1.Cells[3, 2].Text = TxtRoom.Text + " 호실";
            SS3_Sheet1.Cells[3, 4].Text = VB.Space(2) + TxtPano.Text;
            SS3_Sheet1.Cells[4, 2].Text = TxtSName.Text;
            SS3_Sheet1.Cells[4, 3].Text = strSex + " / " + TxtAge.Text + " 세";
            SS3_Sheet1.Cells[4, 4].Text = "병사   외인사   기타 및 불상";

            SS3_Sheet1.Cells[5, 2].Text = VB.Space(5) + VB.Left(dtpIDate.Text, 4) + " 년" + VB.Mid(dtpIDate.Text, 6, 2) + " 월" + VB.Right(dtpIDate.Text, 2) + " 일     " + VB.Left(TxtITime.Text, 2) + " 시   " + VB.Right(TxtITime.Text, 2) + "  분 ";

            SS3_Sheet1.Cells[6, 2].Text = VB.Space(5) + VB.Left(TxtDDate.Text, 4) + " 년" + VB.Mid(TxtDDate.Text, 6, 2) + " 월" + VB.Right(TxtDDate.Text, 2) + " 일     " + VB.Left(TxtDTime.Text, 2) + " 시   " + VB.Right(TxtDTime.Text, 2) + "  분 ";

            SS3_Sheet1.Cells[13, 1].Text = SS3.Text = VB.Space(15) + VB.Left(strDTP, 4) + "    년 " + VB.Mid(strDTP, 6, 2) + "    월 " + VB.Right(strDTP, 2) + "    일";

            strFooter = CS.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
            strFooter += CS.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "   " + "담당자:" + clsType.User.JobName, new Font("굴림체", 9), clsSpread.enmSpdHAlign.Right, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 50, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, false, false, true, true, false, false, false);

            CS.setSpdPrint(SS3, PrePrint, setMargin, setOption, strHeader, strFooter);
        }

        private void frmMortalityReturns_Load(object sender, EventArgs e)
        {
            SSList_Sheet1.Columns[5].Visible = false;

            ComboWard_SET();
            dtpDate.Value = Convert.ToDateTime(strDTP);
            dtpEDate.Value = Convert.ToDateTime(strDTP);
            SCREEN_CLEAR();

            if (GstrHelpCode != "")
            {
                Info_Display(GstrHelpCode);
            }
        }

        private void ComboWard_SET()
        {
            int i = 0;
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            string gsWard = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT WardCode, WardName  ";
                SQL = SQL + ComNum.VBLF + "   FROM KOSMOS_PMPA.BAS_WARD ";
                SQL = SQL + ComNum.VBLF + " WHERE WARDCODE NOT IN ('IU','NP','2W','IQ') ";
                SQL = SQL + ComNum.VBLF + " ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

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
                    Cursor.Current = Cursors.Default;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }
                ComboWard.Items.Clear();
                ComboWard.Items.Add("전체");

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ComboWard.Items.Add(dt.Rows[i]["Wardcode"].ToString().Trim());
                }
                ComboWard.Items.Add("SICU");
                ComboWard.Items.Add("MICU");
                ComboWard.Items.Add("ER");

                ComboWard.SelectedIndex = 0;

                gsWard = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");

                if (gsWard != "")
                {
                    for (i = 0; i < ComboWard.Items.Count; i++)
                    {
                        if (ComboWard.Items.IndexOf(gsWard) == i)
                        {
                            ComboWard.SelectedIndex = i;
                            ComboWard.Enabled = false;
                            return;
                        }
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Info_Display(string ArgIpdNo)
        {
            //int i = 0;
            string SQL = "";
            DataTable dt = null;
            //DataTable dt1 = null;
            string SqlErr = "";
            //int K = 0;
            //string strTemp = "";
            //int nTOT1 = 0;
            int Opt = 0;

            SCREEN_CLEAR();

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = " SELECT TO_CHAR(ActDate,'YYYY-MM-DD') ActDate,TO_CHAR(DDate,'YYYY-MM-DD') DDate,";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(IpDate,'YYYY-MM-DD HH24:MI') IpDate, ";
                SQL = SQL + ComNum.VBLF + "  PANO,IPDNO,DTIME,SNAME,SEX,AGE,DOCT1,DOCT2,WARDCODE,ROOMCODE, ";
                SQL = SQL + ComNum.VBLF + "  DEPTCODE,IPDIAG,DDIAG,DNR,CPCR,BUGUM,TRANS,IPSUGGEST,SUGGESTD,ADD_EXAM,ADD_EXAM_Remark,SISUL,";
                SQL = SQL + ComNum.VBLF + "  SISUL_REMARK , HAPBUNG, HAPBUNG_REMARK, IPSUSUL, IPSUSUL_REMARK, RESUSUL, RESUSUL_REMARK, ";
                SQL = SQL + ComNum.VBLF + "  PROBLEM , Ip48,DeathJong,PROBLEM_REMARK, ENTSABUN,ROWID ";
                SQL = SQL + ComNum.VBLF + "FROM KOSMOS_PMPA.NUR_STD_DEATH ";

                if (ComboWard.Text == "ER")
                    SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + ArgIpdNo + "' ";
                else
                    SQL = SQL + ComNum.VBLF + " WHERE IPDNO = " + VB.Val(ArgIpdNo) + " ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    ComFunc.MsgBox("사망발생보고서 기존등록된 자료를 불러옵니다.", "확인");
                    //FstrFlag = "Y";
                    FstrROWID = dt.Rows[0]["ROWID"].ToString().Trim();
                    //'상세내역
                    TxtPano.Text = dt.Rows[0]["Pano"].ToString().Trim();
                    TxtSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                    TxtAge.Text = dt.Rows[0]["Age"].ToString().Trim();
                    TxtDept.Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                    TxtWard.Text = dt.Rows[0]["WardCode"].ToString().Trim();
                    TxtRoom.Text = dt.Rows[0]["RoomCode"].ToString().Trim();
                    TxtSName.Text = dt.Rows[0]["SName"].ToString().Trim();
                    TxtDoct1.Text = dt.Rows[0]["Doct1"].ToString().Trim();
                    TxtDoct2.Text = dt.Rows[0]["Doct2"].ToString().Trim();
                    dtpIDate.Text = VB.Left(dt.Rows[0]["IpDate"].ToString().Trim(), 10);
                    TxtITime.Text = VB.Right(dt.Rows[0]["IpDate"].ToString().Trim(), 5);
                    TxtDDate.Text = dt.Rows[0]["DDate"].ToString().Trim();
                    TxtDTime.Text = dt.Rows[0]["DTime"].ToString().Trim();
                    TxtActDate.Text = dt.Rows[0]["ActDate"].ToString().Trim();
                    TxtSabun.Text = dt.Rows[0]["EntSabun"].ToString().Trim();
                    TxtIDiag.Text = dt.Rows[0]["IpDiag"].ToString().Trim();
                    TxtDDiag.Text = dt.Rows[0]["DDiag"].ToString().Trim();
                    TxtSisul.Text = dt.Rows[0]["Sisul_Remark"].ToString().Trim();
                    TxtHap.Text = dt.Rows[0]["Hapbung_Remark"].ToString().Trim();
                    TxtSusul.Text = dt.Rows[0]["IpSusul_Remark"].ToString().Trim();
                    TxtSusulDate2.Text = dt.Rows[0]["Resusul"].ToString().Trim();
                    TxtSusulName2.Text = dt.Rows[0]["Resusul_Remark"].ToString().Trim();
                    TxtExamRemark.Text = dt.Rows[0]["ADD_EXAM_Remark"].ToString().Trim();
                    TxtP_REMARK.Text = dt.Rows[0]["PROBLEM_REMARK"].ToString().Trim();


                    if (dt.Rows[0]["DNR"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["DNR"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptDNR0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptDNR1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["CPCR"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["CPCR"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptCPCR0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptCPCR1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Bugum"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Bugum"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptBugum0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptBugum1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Sisul"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Sisul"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptSisul0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptSisul1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["IpSusul"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["IpSusul"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptSusul0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptSusul1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Trans"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Trans"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptTrans0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptTrans1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["IpSuggest"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["IpSuggest"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptDead0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptDead1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Suggestd"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Suggestd"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptDead20.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptDead21.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Add_Exam"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Add_Exam"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptAdd0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptAdd1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Hapbung"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Hapbung"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptHap0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptHap1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Problem"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Problem"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptPro0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptPro1.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["Ip48"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["Ip48"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            Opt480.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            Opt481.Checked = true;
                        }
                    }

                    if (dt.Rows[0]["DeathJong"].ToString().Trim() != "")
                    {
                        Opt = (int)VB.Val(dt.Rows[0]["DeathJong"].ToString().Trim().Replace(",", ""));
                        if (Opt == 0)
                        {
                            OptDJong0.Checked = true;
                        }
                        else if (Opt == 1)
                        {
                            OptDJong1.Checked = true;
                        }
                    }

                    //  '인쇄용 sheet
                    SS2_Sheet1.Cells[3, 3].Text = TxtPano.Text.Trim();
                    SS2_Sheet1.Cells[3, 5].Text = TxtSName.Text.Trim();
                    SS2_Sheet1.Cells[3, 7].Text = TxtDDate.Text.Trim();

                    //Row5
                    SS2_Sheet1.Cells[4, 3].Text = TxtSex.Text.Trim() + "/" + TxtAge.Text.Trim();
                    SS2_Sheet1.Cells[4, 5].Text = TxtDoct1.Text.Trim();
                    SS2_Sheet1.Cells[4, 6].Text = TxtDTime.Text.Trim();

                    //SS2.Row = 6
                    SS2_Sheet1.Cells[5, 3].Text = TxtDept.Text.Trim();
                    SS2_Sheet1.Cells[5, 5].Text = TxtDoct2.Text.Trim();
                    SS2_Sheet1.Cells[5, 7].Text = TxtActDate.Text.Trim();

                    //SS2.Row = 7
                    SS2_Sheet1.Cells[6, 3].Text = TxtWard.Text.Trim();
                    SS2_Sheet1.Cells[6, 5].Text = dtpIDate.Text.Trim();
                    SS2_Sheet1.Cells[6, 7].Text = TxtSabun.Text.Trim();

                    //SS2.Row = 12
                    SS2_Sheet1.Cells[11, 2].Text = TxtIDiag.Text.Trim();

                    //SS2.Row = 17
                    SS2_Sheet1.Cells[16, 2].Text = TxtDDiag.Text.Trim();

                    //SS2.Row = 13
                    SS2_Sheet1.Cells[12, 6].Text = TxtSisul.Text.Trim();

                    //SS2.Row = 15
                    SS2_Sheet1.Cells[14, 6].Text = TxtHap.Text.Trim();

                    //SS2.Row = 17
                    SS2_Sheet1.Cells[16, 6].Text = TxtSusul.Text.Trim();

                    //SS2.Row = 18
                    SS2_Sheet1.Cells[17, 6].Text = TxtSusulDate2.Text.Trim();
                    //SS2.Row = 19
                    SS2_Sheet1.Cells[18, 6].Text = TxtSusulName2.Text.Trim();

                    if (OptDNR0.Checked == true)
                    {
                        SS2_Sheet1.Cells[8, 3].Text = "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[8, 3].Text = "○예 ●아니오";
                    }

                    if (OptCPCR0.Checked == true)
                    {
                        SS2_Sheet1.Cells[8, 5].Text = "●시행 ○시행안함";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[8, 5].Text = "○시행 ●시행안함";
                    }

                    if (OptBugum0.Checked == true)
                    {
                        SS2_Sheet1.Cells[8, 7].Text = "●필요 ○필요없음";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[8, 7].Text = "○필요 ●필요없음";
                    }

                    if (OptSisul0.Checked == true)
                    {
                        SS2_Sheet1.Cells[10, 7].Text = "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[10, 7].Text = "○예 ●아니오";
                    }

                    if (OptHap0.Checked == true)
                    {
                        SS2_Sheet1.Cells[13, 7].Text = "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[13, 7].Text = "○예 ●아니오";
                    }

                    if (OptSusul0.Checked == true)
                    {
                        SS2_Sheet1.Cells[15, 7].Text = "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[15, 7].Text = "○예 ●아니오";
                    }

                    if (OptTrans0.Checked == true)
                    {
                        SS2_Sheet1.Cells[20, 4].Text = "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[20, 4].Text = "○예 ●아니오";
                    }
                    if (Opt480.Checked == true)
                    {
                        SS2_Sheet1.Cells[20, 4].Text = VB.Space(5) + "●Y      ○N";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[20, 4].Text = "○Y      ●N";
                    }


                    if (OptDead0.Checked == true)
                    {
                        SS2_Sheet1.Cells[21, 4].Text = VB.Space(5) + "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[21, 4].Text = "○예 ●아니오";
                    }
                    if (OptDJong0.Checked == true)
                    {
                        SS2_Sheet1.Cells[21, 6].Text = VB.Space(5) + "●병사 ○변사";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[21, 6].Text = "○병사 ●변사";
                    }

                    if (OptDead20.Checked == true)
                    {
                        SS2_Sheet1.Cells[22, 5].Text = VB.Space(5) + "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[22, 5].Text = "○예 ●아니오";
                    }

                    if (OptAdd0.Checked == true)
                    {
                        SS2_Sheet1.Cells[24, 4].Text = VB.Space(5) + "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[24, 4].Text = "○예 ●아니오";
                    }

                    SS2_Sheet1.Cells[25, 2].Text = TxtExamRemark.Text.Trim();

                    if (OptPro0.Checked == true)
                    {
                        SS2_Sheet1.Cells[26, 7].Text = VB.Space(5) + "●예 ○아니오";
                    }
                    else
                    {
                        SS2_Sheet1.Cells[26, 7].Text = "○예 ●아니오";
                    }

                    SS2_Sheet1.Cells[27, 2].Text = TxtP_REMARK.Text.Trim();


                    dt.Dispose();
                    dt = null;
                }
                else
                {
                    dt.Dispose();
                    dt = null;

                    ComFunc.MsgBox("사망발생보고서 기존등록한 자료가 없습니다. 신규등록입니다.");
                    //'신규자료일경우
                    //FstrFlag = "";
                    FstrROWID = "";

                    if (ComboWard.Text == "ER")
                    {
                        SQL = " SELECT A.PANO, A.SEX, AGE, 'ER' WARDCODE, '100' ROOMCODE, ";
                        SQL = SQL + ComNum.VBLF + " A.DEPTCODE, B.SNAME, DRNAME DRCODE, TO_CHAR(INTIME,'YYYY-MM-DD') INDATE, TO_CHAR(INTIME, 'HH24:MI') INTIME";
                        SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.NUR_ER_PATIENT A, KOSMOS_PMPA.BAS_PATIENT B";
                        SQL = SQL + ComNum.VBLF + " WHERE A.PANO = B.PANO ";
                        SQL = SQL + ComNum.VBLF + "    AND A.PANO = '" + ArgIpdNo + "' ";
                    }
                    else
                    {
                        SQL = " SELECT  a.Pano, a.SName, a.Age,a.Sex, TO_CHAR(a.InDate,'YYYY-MM-DD') InDate,InTime,";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(SYSDATE,'YYYY-MM-DD') ActDate, ";
                        SQL = SQL + ComNum.VBLF + " a.DIAGNOSIS , a.Ipdno, a.Grade, b.DeptCode, b.RoomCode,b.WardCode,b.DrCode ";
                        SQL = SQL + ComNum.VBLF + " FROM NUR_MASTER a, IPD_NEW_MASTER b ";
                        SQL = SQL + ComNum.VBLF + " WHERE a.Ipdno=b.Ipdno(+) ";
                        SQL = SQL + ComNum.VBLF + "  AND a.IpdNo =" + VB.Val(ArgIpdNo) + " ";
                    }

                    SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    if (dt.Rows.Count > 0)
                    {
                        TxtIPDNO.Text = ArgIpdNo;
                        TxtPano.Text = dt.Rows[0]["Pano"].ToString().Trim();
                        TxtSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                        TxtAge.Text = VB.Val(dt.Rows[0]["Age"].ToString().Trim().Replace(",", "")).ToString();
                        TxtWard.Text = dt.Rows[0]["WardCode"].ToString().Trim();
                        TxtRoom.Text = VB.Val(dt.Rows[0]["RoomCode"].ToString().Trim().Replace(",", "")).ToString();
                        TxtSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                        TxtDept.Text = dt.Rows[0]["DeptCode"].ToString().Trim();
                        TxtSName.Text = dt.Rows[0]["SName"].ToString().Trim();
                        TxtDoct2.Text = dt.Rows[0]["DrCode"].ToString().Trim();
                        dtpIDate.Text = dt.Rows[0]["InDate"].ToString().Trim();
                        TxtITime.Text = dt.Rows[0]["InTime"].ToString().Trim();
                        TxtActDate.Text = strDTP;
                        TxtSabun.Text = READ_INSA_Name(clsType.User.Sabun);
                    }

                    dt.Dispose();
                    dt = null;

                    //FstrFlag = "";
                }
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_INSA_Name(string argSabun)
        {
            DataTable dt = null;
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            string strData = "";

            if (VB.Val(argSabun) == 0)
                return "";

            try
            {

                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT KorName , buse  ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_ERP + "INSA_MST  ";
                SQL = SQL + ComNum.VBLF + "  WHERE Sabun = '" + argSabun + "' ";
                SQL = SQL + ComNum.VBLF + "   AND ( TOIDAY IS NULL OR TOIDAY < TRUNC(SYSDATE) ) ";

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (dt == null)
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");

                    return "";
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");

                    return "";
                }

                strData = dt.Rows[0]["KorName"].ToString().Trim();


                dt.Dispose();
                dt = null;
                return strData;

            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                return strData;
            }
        }

        private void SSList_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            GstrHelpCode = SSList_Sheet1.Cells[e.Row, 5].Text;
            TxtIPDNO.Text = SSList_Sheet1.Cells[e.Row, 5].Text;

            Info_Display(GstrHelpCode);
        }
    }
}
