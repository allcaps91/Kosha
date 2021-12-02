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
/// File Name       : frmHcResultInputCheckList.cs
/// Description     : 검진결과 입력 Check List
/// Author          : 이상훈
/// Create Date     : 2020-06-12
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "HcMain46(FrmResultCheckList.frm)" />
namespace ComHpcLibB
{
    public partial class frmHcResultInputCheckList : Form
    {
        HicJepsuPatientService hicJepsuPatientService = null;
        HicResultService hicResultService = null;
        HicExjongService hicExjongService = null;
        HicResDentalService hicResDentalService = null;
        HicCancerNewService hicCancerNewService = null;
        HicSchoolNewService hicSchoolNewService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicJinGbnService hicJinGbnService = null;
        HicJepsuService hicJepsuService = null;
        HicResBohum2Service hicResBohum2Service = null;
        HicJepsuPatientJinGbdService hicJepsuPatientJinGbdService = null;
        HicIeMunjinNewService hicIeMunjinNewService = null;
        HicSunapdtlService hicSunapdtlService = null;

        frmHcLtdHelp FrmHcLtdHelp = null;

        HIC_LTD LtdHelpItem = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        ComFunc cf = new ComFunc();

        bool boolSort = false;

        long FnWRTNO;

        public frmHcResultInputCheckList()
        {
            InitializeComponent();
            SetEvent();
            SetControl();
        }

        public frmHcResultInputCheckList(long nWrtno)
        {
            InitializeComponent();
            SetEvent();
            SetControl();

            FnWRTNO = nWrtno;
        }

        void SetEvent()
        {
            hicJepsuPatientService = new HicJepsuPatientService();
            hicResultService = new HicResultService();
            hicExjongService = new HicExjongService();
            hicResDentalService = new HicResDentalService();
            hicCancerNewService = new HicCancerNewService();
            hicSchoolNewService = new HicSchoolNewService();
            comHpcLibBService = new ComHpcLibBService();
            hicJinGbnService = new HicJinGbnService();
            hicJepsuService = new HicJepsuService();
            hicResBohum2Service = new HicResBohum2Service();
            hicJepsuPatientJinGbdService = new HicJepsuPatientJinGbdService();
            hicIeMunjinNewService = new HicIeMunjinNewService();
            hicSunapdtlService = new HicSunapdtlService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnLtdCode.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.txtWrtNo.KeyPress += new KeyPressEventHandler(eTxtKeyPress);
            this.txtLtdCode.KeyPress += new KeyPressEventHandler(eTxtKeyPress);

            this.ssList.CellDoubleClick += new CellClickEventHandler(eSpdDClick);
            this.ssList.CellClick += new CellClickEventHandler(eSpdClick);
        }

        void SetControl()
        {
            LtdHelpItem = new HIC_LTD();
        }

        void eFormLoad(object sender, EventArgs e)
        {
            this.StartPosition = FormStartPosition.CenterParent;

            txtWrtNo.Text = "";
            txtLtdCode.Text = "";

            sp.Spread_All_Clear(ssList);
            txtSName.Text = "";
            chkResult.Checked = false;
            chkPan0.Checked = false;
            chkPan1.Checked = false;
            rdoSort0.Checked = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpFrDate.Text = DateTime.Parse(clsPublic.GstrSysDate).AddDays(-7).ToShortDateString();
            dtpToDate.Text = clsPublic.GstrSysDate;

            //검진종류SET
            cboJong.Items.Clear();
            cboJong.Items.Add("00.전체");
            hb.ComboJong_AddItem(cboJong);
            cboJong.SelectedIndex = 0;

            cboSchool.Items.Clear();
            cboSchool.Items.Add("*.전체");
            cboSchool.Items.Add("1.초교 1,4학년");
            cboSchool.Items.Add("2.초교 2,3,5,6학년");
            cboSchool.SelectedIndex = 0;

            if (FnWRTNO > 0)
            {
                txtWrtNo.Text = FnWRTNO.ToString();
                eBtnClick(btnSearch, new EventArgs());
            }

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

                ComFunc.ReadSysDate(clsDB.DbCon);

                clsSpread.SpdPrint_Margin setMargin;
                clsSpread.SpdPrint_Option setOption;

                strTitle = "검진결과 통보서 발송대장";

                strHeader = sp.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
                strHeader += sp.setSpdPrint_String("작업기간: " + dtpFrDate.Text +"~" + dtpToDate.Text,  new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("검진종류: " + cboJong.Text, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strHeader += sp.setSpdPrint_String("인쇄시각: " +  clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);
                strFooter = sp.setSpdPrint_String(null, null, clsSpread.enmSpdHAlign.Center, true, true);
                setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 40, 10);
                setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, false, 0.7f);
                sp.setSpdPrint(ssList, PrePrint, setMargin, setOption, strHeader, strFooter);
            }

