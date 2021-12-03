using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComLibB
{
    /// <summary>
    /// Description : 용법 선택
    /// Author : 이상훈
    /// Create Date : 2017.07.10
    /// </summary>
    /// <history>
    /// </history>
    /// <seealso cref="FrmDosSearch.frm, FrmViewDosage.frm, FrmViewDosage_PRN.frm, FrmDosage.frm, FrmDosSet.frm, "/>
    public partial class FrmDosCode : Form 
    {
        string strCallFormName;
        string strDosCode;
        string strBun;
        string strGbPrn;
        string strGBIO;
        string strSpecCode;


        int nFRow = -1;
        int nSRow = -1;
        int nLRow = -1;

        string SQL = "";
        DataTable dt = null;
        DataTable dt1 = null;
        string SqlErr = "";

        bool mbolMouse;

        clsSpread SP = new clsSpread();
                
        public delegate void DosCode_OpdDoubleClick(string strFullDosage, string strDosCode, string strDiv, int nRow, string strGubun);
        public static event DosCode_OpdDoubleClick ssDosCodeOpdDoubleClick;

        public delegate void DosCode_IpdDoubleClick(string strFullDosage, string strDosCode, string strDiv, int nRow, string strGubun);
        public static event DosCode_IpdDoubleClick ssDosCodeIpdDoubleClick;

        public delegate void DosCode_ErDoubleClick(string strFullDosage, string strDosCode, string strDiv, int nRow, string strGubun);
        public static event DosCode_ErDoubleClick ssDosCodeErDoubleClick;
        
        public FrmDosCode()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 용법 선택 화면(부모폼, 용법코드, 분류(BUN), PRN 여부, 입원/외래 여부)
        /// </summary>
        /// <param name="sCallFormName"></param>
        /// <param name="sDosCode"></param>
        /// <param name="sBun"></param>
        /// <param name="sGbPrn"></param>
        /// <param name="sGbIO"></param>
        public FrmDosCode(string sCallFormName, string sDosCode, string sBun, string sGbPrn, string sGBIO, string sSpecCode, bool bolMouse)
        {
            strCallFormName = sCallFormName;
            strDosCode = sDosCode;
            strBun = sBun;
            strGbPrn = sGbPrn;
            strGBIO = sGBIO;
            strSpecCode = sSpecCode;
            mbolMouse = bolMouse;

            InitializeComponent();
        }

        private void FrmDosCode_Load(object sender, EventArgs e)
        {
            DataTable dtDos = null;
            string cBun;
            //string strFirstDosCode;
            //string strSecondDosCode;
            //string strLastDosCode;

            this.Location = new Point(300, 300);

            //if (mbolMouse == true)
            //{
            //    for (int i = 0; i < Screen.AllScreens.Length; i++)
            //    {
            //        if (Screen.AllScreens[i].Bounds.X <= Cursor.Position.X
            //        && Screen.AllScreens[i].Bounds.X + Screen.AllScreens[i].Bounds.Width > Cursor.Position.X)
            //        {
            //            if (Screen.AllScreens[i].Bounds.Height < Cursor.Position.Y + this.Height)
            //            {
            //                this.Location = new Point(Cursor.Position.X, Screen.AllScreens[i].Bounds.Height - this.Height);
            //            }
            //            else
            //            {
            //                this.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
            //            }
            //            break;
            //        }
            //    }
            //    this.StartPosition = FormStartPosition.Manual;
            //}
            //else
            //{
            //    this.StartPosition = FormStartPosition.CenterParent;
            //}

            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            {
                this.Close(); //폼 권한 조회
                return;
            }

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            cboBun.Items.Clear();
            cboBun.Items.Add("11.내복약");
            cboBun.Items.Add("12.외용약");
            cboBun.Items.Add("20.주사약");
            cboBun.SelectedIndex = 0;

            if (strCallFormName == "Order")
            {
                pnlGbio.Visible = false;
                pnlGubun.Visible = true;
            }
            else
            {
                pnlGbio.Visible = true;
                pnlGubun.Visible = false;
            }

            if (strBun != "11" && strBun != "12" && strBun != "20")
            {
                cBun = "20";
            }
            else
            {
                cBun = strBun;
            }

            SP.Spread_All_Clear(ssFirstDiv);

            try
            {
                SQL = "";
                if (strCallFormName != "Order")
                {
                    SQL += " SELECT DosFullCode, DosName, to_char(DosCode, 'FM000000') DOSCODE, GBDIV \r";
                    SQL += "   FROM ADMIN.OCS_ODOSAGE                              \r";
                    SQL += "  WHERE Bun   = '" + VB.Left(cboBun.Text, 2) + "'           \r";
                    if (rdoIpd.Checked == true)
                    {
                        SQL += "    AND GbIO IN ('A','I')                               \r";
                    }
                    else if (rdoOpd.Checked == true)
                    {
                        SQL += "    AND GbIO IN ('A','O')                               \r";
                    }
                    SQL += "    AND Substr(DosCode,3,4) = '0000'                        \r";
                    SQL += "    AND (DelDate IS NULL OR DelDate ='')                    \r";
                    SQL += "  ORDER BY DosCode                                          \r";

                }
                else
                {
                    if (strGbPrn == "P")    //PRN 처방
                    //if (clsPublic.GstrPRNChk == "OK")    //PRN 처방
                    {
                        SQL += " SELECT DosFullCode, DosName, to_char(DosCode, 'FM000000') DOSCODE, GBDIV \r";
                        SQL += "   FROM ADMIN.OCS_ODOSAGE                              \r";
                        SQL += "  WHERE Bun   = '" + cBun + "'                              \r";
                        if (strGBIO == "I")
                        {
                            SQL += "    AND GbIO IN ('A','I')                               \r";
                        }
                        else
                        {
                            SQL += "    AND GbIO IN ('A','O')                               \r";
                        }
                        SQL += "    AND DosCode IN ('910000','920000','950000','930000')    \r";
                        SQL += "    AND (DelDate IS NULL OR DelDate = '')                   \r";
                        SQL += "  ORDER BY DosCode                                          \r";
                    }
                    else
                    {
                        //if (clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" &&
                        //(clsOrdFunction.GstrSELECTBun == "11" || clsOrdFunction.GstrSELECTBun == "12" || clsOrdFunction.GstrSELECTBun == "20"))
                        if (clsPublic.Gstr구두Chk == "OK" && clsPublic.Gstr간호처방STS == "구두처방" &&
                            (cBun == "11" || cBun == "12" || cBun == "20"))
                        {
                            SQL += " SELECT DosFullCode, DosName, to_char(DosCode, 'FM000000') DOSCODE, GBDIV \r";
                            SQL += "   FROM ADMIN.OCS_ODOSAGE          \r";
                            SQL += "  WHERE Bun   = '" + cBun + "'          \r";
                            SQL += "    AND GbDiv ='1'                      \r";
                            SQL += "    AND Substr(DosCode,3,4) = '0000'    \r";
                            if (strGBIO == "I")
                            {
                                SQL += "    AND GbIO IN ('A','I')           \r";
                            }
                            else
                            {
                                SQL += "    AND GbIO IN ('A','O')           \r";
                            }
                            SQL += "    AND (DelDate IS NULL OR DelDate ='')\r";
                            SQL += "  ORDER BY DosCode                      \r";
                        }
                        else
                        {
                            SQL += " SELECT DosFullCode, DosName, to_char(DosCode, 'FM000000') DOSCODE, GBDIV \r";
                            SQL += "   FROM ADMIN.OCS_ODOSAGE          \r";
                            SQL += "  WHERE Bun   = '" + cBun + "'          \r";
                            SQL += "    AND Substr(DosCode,3,4) = '0000'    \r";
                            if (strGBIO == "I")
                            {
                                SQL += "    AND GbIO IN ('A','I')           \r";
                            }
                            else
                            {
                                SQL += "    AND GbIO IN ('A','O')           \r";
                            }
                            SQL += "    AND (DelDate IS NULL OR DelDate ='')\r";
                            SQL += "  ORDER BY DosCode                      \r";
                        }
                    }
                }
                SqlErr = clsDB.GetDataTable(ref dtDos, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dtDos.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dtDos, ssFirstDiv, 0, true);

                    if (strDosCode.Trim() != "")
                    {
                        for (int i = 0; i < dtDos.Rows.Count; i++)
                        {
                            if (VB.Left(dtDos.Rows[i]["DosCode"].ToString().Trim(), 2) == VB.Left(strDosCode, 2))
                            {
                                if (clsOrdFunction.GstrLoadFlag != "" || clsOrdFunction.GstrLoadFlag2 != "")
                                {
                                    nFRow = i;                                    
                                    CellClickEventArgs j = new CellClickEventArgs(new SpreadView(), 0, i, 0, i, new MouseButtons(), false, false);
                                    ssFirstDiv_CellClick(ssFirstDiv, j);
                                    ssFirstDiv.ActiveSheet.Cells[0, 0, ssFirstDiv.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                                    if (nFRow == -1)
                                    {
                                        ssFirstDiv.ActiveSheet.Cells[i, 0, i, 1].BackColor = Color.Aqua;
                                    }
                                    else
                                    {
                                        ssFirstDiv.ActiveSheet.Cells[nFRow, 0, nFRow, 1].BackColor = Color.Aqua;
                                    }

                                    if (ssSecondDiv.ActiveSheet.NonEmptyRowCount == 0)
                                    {
                                        nFRow = -1;
                                        strDosCode = "";
                                    }

                                    if (i > 10)
                                    {
                                        ssFirstDiv.ShowRow(0, i, FarPoint.Win.Spread.VerticalPosition.Center);
                                    }
                                    btnOk.Focus();
                                    return;
                                }
                                else
                                {   
                                    CellClickEventArgs j = new CellClickEventArgs(new SpreadView(), 0, i, 0, i, new MouseButtons(), false, false);
                                    ssFirstDiv_CellClick(ssFirstDiv, j);
                                    ssFirstDiv.ActiveSheet.Cells[0, 0, ssFirstDiv.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                                    ssFirstDiv.ActiveSheet.Cells[i, 0, i, 1].BackColor = Color.Aqua;

                                    //if (ssSecondDiv.ActiveSheet.NonEmptyRowCount == 0)
                                    //{
                                    //    nFRow = -1;
                                    //    strDosCode = "";
                                    //}
                                    btnOk.Focus();
                                    return;
                                }
                            }
                        }

                        CellClickEventArgs k = new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false);
                        ssFirstDiv_CellClick(ssFirstDiv, k);
                        ssFirstDiv.ActiveSheet.Cells[0, 0, ssFirstDiv.ActiveSheet.ColumnCount - 1, 1].BackColor = Color.White;
                        ssFirstDiv.ActiveSheet.Cells[0, 0, 0, 1].BackColor = Color.Aqua;
                    }
                }
                dtDos.Dispose();
                dtDos = null;
            }
            catch (Exception ex)
            {
                if (dtDos != null)
                {
                    dtDos.Dispose();
                    dtDos = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            btnOk.Focus();

            if (ssSecondDiv.ActiveSheet.NonEmptyRowCount == 0)
            {
                nFRow = -1;
                nSRow = -1;
                nLRow = -1;
            }
        }

        private void ssFirstDiv_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string cBun;
            string cGbDiv;

            clsOrdFunction.GstrSELECTBun = "20";

            if (e.ColumnHeader == true) return;

            if (strBun != "11" && strBun != "12" && strBun != "20")
            {
                cBun = "20";
            }
            else
            {
                cBun = strBun;
            }

            ssFirstDiv.ActiveSheet.Cells[0, 0, ssFirstDiv.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;

            if (clsOrdFunction.GstrLoadFlag != "" || clsOrdFunction.GstrLoadFlag2 != "")
            {
                if (nFRow == -1)
                {
                    ssFirstDiv.ActiveSheet.Cells[e.Row, 0, e.Row, 1].BackColor = Color.Aqua;
                    cGbDiv = VB.Left(ssFirstDiv.ActiveSheet.Cells[e.Row, 2].Text, 2);
                }
                else
                {
                    ssFirstDiv.ActiveSheet.Cells[nFRow, 0, nFRow, 1].BackColor = Color.Aqua;
                    cGbDiv = VB.Left(ssFirstDiv.ActiveSheet.Cells[nFRow, 2].Text, 2);
                }
            }
            else
            {
                ssFirstDiv.ActiveSheet.Cells[e.Row, 0, e.Row, 1].BackColor = Color.Aqua;
                cGbDiv = VB.Left(ssFirstDiv.ActiveSheet.Cells[e.Row, 2].Text, 2);
            }

            if (strGbPrn == "P")    //PRN 처방
            {
                cGbDiv = cGbDiv + "0100";
            }

            SP.Spread_All_Clear(ssSecondDiv);
            SP.Spread_All_Clear(ssFullDosCode);

            try
            {
                SQL = "";
                if (strCallFormName != "Order")
                {   
                    SQL += " SELECT DosFullCode, DOSNAME, GbDiv, DosCode        \r";
                    SQL += "   FROM ADMIN.OCS_ODOSAGE                      \r";
                    SQL += "  WHERE Bun   = '" + cBun + "'                      \r";
                    SQL += "    AND Substr(DosCode,1,2) = '" + cGbDiv + "'      \r";
                    SQL += "    AND Substr(DosCode,3,2)<> '00'                  \r";
                    SQL += "    AND Substr(DosCode,5, 2) = '00'                 \r";
                    if (rdoIpd.Checked == true)
                    {
                        SQL += "    AND GbIO IN ('A','I')                       \r";
                    }
                    else if (rdoOpd.Checked == true)
                    {
                        SQL += "    AND GbIO IN ('A','O')                       \r";
                    }
                    SQL += "    AND (DelDate IS NULL OR DelDate ='')            \r";
                    SQL += "  ORDER BY DosCode                                  \r";
                }
                else
                {
                    if (strGbPrn == "P")    //PRN 처방
                    {
                        SQL += " SELECT DosFullCode, DOSNAME, GbDiv, DosCode    \r";
                        SQL += "   FROM ADMIN.OCS_ODOSAGE                  \r";
                        SQL += "  WHERE Bun   = '" + cBun + "'                  \r";
                        SQL += "    AND DosCode = '" + cGbDiv + "'              \r";
                        if (strGBIO == "I")
                        {
                            SQL += "    AND GbIO IN ('A','I')                   \r";
                        }
                        else
                        {
                            SQL += "    AND GbIO IN ('A','O')                   \r";
                        }
                        SQL += "    AND (DelDate IS NULL OR DelDate ='')        \r";
                        SQL += "  ORDER BY DosCode                              \r";
                    }
                    else
                    {
                        if (clsPublic.GstrPRNChk == "OK")
                        {
                            SQL += " SELECT DosFullCode, DOSNAME, GbDiv, DosCode                                            \r";
                            SQL += "   FROM ADMIN.OCS_ODOSAGE                                                          \r";
                            SQL += "  WHERE Bun   = '" + cBun + "'                                                          \r";
                            SQL += "    AND Substr(DosCode,1,2) = '" + cGbDiv + "'                                          \r";
                            SQL += "    AND Substr(DosCode,3,2)<> '00'                                                      \r";
                            SQL += "    AND Substr(DosCode,5,2) = '00'                                                      \r";
                            SQL += "    AND DosCode NOT IN ( SELECT TRIM(DOSCODE)                                           \r";
                            SQL += "                           FROM ADMIN.OCS_ODOSAGE                                  \r";
                            SQL += "                          WHERE (dosfullcode LIKE '%PRN%' OR dosname  LIKE '%PRN%') )   \r";
                            if (strGBIO == "I")
                            {
                                SQL += "    AND GbIO IN ('A','I')                                                           \r";
                            }
                            else
                            {
                                SQL += "    AND GbIO IN ('A','O')                                                           \r";
                            }
                            SQL += "    AND (DelDate IS NULL OR DelDate ='')                                                \r";
                            SQL += "  ORDER BY DosCode                                                                      \r";
                        }
                        else
                        {
                            SQL += " SELECT DosFullCode, DOSNAME, GbDiv, DosCode    \r";
                            SQL += "   FROM ADMIN.OCS_ODOSAGE                  \r";
                            SQL += "  WHERE Bun   = '" + cBun + "'                  \r";
                            SQL += "    AND Substr(DosCode,1,2) = '" + cGbDiv + "'  \r";
                            SQL += "    AND Substr(DosCode,3,2)<> '00'              \r";
                            SQL += "    AND Substr(DosCode,5,2) = '00'              \r";
                            if (strGBIO == "I")
                            {
                                SQL += "    AND GbIO IN ('A','I')                   \r";
                            }
                            else
                            {
                                SQL += "    AND GbIO IN ('A','O')                   \r";
                            }
                            SQL += "    AND (DelDate IS NULL OR DelDate ='')        \r";
                            SQL += "  ORDER BY DosCode                              \r";
                        }
                    }
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
                    clsDB.DataTableToSpdRow(dt, ssSecondDiv, 0, true);
                    
                    if (strDosCode.Trim() != "")
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (VB.Left(dt.Rows[i]["DosCode"].ToString().Trim(), 4) == VB.Left(strDosCode, 4))
                            {
                                if (clsOrdFunction.GstrLoadFlag != "" || clsOrdFunction.GstrLoadFlag2 != "")
                                {
                                    nSRow = i;
                                    CellClickEventArgs j = new CellClickEventArgs(new SpreadView(),nSRow, 0, nSRow, 0, new MouseButtons(), false, false);
                                    ssSecondDiv_CellClick(sender, j);
                                    ssSecondDiv.ActiveSheet.Cells[0, 0, ssSecondDiv.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                                    ssSecondDiv.ActiveSheet.Cells[nSRow, 0, nSRow, 1].BackColor = Color.Aqua;
                                    //nSRow = -1;
                                    return;
                                }
                                else
                                {
                                    CellClickEventArgs j = new CellClickEventArgs(new SpreadView(), i, 0, i, 0, new MouseButtons(), false, false);
                                    ssSecondDiv_CellClick(sender, j);
                                    ssSecondDiv.ActiveSheet.Cells[i, i, ssSecondDiv.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                                    ssSecondDiv.ActiveSheet.Cells[i, 0, i, 1].BackColor = Color.Aqua;
                                    return;
                                }
                            }
                        }
                        nSRow = 0;
                        ssSecondDiv.ActiveSheet.Cells[0, 0, 0, 1].BackColor = Color.Aqua;
                        CellClickEventArgs k = new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false);
                        ssSecondDiv_CellClick(ssSecondDiv, k);                        
                    }
                    else
                    {
                        ssSecondDiv.ActiveSheet.Cells[0, 0, 0, 1].BackColor = Color.Aqua;
                        CellClickEventArgs k = new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false);
                        ssSecondDiv_CellClick(ssSecondDiv, k);
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
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //btnOk_Click(btnOk, new EventArgs());
            this.Close();
        }

        private void ssSecondDiv_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string cBun;
            string cGbDiv;

            if (e.ColumnHeader == true) return;

            if (strBun != "11" && strBun != "12" && strBun != "20")
            {
                cBun = "20";
            }
            else
            {
                cBun = strBun;
            }

            ssSecondDiv.ActiveSheet.Cells[0, 0, ssSecondDiv.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;

            if (clsOrdFunction.GstrLoadFlag != "" || clsOrdFunction.GstrLoadFlag2 != "")
            {
                if (nSRow == -1)
                {
                    ssSecondDiv.ActiveSheet.Cells[e.Row, 0, e.Row, 1].BackColor = Color.Aqua;
                    cGbDiv = VB.Mid(ssSecondDiv.ActiveSheet.Cells[e.Row, 3].Text, 1, 4);
                }
                else
                {
                    ssSecondDiv.ActiveSheet.Cells[nSRow, 0, nSRow, 1].BackColor = Color.Aqua;
                    cGbDiv = VB.Mid(ssSecondDiv.ActiveSheet.Cells[nSRow, 3].Text, 1, 4);
                }
            }
            else
            {
                ssSecondDiv.ActiveSheet.Cells[e.Row, 0, e.Row, 1].BackColor = Color.Aqua;
                cGbDiv = VB.Mid(ssSecondDiv.ActiveSheet.Cells[e.Row, 3].Text, 1, 4);
            }

            SP.Spread_All_Clear(ssFullDosCode);

            try
            {
                SQL = "";
                if (strCallFormName != "Order")
                {
                    SQL += " SELECT DosFullCode, DOSNAME, GbDiv, DosCode                                            \r";
                    SQL += "   FROM ADMIN.OCS_ODOSAGE                                                          \r";
                    SQL += "  WHERE Bun   = '" + VB.Left(cboBun.Text, 2) + "'                                       \r";
                    SQL += "    AND Substr(DosCode,1,4) = '" + cGbDiv + "'                                          \r";
                    SQL += "    AND Substr(DosCode,5,2) <> '00'                                                     \r";
                    if (rdoIpd.Checked == true)
                    {
                        SQL += "    AND GbIO IN ('A','I')                                                           \r";
                    }
                    else if (rdoOpd.Checked == true)
                    {
                        SQL += "    AND GbIO IN ('A','O')                                                           \r";
                    }
                    SQL += "    AND (DelDate IS NULL OR DelDate ='')                                                \r";
                    if (clsType.User.DeptCode.Equals("PC"))
                    {
                        SQL += "  ORDER BY CASE WHEN WARDCODE IN ('PC', 'AN', 'OP') THEN 0 ELSE 1 END, DosCode                       \r";
                    }
                    else
                    {
                        SQL += "  ORDER BY DosCode                      \r";
                    }
                }
                else
                {
                    if (strGbPrn == "P")    //PRN 처방
                    {
                        SQL += " SELECT DosFullCode, DOSNAME, GbDiv, DosCode                                            \r";
                        SQL += "   FROM ADMIN.OCS_ODOSAGE                                                          \r";
                        SQL += "  WHERE Bun   = '" + cBun + "'                                                          \r";
                        SQL += "    AND Substr(DosCode,1,4) = '" + cGbDiv + "'                                          \r";
                        SQL += "    AND Substr(DosCode,5,2) = '20'                                                      \r";
                        if (strGBIO == "I")
                        {
                            SQL += "    AND GbIO IN ('A','I')                                                           \r";
                        }
                        else
                        {
                            SQL += "    AND GbIO IN ('A','O')                                                           \r";
                        }
                        SQL += "    AND (DelDate IS NULL OR DelDate ='')                                                \r";
                        SQL += "  ORDER BY DosCode                                                                      \r";
                    }
                    else
                    {
                        SQL += " SELECT DosFullCode, DOSNAME, GbDiv, DosCode                                            \r";
                        SQL += "   FROM ADMIN.OCS_ODOSAGE                                                          \r";
                        SQL += "  WHERE Bun   = '" + cBun + "'                                                          \r";
                        SQL += "    AND Substr(DosCode,1,4) = '" + cGbDiv + "'                                          \r";
                        SQL += "    AND Substr(DosCode,5,2)<> '00'                                                      \r";
                        if (strGBIO == "I")
                        {
                            SQL += "    AND GbIO IN ('A','I')                                                           \r";
                        }
                        else
                        {
                            SQL += "    AND GbIO IN ('A','O')                                                           \r";
                        }
                        SQL += "    AND (DelDate IS NULL OR DelDate ='')                                                \r";
                        //최선택 과장 요구로 품 : 2019-01-22
                        if (strGBIO == "I")
                        {
                            if (clsPublic.GstrPRNChk == "OK")
                            {
                                SQL += "    AND DosCode NOT IN ( '030107' )                                                 \r";
                            }
                        }

                        if (clsType.User.DeptCode.Equals("PC"))
                        {
                            SQL += "  ORDER BY CASE WHEN WARDCODE IN ('PC', 'AN', 'OP') THEN 0 ELSE 1 END, DosCode                       \r";
                        }
                        else
                        {
                            SQL += "  ORDER BY DosCode                      \r";
                        }
                    }
                }
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt1, ssFullDosCode, 0, true);

                    if (strDosCode.Trim() != "")
                    {
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (dt1.Rows[i]["DosCode"].ToString().Trim() == strDosCode)
                            {
                                if (clsOrdFunction.GstrLoadFlag != "" || clsOrdFunction.GstrLoadFlag2 != "")
                                {
                                    nLRow = i;
                                    CellClickEventArgs j = new CellClickEventArgs(new SpreadView(), 0, nLRow, 0, nLRow, new MouseButtons(), false, false);
                                    ssFullDosCode.ActiveSheet.Cells[nLRow, 0, nLRow, 1].BackColor = Color.Aqua;
                                    ssFullDosCode_CellClick(sender, j);
                                    return;
                                }
                                else
                                {
                                    CellClickEventArgs k = new CellClickEventArgs(new SpreadView(), 0, i, 0, i, new MouseButtons(), false, false);
                                    ssFullDosCode.ActiveSheet.Cells[i, 0, i, 1].BackColor = Color.Aqua;
                                    ssFullDosCode_CellClick(sender, k);
                                    return;
                                }
                            }
                        }
                        nLRow = 0;
                        CellClickEventArgs click = new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false);
                        ssFullDosCode.ActiveSheet.Cells[0, 0, 0, 1].BackColor = Color.Aqua;
                        ssFullDosCode_CellClick(sender, click);
                    }
                    else
                    {
                        CellClickEventArgs click = new CellClickEventArgs(new SpreadView(), 0, 0, 0, 0, new MouseButtons(), false, false);
                        ssFullDosCode.ActiveSheet.Cells[0, 0, 0, 1].BackColor = Color.Aqua;
                        ssFullDosCode_CellClick(sender, click);
                    }
                }
                dt1.Dispose();
                dt1 = null;
            }
            catch (Exception ex)
            {
                if (dt1 != null)
                {
                    dt1.Dispose();
                    dt1 = null;
                }

                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssFullDosCode_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (ssFullDosCode.ActiveSheet.NonEmptyRowCount == 0)
            {
                if (clsOrdFunction.GstrLoadFlag != "" || clsOrdFunction.GstrLoadFlag2 != "")                
                {
                    if (nLRow == -1)
                    {
                        lblDosCode.Text = ssSecondDiv.ActiveSheet.Cells[e.Row, 3].Text + "." + ssSecondDiv.ActiveSheet.Cells[nLRow, 0].Text;
                        ssSecondDiv.ActiveSheet.Cells[0, 0, ssFullDosCode.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                        ssSecondDiv.ActiveSheet.Cells[e.Row, 0, nLRow, 1].BackColor = Color.Aqua;
                    }
                    else
                    {
                        lblDosCode.Text = ssSecondDiv.ActiveSheet.Cells[nLRow, 3].Text + "." + ssSecondDiv.ActiveSheet.Cells[nLRow, 0].Text;
                        ssSecondDiv.ActiveSheet.Cells[0, 0, ssFullDosCode.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                        ssSecondDiv.ActiveSheet.Cells[nLRow, 0, nLRow, 1].BackColor = Color.Aqua;
                    }
                 
                }
                else
                {
                    lblDosCode.Text = ssSecondDiv.ActiveSheet.Cells[e.Row, 3].Text + "." + ssSecondDiv.ActiveSheet.Cells[e.Row, 0].Text;
                    ssSecondDiv.ActiveSheet.Cells[0, 0, ssFullDosCode.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                    ssSecondDiv.ActiveSheet.Cells[e.Row, 0, e.Row, 1].BackColor = Color.Aqua;
                }
            }
            else
            {
                if (clsOrdFunction.GstrLoadFlag != "" || clsOrdFunction.GstrLoadFlag2 != "")
                {
                    if (nLRow == -1)
                    {
                        lblDosCode.Text = ssFullDosCode.ActiveSheet.Cells[e.Row, 3].Text + "." + ssFullDosCode.ActiveSheet.Cells[e.Row, 1].Text;
                        ssFullDosCode.ActiveSheet.Cells[0, 0, ssFullDosCode.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                        ssFullDosCode.ActiveSheet.Cells[e.Row, 0, e.Row, 1].BackColor = Color.Aqua;
                    }
                    else
                    {
                        lblDosCode.Text = ssFullDosCode.ActiveSheet.Cells[nLRow, 3].Text + "." + ssFullDosCode.ActiveSheet.Cells[nLRow, 1].Text;
                        ssFullDosCode.ActiveSheet.Cells[0, 0, ssFullDosCode.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                        ssFullDosCode.ActiveSheet.Cells[nLRow, 0, nLRow, 1].BackColor = Color.Aqua;
                    }
                }
                else
                {
                    lblDosCode.Text = ssFullDosCode.ActiveSheet.Cells[e.Row, 3].Text + "." + ssFullDosCode.ActiveSheet.Cells[e.Row, 1].Text;
                    ssFullDosCode.ActiveSheet.Cells[0, 0, ssFullDosCode.ActiveSheet.RowCount - 1, 1].BackColor = Color.White;
                    ssFullDosCode.ActiveSheet.Cells[e.Row, 0, e.Row, 1].BackColor = Color.Aqua;
                }
            }
            clsOrdFunction.GstrLoadFlag = "";
            clsOrdFunction.GstrLoadFlag2 = "";
            strDosCode = "";
        }

        private void ssFullDosCode_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            ssFullDosCode.ActiveSheet.Cells[0, 0, ssFullDosCode.ActiveSheet.RowCount - 1, ssFullDosCode.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            ssFullDosCode.ActiveSheet.Cells[e.Row, 0, e.Row, ssFullDosCode.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;

            if (strCallFormName == "Frm약품식별회신서2015")
            {
                ssDosCodeOpdDoubleClick(ssFullDosCode.ActiveSheet.Cells[e.Row, 2].Text, "", "", 0, "");
            }
            else
            {
                btnOk_Click(btnOk, new EventArgs());
            }
        }

        private void ssFullDosCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (strCallFormName == "Frm약품식별회신서2015")
                {
                    ssDosCodeOpdDoubleClick(ssFullDosCode.ActiveSheet.Cells[ssFullDosCode.ActiveSheet.ActiveRowIndex, 2].Text, "", "", 0, "");
                }
                else
                {
                    btnOk_Click(btnOk, new EventArgs());
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string strFullCode;
            string strDosCode;
            string strDiv;
            string strGubun;

            bool blnSelect = false;
            int nFSelRow = 0;
            int nSSelRow = 0;
            int nLSelRow = 0;

            int nFRow = 0;
            int nSRow = 0;
            int nLRow = 0;

            nFRow = ssFirstDiv.ActiveSheet.ActiveRowIndex;
            nSRow = ssSecondDiv.ActiveSheet.ActiveRowIndex;
            nLRow = ssFullDosCode.ActiveSheet.ActiveRowIndex;

            if (rdoSelect.Checked == true)
            {
                strGubun = "S";
            }
            else
            {
                strGubun = "A";
            }

            for (int i = 0; i < ssFullDosCode.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssFullDosCode.ActiveSheet.Cells[i, 1].BackColor == Color.Aqua)
                {
                    nLSelRow = i;
                    blnSelect = true;
                    break;
                }
            }

            if (blnSelect == false)
            {
                for (int i = 0; i < ssSecondDiv.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssSecondDiv.ActiveSheet.Cells[i, 0].BackColor == Color.Aqua)
                    {
                        nSSelRow = i;
                        blnSelect = true;
                        break;
                    }
                }
            }

            if (blnSelect == false)
            {
                for (int i = 0; i < ssFirstDiv.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssFirstDiv.ActiveSheet.Cells[i, 0].BackColor == Color.Aqua)
                    {
                        nFSelRow = i;
                        blnSelect = true;
                        break;
                    }
                }
            }

            if (ssSecondDiv.ActiveSheet.RowCount == 0)
            {
                //ssFirstDiv.ActiveSheet.Cells[ssFirstDiv.ActiveSheet.ActiveRowIndex, 0, ssFirstDiv.ActiveSheet.ActiveRowIndex, ssFirstDiv.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;

                //strFullCode = ssFirstDiv.ActiveSheet.Cells[ssFirstDiv.ActiveSheet.ActiveRowIndex, 1].Text.Trim();
                //strDosCode = ssFirstDiv.ActiveSheet.Cells[ssFirstDiv.ActiveSheet.ActiveRowIndex, 2].Text.Trim();
                //strDiv = ssFirstDiv.ActiveSheet.Cells[ssFirstDiv.ActiveSheet.ActiveRowIndex, 3].Text.Trim();

                ssFirstDiv.ActiveSheet.Cells[nFSelRow, 0, nFSelRow, ssFirstDiv.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;

                strFullCode = ssFirstDiv.ActiveSheet.Cells[nFSelRow, 1].Text.Trim();
                strDosCode = ssFirstDiv.ActiveSheet.Cells[nFSelRow, 2].Text.Trim();
                strDiv = ssFirstDiv.ActiveSheet.Cells[nFSelRow, 3].Text.Trim();

                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    ssDosCodeOpdDoubleClick(strFullCode, strDosCode, strDiv, ssFirstDiv.ActiveSheet.ActiveRowIndex, strGubun);
                }
                else if (clsOrdFunction.GstrGbJob == "IPD")
                {
                    ssDosCodeIpdDoubleClick(strFullCode, strDosCode, strDiv, ssFirstDiv.ActiveSheet.ActiveRowIndex, strGubun);
                }
                else if (clsOrdFunction.GstrGbJob == "ER")
                {
                    ssDosCodeErDoubleClick(strFullCode, strDosCode, strDiv, ssFirstDiv.ActiveSheet.ActiveRowIndex, strGubun);
                }

                this.Close();
                return;
            }

            //ssFullDosCode.ActiveSheet.Cells[ssFullDosCode.ActiveSheet.ActiveRowIndex, 0, ssFullDosCode.ActiveSheet.ActiveRowIndex, ssFullDosCode.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;

            //strFullCode = ssFullDosCode.ActiveSheet.Cells[ssFullDosCode.ActiveSheet.ActiveRowIndex, 1].Text.Trim();
            //strDosCode = ssFullDosCode.ActiveSheet.Cells[ssFullDosCode.ActiveSheet.ActiveRowIndex, 3].Text.Trim();
            //strDiv = ssFullDosCode.ActiveSheet.Cells[ssFullDosCode.ActiveSheet.ActiveRowIndex, 2].Text.Trim();

            ssFullDosCode.ActiveSheet.Cells[nLSelRow, 0, nLSelRow, ssFullDosCode.ActiveSheet.ColumnCount - 1].BackColor = Color.Aqua;

            strFullCode = ssFullDosCode.ActiveSheet.Cells[nLSelRow, 1].Text.Trim();
            strDosCode = ssFullDosCode.ActiveSheet.Cells[nLSelRow, 3].Text.Trim();
            strDiv = ssFullDosCode.ActiveSheet.Cells[nLSelRow, 2].Text.Trim();

            if (strCallFormName == "Frm약품식별회신서2015")
            {
                //ssDosCodeOpdDoubleClick(ssFullDosCode.ActiveSheet.Cells[ssFullDosCode.ActiveSheet.ActiveRowIndex, 3].Text, "", "", 0, "");
                ssDosCodeOpdDoubleClick(ssFullDosCode.ActiveSheet.Cells[nLSelRow, 3].Text, "", "", 0, "");
            }
            else
            {
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    ssDosCodeOpdDoubleClick(strFullCode, strDosCode, strDiv, ssFullDosCode.ActiveSheet.ActiveRowIndex, strGubun);
                }
                else if (clsOrdFunction.GstrGbJob == "IPD")
                {
                    ssDosCodeIpdDoubleClick(strFullCode, strDosCode, strDiv, ssFullDosCode.ActiveSheet.ActiveRowIndex, strGubun);
                }
                else if (clsOrdFunction.GstrGbJob == "ER")
                {
                    ssDosCodeErDoubleClick(strFullCode, strDosCode, strDiv, ssFullDosCode.ActiveSheet.ActiveRowIndex, strGubun);
                }
            }
            this.Close();
        }
    }
}
