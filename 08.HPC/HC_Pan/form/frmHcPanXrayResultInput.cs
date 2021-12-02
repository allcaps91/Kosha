using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.form.HcView;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using ComLibB;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Pan
/// File Name       : frmHcPanXrayResultInput.cs
/// Description     : 방사선 판독조회 및 결과입력
/// Author          : 이상훈
/// Create Date     : 2019-11-14
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm방사선결과입력.frm(Frm방사선결과입력)" />

namespace HC_Pan
{
    public partial class frmHcPanXrayResultInput : Form
    {
        HicResultService hicResultService = null;
        HicJepsuService hicJepsuService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicRescodeService hicRescodeService = null;
        HicXrayResultService hicXrayResultService = null;
        HicExcodeService hicExcodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        string FstrJepDate;
        string FstrPano;
        string FnRow;
        long FnWRTNO;
        long FnClickRow;    //Help를 Click한 Row
        string FstrSex;
        string FstrRead1;   //첫번째 판독결과 Data
        string FstrRead2;   //두번째 판독결과 Data
        string FstrResult1; //첫번째 판독문
        string FstrResult2; //두번째 판독문

        public frmHcPanXrayResultInput()
        {
            InitializeComponent();
            SetEvent();
        }

        public frmHcPanXrayResultInput(long nWrtNo)
        {
            InitializeComponent();
            FnWRTNO = nWrtNo;
            SetEvent();
        }

        void SetEvent()
        {
            hicResultService = new HicResultService();
            hicJepsuService = new HicJepsuService();
            hicResultExCodeService = new HicResultExCodeService();
            hicRescodeService = new HicRescodeService();
            hicXrayResultService = new HicXrayResultService();
            hicExcodeService = new HicExcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.List1.SelectedIndexChanged += new EventHandler(eListClicked);
            this.SS2.CellClick += new CellClickEventHandler(eSpdClick);
            this.SS2.Change += new ChangeEventHandler(eSpdChange);
        }

        private void eListClicked(object sender, EventArgs e)
        {
            string strGubun = "";
            string strCODE = "";

            strCODE = VB.Pstr(List1.Text, ".", 1).Trim();

            SS2.ActiveSheet.Cells[0, 2].Text = strCODE;
            strGubun = SS2.ActiveSheet.Cells[0, 7].Text.Trim();
            SS2.ActiveSheet.Cells[0, 4].Text = hb.READ_ResultName(strGubun, strCODE);
            SS2.ActiveSheet.Cells[0, 8].Text = "Y"; //변경여부
        }

        void eFormLoad(object sender, EventArgs e)
        {
            long nYear = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            ssPatInfo_Sheet1.Columns.Get(6).Visible = false;

            SS1_Sheet1.Columns.Get(7).Visible = false;
            SS1_Sheet1.Columns.Get(8).Visible = false;
            SS1_Sheet1.Columns.Get(9).Visible = false;
            SS1_Sheet1.Columns.Get(10).Visible = false;
            SS1_Sheet1.Columns.Get(11).Visible = false;
            SS1_Sheet1.Columns.Get(12).Visible = false;

            SS2_Sheet1.Columns.Get(7).Visible = false;
            SS2_Sheet1.Columns.Get(8).Visible = false;
            SS2_Sheet1.Columns.Get(9).Visible = false;
            SS2_Sheet1.Columns.Get(10).Visible = false;

            txtResult.Text = "";

            List1.Items.Clear();
            sp.Spread_All_Clear(ssPatInfo);
            ssPatInfo.ActiveSheet.RowCount = 1;
            sp.Spread_All_Clear(SS1);
            sp.Spread_All_Clear(SS2);
            SS2.ActiveSheet.RowCount = 10;
            FnClickRow = 0;

            txtResult.Text = "";
            
            if (FnWRTNO > 0)
            {
                fn_Screen_Display();
            }
        }

