using ComBase;
using ComBase.Controls;
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
/// File Name       : frmHcPanOpinionAfterCounselMgmt.cs
/// Description     : 유소견자(D1,D2,DN) 사후관리 상담대장
/// Author          : 이상훈
/// Create Date     : 2019-10-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm유소견자사후관리상담대장.frm(Frm유소견자사후관리상담대장)" />
namespace ComHpcLibB
{
    public partial class frmHcPanOpinionAfterCounselMgmt : Form
    {
        HicSpcPanjengJepsuService hicSpcPanjengJepsuService = null;
        HicResSahusangdamService hicResSahusangdamService = null;
        HicJepsuPatientService hicJepsuPatientService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJepsuService hicJepsuService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        ComFunc cf = new ComFunc();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public frmHcPanOpinionAfterCounselMgmt()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            hicSpcPanjengJepsuService = new HicSpcPanjengJepsuService();
            hicResSahusangdamService = new HicResSahusangdamService();
            hicJepsuPatientService = new HicJepsuPatientService();
            comHpcLibBService = new ComHpcLibBService();
            hicJepsuService = new HicJepsuService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.SS1.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            //this.txtLtdCode.Click += new EventHandler(eTxtClick);
            //this.txtLtdCode.LostFocus += new EventHandler(eLostFocus);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-90).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;
            txtLtdCode.Text = "";
            txtWrtNo.Text = "";

            cboPanjeng.Items.Clear();
            cboPanjeng.Items.Add("전체");
            cboPanjeng.Items.Add("D1");
            cboPanjeng.Items.Add("D2");
            cboPanjeng.Items.Add("DN");
            cboPanjeng.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
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
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
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
                string[] strGel = new string[6];

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strGel[0] = "┌──┬────────┬────────┬────────┬────────┐" + "\n";
                strGel[1] = "│결│ 담  당 │ 계  장 │ 과  장 │ 부  장 │" + "\n";
                strGel[2] = "│  ├────────┼────────┼────────┼────────┤" + "\n";
                strGel[3] = "│  │        │        │        │        │" + "\n";
                strGel[4] = "│재│        │        │        │        │" + "\n";
                strGel[5] = "└──┴────────┴────────┴────────┴────────┘" + "\n";

