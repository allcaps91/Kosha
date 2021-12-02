using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using FarPoint.Win;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcPanOpinionAfterMgmtXray.cs
/// Description     : 질병유소견자사후관리(방사선)
/// Author          : 이상훈
/// Create Date     : 2019-10-31
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm질병유소견자사후관리방사선.frm(Frm질병유소견자사후관리방사선)" />

namespace ComHpcLibB
{
    public partial class frmHcPanOpinionAfterMgmtXray : Form
    {
        HicJepsuResSpecialService hicJepsuResSpecialService = null;
        HicResultService hicResultService = null;
        HicJepsuService hicJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();
        clsHcFunc hf = new clsHcFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        string FstrLtdCode;
        string FstrFDate;
        string FstrTDate;

        long FnWRTNO;   //1차 접수번호
        long FnWrtno2;  //2차 접수번호

        string FstrSex;

        int FnCNT1_1;   //특수1차 건수
        int FnCNT1_2;   //특수2차 건수
        int FnCNT2_1;   //일반1차 건수
        int FnCNT2_2;   //일반2차 건수

        int FnCNT3_1;   //생애1차만건수 2010
        int FnCNT3_2;   // 생애2차했는건수 2010

        long FnPano;
        string FstrSName = "";
        string FstrUCodes = "";
        string FstrJepDate = "";
        string FstrGjYear = "";
        string FstrGjBangi = "";
        string FstrGjJong = "";

        int FnRow;
        int FnDoctCnt;
        long[] FnPanDrNo = new long[10];
        int FnStartRow;

        string FstrDate1;
        string FstrDate2;

        string Fstr생애구분;

        string FstrGubun;
        string FstrSogen;
        string FstrExams;
        string FstrJochi;

        int nREAD = 0;
        long nSpcRow = 0;
        int nSpcRowStart = 0;
        int nRow = 0;
        long nResult2CNT = 0;
        long nPanjengDrno = 0;
        int nSpcResCnt = 0; //특검 유질환 물질 건수

        string strSname = "";
        string strSex = "";
        long nAge = 0;
        string strIpsadate = "";
        string strJepDate = "";
        long nGunsok = 0;
        long nGunsokYY = 0;
        long nGunsokMM = 0;
        int nRowCnt2 = 0;
        string strPanName = "";
        string strPanjeng = "";
        string strGbPanBun2 = "";
        string strSogen = "";
        string strResult = "";
        string strOK = "";
        string strGong = "";
        string strUNames = "";
        string strUNames_EDIT = "";
        string strOldData = "";
        string strNewData = "";
        string strMCode = "";
        string strResult2 = "";
        string strTemp = "";

        public frmHcPanOpinionAfterMgmtXray()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicJepsuResSpecialService = new HicJepsuResSpecialService();
            hicJepsuService = new HicJepsuService();
            hicResultService = new HicResultService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnListView.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnExcel.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.chkExcel.Click += new EventHandler(eCheckBoxClick);
            //this.txtLtdCode.Click += new EventHandler(eTxtClick);
            this.cboYear.SelectedIndexChanged += new EventHandler(eCboClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            long nYEAR = 0;

            this.Location = new Point(10, 10);

            ComFunc.ReadSysDate(clsDB.DbCon);

            nYEAR = VB.Left(clsPublic.GstrSysDate, 4).To<long>();

            //건진년도 Combo SET
            cboYear.Items.Clear();
            for (int i = 0; i < 5; i++)
            {
                cboYear.Items.Add(nYEAR);
                nYEAR -= 1;
            }

            cboYear.SelectedIndex = 0;

            cboPrtCnt.Items.Clear();
            cboPrtCnt.Items.Add("1");
            cboPrtCnt.Items.Add("2");
            cboPrtCnt.Items.Add("3");
            cboPrtCnt.SelectedIndex = 0;

            //반기
            cboBangi.Items.Clear();
            cboBangi.Items.Add("전체");
            cboBangi.Items.Add("상반기");
            cboBangi.Items.Add("하반기");
            cboBangi.SelectedIndex = 0;

            dtpFrDate.Text = VB.Left(clsPublic.GstrSysDate, 4) + "-01-01";
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            lblLtdName.Text = "";

            fn_Screen_Clear();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnListView)
            {
                int nREAD = 0;
                int nRow = 0;
                string strGjYear = "";
                string strGjBangi = "";
                long nLtdCode = 0;
                string strFrate = "";
                string strToDate = "";

                sp.Spread_All_Clear(ssList);
                ssList.ActiveSheet.RowCount = 30;
                ssList.Enabled = false;

                lblLtdName.Text = "";
                txtJepDate.Text = "";

                strGjYear = cboYear.Text;
                strFrate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                if (cboBangi.Text == "상반기")
                {
                    strGjBangi = "1";
                }
                else if (cboBangi.Text == "하반기")
                {
                    strGjBangi = "2";
                }

                if (txtLtdCode.Text.Trim() != "")
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                }