        void eBtnClick(object sender, EventArgs e)
        {
            int result = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                string strResult = "";
                string strCODE = "";
                string strROWID = "";
                string strPanjeng = "";
                string strNewPan = "";
                string strChange = "";
                string strResCode = "";
                int nResult = 0;

                if (txtResult.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("검사결과가 없어서 판정이불가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {
                    strCODE = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    strResult = SS2.ActiveSheet.Cells[i, 2].Text.Trim();
                    strPanjeng = SS2.ActiveSheet.Cells[i, 5].Text.Trim();
                    strChange = SS2.ActiveSheet.Cells[i, 8].Text.Trim();
                    strROWID = SS2.ActiveSheet.Cells[i, 9].Text.Trim();
                    strResCode = SS2.ActiveSheet.Cells[i, 7].Text.Trim();
                    strNewPan = hm.ExCode_Result_Panjeng(strCODE, strResult, FstrSex, FstrJepDate, "");

                    if (strChange == "Y" || strPanjeng != strNewPan)
                    {
                        //결과를 저장
                        result = hicResultService.UpdateResultPanjengbyRowId(strResult, strNewPan, strResCode, clsType.User.IdNumber, strROWID);

                        if (result < 0)
                        {
                            MessageBox.Show(i + " 번줄 검사결과를 등록중 오류 발생", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                txtResult.Text = "";
                List1.Items.Clear();
                
                eBtnClick(btnExit, new EventArgs());
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS2)
            {
                string strResCode = "";
                string strData = "";

                strResCode = SS2.ActiveSheet.Cells[e.Row, 7].Text.Trim();

                FnClickRow = e.Row;

                //자료를 READ
                List<HIC_RESCODE> list = hicRescodeService.GetCodeNamebyGubun("2019");

                List1.Items.Clear();
                
                for (int i = 0; i < list.Count; i++)
                {
                    List1.Items.Add(list[i].CODE.Trim() + "." + list[i].NAME.Trim());
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                string strPANO = "";
                string strPacsNo = "";
                string strROWID = "";

                if (e.Column != 4 && e.Column != 5)
                {
                    return;
                }

                if (e.Column == 4)
                {
                    strROWID = SS1.ActiveSheet.Cells[e.Row, 7].Text.Trim();
                    //해당 판독번호로 판독결과를 READ
                    HIC_XRAY_RESULT list = hicXrayResultService.GetItembyRowId(strROWID);

                    if (list == null)
                    {
                        MessageBox.Show("판독결과가 없습니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (list.GBREAD == "1")
                    {
                        txtResult.Text = "";
                        if (list.RESULT1.IsNullOrEmpty())
                        {
                            txtResult.Text = "\r\n" + "\r\n" + "\r\n";
                            txtResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                        }
                        else
                        {
                            txtResult.Text = "판독분류: " + list.RESULT2 + "\r\n";
                            txtResult.Text += "판독분류명: " + list.RESULT3 + "\r\n";
                            txtResult.Text += "판독소견: " + list.RESULT4 + "\r\n";
                        }
                    }
                    else
                    {
                        if (e.Row == 0)
                        {
                            txtResult.Text = " 【분진판독1】";
                            if (list.RESULT1.IsNullOrEmpty())
                            {
                                txtResult.Text += "\r\n" + "\r\n" + "\r\n";
                                txtResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                            }
                            else
                            {
                                txtResult.Text += "\r\n" + "\r\n";
                                txtResult.Text += "판독분류: " + list.RESULT2 + "\r\n";
                                txtResult.Text += "판독분류명: " + list.RESULT3 + "\r\n";
                                txtResult.Text += "판독소견: " + list.RESULT4 + "\r\n";
                            }
                        }
                        else if (e.Row == 1)
                        {
                            txtResult.Text = " 【분진판독2】";
                            if (list.RESULT1_1.IsNullOrEmpty())
                            {
                                txtResult.Text += "\r\n" + "\r\n" + "\r\n";
                                txtResult.Text += "      ▶ 판독중입니다. 잠시후 다시 확인을 하십시오. ◀";
                            }
                            else
                            {
                                txtResult.Text += "\r\n" + "\r\n";
                                txtResult.Text += "판독분류: " + list.RESULT2_1 + "\r\n";
                                txtResult.Text += "판독분류명: " + list.RESULT3_1 + "\r\n";
                                txtResult.Text += "판독소견: " + list.RESULT4_1 + "\r\n";
                            }
                        }
                    }
                }
                else if (e.Column == 5)
                {
                    //영상 View
                    if (SS1.ActiveSheet.Cells[e.Row, 5].Text == "")
                    {
                        MessageBox.Show("영상 확인 불가", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }

                    strPANO = ssPatInfo.ActiveSheet.Cells[0, 0].Text.Trim();
                    strPacsNo = SS1.ActiveSheet.Cells[e.Row, 1].Text.Trim();
                    clsPacs.PACS_Image_View(clsDB.DbCon, strPANO, strPacsNo, "", false);
                }
            }
        }

        void eSpdChange(object sender, ChangeEventArgs e)
        {
            if (sender == SS2)
            {
                SS2.ActiveSheet.Cells[e.Row, 8].Text = "Y";
            }
        }

        void fn_Screen_Display()
        {
            int nREAD = 0;
            int nRow = 0;
            string strXrayno = "";
            string strSex = "";
            string strExCode = "";
            string strResult = "";
            string strResCode = "";
            string strResultType = "";
            string strGbCodeUse = "";
            string strNomal = "";
            string strJepDate = "";
            string strGbRead = "";
            string strYear = "";

            string strFrDate = "";
            string strToDate = "";

            string strTemp1 = "";
            string strTemp2 = "";
            string strTemp3 = "";
            string strTemp4 = "";

            //인적사항을 READ
            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FnWRTNO);

            if (list == null)
            {
                MessageBox.Show("접수번호가 등록 안됨", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            strJepDate = list.JEPDATE;  //접수일자
            FstrJepDate = list.JEPDATE.ToString();
            strJepDate = FstrJepDate;
            strSex = list.SEX;

            ssPatInfo.ActiveSheet.Cells[0, 0].Text = list.PTNO;
            ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
            ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE.ToString();
            ssPatInfo.ActiveSheet.Cells[0, 3].Text = cf.Read_Ltd_Name(clsDB.DbCon, list.LTDCODE.ToString());
            ssPatInfo.ActiveSheet.Cells[0, 4].Text = FstrJepDate;
            ssPatInfo.ActiveSheet.Cells[0, 5].Text = hb.READ_GjJong_Name(list.GJJONG);  //건진유형

            //검사항목을 Display
            List<HIC_RESULT_EXCODE> list2 = hicResultExCodeService.GetXrayItembyWrtNoExCode(FnWRTNO);

            nREAD = list2.Count;
            SS2.ActiveSheet.RowCount = nREAD;
            nRow = 0;
            
            for (int i = 0; i < nREAD; i++)
            {
                strExCode = list2[i].EXCODE;
                strResult = list2[i].RESULT;
                strResCode = list2[i].RESCODE;
                strResultType = list2[i].RESULTTYPE;
                strGbCodeUse = list2[i].GBCODEUSE;

                SS2.ActiveSheet.Cells[i, 0].Text = list2[i].EXCODE;
                SS2.ActiveSheet.Cells[i, 1].Text = list2[i].HNAME;
                SS2.ActiveSheet.Cells[i, 2].Text = strResult;
                //A103(비만도)는 자동계산(입력금지)
                if (strGbCodeUse == "N" || strExCode == "A103")
                {
                    FarPoint.Win.Spread.CellType.TextCellType spdObj = new FarPoint.Win.Spread.CellType.TextCellType();
                    spdObj.Multiline = false;
                    spdObj.WordWrap = true;

                    SS2.ActiveSheet.Cells[i, 3].CellType = spdObj;

                    SS2.ActiveSheet.Cells[i, 3].Text = "";
                }

                if (strGbCodeUse == "Y")
                {
                    if (!strResult.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[i, 4].Text = hb.READ_ResultName(strResCode, strResult);
                    }
                }

                //참고치를 Dispaly
                strNomal = hm.EXAM_NomalValue_SET(strExCode, FstrJepDate, FstrSex, list2[i].MIN_M, list2[i].MAX_M, list2[i].MIN_F, list2[i].MAX_F);

                SS2.ActiveSheet.Cells[i, 6].Text = strNomal;
                SS2.ActiveSheet.Cells[i, 7].Text = strResCode;
                if (list2[i].EXCODE.Trim() == "A151")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "007";
                }

                if (list2[i].EXCODE.Trim() == "TH01" || list2[i].EXCODE.Trim() == "TH02")
                {
                    SS2.ActiveSheet.Cells[i, 7].Text = "022";
                }
                SS2.ActiveSheet.Cells[i, 8].Text = "";
                SS2.ActiveSheet.Cells[i, 9].Text = list2[i].RID;
                SS2.ActiveSheet.Cells[i, 10].Text = list2[i].RESULTTYPE;
            }
            //년도확인
            strYear = hicJepsuService.GetGjYearbyWrtNo(FnWRTNO);

            //자료를 READ
            List<HIC_RESCODE> list3 = hicRescodeService.GetCodeNamebyGubun(strYear);

            List1.Items.Clear();
            for (int i = 0; i < list3.Count; i++)
            {
                List1.Items.Add(list3[i].CODE.Trim() + "." + list3[i].NAME.Trim());
            }

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 5;
            FstrPano = ssPatInfo.ActiveSheet.Cells[0, 0].Text;
            FstrJepDate = ssPatInfo.ActiveSheet.Cells[0, 4].Text;
            strFrDate = DateTime.Parse(FstrJepDate).AddDays(-14).ToShortDateString();
            strToDate = DateTime.Parse(FstrJepDate).AddDays(14).ToShortDateString();
            strGbRead = "";
            FstrRead1 = "";
            FstrRead2 = "";
            FstrResult1 = "";
            FstrResult2 = "";

            //종합건진 여부 확인
            //if (FstrHGubun == "Y")
            //{
            //    MessageBox.Show("종합검진 수검자입니다. 종합검진에서 판독문 조회 가능", "확인창", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //}
            //else
            //{
                List<HIC_XRAY_RESULT> list4 = hicXrayResultService.GetItemByPtnoJepDateList(FstrPano, strFrDate, strToDate);

                if (list4.Count == 0)
                {
                    MessageBox.Show("본관 촬영 하였거나 촬영하지 않았습니다." + "\r\n" + "OCS 검사에서 확인요망", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                    return;
                }
                nREAD = list4.Count;
                SS1.ActiveSheet.RowCount = nREAD;
                strGbRead = list4[0].GBREAD;
                for (int i = 0; i < nREAD; i++)
                {
                    nRow += 1;
                    if (SS1.ActiveSheet.RowCount < nRow)
                    {
                        SS1.ActiveSheet.RowCount = nRow;
                    }

                    strXrayno = list4[i].XRAYNO;
                    SS1.ActiveSheet.Cells[nRow - 1, 0].Text = list4[i].JEPDATE.ToString();
                    SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list4[i].XRAYNO;
                    SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list4[i].SNAME;
                    SS1.ActiveSheet.Cells[nRow - 1, 3].Text = hb.READ_GjJong_Name(list4[i].GJJONG);
                    if (list4[i].GBSTS == "2")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "◎";
                    }
                    else
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "";
                        
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "";
                    if (list4[i].GBPACS == "Y")
                    {
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "▦";
                    }
                    SS1.ActiveSheet.Cells[nRow - 1, 6].Text = fn_Read_ExCode_Name(list4[i].XCODE);
                    SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list4[i].ROWID;
                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list4[i].XRAYNO;
                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = list4[i].PTNO;
                    SS1.ActiveSheet.Cells[nRow - 1, 10].Text = list4[i].RESULT2;
                    SS1.ActiveSheet.Cells[nRow - 1, 11].Text = list4[i].RESULT3;
                    SS1.ActiveSheet.Cells[nRow - 1, 12].Text = list4[i].RESULT4;

                    if (list4[i].GBREAD == "2")
                    {
                        nRow += 1;
                        if (SS1.ActiveSheet.RowCount < nRow)
                        {
                            SS1.ActiveSheet.RowCount = nRow;
                        }

                        strXrayno = list4[i].XRAYNO;
                        SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list4[i].SNAME;
                        SS1.ActiveSheet.Cells[nRow - 1, 3].Text = hb.READ_GjJong_Name(list4[i].GJJONG);
                        if (list4[i].GBSTS == "2")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "◎";
                            btnSave.Enabled = true;
                        }
                        else
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = "";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "";
                        if (list4[i].GBPACS == "Y")
                        {
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = "▦";
                        }
                        SS1.ActiveSheet.Cells[nRow - 1, 6].Text = fn_Read_ExCode_Name(list4[i].XCODE);
                        SS1.ActiveSheet.Cells[nRow - 1, 7].Text = list4[i].ROWID;
                        SS1.ActiveSheet.Cells[nRow - 1, 8].Text = list4[i].XRAYNO;
                        SS1.ActiveSheet.Cells[nRow - 1, 9].Text = list4[i].PTNO;
                        SS1.ActiveSheet.Cells[nRow - 1, 10].Text = list4[i].RESULT2_1;
                        SS1.ActiveSheet.Cells[nRow - 1, 11].Text = list4[i].RESULT3_1;
                        SS1.ActiveSheet.Cells[nRow - 1, 12].Text = list4[i].RESULT4_1;
                    }
                }

                FstrSex = list4[0].SEX;
            //}

            if (SS1.ActiveSheet.Cells[0, 4].Text == "◎")
            {
                eSpdDClick(SS1, new CellClickEventArgs(new SpreadView(), 0, 4, 439, 36, new MouseButtons(), false, false));
            }

            //방사선등록창에 판독결과를 찍어줌
            //판독결과가 2개인지 확인해보고
            //비교 해봐야함
            if (strGbRead == "1")
            {
                strTemp1 = SS1.ActiveSheet.Cells[0, 10].Text.Trim();
                if (SS1.ActiveSheet.Cells[0, 12].Text.IsNullOrEmpty())
                {
                    strTemp3 = SS1.ActiveSheet.Cells[0, 11].Text.Trim();
                }

                if (SS2.ActiveSheet.Cells[0, 2].Text.IsNullOrEmpty())
                {
                    switch (strTemp1)
                    {
                        case "A":
                            SS2.ActiveSheet.Cells[0, 2].Text = "01";
                            break;
                        case "B":
                            SS2.ActiveSheet.Cells[0, 2].Text = "02";
                            break;
                        case "C":
                            SS2.ActiveSheet.Cells[0, 2].Text = "03";
                            break;
                        case "D-A":
                            SS2.ActiveSheet.Cells[0, 2].Text = "04";
                            break;
                        case "D-B":
                            SS2.ActiveSheet.Cells[0, 2].Text = "05";
                            break;
                        case "D-C":
                            SS2.ActiveSheet.Cells[0, 2].Text = "06";
                            break;
                        case "E":
                            SS2.ActiveSheet.Cells[0, 2].Text = "07";
                            break;
                        default:
                            break;
                    }

                    if (!SS2.ActiveSheet.Cells[0, 2].Text.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[0, 8].Text = "Y";
                    }
                }

                if (SS2.ActiveSheet.Cells[1, 2].Text.IsNullOrEmpty())
                {
                    SS2.ActiveSheet.Cells[1, 2].Text = strTemp3;
                    if (!SS2.ActiveSheet.Cells[1, 2].Text.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[1, 8].Text = "Y";
                    }
                }
            }
            else if (strGbRead == "2")
            {
                strTemp1 = SS1.ActiveSheet.Cells[0, 10].Text.Trim();
                if (SS1.ActiveSheet.Cells[0, 12].Text.IsNullOrEmpty())
                {
                    strTemp3 = SS1.ActiveSheet.Cells[0, 11].Text.Trim();
                }

                strTemp2 = SS1.ActiveSheet.Cells[1, 10].Text.Trim();
                if (SS1.ActiveSheet.Cells[1, 12].Text.IsNullOrEmpty())
                {
                    strTemp4 = SS1.ActiveSheet.Cells[1, 11].Text.Trim();
                }

                if (strTemp1 != strTemp2)
                {
                    if (string.Compare(strTemp1, strTemp2) < 0)
                    {
                        strTemp1 = strTemp2;
                        strTemp3 = strTemp4;
                    }
                }

                if (SS2.ActiveSheet.Cells[0, 2].Text.IsNullOrEmpty())
                {
                    switch (strTemp1)
                    {
                        case "A":
                            SS2.ActiveSheet.Cells[0, 2].Text = "01";
                            break;
                        case "B":
                            SS2.ActiveSheet.Cells[0, 2].Text = "02";
                            break;
                        case "C":
                            SS2.ActiveSheet.Cells[0, 2].Text = "03";
                            break;
                        case "D-A":
                            SS2.ActiveSheet.Cells[0, 2].Text = "04";
                            break;
                        case "D-B":
                            SS2.ActiveSheet.Cells[0, 2].Text = "05";
                            break;
                        case "D-C":
                            SS2.ActiveSheet.Cells[0, 2].Text = "06";
                            break;
                        case "E":
                            SS2.ActiveSheet.Cells[0, 2].Text = "07";
                            break;
                        default:
                            break;
                    }

                    if (!SS2.ActiveSheet.Cells[0, 2].Text.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[0, 8].Text = "Y";
                    }
                }

                if (SS2.ActiveSheet.Cells[1, 2].Text.IsNullOrEmpty() && strTemp1 == "A")
                {
                    SS2.ActiveSheet.Cells[1, 2].Text = strTemp3;
                    if (!SS2.ActiveSheet.Cells[1, 2].Text.IsNullOrEmpty())
                    {
                        SS2.ActiveSheet.Cells[1, 8].Text = "Y";
                    }
                }
            }

        }

        /// <summary>
        /// 검사(방사선) 코드명칭을 읽음
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        string fn_Read_ExCode_Name(string argCode)
        {
            string rtnVal = "";

            if (argCode.IsNullOrEmpty())
            {
                rtnVal = "";
                return rtnVal;
            }

            rtnVal = hicExcodeService.GetHNmaebyCode(argCode.Trim());

            return rtnVal;
        }
    }
}
