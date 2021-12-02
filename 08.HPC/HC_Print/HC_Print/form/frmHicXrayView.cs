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
using System.Drawing.Printing;
using System.Windows.Forms;





/// <summary>
/// Class Name      : 
/// File Name       : frmHicXrayView.cs
/// Description     : 건진판독명단 리스트
/// Author          : 김경동
/// Create Date     : 2021-06-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm건진판독명단.frm(Frm건진판독명단)" />
namespace HC_Print
{
    public partial class frmHicXrayView : Form
    {

        string fstrPtno = "";
        string fstrXrayNo = "";


        ComFunc cf = new ComFunc();
        clsHaBase hb = new clsHaBase();
        clsHcMain cHcMain = new clsHcMain();
        clsSpread sp = new clsSpread();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        HicXrayResultService hicXrayResultService = null;
        HicComHpcService hicComHpcService = null;
        HicExjongService hicExjongService = null;



        public frmHicXrayView()
        {
            InitializeComponent();
            SetEvent();
            SetControl();

        }
        private void SetEvent()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdHelp.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);

            this.SSList.CellDoubleClick += new CellClickEventHandler(eSpdDblClick);


        }

        private void SetControl()
        {

            hicXrayResultService = new HicXrayResultService();
            hicComHpcService = new HicComHpcService();
            hicExjongService = new HicExjongService();

        }

        private void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFDate.Text = DateTime.Now.AddDays(0).ToShortDateString();
            dtpTDate.Text = DateTime.Now.ToShortDateString();

            txtLtdCode.Text = "";
            lblLtdName.Text = "";

            cboJong.Items.Clear();
            cboJong.Items.Add("**.전체");
            List<HIC_EXJONG> list = hicExjongService.Read_ExJong_Add(true);
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    cboJong.Items.Add(list[i].CODE + "." + list[i].NAME);
                }
            }
            cboJong.SelectedIndex = 0;
        }

        private void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display(SSList);
                btnPrint.Enabled = true;
            }

            #region 사업장코드찾기
            else if (sender == btnLtdHelp)
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
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            #endregion
            else if (sender == btnPrint)
            {
                string strTitle = "";
                string strHeader = "";
                string strFooter = "";
                bool PrePrint = true;

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "건진 방사선 리스트 명단";

                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("      작업기간 : " + dtpFDate.Text + " ~ " + dtpTDate.Text    , new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("      인쇄시각 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.7f);
                sp.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);

            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        private void eSpdDblClick(object sender, CellClickEventArgs e)
        {
            if (sender == SSList)
            {

                string strXrayNo = "";
                string strRead = "";
                string strGbChul = "";
                string strPtno = "";


                strXrayNo = SSList.ActiveSheet.Cells[e.Row, 12].Text;
                if (SSList.ActiveSheet.Cells[e.Row, 13].Text.IsNullOrEmpty())
                {
                    strRead = "1";
                }
                else
                {
                    strRead = "2";
                }
                strGbChul = SSList.ActiveSheet.Cells[e.Row, 14].Text;
                strPtno = SSList.ActiveSheet.Cells[e.Row, 18].Text;


                if (e.Column == 1)
                {
                    if(strRead.IsNullOrEmpty())
                    {
                        if (MessageBox.Show("선택하신 자료를 분진촬영으로 바꾸겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("선택하신 자료를 일반촬영으로 바꾸겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    if (strXrayNo.IsNullOrEmpty())
                    {
                        return;
                    }


                    if(strRead.IsNullOrEmpty())
                    {
                        int result = hicXrayResultService.UpdateGbReadByXrayNo("2", strXrayNo);

                        if (result < 0)
                        {
                            MessageBox.Show("자료 업데이트시 에러발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    else
                    {
                        int result = hicXrayResultService.UpdateGbReadByXrayNo("1", strXrayNo);

                        if (result < 0)
                        {
                            MessageBox.Show("자료 업데이트시 에러발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    strXrayNo = "";
                    btnSearch.PerformClick();
                }
                else if (e.Column == 2)
                {
                    if (strGbChul == "Y")
                    {
                        if (MessageBox.Show("선택하신 자료의 검진구분을 내원으로 바꾸겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (MessageBox.Show("선택하신 자료의 검진구분을 출장으로 바꾸겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    if (strXrayNo.IsNullOrEmpty())
                    {
                        return;
                    }

                    if (strGbChul == "Y")
                    {
                        int result = hicXrayResultService.UpdateGbChulByXrayNo("N", strXrayNo);

                        if (result < 0)
                        {
                            MessageBox.Show("자료 업데이트시 에러발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        int result = hicXrayResultService.UpdateGbChulByXrayNo("Y", strXrayNo);

                        if (result < 0)
                        {
                            MessageBox.Show("자료 업데이트시 에러발생", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    strXrayNo = "";
                    btnSearch.PerformClick();
                }
                else if (e.Column == 6)
                {
                    if (MessageBox.Show("판독결과지 인쇄를 하시겠습니까?", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {

                        fstrPtno = ""; fstrXrayNo = "";
                        fstrPtno = strPtno; fstrXrayNo = strXrayNo;

                        clsPrint CP = new clsPrint();
                        PrintDocument pd;

                        pd = new PrintDocument();
                        pd.PrintPage += new PrintPageEventHandler(Print_PandokResult);
                        pd.Print();    //프린트
                    }
                }
            }
        }


        private void Screen_Display(FpSpread Spd)
        {
            int nRow = 0;
            long nLtdCode = 0;

            string strJong = "";
            string strGbsts = "";
            string strGbChul = "";
            string strXrayNo = "";

            SSList.ActiveSheet.RowCount = 0;

            if (rdoGubun2.Checked) { strGbChul = "N"; }
            if (rdoGubun3.Checked) { strGbChul = "Y"; }

            if (rdoPanGubun2.Checked) { strGbsts = "2"; } //미판독
            if (rdoPanGubun3.Checked) { strGbsts = "3"; } //판독자
            if (rdoPanGubun4.Checked) { strGbsts = "4"; } //보류자

            strJong = VB.Left(cboJong.Text, 2);

            List<HIC_XRAY_RESULT> list = hicXrayResultService.GetItemByJepdateJongLtd(dtpFDate.Text, dtpTDate.Text, strJong, nLtdCode, strGbsts, strGbChul);

            if (!list.IsNullOrEmpty())
            {

                for (int i = 0; i < list.Count; i++)
                {

                    nRow += 1;
                    SSList.ActiveSheet.RowCount += 1;
                    strXrayNo = list[i].XRAYNO.Trim();

                    SSList.ActiveSheet.Cells[nRow - 1, 0].Text = VB.Left(strXrayNo, 8) + " " + VB.Right(strXrayNo, 5);
                    SSList.ActiveSheet.Cells[nRow - 1, 1].Text = hb.READ_GjJong_Name(list[i].GJJONG);
                    SSList.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                    SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SNAME;
                    SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].RESULT2.To<string>("");
                    SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list[i].RESULT3.To<string>("");
                    SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list[i].RESULT4.To<string>("");
                    SSList.ActiveSheet.Cells[nRow - 1, 7].Text = list[i].AGE + "/" + list[i].SEX;

                    if (list[i].GBSTS == "2")
                    {
                        SSList.ActiveSheet.Cells[nRow - 1, 8].Text = "◎";
                    }
                    if (list[i].GBPACS == "Y")
                    {
                        SSList.ActiveSheet.Cells[nRow - 1, 9].Text = "▦";
                    }

                    SSList.ActiveSheet.Cells[nRow - 1, 10].Text = hb.Read_ExCode_Name(list[i].XCODE);
                    SSList.ActiveSheet.Cells[nRow - 1, 11].Text = list[i].ROWID;
                    SSList.ActiveSheet.Cells[nRow - 1, 12].Text = list[i].XRAYNO;

                    SSList.ActiveSheet.Cells[nRow - 1, 14].Text = list[i].GBCHUL;
                    SSList.ActiveSheet.Cells[nRow - 1, 15].Text = list[i].DELDATE.ToString();

                    SSList.ActiveSheet.Cells[nRow - 1, 16].Text = list[i].READDOCT1.ToString();
                    SSList.ActiveSheet.Cells[nRow - 1, 18].Text = list[i].PTNO;
                }
            }

            //미판독시
            if (strGbsts == "2")
            {
                List<COMHPC> list2 = hicComHpcService.GetItemByDate(dtpFDate.Text, dtpTDate.Text);


                if (!list2.IsNullOrEmpty())
                {
                    for (int i = 0; i < list2.Count; i++)
                    {
                        nRow += 1;
                        SSList.ActiveSheet.RowCount += 1;

                        strXrayNo = list2[i].XRAYNO.Trim();

                        SSList.ActiveSheet.Cells[nRow - 1, 0].Text = VB.Left(strXrayNo, 8) + " " + VB.Right(strXrayNo, 5);
                        SSList.ActiveSheet.Cells[nRow - 1, 1].Text = hb.READ_GjJong_Name(list2[i].GJJONG);
                        SSList.ActiveSheet.Cells[nRow - 1, 2].Text = hb.READ_Ltd_Name(list2[i].LTDCODE.ToString());
                        SSList.ActiveSheet.Cells[nRow - 1, 3].Text = list2[i].SNAME;
                        SSList.ActiveSheet.Cells[nRow - 1, 4].Text = list2[i].RESULT2.Trim();
                        SSList.ActiveSheet.Cells[nRow - 1, 5].Text = list2[i].RESULT3.Trim();
                        SSList.ActiveSheet.Cells[nRow - 1, 6].Text = list2[i].RESULT4.Trim();
                        SSList.ActiveSheet.Cells[nRow - 1, 7].Text = list2[i].AGE + "/" + list2[i].SEX;

                        if (list2[i].GBSTS == "2")
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, 8].Text = "◎";
                        }
                        if (list2[i].GBPACS == "Y")
                        {
                            SSList.ActiveSheet.Cells[nRow - 1, 9].Text = "▦";
                        }

                        SSList.ActiveSheet.Cells[nRow - 1, 10].Text = hb.Read_ExCode_Name(list2[i].XCODE);
                        SSList.ActiveSheet.Cells[nRow - 1, 11].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 12].Text = list2[i].XRAYNO;

                        SSList.ActiveSheet.Cells[nRow - 1, 14].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 15].Text = "";

                        SSList.ActiveSheet.Cells[nRow - 1, 16].Text = "";
                        SSList.ActiveSheet.Cells[nRow - 1, 18].Text = list2[i].PTNO;

                    }
                }
            }
        }

        private void Print_PandokResult(object sender, PrintPageEventArgs ev)
        {
            int nX = 0;
            int nY = 0;
            int nCY = 17;

            BarcodeLib.Barcode b = new BarcodeLib.Barcode();

            HIC_XRAY_RESULT item = hicXrayResultService.GetItemByPtnoXrayno(fstrPtno, fstrXrayNo);

            if (!item.IsNullOrEmpty())
            {

                ev.Graphics.DrawString(VB.Space(21) + "방사선 촬영 결과지", new Font("맑은 고딕", 20f, FontStyle.Bold), Brushes.Black, nX + 5, nY + (nCY * 1), new StringFormat());
                ev.Graphics.DrawString(VB.Space(5) + "=====================================================================================", new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 2), new StringFormat());
                ev.Graphics.DrawString(VB.Space(1), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 3), new StringFormat());
                ev.Graphics.DrawString(VB.Space(5) + "등록번호: "+ ComFunc.LeftH(fstrPtno +VB.Space(20), 20) + "성 명: " + ComFunc.LeftH(item.SNAME + VB.Space(20), 20), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 4), new StringFormat());
                ev.Graphics.DrawString("성    별: "+ item.AGE + "/" + item.SEX, new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 5), new StringFormat());
                ev.Graphics.DrawString(VB.Space(5) + "의 뢰 과: 일반건진" + VB.Space(12) + "의 사 : 일반건진" + VB.Space(12), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 7), new StringFormat());
                ev.Graphics.DrawString(VB.Space(5) + "검사요청일: " + item.JEPDATE, new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 8), new StringFormat());
                ev.Graphics.DrawString(VB.Space(5) + "=====================================================================================", new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 10), new StringFormat());
                ev.Graphics.DrawString(VB.Space(5) + "검사명: Chest PA", new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 13), new StringFormat());

                if (item.RESULT2_1.IsNullOrEmpty())
                {
                    ev.Graphics.DrawString(VB.Space(13)+"판정결과: " + item.RESULT2.Trim() + "." + item.RESULT3.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 16), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13)+"판정소견: " + item.RESULT4.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 17), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13)+"판정일자: " + item.READTIME1.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 18), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13)+"판정의사: " + hb.READ_HIC_InsaName(item.READDOCT1.ToString()), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 19), new StringFormat());
                }
                else
                {
                    ev.Graphics.DrawString(VB.Space(13) + "판정결과1: " + item.RESULT2.Trim() + "." + item.RESULT3.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 16), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13) + "판정소견1: " + item.RESULT4.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 17), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13) + "판정일자1: " + item.READTIME1.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 18), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13) + "판정의사1: " + hb.READ_HIC_InsaName(item.READDOCT1.ToString()), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 19), new StringFormat());

                    ev.Graphics.DrawString(VB.Space(13) + "판정결과2: " + item.RESULT2_1.Trim() + "." + item.RESULT3_1.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 22), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13) + "판정소견2: " + item.RESULT4_1.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 23), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13) + "판정일자2: " + item.READTIME2.Trim(), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 24), new StringFormat());
                    ev.Graphics.DrawString(VB.Space(13) + "판정의사2: " + hb.READ_HIC_InsaName(item.READDOCT2.ToString()), new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 25), new StringFormat());
                }


                ev.Graphics.DrawString(VB.Space(5) + "-------------------------------------------------------------------------------------", new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 53), new StringFormat());
                ev.Graphics.DrawString(VB.Space(5) + "※포항성모병원 영상의학과※                     전화:054-260-8163  FAX:054-260-8006", new Font("맑은 고딕", 12f), Brushes.Black, nX + 5, nY + (nCY * 54), new StringFormat());
            }
        }
    }
}
