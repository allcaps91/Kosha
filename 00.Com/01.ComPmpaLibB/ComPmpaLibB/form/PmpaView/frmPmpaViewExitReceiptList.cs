using System;
using System.Data;
using System.Windows.Forms;
using ComBase; //기본 클래스
using FarPoint.Win.Spread;
using ComDbB;

namespace ComPmpaLibB
{
    public partial class frmPmpaViewExitReceiptList : Form
    {

        /// <summary>
        /// Class Name      : ComPmpaLibB
        /// File Name       : 
        /// Description     : 
        /// Author          : 김효성
        /// Create Date     : 2017-10-19
        /// Update History  : 
        /// </summary>
        /// <history>  
        /// 
        /// </history>
        /// <seealso cref= D:\psmh\IPD\iument\iument.vbp\Frm퇴원진료비영수증_NEW.frm" >> frmSupLbExSTS15.cs 폼이름 재정의" />

        string FstrPano = "";
        string FstrJob = "";

        long FnIPDNO = 0;
        long FnTRSNO = 0;
        long FnBoho_IPDNO = 0;
        
        string strDTP = ComFunc.FormatStrToDate(ComQuery.CurrentDateTime(clsDB.DbCon, "D"), "D");

        public frmPmpaViewExitReceiptList()
        {
            InitializeComponent();
            SetEvent();
            SetCombo();
        }

        public frmPmpaViewExitReceiptList(string ArgPano, string ArgJob)
        {
            InitializeComponent();
            SetEvent();
            SetCombo();

            FstrPano = ArgPano;
            FstrJob = ArgJob;
        }

        void SetEvent()
        {
            this.Load                       += new EventHandler(eFormLoad);
            this.txtPano.KeyPress           += new KeyPressEventHandler(eKeyPress);
            this.dtpFdate.KeyPress          += new KeyPressEventHandler(eKeyPress);
            this.btnExit.Click              += new EventHandler(eBtnClick);
            this.btnMinwon.Click += new EventHandler(eBtnClick);
            this.btnExit2.Click             += new EventHandler(eBtnClick);
            this.btnSearch.Click            += new EventHandler(eBtnClick);
            this.btnSave.Click              += new EventHandler(eBtnClick);
            this.btnSave_Temp.Click         += new EventHandler(eBtnClick);
            this.btnPrint.Click             += new EventHandler(eBtnClick);
            this.SSTrans.CellDoubleClick    += new CellClickEventHandler(eSpdDblClick);
            this.SSAmt.CellDoubleClick      += new CellClickEventHandler(eSpdDblClick);
            this.SSAmtNew.CellDoubleClick   += new CellClickEventHandler(eSpdDblClick);
            this.ssDrg.ButtonClicked        += new EditorNotifyEventHandler(eSpdBtnClick);

        }

