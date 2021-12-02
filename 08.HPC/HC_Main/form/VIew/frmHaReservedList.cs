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
/// Class Name      : HC_Main
/// File Name       : frmHaReservedList.cs
/// Description     : 예약자 조회
/// Author          : 이상훈
/// Create Date     : 2019-10-07
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HaMain65.frm(FrmReservedList)" />

namespace HC_Main
{
    public partial class frmHaReservedList : Form
    {
        HeaJepsuPatientService heaJepsuPatientService = null;
        HicResultService hicResultService = null;
        ComHpcLibBService comHpcLibBService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        frmHcLtdHelp FrmHcLtdHelp = null;
        HIC_LTD LtdHelpItem = null;

        public frmHaReservedList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        void SetEvent()
        {
            heaJepsuPatientService = new HeaJepsuPatientService();
            hicResultService = new HicResultService();
            comHpcLibBService = new ComHpcLibBService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eKeyPress);
            this.txtLtdCode.LostFocus += new EventHandler(eLostFocus);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);

            sp.Spread_All_Clear(SS1);

            txtLtdCode.Text = "";

            dtpYDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(1).ToShortDateString();
            SS1.ActiveSheet.Rows.Get(-1).Height = 24;
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
                frmHcLtdHelp frm = new frmHcLtdHelp();
                frm.rSetGstrValue += new frmHcLtdHelp.SetGstrValue(LtdCd_value);
                frm.ShowDialog();
                frm.rSetGstrValue -= new frmHcLtdHelp.SetGstrValue(LtdCd_value);

