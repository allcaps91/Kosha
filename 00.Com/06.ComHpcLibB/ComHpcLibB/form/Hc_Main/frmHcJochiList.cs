using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcJochiList.cs
/// Description     : 조치대상자 명단조회
/// Author          : 김민철
/// Create Date     : 2020-06-03
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm조치대상자명단(Frm조치대상자명단.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcJochiList :Form
    {
        ComFunc CF = null;
        clsHaBase cHB = null;
        clsSpread cSpd =null;

        HIC_LTD LtdHelpItem = null;
        HIC_JEPSU_PATIENT JepsuPatItem = null;

        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultService hicResultService = null;
        HicSunapdtlService hicSunapdtlService = null;
        HicMemoService hicMemoService = null;
        HicPatientService hicPatientService = null;

        public delegate void SetJochiGstrValue(HIC_JEPSU_PATIENT GstrValue);
        public static event SetJochiGstrValue rSetJochiGstrValue;

        string[] lstExam = { "A124", "A125", "A126", "A121", "A122", "A142", "A123", "A241", "A242", "C404", "C405", "TX26", "E512"};

        long FnPano = 0;
        string fstrPtno = "";

        public frmHcJochiList()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtLtdName.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.ssETC.EditModeOff += new EventHandler(eSpdEditOff);
            this.ssETC.ButtonClicked += new EditorNotifyEventHandler(eSpdBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);
        }

        private void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            HIC_MEMO code = ssETC.GetRowData(e.Row) as HIC_MEMO;

            ssETC.DeleteRow(e.Row);
        }

        private void eSpdEditOff(object sender, EventArgs e)
        {
            int nRow = 0, nCol = 0;

            nRow = ssETC.ActiveSheet.ActiveRowIndex;
            nCol = ssETC.ActiveSheet.ActiveColumnIndex;

            if (sender == ssETC)
            {
                if (nCol == 2)
                {
                    Size size = ssETC.ActiveSheet.GetPreferredCellSize(nRow, nCol);
                    ssETC.ActiveSheet.Rows[nRow].Height = size.Height;
                }
            }
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            string strData = "";
            string strJumin = "";
            string fstrPtno = "";
            long nPano = 0;
            long nWrtNo = 0;
            

            if (e.RowHeader && e.ColumnHeader)
            {
                return;
            }

            if (e.Column == 13)
            {
                nWrtNo = long.Parse(SS1.ActiveSheet.Cells[e.Row, 4].Text);

                strData = "";
                HIC_JEPSU_PATIENT list = hicJepsuPatientService.GetItembyWrtNo(nWrtNo);

                if (!list.IsNullOrEmpty())
                {
                    strJumin = clsAES.DeAES(list.JUMIN2);
                    JepsuPatItem.GJYEAR = string.Format("{0:0000}", list.GJYEAR);           //건진년도
                    JepsuPatItem.GJBANGI = string.Format("{0:0}", list.GJBANGI);            //건진반기
                    JepsuPatItem.JEPDATE = list.JEPDATE;                                    //검진일자
                    JepsuPatItem.PANO = list.PANO;                                          //건진번호
                    JepsuPatItem.GJJONG = string.Format("{0:00}", list.GJJONG);             //건진종류
                    JepsuPatItem.JUMIN = VB.Left(strJumin, 6) + "-";                        //주민번호1
                    JepsuPatItem.JUMIN2 = VB.Mid(strJumin, 7, 7);                           //주민번호2
                    JepsuPatItem.SNAME = list.SNAME;                                        //성명
                    JepsuPatItem.SEX = list.SEX;                                            //성별
                    JepsuPatItem.AGE = list.AGE;                                            //나이
                    JepsuPatItem.WRTNO = nWrtNo;                                            //접수번호
                    JepsuPatItem.PTNO = list.PTNO;                                          //등록번호
                }
                rSetJochiGstrValue(JepsuPatItem);

                //this.Close();
                this.WindowState = FormWindowState.Minimized;
            }
            else
            {
                nPano = long.Parse(SS1.ActiveSheet.Cells[e.Row, 24].Text);
                //if (nPano != FnPano)
                //{
                    lblMemo.Text = "수검자 메모사항";
                    FnPano = nPano;
                    lblMemo.Text += " : " + SS1.ActiveSheet.Cells[e.Row, 1].Text + "(" + FnPano + ")";  //이름(건진번호)
                    Hic_Memo_Screen(FnPano);
                    btnSave.Enabled = true;
                //}
            }
        }

        private void Hic_Memo_Screen(long argPano)
        {
            ComFunc CF = null;

            CF = new ComFunc();

            List<HIC_MEMO> list = hicMemoService.GetItembyPaNo(argPano);

            ssETC.DataSource = list;
            fstrPtno = hicPatientService.GetPtnoByPano(argPano);

            if (!list.IsNullOrEmpty() && list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Size size = ssETC.ActiveSheet.GetPreferredCellSize(i, 2);
                    ssETC.ActiveSheet.Rows[i].Height = size.Height;
                    ssETC.ActiveSheet.Cells[i, 4].Text = CF.Read_SabunName(clsDB.DbCon, list[i].JOBSABUN.To<string>());
                    ssETC.ActiveSheet.Cells[i, 3].Text = clsType.User.IdNumber;
                    ssETC.ActiveSheet.Cells[i, 6].Text = FnPano.ToString();
                    ssETC.ActiveSheet.Cells[i, 7].Text = fstrPtno;
                }

                ssETC.AddRows(3);
                for (int i = 0; i < ssETC.ActiveSheet.RowCount; i++)
                {
                    ssETC.ActiveSheet.Cells[i, 3].Text = clsType.User.IdNumber;
                    ssETC.ActiveSheet.Cells[i, 6].Text = FnPano.ToString();
                    ssETC.ActiveSheet.Cells[i, 7].Text = fstrPtno;
                }
            }
            else
            {
                ssETC.AddRows(5);
                for (int i = 0; i < ssETC.ActiveSheet.RowCount; i++)
                {
                    ssETC.ActiveSheet.Cells[i, 3].Text = clsType.User.IdNumber;
                    ssETC.ActiveSheet.Cells[i, 6].Text = FnPano.ToString();
                    ssETC.ActiveSheet.Cells[i, 7].Text = fstrPtno;
                }
            }

            CF = null;
        }

        private void SetControl()
        {
            CF = new ComFunc();
            cHB = new clsHaBase();
            cSpd = new clsSpread();
            LtdHelpItem = new HIC_LTD();
            JepsuPatItem = new HIC_JEPSU_PATIENT();
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultService = new HicResultService();
            hicSunapdtlService = new HicSunapdtlService();
            hicMemoService = new HicMemoService();
            hicPatientService = new HicPatientService();

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            cHB.ComboJong_AddItem(cboJong, true);
            
            #region 수검자 메모 Spread
            ssETC.Initialize();
            ssETC.AddColumn("삭제",       nameof(HIC_MEMO.GBDEL),     34, FpSpreadCellType.CheckBoxCellType);
            ssETC.AddColumn("입력시각",   nameof(HIC_MEMO.ENTTIME),  160, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = false,   Aligen = CellHorizontalAlignment.Left, BackColor = Color.FromArgb(192, 255, 192) });
            ssETC.AddColumn("내용",       nameof(HIC_MEMO.MEMO),     440, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = true,  Aligen = CellHorizontalAlignment.Left, IsMulti = true });
            ssETC.AddColumn("작업자사번", nameof(HIC_MEMO.JOBSABUN),  48, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("작업자명",   nameof(HIC_MEMO.JOBNAME),   90, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsEditble = true });            
            ssETC.AddColumn("ROWID",      nameof(HIC_MEMO.RID),     30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PANO",       nameof(HIC_MEMO.PANO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            ssETC.AddColumn("PTNO",       nameof(HIC_MEMO.PTNO),      30, FpSpreadCellType.TextCellType,     new SpreadCellTypeOption { IsVisivle = false });
            #endregion

        }

        private void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdName && e.KeyChar == (char)13)
            {
                if (!txtLtdName.Text.Trim().IsNullOrEmpty()) { Ltd_Code_Help(); }
            }
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnLtdHelp)
            {
                Ltd_Code_Help();
            }
            else if (sender == btnSearch)
            {
                cSpd.Spread_Clear_Simple(SS1, 1);
                cSpd.setSpdCellColor(SS1, 0, 0, SS1.ActiveSheet.RowCount - 1, SS1.ActiveSheet.ColumnCount - 1, Color.White);
                Screen_Display();
            }
            else if (sender == btnPrint)
            {
                Spread_Print();
            }
            else if (sender == btnSave)
            {
                Hic_Memo_Save();
                Hic_Memo_Screen(FnPano);
            }
        }

        private void Hic_Memo_Save()
        {
            IList<HIC_MEMO> list = ssETC.GetEditbleData<HIC_MEMO>();
           
            if (list.Count > 0)
            {
                if (hicMemoService.Save(list, "일반",""))
                {
                    MessageBox.Show("저장하였습니다");
                    Screen_Display();
                }
                else
                {
                    MessageBox.Show("오류가 발생하였습니다. ");
                }
            }

        }

        private void Screen_Display()
        {
            string strOK = "";
            string strFDate = dtpFDate.Text;
            string strTDate = dtpTDate.Text;
            string strJong = VB.Left(cboJong.Text, 2);
            long nLtdCode = VB.Pstr(txtLtdName.Text, ".", 1).To<long>();
            long nWRTNO = 0;
            double  nRESULT = 0;
            string strResult = "";
            string strExCode = "";
            string strGroupCode = "";
            string strGbSelf = "";
            string strA124, strA125, strA126, strA121, strA122, strA142;
            string strA123, strA241, strA242, strC404, strC405, strTX26, strE512;
            int nRow = 0;

            List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetListByDate(strFDate, strTDate, strJong, nLtdCode);

            if (list.Count > 0)
            {

                prgBar1.Minimum = 0;
                prgBar1.Maximum = list.Count;

                for (int i = 0; i < list.Count; i++)
                {
                    strOK = "";
                    nWRTNO = list[i].WRTNO;

                    if (nWRTNO == 1149803)
                    {
                        nWRTNO = nWRTNO;
                    }


                    strA124 = ""; strA125 = ""; strA126 = ""; strA121 = ""; strA122 = ""; strA142 = "";
                    strA123 = ""; strA241 = ""; strA242 = ""; strC404 = ""; strC405 = ""; strTX26 = "";
                    strE512 = "";

                    List<HIC_RESULT> lst2 = hicResultService.GetJochiListByWrtnoCodeIN(nWRTNO, lstExam);

                    for (int j = 0; j < lst2.Count; j++)
                    {
                        nRESULT = lst2[j].RESULT.To<double>(); 
                        strResult = lst2[j].RESULT.To<string>("").Trim();
                        strExCode = lst2[j].EXCODE.Trim();
                        if (!lst2[j].GROUPCODE.IsNullOrEmpty())
                        {
                            strGroupCode = lst2[j].GROUPCODE.Trim();
                        }
                        

                        switch (strExCode)
                        {
                            case "A124": //GOT
                                if (nRESULT >= 200) { strOK = "OK"; strA124 = "※" + strResult; }
                                else { strA124 = strResult; }
                                break;
                            case "A125": //GPT
                                if (nRESULT >= 200) { strOK = "OK"; strA125 = "※" + strResult; }
                                else { strA125 = strResult; }
                                break;
                            case "A126": //감마-GPT
                                if (nRESULT >= 500) { strOK = "OK"; strA126 = "※" + strResult; }
                                else { strA126 = strResult; }
                                break;
                            case "A121": //HB
                                if (nRESULT <= 8) { strOK = "OK"; strA121 = "※" + strResult; }
                                else { strA121 = strResult; }
                                break;
                            case "A122": //혈당
                                if (nRESULT >= 200 || nRESULT < 60) { strOK = "OK"; strA122 = "※" + strResult; }
                                else { strA122 = strResult; }
                                break;
                            case "A142":
                                if (nRESULT >= 7) { strOK = "OK"; strA142 = "※" + strResult; }
                                else { strA142 = strResult; }
                                break;
                            case "A241": //중성지방
                                if (nRESULT >= 400 || VB.Left(strResult, 1) == ">") { strOK = "OK"; strA241 = "※" + strResult; }
                                else { strA241 = strResult; }
                                break;
                            case "A123": //총콜레스테롤
                                if (nRESULT >= 300) { strOK = "OK"; strA123 = "※" + strResult; }
                                else { strA123 = strResult; }
                                break;
                            case "A242": //HDL
                                strA242 = strResult; break;
                            case "C404": //LDL
                                strC404 = strResult; break;
                            case "C405": //LDL 2차
                                strC405 = strResult; break;
                            case "TX26": //분변검사
                                if (VB.L(strResult, "Pos") > 1) { strOK = "OK"; strTX26 = "※" + strResult; }
                                else if (strResult.Replace(">", "").To<long>() > 100) { strOK = "OK"; strTX26 = "※" + strResult; }
                                break;
                            case "E512": //C형간염
                                strE512 = strResult; break;
                        }
                    }

                    

                    if (strOK == "OK")
                    {
                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow) { SS1.ActiveSheet.RowCount = nRow; }

                        SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].GJJONG.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].SNAME.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = VB.Left(list[i].JUMIN, 6) + "-" + VB.Right(list[i].JUMIN, 7);
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].AGE.ToString();
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].WRTNO.To<string>();
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].TEL + " " + list[i].HPHONE;
                        SS1.ActiveSheet.Cells[nRow - 1, 19].Text = list[i].JEPDATE.Trim();
                        SS1.ActiveSheet.Cells[nRow - 1, 20].Text = list[i].JONGGUMYN.Trim() == "1" ? "◎" : "";
                        SS1.ActiveSheet.Cells[nRow - 1, 22].Text = strGroupCode;
                        SS1.ActiveSheet.Cells[nRow - 1, 24].Text = list[i].PANO.ToString();

                        strGbSelf = hicSunapdtlService.GetGbSelfByWrtno(nWRTNO);
                        if (strGbSelf == "01" || strGbSelf == "1")
                        {
                            strGbSelf = "OK";
                        }

                        //발송일 읽기
                        SS1.ActiveSheet.Cells[nRow - 1, 21].Text = list[i].TONGBODATE;
                        if (CF.DATE_ILSU(clsDB.DbCon, DateTime.Now.ToShortDateString(), list[i].TONGBODATE.To<string>("")) >= 15)
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 21].BackColor = Color.DarkRed;
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = strA124;
                        if (VB.L(strA124, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 6].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strA125;
                        if (VB.L(strA125, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 7].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = strA126;
                        if (VB.L(strA126, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 8].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = strA121;
                        if (VB.L(strA121, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 9].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = strA122;
                        if (VB.L(strA122, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 10].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = strA142;
                        if (VB.L(strA142, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 11].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 12].Text = strA123;
                        if (VB.L(strA123, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 12].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 13].Text = strA241;
                        if (VB.L(strA241, "※") > 1 && strGroupCode == "1160" && strGbSelf == "OK") { SS1.ActiveSheet.Cells[nRow - 1, 13].BackColor = Color.Wheat; }

                        //C405 접수여부 확인
                        if (!hicResultService.GetRowidByOneExcodeWrtno("C405", nWRTNO).IsNullOrEmpty())
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 13].BackColor = Color.LightCoral;
                        }

                        SS1.ActiveSheet.Cells[nRow - 1, 14].Text = strA242;
                        SS1.ActiveSheet.Cells[nRow - 1, 15].Text = strC404;
                        SS1.ActiveSheet.Cells[nRow - 1, 16].Text = strC405;
                        SS1.ActiveSheet.Cells[nRow - 1, 17].Text = strTX26;
                        if (VB.L(strTX26, "※") > 1) { SS1.ActiveSheet.Cells[nRow - 1, 17].BackColor = Color.Wheat; }
                        SS1.ActiveSheet.Cells[nRow - 1, 18].Text = strE512;
                        if (strE512.To<double>() > 1) { SS1.ActiveSheet.Cells[nRow - 1, 18].BackColor = Color.Wheat; }
                    }
                    

                    prgBar1.Value = i + 1;
                }
            }
        }

        /// <summary>
        /// 사업장 코드 검색창 연동
        /// </summary>
        private void Ltd_Code_Help()
        {
            string strFind = "";

            if (txtLtdName.Text.Contains("."))
            {
                strFind = VB.Pstr(txtLtdName.Text, ".", 2).Trim();
            }
            else
            {
                strFind = txtLtdName.Text.Trim();
            }

            frmHcLtdHelp frm = new frmHcLtdHelp(strFind);
            frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(ePost_value_LTD);
            frm.ShowDialog();

            if (!LtdHelpItem.IsNullOrEmpty() && LtdHelpItem.CODE > 0)
            {
                txtLtdName.Text = LtdHelpItem.CODE.To<string>();
                txtLtdName.Text = txtLtdName.Text + "." + LtdHelpItem.SANGHO;
            }
            else
            {
                txtLtdName.Text = "";
            }
        }

        private void ePost_value_LTD(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eFormLoad(object sender, EventArgs e)
        {
            cboJong.SelectedIndex = 0;
            cSpd.Spread_Clear_Simple(SS1);

            dtpFDate.Text = CF.DATE_ADD(clsDB.DbCon, DateTime.Now.ToShortDateString(), -1);
            dtpTDate.Text = DateTime.Now.ToShortDateString();
            prgBar1.Minimum = 0;
            prgBar1.Maximum = 0;
            prgBar1.Value = 0;
        }

        private void Spread_Print()
        {
            string  strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "검사결과값 이상자 명단";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += cSpd.setSpdPrint_String("조회기간: " + dtpFDate.Text + "~" + dtpTDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("검진종류: " + cboJong.Text.Trim(), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("사업장명: " + VB.Pstr(txtLtdName.Text, ".", 2), new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strHeader += cSpd.setSpdPrint_String("인쇄시각: " + DateTime.Now.ToString() + " " + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
            strFooter = cSpd.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

            cSpd.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}
