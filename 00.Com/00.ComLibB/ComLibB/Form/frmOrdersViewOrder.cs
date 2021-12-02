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
    public partial class frmOrdersViewOrder : Form
    {
        /// <summary>
        /// Class Name      : ComLibB
        /// File Name       : frmOrdersViewOrder.cs
        /// Description     : 처방조회
        /// Author          : 이정현
        /// Create Date     : 2017-06
        /// <history>       
        /// D:\타병원\PSMHH\Ocs\OpdOcs\Oorder\frmOrdersViewOrder.frm => frmOrdersViewOrder.cs 으로 변경함
        /// </history>
        /// <seealso>
        /// D:\타병원\PSMHH\Ocs\OpdOcs\Oorder\frmOrdersViewOrder.frm
        /// </seealso>
        /// <vbp>
        /// default 		: D:\타병원\PSMHH\Ocs\OpdOcs\Oorder\mtsoorder.vbp
        /// seealso 		: 
        /// </vbp>
        /// </summary>
        private string GstrPtNo = "";
        private string strSelDept = "";

        public frmOrdersViewOrder()
        {
            InitializeComponent();
        }

        public frmOrdersViewOrder(string strPtNo)
        {
            InitializeComponent();

            GstrPtNo = strPtNo;
        }

        private void frmOrdersViewOrder_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            FormClear ();
            SetPtInfo();
            GetActivate();
        }

        private void FormClear()
        {
            chk0.Checked = false;
            chk1.Checked = false;
            chk2.Checked = false;
            chk3.Checked = false;
            chk4.Checked = false;
            chk5.Checked = false;

            dtpFrDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));

            ssList_Sheet1.RowCount = 0;
            ssIlls_Sheet1.RowCount = 0;
            ssOrder_Sheet1.RowCount = 0;
        }

        private void SetPtInfo()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            try
            {
                SQL = "";
                SQL = "SELECT TO_CHAR(InDate,  'YYYY-MM-DD') InDate, ";
                SQL = SQL + ComNum.VBLF + "       TO_CHAR(SysDate, 'YYYY-MM-DD') SysDate1  ";
                SQL = SQL + ComNum.VBLF + "  FROM KOSMOS_PMPA.IPD_NEW_MASTER ";
                SQL = SQL + ComNum.VBLF + " WHERE GBSTS ='0' ";
                SQL = SQL + ComNum.VBLF + "   AND Pano ='" + GstrPtNo + "' ";

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
                }
                else if (dt.Rows.Count == 1)
                {
                    dtpFrDate.Value = Convert.ToDateTime(dt.Rows[0]["INDATE"].ToString().Trim());
                    dtpToDate.Value = Convert.ToDateTime(dt.Rows[0]["SYSDATE1"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

                for(i = 9; i < ssIlls_Sheet1.ColumnCount; i++)
                {
                    ssIlls_Sheet1.Columns[i].Visible = false;
                }

                ssIlls_Sheet1.Columns[3].Visible = false;

                for(i = 10; i < ssOrder_Sheet1.ColumnCount; i++)
                {
                    if (i != 11 && i != 15 && i != 14)
                    {
                        ssOrder_Sheet1.Columns[i].Visible = false;
                    }
                }
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

        private void GetActivate()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            if (clsPublic.GstrJobMan == "간호사")
            {
                btnSend.Enabled = false;
            }
            else
            {
                btnSend.Enabled = true;
            }

            //TODO : GOrderFORM
            //Me.Caption = GOrderFORM.LabelTitle.Caption

            ssList_Sheet1.RowCount = 0;

            try
            {
                #region GoSub Read_Ipd_NEW_MASTER

                SQL = "";
                SQL = "SELECT TO_CHAR(InDate,   'YYYY-MM-DD') InDate1,  DeptCode, ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(ACTDate,   'YYYY-MM-DD') DcDate1,            ";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(A.OutDate,'YYYY-MM-DD') OutDate1, DrName    ";
                SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.IPD_NEW_MASTER A, KOSMOS_PMPA.BAS_DOCTOR B  ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano     = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.DrCode = B.DrCode(+)";
                SQL = SQL + ComNum.VBLF + "ORDER BY InDate DESC ";

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
                        ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                        ssList_Sheet1.SetRowHeight(ssList_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["INDATE1"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = ComFunc.LeftH(dt.Rows[i]["DRNAME"].ToString().Trim() + VB.Space(10), 10);
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = "입원";
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["OUTDATE1"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                #region GoSub Read_Opd_Slip

                SQL = "";
                SQL = "SELECT distinct TO_CHAR(BDate,'YYYY-MM-DD') BDate1, DeptCode, DrName ";
                SQL = SQL + ComNum.VBLF + " FROM OCS_OORDER A, KOSMOS_PMPA.BAS_DOCTOR B  ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno     = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbSunap  = '1' ";
                SQL = SQL + ComNum.VBLF + "  AND A.DrCode = B.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "ORDER BY 1 DESC ";

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
                        ssList_Sheet1.RowCount = ssList_Sheet1.RowCount + 1;
                        ssList_Sheet1.SetRowHeight(ssList_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 0].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 2].Text = ComFunc.LeftH(dt.Rows[i]["DRNAME"].ToString().Trim() + VB.Space(10), 10);
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 3].Text = "외래";
                        ssList_Sheet1.Cells[ssList_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion
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

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;

            ssIlls_Sheet1.RowCount = 0;
            ssOrder_Sheet1.RowCount = 0;

            if (ssList_Sheet1.RowCount == 0)
            {
                Read_Orders_IPD();
            }
            else
            {
                for(i = 0; i < ssList_Sheet1.RowCount; i++)
                {
                    if(ssList_Sheet1.IsBlockSelected == true)
                    {
                        strSelDept = ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 1].Text.Trim();

                        if(ssList_Sheet1.Cells[ssList_Sheet1.ActiveRowIndex, 3].Text.Trim() == "입원")
                        {
                            Read_Orders_IPD();
                        }
                        else
                        {
                            Read_Orders_OPD();
                        }
                    }
                }
            }
        }

        private string DrName_Get(string strDrCode)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            string rtnVal = "";

            try
            {
                SQL = "";
                SQL = "SELECT DrName FROM KOSMOS_OCS.OCS_DOCTOR ";
                SQL = SQL + ComNum.VBLF + " WHERE Sabun = '" + strDrCode.Trim() + "' ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if(SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return rtnVal;
                }
                if(dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return rtnVal;
                }

                rtnVal = dt.Rows[0]["DRNAME"].ToString().Trim();

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

        private string SlipNo_Gubun(string strSlipNo, string strDosCode, string strBun)
        {
            string rtnVal = "";

            switch(strSlipNo)
            {
                case "0003":
                case "0004":
                    rtnVal = "Med      999";
                    break;
                case "0005":
                    if (strDosCode == "97" || strDosCode == "99")
                    {
                        rtnVal = "Med      23";
                    }
                    else
                    {
                        rtnVal = "Med      24";
                    }
                    break;
                case "0010": case "0011": case "0012": case "0013": case "0014": case "0015":
                case "0016": case "0017": case "0018": case "0019": case "0020": case "0021":
                case "0022": case "0023": case "0024": case "0025": case "0026": case "0027":
                case "0028": case "0029": case "0030": case "0031": case "0032": case "0033":
                case "0034": case "0035": case "0036": case "0037": case "0038": case "0039":
                case "0040": case "0041": case "0042":
                    rtnVal = "Lab      17";
                    break;
                case "0060":                case "0061":                case "0062":                case "0063":                case "0064":                case "0065":
                case "0067":                case "0069":                case "0070":                case "0071":                case "0072":                case "0073":
                case "0074":                case "0075":                case "0076":                case "0077":                case "0078":                case "0079":
                case "0080":
                    rtnVal = "Xray     14";
                    break;
                case "0066":
                    rtnVal = "RI       15";
                    break;
                case "0068":
                    rtnVal = "Sono     16";
                    break;
                case "A1":
                    rtnVal = "V/S      11";
                    break;
                case "A2":
                    rtnVal = "S/O      12";
                    break;
                case "A4":
                    rtnVal = "S/O      13";
                    break;
                case "OR1":
                case "OR2":
                    rtnVal = "OR       22";
                    break;
                case "0044":
                    if (strBun.Trim() == "78")
                    {
                        rtnVal = "Bmd      19";
                    }
                    else
                    {
                        rtnVal = "Endo     18";
                    }
                    break;
                case "TEL":
                    rtnVal = "TEL      21";
                    break;
                case "0106":
                    rtnVal = "JAGA     25";
                    break;
                default:
                    rtnVal = "Etc      20";
                    break;
            }

            return rtnVal;
        }

        private void Read_Orders_OPD()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";
            int i = 0;

            double dblOrderNo = 0;

            try
            {

                #region GoSub Data_Read

                SQL = "";
                SQL = "SELECT A.*, A.ROWID, TO_CHAR(A.BDate,'YYYY-MM-DD') BDate1 FROM OCS_OORDER A ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno     = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbSunap  = '1'     ";
                SQL = SQL + ComNum.VBLF + "  AND DeptCode = '" + strSelDept.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "  AND A.BDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND A.BDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                if (chk5.Checked == false)
                {
                    if (chk0.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( Bun < '22' ) ";
                    }
                    if (chk1.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( Bun > '22' AND Bun < '65' ) ";
                    }
                    if (chk2.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( Bun > '64' ) ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Seqno ";

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
                        #region GoSub Move_Orders

                        ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;

                        if (i != 0)
                        {
                            if (dt.Rows[i]["BDATE1"].ToString().Trim() != dt.Rows[i - 1]["BDATE1"].ToString().Trim())
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                            }
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["RealQty"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["GbDiv"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["Nal"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 10].Text = DrName_Get(dt.Rows[i]["DrCode"].ToString().Trim());
                        //손동현 아래 한줄 추가
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 36].Text = DrName_Get(dt.Rows[i]["DrCode"].ToString().Trim());

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["GbER"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["GbSelf"].ToString().Trim();

                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = "#";
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 17].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 18].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = ComFunc.LeftH(SlipNo_Gubun(dt.Rows[i]["Slipno"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DosCode"].ToString().Trim(), 2), dt.Rows[i]["Bun"].ToString().Trim()), 7);
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = ComFunc.RightH(SlipNo_Gubun(dt.Rows[i]["Slipno"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DosCode"].ToString().Trim(), 2), dt.Rows[i]["Bun"].ToString().Trim()), 3).Trim();

                        switch(dt.Rows[i]["RealQty"].ToString().Trim())
                        {
                            case "1/2":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.5";
                                break;
                            case "1/3":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.33";
                                break;
                            case "2/3":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.66";
                                break;
                            case "1/4":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.25";
                                break;
                            case "3/4":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.75";
                                break;
                            case "1/5":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.2";
                                break;
                            case "2/5":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.4";
                                break;
                            case "3/5":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.6";
                                break;
                            case "4/5":
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.8";
                                break;
                            default:
                                if (VB.IsNumeric(dt.Rows[i]["RealQty"].ToString().Trim()))
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = VB.Val(dt.Rows[i]["RealQty"].ToString().Trim()).ToString();
                                }
                                else
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "1";
                                }
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.5";
                                break;
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = dt.Rows[i]["DosCode"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 21].Text = dt.Rows[i]["GbBoth"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 22].Text = dt.Rows[i]["Gbinfo"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 23].Text = dt.Rows[i]["Remark"].ToString().Trim();

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 31].Text = dt.Rows[i]["OrderNo"].ToString().Trim();
                        dblOrderNo = VB.Val(ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 31].Text);

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 32].Text = "";

                        #region GoSub Order_Read

                        SQL = "";
                        SQL = "SELECT    DispHeader cDispHeader, OrderName cOrderName,         ";
                        SQL = SQL + ComNum.VBLF + " DispRGB cDispRGB, GbBoth cGbBoth, GbInfo cGbInfo,     ";
                        SQL = SQL + ComNum.VBLF + " GbQty cGbQty, GbDosage cGbDosage, NextCode cNextCode, ";
                        SQL = SQL + ComNum.VBLF + " OrderNameS cOrderNameS, GbImiv cGbImiv ";
                        SQL = SQL + ComNum.VBLF + " FROM OCS_ORDERCODE ";
                        SQL = SQL + ComNum.VBLF + "WHERE OrderCode = '" + dt.Rows[i]["OrderCode"].ToString().Trim() + "' ";
                        SQL = SQL + ComNum.VBLF + "  AND Slipno    = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "' ";

                        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                        if(SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 문제가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }
                        if (dt1.Rows.Count == 0)
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "삭제된 코드입니다.. 변경요망";
                        }

                        #endregion

                        else if(dt1.Rows.Count > 0)
                        {
                            #region GoSub Order_Move

                            if(dt1.Rows[0]["cOrderNameS"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["cOrderName"].ToString().Trim() + " " + dt1.Rows[0]["cOrderNameS"].ToString().Trim();
                            }
                            else if(dt1.Rows[0]["cDispHeader"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["cDispHeader"].ToString().Trim() + " " + dt1.Rows[0]["cOrderName"].ToString().Trim();
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["cOrderName"].ToString().Trim();
                            }

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 24].Text = dt1.Rows[0]["cDispRGB"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 25].Text = dt1.Rows[0]["cGbBoth"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 26].Text = dt1.Rows[0]["cGbInfo"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 27].Text = dt1.Rows[0]["cGbQty"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 28].Text = dt1.Rows[0]["cGbDosage"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 29].Text = dt1.Rows[0]["cNextCode"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 30].Text = dt1.Rows[0]["cGbImiv"].ToString().Trim();

                            if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 11 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 20)
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 6].Text = dt1.Rows[0]["cNextCode"].ToString().Trim();
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 35].Text = dt1.Rows[0]["cNextCode"].ToString().Trim();
                            }

                            if (dt1.Rows[0]["cGbInfo"].ToString().Trim() == "1")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["Gbinfo"].ToString().Trim();
                            }
                            else if (dt1.Rows[0]["cGbDosage"].ToString().Trim() == "1")
                            {
                                #region GoSub Read_Dosage

                                SQL = "";
                                SQL = "SELECT DosName FROM OCS_ODOSAGE ";
                                SQL = SQL + ComNum.VBLF + "WHERE DosCode = '" + dt.Rows[i]["DOSCODE"].ToString().Trim() + "' ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if(SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if(dt2.Rows.Count == 0)
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = "";
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = "";
                                }
                                else
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["DOSNAME"].ToString().Trim();
                                }

                                dt2.Dispose();
                                dt2 = null;

                                #endregion
                            }
                            else
                            {
                                #region GoSub Read_Specman

                                SQL = "";
                                SQL = "SELECT Specname FROM OCS_OSPECIMAN ";
                                SQL = SQL + ComNum.VBLF + "WHERE SpecCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";
                                SQL = SQL + ComNum.VBLF + "  AND Slipno   = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "'  ";

                                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                                if(SqlErr != "")
                                {
                                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }
                                if(dt2.Rows.Count == 0)
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = "";
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = "";
                                }
                                else
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["Specname"].ToString().Trim();
                                }

                                dt2.Dispose();
                                dt2 = null;

                                #endregion
                            }

                            if (dt1.Rows[0]["cGbBoth"].ToString().Trim() == "1")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = ComFunc.LeftH(ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text, 30) + dt.Rows[i]["GbInfo"].ToString().Trim();
                            }
                            
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor
                                = ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["cDispRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));

                            //2012-11-26
                            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "★항혈전 " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].ForeColor = Color.FromArgb(255, 0, 255);
                            }

                            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "<!> " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            }

                            //손동현 추가 오더 화면에 오더코드 보이기
                            if (dt.Rows[i]["ORDERCODE"].ToString().Trim() != "")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = ComFunc.LeftH(dt.Rows[i]["ORDERCODE"].ToString().Trim() + VB.Space(8), 8) + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text.Trim();
                            }
                            
                            #endregion
                        }

                        dt1.Dispose();
                        dt1 = null;

                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = "";
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = VB.Val(dt.Rows[i]["DISPRGB"].ToString().Trim()).ToString("00000000");
                        }

                        //TODO : IORDER.BAS
                        //심사기준사항 disply
                        //GstrSimCode = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text;
                        //GstrSimFlag = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text;

                        //GstrSimYN = SimSaGiJun_Check(GstrSimFlag, GstrSimCode);
                        //ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text = GstrSimYN;

                        #endregion
                    }
                }

                dt.Dispose();
                dt = null;

                #endregion

                Read_Ills_OPD();
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

        private void Read_Ills_OPD()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            
            try
            {
                SQL = "";
                SQL = "SELECT A.*, TO_CHAR(A.BDate,'YYYY-MM-DD') BDate1, IllNameE ";
                SQL = SQL + ComNum.VBLF + " FROM OCS_OILLS A, KOSMOS_PMPA.BAS_ILLS B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.Ptno    = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDate    >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate    <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND A.IllCode = B.IllCode  ";
                SQL = SQL + ComNum.VBLF + "  AND B.IllCLASS = '1' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDate ";

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

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssIlls_Sheet1.RowCount = ssIlls_Sheet1.RowCount + 1;
                    ssIlls_Sheet1.SetRowHeight(ssIlls_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["IllNameE"].ToString().Trim();

                    if (dt.Rows[0]["DEPTCODE"].ToString().Trim() == "DT")
                    {
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["Boowi1"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["Boowi2"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["Boowi3"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["Boowi4"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["IllNameE"].ToString().Trim();
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

        private void Read_Orders_IPD()
        {
            string SQL = "";
            DataTable dt = null;
            DataTable dt1 = null;
            DataTable dt2 = null;
            string SqlErr = "";
            int i = 0;
            double dblOrderNo = 0;

            try
            {
                #region GoSub Data_Read

                SQL = "";
                SQL = "SELECT A.*, TO_CHAR(A.EntDate,'YYYY-MM-DD HH24:Mi') EntDate1, A.ROWID,  ";
                SQL = SQL + ComNum.VBLF + "  TO_CHAR(A.BDate,'YYYY-MM-DD') BDate1  ";
                SQL = SQL + ComNum.VBLF + " FROM OCS_IORDER A     ";
                SQL = SQL + ComNum.VBLF + "WHERE Ptno     = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND GbStatus IN (' ','D','D+')  ";
                SQL = SQL + ComNum.VBLF + "  AND BDate >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";

                //손동현
                //아래 두줄은 간호업무에서 DC한 자료를 읽지 안기 위해서 추가함
                SQL = SQL + ComNum.VBLF + "  AND OrderSite Not Like 'DC%' ";
                SQL = SQL + ComNum.VBLF + "  AND OrderSite <>  'CAN' ";

                if (chk5.Checked == false)
                {
                    if (chk0.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( Bun < '22' And Bun > '01' ) ";
                    }
                    if(chk1.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( Bun < '65' AND Bun > '21' ) ";
                    }
                    if(chk2.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( Bun > '64' ) ";
                    }
                    if(chk3.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( GbPRN = 'P' ) ";
                    }
                    if(chk4.Checked == false)
                    {
                        SQL = SQL + ComNum.VBLF + " AND NOT ( OrderCode = 'S/O' ) ";
                    }
                }

                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Seqno ";

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

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    #region GoSub Move_Orders

                    ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;
                    ssOrder_Sheet1.SetRowHeight(ssOrder_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    if (i != 0)
                    {
                        if (dt.Rows[i]["BDATE1"].ToString().Trim() != dt.Rows[i - 1]["BDATE1"].ToString().Trim())
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                    }

                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim();

                    if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 11 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 20 || VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) == 23)
                    {
                        if (dt.Rows[i]["CONTENTS"].ToString().Trim() == "0")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 6].Text = "";
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 6].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                        }

                        if(dt.Rows[i]["BCONTENTS"].ToString().Trim() == "0")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 34].Text = "";
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 34].Text = dt.Rows[i]["BCONTENTS"].ToString().Trim();
                        }
                    }

                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 7].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 8].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["Nal"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 10].Text = DrName_Get(dt.Rows[i]["DRCODE"].ToString().Trim()) + " " + ComFunc.RightH(dt.Rows[i]["ENTDATE1"].ToString().Trim(), 8);

                    //손동현 위를 아래로 고친다.
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 36].Text = DrName_Get(dt.Rows[i]["DRCODE"].ToString().Trim()) + " " + ComFunc.RightH(dt.Rows[i]["ENTDATE1"].ToString().Trim(), 8);

                    if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 16 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 21)
                    {
                        if (dt.Rows[i]["GBNGT"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                        }
                        if(dt.Rows[i]["GBGROUP"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                        }
                    }
                    else if (VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) >= 28 && VB.Val(dt.Rows[i]["BUN"].ToString().Trim()) <= 39)
                    {
                        //처치/재료는 GbNgt    나머지는 Group
                        if (VB.IsNumeric(dt.Rows[i]["GBGROUP"].ToString().Trim()) || dt.Rows[i]["GBGROUP"].ToString().Trim() == "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBNGT"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                        }
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();
                    }

                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["GBER"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["GBSELF"].ToString().Trim();

                    if (dt.Rows[i]["SLIPNO"].ToString().Trim() != "A1" && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A2" 
                        && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A3" && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A4")
                    {
                        if(dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = "#";
                        }
                        if(dt.Rows[i]["GBPRN"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = dt.Rows[i]["GBPRN"].ToString().Trim();
                        }
                        if(dt.Rows[i]["GBTFLAG"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = dt.Rows[i]["GBTFLAG"].ToString().Trim();
                        }

                        //손동현 추가
                        if(dt.Rows[i]["GBPRN"].ToString().Trim() == "A")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = "";
                        }

                        if (ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text.Trim() == "S")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text = "";
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 44].Text = "S";
                        }
                    }

                    if (dt.Rows[i]["GBPORT"].ToString().Trim() != "")
                    {
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 15].Text = dt.Rows[i]["GBPORT"].ToString().Trim();
                    }

                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 16].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 17].Text = dt.Rows[i]["BUN"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 18].Text = dt.Rows[i]["SLIPNO"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = ComFunc.LeftH(SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2), dt.Rows[i]["BUN"].ToString().Trim()), 7);
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = ComFunc.RightH(SlipNo_Gubun(dt.Rows[i]["SLIPNO"].ToString().Trim(), ComFunc.LeftH(dt.Rows[i]["DOSCODE"].ToString().Trim(), 2), dt.Rows[i]["BUN"].ToString().Trim()), 3).Trim();

                    switch(dt.Rows[i]["GBORDER"].ToString().Trim())
                    {
                        case "F":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "Pre";
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = "92";
                            break;
                        case "T":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "Post";
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = "93";
                            break;
                        case "M":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "Adm";
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 33].Text = "91";
                            break;
                    }

                    if (dt.Rows[i]["GBSTATUS"].ToString().Trim() == "D")
                    {
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 2].Text = "D/C";
                    }

                    switch(dt.Rows[i]["RealQty"].ToString().Trim())
                    {
                        case "1/2":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.5";
                            break;
                        case "1/3":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.33";
                            break;
                        case "2/3":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.66";
                            break;
                        case "1/4":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.25";
                            break;
                        case "3/4":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.75";
                            break;
                        case "1/5":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.2";
                            break;
                        case "2/5":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.4";
                            break;
                        case "3/5":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.6";
                            break;
                        case "4/5":
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.8";
                            break;
                        default:
                            if(VB.IsNumeric(dt.Rows[i]["RealQty"].ToString().Trim()))
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = VB.Val(dt.Rows[i]["RealQty"].ToString().Trim()).ToString();
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "1";
                            }
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 19].Text = "0.5";
                            break;
                    }

                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = dt.Rows[i]["DOSCODE"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 21].Text = dt.Rows[i]["GBBOTH"].ToString().Trim();
                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 22].Text = dt.Rows[i]["GBINFO"].ToString().Trim();

                    if (dt.Rows[i]["SLIPNO"].ToString().Trim() != "A1" && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A2"
                        && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A3" && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A4")
                    {
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 23].Text = "";
                    }
                    else
                    {
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 23].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                    }

                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 31].Text = dt.Rows[i]["ORDERNO"].ToString().Trim();
                    dblOrderNo = VB.Val(ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 31].Text);

                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 32].Text = "";

                    #region GoSub Order_Read

                    SQL = "";
                    SQL = "SELECT    DispHeader cDispHeader, OrderName cOrderName,         ";
                    SQL = SQL + ComNum.VBLF + " DispRGB cDispRGB, GbBoth cGbBoth, GbInfo cGbInfo,     ";
                    SQL = SQL + ComNum.VBLF + " GbQty cGbQty, GbDosage cGbDosage, NextCode cNextCode, ";
                    SQL = SQL + ComNum.VBLF + " OrderNameS cOrderNameS, GbImiv cGbImiv,DrugName cDrugName ";
                    SQL = SQL + ComNum.VBLF + " FROM OCS_ORDERCODE ";
                    SQL = SQL + ComNum.VBLF + "WHERE OrderCode = '" + dt.Rows[i]["ORDERCODE"].ToString().Trim() + "' ";
                    SQL = SQL + ComNum.VBLF + "  AND Slipno    = '" + dt.Rows[i]["SLIPNO"].ToString().Trim() + "' ";

                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                    if(SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 문제가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }

                    #endregion

                    if(dt1.Rows.Count > 0)
                    {
                        #region GoSub Order_Move

                        if(dt1.Rows[0]["cOrderNameS"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["cOrderName"].ToString().Trim() + " " + dt1.Rows[0]["cOrderNameS"].ToString().Trim();
                        }
                        else if (dt1.Rows[0]["cDispHeader"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["cDispHeader"].ToString().Trim() + " " + dt1.Rows[0]["cOrderName"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt1.Rows[0]["cOrderName"].ToString().Trim();
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 24].Text = dt1.Rows[0]["cDispRGB"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 25].Text = dt1.Rows[0]["cGbBoth"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 26].Text = dt1.Rows[0]["cGbInfo"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 27].Text = dt1.Rows[0]["cGbQty"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 28].Text = dt1.Rows[0]["cGbDosage"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 29].Text = dt1.Rows[0]["cNextCode"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 30].Text = dt1.Rows[0]["cGbImiv"].ToString().Trim();

                        if (dt1.Rows[0]["cGbInfo"].ToString().Trim() == "1")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt.Rows[i]["Gbinfo"].ToString().Trim();
                        }
                        else if (dt1.Rows[0]["cGbDosage"].ToString().Trim() == "1")
                        {
                            #region GoSub Read_Dosage

                            SQL = "";
                            SQL = "SELECT DosName FROM OCS_ODOSAGE ";
                            SQL = SQL + ComNum.VBLF + " WHERE DosCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if(SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if(dt2.Rows.Count == 0)
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = "";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = "";
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["DOSNAME"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;

                            #endregion
                        }
                        else
                        {
                            #region GoSub Read_Specman

                            SQL = "";
                            SQL = "SELECT Specname FROM OCS_OSPECIMAN ";
                            SQL = SQL + ComNum.VBLF + "WHERE SpecCode = '" + dt.Rows[i]["DosCode"].ToString().Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND Slipno   = '" + dt.Rows[i]["Slipno"].ToString().Trim() + "'  ";

                            SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                            if(SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if(dt2.Rows.Count == 0)
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = "";
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 20].Text = "";
                            }
                            else
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 5].Text = dt2.Rows[0]["Specname"].ToString().Trim();
                            }

                            dt2.Dispose();
                            dt2 = null;

                            #endregion
                        }

                        if (dt1.Rows[0]["cGbBoth"].ToString().Trim() == "1")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = ComFunc.LeftH(ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text, 30) + dt.Rows[i]["GbInfo"].ToString().Trim();
                        }

                        if(ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text.Trim() == "D/C")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt1.Rows[0]["cDispRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        }

                        //2012-11-26
                        if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "★항혈전 " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].ForeColor = Color.FromArgb(255, 0, 255);
                        }
                        if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "<!> " + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text;
                        }

                        //손동현 추가 오더 화면에 선수납관련이 보이게
                        if (dt.Rows[i]["GBPRN"].ToString().Trim() == "S")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "(A)" + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text.Trim();
                        }
                        else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "A")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "(선)" + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text.Trim();
                        }
                        else if (dt.Rows[i]["GBPRN"].ToString().Trim() == "B")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "(수)" + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text.Trim();
                        }

                        //손동현 추가 오더 화면에 오더코드 보이기
                        if (dt.Rows[i]["ORDERCODE"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = ComFunc.LeftH(dt.Rows[i]["ORDERCODE"].ToString().Trim() + VB.Space(8), 8) + ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text.Trim(); 
                        }

                        if (dt.Rows[i]["GBPRN"].ToString().Trim() == "P")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 64].Text = "#";
                        }

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 65].Text = dt.Rows[i]["PRN_REMARK"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 66].Text = dt.Rows[i]["PRN_INS_GBN"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 67].Text = dt.Rows[i]["PRN_UNIT"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 68].Text = dt.Rows[i]["PRN_INS_SDate"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 69].Text = dt.Rows[i]["PRN_INS_EDate"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 70].Text = dt.Rows[i]["PRN_INS_Max"].ToString().Trim();

                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 71].Text = dt.Rows[i]["PRN_DosCode"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 72].Text = dt.Rows[i]["PRN_Term"].ToString().Trim();
                        ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 73].Text = dt.Rows[i]["PRN_Notify"].ToString().Trim();

                        #endregion
                    }
                    else
                    {
                        if (dt.Rows[i]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A2"
                            || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A4")
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.Brown;
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 24].Text = "80";
                        }
                        else
                        {
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = "삭제된 코드입니다.. 변경요망";
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;

                    if (dt.Rows[i]["SLIPNO"].ToString().Trim() != "A1" && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A2"
                        && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A3" && dt.Rows[i]["SLIPNO"].ToString().Trim() != "A4")
                    {
                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            ssOrder_Sheet1.RowCount = ssOrder_Sheet1.RowCount + 1;
                            ssOrder_Sheet1.SetRowHeight(ssOrder_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 3].Text = "";
                            ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["REMARK"].ToString().Trim();

                            if (dt.Rows[i]["SLIPNO"].ToString().Trim() == "A1" || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A2"
                                || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A3" || dt.Rows[i]["SLIPNO"].ToString().Trim() == "A4")
                            {
                                ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.Brown;
                            }
                            else
                            {
                                if (ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1].Text.Trim() == "D/C")
                                {
                                    ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 1, ssOrder_Sheet1.RowCount - 1, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 0);
                                }
                            }
                        }
                    }

                    //심사기준사항 disply
                    //GstrSimCode = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 13].Text;
                    //GstrSimFlag = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text;

                    //GstrSimYN = SimSaGiJun_Check(GstrSimFlag, GstrSimCode);
                    //ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 45].Text = GstrSimYN;

                    #endregion
                }

                dt.Dispose();
                dt = null;

                #endregion

                Read_Ills_IPD();
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

        private void Read_Ills_IPD()
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;
            
            if (dtpToDate.Value < dtpFrDate.Value)
            {
                dtpToDate.Value = Convert.ToDateTime(ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-"));
            }

            try
            {
                SQL = "";
                SQL = "SELECT A.*, TO_CHAR(A.BDate,'YYYY-MM-DD') BDate1, IllNameE, ";
                SQL = SQL + ComNum.VBLF + "           TO_CHAR(A.RemoveDate,'YYYY-MM-DD') RemoveDate1, ";
                SQL = SQL + ComNum.VBLF + "           TO_CHAR(A.CfDate,'YYYY-MM-DD') CfDate1, ";
                SQL = SQL + ComNum.VBLF + "           TO_CHAR(A.EDate,'YYYY-MM-DD') EDate1    ";
                SQL = SQL + ComNum.VBLF + " FROM OCS_IILLS A, KOSMOS_PMPA.BAS_ILLS B ";
                SQL = SQL + ComNum.VBLF + "WHERE A.Ptno    = '" + GstrPtNo + "' ";
                SQL = SQL + ComNum.VBLF + "  AND BDate    >= TO_DATE('" + dtpFrDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND BDate    <= TO_DATE('" + dtpToDate.Value.ToString("yyyy-MM-dd") + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "  AND A.IllCode = B.IllCode  ";
                SQL = SQL + ComNum.VBLF + "  AND B.IllCLASS = '1' ";
                SQL = SQL + ComNum.VBLF + "ORDER BY BDate, Main  ";

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

                for(i = 0; i < dt.Rows.Count; i++)
                {
                    ssIlls_Sheet1.RowCount = ssIlls_Sheet1.RowCount + 1;
                    ssIlls_Sheet1.SetRowHeight(ssIlls_Sheet1.RowCount - 1, ComNum.SPDROWHT);

                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 1].Text = dt.Rows[i]["BDATE1"].ToString().Trim();
                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 3].Text = dt.Rows[i]["IllCode"].ToString().Trim();
                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 4].Text = dt.Rows[i]["IllNameE"].ToString().Trim();
                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 6].Text = ComFunc.MidH(dt.Rows[i]["RemoveDate1"].ToString().Trim(), 6, 5);
                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 7].Text = ComFunc.MidH(dt.Rows[i]["CfDate1"].ToString().Trim(), 6, 5);
                    ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 13].Text = dt.Rows[i]["EDate1"].ToString().Trim();

                    if(dt.Rows[0]["DEPTCODE"].ToString().Trim() == "DN" || dt.Rows[0]["DEPTCODE"].ToString().Trim() == "DT")
                    {
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 9].Text = dt.Rows[i]["Boowi1"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 10].Text = dt.Rows[i]["Boowi2"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 11].Text = dt.Rows[i]["Boowi3"].ToString().Trim();
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 12].Text = dt.Rows[i]["Boowi4"].ToString().Trim();
                    }
                    else
                    {
                        if(dt.Rows[i]["MAIN"].ToString().Trim() == "*")
                        {
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 5].BackColor = Color.FromArgb(0, 0, 255);
                        }

                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 2].Text = dt.Rows[i]["RO"].ToString().Trim();

                        if(ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 2].Text.Trim() != "")
                        {
                            ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 0, ssIlls_Sheet1.RowCount - 1, 7].BackColor = Color.FromArgb(255, 234, 234);
                        }
                    }

                    if (dt.Rows[i]["EDATE1"].ToString().Trim() != "")
                    {
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 0, ssIlls_Sheet1.RowCount - 1, 7].BackColor = Color.FromArgb(234, 234, 255);
                        ssIlls_Sheet1.Cells[ssIlls_Sheet1.RowCount - 1, 8].Text = ComFunc.MidH(dt.Rows[i]["EDATE1"].ToString().Trim(), 6, 5);
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

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            //TODO : GOrderFORM.SSOrder 변경해야됨
            FarPoint.Win.Spread.SheetView ssSpread_Sheet = null;

            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";

            int i = 0;
            int k = 0;
            string strSuCode = "";
            string strColor = "";

            try
            {
                for(i = 0; i < ssOrder_Sheet1.RowCount; i++)
                {
                    if(Convert.ToBoolean(ssOrder_Sheet1.Cells[i, 0].Value) == true)
                    {
                        strSuCode = ssOrder_Sheet1.Cells[i, 16].Text.Trim();

                        if(ssOrder_Sheet1.Cells[i, 3].Text.Trim() != "")
                        {
                            ssSpread_Sheet.RowCount = ssSpread_Sheet.RowCount + 1;
                            ssSpread_Sheet.SetRowHeight(ssSpread_Sheet.RowCount - 1, ComNum.SPDROWHT);

                            for(k = 4; k < 36; k++)
                            {
                                if(k < 10)
                                {
                                    ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, k - 2].Text = ssOrder_Sheet1.Cells[i, k].Text.Trim();

                                    if(k == 4)
                                    {
                                        if(clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, strSuCode) == "OK")
                                        {
                                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, k - 2].BackColor = Color.FromArgb(255, 0, 0);
                                        }
                                    }
                                }
                                else if(k == 10) { }
                                else if(k == 14)
                                {
                                    if(ssOrder_Sheet1.Cells[i, k].Text.Trim() != "#")
                                    {
                                        ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, k - 2].Text = ssOrder_Sheet1.Cells[i, k].Text.Trim();
                                    }
                                }
                                else if(k == 23) { }
                                else if(k < 33)
                                {
                                    ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, k - 2].Text = ssOrder_Sheet1.Cells[i, k].Text.Trim();
                                }
                                else
                                {
                                    ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, k + 2].Text = ssOrder_Sheet1.Cells[i, k].Text.Trim();
                                }
                            }

                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 1].Text = ssOrder_Sheet1.Cells[i, 2].Text.Trim();
                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 34].Text = ssOrder_Sheet1.Cells[i, 3].Text.Trim();

                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 29].Text = "";
                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 30].Text = "";
                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 32].Text = (ssSpread_Sheet.RowCount - 1).ToString();

                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 38].Text = ssOrder_Sheet1.Cells[i, 34].Text.Trim();

                            if(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 4].Text.Trim() == "0")
                            {
                                ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 4].Text = "";
                            }

                            strColor = ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 22].Text;

                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 36].Text = "";
                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 37].Text = "";
                            
                            ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 1, ssSpread_Sheet.RowCount - 1, ssSpread_Sheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(Convert.ToInt32(VB.Val(strColor)));

                            //손동현 선수납 항목 체크
                            //수가12
                            if(ComFunc.MidH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 9, 4) == "(수)")
                            {
                                ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text = ComFunc.LeftH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 8) + ComFunc.MidH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 9, 4).Replace("(수)", "") + ComFunc.RightH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 13);
                            }
                            else if(ComFunc.MidH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 9, 4) == "(선)")
                            {
                                ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text = ComFunc.LeftH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 8) + ComFunc.MidH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 9, 4).Replace("(선)", "") + ComFunc.RightH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 13);
                            }
                            else if(ComFunc.MidH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 9, 4) == "(A)")
                            {
                                ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text = ComFunc.LeftH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 8) + ComFunc.MidH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 9, 4).Replace("(A)", "") + ComFunc.RightH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text.Trim(), 12);
                            }

                            strSuCode = ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 16].Text.Trim();

                            SQL = "";
                            SQL = "SELECT    SuGbN ";
                            SQL = SQL + ComNum.VBLF + " FROM KOSMOS_PMPA.BAS_SUN  ";
                            SQL = SQL + ComNum.VBLF + "WHERE SuNext = '" + ssOrder_Sheet1.Cells[i, 16].Text.Trim() + "' ";
                            SQL = SQL + ComNum.VBLF + "  AND SuGbN  = '1' ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if(SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if(dt.Rows.Count == 0)
                            {
                                ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 45].Text = "";
                            }
                            else
                            {
                                ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 45].Text = "S";
                                ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 2].Text = ComFunc.LeftH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 45].Text.Trim(), 8) + "(A)" + ComFunc.RightH(ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 45].Text.Trim(), 9);
                            }

                            dt.Dispose();
                            dt = null;

                            //TODO : IORDER.BAS
                            //ssSpread_Sheet.Cells[ssSpread_Sheet.RowCount - 1, 53].Text = READ_MAYAK(strSuCode);

                            if(clsPublic.Gstr산제Chk != "OK")
                            {
                                //TODO : IORDER.BAS
                                //if(READ_POWDER(strSuCode) == "Y")
                                //{
                                //    GOrderFORM.SSOrder.Col = 9:
                                //   'CHECK 박스변경
                                //   GOrderFORM.SSOrder.CellType = SS_CELL_TYPE_CHECKBOX
                                //   GOrderFORM.SSOrder.TypeCheckText = ""
                                //   GOrderFORM.SSOrder.TypeCheckTextAlign = SS_CHECKBOX_TEXT_RIGHT
                                //   GOrderFORM.SSOrder.TypeHAlign = SS_CELL_H_ALIGN_CENTER
                                //   GOrderFORM.SSOrder.TypeVAlign = SS_CELL_V_ALIGN_VCENTER
                                //   GOrderFORM.SSOrder.TypeCheckType = SS_CHECKBOX_NORMAL
                                //   GOrderFORM.SSOrder.TypeCheckPicture(0) = LoadPicture("")
                                //   GOrderFORM.SSOrder.TypeCheckPicture(1) = LoadPicture("")
                                //   GOrderFORM.SSOrder.TypeCheckPicture(2) = LoadPicture("")
                                //   GOrderFORM.SSOrder.TypeCheckPicture(3) = LoadPicture("")
                                //   GOrderFORM.SSOrder.TypeCheckPicture(4) = LoadPicture("")
                                //   GOrderFORM.SSOrder.TypeCheckPicture(5) = LoadPicture("")
                                //}
                            }
                            else
                            {
                                if (clsPublic.Gstr파우더New_STS == "Y")
                                {
                                    clsPublic.Gstr파우더Gubun = "";

                                    //TODO : vb_POWDER.bas
                                    //if (READ_POWDER_SuCode_NEW(strSuCode.Trim()) == "OK")
                                    //{
                                    //    //TODO : IORDER.BAS
                                    //    if(GnReadOrder < i)
                                    //    {
                                    //        GOrderFORM.SSOrder.Col = 9:
                                    //        'CHECK 박스변경
                                    //        GOrderFORM.SSOrder.CellType = SS_CELL_TYPE_CHECKBOX
                                    //        GOrderFORM.SSOrder.TypeCheckText = ""
                                    //        GOrderFORM.SSOrder.TypeCheckTextAlign = SS_CHECKBOX_TEXT_RIGHT
                                    //        GOrderFORM.SSOrder.TypeHAlign = SS_CELL_H_ALIGN_CENTER
                                    //        GOrderFORM.SSOrder.TypeVAlign = SS_CELL_V_ALIGN_VCENTER
                                    //        GOrderFORM.SSOrder.TypeCheckType = SS_CHECKBOX_NORMAL
                                    //        GOrderFORM.SSOrder.Text = "1"
                                    //    }
                                    //}
                                }
                            }

                            //TODO : IORDER.BAS
                            //Verbal_Remark(GOrderFORM.SSOrder.DataRowCnt)

                            //심사기준사항 disply
                            //GstrSimCode = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 14].Text;
                            //GstrSimFlag = ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 46].Text;

                            //GstrSimYN = SimSaGiJun_Check(GstrSimFlag, GstrSimCode);
                            //ssOrder_Sheet1.Cells[ssOrder_Sheet1.RowCount - 1, 46].Text = GstrSimYN;
                        }
                    }
                }
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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if(ComQuery.IsJobAuth(this, "P", clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";

            if (ssOrder_Sheet1.RowCount == 0)
            {
                return;
            }

            if (ComFunc.MsgBoxQ("Order 내역을 출력하시겠습니까?", "확인", MessageBoxDefaultButton.Button1) == System.Windows.Forms.DialogResult.No)
            {
                return;
            }

            ssOrder_Sheet1.Columns[0].Visible = false;

            Application.DoEvents();

            strFont1 = "/fn\"바탕체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"바탕체\" /fz\"11.25\" /fb0 /fi0 /fu0 /fk0 /fs2";

            strHead1 = "/c/f1" + "처방 내역 조회" + "/f1/n";
            strHead2 = "/c/f2" + "출력시간 : " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D", "-") + " " + ComFunc.FormatStrToDateEx(ComQuery.CurrentDateTime(clsDB.DbCon, "T"), "T", ":") + "/f2/n";

            ssOrder_Sheet1.PrintInfo.AbortMessage = "프린터 중입니다.";
            ssOrder_Sheet1.PrintInfo.Orientation = FarPoint.Win.Spread.PrintOrientation.Landscape;
            ssOrder_Sheet1.PrintInfo.Header = strFont1 + strHead1 + strFont2 + strHead2;
            ssOrder_Sheet1.PrintInfo.Margin.Top = 20;
            ssOrder_Sheet1.PrintInfo.Margin.Bottom = 20;
            ssOrder_Sheet1.PrintInfo.ShowColor = false;
            ssOrder_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Hide;
            ssOrder_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssOrder_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssOrder_Sheet1.PrintInfo.ShowBorder = true;
            ssOrder_Sheet1.PrintInfo.ShowGrid = false;
            ssOrder_Sheet1.PrintInfo.ShowColor = false;
            ssOrder_Sheet1.PrintInfo.ShowShadows = false;
            ssOrder_Sheet1.PrintInfo.UseMax = true;
            ssOrder_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssOrder_Sheet1.PrintInfo.UseSmartPrint = false;
            ssOrder_Sheet1.PrintInfo.ShowPrintDialog = false;
            ssOrder_Sheet1.PrintInfo.Preview = false;
            ssOrder.PrintSheet(0);


            ssOrder_Sheet1.Columns[0].Visible = true;
            
            Application.DoEvents();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ssList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(e.ColumnHeader || e.RowHeader)
            {
                return;
            }

            if (dtpFrDate.Value > Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 0].Text))
            {
                dtpFrDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 0].Text);
            }

            if (ssList_Sheet1.Cells[e.Row, 4].Text == "1990-01-01")
            {
                if(dtpFrDate.Value > Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 0].Text))
                {
                    dtpToDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 0].Text);
                }
            }
            else
            {
                if(dtpToDate.Value < Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 0].Text))
                {
                    dtpToDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 0].Text);
                }
            }

            strSelDept = ssList_Sheet1.Cells[e.Row, 1].Text.Trim();
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssIlls_Sheet1.RowCount = 0;
            ssOrder_Sheet1.RowCount = 0;

            if (e.ColumnHeader || e.RowHeader)
            {
                return;
            }

            dtpFrDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 0].Text.Trim());
            dtpToDate.Value = Convert.ToDateTime(ssList_Sheet1.Cells[e.Row, 4].Text.Trim());

            if (ssList_Sheet1.Cells[e.Row, 3].Text.Trim() == "입원")
            {
                Read_Orders_IPD();
            }
            else
            {
                Read_Orders_OPD();
            }
        }

        private void ssOrder_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string SQL = "";
            DataTable dt = null;
            string SqlErr = "";
            int i = 0;

            string strGubun = "";
            string strDataColor = "";

            if(e.Column == 0)
            {
                strGubun = ssOrder_Sheet1.Cells[e.Row, 2].Text.Trim();
                strDataColor = ssOrder_Sheet1.Cells[e.Row, 21].Text.Trim();    //원래의 Color

                if(Convert.ToBoolean(ssOrder_Sheet1.Cells[e.Row, 0].Value) == true)
                {
                    //2012-12-06
                    if(ssOrder_Sheet1.Cells[e.Row, 3].Text.Trim() == "V001" || ssOrder_Sheet1.Cells[e.Row, 3].Text.Trim() == "S/O")
                    {
                    }
                    else
                    {
                        try
                        {
                            SQL = "";
                            SQL = "SELECT * FROM OCS_ORDERCODE ";
                            SQL = SQL + ComNum.VBLF + "     WHERE OrderCode = '" + ssOrder_Sheet1.Cells[e.Row, 3].Text.Trim() + "' ";

                            SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                            if(SqlErr != "")
                            {
                                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }
                            if(dt.Rows.Count == 0)
                            {
                                if (strGubun == "S/O" || strGubun == "S/O")
                                {
                                }
                                else
                                {
                                    ComFunc.MsgBox("삭제된 수가입니다. 처방을 다시 선택 하세요.", "삭제코드");
                                    ssOrder_Sheet1.Cells[e.Row, 0].Value = false;
                                }
                            }
                            else
                            {
                                if (dt.Rows[0]["SENDDEPT"].ToString().Trim() == "N")
                                {
                                    ComFunc.MsgBox("삭제된 수가입니다. 처방을 다시 선택 하세요.", "삭제코드");
                                    ssOrder_Sheet1.Cells[e.Row, 0].Value = false;
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

                    ssOrder_Sheet1.Cells[e.Row, 1, e.Row, ssOrder_Sheet1.ColumnCount - 1].ForeColor = Color.FromArgb(128, 0, 255);
                }
                else
                {
                    ssOrder_Sheet1.Cells[e.Row, 1, e.Row, ssOrder_Sheet1.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(strDataColor, System.Globalization.NumberStyles.AllowHexSpecifier));
                }
            }
            else
            {
                for(i = 0; i < ssOrder_Sheet1.RowCount; i++)
                {
                    if (ssOrder_Sheet1.Cells[i, 1].BackColor == Color.FromArgb(255, 255, 128) && e.Row != i)
                    {
                        ssOrder_Sheet1.Cells[i, 1, i, ssOrder_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
                    }
                }

                if (e.Row <= ssOrder_Sheet1.RowCount)
                {
                    ssOrder_Sheet1.Cells[e.Row, 1, e.Row, ssOrder_Sheet1.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);
                }
            }
        }
    }
}
