using ComBase;
using ComMirLibB.MirEnt;
using FarPoint.Win.Spread.CellType;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ComMirLibB.Com
{
    /// Class Name      : ComMirLibB.dll
    /// File Name       : frmComMirDtlCopy.cs
    /// Description     : 청구내역 조회 및 복사
    /// Author          : 박성완
    /// Create Date     : 2018-06-21
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history>
    /// <vbp>
    /// default : VB\PSMHH\mir\frmMirDtlView2.frm
    public partial class frmComMirDtlCopy : Form
    {
        private clsComMirEnt.ComMirEntInterface ComMirEntInterface = null;

        clsComMir.cls_Table_Mir_Insid TID = new clsComMir.cls_Table_Mir_Insid();
        clsComMirEntSpd MirEntSpd = new clsComMirEntSpd();
        clsComMirEntSQL MirEntSQL = new clsComMirEntSQL();
        clsSpread spread = new clsSpread();
        clsComMir cComMir = new clsComMir();

        int nRow = 0;
        int FnChoiceWRTNO = 0;
        string FstrWrtno = "";
        string FstrPano = "";
        string FstrSname = "";
        string FstrYYMM = "";
        string FstrOldNew = "";
        string FstrOpenJob = "";
        string FstrOpenIO = "";
        string FstrDTno = ""; //치과 구분 61 이면 수행
        string FstrAuto_ILL = "";

        //public frmComMirDtlCopy()
        //{
        //    InitializeComponent();

        //    SetEvent();
        //}

        public frmComMirDtlCopy(string argWrtno, string argPano, string argSname, string argYYMM, string argOldNew, string argOpenJob, string argOpenIO)
        {
            InitializeComponent();

            SetEvent();
            SetCtrl();
            FstrWrtno = argWrtno;
            FstrPano = argPano;
            FstrSname = argSname;
            FstrYYMM = argYYMM;
            FstrOldNew = argOldNew;
            FstrOpenJob = argOpenJob;
            FstrOpenIO = argOpenIO;

        }

        public frmComMirDtlCopy(string argWrtno, string argPano, string argSname, string argYYMM, string argOldNew, string argOpenJob, string argOpenIO, clsComMirEnt.ComMirEntInterface rComMirEntInterface)
        {
            InitializeComponent();

            SetEvent();
            SetCtrl();
            FstrWrtno = argWrtno;
            FstrPano = argPano;
            FstrSname = argSname;
            FstrYYMM = argYYMM;
            FstrOldNew = argOldNew;
            FstrOpenJob = argOpenJob;
            FstrOpenIO = argOpenIO;
            this.ComMirEntInterface = rComMirEntInterface;

        }

        public frmComMirDtlCopy(clsComMir.cls_Table_Mir_Insid argTID, string argWrtno, string argPano, string argSname, string argYYMM, string argOldNew, string argOpenJob, string argOpenIO, string argDTno, int argChoiceWRTNO, string argStrAuto_ILL, clsComMirEnt.ComMirEntInterface rComMirEntInterface)
        {
            InitializeComponent();

            SetEvent();
            SetCtrl();

            FstrWrtno = argWrtno;
            FstrPano = argPano;
            FstrSname = argSname;
            FstrYYMM = argYYMM;
            FstrOldNew = argOldNew;
            FstrOpenJob = argOpenJob;
            FstrOpenIO = argOpenIO;
            FstrDTno = argDTno;
            TID = argTID;
            FnChoiceWRTNO = argChoiceWRTNO;
            FstrAuto_ILL = argStrAuto_ILL;
            this.ComMirEntInterface = rComMirEntInterface;
        }

        private void SetCtrl()
        {
            MirEntSpd.sSpd_enmMirEntMainMirInsDtl(ssList, MirEntSpd.sSpdenmMirEntMainMirInsDtl, MirEntSpd.nSpdenmMirEntMainMirInsDtl, 12, 0);

            FarPoint.Win.Spread.GridLine HGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.None, Color.White);
            FarPoint.Win.Spread.GridLine VGridLine = new FarPoint.Win.Spread.GridLine(FarPoint.Win.Spread.GridLineType.Flat, Color.Black);
            ssList.Sheets[0].HorizontalGridLine = HGridLine;
            ssList.Sheets[0].VerticalGridLine = VGridLine;

            ssList.ActiveSheet.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].BackColor = Color.FromArgb(206, 255, 206);
            ssList.ActiveSheet.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.KTASLEVL].BackColor = Color.FromArgb(206, 255, 206);
            ssList.ActiveSheet.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Div].BackColor = Color.FromArgb(255, 215, 215);
            ssList.ActiveSheet.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbSelf].BackColor = Color.FromArgb(255, 215, 215);

            ssList.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT - 6);
            ssList.ActiveSheet.FrozenColumnCount = (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.CBODRBUNHO;

            CheckBoxCellType spdObj = new CheckBoxCellType();                        
            
            ssList.ActiveSheet.ColumnHeader.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].CellType = spdObj;
            ssList.ActiveSheet.ColumnHeader.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Label = "False";
            ssList.ActiveSheet.ColumnHeader.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].HorizontalAlignment = FarPoint.Win.Spread.CellHorizontalAlignment.Center;
            ssList.ActiveSheet.ColumnHeader.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].VerticalAlignment = FarPoint.Win.Spread.CellVerticalAlignment.Center;
        }

        private void SetEvent()
        {
            this.Load += FrmComMirDtlCopy_Load;
            this.btnCopy.Click += BtnCopy_Click;
            this.btnExit.Click += BtnExit_Click;

            this.ssList.CellClick += SsList_CellClick;   //스프레드 전체 체크박스 클릭 로직 
            this.ss1.CellClick += Ss1_CellClick;

            this.optGB0.Click += OptGB_Click;
            this.optGB1.Click += OptGB_Click;
            this.optGB2.Click += OptGB_Click;
        }

        private void OptGB_Click(object sender, EventArgs e)
        {
            READ_MIR_INSID();
        }

        private void Ss1_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader || e.RowHeader)
            {
                return;
            }

            Read_Mir_DTL_NEW((int)VB.Val(ss1.ActiveSheet.Cells[e.Row, (int)clsComMirEntSpd.enmMirEntPanoChoice.Wrtno].Text));
        }

        private void Read_Mir_DTL_NEW(int nWrtno)
        {
            int nRow = 0;
            int i = 0;
            string strSuNext = "";
            string strSScode = "";
            string strSSgisul = "";
            string strSSbun = "";
            string strCtFrDate = "";
            string strGBSAK = "";
            string strIllCode = "";
            string strColorSet = "";

            double nDrugAmt = 0;
            double nSamt = 0;

            double GnbAmt_new = 0;
            int nGetCountAll = 0;
            int nRowMaxData = 0;

            ssList.ActiveSheet.RowCount = 0;

            DataTable dt2 = null;
            dt2 = MirEntSQL.sel_Mir_InsDtl(clsDB.DbCon, nWrtno, "A");

            #region //데이터셋 읽어 자료 표시

            if (ComFunc.isDataTableNull(dt2) == false)
            {
                ssList.ActiveSheet.RowCount = dt2.Rows.Count;
                //ssList.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT - 3);
                nGetCountAll = dt2.Rows.Count;
                //MirEnt.nGetCountCur = dt2.Rows.Count;
                //MirEnt.nGetCountAll = MirEnt.nGetCountAll + nILLCNT + dt2.Rows.Count;
                //nSSlistHeight = (nGetCountAll + 4) * ConSSlistRowHeight
                //If ConMaxSSlist < nSSlistHeight Then nSSlistHeight = ConMaxSSlist
                //If SSList.MaxRows < nGetCountAll Then SSList.MaxRows = nGetCountAll

                for (i = 0; i < dt2.Rows.Count; i++)
                {
                    if (strSuNext == "########" && strSuNext != dt2.Rows[i]["Sunext"].ToString().Trim())
                    {
                        if (i > 3)
                        {
                            ssList.ActiveSheet.FrozenRowCount = 3;
                        }
                        else
                        {
                            ssList.ActiveSheet.FrozenRowCount = i;
                        }
                        //ssList.ActiveSheet.FrozenTrailingRowCount = nRow;
                        //spread.setColStyle(ssList, nRow, -1, clsSpread.enmSpdType.Label);
                        spread.setColStyle(ssList, nRow, 0, clsSpread.enmSpdType.Label);

                        FarPoint.Win.LineBorder lb = new FarPoint.Win.LineBorder(Color.Blue, 1, false, false, false, true);
                        ssList.ActiveSheet.Rows[i - 1].Border = lb;

                    }
                    //nRow = nRow + 1;
                    strSScode = dt2.Rows[i]["Sunext"].ToString().Trim();

                    //if ( strSScode =="R4165")
                    //{
                    //    i = i;
                    //}

                    strSSgisul = dt2.Rows[i]["GbGisul"].ToString().Trim();
                    strSSbun = dt2.Rows[i]["Bun"].ToString().Trim();
                    if (strSSbun == "72") { strCtFrDate = dt2.Rows[i]["FrDate"].ToString().Trim(); }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SeqNo1].Text = dt2.Rows[i]["Seqno1"].ToString().Trim();
                    //ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SeqNo2].Text = dt2.Rows[i]["Seqno2"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SeqNo2].Text = "0";
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.ItemGubun].Text = dt2.Rows[i]["ItemGubun"].ToString().Trim();

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Bun].Text = dt2.Rows[i]["Bun"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbGisul].Text = dt2.Rows[i]["GbGisul"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbChild].Text = dt2.Rows[i]["GbChild"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbChild_OLD].Text = dt2.Rows[i]["GbChild"].ToString().Trim(); //old

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.UpCheck].Text = dt2.Rows[i]["UpCheck"].ToString().Trim();

                    //ssList.ActiveSheet.Protect = true;
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Locked = false;

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunext].Text = dt2.Rows[i]["Sunext"].ToString();
                    strSuNext = dt2.Rows[i]["Sunext"].ToString().Trim();

                    if (dt2.Rows[i]["GBSAK"].ToString().Trim() == "Y")
                    {
                        strGBSAK = "★";
                    }
                    else
                    {
                        strGBSAK = "";
                    }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Text = strGBSAK + dt2.Rows[i]["Sunamek"].ToString().Trim();

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Locked = true;
                    if (dt2.Rows[i]["GBDR"].ToString().Trim() == "Y")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Font = new Font("굴림", 9, FontStyle.Underline);
                    }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Samt].Text = dt2.Rows[i]["Samt"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Price].Text = dt2.Rows[i]["Price"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Qty].Text = dt2.Rows[i]["Qty"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Nal].Text = dt2.Rows[i]["Nal"].ToString().Trim();

                    //'약 주사만 표시
                    if (dt2.Rows[i]["Bun"].ToString().Trim() == "11" || dt2.Rows[i]["Bun"].ToString().Trim() == "12" || dt2.Rows[i]["Bun"].ToString().Trim() == "20")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Div].Text = dt2.Rows[i]["Div"].ToString().Trim();
                    }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].Text = dt2.Rows[i]["GbNgt"].ToString().Trim();


                    if (dt2.Rows[i]["GbNgt"].ToString().Trim() != "0" && dt2.Rows[i]["GbNgt"].ToString().Trim() != "" && strSScode != "########")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].ForeColor = Color.Yellow;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].BackColor = Color.Red;
                    }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbSelf].Text = dt2.Rows[i]["GbSelf"].ToString().Trim();
                    if (dt2.Rows[i]["GbSelf"].ToString().Trim() == "1" || dt2.Rows[i]["GbSelf"].ToString().Trim() == "2")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].ForeColor = Color.FromArgb(153, 204, 0);
                    }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.KTASLEVL].Text = dt2.Rows[i]["KTASLEVL"].ToString().Trim();
                    if (dt2.Rows[i]["KTASLEVL"].ToString().Trim() == "1" || dt2.Rows[i]["KTASLEVL"].ToString().Trim() == "2" || dt2.Rows[i]["KTASLEVL"].ToString().Trim() == "3")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.KTASLEVL].BackColor = Color.Yellow;
                    }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SugbAA].Text = dt2.Rows[i]["SugbAA"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.KTASLEVL_OLD].Text = dt2.Rows[i]["KTASLEVL"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.KTASLEVL_OLD].Text = dt2.Rows[i]["KTASLEVL"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Amt].Text = dt2.Rows[i]["Amt"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.DrugAmt].Text = dt2.Rows[i]["DrugAmt"].ToString().Trim();
                    nDrugAmt = 0; nSamt = 0;

                    #region 약제상한차액 
                    nSamt = MirEntSQL.READ_BAS_SUGA_SAMT(clsDB.DbCon, dt2.Rows[i]["FrDate"].ToString().Trim(), dt2.Rows[i]["Sunext"].ToString().Trim(), VB.Left(TID.JinDate1, 4) + "-" + VB.Mid(TID.JinDate1, 5, 2) + "-" + VB.Mid(TID.JinDate1, 7, 2));
                    if (nSamt != 0)
                    {
                        nDrugAmt = cComMir.Account_DrugAmt_NEW(nSamt, Convert.ToInt32(dt2.Rows[i]["Qty"]), Convert.ToInt32(dt2.Rows[i]["Nal"]), dt2.Rows[i]["BUN"].ToString().Trim(), dt2.Rows[i]["Sunext"].ToString().Trim());

                        if (dt2.Rows[i]["DrugAmt"].ToString().Trim() != "")
                        {
                            if (Convert.ToInt32(dt2.Rows[i]["DrugAmt"].ToString().Trim()) != nDrugAmt)
                            {
                                ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.DrugAmt].Text = nDrugAmt.ToString();
                                SS_Change_Mark(i);
                            }
                        }
                    }
                    else
                    {
                        if (dt2.Rows[i]["DrugAmt"].ToString().Trim() != "")
                        {
                            if (Convert.ToInt32(dt2.Rows[i]["DrugAmt"].ToString().Trim()) != 0)
                            {
                                ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.DrugAmt].Text = "0";
                                SS_Change_Mark(i);
                            }
                        }
                    }
                    #endregion


                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.WonSayu].Text = dt2.Rows[i]["WonSayu"].ToString().Trim();
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Remark].Text = dt2.Rows[i]["Remark"].ToString().Trim();


                    if (strSScode == "********" || strSScode == "########")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.UpCheck].Text = "*";

                        if (dt2.Rows[i]["ILLCODE"].ToString().Trim() == "")
                        { strIllCode = VB.Left(dt2.Rows[i]["ILLCODE"].ToString().Trim(), 6); }
                        else
                        {
                            strIllCode = dt2.Rows[i]["ILLCODE"].ToString().Trim();
                        }

                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Text = VB.Left(strIllCode + VB.Space(10), 10) + MirEntSQL.sel_ILL_NAME(clsDB.DbCon, strIllCode.Trim(), "1", TID.IpdOpd, TID.IODate, TID.OutDate, TID.JinIlsu, TID.JinDate1);

                        if (dt2.Rows[i]["ILLCODE"].ToString().Trim() == "") SS_Change_Mark(i);

                        //ssList.ActiveSheet.Protect = true;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Locked = false;

                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Samt].Locked = true;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Price].Locked = true;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Qty].Locked = true;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Nal].Locked = true;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Div].Locked = true;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].Locked = false;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Amt].Locked = true;
                        //FarPoint.Win.Spread.Row r;
                        //r = fpSpread1.ActiveSheet.Rows[0];
                        //r.BackColor = Color.Yellow;

                        ssList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(255, 255, 220);
                        //ssList.ActiveSheet.Cells[i, -1].BackColor = Color.FromArgb(255, 255, 220);

                        //해당로직 검검후 삭제예정
                        if (dt2.Rows[i]["GBGISUL"].ToString().Trim() == "3")
                        {//SSList.Col = 16: SSList.BackColor = vbBlue
                            ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].BackColor = Color.Blue;
                        }

                        if (dt2.Rows[i]["GbNgt"].ToString().Trim() != "0" && dt2.Rows[i]["GbNgt"].ToString().Trim() != "")
                        {
                            ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].ForeColor = Color.FromArgb(210, 0, 255);
                        }
                    }
                    else
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunext].Locked = true;
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].Locked = true;

                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Memos].Text = dt2.Rows[i]["Memos"].ToString();
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.FrDate].Text = dt2.Rows[i]["FrDate"].ToString().Trim();
                        //if (dt2.Rows[i]["Memos2"].ToString().Trim() != "" && TID.DeptCode1 != "DT")
                        //{
                        //    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Memos].Text = dt2.Rows[i]["Memos2"].ToString();
                        //}
                    }



                    if (strSSgisul == "0")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].Locked = false;
                    }

                    if (strSSgisul != "0" && strSSgisul != "")
                    {
                        if (strSSbun == "22" || strSSbun == "28" || strSSbun == "34" || strSSbun == "35" || strSSbun == "38")
                        { ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].Locked = false; }
                        else
                        { ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].Locked = true; }
                    }


                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.A35].Text = "";
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.A36].Text = "";


                    //EDI내역을 Display
                    //SSList.Col = 37: SSList.Text = Format(AdoGetNumber(RsSub, "EdiSeq", I), "###")
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EdiSeq].Text = dt2.Rows[i]["EdiSeq"].ToString().Trim();


                    //SSList.Col = 38: SSList.Text = AdoGetString(RsSub, "EdiCode", I)
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EdiCode].Text = dt2.Rows[i]["EdiCode"].ToString().Trim();

                    //SSList.Col = 39: SSList.Text = Format(AdoGetNumber(RsSub, "EdiPrice", I), "#########0.0")
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EdiPrice].Text = dt2.Rows[i]["EdiPrice"].ToString().Trim();

                    //SSList.Col = 40: SSList.Text = Format(AdoGetNumber(RsSub, "EdiQty", I), "#####0.00")
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EdiQty].Text = dt2.Rows[i]["EdiQty"].ToString().Trim();

                    //SSList.Col = 41: SSList.Text = Format(AdoGetNumber(RsSub, "EdiNal", I), "###")
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EdiNal].Text = dt2.Rows[i]["EdiNal"].ToString().Trim();

                    //SSList.Col = 42: SSList.Text = Format(AdoGetNumber(RsSub, "EdiAmt", I), "##########")
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EdiNal].Text = dt2.Rows[i]["EdiNal"].ToString().Trim();

                    //SSList.Col = 43: SSList.Text = Format(AdoGetNumber(RsSub, "EdiDRUGAmt", I), "##########")
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EdiDRUGAmt].Text = dt2.Rows[i]["EdiDRUGAmt"].ToString().Trim();


                    //'수정내역 관리용 칼럼을 Hidden(2001.8.16)
                    //SSList.Col = 44: SSList.Text = AdoGetNumber(RsSub, "Price", I) 'Old단가
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.OLDPrice].Text = dt2.Rows[i]["Price"].ToString().Trim();

                    //SSList.Col = 45: SSList.Text = AdoGetNumber(RsSub, "Qty", I) 'Old수랑
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.OLDQty].Text = dt2.Rows[i]["Qty"].ToString().Trim();
                    //SSList.Col = 46: SSList.Text = AdoGetNumber(RsSub, "Nal", I) 'Old날수
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.OLDNal].Text = dt2.Rows[i]["Nal"].ToString().Trim();
                    //SSList.Col = 47: SSList.Text = AdoGetNumber(RsSub, "AMT", I) 'Old금액
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.OLDAMT].Text = dt2.Rows[i]["AMT"].ToString().Trim();



                    //SSList.Col = 48: SSList.Text = AdoGetNumber(RsSub, "WRTNOS", I) '특정내역번호
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.WRTNOS].Text = dt2.Rows[i]["WRTNOS"].ToString().Trim();

                    if (dt2.Rows[i]["WRTNOS"].ToString().Trim() != "")
                    {
                        if (Convert.ToInt32(dt2.Rows[i]["WRTNOS"].ToString().Trim()) != 0) ssList.ActiveSheet.Rows[i].ForeColor = Color.FromArgb(0, 128, 0);
                    }

                    //SSList.Col = 49: SSList.Text = AdoGetString(RsSub, "Rowid", I) 'mir_insdtl rowid
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Rowid].Text = dt2.Rows[i]["Rowid"].ToString().Trim();
                    //SSList.Col = 50: SSList.Text = AdoGetNumber(RsSub, "DIVQTY", I) '1회 투여량
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.DIVQTY].Text = dt2.Rows[i]["DIVQTY"].ToString().Trim();
                    //SSList.Col = 51: SSList.Text = AdoGetNumber(RsSub, "EDIDIVQTY", I) 'EDI 1회 투여량
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EDIDIVQTY].Text = dt2.Rows[i]["EDIDIVQTY"].ToString().Trim();
                    //SSList.Col = 52: SSList.Text = AdoGetNumber(RsSub, "EDIDIV", I)   'EID 횟수
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.EDIDIV].Text = dt2.Rows[i]["EDIDIV"].ToString().Trim();

                    //SSList.Col = 53: SSList.Text = AdoGetString(RsSub, "SCODESAYU", I) 'EDI 1회 투여량
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SCODESAYU].Text = dt2.Rows[i]["SCODESAYU"].ToString().Trim();
                    if (dt2.Rows[i]["SCODESAYU"].ToString().Trim() != "") { ssList.ActiveSheet.Cells[i, -1].BackColor = Color.FromArgb(255, 230, 230); }


                    //SSList.Col = 54: SSList.Text = AdoGetString(RsSub, "SCODEREMARK", I)   'EID 횟수
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SCODEREMARK].Text = dt2.Rows[i]["SCODEREMARK"].ToString().Trim();


                    //SSList.Col = 55: SSList.Text = AdoGetString(RsSub, "ILLCODE", I)   'ILLCODE
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.ILLCODE].Text = dt2.Rows[i]["ILLCODE"].ToString().Trim();
                    //SSList.Col = 56: SSList.Text = AdoGetString(RsSub, "GbSELF", I)
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.OLDGbSelf].Text = dt2.Rows[i]["GbSELF"].ToString().Trim();

                    //SSList.Col = 57: SSList.Text = AdoGetString(RsSub, "DRBUNHO", I)   '
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.DRBUNHO].Text = dt2.Rows[i]["DRBUNHO"].ToString().Trim();

                    //COMBOBOX에 값을 넣어서 조회 되도록 처리
                    //cbo_Drbunho_set(dt2.Rows[i]["DRBUNHO"].ToString().Trim(),i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.CBODRBUNHO);
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.CBODRBUNHO].Text = Read_Drbunho_Set2(dt2.Rows[i]["DRBUNHO"].ToString().Trim());


                    //SSList.Col = 58: SSList.Text = AdoGetString(RsSub, "DRAUTO", I)
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.DRAUTO].Text = dt2.Rows[i]["DRAUTO"].ToString().Trim();
                    //SSList.Col = 59: SSList.Text = AdoGetString(RsSub, "GBSUGBS", I)
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.OLDGBSUGBS].Text = dt2.Rows[i]["GBSUGBS"].ToString().Trim();

                    //SSList.Col = 65: SSList.Text = AdoGetString(RsSub, "SUGBAC", I)
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SUGBAC].Text = dt2.Rows[i]["SUGBAC"].ToString().Trim();
                    //SSList.Col = 66: SSList.Text = AdoGetString(RsSub, "GBGSADD", I)
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GBGSADD].Text = dt2.Rows[i]["GBGSADD"].ToString().Trim();
                    //SSList.Col = 24:      SSList.Text = Trim(AdoGetString(RsSub, "GBSUGBS", I))      '2016-09-19
                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GBSUGBS].Text = dt2.Rows[i]["GBSUGBS"].ToString().Trim();

                    //if (Convert.ToInt32(dt2.Rows[i]["GBSUGBS"].ToString()) >= 4)
                    if (dt2.Rows[i]["GBSUGBS"].ToString() != null && dt2.Rows[i]["GBSUGBS"].ToString() != "")
                    {
                        if (Convert.ToInt32(dt2.Rows[i]["GBSUGBS"].ToString()) >= 4)
                        {
                            //SSList.ForeColor = RGB(255, 0, 255)
                            //SSList.FontBold = True      '2018-02-23
                            ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GBSUGBS].ForeColor = Color.FromArgb(255, 0, 255);
                        }
                    }

                    if (dt2.Rows[i]["GBSUGBS"].ToString().Trim() == "6")
                    {
                        //            SSList.CellType = SS_CELL_TYPE_EDIT
                        //            SSList.TypeTextOrient = SS_CELL_TEXTORIENT_HORIZONTAL
                        //            SSList.TypeEllipses = False
                        //            SSList.TypeHAlign = SS_CELL_H_ALIGN_CENTER
                        //            SSList.TypeVAlign = SS_CELL_V_ALIGN_VCENTER
                        //            SSList.TypeEditCharSet = SS_CELL_EDIT_CHAR_SET_ASCII
                        //            SSList.TypeEditCharCase = SS_CELL_EDIT_CASE_NO_CASE
                        //            SSList.TypeEditMultiLine = False
                        //            SSList.TypeEditPassword = False
                        //            SSList.TypeMaxEditLen = 1
                    }

                    ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GBSAKDTL].Text = dt2.Rows[i]["GBSAKDTL"].ToString().Trim();

                    //SSList.Col = 63: SSList.Text = ""
                    if (dt2.Rows[i]["SugbAA"].ToString().Trim() == "1" || dt2.Rows[i]["SugbAA"].ToString().Trim() == "2" || dt2.Rows[i]["SugbAA"].ToString().Trim() == "3" || dt2.Rows[i]["Bun"].ToString().Trim() == "35")
                    {
                        GnbAmt_new = 0;

                        if (dt2.Rows[i]["FrDate"].ToString().Trim() != "")
                        {
                            GnbAmt_new = MirEntSQL.Suga_Read_Amt_NEW2(clsDB.DbCon, strSuNext, dt2.Rows[i]["FrDate"].ToString().Trim());
                        }
                        else
                        {
                            GnbAmt_new = MirEntSQL.Suga_Read_Amt_NEW2(clsDB.DbCon, strSuNext, TID.JinDate1);
                        }

                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.ER_Base].Text = GnbAmt_new.ToString();
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.SugbB].Text = MirEntSQL.sel_BAS_SUT_B2(clsDB.DbCon, strSuNext);
                    }


                    if (strSScode == "R4165")
                    {
                        //strSScode = strSScode;
                    }

                    if (VB.Val(dt2.Rows[i]["GbNgt"].ToString().Trim()) >= 1)
                    {

                        ssList.ActiveSheet.Rows[i].ForeColor = Color.FromArgb(210, 0, 255);
                        if (strSScode != "********" && strSScode != "########")
                        {
                            ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.GbNgt].ForeColor = Color.Yellow;
                        }

                    }


                    // if (((TID.IpdOpd == "O" && TID.DeptCode1 == "ER") || TID.IpdOpd == "I") && Convert.ToInt32(dt2.Rows[i]["WRTNOS"].ToString().Trim()) > 0)

                    if (((TID.IpdOpd == "O" && TID.DeptCode1 == "ER") || TID.IpdOpd == "I") && VB.Val(dt2.Rows[i]["WRTNOS"].ToString().Trim()) > 0)
                    {
                        ssList.ActiveSheet.Rows[i].ForeColor = Color.FromArgb(0, 128, 0);
                    }

                    strColorSet = MirEntSQL.Read_Mir_COLOR_SET(clsDB.DbCon, clsType.User.IdNumber, dt2.Rows[i]["Sunext"].ToString().Trim());

                    if (strColorSet != "")
                    {
                        //SSList.Col = -1: SSList.BackColor = AdoGetString(rs2, "RGB", 0)
                        //ssList.ActiveSheet.Rows[i].BackColor = Color.FromArgb(strColorSet);  // TODO: 색깔
                        int intValue = (int)VB.Val(strColorSet);
                        byte[] bytes = BitConverter.GetBytes(intValue);
                        Color c = Color.FromArgb(bytes[0], bytes[1], bytes[2]);
                        //string hexValue = intValue.ToString("X");
                        //Color c = System.Drawing.ColorTranslator.FromHtml("#" + hexValue);
                        ssList.ActiveSheet.Rows[i].BackColor = c;
                    }

                    if (dt2.Rows[i]["GBSAK"].ToString().Trim() == "Y")
                    {
                        //SSList.Col = 10:      SSList.BackColor = RGB(255, 100, 100);
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunamek].BackColor = Color.FromArgb(255, 100, 100);
                    }

                    if (dt2.Rows[i]["WRTNOS"].ToString().Trim() != "")       //'2016-08-26 줄단위 특정내역 있으면 체크
                    {
                        if (Convert.ToInt32(dt2.Rows[i]["WRTNOS"].ToString().Trim()) > 0)
                        {
                            if (MirEntSQL.sel_MIR_IPDSPEC(clsDB.DbCon, TID.Pano, TID.JinDate1, TID.IPDNO, dt2.Rows[i]["Sunext"].ToString().Trim()) != "")
                            {
                                ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Sunext].BackColor = Color.FromArgb(255, 128, 255);
                            }
                        }
                    }

                    //'입원 진료일수 색상표시 2016-11-11
                    if (TID.IpdOpd == "I" && TID.JinIlsu > 0 && TID.JinIlsu < Convert.ToInt32(dt2.Rows[i]["Nal"].ToString().Trim()) && (Convert.ToInt32(dt2.Rows[i]["Bun"].ToString().Trim()) >= 41 && Convert.ToInt32(dt2.Rows[i]["Bun"].ToString().Trim()) <= 73 || dt2.Rows[i]["SuNext"].ToString().Trim() == "C-MD"))
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Nal].BackColor = Color.Red;
                    }

                    if (FstrAuto_ILL == "Y" && dt2.Rows[i]["AUTODEL"].ToString().Trim() == "D")
                    {
                        ssList.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Value = true; //"1";
                        ssList.ActiveSheet.Rows[i].ForeColor = Color.Red;
                        SS_Change_Mark(i);
                    }
                }
                nRowMaxData = nGetCountAll;

                FarPoint.Win.LineBorder lb2 = new FarPoint.Win.LineBorder(Color.Blue, 1, false, false, false, true);
                ssList.ActiveSheet.Rows[nRowMaxData - 1].Border = lb2;
            }
            #endregion
        }


        //TODO: MIRENT 폼안의 함수
        private void SS_Change_Mark(int argRow)
        {
            if (ssList.ActiveSheet.Cells[argRow, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.UpCheck].Text == "*") ssList.ActiveSheet.Cells[argRow, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.UpCheck].Text = "R";
            if (ssList.ActiveSheet.Cells[argRow, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.UpCheck].Text == "") ssList.ActiveSheet.Cells[argRow, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.UpCheck].Text = "U";
        }

        //TODO: MIRENT 폼안의 함수
        private string Read_Drbunho_Set2(string strDrBunho)
        {
            string retVal = "";

            string strDrName = "";
            if (strDrBunho == "") return retVal;


            string[] words = strDrBunho.Split('/');

            foreach (var word in words)
            {
                strDrName = $"{word}";
                if (strDrName != "")
                {
                    retVal = retVal + " " + MirEntSQL.READ_OCS_Doctor_DrName(clsDB.DbCon, strDrName);

                }
            }

            return retVal;
        }


        




       

        private void SsList_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.ColumnHeader == true && e.Column == (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check && ssList.ActiveSheet.Rows.Count > 0)
            {
                if (ssList.ActiveSheet.ColumnHeader.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Label == "True")
                {
                    ssList.ActiveSheet.ColumnHeader.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Label = "False";

                    ssList.ActiveSheet.Cells[0, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check, ssList.ActiveSheet.Rows.Count - 1, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Text = "False";
                }
                else
                {
                    ssList.ActiveSheet.ColumnHeader.Columns[(int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Label = "True";

                    ssList.ActiveSheet.Cells[0, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check, ssList.ActiveSheet.Rows.Count - 1, (int)clsComMirEntSpd.enmMirEntMainMirInsDtl.Check].Text = "True";
                }
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            READ_MIR_OUTDRUG();
            this.Close();
        }

        //TODO:VB에서는 BAS파일의 함수를 사용하며 스프레드는 주로 메인화면 것을 제어한다.
        private void READ_MIR_OUTDRUG()
        {            
            //int i = 0;
            //int j = 0;
            //int nRow = 0;
            //int nRow2 = 0;
            //int nREAD = 0;
            //int nREAD2 = 0;
            //string strSlipDate = "";
            //int nSlipNo = 0;
            //string strFlag = "";

            //string SQL = "";
            //string SqlErr = "";

            //DataTable dt = null;
            //DataTable dt1 = null;

            //TODO:
            //FrmEntryMain.Txt원외처방상병.Text = ""

            //SQL = "";
            //SQL += "SELECT TO_CHAR(a.SlipDate,'YYYYMMDD') SlipDate,a.SlipNo, A.GBV252," + ComNum.VBLF;
            //SQL += "       TO_CHAR(a.BDate,'YYYY-MM-DD') BDate,a.Bi,a.DeptCode, " + ComNum.VBLF;
            //SQL += "       TO_CHAR(a.ActDate,'YYYY-MM-DD') ActDate,a.WRTNO, " + ComNum.VBLF;
            //SQL += "       MAX(DECODE(b.Bun,'11',b.Nal,0)) Bun11," + ComNum.VBLF;
            //SQL += "       MAX(DECODE(b.Bun,'12',b.Nal,0)) Bun12," + ComNum.VBLF;
            //SQL += "       MAX(DECODE(b.Bun,'20',b.Nal,0)) Bun20, A.DIEASE1 " + ComNum.VBLF;
            //SQL += "  FROM KOSMOS_PMPA.MIR_OUTDRUGMST a,KOSMOS_PMPA.MIR_OUTDRUG b " + ComNum.VBLF;
            //SQL += " WHERE a.Pano = '" + TID.Pano + "' " + ComNum.VBLF;
            //SQL += "   AND a.WRTNO=" + TID.WRTNO + " " + ComNum.VBLF;      
            //if (TID.Bi == "31")
            //{
            //    SQL = SQL + "  AND A.BI = '31' " + ComNum.VBLF;
            //}
            //else
            //{
            //    SQL = SQL + "  AND A.BI NOT IN  '31' " + ComNum.VBLF;
            //}
            //SQL = SQL + "   AND a.SlipDate=b.SlipDate " + ComNum.VBLF;
            //SQL = SQL + "   AND a.SlipNo  =b.SlipNo   " + ComNum.VBLF;
            //SQL = SQL + " GROUP BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode,a.ActDate, a.WRTNO, A.GBV252, A.DIEASE1 " + ComNum.VBLF;
            //SQL = SQL + " ORDER BY a.SlipDate,a.SlipNo,a.BDate,a.Bi,a.DeptCode " + ComNum.VBLF;
            //SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //nREAD = dt.Rows.Count;
            //nRow = 0;
            //nRow2 = 0;
            //strFlag = "";
            
        }

        private void BtnCopy_Click(object sender, EventArgs e)
        {
            int i = 0;
            //int j = 0;
            //int k = 0;

            string strChk = "";
            string strSuNext = "";
            //string strSuName = "";
            //long nQty = 0;
            //long nNal = 0;
            //long nPrice = 0;
            //string strFrDate = "";
            //long nAmt = 0;
            string strBun = "";
            //int nCol = 0;
            //int nRow = 0;
            //string strOK = "";
            long nSSAmt = 0;
            //int nSubscriptno = 0;

            for (i = 0; i < ssList.ActiveSheet.NonEmptyRowCount; i++)
            {
                strChk = ssList.ActiveSheet.Cells[i, 8].Text.Trim();
                strSuNext = ssList.ActiveSheet.Cells[i, 9].Text;

                strBun = ssList.ActiveSheet.Cells[i, 4].Text;
                nSSAmt = (long)VB.Val(ssList.ActiveSheet.Cells[i, 29].Text);

                if (strChk == "True")
                {

                    ComMirEntInterface.GetSpread(ssList,i);  //interface
                                                         
                }
            }
        }

        private void FrmComMirDtlCopy_Load(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}
            if (string.IsNullOrEmpty(FstrWrtno))
            {
                return;
            }

            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            READ_MIR_INSID();
        }

        private void READ_MIR_INSID()
        {
            if (string.IsNullOrEmpty(FstrYYMM))
            {
                return;
            }

            lblPano.Text = FstrPano;
            lblName.Text = FstrSname;

            MirEntSpd.sSpd_enmMirEntPanoChoice(ss1, MirEntSpd.sSpdenmMirEntPanoChoice, MirEntSpd.nSpdenmMirEntPanoChoice, 20, 0);

            screen_display();

            //int i = 0;
            //int j = 0;
            //int nREAD = 0;

            //string SQL = "";
            //string SqlErr = "";
            //DataTable dt = null;

            //int YYYY = Convert.ToInt32(VB.Left(FstrYYMM, 4));
            //int MM = Convert.ToInt32(VB.Right(FstrYYMM, 2));

            //DateTime YYMM = new DateTime(YYYY, MM, 1);

            //ss1.ActiveSheet.Columns[11].Visible = false;

            //lblPano.Text = FstrPano;
            //lblName.Text = FstrSname;

            //btnCopy.Enabled = false;

            //SQL = "";
            //SQL += " SELECT YYMM, WRTNO, Seqno,Pano, Sname, Kiho, Gkiho, RateBon,DTno, BohoJong, " + ComNum.VBLF;
            //SQL += "        TO_CHAR(OutDate, 'YYYY-MM-DD') OutDay, Upcnt1, Edimirno, Jindate1 , DEPTCODE1,StopFLAG, SCODE, JAMT, TAMT, BAMT  " + ComNum.VBLF;
            //SQL += "   FROM " + FstrOldNew + "MIR_INSID " + ComNum.VBLF;
            //SQL += "  WHERE IpdOpd = '" + FstrOpenIO + "'" + ComNum.VBLF;
            //SQL += "    AND Johap  = '" + FstrOpenJob + "'" + ComNum.VBLF;
            //SQL += "    AND Pano   = '" + FstrPano + "'" + ComNum.VBLF;
            //SQL += "    AND WRTNO <> '" + FstrWrtno + "' " + ComNum.VBLF;

            //if (optGB0.Checked == true) { SQL += "    AND YYMM >=  '" + YYMM.AddMonths(-6).ToString("yyyyMM") + "'" + ComNum.VBLF; }
            //if (optGB1.Checked == true) { SQL += "    AND YYMM >=  '" + YYMM.AddMonths(-12).ToString("yyyyMM") + "'" + ComNum.VBLF; }

            ////치의과구분
            //if (FstrDTno == "61")
            //{
            //    SQL += "   AND DTno   = '61' " + ComNum.VBLF;
            //}
            //else
            //{
            //    SQL += "   AND DTno   != '61' " + ComNum.VBLF;
            //}

            //SQL += " ORDER BY JINDATE1 DESC , RATEBON ";
            //SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

            //nREAD = dt.Rows.Count;

            //ss1.ActiveSheet.Rows.Count = 0;
            //ss1.ActiveSheet.Rows.Count = nREAD;

            //for (i = 0; i < nREAD; i++)
            //{
            //    ss1.ActiveSheet.Cells[i, 0].Text = (dt.Rows[i]["EdiMirNo"].ToString().CompareTo("0") >= 0) ? "E" : "";

            //    if (dt.Rows[i]["UpCnt1"].ToString() == "9")
            //    {
            //        if (dt.Rows[i]["EdiMirNo"].ToString().CompareTo("0") <= 0)
            //        {
            //            ss1.ActiveSheet.Cells[i, 1].Text = "보";
            //        }
            //        else
            //        {
            //            ss1.ActiveSheet.Cells[i, 1].Text = "반";
            //        }
            //    }
            //    else
            //    {
            //        ss1.ActiveSheet.Cells[i, 1].Text = "    ";
            //    }

            //    //JJY(2003-04-21)
            //    switch (dt.Rows[i]["BohoJong"].ToString())
            //    {
            //        case "1": ss1.ActiveSheet.Cells[i, 2].Text = "재"; break;
            //        case "2": ss1.ActiveSheet.Cells[i, 2].Text = "추"; break;
            //        default: ss1.ActiveSheet.Cells[i, 2].Text = "원"; break;
            //    }

            //    ss1.ActiveSheet.Cells[i, 3].Text = dt.Rows[i]["SeqNo"].ToString();
            //    ss1.ActiveSheet.Cells[i, 4].Text = VB.Left(dt.Rows[i]["SCODE"].ToString().Trim(), 1);

            //    if (dt.Rows[i]["StopFLAG"].ToString() == "Y")
            //    {
            //        ss1.ActiveSheet.Cells[i, 5].Text = "OK";
            //    }
            //    else
            //    {
            //        ss1.ActiveSheet.Cells[i, 5].Text = "";
            //    }

            //    ss1.ActiveSheet.Cells[i, 6].Text = dt.Rows[i]["WRTNO"].ToString();
            //    ss1.ActiveSheet.Cells[i, 7].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "MIR_청구상세진료과", dt.Rows[i]["DtNO"].ToString());

            //    ss1.ActiveSheet.Cells[i, 8].Text = dt.Rows[i]["DeptCode1"].ToString();
            //    ss1.ActiveSheet.Cells[i, 9].Text = dt.Rows[i]["JINDATE1"].ToString();
            //    ss1.ActiveSheet.Cells[i, 10].Text = " " + dt.Rows[i]["RateBon"].ToString() + " %";
            //    ss1.ActiveSheet.Cells[i, 11].Text = " " + dt.Rows[i]["Kiho"].ToString();
            //    ss1.ActiveSheet.Cells[i, 12].Text = " " + dt.Rows[i]["GKIho"].ToString();
            //    ss1.ActiveSheet.Cells[i, 14].Text = dt.Rows[i]["WRTNO"].ToString();
            //    ss1.ActiveSheet.Cells[i, 15].Text = dt.Rows[i]["TAMT"].ToString();
            //    ss1.ActiveSheet.Cells[i, 16].Text = dt.Rows[i]["BAMT"].ToString();


            //    //TODO: GnChoiceWRTNO처리 
            //    //if (GnChoiceWRTNO == dt.Rows[i]["WRTNO"].ToString())
            //    //{
            //    //    ss1.ActiveSheet.AddSelection(i, 0, i, 1);
            //    //}
            //}

            //dt.Dispose();
            //dt = null;
        }

        private void screen_display()
        {
            screen_clear();

            switch (FstrOpenJob)
            {
                case "1":
                case "5":
                    GetData(ss1, FstrPano);
                    break;
                case "7":
                    GetData_SAN(ss1, FstrPano);
                    break;
                case "8":
                    GetData_TA(ss1, FstrPano);
                    break;
            }
            ssList.Focus();
            ssList.ActiveSheet.ActiveRowIndex = nRow;
            ssList.ActiveSheet.ActiveColumnIndex = 0;
        }

        private void screen_clear()
        {
            ssList.ActiveSheet.RowCount = 0;
        }

        void GetData(FarPoint.Win.Spread.FpSpread Spd, string argPano)
        {

            DataTable dt = null;


            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            string strMonth = "";

            if (optGB0.Checked == true)
            {
                strMonth = "6개월전";
            }
            else if (optGB1.Checked == true)
            {
                strMonth = "12개월전";
            }

            dt = MirEntSQL.sel_Mir_Insid_Wrtno(clsDB.DbCon, argPano, FstrYYMM, FstrOpenIO, FstrOpenJob, "", strMonth, FstrWrtno);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["EdiMirno"].ToString().Trim() == "" || dt.Rows[i]["EdiMirno"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.EdiMirno].Text = "";
                    }
                    else if (dt.Rows[i]["EdiMirno"].ToString().Trim() != "0")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.EdiMirno].Text = "E";
                    }

                    if (dt.Rows[i]["Upcnt1"].ToString().Trim() == "9")
                    {
                        if (dt.Rows[i]["EdiMirno"].ToString().Trim() == "" || dt.Rows[i]["EdiMirno"].ToString().Trim() == "0")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = "보";
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = "반";
                        }
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = " ";
                    }

                    switch (dt.Rows[i]["BohoJong"].ToString().Trim())
                    {
                        case "1":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "재";
                            break;
                        case "2":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "추";
                            break;
                        default:
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "원";
                            break;
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Seqno].Text = dt.Rows[i]["SeqNo"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Scode].Text = VB.Left(dt.Rows[i]["SCODE"].ToString().Trim(), 1);

                    if (dt.Rows[i]["StopFLAG"].ToString().Trim() == "Y")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.StopFlag].Text = "OK";
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.StopFlag].Text = "";
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Wrtno].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    if (FstrWrtno == dt.Rows[i]["WRTNO"].ToString().Trim()) nRow = i;
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Dtno].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "MIR_청구상세진료과", dt.Rows[i]["DTno"].ToString().Trim());
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Deptcode1].Text = dt.Rows[i]["DEPTCODE1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.JinDate1].Text = dt.Rows[i]["JINDATE1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.RateBon].Text = " " + dt.Rows[i]["RateBon"].ToString().Trim() + "%";
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Kiho].Text = " " + dt.Rows[i]["Kiho"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.GKiho].Text = " " + dt.Rows[i]["Gkiho"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Wrtno2].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Tamt].Text = dt.Rows[i]["TAMT"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Jamt].Text = dt.Rows[i]["JAMT"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Bamt].Text = dt.Rows[i]["BAMT"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    //If GnChoiceWRTNO = AdoGetNumber(rs2, "WRTNO", I) Then
                    //    Call SS1.SetSelection(1, nRow, 1, nRow)
                    //   'SS1.Col = -1: SS1.BackColor = &HC0FFC0
                    //End If

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;


            Spd.ActiveSheet.ActiveRowIndex = nRow;

            #endregion

        }

        void GetData_SAN(FarPoint.Win.Spread.FpSpread Spd, string argPano)
        {

            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            dt = MirEntSQL.sel_Mir_Sanid_Wrtno(clsDB.DbCon, argPano, FstrYYMM, FstrOpenIO, FstrOpenJob);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["EdiMirno"].ToString().Trim() == "" || dt.Rows[i]["EdiMirno"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.EdiMirno].Text = "";
                    }
                    else// if (Convert.ToInt32(dt.Rows[i]["EdiMirno"].ToString().Trim()) > 0)
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.EdiMirno].Text = "E";
                    }

                    if (dt.Rows[i]["Upcnt1"].ToString().Trim() == "9")
                    {
                        if (dt.Rows[i]["EdiMirno"].ToString().Trim() == "" || dt.Rows[i]["EdiMirno"].ToString().Trim() == "0")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = "보";
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = "반";
                        }
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = " ";
                    }

                    switch (dt.Rows[i]["MIRGBN"].ToString().Trim())
                    {
                        case "1":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "퇴원";
                            break;
                        case "2":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "중간";
                            break;
                        case "3":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "재청";
                            break;
                        case "4":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "추가";
                            break;

                        default:
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "기타";
                            break;
                    }

                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Seqno].Text = dt.Rows[i]["SeqNo"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Scode].Text = VB.Left(dt.Rows[i]["SCODE"].ToString().Trim(), 1);

                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.StopFlag].Text = dt.Rows[i]["ZipCode1"].ToString().Trim();

                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Wrtno].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Dtno].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "MIR_청구상세진료과", dt.Rows[i]["DTno"].ToString().Trim());


                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Deptcode1].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.JinDate1].Text = dt.Rows[i]["FRDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.RateBon].Text = " " + dt.Rows[i]["RateGasan"].ToString().Trim() + "%";
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Kiho].Text = " " + dt.Rows[i]["GelCode"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.GKiho].Text = " " + dt.Rows[i]["CoprName"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Wrtno2].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Tamt].Text = dt.Rows[i]["TAMT"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Jamt].Text = dt.Rows[i]["JAMT"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Bamt].Text = dt.Rows[i]["BAMT"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    //If GnChoiceWRTNO = AdoGetNumber(rs2, "WRTNO", I) Then
                    //    Call SS1.SetSelection(1, nRow, 1, nRow)
                    //   'SS1.Col = -1: SS1.BackColor = &HC0FFC0
                    //End If

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion  

        }

        void GetData_TA(FarPoint.Win.Spread.FpSpread Spd, string argPano)
        {

            DataTable dt = null;

            Spd.ActiveSheet.RowCount = 0;
            Cursor.Current = Cursors.WaitCursor;

            dt = MirEntSQL.sel_Mir_Taid_Wrtno(clsDB.DbCon, argPano, FstrYYMM, FstrOpenIO, FstrOpenJob);

            #region //데이터셋 읽어 자료 표시
            if (dt == null) return;

            if (dt.Rows.Count > 0)
            {
                Spd.ActiveSheet.RowCount = dt.Rows.Count;
                Spd.ActiveSheet.SetRowHeight(-1, ComNum.SPDROWHT);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["EdiMirno"].ToString().Trim() == "" || dt.Rows[i]["EdiMirno"].ToString().Trim() == "0")
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.EdiMirno].Text = "";
                    }
                    else if (Convert.ToInt32(dt.Rows[i]["EdiMirno"].ToString().Trim()) > 0)
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.EdiMirno].Text = "E";
                    }

                    if (dt.Rows[i]["Upcnt1"].ToString().Trim() == "9")
                    {
                        if (dt.Rows[i]["EdiMirno"].ToString().Trim() == "" || dt.Rows[i]["EdiMirno"].ToString().Trim() == "0")
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = "보";
                        }
                        else
                        {
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = "반";
                        }
                    }
                    else
                    {
                        Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.UpCnt1].Text = " ";
                    }

                    switch (dt.Rows[i]["MIRGBN"].ToString().Trim())
                    {
                        case "1":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "퇴원";
                            break;
                        case "2":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "중간";
                            break;
                        case "3":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "재청";
                            break;
                        case "4":
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "추가";
                            break;

                        default:
                            Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.BohoJong].Text = "기타";
                            break;
                    }

                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Seqno].Text = dt.Rows[i]["SeqNo"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Scode].Text = VB.Left(dt.Rows[i]["SCODE"].ToString().Trim(), 1);

                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.StopFlag].Text = dt.Rows[i]["ZipCode1"].ToString().Trim();




                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Wrtno].Text = dt.Rows[i]["WRTNO"].ToString().Trim();

                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Dtno].Text = clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "MIR_청구상세진료과", dt.Rows[i]["DTno"].ToString().Trim());


                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Deptcode1].Text = dt.Rows[i]["DEPTCODE1"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.JinDate1].Text = dt.Rows[i]["FRDATE"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.RateBon].Text = " " + dt.Rows[i]["RateBon"].ToString().Trim() + "%";
                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Kiho].Text = " " + dt.Rows[i]["Kiho"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.GKiho].Text = " " + dt.Rows[i]["Gkiho"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Wrtno2].Text = dt.Rows[i]["WRTNO"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Tamt].Text = dt.Rows[i]["TAMT"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Jamt].Text = dt.Rows[i]["JAMT"].ToString().Trim();
                    Spd.ActiveSheet.Cells[i, (int)clsComMirEntSpd.enmMirEntPanoChoice.Bamt].Text = dt.Rows[i]["BAMT"].ToString().Trim();
                    //Spd.ActiveSheet.Cells[i, (int)clsComSupEndsSpd.enmComSupEndsCLE01A.ROWID].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    //If GnChoiceWRTNO = AdoGetNumber(rs2, "WRTNO", I) Then
                    //    Call SS1.SetSelection(1, nRow, 1, nRow)
                    //   'SS1.Col = -1: SS1.BackColor = &HC0FFC0
                    //End If

                }
            }

            dt.Dispose();
            dt = null;
            Cursor.Current = Cursors.Default;

            #endregion  

        }

    }
}