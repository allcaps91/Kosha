using ComBase;
using ComLibB;
using FarPoint.Win;
using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMedLibB
{
    /// <summary>
    /// File Name       : FrmMedLastOrdView.cs
    /// Description     : 환자 진료내역 조회
    /// Author          : 이상훈
    /// Create Date     : 2017-09
    /// <history>       
    /// </history>
    /// <seealso>
    /// FrmOrdersViewOrder.frm, FrmOrdersViewOrder2.frm, FrmOrdersViewOrder3.frm
    /// </seealso>
    /// </summary>
    public partial class FrmMedLastOrdView : Form
    {
        string FstrPano;
        string FstrBDate;
        string FstrDeptCode;
        string FstrBi;
        string strCallLvl;
        int FnStartRow;
        int nSunapOrdCnt = 0;

        string SQL;
        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = "";     //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수
        
        clsOrdFunction OF = new clsOrdFunction();
        clsSpread SP = new clsSpread();

        public delegate void LastOrder_Click(string strRowId, string strGBIO, int startRow);
        public static event LastOrder_Click LastOrderClick;

        public delegate void Lastills_Click(string sillCode, string sillName, string strRO, string strVCode);
        public static event Lastills_Click LastillsClick;

        public FrmMedLastOrdView(string sPano, string sBDate, string sDeptCode, string sBi, string sCallLvl, int StartRow)
        {
            InitializeComponent();

            FstrPano = sPano;
            FstrBDate = sBDate;
            FstrDeptCode = sDeptCode;
            FstrBi = sBi;
            FnStartRow = StartRow;
        }

        private void FrmMedLastOrdView_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            this.Location = new Point(1, 100);

            //2021-01-07 용량칼럼 추가로 기존 칼럼값 + 1 
            //ssOrder_Sheet1.Columns.Get(8).Visible = false;
            //ssOrder_Sheet1.Columns.Get(11).Visible = false;
            //ssOrder_Sheet1.Columns.Get(12).Visible = false;
            //ssOrder_Sheet1.Columns.Get(13).Visible = false;
            ssOrder_Sheet1.Columns.Get(9).Visible = false;
            ssOrder_Sheet1.Columns.Get(12).Visible = false;
            ssOrder_Sheet1.Columns.Get(13).Visible = false;
            ssOrder_Sheet1.Columns.Get(14).Visible = false;

            //2021-01-07 추가
            if (clsType.User.IdNumber == "53775")
            {
                ssOrder_Sheet1.Columns.Get(3).Visible = true;
                ssOrder_Sheet1.Columns.Get(18).Visible = true;
                ssOrder_Sheet1.Columns.Get(19).Visible = true;
                ssOrder_Sheet1.Columns.Get(20).Visible = true;
                ssOrder_Sheet1.Columns.Get(21).Visible = true;
            }
            else
            {
                ssOrder_Sheet1.Columns.Get(3).Visible = false;
                ssOrder_Sheet1.Columns.Get(18).Visible = false;
                ssOrder_Sheet1.Columns.Get(19).Visible = false;
                ssOrder_Sheet1.Columns.Get(20).Visible = false;
                ssOrder_Sheet1.Columns.Get(21).Visible = false;
            }
            

            if (strCallLvl == "ConsultView")
            {
                btnOrderSend.Visible = false;
                btnRegist.Visible = true;
            }
            else
            {
                btnOrderSend.Visible = true;
                btnRegist.Visible = false;
            }

            // 외래간호사
            if (clsType.User.strNurseUse == "Y")
            {
                btnOrderSend.Visible = false;
                btnRegist.Visible = true;
            }

            txtPano.Text = FstrPano;
            lblSName.Text = clsOrdFunction.Pat.sName;
            lblBiName.Text = clsOrdFunction.Pat.Bi;

            getData();

            fn_ReadOrder();

            //2021-01-07 용량칼럼 추가로 기존 칼럼값 + 1 
            //lblRemark.Text = ssOrder.ActiveSheet.Cells[0, 7].Text;
            lblRemark.Text = ssOrder.ActiveSheet.Cells[0, 8].Text;

            fn_Order_Read_List();
        }

        void fn_ReadOrder()
        {
            string strUnit = "";

            try
            {
                SQL = "";
                SQL += " SELECT OrderName,   DispHeader, C.DosName, A.RealQty,A.DRCODE                          \r";
                SQL += "      , A.Nal,       A.GbDiv,    A.Remark,  B.DispRGB                                   \r";
                SQL += "      , A.OrderCode, A.Seqno,    OrderNameS,A.GbInfo                                    \r";
                SQL += "      , B.GbBoth,    B.Slipno,   A.DosCode, A.GbBoth JUSA, A.ROWID,A.Slipno,A.SUCODE    \r";
                //2021-01-11 안정수 추가
                SQL += "      , A.CONTENTS, A.TUYEOPOINT, A.TUYEOTIME                                           \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER    A                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ODOSAGE   C                                                      \r";
                SQL += "  WHERE Ptno        = '" + txtPano.Text + "'                                            \r";
                SQL += "    AND BDate       = TO_DATE('" + FstrBDate + "','YYYY-MM-DD')                         \r";
                SQL += "    AND A.Seqno     >  0                                                                \r";
                SQL += "    AND A.OrderCode = B.OrderCode(+)                                                    \r";
                SQL += "    AND A.Slipno    = B.Slipno(+)                                                       \r";
                SQL += "    AND A.DosCode   = C.DosCode(+)                                                      \r";
                SQL += "  ORDER BY DECODE(RTRIM(a.ORDERCODE) ,'S/O','000000',a.Slipno) , a.Seqno                \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssOrder.ActiveSheet.RowCount = dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //if (dt.Rows[i]["SLIPNO"].ToString() >= "A1" && dt.Rows[i]["SLIPNO"].ToString() <= "A4")
                        if (dt.Rows[i]["SLIPNO"].ToString() == "A1" ||
                            dt.Rows[i]["SLIPNO"].ToString() == "A2" ||
                            dt.Rows[i]["SLIPNO"].ToString() == "A3" ||
                            dt.Rows[i]["SLIPNO"].ToString() == "A4")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                            //2021-01-07 용량칼럼 추가로 기존 칼럼값 + 1 
                            //if (ssOrder.ActiveSheet.Cells[i, 7].Text == "#")
                            //{
                            //    ssOrder.ActiveSheet.Cells[i, 7].Text = "";
                            //}
                            if (ssOrder.ActiveSheet.Cells[i, 8].Text == "#")
                            {
                                ssOrder.ActiveSheet.Cells[i, 8].Text = "";
                            }
                        }
                        else
                        {
                            if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                            {
                                strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                ssOrder.ActiveSheet.Cells[i, 1].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                            }
                            else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }

                            if (dt.Rows[i]["GBBOTH"].ToString().Trim() == "1" && (dt.Rows[i]["GBINFO"].ToString().Trim() != ""))
                            {
                                //ssOrder.ActiveSheet.Cells[i, 1].Text = VB.Left(ssOrder.ActiveSheet.Cells[i, 1].Text, 30) + dt.Rows[i]["GBINFO"].ToString().Trim();
                                ssOrder.ActiveSheet.Cells[i, 1].Text = VB.Left(ssOrder.ActiveSheet.Cells[i, 1].Text, 31) + dt.Rows[i]["GBINFO"].ToString().Trim();
                            }

                            if (dt.Rows[i]["DOSNAME"].ToString().Trim() != "")
                            {
                                ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DOSNAME"].ToString().Trim();
                            }
                            else
                            {
                                if (dt.Rows[i]["DOSCODE"].ToString().Trim() != "")
                                {
                                    SQL = "";
                                    SQL += " SELECT SpecName                                                    \r";
                                    SQL += "   FROM KOSMOS_OCS.OCS_OSPECIMAN                                    \r";
                                    SQL += "  WHERE Slipno   = '" + dt.Rows[i]["SLIPNO"].ToString() + "'        \r";
                                    SQL += "    AND SpecCode = '" + dt.Rows[i]["DOSCODE"].ToString() + "'       \r";
                                    SQL += "    AND ROWNUM   = 1                                                \r";
                                    SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                                    if (SqlErr != "")
                                    {
                                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                        return;
                                    }

                                    if (dt1.Rows.Count == 1)
                                    {
                                        ssOrder.ActiveSheet.Cells[0, 2].Text = dt1.Rows[0]["SPECNAME"].ToString();
                                    }
                                    else
                                    {
                                        ssOrder.ActiveSheet.Cells[0, 2].Text = dt.Rows[i]["GBINFO"].ToString();
                                    }

                                    dt1.Dispose();
                                    dt1 = null;
                                }
                            }

                            //ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                            //ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["NAL"].ToString().Trim();

                            if(clsType.User.IdNumber == "53775")
                            {
                                ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                                ssOrder.ActiveSheet.Cells[i, 20].Text = dt.Rows[i]["TUYEOPOINT"].ToString().Trim();
                                ssOrder.ActiveSheet.Cells[i, 21].Text = dt.Rows[i]["TUYEOTIME"].ToString().Trim();
                            }

                            ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                            ssOrder.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["NAL"].ToString().Trim();
                            if (dt.Rows[i]["GBDIV"].ToString() != "0")
                            {
                                //ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["GBDIV"].ToString();
                                ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["GBDIV"].ToString();
                            }
                            if (VB.Mid(dt.Rows[i]["DOSCODE"].ToString(), 5, 2) == "01")
                            {
                                if (dt.Rows[i]["JUSA"].ToString() == "3")
                                {
                                    //ssOrder.ActiveSheet.Cells[i, 6].Text = "완료";
                                    ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                                }
                            }
                            if (VB.Mid(dt.Rows[i]["DOSCODE"].ToString(), 5, 2) == "02")
                            {
                                if (dt.Rows[i]["JUSA"].ToString() == "1")
                                {
                                    //ssOrder.ActiveSheet.Cells[i, 6].Text = "완료";
                                    ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                                }
                            }
                            //ssOrder.ActiveSheet.Cells[i, 7].Text = dt.Rows[i]["REMARK"].ToString();
                            //ssOrder.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString();
                            ssOrder.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["REMARK"].ToString();
                            ssOrder.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString();

                            if (dt.Rows[i]["DISPRGB"].ToString() != "")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["DISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                            }

                            //ssOrder.ActiveSheet.Cells[i, 10].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString());
                            ssOrder.ActiveSheet.Cells[i, 11].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString());

                            if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = "★항혈전 " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                                ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 255);
                            }

                            if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = "< !> " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                            }

                            if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString(), FstrDeptCode) == "OK")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = "[재고X] " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                            }
                        }
                    }
                }
                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            try
            {
                ssDiagno.ActiveSheet.RowCount = 0;

                SQL = "";
                SQL += " SELECT A.IllCode, B.ILLNAMEK,a.RO,b.GbVCode                   \r";
                SQL += "      , CASE                                    \r";
                SQL += "           WHEN B.GbV252 = '*' THEN '*'                                    \r";
                SQL += "           WHEN B.GbV352 = '*' THEN '*'                                    \r";
                SQL += "           ELSE ''                                    \r";
                SQL += "        END GbV252                                    \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OILLS A                                          \r";
                SQL += "      , KOSMOS_PMPA.BAS_ILLS B                                          \r";
                SQL += "  WHERE A.Ptno      = '" + txtPano.Text + "'                            \r";
                SQL += "    AND A.BDate     = TO_DATE('" + FstrBDate + "','YYYY-MM-DD')         \r";
                SQL += "    AND A.DeptCode  = '" + FstrDeptCode + "'                            \r";
                SQL += "    AND A.ILLCODE   = B.ILLCODE(+)                                      \r";
                SQL += "    AND B.ILLCLASS  = '1'                                               \r";
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {                    
                    ssDiagno.ActiveSheet.RowCount = dt.Rows.Count;
                    ssDiagno.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssDiagno.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ILLCODE"].ToString();
                        ssDiagno.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["ILLNAMEK"].ToString();
                        if (dt.Rows[i]["GBV252"].ToString() == "*")
                        {
                            ssDiagno.ActiveSheet.Cells[i, 2].Text = "★" + ssDiagno.ActiveSheet.Cells[i, 2].Text;
                        }
                        ssDiagno.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["RO"].ToString();
                        ssDiagno.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["GBVCODE"].ToString();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Order_Read_List()
        {
            ssActDate.ActiveSheet.RowCount = 0;
            ssOutMed.ActiveSheet.RowCount = 0;

            try
            {
                //OPD_READ
                SQL = "";
                SQL += " SELECT DISTINCT TO_CHAR(Bdate,'YYYY-MM-DD') Bdate, DeptCode, DrName, a.DrCode  \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER  A                                                \r";
                SQL += "      , KOSMOS_PMPA.BAS_DOCTOR B                                                \r";
                SQL += "  WHERE Ptno    = '" + txtPano.Text + "'                                        \r";
                SQL += "    AND Seqno   > 0                                                             \r";
                SQL += "    AND A.DrCode = B.DrCode                                                     \r";
                SQL += "  ORDER BY BDate DESC                                                           \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                ssActDate.ActiveSheet.RowCount = dt.Rows.Count;

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssActDate.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString();
                        ssActDate.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssActDate.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString();
                        ssActDate.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["DRCODE"].ToString();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            try
            {
                //IPD_SLIP_READ
                SQL = "";
                SQL += " SELECT TO_CHAR(SysDate,'YYYY-MM-DD') BDate, BI, A.IPDNO        \r";
                SQL += "      , DeptCode, DrName, TO_CHAR(InDate,'YYYY-MM-DD') InDate1  \r";
                SQL += "   FROM KOSMOS_PMPA.IPD_NEW_MASTER A                            \r";
                SQL += "      , KOSMOS_PMPA.BAS_DOCTOR     B                            \r";
                SQL += "  WHERE Pano     = '" + txtPano.Text + "'                       \r";
                SQL += "    AND A.DrCode = B.DrCode                                     \r";
                SQL += "    AND A.GBSTS IN ('0','2')                                    \r";
                SQL += "    AND A.INDATE IS NULL                                        \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count == 1)
                {   
                    SQL = "";
                    SQL += " SELECT GbHost                                              \r";
                    SQL += "   FROM KOSMOS_PMPA.IPD_NEW_SLIP                            \r";   
                    SQL += "  WHERE Pano   = '" + txtPano.Text + "'                     \r";
                    SQL += "    AND GbSlip = 'T'                                        \r";
                    SQL += "    AND Bun    IN ('11','12')                               \r";
                    SQL += "    AND ROWNUM = 1                                          \r";
                    SQL += "    AND TRSNO = '" + dt.Rows[0]["TRSNO"].ToString() + "'    \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                    if (SqlErr != "")
                    {
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    
                    if (dt1.Rows.Count > 0)
                    {
                        ssOutMed.ActiveSheet.RowCount = dt1.Rows.Count;
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            ssOutMed.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString();
                            ssOutMed.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString();
                            ssOutMed.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString();
                            ssOutMed.ActiveSheet.Cells[i, 3].Text = "SLIP ";
                            ssOutMed.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["BI"].ToString();
                            ssOutMed.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["INDATE1"].ToString();
                        }
                    }

                    dt1.Dispose();
                    dt1 = null;
                }

                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            try
            {
                //IPD_TSLIP_READ
                SQL = "";
                SQL += " SELECT TO_CHAR(A.OUTDate,'YYYY-MM-DD') BDate, A.LASTTRS, A.IPDNO   \r";        //'2005-08-20 윤종필 변경함.
                SQL += "      , TO_CHAR(A.OutDate,'YYYY-MM-DD') BDate1                      \r";
                SQL += "      , BI, DeptCode, DrName, TO_CHAR(InDate,'YYYY-MM-DD') InDate1  \r";
                SQL += "   FROM KOSMOS_PMPA.IPD_NEW_MASTER A                                \r";
                SQL += "      , KOSMOS_PMPA.BAS_DOCTOR     B                                \r";
                SQL += "  WHERE Pano     = '" + txtPano.Text + "'                           \r";
                SQL += "    AND A.DrCode = B.DrCode                                         \r";
                SQL += "    AND A.OUTDATE IS NOT NULL                                       \r";
                SQL += "    AND A.GBSTS <> '9'                                              \r"; //0'입원취소 제외
                SQL += "  ORDER BY ActDate DESC                                             \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    ssOutMed.ActiveSheet.RowCount = ssOutMed.ActiveSheet.RowCount + dt.Rows.Count;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ssOutMed.ActiveSheet.Cells[i, 0].Text = dt.Rows[i]["BDATE"].ToString();
                        ssOutMed.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DEPTCODE"].ToString();
                        ssOutMed.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DRNAME"].ToString();
                        ssOutMed.ActiveSheet.Cells[i, 3].Text = "TSLIP";
                        ssOutMed.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["BI"].ToString();
                        ssOutMed.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["INDATE1"].ToString();
                        ssOutMed.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["IPDNO"].ToString();
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            clsOrdFunction.GstrPatNo = "";
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            fn_Order_Read_List();
        }

        void fn_ChkAll_Reset()
        {
            chkAll.Checked = false;
            if (ssOrder.ActiveSheet.NonEmptyRowCount > 0)
            {
                ssOrder.ActiveSheet.Cells[0, 0, ssOrder.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "False";
            }
        }

        private void ssActDate_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strBDate = "";
            string strDeptCode = "";
            string strDrCode = "";

            if (e.ColumnHeader == true)
            {
                SP.setSpdSort(ssActDate, e.Column, true);
                return;
            }

            strBDate = DateTime.Parse(ssActDate.ActiveSheet.Cells[e.Row, 0].Text).ToShortDateString();
            strDeptCode = ssActDate.ActiveSheet.Cells[e.Row, 1].Text.Trim();
            strDrCode = ssActDate.ActiveSheet.Cells[e.Row, 2].Text.Trim();

            fn_Order_Read_List_SS(strBDate, strDeptCode, strDrCode);
            
            fn_ChkAll_Reset();

            if (clsType.User.Sabun.Trim() == "31544")   //심병주 과장
            {
                chkDiagAll.Checked = true;
                chkAll.Checked = true;
                chkDiagAll_Click(chkDiagAll, new EventArgs());
                chkAll_Click(chkAll, new EventArgs());
            }
        }

        int fn_SunapOrder_Count(string sBDate, string sDeptCode, string sGBIO)
        {
            int strRtn = 0;

            try
            {
                ssDiagno.ActiveSheet.RowCount = 0;

                if (sGBIO == "OPD")
                {
                    SQL = "";
                    SQL += " SELECT count('X') sunapOrdCnt                                  \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_OORDER                                   \r";
                    SQL += "  WHERE Ptno        = '" + txtPano.Text + "'                    \r";
                    SQL += "    AND BDate       = TO_DATE('" + sBDate + "','YYYY-MM-DD')    \r";
                    SQL += "    AND DeptCode    = '" + sDeptCode.Trim() + "'                \r";
                    SQL += "    AND GBSUNAP   in('1')                                       \r";
                    SQL += "    AND SEQNO > 0                                               \r";
                    //SQL += "    AND BUN IS NOT NULL                                         \r";
                    //SQL += "    AND GbStatus IN (' ','D+')                                  \r";
                }
                else
                {
                    SQL = "";
                    SQL += " SELECT count('X') sunapOrdCnt                                  \r";
                    SQL += "   FROM KOSMOS_OCS.OCS_IORDER                                   \r";
                    SQL += "  WHERE PTNO = '" + txtPano.Text + "'                           \r";
                    SQL += "    AND BDATE =  TO_DATE('" + sBDate + "','YYYY-MM-DD')         \r";
                    SQL += "    AND Bun IN ('11','12','20')                                 \r";
                    SQL += "    AND GbTFLAG  = 'T'                                          \r";
                    SQL += "    AND (GBSTATUS NOT IN ('D','D-','D+')  OR GBSTATUS =' ' )    \r";
                    SQL += "    AND ORDERSITE NOT IN 'CAN'                                  \r";
                }
                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return strRtn;
                }

                if (dt2.Rows.Count > 0)
                {
                    strRtn = int.Parse(dt2.Rows[0]["SUNAPORDCNT"].ToString());
                }

                dt2.Dispose();
                dt2 = null;
                return strRtn;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return strRtn;
            }
        }

        void fn_Order_Read_List_SS(string sBDate, string sDeptCode, string sDrCode)
        {
            string strUnit = "";
            string strDept = "";
            

            strDept = sDeptCode;

            fn_Screen_Clear();

            try
            {
                SQL = "";
                SQL += " SELECT OrderName,   DispHeader, C.DosName,  A.RealQty, a.drcode                        \r";
                SQL += "      , A.Nal,       A.GbDiv,    A.Remark,   B.DispRGB                                  \r";
                SQL += "      , A.OrderCode, A.Seqno,    OrderNameS, A.GbInfo                                   \r";
                SQL += "      , B.GbBoth,    A.Slipno,   A.DosCode,  a.GbTax                                    \r";
                SQL += "      , A.Gbboth     JUSA,       A.ROWID , a.GBSUNAP, D.NAL JNAL ,  D.ACTNAL  JACTNAL   \r";
                //SQL += "      , a.SuCode,    a.BUN,      a.GBSELF                                               \r";
                //2021-01-07 안정수, CONTENTS, BCONTENTS, GBGROUP 추기
                SQL += "      , a.SuCode,    a.BUN,      a.GBSELF, a.CONTENTS, a.BCONTENTS, a.GBGROUP           \r";
                //2021-01-11 안정수 추가, TUYEOPOINT, TUYEOTIME 
                SQL += "      , a.TUYEOPOINT,    a.TUYEOTIME                                                    \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(A.DOSCODE) DOSNAME1                              \r";
                SQL += "      , KOSMOS_OCS.FC_OCS_OSPECIMAN_NAME(A.DOSCODE, A.SLIPNO) SPECNAME                  \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OORDER    A                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ODOSAGE   C                                                      \r";
                SQL += "      , KOSMOS_OCS.ETC_JUSAMST   D                                                      \r";
                SQL += "  WHERE A.Ptno        = '" + txtPano.Text + "'                                          \r";
                SQL += "    AND A.BDate       = TO_DATE('" + sBDate + "','YYYY-MM-DD')                          \r";
                SQL += "    AND A.DeptCode    = '" + strDept.Trim() + "'                                        \r";
                SQL += "    AND A.GBSUNAP <> '2'                                                                \r";
                SQL += "    AND A.NAL > 0                                                                       \r";
                SQL += "    AND A.Seqno     > 0                                                                 \r";
                SQL += "    AND A.OrderCode = B.OrderCode(+)                                                    \r";
                SQL += "    AND A.Slipno    = B.Slipno(+)                                                       \r";
                SQL += "    AND A.DosCode   = C.DosCode(+)                                                      \r";
                SQL += "    AND A.ORDERNO   = D.ORDERNO(+)                                                      \r";
                //SQL += "  ORDER BY A.GBSUNAP DESC, a.drcode, 10                                                 \r";
                //SQL += "  ORDER BY A.gbsunap desc, A.GBCOPY, A.GBAUTOSEND2, A.Slipno, A.seqno                   \r";
                SQL += "  ORDER BY A.gbsunap desc, A.GBCOPY, A.GBAUTOSEND2, A.seqno                             \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            //ssOrder.ActiveSheet.Cells[i, 7].Text = "#";
                            ssOrder.ActiveSheet.Cells[i, 8].Text = "#";
                        }
                        else
                        {
                            //ssOrder.ActiveSheet.Cells[i, 7].Text = "";
                            ssOrder.ActiveSheet.Cells[i, 8].Text = "";
                        }
                        
                        if (string.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A1") >= 0 &&
                            string.Compare(dt.Rows[i]["SLIPNO"].ToString().Trim(), "A4") <= 0)
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        }
                        else
                        {
                            if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                            {
                                strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                                ssOrder.ActiveSheet.Cells[i, 1].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                            }
                            else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                            else
                            {
                                ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            }
                        }
                        if (dt.Rows[i]["GBBOTH"].ToString().Trim() == "1" && (dt.Rows[i]["GBINFO"].ToString().Trim() != ""))
                        {
                            //ssOrder.ActiveSheet.Cells[i, 1].Text = VB.Left(ssOrder.ActiveSheet.Cells[i, 1].Text, 30) + dt.Rows[i]["GBINFO"].ToString().Trim();
                            ssOrder.ActiveSheet.Cells[i, 1].Text = VB.Left(ssOrder.ActiveSheet.Cells[i, 1].Text, 31) + dt.Rows[i]["GBINFO"].ToString().Trim();
                        }

                        if (dt.Rows[i]["DOSNAME"].ToString().Trim() != "")
                        {
                            ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["DOSNAME1"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[i]["SPECNAME"].ToString().Trim();
                        }
                        //else
                        //{
                        //    if (dt.Rows[i]["DOSCODE"].ToString().Trim() != "")
                        //    {
                        //        SQL = "";
                        //        SQL += " SELECT SpecName                                                    \r";
                        //        SQL += "   FROM KOSMOS_OCS.OCS_OSPECIMAN                                    \r";
                        //        SQL += "  WHERE Slipno   = '" + dt.Rows[i]["SLIPNO"].ToString() + "'        \r";
                        //        SQL += "    AND SpecCode = '" + dt.Rows[i]["DOSCODE"].ToString() + "'       \r";
                        //        SQL += "    AND ROWNUM   = 1                                                \r";
                        //        SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                        //        if (SqlErr != "")
                        //        {
                        //            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        //            return;
                        //        }

                        //        if (dt1.Rows.Count == 1)
                        //        {
                        //            ssOrder.ActiveSheet.Cells[0, 2].Text = dt1.Rows[0]["SPECNAME"].ToString();
                        //        }
                        //        else
                        //        {
                        //            ssOrder.ActiveSheet.Cells[0, 2].Text = dt.Rows[i]["GBINFO"].ToString();
                        //        }

                        //        dt1.Dispose();
                        //        dt1 = null;
                        //    }
                        //}

                        //ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        //ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["NAL"].ToString().Trim();

                        ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["CONTENTS"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["REALQTY"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["NAL"].ToString().Trim();
                        if (dt.Rows[i]["GBDIV"].ToString() != "0")
                        {
                            //ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                            ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["GBDIV"].ToString().Trim();
                        }
                        if (VB.Mid(dt.Rows[i]["DOSCODE"].ToString(), 5, 2) == "01")
                        {
                            if (dt.Rows[i]["JUSA"].ToString() == "3")
                            {
                                //ssOrder.ActiveSheet.Cells[i, 6].Text = "완료";
                                ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                            }
                        }
                        if (VB.Mid(dt.Rows[i]["DOSCODE"].ToString(), 5, 2) == "02")
                        {
                            if (dt.Rows[i]["JUSA"].ToString() == "1")
                            {
                                //ssOrder.ActiveSheet.Cells[i, 6].Text = "완료";
                                ssOrder.ActiveSheet.Cells[i, 7].Text = "완료";
                            }
                        }

                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            //ssOrder.ActiveSheet.Cells[i, 7].Text = "#";
                            ssOrder.ActiveSheet.Cells[i, 8].Text = "#";
                        }
                        else
                        {
                            //ssOrder.ActiveSheet.Cells[i, 7].Text = "";
                            ssOrder.ActiveSheet.Cells[i, 8].Text = "";
                        }


                        //ssOrder.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString();
                        ssOrder.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString();

                        if (dt.Rows[i]["DISPRGB"].ToString() != "")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["DISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));
                        }

                        //ssOrder.ActiveSheet.Cells[i, 10].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString());
                        ssOrder.ActiveSheet.Cells[i, 11].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["DRCODE"].ToString());

                        if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = "★항혈전 " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 255);
                        }

                        if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = "< !> " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                        }

                        if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString(), FstrDeptCode) == "OK")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = "[재고X] " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                        }

                        //ssOrder.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["SUCODE"].ToString();
                        //ssOrder.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        //ssOrder.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                        //ssOrder.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["GBINFO"].ToString().Trim(); //촬영부위

                        //ssOrder.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["SUCODE"].ToString();
                        ssOrder.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["GBSELF"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["GBINFO"].ToString().Trim(); //촬영부위

                        ssOrder.ActiveSheet.Cells[i, 17].Text = dt.Rows[i]["REMARK"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim().PadRight(10) + ssOrder.ActiveSheet.Cells[i, 1].Text;

                        //2021-01-07 추가
                        ssOrder.ActiveSheet.Cells[i, 18].Text = dt.Rows[i]["BCONTENTS"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 19].Text = dt.Rows[i]["GBGROUP"].ToString().Trim();

                        //2021-01-11 추가
                        ssOrder.ActiveSheet.Cells[i, 20].Text = dt.Rows[i]["TUYEOPOINT"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 21].Text = dt.Rows[i]["TUYEOTIME"].ToString().Trim();
                    }

                    ssOrder.ActiveSheet.Cells[0, 1, ssOrder.ActiveSheet.NonEmptyRowCount - 1, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);

                    //외래간호 요청으로 수납 Line 추가(2018.03.13)
                    nSunapOrdCnt = fn_SunapOrder_Count(sBDate, sDeptCode, "OPD");

                    if (nSunapOrdCnt > 0)
                    {
                        ssOrder.ActiveSheet.Cells[nSunapOrdCnt - 1, 1, nSunapOrdCnt - 1, ssOrder.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.Red, 1, false, false, false, true);
                    }
                    
                    try
                    {
                        ssDiagno.ActiveSheet.RowCount = 0;

                        SQL = "";
                        SQL += " SELECT A.IllCode, B.ILLNAMEK,a.RO,b.GbVCode                   \r";
                        SQL += "      , CASE                                    \r";
                        SQL += "           WHEN B.GbV252 = '*' THEN '*'                                    \r";
                        SQL += "           WHEN B.GbV352 = '*' THEN '*'                                    \r";
                        SQL += "           ELSE ''                                    \r";
                        SQL += "        END GbV252                                    \r";
                        SQL += "   FROM KOSMOS_OCS.OCS_OILLS A                                          \r";
                        SQL += "      , KOSMOS_PMPA.BAS_ILLS B                                          \r";
                        SQL += "  WHERE A.Ptno      = '" + txtPano.Text + "'                            \r";
                        SQL += "    AND A.BDate     = TO_DATE('" + sBDate + "','YYYY-MM-DD')            \r";
                        SQL += "    AND A.DeptCode  = '" + strDept.Trim() + "'                          \r";
                        SQL += "    AND A.ILLCODE   = B.ILLCODE(+)                                      \r";
                        SQL += "    AND B.ILLCLASS  = '1'                                               \r";
                        SQL += "  ORDER BY A.SEQNO                                                      \r";
                        SqlErr = clsDB.GetDataTableREx(ref dt1, SQL, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt1.Rows.Count > 0)
                        {
                            ssDiagno.ActiveSheet.RowCount = dt1.Rows.Count;
                            ssDiagno.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                ssDiagno.ActiveSheet.Cells[i, 1].Text = dt1.Rows[i]["ILLCODE"].ToString();
                                ssDiagno.ActiveSheet.Cells[i, 2].Text = dt1.Rows[i]["ILLNAMEK"].ToString();
                                if (dt1.Rows[i]["GBV252"].ToString() == "*")
                                {
                                    ssDiagno.ActiveSheet.Cells[i, 2].Text = "★" + ssDiagno.ActiveSheet.Cells[i, 2].Text;
                                }
                                ssDiagno.ActiveSheet.Cells[i, 3].Text = dt1.Rows[i]["RO"].ToString();
                                ssDiagno.ActiveSheet.Cells[i, 4].Text = dt1.Rows[i]["GBVCODE"].ToString();
                            }
                        }

                        dt1.Dispose();
                        dt1 = null;
                    }
                    catch (OracleException ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssOutMed_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strInDate = "";
            string strBDate = "";
            string strBi = "";
            string strGubun = "";
            long nIpdNo = 0;
            string sDeptCode = "";
            string sDrCode = "";
            
            if (ssOutMed.ActiveSheet.NonEmptyRowCount == 0) return;

            if (e.ColumnHeader == true)
            {
                SP.setSpdSort(ssOutMed, e.Column, true);
                return;
            }
            
            strInDate = DateTime.Parse(ssOutMed.ActiveSheet.Cells[e.Row, 5].Text).ToShortDateString();
            strBDate = DateTime.Parse(ssOutMed.ActiveSheet.Cells[e.Row, 0].Text).ToShortDateString();
            strGubun = ssOutMed.ActiveSheet.Cells[e.Row, 3].Text;
            strBi = ssOutMed.ActiveSheet.Cells[e.Row, 4].Text;
            nIpdNo = Convert.ToInt32(ssOutMed.ActiveSheet.Cells[e.Row, 6].Text);
            sDeptCode = ssOutMed.ActiveSheet.Cells[e.Row, 1].Text;
            sDrCode = ssOutMed.ActiveSheet.Cells[e.Row, 4].Text;
            
            if (ssOrder.ActiveSheet.NonEmptyRowCount > 0)
            { 
                ssOrder.ActiveSheet.Cells[0, 1, ssOrder.ActiveSheet.NonEmptyRowCount - 1, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
            }

            nSunapOrdCnt = fn_SunapOrder_Count(strBDate, sDeptCode, "IPD");

            fn_Ipd_Read_List(strBDate, strBi, strGubun, strInDate, nIpdNo);
            fn_Ipd_Read_Ills(strInDate);

            //lblRemark.Text = ssOrder.ActiveSheet.Cells[0, 6].Text.Trim();
            lblRemark.Text = ssOrder.ActiveSheet.Cells[0, 7].Text.Trim();

            fn_ChkAll_Reset();
        }

        void fn_Ipd_Read_Ills(string strInDate)
        {
            try
            {
                ssDiagno.ActiveSheet.RowCount = 0;

                SQL = "";
                SQL += " SELECT A.IllCode, B.ILLNAMEK                                   \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IILLS A                                  \r";
                SQL += "      , KOSMOS_PMPA.BAS_ILLS B                                  \r";
                SQL += " WHERE A.PTno      = '" + txtPano.Text + "'                     \r";
                SQL += "   AND A.ENTDate   = TO_DATE('" + strInDate + "','YYYY-MM-DD')  \r";
                SQL += "   AND A.ILLCODE = B.ILLCODE(+)                                 \r";
                SQL += "   AND B.ILLCLASS = '1'                                         \r";
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    ssDiagno.ActiveSheet.RowCount = dt1.Rows.Count;
                    ssDiagno.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        ssDiagno.ActiveSheet.Cells[i, 1].Text = dt1.Rows[i]["ILLCODE"].ToString();
                        ssDiagno.ActiveSheet.Cells[i, 2].Text = dt1.Rows[i]["ILLNAMEK"].ToString();
                    }
                }

                dt1.Dispose();
                dt1 = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void fn_Ipd_Read_List(string strBDate, string strBi, string strGubun, string strInDate, long nIpdNo)
        {
            string strUnit = "";

            fn_Screen_Clear();

            Cursor.Current = Cursors.WaitCursor;
            
            try
            {   
                SQL = "";
                SQL += " SELECT A.SUCODE, TO_CHAR(A.BDATE,'YYYY-MM-DD') BDATE, A.NAL SNAL       \r";
                SQL += "      , A.QTY SQTY,  a.ORDERCODE                                        \r";
                SQL += "      , B.OrderName, B.DispHeader, C.DosName,  B.DispRGB                \r";
                SQL += "      , B.OrderCode, B.Seqno,      B.OrderNameS, B.GbBoth               \r";
                //SQL += "      , B.Slipno,    B.SpecCode,   B.SendDept,   A.GbDiv                \r";
                SQL += "      , B.Slipno,    A.DOSCODE,   B.SendDept,   A.GbDiv                 \r";
                SQL += "      , A.REMARK,    A.ROWID,      A.BUN,        A.STAFFID              \r";
                //SQL += "      , KOSMOS_OCS.FC_OCS_ODOSAGE_NAME(A.DOSCODE) DOSNAME               \r";
                SQL += "   FROM KOSMOS_OCS.OCS_IORDER    A                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ORDERCODE B                                      \r";
                SQL += "      , KOSMOS_OCS.OCS_ODOSAGE   C                                      \r";
                SQL += "  WHERE A.PTNO = '" + txtPano.Text + "'                                 \r";
                SQL += "    AND A.BDATE =  TO_DATE('" + strBDate + "','YYYY-MM-DD')             \r";
                SQL += "    AND A.Bun IN ('11','12')                                            \r";
                SQL += "    AND A.GbTFLAG  = 'T'                                                \r";
                SQL += "    AND (A.GBSTATUS NOT IN ('D','D-','D+')  OR A.GBSTATUS =' ' )        \r";
                SQL += "    AND A.ORDERSITE NOT IN 'CAN'                                        \r";
                SQL += "    AND A.ORDERCODE = B.ORDERCODE                                       \r";
                SQL += "    AND B.SEQNO > 0                                                     \r";
                SQL += "    AND B.GBDOSAGE = '1'                                                \r";
                //SQL += "    AND B.SPECCODE = C.DOSCODE(+)                                       \r";
                SQL += "    AND A.DOSCODE = C.DOSCODE(+)                                        \r";
                SQL += " ORDER BY A.Bun, A.SUCODE, A.BDATE                                      \r";
                
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["ORDERNAMES"].ToString().Trim() != "")
                        {
                            strUnit = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                            ssOrder.ActiveSheet.Cells[i, 1].Text = strUnit + " " + dt.Rows[i]["ORDERNAMES"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["DISPHEADER"].ToString().Trim() != "")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["DISPHEADER"].ToString().Trim() + " " + dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        }
                        else
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERNAME"].ToString().Trim();
                        }

                        if (dt.Rows[i]["SENDDEPT"].ToString() == "N")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text += "[사용하지않음]";
                        }

                        if (dt.Rows[i]["DOSNAME"].ToString() != "")
                        {
                            SQL = "";
                            SQL += " SELECT a.OrderCode,a.DosCode,b.DosName,b.GbDiv                                 \r";
                            SQL += "   FROM KOSMOS_OCS.OCS_IORDER  a                                                \r";
                            SQL += "      , KOSMOS_OCS.OCS_ODOSAGE b                                                \r";
                            SQL += "  WHERE a.PtNo      = '" + txtPano.Text + "'                                    \r";
                            SQL += "    AND a.BDate     = TO_DATE('" + DateTime.Parse(dt.Rows[i]["BDate"].ToString()).ToShortDateString() + "','YYYY-MM-DD') \r";
                            SQL += "    AND a.OrderCode = '" + dt.Rows[i]["OrderCode"].ToString().Trim() + "'       \r";
                            SQL += "    AND a.GbTFlag   = 'T'                                                       \r";
                            SQL += "    AND a.DosCode   = b.DosCode(+)                                              \r";
                            SQL += "    AND ROWNUM      = 1                                                         \r";
                            SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                            if (SqlErr != "")
                            {
                                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                return;
                            }

                            if (dt1.Rows.Count == 0)
                            {
                                ssOrder.ActiveSheet.Cells[i, 2].Text = dt.Rows[0]["DOSNAME"].ToString();
                                //ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[0]["GBDIV"].ToString();
                                ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[0]["GBDIV"].ToString();
                            }
                            else
                            {
                                ssOrder.ActiveSheet.Cells[i, 2].Text = dt1.Rows[0]["DOSNAME"].ToString();
                                //ssOrder.ActiveSheet.Cells[i, 4].Text = dt1.Rows[0]["GBDIV"].ToString();
                                //ssOrder.ActiveSheet.Cells[i, 9].Text = dt1.Rows[0]["DOSCODE"].ToString();
                                ssOrder.ActiveSheet.Cells[i, 5].Text = dt1.Rows[0]["GBDIV"].ToString();
                                ssOrder.ActiveSheet.Cells[i, 10].Text = dt1.Rows[0]["DOSCODE"].ToString();
                            }
                            dt1.Dispose();
                            dt1 = null;
                        }
                        else
                        {
                            if (dt.Rows[i]["SPECCODE"].ToString() != "")
                            {
                                SQL = "";
                                SQL += " SELECT SPECNAME FROM KOSMOS_OCS.OCS_OSPECIMAN                          \r";
                                SQL += "  WHERE SpecCode = '" + dt.Rows[i]["SpecCode"].ToString().Trim() + "'   \r";
                                SQL += "    AND ROWNUM = 1                                                      \r";
                                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);
                                if (SqlErr != "")
                                {
                                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                                    return;
                                }

                                if (dt1.Rows.Count > 0)
                                {
                                    ssOrder.ActiveSheet.Cells[i, 2].Text = dt1.Rows[0]["SPECNAME"].ToString();
                                }
                                dt1.Dispose();
                                dt1 = null;
                            }
                        }

                        //ssOrder.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SQTY"].ToString().Trim().Trim();
                        //ssOrder.ActiveSheet.Cells[i, 5].Text = dt.Rows[i]["SNAL"].ToString().Trim().Trim();
                        //ssOrder.ActiveSheet.Cells[i, 6].Text = strGubun;
                        ssOrder.ActiveSheet.Cells[i, 4].Text = dt.Rows[i]["SQTY"].ToString().Trim().Trim();
                        ssOrder.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["SNAL"].ToString().Trim().Trim();
                        ssOrder.ActiveSheet.Cells[i, 7].Text = strGubun;
                        if (dt.Rows[i]["REMARK"].ToString().Trim() != "")
                        {
                            //ssOrder.ActiveSheet.Cells[i, 7].Text = "#";
                            ssOrder.ActiveSheet.Cells[i, 8].Text = "#";
                        }
                        else
                        {
                            //ssOrder.ActiveSheet.Cells[i, 7].Text = "";
                            ssOrder.ActiveSheet.Cells[i, 8].Text = "";
                        }


                        //ssOrder.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        //ssOrder.ActiveSheet.Cells[i, 10].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["STAFFID"].ToString());
                        //ssOrder.ActiveSheet.Cells[i, 12].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        //ssOrder.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["BUN"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 11].Text = clsVbfunc.GetBASDoctorName(clsDB.DbCon, dt.Rows[i]["STAFFID"].ToString());
                        ssOrder.ActiveSheet.Cells[i, 13].Text = dt.Rows[i]["SUCODE"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["BUN"].ToString().Trim();

                        ssOrder.ActiveSheet.Cells[i, 2, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = ColorTranslator.FromWin32(int.Parse(dt.Rows[i]["DISPRGB"].ToString().Trim(), System.Globalization.NumberStyles.AllowHexSpecifier));

                        if (clsOrderEtc.READ_SUGA_ANTIBLOOD(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = "★항혈전 " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].ForeColor = Color.FromArgb(255, 0, 255);
                        }

                        if (clsOrderEtc.READ_SUGA_COMPONENT(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString().Trim()) == "OK")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = "<!> " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                        }

                        if (clsOrderEtc.READ_SUGA_MR_EXPENSIVE_MEDICINE(clsDB.DbCon, dt.Rows[i]["SUCODE"].ToString(), FstrDeptCode) == "OK")
                        {
                            ssOrder.ActiveSheet.Cells[i, 1].Text = "[재고X] " + ssOrder.ActiveSheet.Cells[i, 1].Text;
                        }
                        //ssOrder.ActiveSheet.Cells[i, 16].Text = strUnit + " " + dt.Rows[i]["REMARK"].ToString().Trim();
                        ssOrder.ActiveSheet.Cells[i, 17].Text = strUnit + " " + dt.Rows[i]["REMARK"].ToString().Trim();

                        ssOrder.ActiveSheet.Cells[i, 1].Text = dt.Rows[i]["ORDERCODE"].ToString().Trim().PadRight(10) + ssOrder.ActiveSheet.Cells[i, 1].Text;
                    }
                }

                dt.Dispose();
                dt = null;
            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            if (nSunapOrdCnt > 0)
            {
                ssOrder.ActiveSheet.Cells[nSunapOrdCnt - 1, 1, nSunapOrdCnt - 1, ssOrder.ActiveSheet.ColumnCount - 1].Border = new LineBorder(Color.Red, 1, false, false, false, true);
            }
            Cursor.Current = Cursors.Default;
        }

        private void ssOrder_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.RowHeader == true)
            {
                ssOrder.ActiveSheet.Cells[0, 0, ssOrder.ActiveSheet.RowCount - 1, 0].Text = "True";
            }
            //lblRemark.Text = ssOrder.ActiveSheet.Cells[e.Row, 16].Text.Trim(); 
            lblRemark.Text = ssOrder.ActiveSheet.Cells[e.Row, 17].Text.Trim();
        }

        private void txtPano_Click(object sender, EventArgs e)
        {
            fn_Screen_Clear();

            txtPano.SelectionStart = 0;
            txtPano.SelectionLength = txtPano.Text.Length;

            clsOrdFunction.GstrPatNo = "";

            //FrmMedViewPtno f = new FrmMedViewPtno();
            //f.ShowDialog();
            //OF.fn_ClearMemory(f);
            
            //txtPano.Text = clsOrdFunction.GstrPatNo;            
            //getData();
            //fn_ReadOrder();
            //fn_Order_Read_List();
            //clsOrdFunction.GstrPatNo = "";
        }

        void fn_Screen_Clear()
        {
            SP.Spread_All_Clear(ssOrder);
            ssOrder.ActiveSheet.RowCount = 300;
            //SP.Spread_Clear(ssOrder, ssOrder.ActiveSheet.RowCount - 1, ssOrder.ActiveSheet.ColumnCount - 1);
        }

        private void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar == (char)13)
            //{
            //    txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
            //    getData();
            //    fn_ReadOrder();
            //    fn_Order_Read_List();
            //}                  
        }

        private void ssOrder_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            //lblRemark.Text = ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 7].Text; 
        }

        private void ssOrder_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            if (e.Column != 0) return;

            if (ssOrder.ActiveSheet.Cells[e.Row, 1].Text == "")
            {
                if (ssOrder.ActiveSheet.Cells[e.Row, 0].Text == "True")
                {
                    ssOrder.ActiveSheet.Cells[e.Row, 0].Text = "False";
                    SP.setEnterKey(ssOrder, clsSpread.enmSpdEnterKey.Right);
                    return;
                }
            }

            if (ssOrder.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                ssOrder.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);
            }
            else
            {
                ssOrder.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
            }
        }

        private void btnRegist_Click(object sender, EventArgs e)
        {
            if (VB.Replace(clsPublic.GstrSysDate,"-","") != FstrBDate)
            {
                MessageBox.Show("예약오더는 당일만 입력 가능합니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
                return;
            }

            if (FstrPano.Trim() != "")
            {
                if (FstrDeptCode == "FM")
                {
                    FrmMedRsvOrder RsvOrd = new FrmMedRsvOrder(FstrPano, FstrDeptCode, clsOrdFunction.Pat.DrCode);
                    RsvOrd.ShowDialog();
                }
                else
                {
                    FrmMedRsvOrderNew RsvOrdNew = new FrmMedRsvOrderNew(FstrPano, FstrDeptCode, clsOrdFunction.Pat.DrCode);
                    RsvOrdNew.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("환자선택후 다시 작업하십시오!!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnOrderSend_Click(object sender, EventArgs e)
        {
            string strillCode = "";
            string strillName = "";
            string strRoGbn = "";
            string strOK = "";
            string strVCode = "";
            int k;

            int nillRow = 0;

            nillRow = clsOrdFunction.GnDiagnoCnt;

            for (int i = 0; i < ssDiagno.ActiveSheet.RowCount; i++)
            {
                strillCode = ssDiagno.ActiveSheet.Cells[i, 1].Text;
                strillName = ssDiagno.ActiveSheet.Cells[i, 2].Text;
                strRoGbn = ssDiagno.ActiveSheet.Cells[i, 3].Text;
                strVCode = ssDiagno.ActiveSheet.Cells[i, 4].Text;

                if (ssDiagno.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    strOK = "OK";
                    LastillsClick(strillCode, strillName, strRoGbn, strVCode);
                }
            }

            if (((Button)sender).Name == "btnOrderSend")
            {
                k = 0;

                //if (ssOrder.ActiveSheet.Cells[0, 6].Text == "SLIP" || ssOrder.ActiveSheet.Cells[0, 6].Text == "TSLIP")
                if (ssOrder.ActiveSheet.Cells[0, 7].Text == "SLIP" || ssOrder.ActiveSheet.Cells[0, 7].Text == "TSLIP")
                {
                    for (int i = 0; i < ssOrder.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssOrder.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            //LastOrderClick(ssOrder.ActiveSheet.Cells[i, 8].Text, "IPD", FnStartRow + k);
                            LastOrderClick(ssOrder.ActiveSheet.Cells[i, 9].Text, "IPD", FnStartRow + k);
                            k++;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ssOrder.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssOrder.ActiveSheet.Cells[i, 0].Text == "True")
                        {
                            //LastOrderClick(ssOrder.ActiveSheet.Cells[i, 8].Text, "OPD", FnStartRow + k);
                            LastOrderClick(ssOrder.ActiveSheet.Cells[i, 9].Text, "OPD", FnStartRow + k);
                            k++;
                        }
                    }
                }
            }
            clsOrdFunction.GstrPatNo = "";
            this.Close();
        }

        private void getData()
        {
            try
            {

                btnSearch.Focus();
                SQL = "";
                SQL += " SELECT Sname, BI FROM KOSMOS_PMPA.BAS_PATIENT  \r";
                SQL += "  WHERE Pano = '" + txtPano.Text + "'           \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt.Rows.Count > 0)
                {
                    lblSName.Text = dt.Rows[0]["SNAME"].ToString();
                    lblBiName.Text = clsVbfunc.GetBiName(dt.Rows[0]["BI"].ToString());
                }
                else
                {
                    MessageBox.Show("해당환자가 존재하지 않습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                dt.Dispose();
                dt = null;

            }
            catch (OracleException ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void txtPano_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txtPano.Text = ComFunc.LPAD(txtPano.Text, 8, "0");
                FstrPano = txtPano.Text;

                getData();
                fn_ReadOrder();
                fn_Order_Read_List();
            }
        }

        private void txtPano_DoubleClick(object sender, EventArgs e)
        {
            //fn_Screen_Clear();

            //txtPano.SelectionStart = 0;
            //txtPano.SelectionLength = txtPano.Text.Length;

            //clsOrdFunction.GstrPatNo = "";

            //FrmMedViewPtno f = new FrmMedViewPtno();
            //f.ShowDialog();
            //OF.fn_ClearMemory(f);

            //txtPano.Text = clsOrdFunction.GstrPatNo;
            //getData();
            //fn_ReadOrder();
            //fn_Order_Read_List();
            //clsOrdFunction.GstrPatNo = "";
        }

        private void btnPtno_Click(object sender, EventArgs e)
        {
            fn_Screen_Clear();

            FrmMedViewPtno f = new FrmMedViewPtno();
            f.ShowDialog(this);
            OF.fn_ClearMemory(f);

            txtPano.Text = clsOrdFunction.GstrPatNo;
            FstrPano = clsOrdFunction.GstrPatNo;

            clsOrdFunction.GstrPatNo = "";

            getData();
            fn_ReadOrder();
            fn_Order_Read_List();
        }

        private void btnDrugInfo_Click(object sender, EventArgs e)
        {
            string strDrugCode = "";

            //if (ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 13].Text.Trim() == "11" ||
            //    ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 13].Text.Trim() == "12" ||
            //    ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 13].Text.Trim() == "20")
            if (ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 14].Text.Trim() == "11" ||
                ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 14].Text.Trim() == "12" ||
                ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 14].Text.Trim() == "20")
            {
                //strDrugCode = ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 12].Text.Trim();
                strDrugCode = ssOrder.ActiveSheet.Cells[ssOrder.ActiveSheet.ActiveRowIndex, 13].Text.Trim();

                //2019-07-12 전산업무 의뢰서 2019-822
                //frmSupDrstDifDown f = new frmSupDrstDifDown("MTSOORDER", strDrugCode);
                frmSupDrstInfoEntryNew f = new frmSupDrstInfoEntryNew(strDrugCode, "MTSOORDER", "VIEW");
                f.ShowDialog(this);
                OF.fn_ClearMemory(f);                
            }
            else
            {
                strDrugCode = "";
                MessageBox.Show("약품이 아닙니다.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ssOrder_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        {
            if (ssOrder.ActiveSheet.RowCount > 0)
            {
                if (e.Column == 1)
                {
                    e.ShowTip = true;
                    //e.TipText = "" + (char)13 + (char)10;
                    e.TipText = "";
                    e.TipText += ssOrder.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                    e.View.TextTipAppearance.Font = new Font("굴림체", 10, FontStyle.Bold);
                    e.View.TextTipAppearance.BackColor = Color.FromArgb(255, 225, 225);
                    e.View.TextTipAppearance.ForeColor = Color.Black;
                }
            }
        }

        private void chkAll_Click(object sender, EventArgs e)
        {
            if (ssOrder.ActiveSheet.NonEmptyRowCount > 0)
            {
                if (chkAll.Checked == true)
                {
                    ssOrder.ActiveSheet.Cells[0, 0, ssOrder.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "True";
                    ssOrder.ActiveSheet.Cells[0, 1, ssOrder.ActiveSheet.NonEmptyRowCount - 1, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
                }
                else
                {
                    ssOrder.ActiveSheet.Cells[0, 0, ssOrder.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "False";
                    ssOrder.ActiveSheet.Cells[0, 1, ssOrder.ActiveSheet.NonEmptyRowCount - 1, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                }
            }
            else
            {
                chkAll.Checked = false;
            }
        }

        private void chkDiagAll_Click(object sender, EventArgs e)
        {
            if (ssDiagno.ActiveSheet.NonEmptyRowCount > 0)
            {
                if (chkDiagAll.Checked == true)
                {
                    ssDiagno.ActiveSheet.Cells[0, 0, ssDiagno.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "True";
                    ssDiagno.ActiveSheet.Cells[0, 1, ssDiagno.ActiveSheet.NonEmptyRowCount - 1, ssDiagno.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
                }
                else
                {
                    ssDiagno.ActiveSheet.Cells[0, 0, ssDiagno.ActiveSheet.NonEmptyRowCount - 1, 0].Text = "False";
                    ssDiagno.ActiveSheet.Cells[0, 1, ssDiagno.ActiveSheet.NonEmptyRowCount - 1, ssDiagno.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                }
            }
            else
            {
                chkDiagAll.Checked = false;
            }
        }

        private void chkMed_Click(object sender, EventArgs e)
        {
            if (ssOrder.ActiveSheet.NonEmptyRowCount > 0)
            {
                if (chkMed.Checked == true)
                {
                    for (int i = 0; i < ssOrder.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        //if (ssOrder.ActiveSheet.Cells[i, 13].Text.Trim() == "11" || ssOrder.ActiveSheet.Cells[i, 13].Text.Trim() == "12" || ssOrder.ActiveSheet.Cells[i, 13].Text.Trim() == "20")
                        if (ssOrder.ActiveSheet.Cells[i, 14].Text.Trim() == "11" || ssOrder.ActiveSheet.Cells[i, 14].Text.Trim() == "12" || ssOrder.ActiveSheet.Cells[i, 14].Text.Trim() == "20")
                        {
                            ssOrder.ActiveSheet.Cells[i, 0].Text = "True";
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPSELCOLOR;
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < ssOrder.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        //if (ssOrder.ActiveSheet.Cells[i, 13].Text.Trim() == "11" || ssOrder.ActiveSheet.Cells[i, 13].Text.Trim() == "12" || ssOrder.ActiveSheet.Cells[i, 13].Text.Trim() == "20")
                        if (ssOrder.ActiveSheet.Cells[i, 14].Text.Trim() == "11" || ssOrder.ActiveSheet.Cells[i, 14].Text.Trim() == "12" || ssOrder.ActiveSheet.Cells[i, 14].Text.Trim() == "20")
                        {
                            ssOrder.ActiveSheet.Cells[i, 0].Text = "False";
                            ssOrder.ActiveSheet.Cells[i, 1, i, ssOrder.ActiveSheet.ColumnCount - 1].BackColor = ComNum.SPDESELCOLOR;
                        }
                    }
                }
            }
            else
            {
                chkMed.Checked = false;
            }
        }
    }
}