        void SetCombo()
        {
            DataTable Dt = null;
            int i = 0;
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsPmpaPb cPB = new clsPmpaPb();

            //BAS_소아면제
            Dt = cPF.sel_Bas_OgPdBun(clsDB.DbCon);
            if (Dt != null)
            {
                cPB.ArgV = new string[Dt.Rows.Count];

                for (i = 0; i < Dt.Rows.Count; i++)
                {
                    cPB.ArgV[i] = Dt.Rows[i]["CODE"].ToString().Trim() + "." + Dt.Rows[i]["NAME"].ToString().Trim();
                }

                Dt.Dispose();
                Dt = null;
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int i = 0;
            clsComPmpaSpd cPS = new clsComPmpaSpd();
            clsPmpaPb cPB = new clsPmpaPb();
            Card CARD = new Card();

            cPS.sSpd_enmRcptTrsList(SSTrans, cPB.sSpdRcptTrsList, cPB.nSpdRcptTrsList, 10, 0, cPB.ArgV);
            CARD.CardVariable_Clear(ref clsPmpaType.RSD, ref clsPmpaType.RD);
            txtPano.Text = "";

            SCREEN_CLEAR2();
            
            for (i = 0; i <= SSAmt_Sheet1.RowCount - 1; i++)
            {
                SSAmt_Sheet1.Cells[i, 2].Text = "";
                SSAmt_Sheet1.Cells[i, 3].Text = "";
                SSAmt_Sheet1.Cells[i, 4].Text = "";
                SSAmt_Sheet1.Cells[i, 5].Text = "";
                SSAmt_Sheet1.Cells[i, 6].Text = "";

                SSAmt_Sheet1.Cells[i, 8].Text = "";
                SSAmt_Sheet1.Cells[i, 9].Text = "";
                SSAmt_Sheet1.Cells[i, 10].Text = "";
                SSAmt_Sheet1.Cells[i, 11].Text = "";
                SSAmt_Sheet1.Cells[i, 12].Text = "";

                SSAmt_Sheet1.Cells[i, 14].Text = "";
            }

            for (i = 0; i <= SSAmtNew_Sheet1.RowCount - 1; i++)
            {
                SSAmtNew_Sheet1.Cells[i, 2].Text = "";
                SSAmtNew_Sheet1.Cells[i, 3].Text = "";
                SSAmtNew_Sheet1.Cells[i, 4].Text = "";
                SSAmtNew_Sheet1.Cells[i, 5].Text = "";
                SSAmtNew_Sheet1.Cells[i, 6].Text = "";

                SSAmtNew_Sheet1.Cells[i, 8].Text = "";
                SSAmtNew_Sheet1.Cells[i, 9].Text = "";
                SSAmtNew_Sheet1.Cells[i, 10].Text = "";
                SSAmtNew_Sheet1.Cells[i, 11].Text = "";
                SSAmtNew_Sheet1.Cells[i, 12].Text = "";

                SSAmtNew_Sheet1.Cells[i, 14].Text = "";
            }

            //panDrg.Visible = false;
            chkSaBon.Visible = true;
            dtpFdate.Visible = false;

            if (FstrJob == "퇴원")
            {
                rdoGbn3.Checked = true;
            }
            else if (FstrJob == "재원")
            {
                rdoGbn2.Checked = true;
            }
            else
            {
                rdoGbn2.Checked = true;
            }

            if (FstrPano != "")
            {
                txtPano.Text = FstrPano;
                Display_Ipd_Trans("");
            }

            this.rdoJob0.Checked = true;
            this.Activate();
            this.txtPano.Select();
            this.txtPano.Focus();
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                if (rdoJob0.Checked)
                {
                    txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");
                }
                
                btnSearch.Focus();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == this.btnExit)             //닫기
            {
                this.Close();
                return;
            }
            else if (sender == this.btnExit2)
            {
                panTemp.Visible = false;
            }
            else if (sender == this.btnMinwon)
            {
                //PSMHIUMENTOLD.clsIument_Old psmhIumOld = null;
                //psmhIumOld = new PSMHIUMENTOLD.clsIument_Old();

                //psmhIumOld.DbCon();
                //psmhIumOld.ShowForm_FrmOpdInOut(clsType.User.IdNumber);
                //psmhIumOld.DbDisCon();
                //psmhIumOld = null;
            }
            else if (sender == this.btnSearch)
            {
                Display_Ipd_Trans("");
            }
            else if (sender == this.btnPrint)
            {
                Print_Bill(clsDB.DbCon);
            }
            else if (sender == this.btnSave_Temp)
            {
                panTemp.Visible = true;
            }
            else if (sender == this.btnSave)
            {
                Make_Temp_Trans(clsDB.DbCon);
                Display_Ipd_Trans("임시자격");
            }
        }
        