                strTitle = "유소견자(D1,D2,DN) 사후관리 상담대장";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("맑은 고딕", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                for (int i = 0; i < 6; i++)
                {
                    strHeader += VB.Space(70) + strGel[i];
                }                
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nREAD2 = 0;
                int nRow = 0;
                long nWRTNO = 0;
                string strPanjeng = "";
                string strSogen = "";
                string strJepDate = "";
                string strDate2 = "";
                string strOK = "";
                string strDrno = "";
                string strTongDate = "";
                string strTongDate2 = "";
                string strTongOK = "";

                List<string> lstInstr = new List<string>();

                string strFrDate = "";
                string strToDate = "";
                long nFWrtNo = 0;
                long nLtdCode = 0;
                string strSort = "";

                Cursor.Current = Cursors.WaitCursor;
                btnSearch.Enabled = false;
                btnPrint.Enabled = false;

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;

                if (txtWrtNo.Text.Trim() != "")
                {
                    nFWrtNo = long.Parse(txtWrtNo.Text);
                }
                if (txtLtdCode.Text.Trim() != "")
                {
                    nLtdCode = long.Parse(VB.Pstr(txtLtdCode.Text, ".", 1));
                }

                if (rdoJob1.Checked == true)
                {
                    strSort = "1";
                }
                else if (rdoJob2.Checked == true)
                {
                    strSort = "2";
                }

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 30;

                //미상담 접수번호를 찾음
                if (rdoJob1.Checked == true)
                {
                    List<HIC_SPC_PANJENG_JEPSU> list = hicSpcPanjengJepsuService.GetItembyJepDateWrtnoLtdCode(strFrDate, strToDate, nFWrtNo, nLtdCode);

                    nREAD = list.Count;
                    progressBar1.Maximum = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        //상담내역이 있는지 점검
                        if (hicResSahusangdamService.GetCountbyWrtNo(list[i].WRTNO) == 0)
                        {
                            strPanjeng = "";
                            if (list[i].D1 > 0) strPanjeng += "D1,";
                            if (list[i].D2 > 0) strPanjeng += "D2,";
                            if (list[i].DN > 0) strPanjeng += "DN,";
                            if (strPanjeng != "") strPanjeng = VB.Left(strPanjeng, strPanjeng.Length - 1);
                            if (strPanjeng != "")
                            {
                                lstInstr.Add(list[i].WRTNO.ToString());
                            }
                        }
                        progressBar1.Value = i + 1;
                    }

                    if (lstInstr.Count == 0)
                    {
                        MessageBox.Show("미상담 자료가 없습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        btnSearch.Enabled = true;
                        return;
                    }
                }

                //미상담 자료를 검색함
                if (rdoJob1.Checked == true)
                {
                    List<HIC_JEPSU_PATIENT> list2 = hicJepsuPatientService.GetItembyWrtNoLtdCode(lstInstr, nFWrtNo, nLtdCode, strSort);

                    nREAD = list2.Count;
                    nRow = 0;
                    SS1.ActiveSheet.RowCount = nREAD;
                    SS1_Sheet1.Rows.Get(-1).Height = 24;

                    progressBar1.Maximum = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        nWRTNO = list2[i].WRTNO;
                        strTongOK = "";

                        List<COMHPC> listDetail1 = comHpcLibBService.GetItembySpcPanjengJepsuPatient(nWRTNO);

                        strSogen = "";
                        for (int j = 0; j < listDetail1.Count; j++)
                        {
                            if (VB.InStr(strSogen, listDetail1[j].SOGENREMARK) == 0)
                            {
                                strSogen += listDetail1[j].SOGENREMARK + ",";
                            }
                        }

                        if (strSogen != "")
                        {
                            strSogen = VB.Left(strSogen, strSogen.Length - 1);
                        }

                        //판정결과를 읽음
                        COMHPC listDetail2 = comHpcLibBService.GetItembySpcPanjengJepsu(nWRTNO);
                        if (!listDetail2.IsNullOrEmpty())
                        {
                            strPanjeng = "";
                            if (listDetail2.D1 > 0) strPanjeng = strPanjeng + "D1,";
                            if (listDetail2.D2 > 0) strPanjeng = strPanjeng + "D2,";
                            if (listDetail2.DN > 0) strPanjeng = strPanjeng + "DN,";
                            if (strPanjeng != "") strPanjeng = VB.Left(strPanjeng, strPanjeng.Length - 1);

                            //2차 검진을 하였으면 접수일자를 2차 접수일자로 변경
                            strJepDate = list2[i].JEPDATE.ToString();
                            strDate2 = fn_GET_Second_Date(nWRTNO);
                            strTongDate = fn_GET_Second_TongDate(nWRTNO);

                            if (string.Compare(strDate2, strJepDate) > 0)
                            {
                                strJepDate = strDate2;
                                strTongOK = "OK";
                            }


                            strOK = "";
                            if (cboPanjeng.Text == "전체")
                            {
                                strOK = "OK";
                            }
                            else if (cboPanjeng.Text == "D1")
                            {
                                if (VB.InStr(strPanjeng, "D1") > 0)
                                {
                                    strOK = "OK";
                                }
                            }
                            else if (cboPanjeng.Text == "D2")
                            {
                                if (VB.InStr(strPanjeng, "D2") > 0)
                                {
                                    strOK = "OK";
                                }
                            }
                            else if (cboPanjeng.Text == "DN")
                            {
                                if (VB.InStr(strPanjeng, "DN") > 0)
                                {
                                    strOK = "OK";
                                }
                            }

                            if (strOK == "OK")
                            {
                                nRow += 1;

                                SS1.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_Ltd_Name(listDetail1[0].LTDCODE.ToString());
                                SS1.ActiveSheet.Cells[nRow - 1, 1].Text = listDetail1[0].SNAME;
                                SS1.ActiveSheet.Cells[nRow - 1, 2].Text = listDetail1[0].AGE.ToString() + "/" + listDetail1[0].SEX;
                                SS1.ActiveSheet.Cells[nRow - 1, 3].Text = nWRTNO.ToString();
                                SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list2[i].JUMIN;
                                SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_GjJong_Name(listDetail1[0].GJJONG);
                                SS1.ActiveSheet.Cells[nRow - 1, 6].Text = "";
                                SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strJepDate;
                                if (strTongDate.IsNullOrEmpty())
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Replace(list2[i].TONGBODATE,"-","");
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = strTongDate;
                                }

                                if (strTongOK == "OK")
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 8].Text = strTongDate;
                                }


                                if (listDetail1[0].HPHONE != "")
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = listDetail1[0].HPHONE;
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[nRow - 1, 9].Text = listDetail1[0].TEL;
                                }

