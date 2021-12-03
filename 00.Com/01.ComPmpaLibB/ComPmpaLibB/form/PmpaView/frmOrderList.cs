using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmOrderList.cs
    /// Description     : 개인별 진료 내역 조회
    /// Author          : 이정현
    /// Create Date     : 2017-06
    /// <history> 
    /// 개인별 진료 내역 조회
    /// </history>
    /// <seealso>
    /// PSMH\OPD\oumsad\FrmSlipView.frm
    /// </seealso>
    /// <vbp>
    /// default 		: PSMH\OPD\oumsad\oumsad.vbp
    /// </vbp>
    /// </summary>
    public partial class frmOrderList : Form
    {
        ComFunc CF = new ComFunc();
        clsOumsadChk OCK = new clsOumsadChk();
        clsPmpaPrint PPT = new clsPmpaPrint();

        private string FstrHDChk = "";

        private string[] strArrDate = null;
        private string[] strArrDept = null;
        private string[] strArrBi = null;
        private string[] strArrSeqno = null;
        private string[] strArrDrName = null;
        private string[] strJName = null;
        private string[] strArrDeptName = null;
        private string[] strArrPart = null;
        private string[] strArrBDate = null;
        private string[] strArrAmt6 = null;

        string statEROVER = "";
        int FnRow = 0;
            
        public frmOrderList()
        {
            InitializeComponent();
        }
        
        private void frmOrderList_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            FormClear ();
        }


        private void FormClear()
        {
            txtPtNo.Text = "";
            txtPtNo.BackColor = Color.White;
            
            lblName.Text = "";
            lblSex.Text = "";
            lblSsno.Text = "";
            lblFrDate.Text = "";
            lblEndDate.Text = "";
            lblSayu.Text = "";

            ssList_Sheet1.RowCount = 0;
            ssView_Sheet1.RowCount = 0;
            ssILL_Sheet1.RowCount = 0;

            dtpFDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-")).AddYears(-1);
            dtpTDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            FnRow = 0;
            txtPtNo.Select();
        }

        private void txtPtNo_MouseDown(object sender, MouseEventArgs e)
        {
        //2018.06.14 박병규 : 등록번호 포커스 이동시 화면 CLEAR 막음. 요청사항(손보영)
        //    FormClear();
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if(ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if (e.KeyCode == Keys.Enter)
            {
                if (VB.IsNumeric(txtPtNo.Text) == false)
                {
                    ComFunc.MsgBox("숫자 형식을 입력해주세요.");
                    txtPtNo.Text = "";
                    txtPtNo.Focus();
                    return;
                }

                txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text, 8, "0");

                ssList_Sheet1.RowCount = 0;
                ssView_Sheet1.RowCount = 0;

                Read_PAT_MAST();
                Read_OS_JINRYO();

                btnPrint.Enabled = true;
            }
        }

        private void txtPtNo_Leave(object sender, EventArgs e)
        {
            txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text, 8, "0");
        }

        private void Read_PAT_MAST()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(StartDate, 'YYYY-MM-DD') Sdate,";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(LastDate, 'YYYY-MM-DD') Ldate, ";
                SQL = SQL + ComNum.VBLF + "       BI, Sname, Sex, Jumin1, Jumin2, Jumin3 ";
                SQL = SQL + ComNum.VBLF + "  FROM BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPtNo.Text.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                lblName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                lblSex.Text = dt.Rows[0]["SEX"].ToString().Trim();
                lblSsno.Text = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["Jumin3"].ToString().Trim());
                lblFrDate.Text = dt.Rows[0]["Sdate"].ToString().Trim();
                lblEndDate.Text = dt.Rows[0]["Ldate"].ToString().Trim();

                dt.Dispose();
                dt = null;

                //60일간 HD과 5건이상체크
                SQL = "";
                SQL = "SELECT Pano ";
                SQL = SQL + ComNum.VBLF + "  FROM OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND ActDate>=TO_DATE('" + Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-15).ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='HD' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 2)
                {
                    FstrHDChk = "OK";
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Read_OS_JINRYO()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            string SqlErr = "";
            int i = 0;

            int intRowCount = 0;

            string strRChk = "";
            string strBi = "";

            ssList_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     TO_CHAR(ActDate,'YYYY-MM-DD') Adate, TO_CHAR(O.BDATE,'YYYY-MM-DD') BDATE, ";
                SQL = SQL + ComNum.VBLF + "     O.DeptCode, MAX(B.DrName) DRNAME, Bi, Seqno, O.PART, MAX(O.DrCode) DRCODE, C.DEPTNAMEK ";

                if(chkRes.Checked == true)
                {
                    SQL = SQL + ", '0' AS OPDNO";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP_RES O";
                }
                else
                {
                    SQL = SQL + ", O.OPDNO AS OPDNO";
                    SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP O";
                }

                SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_DOCTOR B";
                SQL = SQL + ComNum.VBLF + "         ON O.DRCODE = B.DRCODE";
                SQL = SQL + ComNum.VBLF + "     LEFT OUTER JOIN " + ComNum.DB_PMPA + "BAS_CLINICDEPT C";
                SQL = SQL + ComNum.VBLF + "         ON O.DEPTCODE = C.DEPTCODE";
                SQL = SQL + ComNum.VBLF + "     WHERE Pano = '" + txtPtNo.Text.Trim() + "' ";

                if(chkAll.Checked == false)
                {
                    //HD일경우 기본 30일전자료 보여줌
                    //if(FstrHDChk == "OK")
                    //{
                    //    dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddDays(-30);
                    //}
                    //else
                    //{
                    //    dtpFDate.Value = Convert.ToDateTime(clsPublic.GstrSysDate).AddYears(-1);
                    //}

                    SQL = SQL + ComNum.VBLF + "         AND O.BDate >=TO_DATE('" + dtpFDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                    SQL = SQL + ComNum.VBLF + "         AND O.BDate <=TO_DATE('" + dtpTDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                }

                if (chkRes.Checked == true)
                    SQL = SQL + ComNum.VBLF + "GROUP BY O.ActDate, O.DeptCode, Bi,Seqno, O.PART, O.BDATE, C.DEPTNAMEK, '0' ";
                else
                    SQL = SQL + ComNum.VBLF + "GROUP BY O.ActDate, O.DeptCode, Bi,Seqno, O.PART, O.BDATE, C.DEPTNAMEK, OPDNO ";

                SQL = SQL + ComNum.VBLF + "ORDER BY ActDate Desc, SeqNo Desc ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    intRowCount = dt.Rows.Count;

                   // if (dt.Rows.Count > 200)
                   // {
                   //     intRowCount = 500;
                   // }

                    strArrDate = new string[intRowCount];
                    strArrDept = new string[intRowCount];
                    strArrBi = new string[intRowCount];
                    strArrSeqno = new string[intRowCount];
                    strArrDrName = new string[intRowCount];
                    strJName = new string[intRowCount];
                    strArrDeptName = new string[intRowCount];
                    strArrPart = new string[intRowCount];
                    strArrBDate = new string[intRowCount];
                    strArrAmt6 = new string[intRowCount];

                    for(i = 0; i < intRowCount; i++)
                    {
                        strArrDate[i] = dt.Rows[i]["ADATE"].ToString().Trim();
                        strArrDept[i] = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        strArrDeptName[i] = dt.Rows[i]["DEPTNAMEK"].ToString().Trim();
                        strArrBi[i] = dt.Rows[i]["BI"].ToString().Trim();
                        strArrDrName[i] = dt.Rows[i]["DRNAME"].ToString().Trim();
                        strArrSeqno[i] = dt.Rows[i]["SEQNO"].ToString().Trim();
                        strArrPart[i] = dt.Rows[i]["PART"].ToString().Trim();
                        strArrBDate[i] = dt.Rows[i]["BDATE"].ToString().Trim();
                        strJName[i] = lblName.Text;

                        //예약검사 체크 -----------------------------------------------------
                        SQL = "";
                        SQL = "SELECT NVL(SUM(AMT6), 0) AMT6 FROM ADMIN.OPD_RESERVED_EXAM ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPtNo.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND DEPTCODE ='" + strArrDept[i] + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND BDATE =TO_DATE('" + strArrBDate[i] + "','YYYY-MM-DD') ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if(SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if(dt1.Rows.Count == 0)
                        {
                            dt1.Dispose();
                            dt1 = null;
                            Cursor.Current = Cursors.Default;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            if (VB.Val(dt1.Rows[0]["AMT6"].ToString().Trim()) != 0)
                            {
                                strRChk = "▣";
                                strArrAmt6[i] = VB.Val(dt1.Rows[0]["AMT6"].ToString().Trim()).ToString();
                            }
                            else
                            {
                                strRChk = "";
                                //strArrAmt6[i] = 0;
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                        ssList_Sheet1.SetRowHeight(ssList_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = strArrDate[i];
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = strArrDept[i];
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = strArrDrName[i];
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = strArrBi[i];
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 5].Text = strArrSeqno[i];
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 6].Text = strArrBDate[i];
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 7].Text = strRChk;
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 11].Text = strArrPart[i];
                        switch (strArrBi[i])
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                                strBi = "보험";
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                            case "25":
                                strBi = "보호";
                                break;
                            case "31":
                            case "32":
                            case "33":
                            case "34":
                            case "35":
                                strBi = "산재";
                                break;
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                                strBi = "보험100%";
                                break;
                            case "52":
                                strBi = "자보";
                                break;
                            case "55":
                                strBi = "자보일반";
                                break;
                            default:
                                strBi = "일반";
                                break;
                        }



                        //TODO : OUMSAD_CHK.bas
                        //2015-12-12
                        if (RTN_외래접수상세코드(txtPtNo.Text.Trim(), strArrBDate[i], strArrDept[i]) == "09")
                        {
                            ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].BackColor = Color.Pink;
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 8].Text = "Y";
                        }
                        else
                        {
                            //ssList_Sheet1.Rows[ssList_Sheet1.RowCount - 1].BackColor = Color.Green;
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 8].Text = "";
                        }

                        SQL = "";
                        SQL = "SELECT KTASLEVL FROM ADMIN.OPD_MASTER ";
                        SQL = SQL + ComNum.VBLF + " Where Pano = '" + txtPtNo.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "   And BDate = TO_DATE('" + strArrBDate[i] + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   And DEPTCODE = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "'";
                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if(SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if(dt1.Rows.Count > 0)
                        {
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 9].Text = dt1.Rows[0]["KTASLEVL"].ToString().Trim();
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (chkRes.Checked == true)
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 10].Text = "0";
                        else
                            ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["OPDNO"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            if(ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            
            if (chkRes.Checked == true)
            {
                ComFunc.MsgBox("예약검사 slip조회는 인쇄안됨!!");
                return;
            }
            
            clsPmpaPb.GstrJeaPrint = "OK";
            clsPmpaPb.GstrCopyPrint = "";   //Gstr사본출력
            clsPmpaPb.GstrPrintChk = "";    //Gstr출력확인
            clsPmpaPb.Gstr입원재출력 = "";

            if (chkIp.Checked == true) {clsPmpaPb.Gstr입원재출력 = "OK";}
            if (chkCopy.Checked == true) { clsPmpaPb.GstrCopyPrint = "OK"; }

            Card.GnOgAmt = 0;

            if (ssList_Sheet1.ActiveRowIndex < 0)
            {
                ComFunc.MsgBox("대상을 선택후 인쇄하세요");
                
                clsPmpaPb.GstrJeaPrint = "";
                clsPmpaPb.GstrCopyPrint = "";
                clsPmpaPb.GstrPrintChk = "";
                clsPmpaPb.Gstr입원재출력 = "";

                return;
            }
            
            clsPmpaPb.GstrJeaDate = strArrDate[ssList_Sheet1.ActiveRowIndex];
            clsPmpaPb.GstrJeaBi = strArrBi[ssList_Sheet1.ActiveRowIndex];
            clsPmpaPb.GstrJeaDept = strArrDept[ssList_Sheet1.ActiveRowIndex];
            clsPmpaPb.GnJeaSeqNo = Convert.ToInt32(VB.Val(strArrSeqno[ssList_Sheet1.ActiveRowIndex]));
            clsPmpaPb.GstrJeaPart = strArrPart[ssList_Sheet1.ActiveRowIndex];

            try
            {
                SQL = "";
                SQL = "SELECT USERNAME FROM BAS_USER ";
                SQL = SQL + ComNum.VBLF + " WHERE idnumber = '" + clsPmpaPb.GstrJeaPart + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    clsPmpaPb.GstrJeaName = dt.Rows[0]["USERNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
                
                //영수증 재발행 
                clsPmpaPb.GstrJeaSunap = "NO";
                
                if (OCK.Check_RePrint_Input_Suga(clsDB.DbCon, ssView_Sheet1, "$$48") == true)
                {
                    clsPmpaPb.GstrHighRiskMother = "OK";        //Gstr고위험임신부
                }
                else
                {
                    clsPmpaPb.GstrHighRiskMother = "";
                }

                //2016-12-31 임산부 외래 KYO
                if (OCK.Check_RePrint_Input_Suga(clsDB.DbCon, ssView_Sheet1, "@F015") == true)
                {
                    clsPmpaPb.GstrOpdMother = "OK";     // Gstr임신부외래
                }
                else
                {
                    clsPmpaPb.GstrOpdMother = "";
                }

                //2016-12-31 조산아 저체중아 외래 KYO
                if (OCK.Check_RePrint_Input_Suga(clsDB.DbCon, ssView_Sheet1, "@F016") == true)
                {
                    clsPmpaPb.GstrLowWeightBaby = "OK";
                }
                else
                {
                    clsPmpaPb.GstrLowWeightBaby = "";
                }

                //2017-03-13 NP 조헌병 외래 KYO
                if (OCK.Check_RePrint_Input_Suga(clsDB.DbCon, ssView_Sheet1, "@V161") == true)
                {
                    clsPmpaPb.GstrSPR = "OK";
                }
                else
                {
                    clsPmpaPb.GstrSPR = "";
                }
                
                string strJinDtl = RTN_외래접수상세코드(txtPtNo.Text, strArrBDate[ssList_Sheet1.ActiveRowIndex], clsPmpaPb.GstrJeaDept);

                FarPoint.Win.Spread.FpSpread ssSpread = null;

                PPT.Report_Print_Sunap_A4_New(clsDB.DbCon, (long)VB.Val(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 10].Text), txtPtNo.Text, strArrDeptName[ssList_Sheet1.ActiveRowIndex], strJName[ssList_Sheet1.ActiveRowIndex], "", clsPmpaPb.GnJeaSeqNo, "", "", strArrBi[ssList_Sheet1.ActiveRowIndex], strArrBDate[ssList_Sheet1.ActiveRowIndex], "", "", clsPmpaPb.GstrJeaDept, picSign, "", "", "", "", "", "", strJinDtl, ssSpread, "R", clsPmpaPb.GstrJeaDate);

                //2018.05.31 박병규 : 전역변수 초기화
                clsPmpaPb.GstrJeaPrint = "";
                clsPmpaPb.GstrCopyPrint = "";   //Gstr사본출력
                clsPmpaPb.GstrPrintChk = "";    //Gstr출력확인
                clsPmpaPb.Gstr입원재출력 = "";
                clsPmpaPb.GstrJeaDate = "";
                clsPmpaPb.GstrJeaBi = "";
                clsPmpaPb.GstrJeaDept = "";
                clsPmpaPb.GnJeaSeqNo = 0;
                clsPmpaPb.GstrJeaPart = "";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            statEROVER = "";
            clsPmpaPb.GstatEROVER = "";


            clsPmpaPb.GstrPrintChk = "";

            this.Close();
        }

        private void btnPrint2_Click(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strOK = "";

            if(ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인
            
            if (chkRes.Checked == true)
            {
                ComFunc.MsgBox("예약검사 slip조회는 인쇄안됨!!");
                return;
            }

            //TODO : OUMSAD.bas
            clsPmpaPb.GstrJeaPrint = "OK";
            clsPmpaPb.GstrCopyPrint = "";
            clsPmpaPb.GstrPrintChk = "NO";

            if (chkCopy.Checked == true)
            {
                clsPmpaPb.GstrCopyPrint = "OK";
            }
            
            if (ComFunc.MsgBoxQ("정말로 선택한것을 인쇄하시겠습니까?", "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.No)
            {
                clsPmpaPb.GstrJeaPrint = "";
                clsPmpaPb.GstrCopyPrint = "";
                clsPmpaPb.GstrPrintChk = "";

                return;
            }

            try
            {
                SQL = "";
                SQL = "SELECT USERNAME FROM BAS_USER ";
                SQL = SQL + ComNum.VBLF + " WHERE idnumber = '" + clsPmpaPb.GstrJeaPart + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    clsPmpaPb.GstrJeaName = dt.Rows[0]["USERNAME"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                for(i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    //선택한것만
                    if (Convert.ToBoolean(ssList_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strOK = "OK";

                        if (Convert.ToDateTime(ssList_Sheet1.Cells[i, 1].Text.Trim()) <= Convert.ToDateTime(clsPublic.GstrSysDate).AddYears(-5))
                        {
                            if (CF.JIN_AMT_PRINT_CHK(clsDB.DbCon, clsType.User.Sabun) == "OK")
                            {
                                strOK = "OK";
                                ComFunc.MsgBox((i + 1) + "번째줄 영수증은 5년이 지났습니다.. 부서장 확인후 고객에게 주세요!!", "확인");
                            }
                            else
                            {
                                strOK = "";
                                ComFunc.MsgBox((i + 1) + "번째줄 영수증은 5년이 지났습니다..인쇄불가!!", "확인");
                            }
                        }

                        if (strOK == "OK")
                        {
                            //2018.06.05 박병규 : 아래로 변경(멀티선택해도 첫 선택영수증만 재발행)
                            //clsPmpaPb.GstrJeaDate = strArrDate[ssList_Sheet1.ActiveRowIndex];
                            //clsPmpaPb.GstrJeaBi = strArrBi[ssList_Sheet1.ActiveRowIndex];
                            //clsPmpaPb.GstrJeaDept = strArrDept[ssList_Sheet1.ActiveRowIndex];
                            //clsPmpaPb.GnJeaSeqNo = Convert.ToInt32(VB.Val(strArrSeqno[ssList_Sheet1.ActiveRowIndex]));
                            //clsPmpaPb.GstrJeaPart = strArrPart[ssList_Sheet1.ActiveRowIndex];
                            clsPmpaPb.GstrJeaDate = strArrDate[i];
                            clsPmpaPb.GstrJeaBi = strArrBi[i];
                            clsPmpaPb.GstrJeaDept = strArrDept[i];
                            clsPmpaPb.GnJeaSeqNo = Convert.ToInt32(VB.Val(strArrSeqno[i]));
                            clsPmpaPb.GstrJeaPart = strArrPart[i];

                            //영수증 재발행
                            clsPmpaPb.GstrJeaSunap = "NO";

                            FarPoint.Win.Spread.FpSpread ssSpread = null;
                            string strJinDtl = RTN_외래접수상세코드(txtPtNo.Text, strArrBDate[i], clsPmpaPb.GstrJeaDept);
                            //2018.06.05 박병규 : 아래로 변경(멀티선택해도 첫 선택영수증만 재발행)
                            //PPT.Report_Print_Sunap_A4(clsDB.DbCon, (long)VB.Val(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 10].Text), txtPtNo.Text, strArrDeptName[ssList_Sheet1.ActiveRowIndex], strJName[ssList_Sheet1.ActiveRowIndex], "", clsPmpaPb.GnJeaSeqNo, "", "", strArrBi[ssList_Sheet1.ActiveRowIndex], strArrBDate[ssList_Sheet1.ActiveRowIndex], "", "", clsPmpaPb.GstrJeaDept, picSign, "", "", "", "", "", "", "", ssSpread, "R", clsPmpaPb.GstrJeaDate);
                            PPT.Report_Print_Sunap_A4_New(clsDB.DbCon, (long)VB.Val(ssList_Sheet1.Cells[i, 10].Text), txtPtNo.Text, strArrDeptName[i], strJName[i], "", clsPmpaPb.GnJeaSeqNo, "", "", strArrBi[i], strArrBDate[i], "", "", clsPmpaPb.GstrJeaDept, picSign, "", "", "", "", "", "", strJinDtl , ssSpread, "R", clsPmpaPb.GstrJeaDate);
                        }
                    }

                    ssList_Sheet1.Cells[i, 0].Value = false;
                }
                
                clsPmpaPb.GstrJeaPrint = "";
                clsPmpaPb.GstrCopyPrint = "";
                clsPmpaPb.GstrPrintChk = "";

                //2018.05.31 박병규 : 전역변수 초기화
                clsPmpaPb.Gstr입원재출력 = "";
                clsPmpaPb.GstrJeaDate = "";
                clsPmpaPb.GstrJeaBi = "";
                clsPmpaPb.GstrJeaDept = "";
                clsPmpaPb.GnJeaSeqNo = 0;
                clsPmpaPb.GstrJeaPart = "";
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            ssList_Sheet1.RowCount = 0;

            txtPtNo.Text = ComFunc.LPAD(txtPtNo.Text.Trim(), 8, "0");

            if (VB.IsNumeric(txtPtNo.Text))
            {
                Read_PAT_MAST();
                Read_OS_JINRYO();

                btnPrint.Enabled = true;
            }
        }

        private void chkRes_CheckedChanged(object sender, EventArgs e)
        {
            txtPtNo.Text = "";

            if (chkRes.Checked == true)
            {
                chkRes.BackColor = Color.Pink;
            }
            else
            {
                chkRes.BackColor = Color.White;
            }

            ssView_Sheet1.RowCount = 0;

            txtPtNo.Focus();
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strDept = "";
            string strBDate = "";
            string strGub = "";

            strDept = ssList_Sheet1.Cells[e.Row, 2].Text.Trim();
            strBDate = ssList_Sheet1.Cells[e.Row, 6].Text.Trim();

            try
            {
                SQL = "";
                SQL = "SELECT JepSuSayu From ADMIN.OPD_MASTER";
                SQL = SQL + ComNum.VBLF + " Where Pano ='" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "     AND DEPTCODE = '" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "     AND BDate = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    lblSayu.Text = dt.Rows[0]["JepSuSayu"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                for(i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    strGub = ssList_Sheet1.Cells[i, 8].Text.Trim();

                    if(i == e.Row)
                    {
                        if(strGub == "Y")
                        {
                            //ssList_Sheet1.Rows[e.Row].BackColor = Color.Pink;
                        }
                        else
                        {
                            //ssList_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(223, 255, 223);
                        }
                    }
                    else
                    {
                        if(strGub == "Y")
                        {
                            //ssList_Sheet1.Rows[e.Row].BackColor = Color.HotPink;
                        }
                        else
                        {
                            //ssList_Sheet1.Rows[e.Row].BackColor = Color.FromArgb(160, 207, 207);
                        }
                    }
                }

                //2015-01-19 응급실 6시간 대상자 구분 추가
                statEROVER = "";

                //2018.06.04 박병규 : 지역변수가 아닌 전역변수를 사용한다.
                clsPmpaPb.GstatEROVER = "";

                if (e.Row >= 0)
                {
                    Read_OPD_SLIP(e.Row);
                }
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        private void Read_OPD_SLIP(int intIndex)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strSunameK = "";
            string strADate = "";
            string strShow = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";
            string strSeqNo = "";
            string strCardNo = "";
            string strRAmt = "";

            ssView_Sheet1.RowCount = 0;
            ssILL_Sheet1.RowCount = 0;

            strRAmt = strArrAmt6[intIndex];
            strADate = strArrDate[intIndex];

            try
            {
                SQL = "";
                SQL = "SELECT Sucode, SunameK, BaseAmt, Qty, Nal, GbSlip,";
                //   SQL = SQL + ComNum.VBLF + "       GbSpc, GbNgt, GbGisul, o.GbSelf, GbChild,b.Sugbs,";
                //SQL = SQL + ComNum.VBLF + "       GbSpc, GbNgt, GbGisul, o.GbSelf, GbChild,o.gbsugbs sugbs,";
                SQL = SQL + ComNum.VBLF + "       Amt1, Amt2, SeqNo, Part, CardSeqNo,  ";

                if(chkRes.Checked == true)
                {
                    SQL = SQL + ComNum.VBLF + "  GbSpc, GbNgt, GbGisul, o.GbSelf, GbChild,b.sugbs,";
                    SQL = SQL + ComNum.VBLF + "  0 as AMT4, '0' GBSUGBS ";
                    SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP_RES O,  BAS_SUN B";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + "  GbSpc, GbNgt, GbGisul, o.GbSelf, GbChild,o.gbsugbs sugbs,";
                    SQL = SQL + ComNum.VBLF + "  GbEr,AMT4, O.GBSUGBS  ";
                    SQL = SQL + ComNum.VBLF + "  FROM OPD_SLIP O,  BAS_SUN B";
                }

                SQL = SQL + ComNum.VBLF + " WHERE ActDate  = TO_DATE('" + strADate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano     = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Bi       = '" + strArrBi[intIndex] + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + strArrDept[intIndex] + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SeqNo    = '" + strArrSeqno[intIndex] + "' ";
                SQL = SQL + ComNum.VBLF + "   AND  PART     = '" + strArrPart[intIndex] + "' ";
                SQL = SQL + ComNum.VBLF + "   AND O.Sunext = B.Sunext ";
                SQL = SQL + ComNum.VBLF + " ORDER BY  SeqNo, Part, O.Bun, O.Sucode, O.Sunext";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    Cursor.Current = Cursors.Default;
                }
                else
                {
                    ssView_Sheet1.RowCount = dt.Rows.Count;
                    ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SUNAMEK"].ToString().Trim();

                        if(dt.Rows[i]["SUCODE"].ToString().Trim() == "Y97")
                        {
                            ssView_Sheet1.Cells[i, 2].Text = (VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim()) + VB.Val(dt.Rows[i]["AMT4"].ToString().Trim())).ToString("###,###,##0");

                            ssView_Sheet1.Cells[i, 6].Text = (VB.Val(dt.Rows[i]["AMT1"].ToString().Trim()) + VB.Val(dt.Rows[i]["AMT4"].ToString().Trim())).ToString("###,###,##0");
                            ssView_Sheet1.Cells[i, 7].Text = (VB.Val(dt.Rows[i]["AMT2"].ToString().Trim())).ToString("###,###,##0");
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 2].Text = (VB.Val(dt.Rows[i]["BaseAmt"].ToString().Trim())).ToString("###,###,##0");

                            ssView_Sheet1.Cells[i, 6].Text = (VB.Val(dt.Rows[i]["AMT1"].ToString().Trim())).ToString("###,###,##0");
                            ssView_Sheet1.Cells[i, 7].Text = (VB.Val(dt.Rows[i]["AMT2"].ToString().Trim())).ToString("###,###,##0");
                        }
                        ssView_Sheet1.Cells[i, 3].Text = Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim()).ToString("#0.00");
                        ssView_Sheet1.Cells[i, 4].Text = Convert.ToDouble(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");
                        ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GbSpc"].ToString().Trim() + dt.Rows[i]["GbNgt"].ToString().Trim() + dt.Rows[i]["GbGisul"].ToString().Trim() + dt.Rows[i]["GbSelf"].ToString().Trim() + dt.Rows[i]["GbChild"].ToString().Trim() + dt.Rows[i]["sugbs"].ToString().Trim();
                        ssView_Sheet1.Cells[i, 8].Text = VB.Val(dt.Rows[i]["SeqNo"].ToString().Trim()).ToString("###0");
                        ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["Part"].ToString().Trim();

                        if (chkRes.Checked == true)
                        {
                            ssView_Sheet1.Cells[i, 10].Text = "";
                        }
                        else
                        {
                            ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["GbEr"].ToString().Trim();

                            if (VB.Val(dt.Rows[i]["GbEr"].ToString().Trim()) > 0 && VB.Val(dt.Rows[i]["GbEr"].ToString().Trim()) <= 3)
                                ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.Pink;
                            else
                                ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.White;
                        }

                        if (string.Compare(dt.Rows[i]["GBSUGBS"].ToString().Trim(), "1") > 0)
                        {
                            ssView_Sheet1.Cells[i, 0, i, ssView_Sheet1.ColumnCount - 1].BackColor = Color.LightGoldenrodYellow;
                        }
                    
                    

                        if (dt.Rows[i]["GbSlip"].ToString().Trim() == "Z" || dt.Rows[i]["GbSlip"].ToString().Trim() == "E" || dt.Rows[i]["GbSlip"].ToString().Trim() == "Q")
                        {
                            statEROVER = "*";
                            clsPmpaPb.GstatEROVER = "*";

                        }
                        ssView_Sheet1.Cells[i, 11].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;

                    if(VB.Val(strRAmt) > 0 && chkRes.Checked == false)
                    {
                        ComFunc.MsgBox("해당일자 예약금 : " + strRAmt + " 있습니다", "예약검사금액");
                    }

                    if(Convert.ToDateTime(strADate) <= Convert.ToDateTime(clsPublic.GstrSysDate).AddYears(-5))
                    {
                        ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다", "확인");

                        btnPrint.Enabled = false;
                        
                        if (CF.JIN_AMT_PRINT_CHK(clsDB.DbCon, clsType.User.Sabun) == "OK")
                        {
                            btnPrint.Enabled = true;
                        }
                    }
                    else
                    {
                        btnPrint.Enabled = true;
                    }
                }
                
                SQL = "";
                SQL = "SELECT A.ILLCODE, B.ILLNAMEK FROM ADMIN.OCS_OILLS A, ADMIN.BAS_ILLS B";
                SQL = SQL + ComNum.VBLF + " WHERE a.BDate  = TO_DATE('" + strADate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.PTno     = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.DeptCode = '" + strArrDept[intIndex] + "' ";
                SQL = SQL + ComNum.VBLF + "   AND b.IllClass ='1' ";
                SQL = SQL + ComNum.VBLF + "   AND A.ILLCODE = B.ILLCODE";
                SQL = SQL + ComNum.VBLF + " ORDER BY A.SEQNO ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    //ComFunc.MsgBox("해당 DATA가 없습니다.");
                }
                else
                {
                    ssILL_Sheet1.RowCount = dt.Rows.Count;
                    ssILL_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssILL_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssILL_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }
            }
            catch(Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }
        }

        /// <summary>
        /// RTN_외래접수상세코드
        /// </summary>
        /// <param name="strPano"></param>
        /// <param name="strDATE"></param>
        /// <param name="strDept"></param>
        /// <returns></returns>
        private string RTN_외래접수상세코드(string strPano, string strDATE, string strDept)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                SQL = "";
                SQL = "SELECT";
                SQL = SQL + ComNum.VBLF + "     JINDTL";
                SQL = SQL + ComNum.VBLF + "From " + ComNum.DB_PMPA + "OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + "     Where Pano = '" + strPano + "' ";
                SQL = SQL + ComNum.VBLF + "         AND ACTDATE = TO_DATE('" + strDATE + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "         AND DEPTCODE = '" + strDept + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return rtnVal;
                }
                if (dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["JINDTL"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
            catch (Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return rtnVal;
            }
        }

        private void txtPtNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPtNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (Char)13)
            {
                if (!VB.IsNumeric(txtPtNo.Text))
                {
                    txtPtNo.Text = "";
                    txtPtNo.Select();
                    return;
                }

                if (txtPtNo.Text.Trim() == "")
                {
                    txtPtNo.Select();
                    return;
                }

                if (CF.READ_BARCODE(txtPtNo.Text.Trim()) == true)
                {
                    txtPtNo.Text = clsPublic.GstrBarPano;
                }
                else
                {
                    txtPtNo.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtNo.Text));
                }

                e.KeyChar = (char)0;
            }
        }

        private void ssView_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            clsSpread CS = new clsSpread();
            ComFunc CF = new ComFunc();
            clsPmpaFunc CPF = new clsPmpaFunc();

            if (sender == this.ssView)
            {
                if (e.ColumnHeader == true)
                {
                    CS.setSpdSort(ssView, e.Column, true);
                    return;
                }


            }
       
        }
    }
}