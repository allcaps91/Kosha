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
    /// <summary>
    /// Class Name      : ComLibB
    /// File Name       : frmSlipView3.cs
    /// Description     : 외래 진료내역 조회(청구)하는 폼
    /// Author          : 안정수
    /// Create Date     : 2017-06
    /// Update History  : try-catch문 수정
    /// <history>       
    /// D:\타병원\PSMHH\mir\OVIEWA09.FRM(FrmSlipView) => frmSlipView3.cs 으로 변경함
    /// 원래폼과 디자인 다름
    /// </history>
    /// <seealso>
    /// D:\타병원\PSMHH\mir\OVIEWA09.FRM(FrmSlipView)
    /// </seealso>
    /// <vbp>
    /// default 		: C:\Users\user1.2015-2552\Desktop\포항성모병원 VB Source(2017.06.01)\mir -> vbp 따로 존재 x
    /// seealso 		: 
    /// </vbp>
    /// </summary>
    public partial class frmSlipView3 : Form
    {
        string strFlagChange = "";

        string strArrDate = "";
        string strArrDept = "";
        string strArrBi = "";
        public frmSlipView3()
        {
            InitializeComponent();
        }

        void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void frmSlipView3_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); return; } //폼 권한 조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            SetInit();
        }

        void SetInit()
        {
            txtName.Text = "";
            txtSex.Text = "";
            txtJumin.Text = "";
            txtFDate.Text = "";
            txtTDate.Text = "";

            ss_Clear(ssList1_Sheet1);
            ss_Clear(ssList2_Sheet1);

            ssList1.Enabled = true;
            ssList2.Enabled = true;
        }

        void ss_Clear(FarPoint.Win.Spread.SheetView Spd)
        {
            for(int i = 0; i < Spd.RowCount; i++)
            {
                for(int j = 0; j < Spd.ColumnCount; j++)
                {
                    Spd.Cells[i, j].Text = "";
                }
            }
        }

        void txtPano_Leave(object sender, EventArgs e)
        {
            if(strFlagChange == "**")
            {
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
                if (VB.IsNumeric(txtPano.Text))
                {
                    Read_PAT_MAST();
                    Read_OS_JINRYO();
                }
            }
        }

        void txtPano_TextChanged(object sender, EventArgs e)
        {            
            strFlagChange = "**";

            txtName.Text = "";
            txtSex.Text = "";
            txtJumin.Text = "";
            txtFDate.Text = "";
            txtTDate.Text = "";

            ss_Clear(ssList1_Sheet1);
            ss_Clear(ssList2_Sheet1);

            ssList1.Enabled = true;
            ssList2.Enabled = true;

        }

        void Read_PAT_MAST()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;            

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(StartDate, 'YYYY-MM-DD') Sdate,";
                SQL = SQL + ComNum.VBLF + "    TO_CHAR(LastDate, 'YYYY-MM-DD') Ldate, ";
                SQL = SQL + ComNum.VBLF + "    Sname, Sex, Jumin1, Jumin2 ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "BAS_PATIENT";
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + txtPano.Text.Trim() + "' ";
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

                if (dt.Rows.Count > 0)
                {
                    txtName.Text = dt.Rows[0]["Sname"].ToString().Trim();
                    txtSex.Text = dt.Rows[0]["Sex"].ToString().Trim();
                    txtJumin.Text = dt.Rows[0]["Jumin1"].ToString().Trim() + "-" + dt.Rows[0]["Jumin2"].ToString().Trim();
                    txtFDate.Text = dt.Rows[0]["Sdate"].ToString().Trim();
                    txtTDate.Text = dt.Rows[0]["Ldate"].ToString().Trim();
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }

        }

        void Read_OS_JINRYO()
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            string strShow = "";
            string strDoctor = "";
            string strbi = "";

            ssList1_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT TO_CHAR(ActDate,'YYYY-MM-DD') Adate, ";
                SQL = SQL + ComNum.VBLF + "      DeptCode, DrName, Bi ";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP O, " + ComNum.DB_PMPA + "BAS_DOCTOR B ";
                SQL = SQL + ComNum.VBLF + "WHERE Pano = '" + txtPano.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "     AND O.DrCode = B.DrCode(+) ";
                SQL = SQL + ComNum.VBLF + "GROUP BY ActDate, DeptCode, DrName, Bi";
                SQL = SQL + ComNum.VBLF + "ORDER BY ActDate Desc";
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

                ssList1_Sheet1.RowCount = dt.Rows.Count;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < ssList1_Sheet1.RowCount; i++)
                    {
                        strArrDate = dt.Rows[i]["Adate"].ToString().Trim();
                        strArrDept = dt.Rows[i]["DeptCode"].ToString().Trim();
                        strArrBi = dt.Rows[i]["Bi"].ToString().Trim();
                        strDoctor = dt.Rows[i]["DrName"].ToString().Trim();

                        switch (strArrBi)
                        {
                            case "11":
                                strbi = "보험";
                                break;
                            case "12":
                                strbi = "보험";
                                break;
                            case "13":
                                strbi = "보험";
                                break;
                            case "14":
                                strbi = "보험";
                                break;

                            case "21":
                                strbi = "보호";
                                break;
                            case "22":
                                strbi = "보호";
                                break;
                            case "23":
                                strbi = "보호";
                                break;
                            case "241":
                                strbi = "보호";
                                break;
                            case "25":
                                strbi = "보호";
                                break;

                            case "31":
                                strbi = "산재";
                                break;
                            case "32":
                                strbi = "산재";
                                break;
                            case "33":
                                strbi = "산재공상";
                                break;

                            case "41":
                                strbi = "보험180";
                                break;
                            case "42":
                                strbi = "보험180";
                                break;
                            case "43":
                                strbi = "보험180";
                                break;
                            case "44":
                                strbi = "보험180";
                                break;
                            case "45":
                                strbi = "보험180";
                                break;

                            case "52":
                                strbi = "TA보험";
                                break;

                            default:
                                strbi = "일반";
                                break;
                        }

                        ssList1_Sheet1.Cells[i, 0].Text = strArrDate;
                        ssList1_Sheet1.Cells[i, 1].Text = strArrDept;
                        ssList1_Sheet1.Cells[i, 2].Text = strArrBi;
                        ssList1_Sheet1.Cells[i, 3].Text = strDoctor;
                        ssList1_Sheet1.Cells[i, 4].Text = strbi;
                    }
                }

                dt.Dispose();
                dt = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }           
        }

        void ssList1_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if(ssList1_Sheet1.RowCount > -1)
            {
                Read_OPD_SLIP(ssList1_Sheet1.RowCount);
            }
        }

        void Read_OPD_SLIP(int ArgIndex)
        {
            string strSunameK = "";
            string strAdate = "";
            string strBaseAmt = "";
            string strQty = "";
            string strNal = "";
            string strAmt1 = "";
            string strAmt2 = "";
            string strSeqNo = "";
            int i, j;

            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            strAdate = strArrDate;

            ssList2_Sheet1.RowCount = 0;

            try
            {
                SQL = "";
                SQL = SQL + ComNum.VBLF + "SELECT";
                SQL = SQL + ComNum.VBLF + "    Sucode, SunameK, BaseAmt, Qty, Nal,";
                SQL = SQL + ComNum.VBLF + "    GbSpc, GbNgt, GbGisul, GbSelf, GbChild,";
                SQL = SQL + ComNum.VBLF + "    Amt1, Amt2, SeqNo, Part";
                SQL = SQL + ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_SLIP O, " + ComNum.DB_PMPA + "BAS_SUN B";
                SQL = SQL + ComNum.VBLF + "WHERE 1=1";
                SQL = SQL + ComNum.VBLF + "     AND ActDate  = TO_DATE('" + strAdate + "','YYYY-MM-DD') ";
                SQL = SQL + ComNum.VBLF + "     AND  Pano     = '" + txtPano.Text.Trim() + "' ";
                SQL = SQL + ComNum.VBLF + "     AND  Bi       = '" + strArrBi + "' ";
                SQL = SQL + ComNum.VBLF + "     AND  DeptCode = '" + strArrDept + "' ";
                SQL = SQL + ComNum.VBLF + "     AND  O.Sunext = B.Sunext ";
                SQL = SQL + ComNum.VBLF + "ORDER  BY  SeqNo, O.Bun, O.Sucode, O.Sunext";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssList2_Sheet1.RowCount = dt.Rows.Count;

                for (i = 0; i < ssList2_Sheet1.RowCount; i++)
                {
                    strSunameK = dt.Rows[i]["SunameK"].ToString().Trim();
                    strQty = String.Format("{0:#,##0}", dt.Rows[i]["Qty"]);
                    strNal = String.Format("{0:#,##0}", dt.Rows[i]["Nal"]);
                    strAmt1 = String.Format("{0:#,##0}", dt.Rows[i]["Amt1"]);
                    strAmt2 = String.Format("{0:#,##0}", dt.Rows[i]["Amt2"]);
                    strBaseAmt = String.Format("{0:#,##0}", dt.Rows[i]["BaseAmt"]);
                    strSeqNo = String.Format("{0:#,##0}", dt.Rows[i]["SeqNo"]);

                    ssList2_Sheet1.Cells[i, 0].Text = strSunameK;
                    ssList2_Sheet1.Cells[i, 1].Text = strQty;
                    ssList2_Sheet1.Cells[i, 2].Text = strNal;
                    ssList2_Sheet1.Cells[i, 3].Text = strAmt1;
                    ssList2_Sheet1.Cells[i, 4].Text = strAmt2;
                    ssList2_Sheet1.Cells[i, 5].Text = strBaseAmt;
                    ssList2_Sheet1.Cells[i, 6].Text = strSeqNo;
                    ssList2_Sheet1.Cells[i, 7].Text = dt.Rows[i]["Sucode"].ToString().Trim();

                    //ssList2_Sheet1.Cells[i, 8].Text = ssList2_Sheet1.Cells[i, 8].Text + VB.Space(1) + strSunameK;
                    //ssList2_Sheet1.Cells[i, 9].Text = ssList2_Sheet1.Cells[i, 8].Text + VB.Space(10 - VB.Len(strBaseAmt)) + strBaseAmt;
                    //ssList2_Sheet1.Cells[i, 10].Text = ssList2_Sheet1.Cells[i, 9].Text + VB.Space(6 - VB.Len(strQty)) + strQty;
                    //ssList2_Sheet1.Cells[i, 11].Text = ssList2_Sheet1.Cells[i, 10].Text + VB.Space(6 - VB.Len(strNal)) + strNal;
                    //ssList2_Sheet1.Cells[i, 12].Text = ssList2_Sheet1.Cells[i, 11].Text + VB.Space(2) + dt.Rows[i]["GbSpc"].ToString().Trim();
                    //ssList2_Sheet1.Cells[i, 13].Text = ssList2_Sheet1.Cells[i, 12].Text + dt.Rows[i]["GbNgt"].ToString().Trim();
                    //ssList2_Sheet1.Cells[i, 14].Text = ssList2_Sheet1.Cells[i, 13].Text + dt.Rows[i]["GbGisul"].ToString().Trim();
                    //ssList2_Sheet1.Cells[i, 15].Text = ssList2_Sheet1.Cells[i, 14].Text + dt.Rows[i]["GbSelf"].ToString().Trim();
                    //ssList2_Sheet1.Cells[i, 16].Text = ssList2_Sheet1.Cells[i, 15].Text + dt.Rows[i]["GbChild"].ToString().Trim();
                    //ssList2_Sheet1.Cells[i, 17].Text = ssList2_Sheet1.Cells[i, 16].Text + VB.Space(10 - VB.Len(strAmt1)) + strAmt1;
                    //ssList2_Sheet1.Cells[i, 18].Text = ssList2_Sheet1.Cells[i, 17].Text + VB.Space(8 - VB.Len(strAmt2)) + strAmt2;
                    //ssList2_Sheet1.Cells[i, 19].Text = ssList2_Sheet1.Cells[i, 18].Text + VB.Space(5 - VB.Len(strSeqNo)) + strSeqNo;
                    //ssList2_Sheet1.Cells[i, 20].Text = ssList2_Sheet1.Cells[i, 19].Text + VB.Space(2) + dt.Rows[i]["Part"].ToString().Trim();

                }

                if (dt.Rows.Count > 0)
                {
                    ssList2.Enabled = true;
                }

            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
            }
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == 13)
            {
                txtName.Focus();
            }
        }
    }
}
