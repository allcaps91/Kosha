using ComBase;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace ComLibB
{
    public partial class FrmMedViewSlips : Form
    {
        FrmMedViewSlipSub FrmMedViewSlipSubEvent = null;

        clsOrdFunction OF = new clsOrdFunction();
        clsSpread SP = new clsSpread();
        Network nw = new Network();

        
        //clsAntiMed am = new clsAntiMed();

        //FarPoint.Win.Spread.CellType.TextCellType TxtCType = new FarPoint.Win.Spread.CellType.TextCellType();
        //FarPoint.Win.Spread.CellType.ComboBoxCellType combo = new FarPoint.Win.Spread.CellType.ComboBoxCellType();

        DataTable dt = null;
        DataTable dt1 = null;
        DataTable dt2 = null;
        string SqlErr = ""; //에러문 받는 변수
        int intRowAffected = 0; //변경된 Row 받는 변수

        string SQL;
        long rowcounter;

        TreeNode RootNode;//, SubNode, LastNode;
        string OrdName;
        string KeyVal;
        string strCallPath;
        string strFindText;
        string strSlipName;
        FarPoint.Win.Spread.FpSpread ssOrdSpread = null;

        string sNodeName = "";

        private int mintMonitor = 0;
        private int[] mintWidth = null;
        private int[] mintHeight = null;

        //OPD

        public delegate void Spd_DoubleClick(string OrderCode, string SlipNo);
        public event Spd_DoubleClick ssOrderCodeDoubleClick;

        public delegate void Spread_Check(string OrderCode, string SlipNo);
        public event Spread_Check ssOrderCodeCheck;

        public delegate void XrayDup_Check(string OrderCode);
        public event XrayDup_Check ssOrderXrayDupCodeCheck;

        public delegate void SpreadOrderDup_Check(string OrderCode, string OrdName, string strOK);
        public event SpreadOrderDup_Check OrderDup_Check;

        //ER

        public delegate void Spd_ErDoubleClick(string OrderCode, string SlipNo);
        public event Spd_ErDoubleClick ssErOrderCodeDoubleClick;

        public delegate void SpdEr_SODoubleClick(string OrderCode, string SlipNo, string strOrderName);
        public event SpdEr_SODoubleClick ssErOrderCodeSODoubleClick;

        public delegate void Spread_ErCheck(string OrderCode, string SlipNo);
        public event Spread_ErCheck ssErOrderCodeCheck;

        public delegate void XrayDup_ErCheck(string OrderCode);
        public event XrayDup_ErCheck ssOrderXrayDupCodeErCheck;

        public delegate void SpreadOrderDup_ErCheck(string OrderCode, string OrdName, string strOK);
        public event SpreadOrderDup_ErCheck OrderDup_ErCheck;

        //IPD

        public delegate void Spd_IpdDoubleClick(string OrderCode, string SlipNo);
        public event Spd_IpdDoubleClick ssIpdOrderCodeDoubleClick;

        public delegate void SpdIpd_SODoubleClick(string OrderCode, string SlipNo, string strOrderName);
        public event SpdIpd_SODoubleClick ssIpdOrderCodeSODoubleClick;

        public delegate void Spread_IpdCheck(string OrderCode, string SlipNo);
        public event Spread_IpdCheck ssIpdOrderCodeCheck;

        public delegate void XrayDup_IpdCheck(string OrderCode);
        public event XrayDup_IpdCheck ssOrderXrayDupCodeIpdCheck;

        public delegate void SpreadOrderDup_IpdCheck(string OrderCode, string OrdName, string strOK);
        public event SpreadOrderDup_IpdCheck OrderDup_IpdCheck;



        FarPoint.Win.Spread.CellType.TextCellType txt = new FarPoint.Win.Spread.CellType.TextCellType();

        //public FrmMedViewSlips()
        //{
        //    InitializeComponent();
        //}

        public FrmMedViewSlips(string sCallPath, string sFindText, string sSlipName,
            FarPoint.Win.Spread.FpSpread ssSpread)
        {
            InitializeComponent();
            strFindText = sFindText;
            strCallPath = sCallPath;
            strSlipName = sSlipName;
            ssOrdSpread = ssSpread;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            OF.Dispose(); OF = null;
            SP.Dispose(); SP = null;
            nw.Dispose(); nw = null;

            if(dt != null)
            {
                dt.Dispose();
                dt = null;
            }
            if (dt1 != null)
            {
                dt1.Dispose();
                dt1 = null;
            }
            if (dt2 != null)
            {
                dt2.Dispose();
                dt2 = null;
            }

            txt = null;
            //DataTable dt = null;
            //DataTable dt1 = null;
            //DataTable dt2 = null;
            this.Close(); this.Dispose();
        }

        

        private void trvSlipNo_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            clsOrdFunction.GstrSelSlipno = "";

            SP.Spread_All_Clear(ssOrderCode);

            lblSlipName.Text = "";

            if (e.Node.Level == 0)
            {
                Cursor.Current = Cursors.WaitCursor;

                if (e.Node.Nodes.Count == 0)
                {
                    try
                    {
                        SQL = "";
                        SQL += " SELECT SLIPNO FROM " + ComNum.DB_MED + "ocs_ordercode      \r";
                        SQL += "  WHERE ORDERNAME = '" + e.Node.Text.Trim() + "'            \r";
                        SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                        if (SqlErr != "")
                        {
                            ComFunc.MsgBox("조회중 오류가 발생했습니다");
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            return;
                        }

                        if (dt.Rows.Count > 0)
                        {
                            clsOrdFunction.GstrSelSlipno = dt.Rows[0]["SLIPNO"].ToString().Trim();

                            //fn_Set_Check(clsOrdFunction.GstrSelSlipno);
                            Sel_Slip();
                        }
                        else
                        {
                            clsOrdFunction.GstrSelSlipno = e.Node.Text;
                            //fn_Set_Check(clsOrdFunction.GstrSelSlipno);
                            Sel_Slip();
                        }
                        Cursor.Current = Cursors.No;
                        dt.Dispose();
                        dt = null;
                    }

                    catch (Exception ex)
                    {
                        ComFunc.MsgBox(ex.Message);
                        clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                    }
                }
                else
                {
                    SP.Spread_All_Clear(ssOrderCode);
                }
            }
            else if (e.Node.Level == 2)
            {
                try
                {
                    SQL = "";
                    SQL += " SELECT SLIPNO FROM " + ComNum.DB_MED + "ocs_ordercode      \r";
                    SQL += "  WHERE ORDERNAME = '" + e.Node.Text.Trim() + "'            \r";
                    SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        ComFunc.MsgBox("조회중 오류가 발생했습니다");
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                        return;
                    }
                    if (dt.Rows.Count == 0)
                    {
                        dt.Dispose();
                        dt = null;
                        return;
                    }

                    rowcounter = 0;
                    rowcounter = dt.Rows.Count;

                    if (rowcounter > 0)
                    {
                        clsOrdFunction.GstrSelSlipno = dt.Rows[0]["SLIPNO"].ToString();
                        Sel_Slip();
                    }
                    else
                    {
                        clsOrdFunction.GstrSelSlipno = e.Node.Text;
                        Sel_Slip();
                    }
                    Cursor.Current = Cursors.No;
                    dt.Dispose();
                    dt = null;
                }

                catch (Exception ex)
                {
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장`
                }
            }
            if (e != null)
            {
                lblSlipName.Text = e.Node.Text + "[" + clsOrdFunction.GstrSelSlipno.Trim() + "]";
                sNodeName = e.Node.Text;
            }

            if (e.Node.Text == "V/S, Bed Rest")
            {
                if (ssOrdSpread.Name != "ssOpdOrder")
                {
                    FrmMedViewVital Vital = new FrmMedViewVital(ssOrdSpread, true, "A1");
                    Vital.ShowDialog(this);
                    OF.fn_ClearMemory(Vital);
                }
            }
        }

        private void FrmMedViewSlips_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) //폼 권한 조회
            {
                this.Close();
                return;
            }
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등            

            this.Location = new Point(365, 60);

            cboSelect.Items.Clear();
            cboSelect.Items.Add("1.개인코드(상용)");
            cboSelect.Items.Add("2.과코드(상용)");
            cboSelect.Items.Add("3.전체코드");

            if (strFindText != "")
            {
                btnInput.Enabled = false;
                btnPersonInput.Enabled = false;
                btnDeptInput.Enabled = false;
                btnDelete.Enabled = false;
            }
            else
            {
                btnInput.Enabled = true;
                btnPersonInput.Enabled = true;
                btnDeptInput.Enabled = true;
                btnDelete.Enabled = true;
            }

            //개인별 환경설정 로직 추가 할 것(개인/과/전체 로드)
            if (clsOrdFunction.GEnvSet_Item02 != "")
            {
                cboSelect.SelectedIndex = int.Parse(clsOrdFunction.GEnvSet_Item02) - 1;
            }
            else
            {
                cboSelect.SelectedIndex = 2;
            }

            //2018.07.27 임시 막음
            //clsOrdFunction.GstrBDate = clsPublic.GstrSysDate;

            ssOrderCode_Sheet1.Cells[0, 5, (int)ssOrderCode.ActiveSheet.RowCount - 1, 5].CellType = new FarPoint.Win.Spread.CellType.RichTextCellType();

            TreView_Refresh();
             
            if (strFindText != "")
            {
                txtSearch.Text = strFindText;
                btnView_Click(sender, e);
            }
            else
            {
                txtSearch.Text = "";
            }

            if (strSlipName != "")
            {
                for (int i = 0; i < (int)trvSlipNo.Nodes.Count; i++)
                {
                    if (trvSlipNo.Nodes[i].Text == strSlipName)
                    {
                        trvSlipNo.Select();
                        //trvSlipNo.Nodes[i].Expand();
                        TreeNodeMouseClickEventArgs TreeE = new TreeNodeMouseClickEventArgs(trvSlipNo.Nodes[i], MouseButtons.Left, 1, 1, 1);
                        trvSlipNo_NodeMouseClick(trvSlipNo.Nodes[i].Text, TreeE);
                        break;
                    }
                }
            }           

            if (strCallPath == "SetDept")
            {
                for (int i = 0; i < (int)trvSlipNo.Nodes.Count; i++)
                {
                    if (trvSlipNo.Nodes[i].Text == clsVbfunc.GetBASClinicDeptNameK(clsDB.DbCon, clsPublic.GstrDeptCode))
                    {   
                        //trvSlipNo.Nodes[i].Expand();
                        trvSlipNo.Select();
                        TreeNodeMouseClickEventArgs TreeE = new TreeNodeMouseClickEventArgs(trvSlipNo.Nodes[i], MouseButtons.Left, 1, 1, 1);
                        trvSlipNo_NodeMouseClick(trvSlipNo.Nodes[i].Text, TreeE);
                        //trvSlipNo.Update();
                        break;
                    }
                }                
            }

            SetFormLocation();
        }

        private void SetFormLocation()
        {
            GetMonitorInfo();

            if (mintWidth[0] < 1500)
            {
                this.Top = 20;
                this.Left = 60;
                //this.StartPosition = FormStartPosition.CenterParent;
            }
        }
        
        private void GetMonitorInfo()
        {
            Screen[] screens = Screen.AllScreens;

            mintMonitor = screens.Length;
            mintWidth = new int[mintMonitor];
            mintHeight = new int[mintMonitor];

            int i = 0;
            foreach (Screen screen in screens)
            {
                mintWidth[i] = screen.Bounds.Width;
                mintHeight[i] = screen.Bounds.Height;
                i = i + 1;
            }
        }

        private void TreView_Refresh()
        {
            int nRow;

            try
            {
                SQL = "";
                SQL += " SELECT OrderName, DispHeader, Slipno               \r";
                SQL += "   FROM " + ComNum.DB_MED + "OCS_ORDERCODE          \r";
                SQL += "  WHERE Seqno = 0                                   \r";
                if (clsVbfunc.GetIpAddressOeacle(clsDB.DbCon) == "192.168.2.76" ||
                    clsVbfunc.GetIpAddressOeacle(clsDB.DbCon) == "192.168.9.41" ||
                    clsVbfunc.GetIpAddressOeacle(clsDB.DbCon) == "192.168.10.87")  //전산실, 가정간호 
                {
                    SQL += "    AND (Slipno < 'A' Or Slipno = '" + clsPublic.GstrDeptCode.Trim() + "' OR SLIPNO ='A6' OR SLIPNO = 'A5'  \r"; //처치수술"
                }
                else
                {
                    //SQL += "    AND (Slipno < 'A' Or Slipno = '" + clsPublic.GstrDeptCode.Trim() + "' OR SLIPNO ='A6' )    \r";
                    SQL += "    AND (SUBSTR(Slipno,1,1) < 'B' Or  Slipno = '" + clsPublic.GstrDeptCode.Trim() + "' OR SLIPNO IN('A1','A2','A3','A4','A5','A6' ) )   \r";
                }

                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    SQL += "    AND SlipNo   NOT IN ('A1','A2','A4')                    \r";
                }


                if (clsPublic.GstrDeptCode == "OC")
                {
                    SQL += "    AND SlipNo   IN ('0105')                                \r";
                }
                else
                {
                    if (clsPublic.GstrDeptCode == "PC" || clsPublic.GstrDeptCode == "AN")
                    {
                        SQL += "    AND SlipNo   NOT IN ('0105','0106','0075')              \r";
                    }
                    else
                    {
                        SQL += "    AND SlipNo   NOT IN ('0105','0106','0074','0075')       \r";
                    }
                }

                //SQL += "    AND slipno <> '0001'                            \r";
                SQL += "    AND slipno NOT IN('0001', 'A7')                             \r";
                SQL += "    and ordername is not null                                   \r";
                //SQL += "    and nal <> '99'                                 \r";
                SQL += "  ORDER BY NAL, SLIPNO                                          \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                nRow = (int)rowcounter;

                trvSlipNo.ImageList = new ImageList();
                trvSlipNo.ImageList.TransparentColor = System.Drawing.Color.Transparent;

                if (rowcounter != 0)
                {
                    this.trvSlipNo.BeginUpdate();

                    for (int i = 0; i < nRow; i++)
                    {
                        OrdName = dt.Rows[i]["OrderName"].ToString();
                        KeyVal = dt.Rows[i]["Slipno"].ToString();

                        if (File.Exists(dt.Rows[i]["DispHeader"].ToString().Trim()) == true)
                        {
                            trvSlipNo.ImageList.Images.Add(Image.FromFile(dt.Rows[i]["DispHeader"].ToString().Trim()));
                        }

                        RootNode = new TreeNode(OrdName, i, i);
                        this.trvSlipNo.Nodes.Add(RootNode);
                    }
                    this.trvSlipNo.EndUpdate();
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void ssOrderCode_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true)
            {
                SP.setSpdSort(ssOrderCode, e.Column, true);
                return;
            }

            if (e.Button == MouseButtons.Right)
            {
                //ssOrderCode.ActiveSheet.Cells[0, 0, ssOrderCode.ActiveSheet.RowCount - 1, (int)ssOrderCode_Sheet1.ColumnCount - 1].BackColor = Color.White;
                this.Close();
            }
            else
            {
                //if (ssOrderCode.ActiveSheet.Cells[e.Row, 0, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor != Color.LightCyan)
                //{
                //    ssOrderCode.ActiveSheet.Cells[e.Row, 0, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.LightCyan;
                //}
                //else
                //{
                //    ssOrderCode.ActiveSheet.Cells[e.Row, 0, e.Row, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                //}

                lblSlipName.Text = ssOrderCode.ActiveSheet.Cells[e.Row, 1].Text + "(" + ssOrderCode.ActiveSheet.Cells[e.Row, 15].Text + ")";

                //clsOrdFunction.GstrBunExamInfo = ssOrderCode.ActiveSheet.Cells[e.Row, 7].Text;
                //clsOrdFunction.GstrOrderExamInfo = ssOrderCode.ActiveSheet.Cells[e.Row, 3].Text;
            }
        }

        private void SetSpcOrder(int Row, int Col)
        {
            string GstrGubun = "";

            if (ssOrdSpread.Name == "ssIpdOrder") { GstrGubun = "IPD"; }
            else if (ssOrdSpread.Name == "ssErOrder") { GstrGubun = "ER"; }

            int nRow = 0;
            nRow = ssOrdSpread.ActiveSheet.NonEmptyRowCount;

            ssOrdSpread.ActiveSheet.ActiveRowIndex = nRow;
            ssOrdSpread.ActiveSheet.ActiveColumnIndex = 1;

            //if (clsOrdFunction.GnActiveRow != 0)
            //{
            //    if (nActiveRow == clsOrdFunction.GnActiveRow)
            //    {
            //        ssOrdSpread.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
            //        ssOrdSpread.ActiveSheet.ActiveColumnIndex = 1;
            //        nActiveRow = 0;
            //    }
            //    else
            //    {
            //        ssOrdSpread.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow + 1;
            //        ssOrdSpread.ActiveSheet.AddRows(ssOrdSpread.ActiveSheet.ActiveRowIndex, 1);
            //        ssOrdSpread.ActiveSheet.ActiveRowIndex = clsOrdFunction.GnActiveRow;
            //        ssOrdSpread.ActiveSheet.ActiveColumnIndex = 1;
            //    }
            //}
            //else
            //{
            //    ssOrdSpread.ActiveSheet.ActiveRowIndex = ssOrdSpread.ActiveSheet.NonEmptyRowCount;
            //}

            if (clsOrdFunction.GstrSelSlipno.Trim() == "A1")
            {

            }
            else if (clsOrdFunction.GstrSelSlipno.Trim() == "A2" || clsOrdFunction.GstrSelSlipno.Trim() == "A4")
            {
                if (GstrGubun == "IPD")
                {
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.ORDERCODE].Text = "S/O";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.NAMEENG].Text = ssOrderCode_Sheet1.Cells[Row, 4].Text.Trim();
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.CONTENTS].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.BCONTENTS].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.REALQTY].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.DIV].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.NAL].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.NGT].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.ER].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.SELF].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.PRN].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.POTABLE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.SUCODE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.BUN].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.SLIPNO].Text = clsOrdFunction.GstrSelSlipno;
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(clsOrdFunction.GstrSelSlipno, "", ""), 7);
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.SLIPGUBUN].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(clsOrdFunction.GstrSelSlipno, "", ""), 3).Trim();
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.QTY].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.DOSAGE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.GBBOTH].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.GBINFO].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.REMARK].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.DISPRGB].Text = "80";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.CGBBOTH].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.CGBINFO].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.GBQTY].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.GBDOSAGE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.NEXTCODE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.GBIMIV].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.ORDERNO].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.ROWID].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.GBACT].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.SEQNO].Text = nRow.ToString();
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.IpdOrderCol.ORDERGUBUN, nRow, ssOrdSpread.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                }
                else
                {
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.ORDERCODE].Text = "S/O";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.NAMEENG].Text = ssOrderCode_Sheet1.Cells[Row, 4].Text.Trim();
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.CONTENTS].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.BCONTENTS].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.REALQTY].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.DIV].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.NAL].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.NGT].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBER].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.SELF].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBREMARK].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.POTABLE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.SUCODE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.BUN].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.SLIPNO].Text = clsOrdFunction.GstrSelSlipno;
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(clsOrdFunction.GstrSelSlipno, "", ""), 7);
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.SORT].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(clsOrdFunction.GstrSelSlipno, "", ""), 3).Trim();
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.QTY].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.DOSCODE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBINFO].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.REMARK].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.DISPRGB].Text = "80";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBBOTH1].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBINFO1].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBQTY].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBDOSAGE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.NEXTCODE].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBIMIV].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.ORDERNO].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.ROWID].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.GBACT].Text = "";
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.SEQNO].Text = nRow.ToString();
                    ssOrdSpread.ActiveSheet.Cells[nRow, (int)BaseOrderInfo.ErOrderCol.ORDERGUBUN, nRow, ssOrdSpread.ActiveSheet.ColumnCount - 1].ForeColor = Color.Brown;
                }
            }
        }

        private void ssOrderCode_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strBun = "";
            string strCBun = "";
            string strNextCode = "";
            int nRow = 0;
            string strOrderName = "";

            if (ssOrderCode_Sheet1.RowCount == 0) return;

            if (e.ColumnHeader == true)
            {   
                return;
            }

            if (ssOrderCode_Sheet1.Cells[e.Row, 4].Text.Trim() == "") return;

            //if (clsOrdFunction.GstrSelSlipno.Trim() == "A1" || clsOrdFunction.GstrSelSlipno.Trim() == "A2" || clsOrdFunction.GstrSelSlipno.Trim() == "A4")
            if (clsOrdFunction.GstrSelSlipno == null)
            {
                clsOrdFunction.GstrSelSlipno = "";
            }

            //if (clsOrdFunction.GstrSelSlipno.Trim() == "A2" || clsOrdFunction.GstrSelSlipno.Trim() == "A4")
            //{
            //    if (ssOrdSpread.Name != "ssOpdOrder")
            //    {
            //        SetSpcOrder(e.Row, 1);
            //        return;
            //    }
            //}

            clsOrdFunction.GstrOrderSelect = "ORD";

            clsOrdFunction.GstrSELECTOrderCode = ssOrderCode_Sheet1.Cells[e.Row, 2].Text.Trim();
            clsOrdFunction.GstrselOrderCode = ssOrderCode_Sheet1.Cells[e.Row, 2].Text.Trim();
            clsOrdFunction.GstrOrderCode = ssOrderCode_Sheet1.Cells[e.Row, 2].Text.Trim();
            clsOrdFunction.GstrOrderName = ssOrderCode_Sheet1.Cells[e.Row, 4].Text.Trim();
            clsOrdFunction.GstrSelSlipno = ssOrderCode_Sheet1.Cells[e.Row, 15].Text.Trim();
            clsOrdFunction.GstrSubRate = ssOrderCode_Sheet1.Cells[e.Row, 19].Text.Trim();
            clsOrdFunction.GdosLoadLocation = "Order";
            clsOrdFunction.GstrSubRate = "";

            if (clsOrdFunction.GstrSelSlipno.Trim() == "A2" || clsOrdFunction.GstrSelSlipno.Trim() == "A4")
            {
                if (ssOrdSpread.Name != "ssOpdOrder")
                {
                    SetSpcOrder(e.Row, 1);
                    return;
                }
            }

            //2021-02-08 추가, 전산의뢰<2021-41>
            //타과에서 전문재활치료 처방 못넣도록..
            //clsPublic.GstrDeptCode
            if (clsOrdFunction.GstrSelSlipno.Trim() == "0101" && clsPublic.GstrDeptCode != "RM") 
            {
                ComFunc.MsgBox("전문재활치료 처방은 재활의학과에서만 처방 가능합니다!!");
                return;
            }

            try
            {
                if (ssOrderCode.ActiveSheet.Cells[e.Row, 19].Text.Trim() != "")
                {
                    clsOrdFunction.GstrSubRate = ssOrderCode.ActiveSheet.Cells[e.Row, 19].Text.Trim();
                    FrmMedViewSlipSubEvent = new FrmMedViewSlipSub(clsOrdFunction.GstrSelSlipno, clsOrdFunction.GstrSubRate);
                    FrmMedViewSlipSubEvent.OrdSubCodeDoubleClick -= FrmMedViewSlipSubEvent_OrdSubCodeDoubleClick;
                    FrmMedViewSlipSubEvent.OrdSubCodeDoubleClick += FrmMedViewSlipSubEvent_OrdSubCodeDoubleClick;
                    FrmMedViewSlipSubEvent.ShowDialog(this);
                    OF.fn_ClearMemory(FrmMedViewSlipSubEvent);
                    return;
                }

                //입력불가 GBINPUT(1 : 입력가능)
                if (clsOrdFunction.GstrSelSlipno.Trim() != "A1" && clsOrdFunction.GstrSelSlipno.Trim() != "A2" &&
                    clsOrdFunction.GstrSelSlipno.Trim() != "A3" && clsOrdFunction.GstrSelSlipno.Trim() != "A4")
                {
                    if (ssOrderCode.ActiveSheet.Cells[e.Row, 6].Text.Trim() != "1")
                    {
                        MessageBox.Show("해당 처방은 처방입력이 불가합니다!!!", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }

                strBun = ssOrderCode.ActiveSheet.Cells[e.Row, 9].Text;
                strCBun = ssOrderCode.ActiveSheet.Cells[e.Row, 23].Text;
                strNextCode = ssOrderCode.ActiveSheet.Cells[e.Row, 10].Text;

                if (strNextCode.Trim() == "")
                {
                    for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssOrderCode.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            if (ssOrderCode.ActiveSheet.Cells[i, 10].Text == strNextCode.Trim())
                            {
                                nRow = 2;
                                strOrderName = ssOrderCode.ActiveSheet.Cells[i, 4].Text.Trim();
                                break;
                            }
                        }
                    }

                    if (nRow == 2)
                    {
                        for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
                        {
                            if (ssOrderCode.ActiveSheet.Cells[i, 0].Text == "True")
                            {
                                if (ssOrderCode.ActiveSheet.Cells[i, 10].Text.Trim() == strNextCode.Trim())
                                {
                                    nRow = 2;
                                    strOrderName = ssOrderCode.ActiveSheet.Cells[i, 4].Text;
                                    break;
                                }
                            }
                        }
                    }
                }

                for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssOrderCode.ActiveSheet.Cells[i, 0].Text != "True")
                    {
                        if (strNextCode.Trim() == ssOrderCode.ActiveSheet.Cells[i, 10].Text.Trim())
                        {
                            nRow = 2;
                            strOrderName = ssOrderCode.ActiveSheet.Cells[i, 4].Text;
                            break;
                        }
                    }
                }

                if (nRow != 2)
                {
                    for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
                    {
                        if (ssOrderCode.ActiveSheet.ActiveRowIndex != i && ssOrderCode.ActiveSheet.Cells[i, 0].Text != "True")
                        {
                            if (ssOrderCode.ActiveSheet.Cells[i, 10].Text.Trim() == strNextCode.Trim())
                            {
                                nRow = 2;
                                strOrderName = ssOrderCode.ActiveSheet.Cells[i, 4].Text.Trim();
                                break;
                            }
                        }
                    }
                }

                if (nRow == 1 && dt.Rows[0]["SLIPNO"].ToString().Trim() == "0102")
                {
                    string cPT_PtNo = "";

                    //PT 치료중 확인
                    if (e.Row == 0) return;

                    //????????
                    cPT_PtNo = clsOrdFunction.Pat.PtNo;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("****1****" + ex.Message);
            }

            try
            {
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    ssOrderCodeDoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim());
                }
                else if (clsOrdFunction.GstrGbJob == "ER")
                {
                    if (clsOrdFunction.GstrOrderCode.Trim() == "S/O")
                    {
                        if (ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text.Trim() == "") return;

                        ssErOrderCodeSODoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim(), clsOrdFunction.GstrOrderName);
                    }
                    else
                    {
                        if (ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text.Trim() == "") return;

                        ssErOrderCodeDoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim());
                    }
                }
                else if (clsOrdFunction.GstrGbJob == "IPD")
                {
                    if (clsOrdFunction.GstrOrderCode.Trim() == "S/O")
                    {
                        if (ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text.Trim() == "") return;

                        ssIpdOrderCodeSODoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim(), clsOrdFunction.GstrOrderName);
                    }
                    else
                    {
                        if (ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text.Trim() == "") return;

                        ssIpdOrderCodeDoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim());
                    }
                }

                if (ssOrderCode.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor != Color.FromArgb(255, 255, 234))
                {
                    ssOrderCode.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
                }

                ssOrderCode.ActiveSheet.Cells[e.Row, 9].Text = ""; //추가 검사 및 금액 Clear
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox("****2****" + ex.Message);
            }
            
        }

        private void FrmMedViewSlipSubEvent_OrdSubCodeDoubleClick(string OrderCode, string SlipNo)
        {
            if (clsOrdFunction.GstrGbJob == "OPD")
            {
                ssOrderCodeDoubleClick(OrderCode, SlipNo);
            }
            else if (clsOrdFunction.GstrGbJob == "ER")
            {
                ssErOrderCodeDoubleClick(OrderCode, SlipNo);
            }
            else if (clsOrdFunction.GstrGbJob == "IPD")
            {
                ssIpdOrderCodeDoubleClick(OrderCode, SlipNo);
            }
        }

        //void FrmViewBoth_ssXrayBothDoubleClick(string strSuCode, string strGbInfo)
        //{
        //    ssOrderCode.ActiveSheet.Cells[ssOrderCode.ActiveSheet.ActiveRowIndex, 11].Text = strGbInfo;
        //}

        //private void ssOrderCode_TextTipFetch(object sender, FarPoint.Win.Spread.TextTipFetchEventArgs e)
        //{
        //    if (e.Column != 4) return;
            
        //    e.ShowTip = true;
        //    e.TipText = "" + (char)13 + (char)10;
        //    e.TipText += ssOrderCode_Sheet1.Cells[e.Row, 4].Text.Trim();
        //    e.View.TextTipAppearance.Font = new Font("굴림체", 10, FontStyle.Bold);
        //    e.View.TextTipAppearance.BackColor = Color.FromArgb(255, 225, 225);
        //    e.View.TextTipAppearance.ForeColor = Color.Black;
        //}

        private void rdoName_Click(object sender, EventArgs e)
        {
            RDOSearch_ColorSet(rdoName);
            txtSearch.ImeMode = ImeMode.Alpha;
        }

        private void RDOSearch_ColorSet(RadioButton Button_Name)
        {   
            rdoName.BackColor = Color.White;
            rdoSuNameG.BackColor = Color.White;
            rdoOrderCode.BackColor = Color.White;
            rdoBohumCode.BackColor = Color.White;

            if (Button_Name.Checked == true)
            {
                Button_Name.BackColor = Color.LightSteelBlue;
            }
            else
            {
                Button_Name.BackColor = Color.White;
            }
            txtSearch.Focus();
        }

        private void rdoSuNameG_Click(object sender, EventArgs e)
        {
            RDOSearch_ColorSet(rdoSuNameG);
            txtSearch.ImeMode = ImeMode.Alpha;
        }

        private void rdoOrderCode_Click(object sender, EventArgs e)
        {
            RDOSearch_ColorSet(rdoOrderCode);
            txtSearch.ImeMode = ImeMode.Alpha;
        }

        private void rdoBohumCode_Click(object sender, EventArgs e)
        {
            RDOSearch_ColorSet(rdoBohumCode);
            txtSearch.ImeMode = ImeMode.Alpha;
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            string strSearch = "";
            //string PATIENTNAME;

            SP.Spread_All_Clear(ssOrderCode);

            if (clsOrdFunction.GstrSlipSearch == "Y")
            {
                if (txtSlipSearch.Text.Trim().Length < 2)
                {
                    MessageBox.Show("검색하실 문자를 두글자 이상 입력하세요!!!  ");
                    txtSlipSearch.Focus();
                    ComFunc.StartLen(txtSlipSearch);
                    return;
                }
                strSearch = txtSlipSearch.Text.Trim().ToUpper();
            }
            else
            {
                if (txtSearch.Text.Trim().Length < 2)
                {
                    MessageBox.Show("검색하실 문자를 두글자 이상 입력하세요!!!  ");
                    txtSearch.Focus();
                    ComFunc.StartLen(txtSearch);
                    return;
                }
                strSearch = txtSearch.Text.Trim().ToUpper();
            }

            try
            {
                #region //이전쿼리
                //SQL = "";
                //SQL += " select '', max(slipname) slipname, ordercode, '', max(ordername) ordername                     \r";
                //SQL += "      , max(remark) remark, max(gbinput) gbinput, max(gbinfo) gbinfo                            \r";
                //SQL += "      , max(gbboth) gbboth, max(Bun) Bun, max(NEXTCODE) NEXTCODE                                \r";
                //SQL += "      , max(SUCODE) SUCODE, max(info) info, max(Speccode) Speccode, max(GbDosage) GbDosage      \r";
                //SQL += "      , max(Slipno) Slipno, max(SPECNAME) SPECNAME, max(GbImiv) GbImiv, max(INPUTSEQ) INPUTSEQ  \r";
                //SQL += "      , max(SubRate) SubRate, max(SENDDEPT) SENDDEPT, max(temp1) temp1, max(temp2) temp2        \r";
                //SQL += "      , max(CBUN) CBUN, max(DELOUTSTAT) DELOUTSTAT                                              \r";
                //SQL += "  from(                                                                                         \r";
                //SQL += "       SELECT ''                                                                                \r";
                //SQL += "           , (select ordername                                                                  \r";
                //SQL += "                from KOSMOS_OCS.OCS_ORDERCODE                                                   \r";
                //SQL += "               where slipno = a.slipno                                                          \r";
                //SQL += "                 and seqno = 0 ) slipname                                                       \r";
                //SQL += "          , a.OrderCode, ''                                                                     \r";                
                //SQL += "          , decode(case when a.bun = '11' then '11'                                             \r";
                //SQL += "                        when a.bun = '12' then '12'                                             \r";
                //SQL += "                        when a.bun = '14' then '14'                                             \r";
                //SQL += "                        when a.bun = '15' then '15'                                             \r";
                //SQL += "                        when a.bun = '74' then '74'                                             \r";
                //SQL += "                        when a.bun = '04' then '04'                                             \r";
                //SQL += "                        when a.bun >= '16' and a.bun <= '21' then '20'                          \r";
                //SQL += "                   else ''                                                                      \r";
                ////SQL += "                    end , '', a.ordername, RPAD(NVL(a.ordername, '                    ') , 20) || a.ordernames) ordername \r";
                //SQL += "                    end , '', a.ordername, NVL(a.ordername, '                    ') || a.ordernames) ordername \r";
                //SQL += "          , '' remark                                                                           \r";
                //SQL += "          , a.GbInput,   a.GBINFO,    a.GBBOTH                                                  \r";
                //SQL += "          , a.Bun,       a.NEXTCODE,  A.SUCODE,      '' INFO                                    \r";
                //SQL += "          , a.Speccode,  a.GbDosage,  a.Slipno,      '' SPECNAME                                \r";
                //SQL += "          , a.GbImiv,    '' INPUTSEQ, a.SubRate,     SENDDEPT                                   \r";
                //SQL += "          , '' temp1, '' temp2, a.CBUN                                                          \r";
                //SQL += "          , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, a.CBUN) DELOUTSTAT                              \r";
                //SQL += "       FROM kosmos_ocs.ocs_ordercode a                                                          \r";
                //SQL += "          , kosmos_pmpa.bas_sut      b                                                          \r";
                //SQL += "          , kosmos_pmpa.bas_sun      c                                                          \r";
                //SQL += "      WHERE a.SuCode = b.SuCode(+)                                                              \r";
                //SQL += "        AND a.SuCode = c.SuNext(+)                                                              \r";
                //SQL += "        AND a.Seqno <> 0                                                                        \r";
                //SQL += "        AND (B.DELDATE IS NULL OR B.DELDATE > TRUNC(SYSDATE))                                   \r";
                //if (clsPublic.GstrIpAddress != "192.168.2.15")
                //{
                //    SQL += "        AND RTRIM(a.SendDept) IS NULL                                                       \r";
                //}
                //SQL += "        AND a.SuCode = b.SuCode(+)                                                              \r";
                //SQL += "        AND(b.SugbJ <> '2' OR b.SugbJ IS NULL)                                                  \r";
                //SQL += "        AND a.SLIPNO NOT IN ('0106', 'MD')                                                      \r";    //자가약 제외
                //SQL += "        AND (a.GBSUB <> '1' OR a.GBSUB IS NULL)                                                 \r";
                //SQL += "        AND a.SLIPNO NOT IN(select trim(deptcode) from kosmos_pmpa.bas_clinicdept where gbjupsu = '1') \r"; //과처방 제외
                //SQL += "        AND(upper(a.ordercode)  like '" + "%" + strSearch + "%" + "'                            \r";
                //SQL += "         or upper(c.sunameg)    like '" + "%" + strSearch + "%" + "'                            \r";
                //SQL += "         or upper(a.ordername)  like '" + "%" + strSearch + "%" + "'                            \r";
                //SQL += "         or upper(a.ordernames) like '" + "%" + strSearch + "%" + "'                            \r";
                //SQL += "         or upper(a.drugName)   like '" + "%" + strSearch + "%" + "')                           \r";
                //if (clsOrdFunction.GstrGbJob == "OPD")
                //{
                //    SQL += "        AND a.SLIPNO NOT IN('A1', 'A2', 'A4')                                               \r";
                //}

                //if(clsOrdFunction.GstrSlipSearch == "Y")
                //{
                //    SQL += "        AND a.SLIPNO = '" + clsOrdFunction.GstrSelSlipno.Trim() + "'                        \r";
                //}
                ////SQL += "        AND a.BUN NOT IN('11', '12', '20')                                                      \r";
                //SQL += "      UNION ALL                                                                                 \r";
                //SQL += "     SELECT ''                                                                                  \r";
                //SQL += "          , (select ordername                                                                   \r";
                //SQL += "               from KOSMOS_OCS.OCS_ORDERCODE                                                    \r";
                //SQL += "              where slipno = (select slipno from kosmos_ocs.ocs_ordercode where ordercode = a.sunext)\r";
                //SQL += "                and seqno = 0 ) slipname                                                        \r";
                //SQL += "          , a.sunext, ''                                                                        \r";
                //SQL += "          , RPAD(NVL(a.UNIT, '                    ') , 20) || a.HNAME ordername                 \r";
                //SQL += "          , '' remark                                                                           \r";
                //SQL += "          , '1' GbInput,  '' GBINFO,   '' GBBOTH                                                \r";
                //SQL += "          , (select bun from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) Bun           \r";
                //SQL += "          , '' NEXTCODE,  A.sunext,      '' INFO                                                \r";
                //SQL += "          , (select SPECCODE from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) Speccode \r";
                //SQL += "          , (select GbDosage from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) GbDosage \r";
                //SQL += "          , (select SLIPNO from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) Slipno     \r";
                //SQL += "          , '' SPECNAME                                                                         \r";
                //SQL += "          , (select GBIMIV from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) GbImiv     \r";
                //SQL += "          , '' INPUTSEQ                                                                         \r";
                //SQL += "          , (select SUBRATE from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) SubRate   \r";
                //SQL += "          , (select SENDDEPT from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) SENDDEPT \r";
                //SQL += "          , '' temp1, '' temp2, (select CBUN from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) CBUN \r";
                //SQL += "          , KOSMOS_OCS.FC_DRUG_OUTCHK(a.SUNEXT, (select CBUN from kosmos_ocs.ocs_ordercode where ordercode = a.sunext)) DELOUTSTAT  \r";
                //SQL += "       FROM KOSMOS_OCS.OCS_DRUGINFO_new  A                                                      \r";
                //SQL += "          , KOSMOS_ADM.DRUG_JEP          B                                                      \r";
                //SQL += "      WHERE A.SUNEXT = B.JEPCODE(+)                                                             \r";
                //SQL += "        AND B.DELDATE IS NULL                                                                   \r";
                //if (clsOrdFunction.GstrSlipSearch == "Y")
                //{
                //    SQL += "    AND (select slipno from kosmos_ocs.ocs_ordercode where ordercode = a.sunext             \r";
                //    SQL += "                and seqno = 0 ) = '" + clsOrdFunction.GstrSelSlipno.Trim() + "'             \r";
                //}
                //SQL += "        AND (upper(A.SName) LIKE '" + "%" + strSearch + "%" + "'                                \r";
                //SQL += "         or upper(A.SuNext) LIKE '" + "%" + strSearch + "%" + "'                                \r";
                //SQL += "         or upper(A.HName) LIKE '" + "%" + strSearch + "%" + "'                                 \r";
                //SQL += "         or upper(A.ENAME) LIKE '" + "%" + strSearch + "%" + "')                                \r";
                //SQL += "      )                                                                                         \r";                
                //SQL += "   group by ordercode                                                                           \r";
                //SQL += "   order by slipno, ordercode                                                                   \r";
                #endregion
                
                
                // 2019-01-19 쿼리변경
                SQL = "";
                SQL += " select '', max(slipname) slipname, ordercode, '', max(ordername) ordername                     \r";
                SQL += "      , max(remark) remark, max(gbinput) gbinput, max(gbinfo) gbinfo                            \r";
                SQL += "      , max(gbboth) gbboth, max(Bun) Bun, max(NEXTCODE) NEXTCODE                                \r";
                SQL += "      , max(SUCODE) SUCODE, max(info) info, max(Speccode) Speccode, max(GbDosage) GbDosage      \r";
                SQL += "      , max(Slipno) Slipno, max(SPECNAME) SPECNAME, max(GbImiv) GbImiv, max(INPUTSEQ) INPUTSEQ  \r";
                SQL += "      , max(SubRate) SubRate, max(SENDDEPT) SENDDEPT, max(temp1) temp1, max(temp2) temp2        \r";
                SQL += "      , max(CBUN) CBUN, max(DELOUTSTAT) DELOUTSTAT                                              \r";
                SQL += "  from(                                                                                         \r";
                SQL += "       SELECT ''                                                                                \r";
                SQL += "           , (select ordername                                                                  \r";
                SQL += "                from KOSMOS_OCS.OCS_ORDERCODE                                                   \r";
                SQL += "               where slipno = a.slipno                                                          \r";
                SQL += "                 and seqno = 0 ) slipname                                                       \r";
                SQL += "          , a.OrderCode, ''                                                                     \r";
                SQL += "          , decode(case when a.bun = '11' then '11'                                             \r";
                SQL += "                        when a.bun = '12' then '12'                                             \r";
                SQL += "                        when a.bun = '14' then '14'                                             \r";
                SQL += "                        when a.bun = '15' then '15'                                             \r";
                SQL += "                        when a.bun = '74' then '74'                                             \r";
                SQL += "                        when a.bun = '04' then '04'                                             \r";
                SQL += "                        when a.bun >= '16' and a.bun <= '21' then '20'                          \r";
                SQL += "                   else ''                                                                      \r";                
                SQL += "                    end , '', a.ordername, NVL(a.ordername, '                    ') || a.ordernames) ordername \r";
                SQL += "          , '' remark                                                                           \r";
                SQL += "          , a.GbInput,   a.GBINFO,    a.GBBOTH                                                  \r";
                SQL += "          , a.Bun,       a.NEXTCODE,  A.SUCODE,      '' INFO                                    \r";
                SQL += "          , a.Speccode,  a.GbDosage,  a.Slipno,      '' SPECNAME                                \r";
                SQL += "          , a.GbImiv,    '' INPUTSEQ, a.SubRate,     SENDDEPT                                   \r";
                SQL += "          , '' temp1, '' temp2, a.CBUN                                                          \r";
                SQL += "          , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, a.CBUN) DELOUTSTAT                              \r";
                SQL += "       FROM kosmos_ocs.ocs_ordercode a                                                          \r";
                SQL += "       LEFT OUTER JOIN kosmos_pmpa.bas_sut b                                                    \r";
                SQL += "         ON a.SuCode = b.SuCode                                                                 \r";
                SQL += "        AND (B.DELDATE IS NULL OR B.DELDATE > TRUNC(SYSDATE))                                   \r";
                SQL += "        AND (b.SugbJ <> '2' OR b.SugbJ IS NULL)                                                 \r";
                SQL += "       LEFT OUTER JOIN kosmos_pmpa.bas_sun c                                                    \r";
                SQL += "         ON a.SuCode = c.SuNext                                                                 \r";                
                SQL += "      WHERE a.Seqno <> 0                                                                        \r";                
                if (clsPublic.GstrIpAddress != "192.168.2.15")
                {
                    SQL += "        AND RTRIM(a.SendDept) IS NULL                                                       \r";
                }
                if (clsPublic.GstrDeptCode == "PC" || clsPublic.GstrDeptCode == "AN")
                {
                    SQL += "    AND a.SlipNo   NOT IN ('0106', 'MD', '0105','0106','0075')              \r";
                }
                else if (clsPublic.GstrDeptCode == "ER")    //전산업무의뢰서 2019-1295
                {
                    SQL += "    AND a.SlipNo   NOT IN ('0106', 'MD', '0105','0106','0074','0075','0045')       \r";
                }
                else
                {
                    SQL += "    AND a.SlipNo   NOT IN ('0106', 'MD', '0105','0106','0074','0075')       \r";
                }
                //SQL += "        AND a.SLIPNO NOT IN ('0106', 'MD')                                                      \r";    //자가약 제외
                SQL += "        AND (a.GBSUB <> '1' OR a.GBSUB IS NULL)                                                 \r";
                SQL += "        AND a.SLIPNO NOT IN(select trim(deptcode) from kosmos_pmpa.bas_clinicdept where gbjupsu = '1') \r"; //과처방 제외
                SQL += "        AND(upper(a.ordercode)  like '" + "%" + strSearch + "%" + "'                            \r";
                SQL += "         or upper(c.sunameg)    like '" + "%" + strSearch + "%" + "'                            \r";
                SQL += "         or upper(a.ordername)  like '" + "%" + strSearch + "%" + "'                            \r";
                SQL += "         or upper(a.ordernames) like '" + "%" + strSearch + "%" + "'                            \r";
                SQL += "         or upper(a.drugName)   like '" + "%" + strSearch + "%" + "')                           \r";
                if (clsOrdFunction.GstrGbJob == "OPD")
                {
                    SQL += "        AND a.SLIPNO NOT IN('A1', 'A2', 'A4')                                               \r";
                }

                if (clsOrdFunction.GstrSlipSearch == "Y")
                {
                    SQL += "        AND a.SLIPNO = '" + clsOrdFunction.GstrSelSlipno.Trim() + "'                        \r";
                }                
                SQL += "     UNION ALL                                                                                 \r";
                SQL += "     SELECT ''                                                                                  \r";
                SQL += "          , (select ordername                                                                   \r";
                SQL += "               from KOSMOS_OCS.OCS_ORDERCODE                                                    \r";
                SQL += "              where slipno = (select slipno from kosmos_ocs.ocs_ordercode where ordercode = a.sunext)\r";
                SQL += "                and seqno = 0 ) slipname                                                        \r";
                SQL += "          , a.sunext, ''                                                                        \r";
                SQL += "          , RPAD(NVL(a.UNIT, '                    ') , 20) || a.HNAME ordername                 \r";
                SQL += "          , '' remark                                                                           \r";
                SQL += "          , '1' GbInput,  '' GBINFO,   '' GBBOTH                                                \r";
                SQL += "          , (select bun from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) Bun           \r";
                SQL += "          , '' NEXTCODE,  A.sunext,      '' INFO                                                \r";
                SQL += "          , (select SPECCODE from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) Speccode \r";
                SQL += "          , (select GbDosage from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) GbDosage \r";
                SQL += "          , (select SLIPNO from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) Slipno     \r";
                SQL += "          , '' SPECNAME                                                                         \r";
                SQL += "          , (select GBIMIV from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) GbImiv     \r";
                SQL += "          , '' INPUTSEQ                                                                         \r";
                SQL += "          , (select SUBRATE from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) SubRate   \r";
                SQL += "          , (select SENDDEPT from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) SENDDEPT \r";
                SQL += "          , '' temp1, '' temp2, (select CBUN from kosmos_ocs.ocs_ordercode where ordercode = a.sunext) CBUN \r";
                SQL += "          , KOSMOS_OCS.FC_DRUG_OUTCHK(a.SUNEXT, (select CBUN from kosmos_ocs.ocs_ordercode where ordercode = a.sunext)) DELOUTSTAT  \r";
                SQL += "       FROM KOSMOS_OCS.OCS_DRUGINFO_new A                                                       \r";
                SQL += "       LEFT OUTER JOIN KOSMOS_ADM.DRUG_JEP B                                                    \r";
                SQL += "         ON A.SUNEXT = B.JEPCODE                                                                \r";
                SQL += "      WHERE B.DELDATE IS NULL                                                                   \r";                
                if (clsOrdFunction.GstrSlipSearch == "Y")
                {
                    SQL += "    AND (select slipno from kosmos_ocs.ocs_ordercode where ordercode = a.sunext             \r";
                    SQL += "                and seqno = 0 ) = '" + clsOrdFunction.GstrSelSlipno.Trim() + "'             \r";
                }
                SQL += "        AND (upper(A.SName) LIKE '" + "%" + strSearch + "%" + "'                                \r";
                SQL += "         or upper(A.SuNext) LIKE '" + "%" + strSearch + "%" + "'                                \r";
                SQL += "         or upper(A.HName) LIKE '" + "%" + strSearch + "%" + "'                                 \r";
                SQL += "         or upper(A.ENAME) LIKE '" + "%" + strSearch + "%" + "')                                \r";
                SQL += "      )                                                                                         \r";
                SQL += "   WHERE SLIPNAME IS NOT NULL                                                                   \r";
                SQL += "   group by ordercode                                                                           \r";
                SQL += "   order by slipno, ordercode                                                                   \r";
                

                SqlErr = clsDB.GetDataTableEx(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count > 0)
                {
                    clsDB.DataTableToSpdRow(dt, ssOrderCode, 0, true);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void Read_Order(string sFromWord, string sToWord, string sGubun)
        {
            SP.Spread_All_Clear(ssOrderCode);

            try
            {
                if (cboSelect.SelectedIndex == 2)
                {
                    SQL = "";
                    SQL += " SELECT ''                                                                      \r";
                    SQL += "      , (select ordername                                                       \r";
                    SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                    SQL += "          where slipno = b.slipno                                               \r";
                    SQL += "            and seqno = 0 ) slipname                                            \r";
                    SQL += "      , b.OrderCode, ''                                                         \r";
                    SQL += "      , decode(case when b.bun = '11' then '11'                                 \r";
                    SQL += "                    when b.bun = '12' then '12'                                 \r";
                    SQL += "                    when b.bun = '14' then '14'                                 \r";
                    SQL += "                    when b.bun = '15' then '15'                                 \r";
                    SQL += "                    when b.bun = '74' then '74'                                 \r";
                    SQL += "                    when b.bun = '04' then '04'                                 \r";
                    SQL += "                    when b.bun >= '16' and b.bun <= '21' then '20'              \r";
                    SQL += "               else ''                                                          \r";
                    SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                    SQL += "      , '' REMARK                                                               \r";
                    SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                    SQL += "      , b.Bun,       B.NEXTCODE,  B.SUCODE,      '' INFO                        \r";
                    SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                    SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     SENDDEPT                       \r";
                    SQL += "      , '', '', B.CBUN                                                          \r";
                    SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                    SQL += "   FROM " + ComNum.DB_MED + "OCS_ORDERCODE  B                                   \r";
                    SQL += "      , " + ComNum.DB_PMPA + "BAS_SUT       C                                   \r";
                    SQL += "      , " + ComNum.DB_PMPA + "BAS_SUN       D                                   \r";
                    SQL += "  WHERE 1=1                                                                     \r";
                    SQL += "    AND B.Seqno    <> 0                                                         \r";
                    SQL += "    AND RTRIM(B.SendDept) IS NULL                                               \r"; //사용하지않는 코드 제외
                    SQL += "    AND B.SuCode = C.SuCode(+)                                                  \r";
                    SQL += "    AND B.SuCode = D.SuNEXT(+)                                                  \r";
                    SQL += "    AND (C.SugbJ <> '2' OR C.SugbJ IS NULL)                                     \r";
                    SQL += "    AND (C.DELDATE IS NULL OR C.DELDATE > TRUNC(SYSDATE))                       \r";
                    if (sFromWord != "ALL")
                    {
                        if (rdoName.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(b.ORDERNAME)) >= '" + sFromWord + '%' + "'              \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND ((UPPER(TRIM(b.ORDERNAME)) >= '" + sFromWord + '%' + "'         \r";
                                    SQL += "    AND  UPPER(TRIM(b.ORDERNAME)) < '" + sToWord + '%' + "')            \r";
                                    SQL += "     or (UPPER(TRIM(b.ORDERNAMES)) >= '" + sFromWord + '%' + "'         \r";
                                    SQL += "    AND  UPPER(TRIM(b.ORDERNAMES)) < '" + sToWord + '%' + "')           \r";
                                    SQL += "     or (UPPER(TRIM(b.DRUGNAME)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND  UPPER(TRIM(b.DRUGNAME)) < '" + sToWord + '%' + "')  )          \r";
                                }
                                else
                                {
                                    SQL += "    AND (UPPER(TRIM(b.ORDERNAME)) LIKE '" + sFromWord + '%' + "'        \r";
                                    SQL += "     or UPPER(TRIM(b.ORDERNAMES)) LIKE '" + sFromWord + '%' + "'        \r";
                                    SQL += "     or UPPER(TRIM(b.DRUGNAME)) LIKE '" + sFromWord + '%' + "')         \r";
                                }
                                SQL += "  ORDER BY b.ORDERNAME                                                      \r";
                            }
                        }
                        if (rdoSuNameG.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + sFromWord + '%' + "'                \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + sFromWord + '%' + "'             \r";
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) < '" + sToWord + '%' + "'                \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) LIKE '" + sFromWord + '%' + "'           \r";
                                }
                            }                        
                            SQL += "  ORDER BY D.SUNAMEG                                                            \r";
                        }
                        if (rdoOrderCode.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'              \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) < '" + sToWord + '%' + "'              \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) <= '" + sToWord + '%' + "'             \r";
                                }
                                SQL += "  ORDER BY b.ORDERCODE                                                      \r";
                                
                            }
                        }
                        if (rdoBohumCode.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'                   \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'               \r";
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) < '" + sToWord + '%' + "'                  \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'               \r";
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) <= '" + sToWord + '%' + "'                 \r";
                                }
                                SQL += "  ORDER BY d.BCODE                                                          \r";
                            }
                        }
                    }
                }
                else if (cboSelect.SelectedIndex == 0)
                {
                    SQL = "";
                    SQL += " SELECT ''                                                                      \r";
                    SQL += "      , (select ordername                                                       \r";
                    SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                    SQL += "          where slipno = b.slipno                                               \r";
                    SQL += "            and seqno = 0 ) slipname                                            \r";
                    SQL += "      , b.OrderCode, ''                                                         \r";
                    SQL += "      , decode(case when b.bun = '11' then '11'                                 \r";
                    SQL += "                    when b.bun = '12' then '12'                                 \r";
                    SQL += "                    when b.bun = '14' then '14'                                 \r";
                    SQL += "                    when b.bun = '15' then '15'                                 \r";
                    SQL += "                    when b.bun = '74' then '74'                                 \r";
                    SQL += "                    when b.bun = '04' then '04'                                 \r";
                    SQL += "                    when b.bun >= '16' and b.cbun <= '21' then '20'             \r";
                    SQL += "               else ''                                                          \r";
                    SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                    SQL += "      , '' remark                                                               \r";
                    SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                    SQL += "      , b.Bun,       B.NEXTCODE,  B.SUCODE,      '' INFO                        \r";
                    SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                    SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     SENDDEPT                       \r";
                    SQL += "      , '', '', B.CBUN                                                          \r";
                    SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                    SQL += "   FROM " + ComNum.DB_MED + "OCS_OPARMDEF  A                                    \r";
                    SQL += "      , " + ComNum.DB_MED + "OCS_ORDERCODE B                                    \r";
                    SQL += "      , " + ComNum.DB_PMPA + "BAS_SUT      C                                    \r";
                    SQL += "      , " + ComNum.DB_PMPA + "BAS_SUN      D                                    \r";
                    SQL += "  WHERE 1=1                                                                     \r";
                    SQL += "    AND A.OrderCode = B.OrderCode                                               \r";
                    SQL += "    AND B.Seqno    <> 0                                                         \r";
                    SQL += "    AND RTRIM(B.SendDept) IS NULL                                               \r"; //사용하지않는 코드 제외
                    SQL += "    AND B.SuCode = C.SuCode(+)                                                  \r";
                    SQL += "    AND B.SuCode = D.SuNEXT(+)                                                  \r";
                    SQL += "    AND (C.SugbJ <> '2' OR C.SugbJ IS NULL)                                     \r";
                    SQL += "    AND (C.DELDATE IS NULL OR C.DELDATE > TRUNC(SYSDATE))                       \r";
                    SQL += "    AND A.DEPTDR = '" + clsOrdFunction.Pat.DeptCode + "'                        \r";
                    if (sFromWord != "ALL")
                    {
                        if (rdoName.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(b.ORDERNAME)) >= '" + sFromWord + '%' + "'              \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND ((UPPER(TRIM(b.ORDERNAME)) >= '" + sFromWord + '%' + "'         \r";
                                    SQL += "    AND  UPPER(TRIM(b.ORDERNAME)) < '" + sToWord + '%' + "')            \r";
                                    SQL += "     or (UPPER(TRIM(b.ORDERNAMES)) >= '" + sFromWord + '%' + "'         \r";
                                    SQL += "    AND  UPPER(TRIM(b.ORDERNAMES)) < '" + sToWord + '%' + "')           \r";
                                    SQL += "     or (UPPER(TRIM(b.DRUGNAME)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND  UPPER(TRIM(b.DRUGNAME)) < '" + sToWord + '%' + "')  )          \r";
                                }
                                else
                                {
                                    SQL += "    AND (UPPER(TRIM(b.ORDERNAME)) LIKE '" + sFromWord + '%' + "'        \r";
                                    SQL += "     or UPPER(TRIM(b.ORDERNAMES)) LIKE '" + sFromWord + '%' + "'        \r";
                                    SQL += "     or UPPER(TRIM(b.DRUGNAME)) LIKE '" + sFromWord + '%' + "')         \r";
                                }
                                SQL += "  ORDER BY b.ORDERNAME                                                      \r";
                            }
                        }
                        if (rdoSuNameG.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + sFromWord + '%' + "'                \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + sFromWord + '%' + "'             \r";
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) < '" + sToWord + '%' + "'                \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) LIKE '" + sFromWord + '%' + "'           \r";
                                }
                            }
                            SQL += "  ORDER BY D.SUNAMEG                                                            \r";
                        }
                        if (rdoOrderCode.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'                \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) < '" + sToWord + '%' + "'              \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) <= '" + sToWord + '%' + "'             \r";
                                }
                                SQL += "  ORDER BY b.ORDERCODE                                                      \r";

                            }
                        }
                        if (rdoBohumCode.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'                \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'               \r";
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) < '" + sToWord + '%' + "'                  \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'               \r";
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) <= '" + sToWord + '%' + "'                 \r";
                                }
                                SQL += "  ORDER BY d.BCODE                                                          \r";
                            }
                        }
                    }
                }
                else if (cboSelect.SelectedIndex == 1)
                {
                    SQL = "";
                    SQL += " SELECT ''                                                                      \r";
                    SQL += "      , (select ordername                                                       \r";
                    SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                    SQL += "          where slipno = b.slipno                                               \r";
                    SQL += "            and seqno = 0 ) slipname                                            \r";
                    SQL += "      , b.OrderCode, ''                                                         \r";
                    SQL += "      , decode(case when b.bun = '11' then '11'                                 \r";
                    SQL += "                    when b.bun = '12' then '12'                                 \r";
                    SQL += "                    when b.bun = '14' then '14'                                 \r";
                    SQL += "                    when b.bun = '15' then '15'                                 \r";
                    SQL += "                    when b.bun = '74' then '74'                                 \r";
                    SQL += "                    when b.bun = '04' then '04'                                 \r";
                    SQL += "                    when b.bun >= '16' and b.bun <= '21' then '20'             \r";
                    SQL += "               else ''                                                          \r";
                    SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                    SQL += "      , '' remark                                                               \r";              
                    SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                    SQL += "      , b.Bun,       B.NEXTCODE,  B.SUCODE,      '' INFO                        \r";
                    SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                    SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     SENDDEPT                       \r";
                    SQL += "      , '', '', B.CBUN                                                          \r";
                    SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                    SQL += "   FROM " + ComNum.DB_MED + "OCS_OPARMDEF  A                                    \r";
                    SQL += "      , " + ComNum.DB_MED + "OCS_ORDERCODE B                                    \r";
                    SQL += "      , " + ComNum.DB_PMPA + "BAS_SUT      C                                    \r";
                    SQL += "      , " + ComNum.DB_PMPA + "BAS_SUN      D                                    \r";
                    SQL += "  WHERE 1=1                                                                     \r";
                    SQL += "    AND A.OrderCode = B.OrderCode                                               \r";
                    SQL += "    AND B.Seqno    <> 0                                                         \r";
                    //SQL += "    AND B.Slipno||'' = '" + clsOrdFunction.GstrSelSlipno + "'                   \r";
                    SQL += "    AND RTRIM(B.SendDept) IS NULL                                               \r"; //사용하지않는 코드 제외
                    SQL += "    AND B.SuCode = C.SuCode(+)                                                  \r";
                    SQL += "    AND B.SuCode = D.SuNEXT(+)                                                  \r";
                    SQL += "    AND (C.SugbJ <> '2' OR C.SugbJ IS NULL)                                     \r";
                    SQL += "    AND (C.DELDATE IS NULL OR C.DELDATE > TRUNC(SYSDATE))                       \r";
                    SQL += "    AND A.DEPTDR = '" + clsOrdFunction.Pat.DrCode + "'                          \r";
                    if (sFromWord != "ALL")
                    {
                        if (rdoName.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(b.ORDERNAME)) >= '" + sFromWord + '%' + "'              \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND ((UPPER(TRIM(b.ORDERNAME)) >= '" + sFromWord + '%' + "'         \r";
                                    SQL += "    AND  UPPER(TRIM(b.ORDERNAME)) < '" + sToWord + '%' + "')            \r";
                                    SQL += "     or (UPPER(TRIM(b.ORDERNAMES)) >= '" + sFromWord + '%' + "'         \r";
                                    SQL += "    AND  UPPER(TRIM(b.ORDERNAMES)) < '" + sToWord + '%' + "')           \r";
                                    SQL += "     or (UPPER(TRIM(b.DRUGNAME)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND  UPPER(TRIM(b.DRUGNAME)) < '" + sToWord + '%' + "')  )          \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(b.ORDERNAME)) LIKE '" + sFromWord + '%' + "'         \r";
                                    SQL += "     or UPPER(TRIM(b.ORDERNAMES)) LIKE '" + sFromWord + '%' + "'        \r";
                                    SQL += "     or UPPER(TRIM(b.DRUGNAME)) LIKE '" + sFromWord + '%' + "'          \r";
                                }
                                SQL += "  ORDER BY b.ORDERNAME                                                      \r";
                            }
                        }
                        if (rdoSuNameG.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + sFromWord + '%' + "'                \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) >= '" + sFromWord + '%' + "'             \r";
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) < '" + sToWord + '%' + "'                \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(D.SUNAMEG)) LIKE '" + sFromWord + '%' + "'           \r";
                                }
                            }
                            SQL += "  ORDER BY D.SUNAMEG                                                            \r";
                        }
                        if (rdoOrderCode.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'                \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) < '" + sToWord + '%' + "'              \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) >= '" + sFromWord + '%' + "'           \r";
                                    SQL += "    AND UPPER(TRIM(b.ORDERCODE)) <= '" + sToWord + '%' + "'             \r";
                                }
                                SQL += "  ORDER BY b.ORDERCODE                                                      \r";

                            }
                        }
                        if (rdoBohumCode.Checked == true)
                        {
                            if (sFromWord == "하")
                            {
                                SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'                \r";
                            }
                            else
                            {
                                if (sGubun == "K")
                                {
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'               \r";
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) < '" + sToWord + '%' + "'                  \r";
                                }
                                else
                                {
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) >= '" + sFromWord + '%' + "'               \r";
                                    SQL += "    AND UPPER(TRIM(d.BCODE)) <= '" + sToWord + '%' + "'                 \r";
                                }
                                SQL += "  ORDER BY d.BCODE                                                          \r";
                            }
                        }
                    }
                }
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    //ssOrderCode.ActiveSheet.Cells[0, 0, (int)ssOrderCode.ActiveSheet.RowCount - 1, 0].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                    //ssOrderCode.ActiveSheet.Cells[0, 6, (int)ssOrderCode.ActiveSheet.RowCount - 1, 6].CellType = new FarPoint.Win.Spread.CellType.TextCellType();
                    clsDB.DataTableToSpdRow(dt, ssOrderCode, 0, true);
                }
                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        private void btnA_Click(object sender, EventArgs e)
        {
            Read_Order("A", "A", "E");
        }

        private void btnB_Click(object sender, EventArgs e)
        {
            Read_Order("B", "B", "E");
        }

        private void btnC_Click(object sender, EventArgs e)
        {
            Read_Order("C", "C", "E");
        }

        private void btnD_Click(object sender, EventArgs e)
        {
            Read_Order("D", "D", "E");
        }

        private void btnE_Click(object sender, EventArgs e)
        {
            Read_Order("E", "E", "E");
        }

        private void btnF_Click(object sender, EventArgs e)
        {
            Read_Order("F", "F", "E");
        }

        private void btnG_Click(object sender, EventArgs e)
        {
            Read_Order("G", "G", "E");
        }

        private void btnH_Click(object sender, EventArgs e)
        {
            Read_Order("H", "H", "E");
        }

        private void btnI_Click(object sender, EventArgs e)
        {
            Read_Order("I", "I", "E");
        }

        private void btnJ_Click(object sender, EventArgs e)
        {
            Read_Order("J", "J", "E");
        }

        private void btnK_Click(object sender, EventArgs e)
        {
            Read_Order("K", "K", "E");
        }

        private void btnL_Click(object sender, EventArgs e)
        {
            Read_Order("L", "L", "E");
        }

        private void btnM_Click(object sender, EventArgs e)
        {
            Read_Order("M", "M", "E");
        }

        private void btnN_Click(object sender, EventArgs e)
        {
            Read_Order("N", "N", "E");
        }

        private void btnO_Click(object sender, EventArgs e)
        {
            Read_Order("O", "O", "E");
        }

        private void btnP_Click(object sender, EventArgs e)
        {
            Read_Order("P", "P", "E");
        }

        private void btnQ_Click(object sender, EventArgs e)
        {
            Read_Order("Q", "Q", "E");
        }

        private void btnR_Click(object sender, EventArgs e)
        {
            Read_Order("R", "R", "E");
        }

        private void btnS_Click(object sender, EventArgs e)
        {
            Read_Order("S", "S", "E");
        }

        private void btnT_Click(object sender, EventArgs e)
        {
            Read_Order("T", "T", "E");
        }

        private void btnU_Click(object sender, EventArgs e)
        {
            Read_Order("U", "U", "E");
        }

        private void btnV_Click(object sender, EventArgs e)
        {
            Read_Order("V", "V", "E");
        }

        private void btnW_Click(object sender, EventArgs e)
        {
            Read_Order("W", "W", "E");
        }

        private void btnX_Click(object sender, EventArgs e)
        {
            Read_Order("X", "X", "E");
        }

        private void btnY_Click(object sender, EventArgs e)
        {
            Read_Order("Y", "Y", "E");
        }

        private void btnZ_Click(object sender, EventArgs e)
        {
            Read_Order("Z", "Z", "E");
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            Read_Order("ALL", "ALL", "E");
        }

        private void btnPersonInput_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("개인 코드로 등록 하시겠습니까??", "개인등록", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //Personal_Dept_Regist(clsOrdFunction.GstrDrCode);
                Personal_Dept_Regist(clsType.User.Sabun.Trim());
            }
        }

        void Personal_Dept_Regist(string argDeptDr)
        {
            string strOrderCode;
            string strBun;
            string strCBun;
            int k;

            k = 0;
            for (int i = 0; i < ssOrderCode.ActiveSheet.RowCount; i++)
            {
                //if (ssOrderCode.ActiveSheet.Cells[i, 1].BackColor == Color.LightCyan)
                if (ssOrderCode.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    k += 1;
                    break;
                }
            }

            if (k == 0)
            {
                MessageBox.Show("선택된 코드가 없습니다!!, 코드를 선택하신후 등록 하십시오!!!");
                return;
            }

            for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (Convert.ToBoolean(ssOrderCode.ActiveSheet.Cells[i, 0].Value) == true)
                {
                    strOrderCode = ssOrderCode_Sheet1.Cells[i, 2].Text.ToString().Trim();
                    strBun = ssOrderCode_Sheet1.Cells[i, 9].Text.ToString().Trim();
                    strCBun = ssOrderCode_Sheet1.Cells[i, 23].Text.ToString().Trim();

                    if (strBun == "")
                    {
                        MessageBox.Show(strOrderCode + " 코드는 입력 불가한 코드이므로 등록 할 수 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        ssOrderCode.ActiveSheet.Cells[i, 0].Text = "";
                        continue;
                    }

                    //if (strOrderCode != "" && strBun != "" && strCBun != "")
                    if (strOrderCode != "")
                    {
                        if (Read_ORDERCODE_COMUSE(argDeptDr, strOrderCode) == false)
                        {
                            Personl_CodeReg(argDeptDr, strBun, strCBun, strOrderCode);
                        }
                    }
                }
            }

            if (k > 0)
            {
                MessageBox.Show("등록 되었습니다!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            for (int i = 0; i < ssOrderCode.ActiveSheet.RowCount; i++)
            {
                ssOrderCode.ActiveSheet.Cells[i, 0, i, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            }
        }

        private void btnDeptInput_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("선택된 자료를 과 상용처방으로 등록 하시겠습니까? ", "과등록", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Personal_Dept_Regist(clsPublic.GstrDeptCode);
            }                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
        }

        void Personl_CodeReg(string argDeptDrCode, string argBun, string argCBun, string argOrderCode)
        {
            
            clsDB.setBeginTran(clsDB.DbCon);
            
            try
            {
                SQL = "";
                SQL += " INSERT INTO KOSMOS_OCS.OCS_OPARMDEF                \r";
                SQL += "        (DEPTDR                                     \r";
                SQL += "       , BUN                                        \r";
                SQL += "       , ORDERCODE)                                 \r";
                SQL += " VALUES ('" + argDeptDrCode + "'                    \r";
                SQL += "       , '" + argBun + "',                          \r";
                SQL += "         '" + argOrderCode.ToString().Trim() + "')  \r";
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }
                clsDB.setCommitTran(clsDB.DbCon);
                //ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                MessageBox.Show("코드 등록중 Error 발생!!!: " + ex.Message);
            }
        }

        bool Read_ORDERCODE_COMUSE(string argDeptDrCode, string argOrderCode)
        {
            try
            {
                SQL = "";
                SQL += " SELECT ORDERCODE                               \r";
                SQL += "   FROM KOSMOS_OCS.OCS_OPARMDEF                 \r";
                SQL += "  WHERE DEPTDR = '" + argDeptDrCode + "'        \r";
                SQL += "    AND ORDERCODE = '" + argOrderCode + "'      \r";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return false;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }

                rowcounter = 0;
                rowcounter = dt.Rows.Count;

                if (rowcounter > 0)
                {
                    dt.Dispose();
                    dt = null;
                    return true;
                }
                else
                {
                    dt.Dispose();
                    dt = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                return false;
            }
        }        

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (cboSelect.SelectedIndex == 2) return;
            string strOrderCode;
            int k;

            k = 0;
            for (int i = 0; i < ssOrderCode.ActiveSheet.RowCount; i++)
            {
                if (ssOrderCode.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    k += 1;
                    break;
                }
            }

            if (k == 0)
            {
                MessageBox.Show("선택된 코드가 없습니다!!, 코드를 선택하신후 삭제 하십시오!!!");
                return;
            }

            if (MessageBox.Show(cboSelect.Text + " 항목에 해당 처방을 삭제합니다." + "\r\n\r\n" + "삭제를 진행 하시겠습니까?", "안내", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            
            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                for (int i = 0; i < ssOrderCode.ActiveSheet.RowCount; i++)
                {
                    if (ssOrderCode.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strOrderCode = ssOrderCode_Sheet1.Cells[i, 2].Text.ToString().Trim();

                        SQL = "";
                        SQL += " DELETE FROM KOSMOS_OCS.OCS_OPARMDEF                            \r";
                        SQL += "  WHERE 1 = 1                                                   \r";
                        if (cboSelect.SelectedIndex == 0)
                        {
                            SQL += "    AND (DEPTDR = '" + clsOrdFunction.GstrDrCode + "'        \r";
                            SQL += "     or DEPTDR = '" + clsType.User.Sabun.Trim() + "')        \r";
                        }
                        else if (cboSelect.SelectedIndex == 1)
                        {
                            SQL += "    AND DEPTDR = '" + clsPublic.GstrDeptCode + "'           \r";
                        }
                        SQL += "    AND ORDERCODE = '" + strOrderCode.ToString() + "'           \r";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
                clsDB.setCommitTran(clsDB.DbCon);

                if (sNodeName != "")
                {
                    for (int i = 0; i < (int)trvSlipNo.Nodes.Count; i++)
                    {
                        if (trvSlipNo.Nodes[i].Text == sNodeName)
                        {
                            trvSlipNo.Select();
                            TreeNodeMouseClickEventArgs TreeE = new TreeNodeMouseClickEventArgs(trvSlipNo.Nodes[i], MouseButtons.Left, 1, 1, 1);
                            trvSlipNo_NodeMouseClick(trvSlipNo.Nodes[i].Text, TreeE);
                            break;
                        }
                    }
                }

                ComFunc.MsgBox("삭제 되었습니다.");
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                MessageBox.Show("상용처방 삭제중 Error 발생!!!: " + ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
            //ssOrderCode.ActiveSheet.Cells[0, 0, (int)ssOrderCode.ActiveSheet.RowCount - 1, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
            Sel_Slip();
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ssOrderCode.ActiveSheet.RowCount; i++)
            {
                if (ssOrderCode_Sheet1.Cells[i, 0].Text == "True")
                {
                    //if (clsOrdFunction.GstrSelSlipno.Trim() == "A1" || clsOrdFunction.GstrSelSlipno.Trim() == "A2" || clsOrdFunction.GstrSelSlipno.Trim() == "A4")
                    if (clsOrdFunction.GstrSelSlipno.Trim() == "A2" || clsOrdFunction.GstrSelSlipno.Trim() == "A4")
                    {
                        if (ssOrdSpread.Name != "ssOpdOrder")
                        {
                            SetSpcOrder(i, 1);
                            continue;
                        }
                    }

                    clsOrdFunction.GstrselOrderCode = ssOrderCode_Sheet1.Cells[i, 2].Text.Trim();
                    clsOrdFunction.GstrOrderCode = ssOrderCode_Sheet1.Cells[i, 2].Text.Trim();
                    clsOrdFunction.GstrOrderName = ssOrderCode_Sheet1.Cells[i, 4].Text.Trim();
                    clsOrdFunction.GstrSelSlipno = ssOrderCode_Sheet1.Cells[i, 15].Text.Trim();
                    
                    clsOrdFunction.GstrSubRate = "";

                    if (ssOrderCode.ActiveSheet.Cells[i, 19].Text.Trim() != "")
                    {
                        clsOrdFunction.GstrSubRate = ssOrderCode.ActiveSheet.Cells[i, 19].Text.Trim();
                        FrmMedViewSlipSubEvent = new FrmMedViewSlipSub(clsOrdFunction.GstrSelSlipno, clsOrdFunction.GstrSubRate);
                        FrmMedViewSlipSubEvent.OrdSubCodeDoubleClick += FrmMedViewSlipSubEvent_OrdSubCodeDoubleClick;
                        FrmMedViewSlipSubEvent.ShowDialog(this);
                        OF.fn_ClearMemory(FrmMedViewSlipSubEvent);
                        continue;
                    }

                    //입력불가 GBINPUT(1 : 입력가능)
                    if (VB.Left(clsOrdFunction.GstrSelSlipno, 1) != "A")
                    {
                        if (ssOrderCode.ActiveSheet.Cells[i, 6].Text.Trim() != "1")
                        {
                            MessageBox.Show(ssOrderCode.ActiveSheet.Cells[i, 4].Text + "[" + clsOrdFunction.GstrOrderCode + "] " + "처방은 처방입력이 불가합니다!!!", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            ssOrderCode_Sheet1.Cells[i, 0].Text = "False";
                            ssOrderCode.ActiveSheet.Cells[i, 1, i, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
                            continue;
                        }
                    }

                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        ssOrderCodeDoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim());
                    }
                    else if (clsOrdFunction.GstrGbJob == "ER")
                    {
                        if (clsOrdFunction.GstrselOrderCode.Trim() != "S/O")
                        {
                            ssErOrderCodeDoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim());
                        }
                        else
                        {
                            ssErOrderCodeSODoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim(), clsOrdFunction.GstrOrderName);
                        }
                    }
                    else if (clsOrdFunction.GstrGbJob == "IPD")
                    {
                        if (clsOrdFunction.GstrselOrderCode.Trim() != "S/O")
                        {
                            ssIpdOrderCodeDoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim());
                        }
                        else
                        {
                            ssIpdOrderCodeSODoubleClick(clsOrdFunction.GstrOrderCode.Trim(), clsOrdFunction.GstrSelSlipno.Trim(), clsOrdFunction.GstrOrderName);
                        }
                    }
                }
            }
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txtSearch.Text != "" && e.KeyChar == (char)13)
            {
                e.Handled = true;
                btnView_Click(btnView, new EventArgs());
            }
        }

        private void ssOrderCode_SelectionChanging(object sender, FarPoint.Win.Spread.SelectionChangingEventArgs e)
        {
            int nStartRow = 0;
            int nEndRow = 0;

            nStartRow = e.Range.Row;
            nEndRow = e.CurrentRow;

            if (nStartRow < nEndRow)
            {
                ssOrderCode_Sheet1.Cells[nStartRow, 0, (int)ssOrderCode.ActiveSheet.RowCount - 1, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.White;
                for (int i = nStartRow; i <= nEndRow; i++)
                {
                    if (ssOrderCode_Sheet1.Cells[i, 1, i, 1].Text != "")
                    {
                        ssOrderCode_Sheet1.Cells[i, 0, i, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.LightCyan;
                    }
                }
            }
            else if (nStartRow >= nEndRow)
            {
                for (int i = nStartRow; i == nEndRow; i--)
                {
                    if (ssOrderCode_Sheet1.Cells[i, 1, i, 1].Text != "")
                    {
                        ssOrderCode_Sheet1.Cells[i, 0, i, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.LightCyan;
                    }
                }
            }
        }

        void fn_Set_Check(string cSlipNo)
        {
            string strSet = "";
            int nRowCount = 0;

            try
            {
                SQL = "";
                SQL += " SELECT * FROM KOSMOS_OCS.OCS_ORDERCODE \r";
                SQL += "  WHERE Slipno = '" + cSlipNo + "'      \r";
                SQL += "    AND ( Seqno = 0 OR Bun > '  ' )     \r";
                SQL += "  ORDER BY Seqno                        \r";
                SqlErr = clsDB.GetDataTable(ref dt2, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                }

                if (dt2.Rows.Count >= 2) nRowCount = 2;
                
                if (nRowCount == 2)
                {
                    strSet = "";
                    if (cSlipNo == "0003" || cSlipNo == "0004" || cSlipNo == "0005")
                    {
                        switch (dt2.Rows[1]["BUN"].ToString().Trim())
                        {
                            case "11":
                                strSet = "11";
                                break;
                            case "12":
                                strSet = "12";
                                break;
                            case "16":
                            case "17":
                            case "18":
                            case "19":
                            case "20":
                            case "21":
                                strSet = "20";
                                break;
                            default:
                                strSet = "";
                                break;
                        }
                    }
                    
                    if (strSet != "")
                    {
                        cboSelect.SelectedIndex = 1;
                    }

                    if (cSlipNo == "A1")
                    {
                        cboSelect.SelectedIndex = 2;
                    }
                    else if (cSlipNo == "A4")
                    {
                        cboSelect.SelectedIndex = 1;
                    }
                    else if (cSlipNo == "A2")
                    {
                        cboSelect.SelectedIndex = 2;
                    }
                }

                dt2.Dispose();
                dt2 = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void Sel_Slip()
        {
            string strSlipNo;
            string strDeptDr;
            string strPrm;

            if (clsOrdFunction.GstrSelSlipno.Trim() == "") return;
            
            //lblSlipName.Text = OM.GetSlipName(clsOrdFunction.GstrSelSlipno) + "(" + clsOrdFunction.GstrSelSlipno + ")";    
            try
            {
                if (clsOrdFunction.GstrSelSlipno.Trim() == "A1" || clsOrdFunction.GstrSelSlipno.Trim() == "A2" || clsOrdFunction.GstrSelSlipno.Trim() == "A4")
                {
                    if (cboSelect.SelectedIndex != 2)
                    {
                        SQL = "";
                        SQL += " SELECT '' chk                                                                  \r";
                        SQL += "      , (select ordername                                                       \r";
                        SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                        SQL += "          where slipno = '" + clsOrdFunction.GstrSelSlipno + "'                 \r";
                        SQL += "            and seqno = 0 ) slipname                                            \r";
                        SQL += "      , a.ORDERCODE, '' blank                                                   \r";
                        SQL += "      , nvl(b.ordername, a.remark) ordername                                    \r";
                        SQL += "      , a.remark                                                                \r";
                        SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                        SQL += "      , decode(TRIM(B.ORDERCODE), '##11', '15', B.Bun) Bun,  B.NEXTCODE,  B.SUCODE, '' INFO \r";
                        SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                        SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     B.SENDDEPT                     \r";
                        SQL += "      , '', '', B.CBUN                                                          \r";
                        SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                        SQL += "   FROM " + ComNum.DB_MED + "OCS_ISO       A                                    \r";
                        SQL += "      , " + ComNum.DB_MED + "OCS_ORDERCODE B                                    \r";
                        if (cboSelect.SelectedIndex == 0)
                        {
                            SQL += "  WHERE A.DeptDr    = '" + clsType.User.Sabun.Trim() + "'                   \r";
                        }
                        else
                        {
                            SQL += "  WHERE A.DeptDr    = '" + clsOrdFunction.Pat.DeptCode.Trim() + "'          \r";
                        }
                        SQL += "    AND A.OrderCode = B.OrderCode(+)                                            \r";
                        SQL += "    AND '" + clsOrdFunction.GstrSelSlipno.Trim() + "' = B.Slipno(+)             \r";
                        SQL += "    AND 0          <> B.Seqno(+)                                                \r";
                        SQL += "    AND (b.senddept != 'N' or b.senddept is null)                               \r";
                        SQL += "  ORDER BY NVL(b.OrderName,Remark)                                              \r";
                    }
                    else
                    {
                        SQL = "";
                        SQL += " SELECT '' chk                                                                  \r";
                        SQL += "      , (select ordername                                                       \r";
                        SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                        SQL += "          where slipno = '" + clsOrdFunction.GstrSelSlipno + "'                 \r";
                        SQL += "            and seqno = 0 ) slipname                                            \r";
                        SQL += "      , B.ORDERCODE, '' blank                                                   \r";
                        SQL += "      , b.ordername                                                             \r";
                        SQL += "      , '' remark                                                               \r";
                        SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                        SQL += "      , decode(TRIM(B.ORDERCODE), '##11', '15', B.Bun) Bun, B.NEXTCODE,  B.SUCODE, '' INFO  \r";
                        SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                        SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     B.SENDDEPT                     \r";
                        SQL += "      , '', '', B.CBUN                                                          \r";
                        SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                        SQL += "   FROM " + ComNum.DB_MED + "OCS_ORDERCODE B                                    \r";
                        SQL += "  WHERE 1=1                                                                     \r";
                        SQL += "    AND B.Seqno    <> 0                                                         \r";
                        SQL += "    AND B.Slipno = '" + clsOrdFunction.GstrSelSlipno + "'                       \r";
                        SQL += "    AND (B.senddept != 'N' or B.senddept is null)                               \r";
                        SQL += "  ORDER BY b.OrderName                                                          \r";
                    }
                }
                else
                {
                    if (cboSelect.SelectedIndex == 2)
                    {
                        SQL = "";
                        SQL += " SELECT '' chk                                                                  \r";
                        SQL += "      , (select ordername                                                       \r";
                        SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                        SQL += "          where slipno = '" + clsOrdFunction.GstrSelSlipno + "'                 \r";
                        SQL += "            and seqno = 0 ) slipname                                            \r";
                        SQL += "      , B.ORDERCODE, '' blank                                                   \r";
                        //SQL += "      , decode(case when b.cbun = '110' then '110'                              \r";
                        //SQL += "                    when b.cbun = '120' then '120'                              \r";
                        //SQL += "                    when b.cbun = '140' then '140'                              \r";
                        //SQL += "                    when b.cbun = '150' then '150'                              \r";
                        //SQL += "                    when b.cbun = '740' then '740'                              \r";
                        //SQL += "                    when b.cbun = '040' then '040'                              \r";
                        //SQL += "                    when b.cbun >= '160' and b.Cbun <= '210' then '200'         \r";
                        //SQL += "               else ''                                                          \r";
                        //SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                        SQL += "      , decode(case when b.bun = '11' then '11'                                 \r";
                        SQL += "                    when b.bun = '12' then '12'                                 \r";
                        SQL += "                    when b.bun = '14' then '14'                                 \r";
                        SQL += "                    when b.bun = '15' then '15'                                 \r";
                        //SQL += "                    when b.bun = '23' then '23'                                 \r";
                        //SQL += "                    when b.bun = '29' then '29'                                 \r";
                        //SQL += "                    when b.bun = '31' then '31'                                 \r";
                        //SQL += "                    when b.bun = '48' then '48'                                 \r";
                        //SQL += "                    when b.bun = '66' then '66'                                 \r";
                        //SQL += "                    when b.bun = '72' then '72'                                 \r";
                        //SQL += "                    when b.bun = '73' then '73'                                 \r";
                        SQL += "                    when b.bun = '74' then '74'                                 \r";
                        SQL += "                    when b.bun = '04' then '04'                                 \r";
                        SQL += "                    when b.bun >= '16' and b.bun <= '21' then '20'              \r";
                        SQL += "               else ''                                                          \r";
                        SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                        //SQL += "                end , '', b.ordername, NVL(b.ordername, '                    ') || b.ordernames) ordername \r";
                        SQL += "      , '' remark                                                               \r";
                        SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                        SQL += "      , b.Bun,       B.NEXTCODE,  B.SUCODE,      '' INFO                        \r";
                        SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                        SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     B.SENDDEPT                     \r";
                        SQL += "      , '', '', B.CBUN                                                          \r";
                        SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                        SQL += "   FROM " + ComNum.DB_MED + "OCS_ORDERCODE B                                    \r";
                        SQL += "      , " + ComNum.DB_PMPA + "BAS_SUT      C                                    \r";
                        SQL += "      , " + ComNum.DB_PMPA + "BAS_SUN      D                                    \r";
                        SQL += "  WHERE 1=1                                                                     \r";
                        SQL += "    AND B.Seqno    <> 0                                                         \r";
                        SQL += "    AND B.Slipno = '" + clsOrdFunction.GstrSelSlipno + "'                       \r";
                        if (nw.Read_IPAddress_SQL() != "192.168.2.15")
                        {
                            SQL += "    AND (b.senddept != 'N' or b.senddept is null)                           \r"; //사용하지않는 코드 제외
                        }
                        SQL += "    AND B.SuCode     = C.SuCode(+)                                              \r";
                        SQL += "    AND B.SuCode     = D.SuNext(+)                                              \r";
                        if (clsOrdFunction.GstrSelSlipno.Trim() != "0074")
                        {
                            SQL += "    AND (C.SugbJ <> '2' OR C.SugbJ IS NULL)                                 \r";
                        }
                        SQL += "    AND (b.gbsub != '1' OR b.gbsub IS NULL)                                     \r";
                        //if (Convert.ToInt32(clsOrdFunction.GstrSelSlipno) >= Convert.ToInt32("0003") && Convert.ToInt32(clsOrdFunction.GstrSelSlipno) <= Convert.ToInt32("0005"))
                        if (clsOrdFunction.GstrSelSlipno == "003" || clsOrdFunction.GstrSelSlipno == "0004" || clsOrdFunction.GstrSelSlipno == "0005")
                        {
                            SQL += "  ORDER BY b.SUCODE                                                         \r";
                        }                        
                        else
                        {
                            SQL += "  ORDER BY b.SEQNO                                                          \r";
                        }
                    }
                    else if (cboSelect.SelectedIndex == 1)
                    {
                        SQL = "";
                        SQL += " SELECT '' chk                                                                  \r";
                        SQL += "      , (select ordername                                                       \r";
                        SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                        SQL += "          where slipno = '" + clsOrdFunction.GstrSelSlipno + "'                 \r";
                        SQL += "            and seqno = 0 ) slipname                                            \r";
                        SQL += "      , B.ORDERCODE, '' blank                                                   \r";
                        //SQL += "      , decode(case when b.cbun = '110' then '110'                              \r";
                        //SQL += "                    when b.cbun = '120' then '120'                              \r";
                        //SQL += "                    when b.cbun = '140' then '140'                              \r";
                        //SQL += "                    when b.cbun = '150' then '150'                              \r";
                        //SQL += "                    when b.cbun = '740' then '740'                              \r";
                        //SQL += "                    when b.cbun = '040' then '040'                              \r";
                        //SQL += "                    when b.cbun >= '160' and b.Cbun <= '210' then '200'         \r";
                        //SQL += "               else ''                                                          \r";
                        //SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                        SQL += "      , decode(case when b.bun = '11' then '11'                                 \r";
                        SQL += "                    when b.bun = '12' then '12'                                 \r";
                        SQL += "                    when b.bun = '14' then '14'                                 \r";
                        SQL += "                    when b.bun = '15' then '15'                                 \r";
                        //SQL += "                    when b.bun = '23' then '23'                                 \r";
                        //SQL += "                    when b.bun = '29' then '29'                                 \r";
                        //SQL += "                    when b.bun = '31' then '31'                                 \r";
                        //SQL += "                    when b.bun = '48' then '48'                                 \r";
                        //SQL += "                    when b.bun = '66' then '66'                                 \r";
                        //SQL += "                    when b.bun = '72' then '72'                                 \r";
                        //SQL += "                    when b.bun = '73' then '73'                                 \r";
                        SQL += "                    when b.bun = '74' then '74'                                 \r";
                        SQL += "                    when b.bun = '04' then '04'                                 \r";
                        SQL += "                    when b.bun >= '16' and b.bun <= '21' then '20'              \r";
                        SQL += "               else ''                                                          \r";
                        //SQL += "                end , '', b.ordername, NVL(b.ordername, '                    ') || b.ordernames) ordername \r";
                        SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                        SQL += "      , '' remark                                                               \r";
                        SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                        SQL += "      , b.Bun,       B.NEXTCODE,  b.SUCODE,      '' INFO                        \r";
                        SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                        SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     B.SENDDEPT                     \r";
                        SQL += "      , '', '', B.CBUN                                                          \r";
                        SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                        SQL += "   FROM " + ComNum.DB_MED + "OCS_OPARMDEF  A                                    \r";
                        SQL += "      , " + ComNum.DB_MED + "OCS_ORDERCODE B                                    \r";
                        SQL += "      , " + ComNum.DB_PMPA + "BAS_SUT      C                                    \r";
                        SQL += "      , " + ComNum.DB_PMPA + "BAS_SUN      D                                    \r";
                        SQL += "  WHERE 1=1                                                                     \r";
                        SQL += "    AND A.OrderCode = B.OrderCode                                               \r";
                        SQL += "    AND B.Seqno    <> 0                                                         \r";
                        SQL += "    AND B.Slipno||'' = '" + clsOrdFunction.GstrSelSlipno + "'                   \r";
                        if (nw.Read_IPAddress_SQL() != "192.168.2.15")
                        {
                            SQL += "    AND (b.senddept != 'N' or b.senddept is null)                          \r"; //사용하지않는 코드 제외
                        }
                        SQL += "    AND B.SuCode     = C.SuCode(+)                                              \r";
                        SQL += "    AND B.SuCode     = D.SuNext(+)                                              \r";
                        SQL += "    AND (C.SugbJ <> '2' OR C.SugbJ IS NULL)                                     \r";
                        SQL += "    AND A.DEPTDR = '" + clsOrdFunction.Pat.DeptCode + "'                        \r";
                        SQL += "    AND (b.gbsub != '1' OR b.gbsub IS NULL)                                     \r";
                        //if (Convert.ToInt32(clsOrdFunction.GstrSelSlipno) >= Convert.ToInt32("0003") && Convert.ToInt32(clsOrdFunction.GstrSelSlipno) <= Convert.ToInt32("0005"))
                        if (clsOrdFunction.GstrSelSlipno == "003" || clsOrdFunction.GstrSelSlipno == "0004" || clsOrdFunction.GstrSelSlipno == "0005")
                        {
                            SQL += "  ORDER BY b.SUCODE                                                         \r";
                        }
                        else
                        {
                            SQL += "  ORDER BY b.SEQNO                                                          \r";
                        }
                    }
                    else if (cboSelect.SelectedIndex == 0)
                    {
                        SQL = "";
                        SQL += " SELECT '' chk                                                                  \r";
                        SQL += "      , (select ordername                                                       \r";
                        SQL += "           from KOSMOS_OCS.OCS_ORDERCODE                                        \r";
                        SQL += "          where slipno = '" + clsOrdFunction.GstrSelSlipno + "'                 \r";
                        SQL += "            and seqno = 0 ) slipname                                            \r";
                        SQL += "      , B.ORDERCODE, '' blank                                                   \r";
                        //SQL += "      , decode(case when b.cbun = '110' then '110'                              \r";
                        //SQL += "                    when b.cbun = '120' then '120'                              \r";
                        //SQL += "                    when b.cbun = '140' then '140'                              \r";
                        //SQL += "                    when b.cbun = '150' then '150'                              \r";
                        //SQL += "                    when b.cbun = '740' then '740'                              \r";
                        //SQL += "                    when b.cbun = '040' then '040'                              \r";
                        //SQL += "                    when b.cbun >= '160' and b.Cbun <= '210' then '200'         \r";
                        //SQL += "               else ''                                                          \r";
                        //SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                        SQL += "      , decode(case when b.bun = '11' then '11'                                 \r";
                        SQL += "                    when b.bun = '12' then '12'                                 \r";
                        SQL += "                    when b.bun = '14' then '14'                                 \r";
                        SQL += "                    when b.bun = '15' then '15'                                 \r";
                        //SQL += "                    when b.bun = '23' then '23'                                 \r";
                        //SQL += "                    when b.bun = '29' then '29'                                 \r";
                        //SQL += "                    when b.bun = '31' then '31'                                 \r";
                        //SQL += "                    when b.bun = '48' then '48'                                 \r";
                        //SQL += "                    when b.bun = '66' then '66'                                 \r";
                        //SQL += "                    when b.bun = '72' then '72'                                 \r";
                        //SQL += "                    when b.bun = '73' then '73'                                 \r";
                        SQL += "                    when b.bun = '74' then '74'                                 \r";
                        SQL += "                    when b.bun = '04' then '04'                                 \r";
                        SQL += "                    when b.bun >= '16' and b.bun <= '21' then '20'              \r";
                        SQL += "               else ''                                                          \r";
                        SQL += "                end , '', b.ordername, RPAD(NVL(b.ordername, '                    ') , 20) || b.ordernames) ordername \r";
                        //SQL += "                end , '', b.ordername, NVL(b.ordername, '                    ') || b.ordernames) ordername  \r";
                        SQL += "      , '' remark                                                               \r";
                        SQL += "      , B.GbInput,   B.GBINFO,    B.GBBOTH                                      \r";
                        SQL += "      , b.Bun,       B.NEXTCODE,  b.SUCODE,      '' INFO                        \r";
                        SQL += "      , B.Speccode,  B.GbDosage,  B.Slipno,      '' SPECNAME                    \r";
                        SQL += "      , B.GbImiv,    '' INPUTSEQ, B.SubRate,     B.SENDDEPT                     \r";
                        SQL += "      , '', '', B.CBUN                                                          \r";
                        SQL += "      , KOSMOS_OCS.FC_DRUG_OUTCHK(B.SUCODE, B.CBUN) DELOUTSTAT                  \r";
                        SQL += "   FROM " + ComNum.DB_MED + "OCS_OPARMDEF  A                                    \r";
                        SQL += "      , " + ComNum.DB_MED + "OCS_ORDERCODE B                                    \r";
                        SQL += "      , " + ComNum.DB_PMPA + "BAS_SUT      C                                    \r";
                        SQL += "      , " + ComNum.DB_PMPA + "BAS_SUN      D                                    \r";
                        SQL += "  WHERE 1=1                                                                     \r";
                        SQL += "    AND A.OrderCode = B.OrderCode                                               \r";
                        SQL += "    AND B.Seqno    <> 0                                                         \r";
                        SQL += "    AND B.Slipno||'' = '" + clsOrdFunction.GstrSelSlipno + "'                   \r";
                        if (nw.Read_IPAddress_SQL() != "192.168.2.15")
                        {
                            SQL += "    AND (b.senddept != 'N' or b.senddept is null)                           \r"; //사용하지않는 코드 제외
                        }
                        SQL += "    AND B.SuCode     = C.SuCode(+)                                              \r";
                        SQL += "    AND B.SuCode     = D.SuNext(+)                                              \r";
                        SQL += "    AND (C.SugbJ <> '2' OR C.SugbJ IS NULL)                                     \r";
                        SQL += "    AND (A.DEPTDR = '" + clsOrdFunction.Pat.DrCode + "'                         \r";
                        SQL += "     OR  A.DEPTDR = '" + clsType.User.Sabun.Trim() + "')                        \r"; 
                        SQL += "    AND (b.gbsub != '1' OR b.gbsub IS NULL)                                     \r";
                        //if (Convert.ToInt32(clsOrdFunction.GstrSelSlipno) >= Convert.ToInt32("0003") && Convert.ToInt32(clsOrdFunction.GstrSelSlipno) <= Convert.ToInt32("0005"))
                        if (clsOrdFunction.GstrSelSlipno == "003" || clsOrdFunction.GstrSelSlipno == "0004" || clsOrdFunction.GstrSelSlipno == "0005")
                        {
                            SQL += "  ORDER BY b.SUCODE                                                         \r";
                        }
                        else
                        {
                            SQL += "  ORDER BY b.SEQNO                                                          \r";
                        }
                    }
                }
                SqlErr = clsDB.GetDataTable(ref dt1, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 오류가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }

                if (dt1.Rows.Count > 0)
                {
                    SsSet_Default();
                    ssOrderCode.ActiveSheet.RowCount = dt1.Rows.Count;

                    clsDB.DataTableToSpdRow(dt1, ssOrderCode, 0, true);

                    if ((int)ssOrderCode.ActiveSheet.RowCount > 0)
                    {
                        ssOrderCode.ActiveSheet.Cells[0, 0, (int)ssOrderCode.ActiveSheet.RowCount - 1, 0].CellType = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                        //clsDB.DataTableToSpdRow(dt1, ssOrderCode, 0, true);
                        for (int i = 0; i < dt1.Rows.Count; i++)
                        {
                            if (dt1.Rows[i]["GBINFO"].ToString() == "1")
                            {
                                ssOrderCode.ActiveSheet.Cells[i, 4].Text = "▲" + dt1.Rows[i]["ordername"].ToString().Trim();
                                ssOrderCode.ActiveSheet.Cells[i, 0, i, ssOrderCode.ActiveSheet.ColumnCount - 1].ForeColor = Color.DarkGreen;
                            }
                            else if (dt1.Rows[i]["SUBRATE"].ToString() != "")
                            {
                                ssOrderCode.ActiveSheet.Cells[i, 4].Text = "☜" + dt1.Rows[i]["ordername"].ToString().Trim();
                                ssOrderCode.ActiveSheet.Cells[i, 0, i, ssOrderCode.ActiveSheet.ColumnCount - 1].ForeColor = Color.Navy;
                            }
                            else
                            {
                                ssOrderCode.ActiveSheet.Cells[i, 4].Text = dt1.Rows[i]["ordername"].ToString().Trim();
                            }

                            ssOrderCode.ActiveSheet.Cells[i, 17].Text = "";

                            if (dt1.Rows[i]["SENDDEPT"].ToString() == "N")
                            {
                                ssOrderCode.ActiveSheet.Cells[i, 17].Text = "삭제";
                                ssOrderCode.ActiveSheet.Cells[i, 0, i, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 217, 217);
                            }
                            else
                            {
                                //쿼리단으로 옮김. 속도개선(2017.11.15)
                                //ssOrderCode.ActiveSheet.Cells[i, 17].Text = clsOrderEtc.READ_DRUG_OUTCHK(clsDB.DbCon, ssOrderCode.ActiveSheet.Cells[i, 10].Text, ssOrderCode.ActiveSheet.Cells[i, 8].Text);
                                ssOrderCode.ActiveSheet.Cells[i, 23].Text = dt1.Rows[i]["DELOUTSTAT"].ToString();
                                if (ssOrderCode.ActiveSheet.Cells[i, 23].Text == "원외" && clsOrdFunction.GstrGbJob == "IPD")
                                {
                                    ssOrderCode.ActiveSheet.Cells[i, 0, i, (int)ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 217, 217);
                                }
                            }

                            if (ssOrderCode.ActiveSheet.Cells[i, 15].Text.Trim() == "")
                            {
                                ssOrderCode.ActiveSheet.Cells[i, 15].Text = clsOrdFunction.GstrSelSlipno;
                            }

                            //if (clsOrdFunction.GstrSelSlipno == "A1" || clsOrdFunction.GstrSelSlipno == "A2" || clsOrdFunction.GstrSelSlipno == "A4")
                            //{
                            //    ssOrderCode.ActiveSheet.Cells[i, 15].Text = clsOrdFunction.GstrSelSlipno;
                            //    ssOrderCode.ActiveSheet.Cells[i, 1].Text = ComFunc.LeftH(clsOrdFunction.SlipNo_Gubun(clsOrdFunction.GstrSelSlipno, "", ""), 7);
                            //    ssOrderCode.ActiveSheet.Cells[i, 34].Text = ComFunc.RightH(clsOrdFunction.SlipNo_Gubun(clsOrdFunction.GstrSelSlipno, "", ""), 3).Trim();
                            //}
                        }
                    }
                }

                if (clsOrdFunction.GstrSelSlipno == "A2" || clsOrdFunction.GstrSelSlipno == "A4")
                {
                    ssOrderCode.ActiveSheet.AddRows(0, 1);
                    ssOrderCode.ActiveSheet.Cells[0, 4].CellType = txt;
                    ssOrderCode.ActiveSheet.Cells[0, 4].Locked = false;
                    ssOrderCode.ActiveSheet.Cells[0, 1].Text = OF.fn_GetSlipName(clsDB.DbCon, clsOrdFunction.GstrSelSlipno);
                    ssOrderCode.ActiveSheet.Cells[0, 2].Text = "S/O";
                    ssOrderCode.ActiveSheet.Cells[0, 6].Text = "1";
                    ssOrderCode.ActiveSheet.Cells[0, 9].Text = "";
                    ssOrderCode.ActiveSheet.Cells[0, 15].Text = clsOrdFunction.GstrSelSlipno;
                    ssOrderCode.ActiveSheet.SetActiveCell(0, 4);
                }
                
                dt1.Dispose();
                dt1 = null;
            }

            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
        }

        void SsSet_Default()
        {   
            ssOrderCode.ActiveSheet.RowCount = 0;
            ssOrderCode.ActiveSheet.ClearControls();
        }

        private void txtSearch_Click(object sender, EventArgs e)
        {
            ComFunc.StartLen(txtSearch);
        }

        private void ssOrderCode_ButtonClicked(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            string strORDERCODE;
            string strOK = "";
            string strBun;
            string strSpecName;
            string strSlipNo;
            
            if (e.Column != 0) return;

            if (ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text == "True" || ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text == "False")
            {
                ssOrderCode.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
                return;
            }

            if (ssOrderCode.ActiveSheet.Cells[e.Row, 1].Text == "")
            {
                ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text = "False";
                return;
            }

            strBun = ssOrderCode.ActiveSheet.Cells[e.Row, 6].Text;
            strSlipNo = ssOrderCode.ActiveSheet.Cells[e.Row, 15].Text;
            
            if (ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                ssOrderCode.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 128);
            }
            else
            {
                ssOrderCode.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
                return;
            }
            clsOrdFunction.GstrSubRate = "";
            if (ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text == "True")
            {
                if (ssOrderCode.ActiveSheet.Cells[e.Row, 19].Text != "")
                {
                    //clsOrdFunction.GstrSubRate = ssOrderCode.ActiveSheet.Cells[e.Row, 16].Text;
                    //FrmMedViewSlipSub ff = new FrmMedViewSlipSub();
                    //ff.ShowDialog();
                    //OF.fn_ClearMemory(ff);

                    clsOrdFunction.GstrSubRate = ssOrderCode.ActiveSheet.Cells[e.Row, 19].Text.Trim();
                    FrmMedViewSlipSubEvent = new FrmMedViewSlipSub(clsOrdFunction.GstrSelSlipno, clsOrdFunction.GstrSubRate);
                    FrmMedViewSlipSubEvent.OrdSubCodeDoubleClick += FrmMedViewSlipSubEvent_OrdSubCodeDoubleClick;
                    FrmMedViewSlipSubEvent.ShowDialog(this);
                    OF.fn_ClearMemory(FrmMedViewSlipSubEvent);

                    ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text = "False";
                    return;
                }
            }

            if (ssOrderCode.ActiveSheet.Cells[e.Row, 6].Text != "1")
            {
                MessageBox.Show("해당 처방은 처방입력이 불가합니다!!!", "처방불가", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text = "False";
                ssOrderCode.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);                
                return;
            }

            strOK = "OK";
            
            string strCBun = "";
            string strNextCode = "";
            int nRow = 0;
            string strOrderName = "";
            string strillName = "";
            int nNextRow = 0;

            clsOrdFunction.GstrOrderSelect = "ORD";

            strORDERCODE = ssOrderCode_Sheet1.Cells[e.Row, 2].Text.Trim();

            clsOrdFunction.GstrselOrderCode = ssOrderCode_Sheet1.Cells[e.Row, 2].Text.Trim();
            clsOrdFunction.GstrOrderCode = ssOrderCode_Sheet1.Cells[e.Row, 2].Text.Trim();
            clsOrdFunction.GstrSelSlipno = ssOrderCode_Sheet1.Cells[e.Row, 15].Text.Trim();
            clsOrdFunction.GstrSubRate = ssOrderCode_Sheet1.Cells[e.Row, 19].Text.Trim();

            ssOrderCodeCheck(clsOrdFunction.GstrOrderCode, clsOrdFunction.GstrSelSlipno);
            strOrderName = ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text;
            strBun = ssOrderCode.ActiveSheet.Cells[e.Row, 9].Text;
            strCBun = ssOrderCode.ActiveSheet.Cells[e.Row, 23].Text;
            strNextCode = ssOrderCode.ActiveSheet.Cells[e.Row, 10].Text;

            //Xray_Check:
            if (ssOrderCode.ActiveSheet.Cells[e.Row, 8].Text == "")
            {
                //call Xray_Check1:
                int nSeleCount;
                strNextCode = ssOrderCode.ActiveSheet.Cells[e.Row, 2].Text;
                strOK = "NO";
                nSeleCount = 0;

                for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
                {
                    if (ssOrderCode.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        nSeleCount += 1;
                        if (nSeleCount > 1)
                        {
                            strOK = "OK";
                            break;
                        }
                    }
                }

                //Xray_Check2:
                if (strOK == "NO")
                {
                    strOK = "OK";

                    ssOrderXrayDupCodeCheck(strNextCode);
                }
            }

            for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (i != e.Row && ssOrderCode.ActiveSheet.Cells[i, 0].Text == "True")
                {
                    nNextRow = i;
                    if (strNextCode.Trim() == ssOrderCode.ActiveSheet.Cells[i, 2].Text.Trim())
                    {
                        strOK = "NO";
                        break;
                    }
                }
            }

            if (strOK == "OK")
            {
                OrderDup_Check(strNextCode, strOrderName, strOK);

                if (clsOrdFunction.GstrDupOrderOK == "NO")
                {
                    ssOrderCode.ActiveSheet.Cells[e.Row, 1, e.Row, ssOrderCode.ActiveSheet.ColumnCount - 1].BackColor = Color.FromArgb(255, 255, 234);
                    ssOrderCode.ActiveSheet.Cells[e.Row, 0].Text = "False";
                }

                clsOrdFunction.GstrDupOrderOK = "";
            }

            //if (strNextCode.Trim() == "")
            //{
            //    for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
            //    {
            //        if (ssOrderCode.ActiveSheet.Cells[i, 0].Text != "True")
            //        {
            //            if (ssOrderCode.ActiveSheet.Cells[i, 9].Text == strNextCode.Trim())
            //            {
            //                nRow = 2;
            //                strOrderName = ssOrderCode.ActiveSheet.Cells[i, 2].Text.Trim();
            //                break;
            //            }
            //        }
            //    }

            //    if (nRow == 2)
            //    {
            //        for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
            //        {
            //            if (ssOrderCode.ActiveSheet.Cells[i, 0].Text == "True")
            //            {
            //                if (ssOrderCode.ActiveSheet.Cells[i, 2].Text.Trim() == strNextCode.Trim())
            //                {
            //                    nRow = 2;
            //                    strOrderName = ssOrderCode.ActiveSheet.Cells[i, 2].Text;
            //                    break;
            //                }
            //            }
            //        }
            //    }
            //}

            //for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
            //{
            //    if (ssOrderCode.ActiveSheet.Cells[i, 0].Text != "True")
            //    {
            //        if (strNextCode.Trim() == ssOrderCode.ActiveSheet.Cells[i, 1].Text.Trim())
            //        {
            //            nRow = 2;
            //            strOrderName = ssOrderCode.ActiveSheet.Cells[i, 2].Text;
            //            break;
            //        }
            //    }
            //}

            //if (nRow != 2)
            //{
            //    for (int i = 0; i < ssOrderCode.ActiveSheet.NonEmptyRowCount; i++)
            //    {
            //        if (ssOrderCode.ActiveSheet.ActiveRowIndex != i && ssOrderCode.ActiveSheet.Cells[i, 0].Text != "True")
            //        {
            //            if (ssOrderCode.ActiveSheet.Cells[i, 2].Text.Trim() == strNextCode.Trim())
            //            {
            //                nRow = 2;
            //                strOrderName = ssOrderCode.ActiveSheet.Cells[i, 1].Text.Trim();
            //                break;
            //            }
            //        }
            //    }
            //}

            if (nRow == 1 && dt.Rows[0]["SLIPNO"].ToString().Trim() == "0102")
            {
                string cPT_PtNo = "";

                //PT 치료중 확인
                if (e.Row == 0) return;

                //????????
                cPT_PtNo = clsOrdFunction.Pat.PtNo;
            }

            if (ssOrderCode.ActiveSheet.Cells[e.Row, 7].Text == "1") //금액적용
            {
                FrmMedViewAmt f = new FrmMedViewAmt();
                f.ShowDialog();
                OF.fn_ClearMemory(f);
                ssOrderCode.ActiveSheet.Cells[e.Row, 12].Text = clsOrdFunction.GstrSELECTGbInfo;
                lblLoad.Text = "안내 : " + clsOrdFunction.GstrSELECTGbInfo;
            }

            //임신주수 입력
            strOrderName = ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text.Trim();
            if (clsOrderEtc.READ_PREGNANCY_WEEK_NUMBER_USG_CODE(clsDB.DbCon, strORDERCODE) == true)
            {
                clsPublic.Gstr임신차수 = "";
                FrmMedPregnantOrder f = new FrmMedPregnantOrder(strORDERCODE, strOrderName, e.Row);
                f.ShowDialog();
                OF.fn_ClearMemory(f);
            }

            clsOrderEtc.CHECK_F0913(strORDERCODE, clsOrdFunction.Pat.DeptCode, clsOrdFunction.Pat.Sex, clsOrdFunction.Pat.Age);

            //if (ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text == "1")
            //{
            //    ssOrderCode.ActiveSheet.Cells[e.Row, 8].Text = clsOrdFunction.GstrSELECTSuCode;
            //    ssOrderCode.ActiveSheet.Cells[e.Row, 9].Text = clsOrdFunction.GstrSELECTGbInfo;

            //    FrmViewBoth f = new FrmViewBoth("OpdOrder", strORDERCODE);
            //    f.ShowDialog();
            //    f.Dispose();

            //    ssOrderCode.ActiveSheet.Cells[e.Row, 10].Text = clsOrdFunction.GstrSELECTSuCode;
            //    //ssOrderCode.ActiveSheet.Cells[e.Row, 9].Text = clsOrdFunction.GstrSELECTGbInfo;
            //    ssOrderCode.ActiveSheet.Cells[e.Row, 11].Text = clsOrdFunction.GstrSELECTGbInfo;
            //}

            switch (ssOrderCode.ActiveSheet.Cells[e.Row, 10].Text)
            {
                case "2":
                    clsOrdFunction.GstrSELECTDosCode = ssOrderCode.ActiveSheet.Cells[e.Row, 14].Text;
                    clsOrdFunction.GstrSELECTSlipnos = ssOrderCode.ActiveSheet.Cells[e.Row, 15].Text;
                    FrmMedViewSpecimen f = new FrmMedViewSpecimen();
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                    ssOrderCode.ActiveSheet.Cells[e.Row, 14].Text = clsOrdFunction.GstrSELECTDosCode;
                    ssOrderCode.ActiveSheet.Cells[e.Row, 15].Text = clsOrdFunction.GstrSELECTSlipnos;
                    break;
                default:
                    break;
            }

            clsOrdFunction.GstrSELECTGbImiv = ssOrderCode.ActiveSheet.Cells[e.Row, 17].Text;
            string SuCode = ssOrderCode.ActiveSheet.Cells[e.Row, 8].Text;

            switch (clsOrdFunction.GstrSELECTGbImiv)
            {
                case "4":
                    FrmViewEndoRemark f = new FrmViewEndoRemark();  //기관지
                    f.ShowDialog();
                    OF.fn_ClearMemory(f);
                    break;
                case "5":
                    FrmViewEndoRemark f1 = new FrmViewEndoRemark(); //위
                    f1.ShowDialog();
                    OF.fn_ClearMemory(f1);
                    break;
                case "6":
                    FrmViewEndoRemark f2 = new FrmViewEndoRemark(); //대장
                    f2.ShowDialog();
                    OF.fn_ClearMemory(f2);
                    break;
                case "7":
                    FrmViewEndoRemark f3 = new FrmViewEndoRemark(); //E.R.C.P
                    f3.ShowDialog();
                    OF.fn_ClearMemory(f3);
                    break;
                case "8":
                    clsOrdFunction.GstrAnatCode = ssOrderCode.ActiveSheet.Cells[e.Row, 2].Text;
                    clsOrdFunction.GstrAnatName = VB.Mid(ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text, 11, 50);

                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        FrmViewAnat f4 = new FrmViewAnat(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "O", true);   //Cytology
                        f4.ShowDialog();
                        OF.fn_ClearMemory(f4);
                    }
                    else
                    {
                        FrmViewAnat f4 = new FrmViewAnat(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", true);   //Cytology
                        f4.ShowDialog();
                        OF.fn_ClearMemory(f4);
                    }
                    break;
                case "9":
                    clsOrdFunction.GstrAnatCode = ssOrderCode.ActiveSheet.Cells[e.Row, 2].Text;
                    clsOrdFunction.GstrAnatName = VB.Mid(ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text, 11, 50);
                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        FrmViewAnat2 f5 = new FrmViewAnat2(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "O", SuCode); //Pathology
                        f5.ShowDialog();
                        OF.fn_ClearMemory(f5);
                    }
                    else
                    {
                        FrmViewAnat2 f5 = new FrmViewAnat2(clsOrdFunction.GstrAnatCode, clsOrdFunction.GstrAnatName, "I", SuCode); //Pathology
                        f5.ShowDialog();
                        OF.fn_ClearMemory(f5);
                    }                    
                    break;
                case "A":
                    clsOrdFunction.GstrAnatCode = ssOrderCode.ActiveSheet.Cells[e.Row, 2].Text;
                    clsOrdFunction.GstrAnatName = VB.Mid(ssOrderCode.ActiveSheet.Cells[e.Row, 4].Text, 11, 50);
                    strillName =  clsOrdFunction.Read_illName(clsOrdFunction.Pat.PtNo, clsOrdFunction.GstrBDate, clsOrdFunction.Pat.DeptCode);
                    if (clsOrdFunction.GstrGbJob == "OPD")
                    {
                        FrmViewAnat3 f6 = new FrmViewAnat3(strillName, "O"); //PB smear 소견
                        f6.ShowDialog();
                        OF.fn_ClearMemory(f6);
                    }
                    else
                    {
                        FrmViewAnat3 f6 = new FrmViewAnat3(strillName, "I"); //PB smear 소견
                        f6.ShowDialog();
                        OF.fn_ClearMemory(f6);
                    }
                    break;
                default:
                    break;
            }

            strSpecName = ssOrderCode.ActiveSheet.Cells[e.Row, 13].Text;

            lblLoad.Text = "안내 : " + ssOrderCode.ActiveSheet.Cells[e.Row, 11].Text + " " + strSpecName.Trim();

            clsOrdFunction.GnSort += 1;
            ssOrderCode.ActiveSheet.Cells[e.Row, 15].Text = clsOrdFunction.GnSort;
        }

        private void ssOrderCode_LeaveCell(object sender, FarPoint.Win.Spread.LeaveCellEventArgs e)
        {
            string strSpecName = "";
            string strOrderCode = "";
            string strBun = "";

            if (e.NewRow == -1 || e.NewColumn == -1)
            {
                return;
            }

            if (lblLoad.TextAlign != ContentAlignment.TopLeft)
            {
                lblLoad.TextAlign = ContentAlignment.TopLeft;
                lblLoad.ForeColor = Color.FromArgb(0, 0, 0);
            }

            strBun = ssOrderCode.ActiveSheet.Cells[e.NewRow, 9].Text.Trim();
            strOrderCode = ssOrderCode.ActiveSheet.Cells[e.NewRow, 2].Text.Trim();

            strSpecName = ssOrderCode.ActiveSheet.Cells[e.NewRow, 16].Text.Trim();
            lblLoad.Text = "안내 : " + ssOrderCode.ActiveSheet.Cells[e.NewRow, 12].Text.Trim();
        }

        private void ssOrderCode_KeyDown(object sender, KeyEventArgs e)
        {
            int nStartRow = 0;
            int i = 0;
            string strSearch = "";
            string strOrderNmae = "";
            string strSlipNo = "";

            strSearch = VB.UCase(e.KeyCode.ToString());
            
            //if (Int32.Parse(strSearch) < (int)'A' && Int32.Parse(strSearch) > (int)'Z')
            //{
            //    return; ;
            //}

            nStartRow = ssOrderCode.ActiveSheet.ActiveRowIndex + 1;

            if (nStartRow < 0)
            {
                nStartRow = 0;
                i = nStartRow;

                do
                {
                    if (i > ssOrderCode.ActiveSheet.RowCount)
                    {
                        break;
                    }

                    if (Convert.ToInt32(strSlipNo) < Convert.ToInt32("0010"))
                    {
                        strOrderNmae = VB.UCase(VB.Mid(ssOrderCode.ActiveSheet.Cells[i, 1].Text.Trim().ToUpper(), 11, 1));
                    }
                    else
                    {
                        strOrderNmae = VB.UCase(ssOrderCode.ActiveSheet.Cells[i, 1].Text.Trim());
                    }

                    if (strSearch == strOrderNmae)
                    {
                        ssOrderCode.ActiveSheet.ActiveRowIndex = i;
                        break;
                    }
                    i ++;
                } while (i == nStartRow - 1);
            }
        }

        private void btn가_Click(object sender, EventArgs e)
        {
            Read_Order("가", "나", "K");
        }

        private void btn나_Click(object sender, EventArgs e)
        {
            Read_Order("나", "다", "K");
        }

        private void btn다_Click(object sender, EventArgs e)
        {
            Read_Order("다", "라", "K");
        }

        private void btn라_Click(object sender, EventArgs e)
        {
            Read_Order("라", "마", "K");
        }

        private void btn마_Click(object sender, EventArgs e)
        {
            Read_Order("마", "바", "K");

        }

        private void btn바_Click(object sender, EventArgs e)
        {
            Read_Order("바", "사", "K");
        }

        private void btn사_Click(object sender, EventArgs e)
        {
            Read_Order("사", "아", "K");
        }

        private void btn아_Click(object sender, EventArgs e)
        {
            Read_Order("아", "자", "K");
        }

        private void btn자_Click(object sender, EventArgs e)
        {
            Read_Order("자", "차", "K");
        }

        private void btn차_Click(object sender, EventArgs e)
        {
            Read_Order("차", "카", "K");
        }

        private void btn카_Click(object sender, EventArgs e)
        {
            Read_Order("카", "타", "K");
        }

        private void btn타_Click(object sender, EventArgs e)
        {
            Read_Order("타", "파", "K");
        }

        private void btn파_Click(object sender, EventArgs e)
        {
            Read_Order("파", "하", "K");
        }

        private void btn하_Click(object sender, EventArgs e)
        {
            Read_Order("하", "하", "K");
        }

        private void ssOrderCode_EditModeOff(object sender, EventArgs e)
        {
            int nRow = ssOrderCode.ActiveSheet.ActiveRowIndex;
            int nCol = ssOrderCode.ActiveSheet.ActiveColumnIndex;

            if (ssOrderCode.ActiveSheet.Cells[nRow, 2].Text.Trim() == "S/O")
            {
                ssOrderCode.ActiveSheet.Cells[nRow, 0].Text = "True";
                ssOrderCode.ActiveSheet.AddRows(nRow + 1, 1);
                ssOrderCode.ActiveSheet.Cells[nRow + 1, 4].CellType = txt;
                ssOrderCode.ActiveSheet.Cells[nRow + 1, 4].Locked = false;
                ssOrderCode.ActiveSheet.Cells[nRow + 1, 1].Text = OF.fn_GetSlipName(clsDB.DbCon, clsOrdFunction.GstrSelSlipno);
                ssOrderCode.ActiveSheet.Cells[nRow + 1, 2].Text = "S/O";
                ssOrderCode.ActiveSheet.Cells[nRow + 1, 6].Text = "1";
                ssOrderCode.ActiveSheet.Cells[nRow + 1, 9].Text = "";
                ssOrderCode.ActiveSheet.Cells[nRow + 1, 15].Text = clsOrdFunction.GstrSelSlipno;
                ssOrderCode.ActiveSheet.SetActiveCell(nRow + 1, 4);
            }
        }

        private void txtSlipSearch_Click(object sender, EventArgs e)
        {
            ComFunc.StartLen(txtSlipSearch);
        }

        private void txtSlipSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (this.txtSlipSearch.Text != "" && e.KeyChar == (char)13)
            {
                if (lblSlipName.Text.Trim() == "")
                {
                    MessageBox.Show("좌측 슬립을 선택후 검색 바랍니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                clsOrdFunction.GstrSlipSearch = "Y";
                e.Handled = true;
                btnView_Click(btnView, new EventArgs());
                clsOrdFunction.GstrSlipSearch = "";
            }
        }

        private void btnSono_Click(object sender, EventArgs e)
        {
            frmComLibSonoBasis frm = new frmComLibSonoBasis();
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.ShowDialog(this);
        }
    }
}
