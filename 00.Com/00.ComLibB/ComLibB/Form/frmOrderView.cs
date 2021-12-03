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

namespace ComLibB
{
    public partial class frmOrderView : Form
    {
        private string strFlagChange = "";
        private string strArrDate = "";
        private string strArrDept = "";
        private string strArrBi = "";
        private string strArrDrCode = "";
        private string strArrSeqNo = "";
        private string strArrActDate = "";

        public frmOrderView()
        {
            InitializeComponent();
        }

        private void frmOrderView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            FormClear ();
        }

        private void FormClear()
        {
            lblName.Text = "";
            lblSex.Text = "";
            lblSsno.Text = "";
            lblFrDate.Text = "";
            lblEndDate.Text = "";
            lblSayu.Text = "";

            txtPtNo.Text = "";
            rtxt.Rtf = "";
            panMsg.Visible = false;

            ssILL_Sheet1.RowCount = 0;
            ssPat_Sheet1.RowCount = 0;
            ssSlip_Sheet1.RowCount = 0;

            ssRate_Sheet1.RowCount = 0;
            ssRate_Sheet1.RowCount = 1;

            ssSlip_Sheet1.Columns[16].Visible = false;  //rowid
            ssSlip_Sheet1.Columns[17].Visible = false;  //bun
            ssSlip_Sheet1.Columns[18].Visible = false;  //F_old
            ssSlip_Sheet1.Columns[19].Visible = false;  //bi
            ssSlip_Sheet1.Columns[20].Visible = false;  //nu
            ssSlip_Sheet1.Columns[21].Visible = false;  //nu_old
            ssSlip_Sheet1.Columns[22].Visible = false;  //q항

            cboPtNo.Items.Clear();

            txtPtNo.Text = clsPublic.GstrPANO;      //등록번호 자동입력 2012-04-03 이주형
        }