                if (!LtdHelpItem.IsNullOrEmpty())
                {
                    txtLtdCode.Text = LtdHelpItem.CODE.ToString() + "." + LtdHelpItem.SANGHO;
                }
                else
                {
                    txtLtdCode.Text = "";
                }
            }
            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                int[] nCNT = new int[2];
                string strJumin = "";
                string strXray = "";
                string strXName = "";
                string strSDate = "";
                string strGbSts = "";
                string strLtdCode = "";
                string strSort = "";

                sp.Spread_All_Clear(SS1);
                SS1.ActiveSheet.RowCount = 30;

                strSDate = dtpYDate.Text;
                if (chkAll.Checked == false)
                {
                    strGbSts = "1";
                }
                else
                {
                    strGbSts = "";
                }

                if (rdoSort1.Checked == true)
                {
                    strSort = "1";
                }
                else if (rdoSort2.Checked == true)
                {
                    strSort = "2";
                }
                else if (rdoSort3.Checked == true)
                {
                    strSort = "3";
                }

                //자료를 SELECT
                List<HEA_JEPSU_PATIENT> list = heaJepsuPatientService.GetItembySDateLtdCode(strSDate, strGbSts, strLtdCode, strSort);

                nREAD = list.Count;
                nCNT[0] = 0;
                nCNT[1] = 0;
                if (nREAD > 0)
                {
                    for (int i = 0; i < nREAD; i++)
                    {
                        strJumin = clsAES.DeAES(list[i].JUMIN2);
                        strXray = "";
                        strXName = "";

                        List<HIC_RESULT> list2 = hicResultService.GetExcodebyExCode(list[i].WRTNO);

                        for (int j = 0; j < list2.Count; j++)
                        {
                            switch (list2[j].EXCODE.Trim())
                            {
                                case "TX53":
                                    strXName = "L-Spine,AP,Lat/";
                                    strXray = "OK";
                                    break;
                                case "TX54":
                                    strXName = "L-Spine,Series(4)/";
                                    strXray = "OK";
                                    break;
                                case "TX05":
                                    strXName = "MRI Spine(L),Non Contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX57":
                                    strXName = "CT Brain,Contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX58":
                                    strXName = "CT Chest,Contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX59":
                                    strXName = "CT Abdomen,3중 (Contr) OPD/";
                                    strXray = "OK";
                                    break;
                                case "TX60":
                                    strXName = "CT,Chest(HR)/";
                                    strXray = "OK";
                                    break;
                                case "TX63":
                                    strXName = "MRI Knee,Non Contrast(L)/";
                                    strXray = "OK";
                                    break;
                                case "TX65":
                                    strXName = "Spine(C)3D,non contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX67":
                                    strXName = "CT CTA Coronary  (Contr)with Indenol/";
                                    strXray = "OK";
                                    break;
                                case "TX69":
                                    strXName = "CT Neck 3D,Non Contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX75":
                                    strXName = "CT Brain,Non Contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX76":
                                    strXName = "CT Coronary -Calcium Scoring/";
                                    strXray = "OK";
                                    break;
                                case "TX80":
                                    strXName = "CT Spine(L)3D,non contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX86":
                                    strXName = "MRI Add Contrast,Brain/";
                                    strXray = "OK";
                                    break;
                                case "TX88":
                                    strXName = "Chest C-T(Low Dose)/";
                                    strXray = "OK";
                                    break;
                                case "TX93":
                                    strXName = "CT Abdomen,Non Contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX96":
                                    strXName = "MRI Brain,Willi`s Angio,Non Contrast/";
                                    strXray = "OK";
                                    break;
                                case "TX97":
                                    strXName = "MRI Brain,Willi`s Angio,Contrast/";
                                    strXray = "OK";
                                    break;
                                case "ZE70":
                                    strXName = "C-Spine CT/";
                                    strXray = "OK";
                                    break;
                                default:
                                    break;
                            }
                        }

                        if (chkXray.Checked == false)
                        {
                            strXray = "OK";
                        }

                        if (strXray == "OK")
                        {
                            nRow += 1;
                            if (nRow > SS1.ActiveSheet.RowCount)
                            {
                                SS1.ActiveSheet.RowCount = nRow;

                                SS1.ActiveSheet.Cells[i, 0].Text = list[i].PTNO;
                                SS1.ActiveSheet.Cells[i, 1].Text = list[i].SNAME;
                                SS1.ActiveSheet.Cells[i, 2].Text = list[i].SEX;
                                SS1.ActiveSheet.Cells[i, 3].Text = list[i].AGE.To<string>();

                                COMHPC list3 = comHpcLibBService.GetTypebyWrtNo(list[i].WRTNO);

                                if (list3 != null)
                                {
                                    if (list3.TYPE.Trim() != "")
                                    {
                                        SS1.ActiveSheet.Cells[i, 4].Text = list3.TYPE + "형";
                                    }
                                }
                                SS1.ActiveSheet.Cells[i, 5].Text = hb.READ_Ltd_Name(list[i].LTDCODE.To<string>());
                                if (long.Parse(clsType.User.IdNumber) == 1859)
                                {
                                    SS1.ActiveSheet.Cells[i, 6].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 7);
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[i, 6].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                                }
                                SS1.ActiveSheet.Cells[i, 7].Text = list[i].JEPDATE.ToString();
                                SS1.ActiveSheet.Cells[i, 8].Text = list[i].PANO.ToString();
                                if (list[i].GJJONG.Trim() == "11" || list[i].GJJONG.Trim() == "12")
                                {
                                    SS1.ActiveSheet.Cells[i, 9].Text = "개인";
                                    nCNT[0] += 1;
                                }
                                else
                                {
                                    SS1.ActiveSheet.Cells[i, 9].Text = "단체";
                                    nCNT[1] += 1;
                                }

                                SS1.ActiveSheet.Cells[i, 10].Text = strXName;
                            }
                        }
                    }
                    SS1.ActiveSheet.RowCount = nRow + 1;

                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount, 0].Text = "개  인" + VB.Space(2) + nCNT[0] + "명";
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount, 3].Text = "단  체" + VB.Space(2) + nCNT[1] + "명";
                    SS1.ActiveSheet.Cells[SS1.ActiveSheet.RowCount, 7].Text = "합  계" + VB.Space(2) + nCNT[0] + nCNT[1] + "명";
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

                strTitle = "예 약 자  명 단";
                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("검사예정일:" + dtpYDate.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("출력일자:" + VB.Now().ToString() + "  PAGE:" + "/p", new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);

                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Portrait, PrintType.All, 0, 0, true, false, true, true, false, false, false);

                sp.setSpdPrint(SS1, PrePrint, setMargin, setOption, strHeader, strFooter);
            }
        }

        void eKeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == txtLtdCode)
            {
                if (e.KeyChar == (char)13)
                {
                    if (txtLtdCode.Text.Length > 4)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                }
            }
        }

        void eLostFocus(object sender, EventArgs e)
        {
            //txtLtdCode.Text = txtLtdCode.Text.Trim();

            //if (txtLtdCode.Text.Trim() == "")
            //{
            //    txtLtdCode.Text = "ALL.전체";
            //    return;
            //}

            //txtLtdCode.Text += "." + hb.READ_Ltd_Name(VB.Pstr(txtLtdCode.Text, ".", 1));
        }

        /// <summary>
        /// 거래처코드 찾기 화면 스프레드더블클릭이벤트
        /// </summary>
        /// <param name="item"></param>
        private void LtdCd_value(HIC_LTD item)
        {
            LtdHelpItem = item;
        }
    }
}