                                SS1.ActiveSheet.Cells[nRow - 1, 10].Text = strSogen;
                                SS1.ActiveSheet.Cells[nRow - 1, 11].Text = strPanjeng;
                                SS1.ActiveSheet.Cells[nRow - 1, 12].Text = hb.READ_License_DrName(listDetail1[0].PANJENGDRNO).Replace(" ", "");
                                
                            }
                        }
                        progressBar1.Value = i + 1;
                    }

                    for (int i = 0; i < list2.Count; i++)
                    {
                        Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 9);
                        Size size1 = SS1.ActiveSheet.GetPreferredCellSize(i, 14);

                        if (size.Height >= size1.Height)
                        {
                            SS1.ActiveSheet.Rows[i].Height = size.Height;
                        }
                        else
                        {
                            SS1.ActiveSheet.Rows[i].Height = size1.Height;
                        }
                    }
                }
                else
                {
                    //2차 검진을 하였으면 접수일자를 2차 접수일자로 변경
                    List<COMHPC> list3 = comHpcLibBService.GetItembyJepDate(strFrDate, strToDate, nFWrtNo, nLtdCode, strSort);

                    nREAD = list3.Count;
                    nRow = 0;
                    SS1.ActiveSheet.RowCount = nREAD;
                    SS1_Sheet1.Rows.Get(-1).Height = 24;

                    progressBar1.Maximum = nREAD;
                    for (int i = 0; i < nREAD; i++)
                    {
                        nWRTNO = list3[i].WRTNO;

                        strJepDate = list3[i].JEPDATE;
                        strDate2 = fn_GET_Second_Date(nWRTNO);
                        if (string.Compare(strDate2, strJepDate) > 0)
                        {
                            if (string.Compare(strDate2, list3[i].SDATE.ToString()) < 0)
                            {
                                strJepDate = strDate2;
                            }
                        }

                        strTongDate = fn_GET_Second_TongDate(nWRTNO);

                        strOK = "";
                        strPanjeng = list3[i].PANJENGGBN;

                        if (cboPanjeng.Text == "전체" || cboPanjeng.Text == "")
                        {
                            strOK = "OK";
                        }
                        else if (cboPanjeng.Text == "D1")
                        {
                            if (VB.InStr(strPanjeng, "D1") > 0)
                            {
                                strOK = "OK";
                            }
                        }
                        else if (cboPanjeng.Text == "D2")
                        {
                            if (VB.InStr(strPanjeng, "D2") > 0)
                            {
                                strOK = "OK";
                            }
                        }
                        else if (cboPanjeng.Text == "DN")
                        {
                            if (VB.InStr(strPanjeng, "DN") > 0)
                            {
                                strOK = "OK";
                            }
                        }

                        if (strOK == "OK")
                        {
                            nRow += 1;
                            
                            strDrno = list3[i].PANJENGDRNO.ToString();

                            SS1.ActiveSheet.Cells[nRow - 1, 0].Text = hb.READ_Ltd_Name(list3[i].LTDCODE.ToString());
                            SS1.ActiveSheet.Cells[nRow - 1, 1].Text = list3[i].SNAME;
                            SS1.ActiveSheet.Cells[nRow - 1, 2].Text = list3[i].AGE.ToString() + "/" + list3[i].SEX;
                            SS1.ActiveSheet.Cells[nRow - 1, 3].Text = nWRTNO.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 4].Text = list3[i].JUMIN;
                            SS1.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_HIC_GjJong_Name(list3[i].GJJONG);
                            SS1.ActiveSheet.Cells[nRow - 1, 6].Text = list3[i].SDATE.ToString();
                            SS1.ActiveSheet.Cells[nRow - 1, 7].Text = strJepDate;
                            if (strTongDate.IsNullOrEmpty())
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 8].Text = VB.Left(list3[i].TONGBODATE.ToString(), 10);
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 8].Text = strTongDate;
                            }

                            if (list3[i].HPHONE != "" && list3[i].HPHONE != null)
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = list3[i].HPHONE;
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 9].Text = list3[i].TEL;
                            }

                            SS1.ActiveSheet.Cells[nRow - 1, 10].Text = list3[i].SOGEN;
                            SS1.ActiveSheet.Cells[nRow - 1, 11].Text = list3[i].PANJENGGBN; 
                            if (strDrno != "")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 12].Text = hb.READ_License_DrName(long.Parse(strDrno)).Replace(" ", "");
                            }

                            if (list3[i].GBN == "1")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 13].Text = "내원";
                            }
                            else if (list3[i].GBN == "2")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 13].Text = "전화";
                            }
                            else if (list3[i].GBN == "3")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 13].Text = "서면";
                            }
                            else if (list3[i].GBN == "4")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 13].Text = "제외";
                            }
                            else
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 13].Text = "";
                            }

                            //2017-08-23 이상규계장 요청으로 상담의사에서 통보자로 변경함
                            SS1.ActiveSheet.Cells[i, 14].Text = "";
                            SS1.ActiveSheet.Cells[nRow - 1, 14].Text = cf.Read_SabunName(clsDB.DbCon, list3[i].SABUN);
                            if (SS1.ActiveSheet.Cells[nRow - 1, 14].Text.Trim() == "")
                            {
                                SS1.ActiveSheet.Cells[nRow - 1, 14].Text = hb.READ_License_DrName(long.Parse(strDrno)).Replace(" ", "");
                            }

                            SS1.ActiveSheet.Cells[nRow - 1, 15].Text = list3[i].REMARK.To<string>("");
                        }
                        progressBar1.Value = i + 1;
                    }

                    for (int i = 0; i < list3.Count; i++)
                    {
                        Size size = SS1.ActiveSheet.GetPreferredCellSize(i, 9);
                        Size size1 = SS1.ActiveSheet.GetPreferredCellSize(i, 14);

                        if (size.Height >= size1.Height)
                        {
                            SS1.ActiveSheet.Rows[i].Height = size.Height;
                        }
                        else
                        {
                            SS1.ActiveSheet.Rows[i].Height = size1.Height;
                        }
                    }
                }
                SS1.ActiveSheet.RowCount = nRow;

                Cursor.Current = Cursors.Default;
                btnSearch.Enabled = true;
                btnPrint.Enabled = true;
            }
        }

        string fn_GET_Second_Date(long argWrtNo)
        {
            string rtnVal = "";
            long nPano = 0;
            string strGjYear = "";

            HIC_JEPSU list = hicJepsuService.GetPaNoGjYearbyWrtNo(argWrtNo);

            nPano = list.PANO;
            strGjYear = list.GJYEAR;

            //2차 접수일자를 읽음
            rtnVal = hicJepsuService.GetJepDatebyPanoGjYear(nPano, strGjYear);

            return rtnVal;
        }

        string fn_GET_Second_TongDate(long argWrtNo)
        {
            string rtnVal = "";
            long nPano = 0;
            string strGjYear = "";

            HIC_JEPSU list = hicJepsuService.GetPaNoGjYearbyWrtNo(argWrtNo);

            nPano = list.PANO;
            strGjYear = list.GJYEAR;

            //2차 통보일자를 읽음
            rtnVal = hicJepsuService.GetTongDatebyPanoGjYear(nPano, strGjYear);

            return rtnVal;
        }

        

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == SS1)
            {
                if (e.ColumnHeader != true && e.RowHeader != true)
                {
                    //유소견자 상담등록창연계
                    long nWRTNO = SS1.ActiveSheet.Cells[e.Row, 3].Text.To<long>(0);
                    frmHcPanOpinionCounselReg frm = new frmHcPanOpinionCounselReg(nWRTNO);
                    frm.ShowDialog();
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

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == btnLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (btnLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }
    }
}