        private void txtPtNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPtNoEnter();
            }
        }

        private void txtPtNoEnter()
        {
            string strChk = "";
            int i = 0;

            if(ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            if(VB.IsNumeric(txtPtNo.Text))
            {
                Read_PAT_MAST();
                Read_OS_JINRYO();

                ssSlip_Sheet1.Columns[8].Locked = true;

                //삼사계전용:
                switch(clsPublic.GnJobSabun)
                {
                    case 4349:
                    case 7834:
                    case 7843:
                    case 13537:
                    case 468:
                    case 2749:
                    case 15273:
                    case 19399:
                    case 21181:
                    case 13635:
                    case 28253:
                    case 17812:
                    case 22456:
                    case 22699:
                    case 33674:
                    case 37074:
                        Read_BAS_OCSMEMO_O();
                        Read_BAS_OCSMEMO_MIR();
                        ssSlip_Sheet1.Columns[8].Locked = false;
                        break;
                }

                strChk = "";

                for(i = 0; i < cboPtNo.Items.Count; i++)
                {
                    if(cboPtNo.Items[i].ToString().Trim() == txtPtNo.Text.Trim())
                    {
                        strChk = "NO";
                        break;
                    }
                }

                if(strChk == "")
                {
                    cboPtNo.Items.Add(txtPtNo.Text.Trim());
                }

                clsPublic.GstrPANO = txtPtNo.Text;      //등록번호 자동입력을 위한 전역 변수 2012-04-03 이주형
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
                SQL = SQL + ComNum.VBLF + "       Sname, Sex, Jumin1, Jumin2, Jumin3 ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.BAS_PATIENT ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano = '" + txtPtNo.Text + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    return;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                lblName.Text = dt.Rows[0]["SNAME"].ToString().Trim();
                lblSex.Text = dt.Rows[0]["SEX"].ToString().Trim();
                lblSsno.Text = dt.Rows[0]["JUMIN1"].ToString().Trim() + "-" + clsAES.DeAES(dt.Rows[0]["JUMIN3"].ToString().Trim());
                lblFrDate.Text = dt.Rows[0]["SDATE"].ToString().Trim();
                lblEndDate.Text = dt.Rows[0]["LDATE"].ToString().Trim();

                dt.Dispose();
                dt = null;

            }
            catch(Exception ex)
            {
                if (dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

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

            ssPat_Sheet1.RowCount = 0;
            ssSlip_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(ActDate,'YYYY-MM-DD') Adate, ";
                SQL = SQL + ComNum.VBLF + "       DeptCode, DrName, Bi, o.DRCode,   ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(BDATE,'YYYY-MM-DD') BDATE ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OPD_SLIP O, ADMIN.BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + " WHERE Pano     = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND O.DrCode = B.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + " GROUP BY ActDate, DeptCode, DrName, Bi, o.DrCode, BDATE ";
                SQL = SQL + ComNum.VBLF + " ORDER BY ActDate Desc, BDATE Desc ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssPat_Sheet1.RowCount = ssPat_Sheet1.RowCount + 1;
                        ssPat_Sheet1.SetRowHeight(ssPat_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["ADATE"].ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["DRNAME"].ToString().Trim();

                        switch(dt.Rows[i]["BI"].ToString().Trim())
                        {
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "보험";
                                break;
                            case "21":
                            case "22":
                            case "23":
                            case "24":
                            case "25":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "보호";
                                break;
                            case "31":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "산재";
                                break;
                            case "32":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "공무공상";
                                break;
                            case "33":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "산재공상";
                                break;
                            case "41":
                            case "42":
                            case "43":
                            case "44":
                            case "45":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "보험100%";
                                break;
                            case "52":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "TA보험";
                                break;
                            case "55":
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "TA일반";
                                break;
                            default:
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 3].Text = "일반";
                                break;
                        }

                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["DRCODE"].ToString().Trim();
                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["BI"].ToString().Trim();
                        
                        SQL = "";
                        SQL = "SELECT TO_CHAR(ACTDATE, 'YYYY-MM-DD') JOBDATE, ";
                        SQL = SQL + ComNum.VBLF + " TO_CHAR(BDATE, 'YYYY-MM-DD') BDATE ";
                        SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_HOANBUL ";
                        SQL = SQL + ComNum.VBLF + " WHERE PANO  = '" + txtPtNo.Text + "'";
                        SQL = SQL + ComNum.VBLF + "   AND JOBDATE = TO_DATE('" + dt.Rows[i]["ADATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if(SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if(dt1.Rows.Count > 0)
                        {
                            if (dt1.Rows[0]["JOBDATE"].ToString().Trim() != dt1.Rows[0]["BDATE"].ToString().Trim())
                            {
                                ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 6].Text = "◈";
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;

                        ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["BDATE"].ToString().Trim();

                        SQL = "";
                        SQL = "SELECT BonRate From ADMIN.OPD_Master ";
                        SQL = SQL + ComNum.VBLF + " Where Pano= '" + txtPtNo.Text + "' ";
                        SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + dt.Rows[i]["BDATE"].ToString().Trim() + "','YYYY-MM-DD') ";
                        SQL = SQL + ComNum.VBLF + "   AND DEPTCODE = '" + dt.Rows[i]["DEPTCODE"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if(SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if(dt1.Rows.Count > 0)
                        {
                            ssPat_Sheet1.Cells[ssPat_Sheet1.RowCount - 1, 8].Text = VB.IIf(dt1.Rows[0]["BONRATE"].ToString().Trim() == "E", "E", "").ToString();
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                }

                dt.Dispose();
                dt = null;

                txtPtNo.Focus();
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Read_BAS_OCSMEMO_O()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strMsg = "";
            int i = 0;

            try
            {
                SQL = "";
                SQL = "SELECT MEMO FROM ADMIN.BAS_OCSMEMO_O ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if (i == 0)
                        {
                            strMsg = dt.Rows[i]["MEMO"].ToString().Trim();
                        }

                        strMsg = strMsg + dt.Rows[i]["MEMO"].ToString().Trim();
                    }

                    strMsg = strMsg.Replace("`", "'");

                    if (strMsg != "")
                    {
                        rtxt.Rtf = strMsg;
                        panMsg.Visible = true;
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void Read_BAS_OCSMEMO_MIR()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string strMsg = "";
            int i = 0;

            try
            {
                SQL = "";
                SQL = "SELECT MEMO FROM ADMIN.BAS_OCSMEMO_MIR ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO ='" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DDATE IS NULL ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if(dt.Rows.Count > 0)
                {
                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        if(i == 0)
                        {
                            strMsg = dt.Rows[i]["MEMO"].ToString().Trim();
                        }

                        strMsg = strMsg + dt.Rows[i]["MEMO"].ToString().Trim();
                    }

                    strMsg = strMsg.Replace("`", "'");

                    if(strMsg != "")
                    {
                        rtxt.Rtf = strMsg;
                        panMsg.Visible = true;
                    }
                }

                dt.Dispose();
                dt = null;

            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void cboPtNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboPtNo.Text != "")
            {
                txtPtNo.Text = cboPtNo.Text.Trim();

                txtPtNoEnter();
            }
        }

        private void btnNHic_Click(object sender, EventArgs e)
        {
            string strSname = "";
            string strPtNo = "";
            string strJumin = "";
            string strDeptCode = "";
            string strBDate = "";

            strBDate = clsPublic.GstrSysDate;
            strSname = lblName.Text.Trim();
            strPtNo = txtPtNo.Text.Trim();
            strJumin = lblSsno.Text.Replace("-", "");
            strDeptCode = strArrDept;
            
            if (strPtNo == "")
            {
                ComFunc.MsgBox("환자등록번호가 공란입니다..");
                return;
            } 

            if (strDeptCode == "")
            {
                strDeptCode = "ME";
                ComFunc.MsgBox("최종진료과가 없습니다.. 자격조회시 ME로 자격조회합니다..");
            }

            if (strPtNo == "06927136")
            {
                strSname = "마리벨시파곤산";
            }

            //2012-09-11
            if(VB.I(strSname, "A") > 1 || VB.I(strSname, "B") > 1 || VB.I(strSname, "C") > 1 || VB.I(strSname, "D") > 1)
            {
                if (ComFunc.MsgBoxQ("성명에 A,B,C,D가 포함되어 있습니다...성명수정후 자격조회 하시겠습니까??"
                    , "확인", MessageBoxDefaultButton.Button2) == System.Windows.Forms.DialogResult.Yes)
                {
                    strSname = VB.InputBox("성명을 다시 확인하십시오.", "이름확인", strSname);
                }
            }

            clsPublic.GstrHelpCode = strPtNo + "," + strDeptCode + "," + strSname + "," + strJumin + "," + strBDate;

            //TODO : 원무 자격조회
            //Frm보험자격확인NEW2

            clsPublic.GstrHelpCode = "";
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtPtNoEnter();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            rtxt.Rtf = "";
            panMsg.Visible = false;
        }

        private void ssPat_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strDate = "";

            if(e.ColumnHeader == true || e.RowHeader == true)
            {
                return;
            }

            strDate = ssPat_Sheet1.Cells[e.Row, 0].Text.Trim();
            strArrActDate = ssPat_Sheet1.Cells[e.Row, 0].Text.Trim();
            strArrDept = ssPat_Sheet1.Cells[e.Row, 1].Text.Trim();
            strArrDrCode = ssPat_Sheet1.Cells[e.Row, 4].Text.Trim();
            strArrBi = ssPat_Sheet1.Cells[e.Row, 5].Text.Trim();
            strArrDate = ssPat_Sheet1.Cells[e.Row, 7].Text.Trim();

            ssRate_Sheet1.RowCount = 0;
            ssRate_Sheet1.RowCount = 1;

            Read_OPD_SLIP(strDate, strArrDept, strArrDrCode, strArrBi, strArrDate);
        }

        private void Read_OPD_SLIP(string strDate, string strDept, string strDrCode, string strBi, string strBDate)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strNew = "";
            string strOld = "";
            double dblSeqNo = 0;

            lblSayu.Text = Read_Opd_Jepsu_Gubun(txtPtNo.Text, strBDate, strDept);

            ssSlip_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = "SELECT Sucode, SunameK, BaseAmt, Qty, Nal,o.bun,o.bi,o.nu,b.SUGBQ,";
                SQL = SQL + ComNum.VBLF + "       GbSpc, GbNgt, GbGisul, GbSelf, GbChild,";
                SQL = SQL + ComNum.VBLF + "       Amt1, Amt2, SeqNo, Part, O.Rowid, O.GbSlip, O.GbSuGbS ";
                SQL = SQL + ComNum.VBLF + "  FROM ADMIN.OPD_SLIP O,  ADMIN.BAS_SUN B";
                SQL = SQL + ComNum.VBLF + " WHERE ActDate  = TO_DATE('" + strDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND Pano     = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND Bi       = '" + strBi + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDATE = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DeptCode = '" + strDept + "' ";
                SQL = SQL + ComNum.VBLF + "   AND DrCode = '" + strDrCode + "' ";
                SQL = SQL + ComNum.VBLF + "   AND O.Sunext = B.Sunext ";

                if(chkF.Checked == false)
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY  SeqNo, O.Bun, O.Sucode, O.Sunext";
                }
                else
                {
                    SQL = SQL + ComNum.VBLF + " ORDER BY SeqNo,decode(o.bun,'92','99','96','99','98','99','99','99',gbself) , O.Bun, O.Sucode, O.Sunext";
                }

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
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                ssSlip_Sheet1.RowCount = dt.Rows.Count;
                ssSlip_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    strNew = dt.Rows[i]["SEQNO"].ToString().Trim();
                    dblSeqNo = VB.Val(strNew);

                    if (strNew != strOld)
                    {
                        //.SetCellBorder 1, .Row, .MaxCols, .Row, SS_BORDER_TYPE_TOP Or SS_BORDER_TYPE_OUTLINE, &HFF0000, SS_BORDER_STYLE_SOLID  '2012-11-30

                        if(i != 0)
                        {
                            ssSlip_Sheet1.Cells[i - 1, 12].Text = READ_SLIP_nHIC_STS(dt.Rows[i - 1]["PART"].ToString().Trim(), VB.Val(dt.Rows[i - 1]["SEQNO"].ToString().Trim()));
                        }
                    }

                    ssSlip_Sheet1.Cells[i, 0].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SunameK"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 2].Text = Convert.ToDouble(dt.Rows[i]["BaseAmt"].ToString().Trim()).ToString("###,###,##0");
                    ssSlip_Sheet1.Cells[i, 3].Text = Convert.ToDouble(dt.Rows[i]["Qty"].ToString().Trim()).ToString("#0.0");
                    ssSlip_Sheet1.Cells[i, 4].Text = Convert.ToDouble(dt.Rows[i]["Nal"].ToString().Trim()).ToString("##0");
                    ssSlip_Sheet1.Cells[i, 5].Text = dt.Rows[i]["GbSpc"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GbNgt"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 7].Text = dt.Rows[i]["GbGisul"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 8].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 9].Text = dt.Rows[i]["GbChild"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 10].Text = Convert.ToDouble(dt.Rows[i]["Amt1"].ToString().Trim()).ToString("###,###,##0");
                    ssSlip_Sheet1.Cells[i, 11].Text = Convert.ToDouble(dt.Rows[i]["Amt2"].ToString().Trim()).ToString("###,###,##0");

                    ssSlip_Sheet1.Cells[i, 13].Text = Convert.ToDouble(dt.Rows[i]["SEQNO"].ToString().Trim()).ToString("###0");
                    ssSlip_Sheet1.Cells[i, 14].Text = dt.Rows[i]["PART"].ToString().Trim();

                    if (dt.Rows[i]["GBSLIP"].ToString().Trim() == "Z" || dt.Rows[i]["GBSLIP"].ToString().Trim() == "E" | dt.Rows[i]["GBSLIP"].ToString().Trim() == "Q")
                    {
                        ssSlip_Sheet1.Cells[i, 15].Text = "E";
                    }

                    ssSlip_Sheet1.Cells[i, 16].Text = dt.Rows[i]["RowID"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 17].Text = dt.Rows[i]["Bun"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 18].Text = dt.Rows[i]["GbSelf"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 19].Text = dt.Rows[i]["Bi"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 20].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 21].Text = dt.Rows[i]["Nu"].ToString().Trim();
                    ssSlip_Sheet1.Cells[i, 22].Text = dt.Rows[i]["SUGBQ"].ToString().Trim();

                    if (i == dt.Rows.Count)
                    {
                        ssSlip_Sheet1.Cells[i, 12].Text = READ_SLIP_nHIC_STS(dt.Rows[i]["PART"].ToString().Trim(), dblSeqNo);
                    }

                    ssSlip_Sheet1.Cells[i, 23].Text = dt.Rows[i]["GBSUGBS"].ToString().Trim();

                    if(READ_BAS_Sun_S항(dt.Rows[i]["SUCODE"].ToString().Trim()) == "6" || READ_BAS_Sun_S항(dt.Rows[i]["SUCODE"].ToString().Trim()) == "7")
                    {
                        ssSlip_Sheet1.Cells[i, 0, i, ssSlip_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(169, 208, 142);
                    }
                    else
                    {
                        ssSlip_Sheet1.Cells[i, 0, i, ssSlip_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 255);
                    }

                    strOld = dt.Rows[i]["SEQNO"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;

                ssILL_Sheet1.RowCount = 0;

                //JJY(2005-02-17) 심사계요청으로 추가
                SQL = "";
                SQL = "SELECT A.ILLCODE, B.ILLNAMEK FROM ADMIN.OCS_OILLS A, ADMIN.BAS_ILLS B";
                SQL = SQL + ComNum.VBLF + " WHERE a.BDate  = TO_DATE('" + strArrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND a.PTno     = '" + txtPtNo.Text + "' ";
                SQL = SQL + ComNum.VBLF + "   AND A.DeptCode = '" + strDept + "' ";
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
                if(dt.Rows.Count > 0)
                {
                    ssILL_Sheet1.RowCount = dt.Rows.Count;
                    ssILL_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for(i = 0; i < dt.Rows.Count; i++)
                    {
                        ssILL_Sheet1.Cells[i, 0].Text = dt.Rows[i]["ILLCODE"].ToString().Trim();
                        ssILL_Sheet1.Cells[i, 1].Text = dt.Rows[i]["ILLNAMEK"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private string READ_BAS_Sun_S항(string strSuNext)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            if (strSuNext.Trim() == "")
            {
                return rtnVal;
            }

            try
            {
                SQL = "";
                SQL = "SELECT SugbS FROM ADMIN.BAS_SUN  ";
                SQL = SQL + ComNum.VBLF + " WHERE SuNext  = '" + strSuNext.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["SUGBS"].ToString().Trim();
                    return rtnVal;
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string Read_Opd_Jepsu_Gubun(string strPtNo, string strBDate, string strDept)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT JIN, JINDTL, JepsuSayu From ADMIN.OPD_MASTER ";
                SQL = SQL + ComNum.VBLF + " Where Pano ='" + strPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "   AND BDate = TO_DATE('" + strBDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND DEPTCODE ='" + strDept + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count > 0)
                {
                    rtnVal = dt.Rows[0]["JepsuSayu"].ToString().Trim();

                    if(dt.Rows[0]["JINDTL"].ToString().Trim() != "")
                    {
                        rtnVal = rtnVal + " (" + dt.Rows[0]["JINDTL"].ToString().Trim() + ")";
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private string READ_SLIP_nHIC_STS(string strPart, double dblSeq)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT PANO,VCODE,MCODE,JIN,JINDTL,ETCAMT,ETCAMT2,JINDTL2,GELCODE,Remark,amt ";
                SQL = SQL + ComNum.VBLF + " FROM ADMIN.OPD_SUNAP ";
                SQL = SQL + ComNum.VBLF + "  WHERE PANO ='" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "   AND (DEPTCODE ='" + strArrDept + "' OR DEPTCODE ='II')  ";
                SQL = SQL + ComNum.VBLF + "   AND ACTDATE =TO_DATE('" + strArrActDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "   AND (BDATE =TO_DATE('" + strArrDate + "','YYYY-MM-DD') OR BDATE IS NULL)  ";
                SQL = SQL + ComNum.VBLF + "   AND PART ='" + strPart + "' ";
                SQL = SQL + ComNum.VBLF + "   AND SEQNO2 =" + dblSeq + " ";
                SQL = SQL + ComNum.VBLF + "   AND (DELDATE IS NULL OR DELDATE ='') ";

                //접수비
                if(dblSeq == 0)
                {
                    SQL = SQL + ComNum.VBLF + "   AND REMARK IN ('접수비')  ";
                }

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["VCODE"].ToString().Trim() != "")
                    {
                        rtnVal = rtnVal + dt.Rows[0]["VCODE"].ToString().Trim() + " ";
                    }

                    if(dt.Rows[0]["MCODE"].ToString().Trim() != "")
                    {
                        rtnVal = rtnVal + dt.Rows[0]["MCODE"].ToString().Trim() + " ";
                    }

                    if (dt.Rows[0]["JinDtl2"].ToString().Trim() != "")
                    {
                        switch(dt.Rows[0]["JinDtl2"].ToString().Trim())
                        {
                            case "01":
                                rtnVal = rtnVal + "장루.요루";
                                break;
                            case "02":
                                rtnVal = rtnVal + "완전틀니";
                                break;
                        }
                    }

                    if (VB.Val(dt.Rows[0]["AMT"].ToString().Trim()) < 0)
                    {
                        rtnVal = "";
                    }
                    else
                    {
                        rtnVal = rtnVal.Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                return rtnVal;
            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return rtnVal;
            }
        }

        private void ssSlip_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPtNo = "";
            string strActDate = "";
            string strBDate = "";
            string strBI = "";
            string strDept = "";
            string strPart = "";
            int intSeqNo = 0;

            strPtNo = txtPtNo.Text.Trim();
            strActDate = strArrActDate;
            strBDate = strArrDate;

            strBI = strArrBi;
            strDept = strArrDept;

            if (e.ColumnHeader == false && e.RowHeader == false)
            {
                intSeqNo = Convert.ToInt32(VB.Val(ssSlip_Sheet1.Cells[e.Row, 13].Text.Trim()));
                strPart = ssSlip_Sheet1.Cells[e.Row, 14].Text.Trim();

                //TODO : 원무 접수비 루틴
                //Read_Patient_Rate_Chk(ssRate, "O", strPtNo, strActDate, strBDate, strBI, strDept, strPart, intSeqNo, 0, 0);
            }
        }

        private void ssSlip_EditModeOff(object sender, EventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int intRowAffected = 0;

            string strROWID = "";
            string strF = "";
            string strOldF = "";
            string strNu = "";
            string strOldNu = "";
            string strSuCode = "";
            string strBi = "";
            string strQ = "";

            double dblAmt1 = 0;
            double dblAmt2 = 0;

            if(ComQuery.IsJobAuth(this, "U", clsDB.DbCon) == false) return; //권한 확인

            if (ssSlip_Sheet1.ActiveColumnIndex != 8)
            {
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            strSuCode = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 0].Text.Trim();
            strF = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 8].Text.Trim();
            strOldF = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 18].Text.Trim();
            strBi = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 19].Text.Trim();

            strNu = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 20].Text.Trim();
            strOldNu = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 21].Text.Trim();
            strQ = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 22].Text.Trim();

            dblAmt1 = VB.Val(VB.TR(ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 10].Text.Trim(), ",", ""));
            dblAmt2 = VB.Val(VB.TR(ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 11].Text.Trim(), ",", ""));

            strROWID = ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 16].Text.Trim();

            if (dblAmt1 + dblAmt2 != 0)
            {
                ComFunc.MsgBox("금액발생 수가는 수정불가함!!");
                ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 8].Text = strOldF;
                return;
            }

            if (strF != "0" && strF != "1" && strF != "2")
            {
                ComFunc.MsgBox("F항의 값은 0, 1, 2 값만 허용됩니다.");
                ssSlip_Sheet1.Cells[ssSlip_Sheet1.ActiveRowIndex, 8].Text = strOldF;
                return;
            }

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                //누적재정리  2012-11-12
                if(strBi == "31" && VB.Val(strQ) > 0)
                {
                    switch(strOldNu)
                    {
                        case "38":
                            strNu = "20";
                            break;
                    }
                }
                else if(VB.Val(strBi) < 40)
                {
                    if(strF == "2")
                    {
                        switch(strOldNu)
                        {
                            case "01":
                            case "02":
                            case "03":
                                strNu = "21";
                                break;
                            case "04":
                            case "05":
                            case "06":
                            case "07":
                            case "08":
                            case "09":
                            case "10":
                            case "11":
                            case "12":
                            case "13":
                            case "14":
                            case "15":
                                strNu = (VB.Val(strOldNu) + 18).ToString("00");
                                break;
                            case "16":
                                strNu = "34";
                                break;
                            case "17":
                                strNu = "42";
                                break;
                            case "18":
                                strNu = "38";
                                break;
                            case "19":
                                strNu = "37";
                                break;
                            case "20":
                                strNu = "27";
                                break;
                        }
                    }
                }
                else if (strF == "0" || strF == "")
                {
                    //기존  수가 읽기 2012-11-12
                    SQL = "";
                    SQL = "SELECT Sucode,Bun,Nu,SugbA,SugbB,SugbC,                                       ";
                    SQL = SQL + ComNum.VBLF + "       SugbD,SugbE,SugbF,SugbG,SugbH,SugbI,SugbJ, n.SugbQ, n.SugbR,n.SugbS, Iamt,     ";
                    SQL = SQL + ComNum.VBLF + "       Tamt,Bamt,TO_CHAR(Sudate, 'yyyy-mm-dd') Suday,                         ";
                    SQL = SQL + ComNum.VBLF + "       OldIamt,OldTamt,OldBamt,DayMax,TotMax, t.Sunext,                       ";
                    SQL = SQL + ComNum.VBLF + "       TO_CHAR(t.Sudate3, 'yyyy-mm-dd') Sudate3,                              ";
                    SQL = SQL + ComNum.VBLF + "       t.Iamt3, t.Tamt3, t.Bamt3, TO_CHAR(t.Sudate4, 'yyyy-mm-dd') Sudate4,   ";
                    SQL = SQL + ComNum.VBLF + "       t.Iamt4, t.Tamt4, t.Bamt4, TO_CHAR(t.Sudate5, 'yyyy-mm-dd') Sudate5,   ";
                    SQL = SQL + ComNum.VBLF + "       t.Iamt5, t.Tamt5, t.Bamt5,                                             ";
                    SQL = SQL + ComNum.VBLF + "       Sunamek,SuHam,Unit,Hcode,TO_CHAR(DelDate,'YYYY-MM-DD') DelDay          ";
                    SQL = SQL + ComNum.VBLF + "  FROM BAS_SUT t, BAS_SUN n                                                   ";
                    SQL = SQL + ComNum.VBLF + " WHERE t.Sucode = '" + strSuCode.Trim() + "'                                 ";
                    SQL = SQL + ComNum.VBLF + "   AND T.SuNext = n.SuNext(+)                                                 ";

                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if(dt.Rows.Count > 0)
                    {
                        strNu = dt.Rows[0]["NU"].ToString().Trim();
                    }

                    dt.Dispose();
                    dt = null;
                }

                //외래 처방 수정 2012-11-12
                SQL = "";
                SQL = "UPDATE OPD_SLIP";
                SQL = SQL + ComNum.VBLF + " SET";
                SQL = SQL + ComNum.VBLF + "     GBSELF = '" + strF + "' , ";
                SQL = SQL + ComNum.VBLF + "     Nu ='" + strNu + "'  ";
                SQL = SQL + ComNum.VBLF + " WHERE ROWID = '" + strROWID + "' ";

                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                //원외약제 루틴
                SQL = "";
                SQL = "UPDATE ADMIN.OCS_OUTDRUG";
                SQL = SQL + ComNum.VBLF + " SET";
                SQL = SQL + ComNum.VBLF + "     GBSELF = '" + strF + "' ";
                SQL = SQL + ComNum.VBLF + " WHERE PANO = '" + txtPtNo.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "     AND SLIPDATE = TO_DATE('" + strArrDate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND SUCODE = '" + strSuCode + "' ";
                SQL = SQL + ComNum.VBLF + "     AND DEPTCODE = '" + strArrDept + "' ";
                
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);

                if(SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

            }
            catch(Exception ex)
            {
                if(dt != null)
                {
                    dt.Dispose();
                    dt = null;
                }

                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }
        }

        private void ssSlip_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 0)
            {
                clsPublic.GstrSunext_B = ssSlip_Sheet1.Cells[e.Row, 0].Text.Trim();
                
                //TODO : 약정보 불러오는 폼
                //FrmDrug.Show
            }
        }
    }
}