        void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == this.SSTrans)
            {
                DisPlay_Trans_Amt(e);
            }
            else if (sender == this.SSAmt || sender == this.SSAmtNew)
            {
                DisPlay_JinDtl(e);
            } 
        }

        void eSpdBtnClick(object sender, EditorNotifyEventArgs e)
        {
            if (sender == ssDrg)
            {
                DisPlay_DrgDtl(e);
            }
        }

        void rdoJob_CheckedChanged(object sender, EventArgs e)
        {
            int i = 0;

            txtPano.Text = "";

            if (rdoJob0.Checked == true)
            {
                lblPano.Text = "등록번호";
                txtPano.Text = "";
                dtpFdate.Visible = false;
                txtPano.Visible = true;
                txtPano.Select();
            }
            else if (rdoJob1.Checked == true)
            {
                lblPano.Text = "환자성명";
                txtPano.Text = "";
                dtpFdate.Visible = false;
                txtPano.Visible = true;
                txtPano.Select();
            }
            else
            {
                lblPano.Text = "퇴원일자";
                dtpFdate.Visible = true;
                txtPano.Visible = false;
                dtpFdate.Select();
            }
        }
        
        void Display_Ipd_Trans(string strTemp)
        {
            DataTable dt = null;
            clsSpread CS = new clsSpread();
            clsPmpaFunc cPF = new clsPmpaFunc();
            clsIument cIMT = new clsIument();

            string strKey = string.Empty;
            string strView = string.Empty;
            string strSTS = string.Empty;
            string strPano = string.Empty;
            string strDrg = string.Empty;

            int i = 0;
            int nRead = 0;
            long nIPDNO = 0;
            long nTRSNo = 0;

            if (strTemp == "임시자격")
            {
                strView = "1";  //등록번호
                strKey = txtPano.Text.Trim();
                strSTS = "J";
            }
            else
            {
                txtPano.Text = txtPano.Text.Trim();

                if (rdoJob0.Checked == true)
                {
                    if (txtPano.Text != "")
                    {
                        txtPano.Text = VB.Format(VB.Val(txtPano.Text), "00000000");
                    }
                    strView = "1";  //등록번호
                    strKey = txtPano.Text.Trim();
                }
                else if (rdoJob1.Checked == true)
                {
                    strView = "2";  //환자성명
                    strKey = txtPano.Text.Trim();

                }
                else if (rdoJob2.Checked == true)
                {
                    strView = "3";  //퇴원일자
                    strKey = dtpFdate.Text;
                }

                strSTS = "";
                if (rdoGbn2.Checked)
                {
                    strSTS = "J";
                }
                else if (rdoGbn3.Checked)
                {
                    strSTS = "T";
                }
            }
            
            Cursor.Current = Cursors.WaitCursor;
            btnSearch.Enabled = false;

            try
            {
                dt = cPF.sel_IpdTrs(clsDB.DbCon, strKey, strView, strSTS, strTemp);
                
                if (dt != null)
                {

                    nRead = dt.Rows.Count;

                    if (dt.Rows.Count == 0)
                    {

                        dt.Dispose();
                        dt = null;
                        btnSearch.Enabled = true;
                        ComFunc.MsgBox("해당 DATA가 없습니다.");
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    //스프레드 출력문
                    SSTrans_Sheet1.Rows.Count = 0;
                    SSTrans_Sheet1.Rows.Count = nRead;
                    SSTrans_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                    for (i = 0; i < nRead; i++)
                    {
                        nIPDNO = Convert.ToInt64(dt.Rows[i]["IPDNO"].ToString().Trim());
                        nTRSNo = Convert.ToInt64(dt.Rows[i]["TRSNO"].ToString().Trim());
                        strPano = dt.Rows[i]["PANO"].ToString().Trim();
                        strDrg = dt.Rows[i]["GBDrg"].ToString().Trim();

                        //장애구분 표시
                        //if (dt.Rows[i]["Bi"].ToString().Trim() == "22")
                        //{
                        //    SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.BOHUN].Text = dt.Rows[i]["Bohun"].ToString().Trim() == "3" ? "장애" : "";
                        //}
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.BOHUN].Text = dt.Rows[i]["Bohun"].ToString().Trim() == "3" ? "장애" : "";

                        if (dt.Rows[i]["Secret"].ToString().Trim() != "" && dt.Rows[i]["GBSTS"].ToString().Trim() != "퇴원수납완료")
                        {
                            ComFunc.MsgBox("사생활보호 대상요청자 입니다. 안내시 주의하십시오.", "안내주의");
                        }
                        

                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.GBSTS].Text      = dt.Rows[i]["GBSTS"].ToString().Trim(); 
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.PANO].Text       = dt.Rows[i]["PANO"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.SNAME].Text      = dt.Rows[i]["SNAME"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.INDATE].Text     = dt.Rows[i]["InDate"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.OUTDATE].Text    = dt.Rows[i]["OutDate"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.ILSU].Text       = dt.Rows[i]["Ilsu"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.WARD].Text       = dt.Rows[i]["WARDCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.ROOM].Text       = dt.Rows[i]["ROOMCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.BI].Text         = dt.Rows[i]["Bi"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.DEPT].Text       = dt.Rows[i]["DeptCode"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.DRCODE].Text     = dt.Rows[i]["DRCODE"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.GBIPD].Text      = dt.Rows[i]["GbIPD"].ToString().Trim() == "9" ? "지병" : "";
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.SANG].Text       = VB.Val(dt.Rows[i]["SangAmt"].ToString().Trim()) > 0 ? "상한" : "";
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.OGPDBUN].Text    = dt.Rows[i]["OgPdBun"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.VCODE].Text      = Rtn_SSTrans_VCode(dt.Rows[i]["VCODE"].ToString().Trim());
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.JSIM].Text       = dt.Rows[i]["AmSet3"].ToString().Trim() == "9" ? "완료" : "";
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.ACTDATE].Text    = dt.Rows[i]["ActDate"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.IPDNO].Text      = nIPDNO.ToString();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.TRSNO].Text      = nTRSNo.ToString();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.GBSPC].Text      = dt.Rows[i]["GbSPC"].ToString().Trim() == "1" ? "Y" : "";
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.OGPDBUNDTL].Text = dt.Rows[i]["OgPdBundtl"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.SECRET].Text     = dt.Rows[i]["Secret"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.DRGCODE].Text    = dt.Rows[i]["DrgCode"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.DRG].Text        = dt.Rows[i]["GBDrg"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.ROWID].Text      = dt.Rows[i]["ROWID"].ToString().Trim();
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.TEMP].Text       = strTemp;
                        SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text      = "";

                        if (dt.Rows[i]["GBSTS"].ToString().Trim() == "재원")
                        {
                            CS.setSpdCellColor(SSTrans, i, 0, i, SSTrans.ActiveSheet.ColumnCount - 1, System.Drawing.Color.FromArgb(192, 255, 192));
                        }
                        
                        #region FCODE 표기
                        if (dt.Rows[i]["GbIPD"].ToString().Trim() == "9")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "지병" + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                            if (dt.Rows[i]["JINDTL"].ToString().Trim() == "01") { SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text="지병+틀니"; }
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "P" || dt.Rows[i]["OGPDBUN"].ToString().Trim() == "O")
                        {
                            // '2015-05-18 입원명령결핵 추가
                            if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                            }
                            else if (dt.Rows[i]["VCODE"].ToString().Trim() == "F010")
                            {
                                //'2015-06-30
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F010";
                            }
                            else
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "면제";

                                if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                                {
                                    SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = " ★결핵★";
                                }
                            }
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                        {
                            //'2013-02-15
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "E+V";
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                        {
                            //'2013-02-15
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "F+V";
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "1")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증E+V+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V193" && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "2")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증F+V+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        //'V268 뇌출혈추가
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V191" || dt.Rows[i]["VCODE"].ToString().Trim() == "V192" || dt.Rows[i]["VCODE"].ToString().Trim() == "V193" || dt.Rows[i]["VCODE"].ToString().Trim() == "V194" || dt.Rows[i]["VCODE"].ToString().Trim() == "V268" || dt.Rows[i]["VCODE"].ToString().Trim() == "V275")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                        {
                            //'2015-05-18 입원명령결핵 추가
                            if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상위E(" + dt.Rows[i]["VCODE"].ToString().Trim() + "+F008)";
                            }
                            else
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상위E+★결핵★";
                            }
                        }

                        else if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                        {
                            //'2015-05-18 입원명령결핵 추가
                            if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상위F(" + dt.Rows[i]["VCODE"].ToString().Trim() + "+F008)";
                            }
                            else
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상위F+★결핵★";
                            }

                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E" && dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증화상E+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F" && dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증화상F+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["VCODE"].ToString().Trim() == "V247" || dt.Rows[i]["VCODE"].ToString().Trim() == "V248" || dt.Rows[i]["VCODE"].ToString().Trim() == "V249" || dt.Rows[i]["VCODE"].ToString().Trim() == "V250")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "중증화상+" + dt.Rows[i]["VCODE"].ToString().Trim();
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "H")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "희귀H";
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "V")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "희귀H";

                            if (dt.Rows[i]["FCODE"].ToString().Trim() == "F008")
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = dt.Rows[i]["VCODE"].ToString().Trim() + "+F008";
                            }
                            else
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "희귀V";

                                if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "11/04/01") >= 0 && dt.Rows[i]["VCODE"].ToString().Trim() == "V206" || dt.Rows[i]["VCODE"].ToString().Trim() == "V231")
                                {
                                    SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = " ★결핵★";
                                }
                            }

                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "C")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상";
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "E")
                        {
                            if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상E+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                            else
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상E" + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                        }
                        else if (dt.Rows[i]["OGPDBUN"].ToString().Trim() == "F")
                        {
                            if (dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() != "")
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상F+" + dt.Rows[i]["OGPDBUNdtl"].ToString().Trim() + " " + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                            else
                            {
                                SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "차상F" + dt.Rows[i]["VCODE"].ToString().Trim();
                            }
                        }
                        else if (string.Compare(dt.Rows[i]["InDate"].ToString().Trim(), "16/07/01") >= 0 && dt.Rows[i]["FCODE"].ToString().Trim() == "F014")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "F014";
                        }
                        else if ( dt.Rows[i]["FCODE"].ToString().Trim() == "MT04")
                        {
                            SSTrans_Sheet1.Cells[i, (int)clsPmpaPb.enmRcptTrsList.FCODE].Text = "신종코로나";
                        }
                        #endregion

                    }
                    dt.Dispose();
                    dt = null;

                    if (nRead == 1)
                    {
                        FnBoho_IPDNO = nIPDNO;
                        FstrPano = strPano;
                        FnIPDNO = nIPDNO;
                        FnTRSNO = nTRSNo;

                        cIMT.DISPLAY_IPD_TRANS_AMT_NEW(clsDB.DbCon, SSAmt, SSAmtNew, ssDrg, ssDrgAmt, Convert.ToInt64(nIPDNO), Convert.ToInt64(nTRSNo), "", strPano, strDrg);
                    }
                }

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;

            }
            catch (Exception ex)
            {
                btnSearch.Enabled = true;
                ComFunc.MsgBox(ex.Message);
                Cursor.Current = Cursors.Default;
            }
        }
        
        void SCREEN_CLEAR2()
        {
            int i = 0;
            
            for (i = 0; i < ssDrg_Sheet1.RowCount; i++)
            {
                ssDrg_Sheet1.Cells[i, 1].Text = "";
                ssDrg_Sheet1.Cells[i, 2].Text = "";
                ssDrg_Sheet1.Cells[i, 3].Text = "";
                ssDrg_Sheet1.Cells[i, 4].Text = "";
                ssDrg_Sheet1.Cells[i, 5].Text = "";
                ssDrg_Sheet1.Cells[i, 6].Text = "";
            }

            for (i = 0; i < ssDrgAmt_Sheet1.RowCount; i++)
            {
                ssDrgAmt_Sheet1.Cells[i, 1].Text = "";
            }

            lblJobProg.Visible = false;
            lblJobProg2.Visible = false;
        }
        
        void DisPlay_Trans_Amt(CellClickEventArgs e)
        {
            long nIPDNO = 0;
            long nTRSNo = 0;
            string strPano = "";
            string strInDate = "";
            string strOutDate = "";
            string strOutDate_Drg = "";
            string strDrgCode = "";
            string strTemp = string.Empty;
            string strGbDrg = "";
            int i = 0;

            clsIument cIMT = new clsIument();
            ComFunc CF = new ComFunc();

            if (e.ColumnHeader == true)
            {
                return;
            }
            
            strPano = SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.PANO].Text;
            if (strPano == "")
            {
                return;
            }
            strInDate       = SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.INDATE].Text;
            strOutDate      = SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.OUTDATE].Text;
            strOutDate_Drg  = SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.OUTDATE].Text;
            nIPDNO          = Convert.ToInt64(SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.IPDNO].Text);
            nTRSNo          = Convert.ToInt64(SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.TRSNO].Text);
            strDrgCode      = SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.DRGCODE].Text;
            strGbDrg        = SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.DRG].Text;
            strTemp         = SSTrans_Sheet1.Cells[e.Row, (int)clsPmpaPb.enmRcptTrsList.TEMP].Text;

            if (strOutDate == "")
            {
                strOutDate = strDTP;
            }

            FstrPano     = strPano;
            FnIPDNO      = nIPDNO;
            FnTRSNO      = nTRSNo;
            FnBoho_IPDNO = nIPDNO;

            #region 영수증 Spread Clear
            //금액을 표시할 Sheet를 Clear
            //구버전 영수증
            for (i = 0; i < SSAmt_Sheet1.RowCount; i++)
            {
                SSAmt_Sheet1.Cells[i, 2].Text = "";
                SSAmt_Sheet1.Cells[i, 3].Text = "";
                SSAmt_Sheet1.Cells[i, 4].Text = "";
                SSAmt_Sheet1.Cells[i, 5].Text = "";
                SSAmt_Sheet1.Cells[i, 6].Text = "";
                SSAmt_Sheet1.Cells[i, 8].Text = "";
                SSAmt_Sheet1.Cells[i, 9].Text = "";
                SSAmt_Sheet1.Cells[i, 10].Text = "";
                SSAmt_Sheet1.Cells[i, 11].Text = "";
                SSAmt_Sheet1.Cells[i, 12].Text = "";
                SSAmt_Sheet1.Cells[i, 14].Text = "";
            }

            //신버전 영수증
            for (i = 0; i < SSAmtNew_Sheet1.RowCount; i++)
            {
                SSAmtNew_Sheet1.Cells[i, 2].Text = "";
                SSAmtNew_Sheet1.Cells[i, 3].Text = "";
                SSAmtNew_Sheet1.Cells[i, 4].Text = "";
                SSAmtNew_Sheet1.Cells[i, 5].Text = "";
                SSAmtNew_Sheet1.Cells[i, 6].Text = "";
                SSAmtNew_Sheet1.Cells[i, 8].Text = "";
                SSAmtNew_Sheet1.Cells[i, 9].Text = "";
                SSAmtNew_Sheet1.Cells[i, 10].Text = "";
                SSAmtNew_Sheet1.Cells[i, 11].Text = "";
                SSAmtNew_Sheet1.Cells[i, 12].Text = "";
                SSAmtNew_Sheet1.Cells[i, 14].Text = "";
            } 
            #endregion

            Cursor.Current = Cursors.WaitCursor;

            lblJobProg.Visible = true;
            lblJobProg2.Visible = true;

            SCREEN_CLEAR2();

            //Main 계산 루틴 
            if (chkHU.Checked == true)
            {
                cIMT.DISPLAY_IPD_TRANS_AMT_NEW(clsDB.DbCon, SSAmt, SSAmtNew, ssDrg, ssDrgAmt, nIPDNO, nTRSNo, strTemp, strPano, strGbDrg, "1");
            }
            else
            {
                cIMT.DISPLAY_IPD_TRANS_AMT_NEW(clsDB.DbCon, SSAmt, SSAmtNew, ssDrg, ssDrgAmt, nIPDNO, nTRSNo, strTemp, strPano, strGbDrg);
            }

            if (chkHU.Checked == true)
            {
                btnPrint.Enabled = false;
            }
            else
            {
                // '2016-03-31 퇴원일기준 계산서인쇄 5년까지만 가능
                if (Convert.ToDateTime(strOutDate) <= Convert.ToDateTime(strDTP).AddDays(-1830))
                {
                    ComFunc.MsgBox("영수증 재발행은 5년까지만 가능합니다", "확인");
                    btnPrint.Enabled = false;

                    if (CF.JIN_AMT_PRINT_CHK(clsDB.DbCon, clsType.User.Sabun) == "OK")
                    {
                        btnPrint.Enabled = true;
                    }
                    else
                    {
                        btnPrint.Enabled = true;
                    }
                }
            }

            lblJobProg.Visible = false;
            lblJobProg2.Visible = false;

            Cursor.Current = Cursors.Default;
        }

        void DisPlay_JinDtl(CellClickEventArgs e)
        {
            string strHang = "";
            string strJOB = "";
            string strHangDtl = "";

            if (e.Row < 0) { return; }

            strHang = "00";
            strHangDtl = "00";
            strJOB = "";

            if ((e.Column > 1 && e.Column < 5) || (e.Column > 7 && e.Column < 11))
            {
                strJOB = "내역";
            }
            else if (e.Column == 5 || e.Column == 6 || e.Column == 11 || e.Column == 12)
            {
                strJOB = "상세내역";
            }
            else
            {
                return;
            }

            if (e.Column > 1 && e.Column < 7)
            {
                switch (e.Row)
                {
                    case 0:
                        strHang = 1.ToString("00");
                        break;
                    case 1:
                        strHang = 2.ToString("00");
                        break;  //'입원
                    case 2:
                        strHang = 3.ToString("00");
                        break;  //'식대
                    case 3:
                        strHang = 4.ToString("00");   //'투약
                        strHangDtl = "투약행위";
                        break;
                    case 4:
                        strHang = 4.ToString("00");
                        strHangDtl = "투약약품";
                        break;
                    case 5:
                        strHang = 5.ToString("00");   //'주사
                        strHangDtl = "주사행위";
                        break;
                    case 6:
                        strHang = 5.ToString("00");
                        strHangDtl = "주사약품";
                        break;
                    case 7:
                        strHang = 6.ToString("00");
                        break;   //'마취
                    case 8:
                        strHang = 7.ToString("00");
                        break;   //'처치
                    case 9:
                        strHang = 8.ToString("00");
                        break; //'검사
                    case 10:
                        strHang = 9.ToString("00");
                        break; //'영상
                    case 11:
                        strHang = 9.ToString("00");
                        break; //'방사선
                    case 12:
                        strHang = 10.ToString("00");
                        break; //'치료재
                    case 13:
                        strHang = 12.ToString("00");
                        break; //'물리치료
                    case 14:
                        strHang = 13.ToString("00");
                        break; //'정신
                    case 15:
                        strHang = 18.ToString("00");
                        break; //'전혈
                }
            }
            else if (e.Column > 7 && e.Column < 13)
            {
                switch (e.Row)
                {
                    case 0:
                        strHang = 14.ToString("00");
                        break;//'CT
                    case 1:
                        strHang = 15.ToString("00");
                        break;//'MRI
                    case 2:
                        strHang = 16.ToString("00");
                        break;//'초음파
                    case 3:
                        strHang = 17.ToString("00");
                        break;//'보철
                    case 4:
                        strHang = 19.ToString("00");
                        break;//'예약진찰
                    case 5:
                        strHang = 20.ToString("00");
                        break;//'증명
                    case 6:
                        strHang = 21.ToString("00");
                        break;//'병실차
                    case 7:
                        strHang = 22.ToString("00");
                        break;//'선별급여 2016-08-31
                }
            }

            if (strHang == "")
            {
                return;
            }
            if (chkHU.Checked == true)
            {
                frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList(strJOB, strHang, FnIPDNO, FnTRSNO, "", "", strHangDtl, "", "", "1");
                frm.ShowDialog();
            }
            else
            {
                frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList(strJOB, strHang, FnIPDNO, FnTRSNO, "", "", strHangDtl, "", "");
                frm.ShowDialog();
            }
        }

        void DisPlay_DrgDtl(EditorNotifyEventArgs e)
        {
            if (e.Column < 0 || e.Row < 0)
            {
                return;
            }
            
            if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "부수술비용")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2015-01-30") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "부수술비용", "", "");
                    frm.ShowDialog();
                }
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "추가입원료")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2016-07-01") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "추가입원료", "", "");
                    frm.ShowDialog();
                }
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "응급가산수가")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2016-07-01") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "응급가산수가", "", "");
                    frm.ShowDialog();
                }
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "의료질평가지원금")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2015-09-01") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "의료질평가지원", "", "");
                    frm.ShowDialog();
                }
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "간호간병료")
            {
                frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "간호간병료", "", "");
                frm.ShowDialog();
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "급여초음파")
            {
                frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "급여초음파", "", "");
                frm.ShowDialog();
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "인정비급여")
            {
                frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "인정비급여", "", "");
                frm.ShowDialog();
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "안전.야간.감염예방관리료")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2018-01-01") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "안전.야간.감염예방관리료", "", "");
                    frm.ShowDialog();
                }
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "수정체제외")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2020-01-01") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "수정체제외", "", "");
                    frm.ShowDialog();
                }
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "별도보상(ADD)")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2020-01-01") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "별도보상(ADD)", "", "");
                    frm.ShowDialog();
                }
            }
            else if (ssDrg_Sheet1.Cells[e.Row, 0].Text == "선별급여")
            {
                if (string.Compare(clsPmpaType.TIT.InDate, "2017-01-31") >= 0)
                {
                    frmPmpaViewMedicalDetailList frm = new frmPmpaViewMedicalDetailList("", "DRG", FnIPDNO, FnTRSNO, "", "", "선별급여", "", "");
                    frm.ShowDialog();
                }
            }
        }
        
        string Rtn_SSTrans_VCode(string strVCode)
        {
            string rtnVal = string.Empty;

            switch (strVCode)
            {
                case "V191":
                case "V192":
                case "V193":
                case "V194":
                case "V268":
                case "V275":
                case "V247":
                case "V248":
                case "V249":
                case "V250":
                    rtnVal = "중증"; break;
                default:
                    break;
            }

            return rtnVal;
        }

        void Make_Temp_Trans(PsmhDb pDbCon)
        {
            string strInDate = string.Empty;
            string strOutDate = string.Empty;

            string SQL = "";
            string SqlErr = "";
            int intRowAffected = 0;

            if (clsPmpaType.TIT.Trsno == 0)
            {
                ComFunc.MsgBox("환자 선택 후 작업하세요", "대상없음");
                return;
            }

            if (ComFunc.MsgBoxQ("설정한 기간으로 임시자격을 생성하시겠습니까??", "임시자격 생성", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            strInDate = dtpDate1.Text;
            strOutDate = dtpDate2.Text;

            Cursor.Current = Cursors.WaitCursor;
            
            clsDB.setBeginTran(pDbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + "INSERT INTO " + ComNum.DB_PMPA + "WORK_IPD_TRANS_TERM (                                   ";
                SQL += ComNum.VBLF + "       TrsNo,IPDNO,PANO,GBIPD,INDATE,OUTDATE,ActDate,DEPTCODE,DRCODE,ILSU,BI,KIHO,        ";
                SQL += ComNum.VBLF + "       GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,AMSET1,AMSET2,AMSET3,AMSET4,    ";
                SQL += ComNum.VBLF + "       AMSET5,AMSETB,FROMTRANS,ERAMT,REMARK,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,DTGAMEK,       ";
                SQL += ComNum.VBLF + "       OGPDBUN,OGPDBUNdtl,ENTDATE,ENTSABUN,GBSTS,GELCODE,Gbilban2,GbSPC)                  ";

                SQL += ComNum.VBLF + " SELECT TrsNo,IPDNO,PANO,GBIPD,TO_DATE('" + strInDate + "','YYYY-MM-DD'),                 ";
                SQL += ComNum.VBLF + "        TO_DATE('" + strOutDate + "','YYYY-MM-DD'),ActDate,DEPTCODE,DRCODE,ILSU,BI,KIHO,  ";
                SQL += ComNum.VBLF + "        GKIHO,PNAME,GWANGE,BONRATE,GISULRATE,GBGAMEK,BOHUN,AMSET1,AMSET2,AMSET3,AMSET4,   ";
                SQL += ComNum.VBLF + "        AMSET5,AMSETB,FROMTRANS,ERAMT,REMARK,JUPBONO,GBDRG,DRGWRTNO,SANGAMT,DTGAMEK,      ";
                SQL += ComNum.VBLF + "        OGPDBUN,OGPDBUNdtl,ENTDATE,ENTSABUN,GBSTS,GELCODE,Gbilban2,GbSPC                  ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "IPD_TRANS ";
                SQL += ComNum.VBLF + "  WHERE TRSNO =" + clsPmpaType.TIT.Trsno + " ";
                
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, pDbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(pDbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(pDbCon);
                ComFunc.MsgBox ("임시자격 생성 완료", "작업완료");
                Cursor.Current = Cursors.Default;
                
                return;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(pDbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        void Print_Bill(PsmhDb pDbCon)
        {
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            string strInDate = string.Empty;
            string strOutDate_Drg = string.Empty;
            string strNgt = string.Empty;
            string strDrgCode = string.Empty;
            string strPano = string.Empty;

            long nChaamt = 0;
            long nIPDNO = 0;
            long nTRSNO = 0;
            
            if (FnIPDNO == 0)
            {
                ComFunc.MsgBox("환자선택 안됨.", "출력불가");
                return;
            }

            clsIument cIM = new clsIument();
            clsPmpaPrint cPP = new clsPmpaPrint();
            DRG DRG = new DRG();

            //자격정보 읽기
            cIM.Read_Ipd_Mst_Trans(pDbCon, FstrPano, FnTRSNO, "");
            
            strPano = clsPmpaType.TIT.Pano;
            strDrgCode = clsPmpaType.TIT.DrgCode;
            nIPDNO = clsPmpaType.TIT.Ipdno;
            nTRSNO = clsPmpaType.TIT.Trsno;

            // 영수증 항목별금액 합산 vb60new\report_print2.bas
            if (clsPmpaType.TIT.GbDRG == "D")
            {
                #region DRG 이전 Data를 위한 루틴
                //고도화 사업 이전 DRG 계산을 위한 임시용 루틴입니다.
                //일정시점이 지나면 아래루틴을 제거 하세요.
                if (clsPmpaType.TIT.Amt[70] >= 0)           //DRG 예전버전인 경우 
                //if (0 == 0)           //DRG 예전버전인 경우 

                {
                    //퇴원일자가 없으면 현재일자로 세팅
                    if (clsPmpaType.TIT.OutDate == "")
                    {
                        strOutDate_Drg = clsPublic.GstrSysDate;
                    }
                    else
                    {
                        strOutDate_Drg = clsPmpaType.TIT.OutDate;
                    }

                    strInDate = clsPmpaType.TIT.InDate;
                    //strOutDate_Drg = "2017-06-30";

                    strNgt = DRG.Read_GbNgt_DRG(pDbCon, strPano, nIPDNO, nTRSNO);

                    if (DRG.READ_DRG_AMT_MASTER(pDbCon, strDrgCode, strPano, nIPDNO, nTRSNO, strNgt, strInDate, strOutDate_Drg) == false)
                    {
                        Cursor.Current = Cursors.Default;
                        ComFunc.MsgBox("DRG 금액을 계산 도중에 오류가 발생함!!");
                        return;
                    }
                }
                #endregion

                //DRG 금액 RPG.Amt 변수에 저장 (인정비급여, 선별급여 포함)
                cIM.Ipd_Trans_PrtAmt_Read_Drg(pDbCon, nTRSNO);

                if (ssDrg != null && ssDrgAmt != null)
                {
                    //DRG 세부내역 표시
                    cIM.Ipd_Tewon_PrtAmt_Gesan_Drg(pDbCon, ssDrg, ssDrgAmt, strPano, nIPDNO, nTRSNO);
                }
            }
            else
            {
                cIM.Ipd_Trans_PrtAmt_Read(pDbCon, nTRSNO, "");
                cIM.Ipd_Tewon_PrtAmt_Gesan(pDbCon, strPano, nIPDNO, nTRSNO, "", "");
            }
            
            #region 이미납부한 금액
            SQL = "";
            SQL = "SELECT SUM(SUM(CASE WHEN SUNEXT = 'Y88' THEN AMT END)) Y88 ";
            SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "IPD_NEW_CASH  ";
            SQL = SQL + ComNum.VBLF + " WHERE TRSNO =" + nTRSNO + " ";
            SQL = SQL + ComNum.VBLF + "   AND SuNext IN ('Y88') ";
            SQL = SQL + ComNum.VBLF + " GROUP BY SuNext ";

            SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                clsPmpaType.TIT.Amt[51] = (long)VB.Val(dt.Rows[0]["Y88"].ToString().Trim());
            }

            dt.Dispose();
            dt = null;
            #endregion

            


            clsDB.setBeginTran(pDbCon);
            
            //cPP.IPD_Sunap_Report_A4_New(pDbCon, FstrPano, FnIPDNO, FnTRSNO, "", chkSaBon.Checked);  //한기호
            cPP.IPD_Sunap_Report_A4_New(pDbCon, FstrPano, FnIPDNO, FnTRSNO, "", chkSaBon.Checked);

            clsDB.setCommitTran(pDbCon);

            if (clsPmpaType.TIT.GbDRG == "D")
            {
                nChaamt = (DRG.GnDrgJohapAmt + DRG.GnDrgFoodAmt[1] + DRG.GnDrgRoomAmt[1] + DRG.GnGs50Amt_J + DRG.GnGs80Amt_J + DRG.GnGs90Amt_J) - clsPmpaType.TIT.Amt[53];
            }
            else
            {
                nChaamt = clsPmpaType.RPG.Amt6[50] - clsPmpaType.TIT.Amt[53];
            }

            if ((nChaamt <= -100 || nChaamt >= 100) && string.Compare(clsPmpaType.TIT.Bi, "30") <= 0)
            {
                ComFunc.MsgBox(nChaamt + "원 영수증 차액이 발생되었습니다.참고사항!!");
            }


        }
    }
}