                //자료를 SELECT
                List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItembyMunjinNameLtdCodeCount(strFrate, strToDate, strGjYear, strGjBangi, nLtdCode);

                nREAD = list.Count;
                nRow = 0;
                ssList.ActiveSheet.RowCount = nREAD;
                ssList_Sheet1.Rows.Get(-1).Height = 24;
                progressBar1.Maximum = nREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (list[i].NAME != null)
                    {
                        ssList.ActiveSheet.Cells[i, 0].Text = list[i].NAME;
                    }
                    ssList.ActiveSheet.Cells[i, 1].Text = list[i].CNT.To<string>();
                    ssList.ActiveSheet.Cells[i, 2].Text = list[i].LTDCODE.To<string>();
                    ssList.ActiveSheet.Cells[i, 3].Text = list[i].MINDATE;
                    FstrDate1 = list[i].MINDATE.To<string>();
                    ssList.ActiveSheet.Cells[i, 4].Text = list[i].MAXDATE;
                    FstrDate2 = list[i].MAXDATE.To<string>();
                    progressBar1.Value = i + 1;
                }

                ssList.Enabled = true;
            }
            else if (sender == btnLtdCode)
            {
                string strLtdCode = "";

                if (txtLtdCode.Text.IndexOf(".") > 0)
                {
                    strLtdCode = VB.Pstr(txtLtdCode.Text, ".", 2);
                }
                else
                {
                    strLtdCode = txtLtdCode.Text;
                }

                FrmHcLtdHelp = new frmHcLtdHelp(strLtdCode);
                FrmHcLtdHelp.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                FrmHcLtdHelp.ShowDialog();
                FrmHcLtdHelp.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.To<string>() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "방사선종사자 사후관리 소견서";                
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("사업장명: " + VB.Pstr(txtLtdCode.Text, ".", 2) + VB.Space(20) + "건강진단실시기간: " + txtJepDate.Text + VB.Space(25) + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                for (int i = 0; i < int.Parse(cboPrtCnt.Text); i++) //인쇄매수
                {
                    sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
                }
            }
            else if (sender == btnExcel)
            {
                string strPath = "C:\\ExcelFileDown\\";
                string strFileName = "C:\\ExcelFileDown\\방사선종사자사후관리소견서_" + lblLtdName.Text + "(" + txtJepDate.Text + ")";

                hf.Excel_File_Create(strPath, strFileName, SS1, SS1_Sheet1);
            }
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }

        void eTxtClick(object sender, EventArgs e)
        {
            if (sender == txtLtdCode)
            {
                eBtnClick(btnLtdCode, new EventArgs());
            }
        }

        void eCboClick(object sender, EventArgs e)
        {
            if (sender == cboYear)
            {
                dtpFrDate.Text = cboYear.Text + "-01-01";
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                string strJepDate2 = "";
                string strFDate = "";
                string strTDate = "";
                string strGjYear = "";

                lblLtdName.Text = " " + ssList.ActiveSheet.Cells[e.Row, 0].Text.Trim();
                FstrLtdCode = ssList.ActiveSheet.Cells[e.Row, 2].Text.Trim();
                FstrFDate = ssList.ActiveSheet.Cells[e.Row, 3].Text.Trim();
                FstrTDate = ssList.ActiveSheet.Cells[e.Row, 4].Text.Trim();
                strGjYear = cboYear.Text;

                HIC_JEPSU list = hicJepsuService.GetMinMaxDatebyWrtNo(FstrFDate, FstrTDate, strGjYear, FstrLtdCode);

                if (!list.IsNullOrEmpty())
                {
                    strFDate = list.MINDATE;
                    strTDate = list.MAXDATE;
                }

                if (strFDate == "" || strTDate == "")
                {
                    txtJepDate.Text = FstrDate1 + " ~ " + FstrDate2;
                }
                else
                {
                    txtJepDate.Text = strFDate + " ~ " + strTDate;
                }

                fn_Screen_Display();

                MessageBox.Show("작업 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        void eCheckBoxClick(object sender, EventArgs e)
        {
            if (chkExcel.Checked == true)
            {
                btnPrint.Enabled = false;
                btnExcel.Enabled = true;
            }
            else
            {
                btnPrint.Enabled = true;
                btnExcel.Enabled = false;
            }
        }

        void fn_Screen_Clear()
        {
            txtLtdCode.Text = "";
            txtJepDate.Text = "";
            btnPrint.Enabled = false;
            SS1.ActiveSheet.RowCount = 50;
            //의사도장 복사 때문에 수동으로 클리어

            sp.Spread_All_Clear(SS1);

            SS1_Sheet1.Rows.Get(-1).Border = new LineBorder(Color.Black, 1, false, false, false, false);
        }

        void fn_Screen_Display()
        {
            int nRow = 0;
            int nREAD = 0;
            long nPanjengDrno = 0;
            string strOK = "";
            int nPrtCNT = 0;
            string strGjYear = "";
            string strBangi = "";
            string strJob = "";
            long nWrtNo = 0;
            string[] strResult = new string[4];
            string strPanjeng = "";
            long nCNT = 0;

            SS1.ActiveSheet.RowCount = 50;
            sp.Spread_All_Clear(SS1);
            SS1_Sheet1.Rows.Get(-1).Border = new LineBorder(Color.Black, 1, false, false, false, false);
            btnPrint.Enabled = true;
            Cursor.Current = Cursors.WaitCursor;

            //변수를 Clear
            FnRow = 0;
            FnDoctCnt = 0;
            nPrtCNT = 0;

            strGjYear = cboYear.Text;
            if (cboBangi.Text.Trim() == "상반기")
            {
                strBangi = "1";
            }
            else if (cboBangi.Text.Trim() == "하반기")
            {
                strBangi = "2";
            }
            
            for (int i = 0; i < 10; i++)
            {
                FnPanDrNo[i] = 0;
            }

            //회사별 특수검진 명단을 읽음
            List<HIC_JEPSU_RES_SPECIAL> list = hicJepsuResSpecialService.GetItembyMunjinJepDateLtdCodeGjYear(FstrFDate, FstrTDate, FstrLtdCode, strGjYear, strBangi, strJob);

            nREAD = list.Count;
            SS1.ActiveSheet.RowCount = nREAD;
            SS1_Sheet1.Rows.Get(-1).Height = 24;
            nRow = 0;

            progressBar1.Maximum = nREAD;
            for (int i = 0; i < nREAD; i++)
            {
                nWrtNo = list[i].WRTNO;
                nRow += 1;
                if (nRow > SS1.ActiveSheet.RowCount)
                {
                    SS1.ActiveSheet.RowCount = nRow;
                }

                SS1.ActiveSheet.Cells[i, 0].Text = list[i].SNAME;

                if (list[i].SEX == "M")
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = "남";
                }
                else
                {
                    SS1.ActiveSheet.Cells[i, 1].Text = "여";
                }
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].AGE.To<string>();
                SS1.ActiveSheet.Cells[i, 3].Text = list[i].JEPDATE;

                strPanjeng = list[i].SOGEN + "/";
                strPanjeng += list[i].PANJENG;

                //WBC,PLT,Hb,RBC 검사결과를 읽음
                List<HIC_RESULT> list2 = hicResultService.GetExCodeResultbyWrtNo(nWrtNo, "Y");

                strResult[0] = "";
                strResult[1] = "";
                strResult[2] = "";
                strResult[3] = "";

                for (int j = 0; j < list2.Count; j++)
                {
                    switch (list2[j].EXCODE)
                    {
                        case "A282":
                            strResult[0] = list2[j].RESULT;
                            break;
                        case "H805":
                            strResult[1] = list2[j].RESULT;
                            break;
                        case "A121":
                            strResult[2] = list2[j].RESULT;
                            break;
                        case "A283":
                            strResult[3] = list2[j].RESULT;
                            break;
                        default:
                            break;
                    }
                }

                SS1.ActiveSheet.Cells[i, 4].Text = strResult[0];
                SS1.ActiveSheet.Cells[i, 5].Text = strResult[1];
                SS1.ActiveSheet.Cells[i, 6].Text = strResult[2];
                SS1.ActiveSheet.Cells[i, 7].Text = strResult[3];

                SS1.ActiveSheet.Cells[i, 8].Text = strPanjeng;
                progressBar1.Value = i + 1;
            }

            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 8);
                SS1.ActiveSheet.Rows[i].Height = size.Height;
            }
            Cursor.Current = Cursors.Default;
        }
    }
}