            else if (sender == btnSearch)
            {
                int nREAD = 0;
                int nRow = 0;
                long nWRTNO = 0;
                //int nCNT = 0;
                string strGbSTS = "";
                int nCol = 0;

                //bool bGbAm;         //암검사 여부
                //bool bGbAmMunjin = false;  //암검사 문진입력 여부
                bool bDentOnly = false;  //구강검진만 실시한 경우

                string strMunjin = "";
                string strSangDam = "";
                bool[] bGbPrt = new bool[6];

                string strJong = "";
                string strGjYear = "";
                string strTable = "";
                string strUcode = "";
                string strUCodes = "";
                string strChasu = "";
                string strPan = "";
                string strPanDate = "";
                string strOK = "";
                long nDrNO = 0;
                string strJumin = "";
                string strSchoolValue = "";
                string strPANO = "";
                string strAmChk = "";
                string strADDPAN = "";

                string strFrDate = "";
                string strToDate = "";
                string strResult = "";
                string strPart = "";
                string strGjJong = "";
                string strSchool = "";
                string strSName = "";
                long nLtdCode = 0;
                string strHea = "";
                string strSort = "";

                string[] strExCodes = { "ZD00", "ZD99" };

                Cursor.Current = Cursors.WaitCursor;

                COMHPC listCom = null;
                HIC_RES_DENTAL listDenetal = null;
                List<HIC_RES_DENTAL> listComDental = null;

                sp.Spread_All_Clear(ssList);

                strFrDate = dtpFrDate.Text;
                strToDate = dtpToDate.Text;
                if (chkResult.Checked == true)
                {
                    strResult = "Y";
                }
                if (rdoPart1.Checked == true)
                {
                    strPart = "1";
                }
                else if (rdoPart2.Checked == true)
                {
                    strPart = "2";
                }

                if (VB.Left(cboJong.Text, 2) != "00")
                {
                    strGjJong = VB.Left(cboJong.Text, 2);
                }

                strSchool = VB.Left(cboSchool.Text, 1);
                strSName = txtSName.Text;
                if (!txtLtdCode.Text.IsNullOrEmpty())
                {
                    nLtdCode = VB.Pstr(txtLtdCode.Text, ".", 1).To<long>();
                }
                else
                {
                    nLtdCode = 0;
                }

                if (chkHea.Checked == true)
                {
                    strHea = "Y";
                }
                else
                {
                    strHea = "";
                }

                if (rdoSort0.Checked == true)
                {
                    strSort = "0";
                }
                else
                {
                    strSort = "1";
                }

                sp.Spread_All_Clear(ssList);
                if (!txtWrtNo.Text.IsNullOrEmpty())
                {
                    nWRTNO = long.Parse(txtWrtNo.Text);
                }

                if (chkUcodes.Checked == true && strGjJong == "11")
                {
                    strUcode = "Y";
                }
                else
                {
                    strUcode = "";
                }

                //자료를 Select
                List<HIC_JEPSU_PATIENT> list = hicJepsuPatientService.GetItembyWrtNoJepDateSNameLtdCode(nWRTNO, strFrDate, strToDate, strResult, strPart, strGjJong, strSchool, strSName, nLtdCode, strHea, strSort, strUcode);

                nREAD = list.Count;
                ssList.ActiveSheet.RowCount = nREAD;
                this.ssList_Sheet1.Rows.Get(-1).Height = 36;
                for (int i = 0; i < nREAD; i++)
                {
                    progressBar1.Maximum = nREAD;

                    nDrNO = 0;
                    strPanDate = "";
                    strPan = "";
                    strOK = "";
                    strTable = "";
                    nWRTNO = list[i].WRTNO;
                    strGbSTS = list[i].GBSTS;   //검사상태
                    strJong = list[i].GJJONG;   //검진종류
                    strUCodes = list[i].UCODES; //유해인자
                    strChasu = list[i].GJCHASU;  //검진차수
                    strADDPAN = list[i].GBADDPAN; //추가판정

                    //구강만 하였는지 점검
                    bDentOnly = false;
                    if (strJong != "56" && list[i].GBDENTAL == "Y")
                    {
                        if (hicResultService.GetCountbyWrtNoNotIn(nWRTNO, strExCodes) == 0)
                        {
                            bDentOnly = true;
                        }
                    }

                    //검진종류 읽기
                    HIC_EXJONG list2 = hicExjongService.GetItembyCode(strJong);
                    strMunjin = list2.GBMUNJIN;
                    strSangDam = list2.GBSANGDAM;

                    if (list2.GBPRT1 == "Y")
                    {
                        bGbPrt[0] = true;
                    }
                    else
                    {
                        bGbPrt[0] = false;
                    }

                    if (list2.GBPRT2 == "Y")
                    {
                        bGbPrt[1] = true;
                    }
                    else
                    {
                        bGbPrt[1] = false;
                    }

                    if (list2.GBPRT3 == "Y")
                    {
                        bGbPrt[2] = true;
                    }
                    else
                    {
                        bGbPrt[2] = false;
                    }

                    if (list2.GBPRT4 == "Y")
                    {
                        bGbPrt[3] = true;
                    }
                    else
                    {
                        bGbPrt[3] = false;
                    }

                    if (list2.GBPRT5 == "Y")
                    {
                        bGbPrt[4] = true;
                    }
                    else
                    {
                        bGbPrt[4] = false;
                    }

                    if (list2.GBPRT6 == "Y")
                    {
                        bGbPrt[5] = true;
                    }
                    else
                    {
                        bGbPrt[5] = false;
                    }

                    if (bDentOnly == true)
                    {
                        HIC_RES_DENTAL list3 = hicResDentalService.GetItemByWrtno(nWRTNO);
                        listDenetal = list3;
                        strPan = "";
                        strOK = "";
                        if (!list3.IsNullOrEmpty())
                        {
                            if (list3.PANJENGDRNO > 0)
                            {
                                strPan = "OK";
                            }
                            if (chkPan1.Checked == true && strPan.IsNullOrEmpty())
                            {
                                strOK = "NO";
                            }
                            if (strPan == "OK")
                            {
                                if (chkPan0.Checked == true)
                                {
                                    strOK = "NO";
                                }
                            }
                            else
                            {
                                if (chkPan1.Checked == true)
                                {
                                    strOK = "NO";
                                }
                            }

                            if (chkPrint0.Checked == true)
                            {
                                if (!list3.TONGBODATE.IsNullOrEmpty())
                                {
                                    strOK = "NO";
                                }
                            }

                            if (chkPrint1.Checked == true)
                            {
                                if (list3.TONGBODATE.IsNullOrEmpty())
                                {
                                    strOK = "NO";
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int j = 0; j <= 5; j++)
                        {
                            if (bGbPrt[j] == true || strJong == "50" || strJong == "51" || strJong == "54" ||strJong == "56" || strJong == "62" || strJong == "69" || strJong == "32")
                            {
                                switch (j)
                                {
                                    case 0:
                                        strTable = "HIC_RES_BOHUM1";
                                        nCol = 18;
                                        break;
                                    case 1:
                                        strTable = "HIC_RES_BOHUM2";
                                        nCol = 18;
                                        break;
                                    case 2:
                                        strTable = "HIC_RES_SPECIAL";
                                        nCol = 20;
                                        break;
                                    case 3:
                                        strTable = "HIC_CANCER_NEW";
                                        nCol = 21;
                                        break;
                                    case 4:
                                        strTable = "HIC_RES_DENTAL";
                                        nCol = 19;
                                        break;
                                    case 5:
                                        strTable = "HIC_SCHOOL_NEW";
                                        nCol = 18;
                                        break;
                                    default:
                                        break;
                                }

                                if (strJong == "54" || strJong == "62" || strJong == "69")
                                {
                                    strTable = "HIC_RES_ETC";
                                    nCol = 18;
                                }
                                else if (strJong == "50" || strJong == "51")
                                {
                                    strTable = "HIC_X_MUNJIN";
                                    nCol = 18;
                                }
                                else if (strJong == "32")
                                {
                                    strTable = "HIC_JIN_GBN";
                                    nCol = 18;
                                }

                                if (j == 3)
                                {
                                    COMHPC list4 = comHpcLibBService.GetCancerNewbyWrtNo(nWRTNO);

                                    listCom = list4;
                                }
                                else if (j == 5)
                                {
                                    COMHPC list4 = comHpcLibBService.GetSchoolNewbyWrtNo(nWRTNO);

                                    listCom = list4;
                                }
                                else if (j == 4)
                                {
                                    COMHPC list4 = comHpcLibBService.GetTablebyWrtNo(strTable, nWRTNO);
                                    listCom = list4;
                                }
                                else
                                {
                                    if (bDentOnly == true)
                                    {
                                        COMHPC list4 = comHpcLibBService.GetResDentalbyWrtNo(nWRTNO);

                                        listCom = list4;
                                    }
                                    else if (strTable == "HIC_JIN_GBN")
                                    {
                                        COMHPC list4 = comHpcLibBService.GetJinGbnbyWrtNo(nWRTNO, strTable);

                                        listCom = list4;
                                    }
                                    else
                                    {
                                        COMHPC list4 = comHpcLibBService.GetTableJoinbyWrtNo(nWRTNO, strTable);

                                        listCom = list4;
                                    }
                                }

                                //판정유무
                                if (j == 3)
                                {
                                    if (!listCom.IsNullOrEmpty())
                                    {
                                        if ((strPan.IsNullOrEmpty() || strPan == "OK") && listCom.GBSTOMACH == "1")   //위암
                                        {
                                            strPan = listCom.NEW_WOMAN32.IsNullOrEmpty() ? "NO" : "OK";
                                        }
                                        if ((strPan.IsNullOrEmpty() || strPan == "OK") && listCom.GBRECTUM == "1")    //대장암
                                        {
                                            strPan = listCom.NEW_WOMAN33.IsNullOrEmpty() ? "NO" : "OK";
                                        }
                                        if ((strPan.IsNullOrEmpty() || strPan == "OK") && listCom.GBLIVER == "1")     //간암
                                        {
                                            strPan = listCom.NEW_WOMAN34.IsNullOrEmpty() ? "NO" : "OK";
                                        }
                                        if ((strPan.IsNullOrEmpty() || strPan == "OK") && listCom.GBBREAST == "1")    //유방암
                                        {
                                            strPan = listCom.NEW_WOMAN35.IsNullOrEmpty() ? "NO" : "OK";
                                        }
                                        if ((strPan.IsNullOrEmpty() || strPan == "OK") && listCom.GBWOMB == "1")      //자궁암
                                        {
                                            strPan = listCom.NEW_WOMAN36.IsNullOrEmpty() ? "NO" : "OK";
                                        }
                                        if ((strPan.IsNullOrEmpty() || strPan == "OK") && listCom.GBLUNG == "1")      //폐암
                                        {
                                            strPan = listCom.NEW_WOMAN37.IsNullOrEmpty() ? "NO" : "OK";
                                        }
                                    }
                                }
                                else
                                {
                                    if (listCom.PANJENGDRNO == 0)
                                    {
                                        strPan = "NO";
                                    }
                                    else
                                    {
                                        strPan = "OK";
                                    }
                                }

                                if (j == 3)
                                {
                                    if (strPan.IsNullOrEmpty())
                                    {
                                        List<HIC_RES_DENTAL> listDental = hicResDentalService.GetItemsbyWrtNo(nWRTNO);

                                        listComDental = listDental;

                                        if (chkPan1.Checked == true)
                                        {
                                            strOK = "NO";
                                        }
                                    }
                                }

                                if (strPan != "NO")
                                {
                                    if (chkPan0.Checked == true)
                                    {
                                        strOK = "NO";
                                    }
                                }
                                else
                                {
                                    if (chkPan1.Checked == true)
                                    {
                                        strOK = "NO";
                                    }
                                }

                                if (chkPrint0.Checked == true)
                                {
                                    if (strJong == "21" || strJong == "62") //채용검진,혈액종검
                                    {
                                        if (listCom.PRTSABUN > 0)
                                        {
                                            strOK = "NO";
                                        }
                                    }
                                    else if (strJong == "32")
                                    {
                                        if (!listCom.GBPRINT.IsNullOrEmpty())
                                        {
                                            strOK = "NO";
                                        }
                                    }
                                    else if (strJong != "56")
                                    {
                                        if (!listCom.IsNullOrEmpty())
                                        {
                                            if (!listCom.TONGBODATE.IsNullOrEmpty())
                                            {
                                                strOK = "NO";
                                            }
                                        }
                                    }
                                }

                                if (chkPrint1.Checked == true)
                                {
                                    if (strJong == "21" || strJong == "62") //채용검진,혈액종검
                                    {
                                        if (listCom.PRTSABUN.IsNullOrEmpty())
                                        {
                                            strOK = "NO";
                                        }
                                    }
                                    else if (strJong != "56")
                                    {
                                        if (listCom.TONGBODATE.IsNullOrEmpty())
                                        {
                                            strOK = "NO";
                                        }
                                    }
                                }

                                break;
                            }
                        }
                    }

                    //학생 구강검진만 있는 경우 결과미입력 제외
                    if (strOK != "NO")
                    {
                        if (chkResult.Checked == true)
                        {
                            if (strJong == "56" && list[i].GBDENTAL == "Y")
                            {
                                strOK = "NO";
                            }
                        }
                    }

                    //학생검진 판정/미판정, 출력/미출력
                    strSchoolValue = "";
                    if (strOK != "NO" && strJong == "56")
                    {
                        HIC_SCHOOL_NEW list5 = hicSchoolNewService.GetDPPanDrNoPPanDrNobyWrtNo(nWRTNO);

                        if (!list5.IsNullOrEmpty())
                        {
                            strSchoolValue += list5.DPANDRNO + ",";
                            strSchoolValue += list5.PPANDRNO + ",";
                            strSchoolValue += list5.GBDNTPRT + ",";
                            strSchoolValue += list5.GBPANPRT + ",";

                            //미판정
                            if (chkPan0.Checked == true)
                            {
                                if (list5.DPANDRNO > 0 || list5.PPANDRNO > 0)
                                {
                                    strOK = "NO";
                                }
                            }
                            //판정완료
                            if (chkPan1.Checked == true)
                            {
                                if (list5.DPANDRNO == 0 && list5.PPANDRNO == 0)
                                {
                                    strOK = "NO";
                                }
                            }
                            //미출력
                            if (chkPrint0.Checked == true)
                            {
                                if (list5.GBDNTPRT == "Y" || list5.GBPANPRT == "Y")
                                {
                                    strOK = "NO";
                                }
                            }
                            //출력완료
                            if (chkPrint1.Checked == true)
                            {
                                if (list5.GBDNTPRT != "Y" && list5.GBPANPRT != "Y")
                                {
                                    strOK = "NO";
                                }
                            }
                        }
                    }

                    //(32종 추가조건)
                    if (strJong == "32" && chkPrint0.Checked == true)
                    {
                        if (hicResultService.GetCountbyWrtNo(nWRTNO, null) == 0)
                        {
                            strOK = "NO";
                        }
                    }

                    if (strOK != "NO")
                    {
                        strJumin = clsAES.DeAES(list[i].JUMIN2);

                        nRow += 1;
                        if (nRow > ssList.ActiveSheet.RowCount)
                        {
                            ssList.ActiveSheet.RowCount = nRow;
                        }

                        //기본인적사항
                        ssList.ActiveSheet.Cells[nRow - 1, 0].Text = list[i].JEPDATE;
                        ssList.ActiveSheet.Cells[nRow - 1, 1].Text = list[i].GJJONG + "." + hb.READ_GjJong_Name(list[i].GJJONG);

                        //2018-11-13(검진부장님 요청사항 제외처리)
                        if (VB.Left(strJong, 1) == "1" && VB.Left(strUCodes, 3) != "ZZZ")
                        {
                            if (!strUCodes.IsNullOrEmpty())
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 1].Text = "11.일반+특수";
                            }
                        }
                        else if (VB.Left(strJong, 1) == "4")
                        {
                            if (!strUCodes.IsNullOrEmpty())
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 1].Text = "41.생애+특수";
                            }
                        }
                        ssList.ActiveSheet.Cells[nRow - 1, 2].Text = list[i].WRTNO.To<string>();
                        ssList.ActiveSheet.Cells[nRow - 1, 3].Text = list[i].SNAME;
                        ssList.ActiveSheet.Cells[nRow - 1, 4].Text = list[i].AGE.ToString() + "/" + list[i].SEX;
                        ssList.ActiveSheet.Cells[nRow - 1, 5].Text = hb.READ_Ltd_Name(list[i].LTDCODE.ToString());
                        ssList.ActiveSheet.Cells[nRow - 1, 6].Text = VB.Left(strJumin, 6) + "-" + VB.Mid(strJumin, 7, 1) + "******";
                        if (!hb.READ_Sabun_Name(list[i].JOBSABUN).IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 7].Text = hb.READ_Sabun_Name(list[i].JOBSABUN).Trim();
                        }
                        if (!list[i].SANGDAMDATE.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 8].Text = Convert.ToDateTime(list[i].SANGDAMDATE.ToString()).ToShortDateString();
                        }
                        if (!hb.READ_Sabun_Name(list[i].SANGDAMDRNO.ToString()).IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 9].Text = hb.READ_Sabun_Name(list[i].SANGDAMDRNO.ToString()).Trim(); //list[i].SANGDAMDRNO.ToString();
                        }
                        //접수상태
                        ssList.ActiveSheet.Cells[nRow - 1, 11].Text = hm.READ_JepsuSTS_Name(list[i].GBSTS);

                        strPANO = list[i].PANO.ToString();
                        strGjYear = list[i].GJYEAR;
                        strAmChk = "";

                        if (hicJepsuService.GetCountbyPaNoGjYearGjjong(strPANO, strGjYear, strJong) > 1)
                        {
                            strAmChk = "OK";
                        }

                        //일반문진
                        if (bDentOnly == true)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 13].Text = " ";
                        }
                        else if (list[i].GBMUNJIN1 == "Y")
                        {
                            if (strJong == "31" || strJong == "35")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 13].Text = " ";
                                ssList.ActiveSheet.Cells[nRow - 1, 16].Text = "◎";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 13].Text = "◎";
                                ssList.ActiveSheet.Cells[nRow - 1, 16].Text = " ";
                            }
                        }
                        else if (list[i].GBMUNJIN1 == "N")
                        {
                            if (strJong == "31" || strJong == "35")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 13].Text = " ";
                                ssList.ActiveSheet.Cells[nRow - 1, 16].Text = "X";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 13].Text = "X";
                                ssList.ActiveSheet.Cells[nRow - 1, 16].Text = " ";
                            }
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 13].Text = " ";
                        }

                        if (strAmChk == "OK")
                        {
                            if (strJong == "31" || strJong == "35")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 13].Text = " ";
                                ssList.ActiveSheet.Cells[nRow - 1, 16].Text = "◎";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 13].Text = "◎";
                                ssList.ActiveSheet.Cells[nRow - 1, 16].Text = " ";
                            }
                        }

                        //특수문진
                        if (bDentOnly == true)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = " ";
                        }
                        else if (list[i].GBSPCMUNJIN == "Y")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = "◎";
                        }
                        else if (list[i].GBSPCMUNJIN.IsNullOrEmpty() && !strUCodes.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = "X";
                        }
                        else if (list[i].GBSPCMUNJIN.IsNullOrEmpty() && strUCodes.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = " ";
                        }

                        //특수문진
                        if (bDentOnly == true)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = " ";
                        }
                        else if (list[i].GBSPCMUNJIN == "Y")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = "◎";
                        }
                        else if (!list[i].GBSPCMUNJIN.IsNullOrEmpty() && !strUCodes.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = "X";
                        }
                        else if (list[i].GBSPCMUNJIN.IsNullOrEmpty() && strUCodes.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 14].Text = " ";
                        }

                        //구강문진
                        if (list[i].GBDENTAL == "Y" && list[i].GJJONG != "23")
                        {
                            if (list[i].GBMUNJIN2 == "Y")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 15].Text = "◎";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 15].Text = "Ⅹ";
                            }
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 15].Text = " ";
                        }

                        if (strPan == "OK" && bDentOnly == false)
                        {
                            if (nCol == 0)
                            {
                                ssList_Sheet1.RowHeader.Cells.Get(nRow - 1, 0).Value = "◎";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, nCol].Text = "◎";
                            }
                        }
                        else
                        {
                            if (nCol == 0)
                            {
                                ssList_Sheet1.RowHeader.Cells.Get(nRow - 1, 0).Value = "Ⅹ";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, nCol].Text = "Ⅹ";
                            }
                        }

                        //특수판정표시(일+특 경우 일반판정과 동일하게 표시)
                        if (strPan == "OK" && bDentOnly == false)
                        {
                            if (!strUCodes.IsNullOrEmpty())
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 20].Text = "◎";
                            }
                        }
                        else
                        {
                            if (!strUCodes.IsNullOrEmpty())
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 20].Text = "Ⅹ";
                            }
                        }

                        //구강판정 여부
                        ssList.ActiveSheet.Cells[nRow - 1, 19].Text = "";
                        if (list[i].GBDENTAL == "Y")
                        {
                            HIC_RES_DENTAL list4 = hicResDentalService.GetItemByWrtno(nWRTNO);

                            ssList.ActiveSheet.Cells[nRow - 1, 19].Text = "Ⅹ";

                            if (!list4.IsNullOrEmpty())
                            {
                                if (!hb.READ_License_DrName(list4.PANJENGDRNO).IsNullOrEmpty())
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 19].Text = "◎";
                                }
                            }
                        }
                        else
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 19].Text = " ";
                        }

                        //판정일자,판정의사,통보일자
                        if (strJong == "56")
                        {
                            //학생구강검진
                            if (list[i].GBDENTAL == "Y")
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 8].Text = listCom.PANJENGDATE.ToString();
                                ssList.ActiveSheet.Cells[nRow - 1, 9].Text = hb.READ_License_DrName(listCom.PANJENGDRNO).Trim();
                            }
                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = Convert.ToDateTime(listCom.PANJENGDATE.ToString()).ToShortDateString();
                            ssList.ActiveSheet.Cells[nRow - 1, 23].Text = hb.READ_License_DrName(listCom.PANJENGDRNO);

                            if (VB.Pstr(strSchoolValue, ",", 3) == "Y" || VB.Pstr(strSchoolValue, ",", 4) == "Y")
                            {
                                if (!listCom.PRTSABUN.IsNullOrEmpty())
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(listCom.PRTSABUN.ToString()).Trim();
                                }
                                else
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "인쇄함";
                                }
                                if (!listCom.TONGBODATE.IsNullOrEmpty())
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 26].Text = Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                    ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "우편" + "\r\n" + Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                }
                                else
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 26].Text = "통보함";
                                }
                            }
                        }
                        else if (strTable != "HIC_RES_DENTAL")
                        {
                            if (strTable != "HIC_RES_SPECIAL" && strTable != "HIC_CANCER_NEW")
                            {
                                if (!listCom.IsNullOrEmpty())
                                {

                                    if (listCom.PANJENGDRNO.ToString().IsNullOrEmpty())
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                    }
                                    else
                                    {
                                        if (hb.READ_License_DrName(listCom.PANJENGDRNO).IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                        }
                                        else
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = listCom.PANJENGDATE.ToString();
                                        }
                                    }

                                    ssList.ActiveSheet.Cells[nRow - 1, 23].Text = hb.READ_License_DrName(listCom.PANJENGDRNO);
                                    if (bDentOnly == true && strJong != "56")
                                    {
                                        HIC_JEPSU list5 = hicJepsuService.GetPrtSabunbyWrtNo(nWRTNO);
                                        if (!list5.IsNullOrEmpty())
                                        {
                                            if (!list5.PRTSABUN.IsNullOrEmpty())
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(list5.PRTSABUN.ToString()).Trim();
                                            }
                                            else
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "구강검진";
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (!listCom.PRTSABUN.ToString().IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(listCom.PRTSABUN.ToString()).Trim();
                                        }
                                    }

                                    if (!listCom.TONGBODATE.IsNullOrEmpty())
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 26].Text = Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                        ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "우편" + "\r\n" + Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                    }
                                }
                                else if (bDentOnly = true)
                                {
                                    if (!listDenetal.IsNullOrEmpty())
                                    {
                                        if (listDenetal.PANJENGDRNO.ToString().IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                        }
                                        else
                                        {
                                            if (hb.READ_License_DrName(listDenetal.PANJENGDRNO).IsNullOrEmpty())
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                            }
                                            else
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 22].Text = listDenetal.PANJENGDATE.ToString();
                                            }
                                        }
                                        ssList.ActiveSheet.Cells[nRow - 1, 23].Text = hb.READ_License_DrName(listDenetal.PANJENGDRNO);

                                        if (bDentOnly == true && strJong != "56")
                                        {
                                            HIC_JEPSU list5 = hicJepsuService.GetPrtSabunbyWrtNo(nWRTNO);
                                            if (!list5.IsNullOrEmpty())
                                            {
                                                if (!list5.PRTSABUN.IsNullOrEmpty())
                                                {
                                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(list5.PRTSABUN.ToString()).Trim();
                                                }
                                                else
                                                {
                                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "구강검진";
                                                }
                                            }

                                            if (!listDenetal.TONGBODATE.IsNullOrEmpty())
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 26].Text = Convert.ToDateTime(listDenetal.TONGBODATE.ToString()).ToShortDateString();
                                                ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "우편" + "\r\n" + Convert.ToDateTime(listDenetal.TONGBODATE.ToString()).ToShortDateString();
                                            }
                                        }

                                    }
                                }
                                else
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                    ssList.ActiveSheet.Cells[nRow - 1, 23].Text = "";
                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "";
                                    ssList.ActiveSheet.Cells[nRow - 1, 26].Text = "";
                                    ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "";
                                }

                            }
                            else
                            {
                                if (!strUCodes.IsNullOrEmpty())
                                {
                                    if (listCom.PANJENGDRNO.ToString().IsNullOrEmpty())
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                    }
                                    else
                                    {
                                        if (!listCom.PANJENGDATE.IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = listCom.PANJENGDATE.ToString().Trim();
                                        }
                                    }

                                    ssList.ActiveSheet.Cells[nRow - 1, 23].Text = hb.READ_License_DrName(listCom.PANJENGDRNO);

                                    if (listCom.PRTSABUN.ToString().IsNullOrEmpty())
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "";
                                    }
                                    else
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(listCom.PRTSABUN.ToString()).Trim();
                                    }

                                    //ssList.ActiveSheet.Cells[nRow - 1, 26].Text = listCom.TONGBODATE.ToString();
                                    if (!listCom.TONGBODATE.IsNullOrEmpty())
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 26].Text = Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                    }
                                    else
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 26].Text = "";
                                    }


                                    if (!listCom.TONGBODATE.IsNullOrEmpty())
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "우편" + "\r\n" + Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                    }


                                }
                                else
                                {
                                    if (strJong == "21")
                                    {
                                        if (hb.READ_License_DrName(listCom.PANJENGDRNO).IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                        }
                                        else
                                        {
                                            if (!listCom.TONGBODATE.IsNullOrEmpty())
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 22].Text = Convert.ToDateTime(listCom.PANJENGDATE.ToString()).ToShortDateString(); ;
                                            }
                                        }
                                        ssList.ActiveSheet.Cells[nRow - 1, 23].Text = hb.READ_License_DrName(listCom.PANJENGDRNO);
                                        if (!listCom.PRTSABUN.ToString().IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(listCom.PRTSABUN.ToString()).Trim();
                                        }
                                        if (!listCom.PANJENGDATE.IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 26].Text = listCom.PANJENGDATE.ToString();
                                        }
                                        if (!listCom.TONGBODATE.IsNullOrEmpty())
                                        {
                                            ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "우편" + "\r\n" + Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                        }
                                    }
                                    else
                                    {
                                        if (list[i].PANJENGDRNO > 0)
                                        {
                                            if (!list[i].PANJENGDATE.IsNullOrEmpty())
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 22].Text = Convert.ToDateTime(list[i].PANJENGDATE.ToString()).ToShortDateString();
                                            }
                                            ssList.ActiveSheet.Cells[nRow - 1, 23].Text = hb.READ_License_DrName(list[i].PANJENGDRNO);
                                        }
                                        if (!list[i].TONGBODATE.IsNullOrEmpty())
                                        {
                                            HIC_RES_BOHUM2 list6 = hicResBohum2Service.GetItemByWrtno(nWRTNO);

                                            if (!list6.IsNullOrEmpty())
                                            {
                                                if (!list6.PRTSABUN.ToString().IsNullOrEmpty())
                                                {
                                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(list6.PRTSABUN.ToString()).Trim();
                                                }
                                            }
                                            else
                                            {
                                                if (!list[i].PRTSABUN.ToString().IsNullOrEmpty())
                                                {
                                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(list[i].PRTSABUN.ToString()).Trim();
                                                }
                                            }
                                            if (!list[i].PANJENGDATE.IsNullOrEmpty())
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 26].Text = Convert.ToDateTime(list[i].PANJENGDATE.ToString()).ToShortDateString();
                                            }
                                            //2020-11-20(오류로 인한 수정)
                                            //if (!list[i].TONGBODATE.IsNullOrEmpty())
                                            if (!listCom.TONGBODATE.IsNullOrEmpty())
                                            {
                                                ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "우편" + "\r\n" + Convert.ToDateTime(listCom.TONGBODATE.ToString()).ToShortDateString();
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (strJong == "32")
                        {
                            HIC_JEPSU_PATIENT_JIN_GBD list6 = hicJepsuPatientJinGbdService.GetItembyWrtNoGjJong(nWRTNO, "32", "Y");

                            if (!list6.IsNullOrEmpty())
                            {
                                if (!list6.PRTSABUN.ToString().IsNullOrEmpty())
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(list6.PRTSABUN.ToString()).Trim();
                                }
                                if(!list6.TONGBODATE.IsNullOrEmpty())
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 26].Text = list6.TONGBODATE.Trim();
                                }
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 24].Text = "재출력";
                            }
                        }

                        if (!list[i].WEBPRINTSEND.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 26].Text = "전자출력" + "\r\n" + VB.Left(list[i].WEBPRINTSEND, 10);
                            ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "알톡" + "\r\n" + VB.Left(list[i].WEBPRINTSEND, 10);
                        }

                        if (!list[i].WEBPRINTREQ.IsNullOrEmpty() && list[i].WEBPRINTSEND.IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "알톡";
                        }

                        if (strTable == "HIC_CANCER_NEW")
                        {
                            if (strPan == "OK")
                            {
                                if (listCom.GBSTOMACH == "1")   //위암
                                {
                                    nDrNO = listCom.NEW_WOMAN32.To<long>();
                                    strPanDate = listCom.S_PANJENGDATE.To<string>();
                                }
                                if (listCom.GBRECTUM == "1")   //대장암
                                {
                                    nDrNO = listCom.NEW_WOMAN33.To<long>();
                                    strPanDate = listCom.C_PANJENGDATE.To<string>();
                                }
                                if (listCom.GBLIVER == "1")   //간암
                                {
                                    nDrNO = listCom.NEW_WOMAN34.To<long>();
                                    strPanDate = listCom.L_PANJENGDATE.To<string>();
                                }
                                if (listCom.GBBREAST == "1")   //유방암
                                {
                                    nDrNO = listCom.NEW_WOMAN35.To<long>();
                                    strPanDate = listCom.B_PANJENGDATE.To<string>();
                                }
                                if (listCom.GBWOMB == "1")   //자궁암
                                {
                                    nDrNO = listCom.NEW_WOMAN36.To<long>();
                                    strPanDate = listCom.W_PANJENGDATE.To<string>();
                                }
                                if (listCom.GBLUNG == "1")   //폐암
                                {
                                    nDrNO = listCom.NEW_WOMAN37.To<long>();
                                    strPanDate = listCom.L_PANJENGDATE1.To<string>();
                                }

                                ssList.ActiveSheet.Cells[nRow - 1, 22].Text = Convert.ToDateTime(strPanDate).ToShortDateString();
                                //ssList.ActiveSheet.Cells[nRow - 1, 22].Text = strPanDate;
                                ssList.ActiveSheet.Cells[nRow - 1, 23].Text = hb.READ_License_DrName(nDrNO);
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 22].Text = "";
                                ssList.ActiveSheet.Cells[nRow - 1, 23].Text = "";
                            }
                            if (!listCom.IsNullOrEmpty())
                            {
                                if (!listCom.PRTSABUN.ToString().IsNullOrEmpty())
                                {
                                    ssList.ActiveSheet.Cells[nRow - 1, 24].Text = hb.READ_Sabun_Name(listCom.PRTSABUN.ToString()).Trim();
                                }
                                //전자출력표시 오류로 수정
                                if (VB.Left(ssList.ActiveSheet.Cells[nRow - 1, 26].Text, 4) != "전자출력")
                                {
                                    if (!listCom.TONGBODATE.IsNullOrEmpty())
                                    {
                                        ssList.ActiveSheet.Cells[nRow - 1, 26].Text = Convert.ToDateTime(listCom.TONGBODATE.To<string>()).ToShortDateString();
                                    }
                                }
                            }
                        }

                        //학생검진일 경우
                        if (strTable == "HIC_SCHOOL_NEW")
                        {
                            if (listCom.DPANDRNO > 0)
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 19].Text = "◎";
                            }
                            else
                            {
                                ssList.ActiveSheet.Cells[nRow - 1, 19].Text = "Ⅹ";
                            }
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 28].Text = "";
                        if (list[i].IEMUNNO > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 28].Text = "";
                            ssList.ActiveSheet.Cells[nRow - 1, 28].Text = hb.IEMunjin_Name_Display(hicIeMunjinNewService.GetRecvFormbyWrtNo(list[i].IEMUNNO));
                        }

                        ssList.ActiveSheet.Cells[nRow - 1, 29].Text = list[i].WRTNO.To<string>();
                        //(폐암 사후상담대상자 표시)
                        if (hicSunapdtlService.GetCountbyWrtNoCode(nWRTNO, "3170") > 0)
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 2].BackColor = Color.FromArgb(255, 255, 0);
                        }

                        if (hb.READ_CHARTTRANS_PRINT(nWRTNO) != "")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 27].Text = hb.READ_CHARTTRANS_PRINT(nWRTNO);
                        }

                        if (ssList.ActiveSheet.Cells[nRow - 1, 27].Text.Trim().IsNullOrEmpty())
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "우편";
                        }

                        if (strJong == "69" && strADDPAN != "Y")
                        {
                            ssList.ActiveSheet.Cells[nRow - 1, 9].Text = "상담불필요";
                            ssList.ActiveSheet.Cells[nRow - 1, 23].Text = "판정불필요";
                            ssList.ActiveSheet.Cells[nRow - 1, 27].Text = "";
                        }
                    }
                    progressBar1.Value = i + 1;
                }

                ssList.ActiveSheet.RowCount = nRow;
                Cursor.Current = Cursors.Default;
            }
        }

        void eTxtKeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                if (sender == txtLtdCode)
                {
                    if (txtLtdCode.Text.Length >= 2)
                    {
                        eBtnClick(btnLtdCode, new EventArgs());
                    }
                    SendKeys.Send("{TAB}");
                }
                else if (sender == txtWrtNo)
                {
                    txtWrtNo.Text = txtWrtNo.Text.Trim();
                    if (!txtWrtNo.Text.Trim().IsNullOrEmpty())
                    {
                        eBtnClick(btnSearch, new EventArgs());
                    }
                }
            }
        }

        void eSpdClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                if (e.ColumnHeader == true)
                {
                    //SortIndicator 표식 없이 정렬(Sort)
                    clsSpread.gSpdSortRow(ssList, e.Column, ref boolSort, true);
                    return;
                }
            }
        }

        void eSpdDClick(object sender, CellClickEventArgs e)
        {
            if (sender == ssList)
            {
                if (e.Column < 15)
                {
                    return;
                }

                clsPublic.GstrHelpCode = ssList.ActiveSheet.Cells[e.Row, 23].Text.Trim();
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
    }

}
