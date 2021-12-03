using ComBase;
using ComBase.Mvc;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Model;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data;
using ComBase.Controls;
using FarPoint.Win.Spread;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmWaitSeqReg.cs
/// Description     : SONO 예약자 명단
/// Author          : 이상훈
/// Create Date     : 2019-08-23
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm대기순번등록.frm(Frm대기순번등록)" />

namespace HC_Act
{
    public partial class frmWaitSeqReg : Form
    {
        HicCodeService hicCodeService = null;
        BasPcconfigService basPcconfigService = null;
        HicJepsuService hicJepsuService = null;
        ActingCheckService actingCheckService = null;
        HicResultService hicResultService = null;
        HicWaitRoomService hicWaitRoomService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicSangdamNewService hicSangdamNewService = null;
        HicSangdamWaitService hicSangdamWaitService = null;
          
        clsSpread sp = new clsSpread();

        string FstrWaitRemark = "";
        long FWrtNo = 0;
        long FPaNo = 0;

        public frmWaitSeqReg(long WrtNo, long PaNo)
        {
            InitializeComponent();
            FWrtNo = WrtNo;
            FPaNo = PaNo;

            SetEvent();
        }

        void SetEvent()
        {
            hicCodeService = new HicCodeService();
            basPcconfigService = new BasPcconfigService();
            actingCheckService = new ActingCheckService();
            hicResultService = new HicResultService();
            hicJepsuService = new HicJepsuService();
            hicWaitRoomService = new HicWaitRoomService();
            comHpcLibBService = new ComHpcLibBService();
            hicSangdamNewService = new HicSangdamNewService();
            hicSangdamWaitService = new HicSangdamWaitService();

            this.Load += new EventHandler(eFormLoad);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.SS2.CellClick += new CellClickEventHandler(eSpreadClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
        }

        void eSpreadClick(object sender, CellClickEventArgs e)
        {
            FpSpread s = (FpSpread)sender;

            if (sender == this.SS2)
            {
                if (e.Column == 2 || e.Column == 3 || e.Column == 4 || e.Column == 5)
                {
                    if (SS2.ActiveSheet.Cells[e.Row, e.Column].Text == "True")
                    {
                        for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                        {
                            if (i != e.Row)
                            {
                                SS2.ActiveSheet.Cells[i, e.Column].Text = "False";
                            }
                        }
                    }

                    if (SS2.ActiveSheet.Cells[e.Row, e.Column].Text == "True")
                    {
                        for (int i = 2; i < SS2.ActiveSheet.ColumnCount; i++)
                        {
                            if (i != e.Column)
                            {
                                SS2.ActiveSheet.Cells[e.Row, i].Text = "False";
                            }
                        }
                    }
                }
            }
        }

        void fn_Form_Load()
        {
            long nCNT = 0;
            int nREAD = 0;
            int nRow = 0;
            string strRoom = "";
            //string strTemp = "";
            List<string> strTemp = new List<string>();
            string strGbSpc = "";
            string strList = "";
            string strCODE = "";
            string strDrNo = "";

            HIC_JEPSU list = hicJepsuService.GetItembyWrtNo(FWrtNo);

            if (!list.IsNullOrEmpty())
            {
                ssPatInfo.ActiveSheet.Cells[0, 0].Text = FWrtNo.ToString();
                ssPatInfo.ActiveSheet.Cells[0, 1].Text = list.SNAME;
                ssPatInfo.ActiveSheet.Cells[0, 2].Text = list.AGE + "/" + list.SEX;
                ssPatInfo.ActiveSheet.Cells[0, 3].Text = list.LTDCODE.ToString();
                ssPatInfo.ActiveSheet.Cells[0, 4].Text = list.JEPDATE.ToString();
                ssPatInfo.ActiveSheet.Cells[0, 5].Text = list.GJJONG;
                FstrWaitRemark = list.WAITREMARK;
            }

            if (!FstrWaitRemark.IsNullOrEmpty())
            {
                if (VB.Left(FstrWaitRemark, 1) == "Y")
                {
                    chkWait1.Checked = true;
                }

                if (VB.Mid(FstrWaitRemark, 2, 1) == "Y")
                {
                    chkWait2.Checked = true;
                }

                if (FstrWaitRemark.Length >= 3)
                {
                    txtWaitRemark.Text = VB.Right(FstrWaitRemark, FstrWaitRemark.Length - 2);
                }
            }

            ComFunc.ReadSysDate(clsDB.DbCon);

            ACTING_CHECK(FWrtNo, clsPublic.GstrSysDate, FPaNo);

            nRow = 0;
            for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
            {
                strRoom = SS1.ActiveSheet.Cells[i, 2].Text.Trim();
                strTemp.Add(strRoom);
                if (strRoom == "04") strTemp.Clear(); strTemp.Add("05");
                if (strRoom == "06") strTemp.Clear(); strTemp.Add("05"); strTemp.Add("07");
                if (strRoom == "07") strTemp.Clear(); strTemp.Add("08"); strTemp.Add("09");
                if (strRoom == "12") strTemp.Clear(); strTemp.Add("12"); strTemp.Add("13");
                if (strRoom == "15")
                {
                    List<HIC_JEPSU> lst = hicJepsuService.GetUcodesbyPaNo(FPaNo);
                    strGbSpc = "N";
                    nREAD = lst.Count;
                    for (int j = 0; j < nREAD; j++)
                    {
                        if (lst[j].UCODES.Trim() != "")
                        {
                            strGbSpc = "Y";
                            switch (lst[j].GJJONG.Trim())
                            {
                                case "22":
                                case "23":
                                case "24":
                                case "25":
                                case "26":
                                case "29":
                                    strGbSpc = "Y";
                                    break;
                                case "30":
                                case "33":
                                case "49":
                                case "50":
                                case "51":
                                case "69":
                                    strGbSpc = "Y";
                                    break;
                                default:
                                    break;
                            }
                            if (strGbSpc == "Y")
                            {
                                strTemp.Clear(); strTemp.Add("15"); strTemp.Add("17"); strTemp.Add("18"); strTemp.Add("19");
                            }
                            else
                            {
                                strTemp.Clear(); strTemp.Add("15"); strTemp.Add("16"); strTemp.Add("17"); strTemp.Add("18"); strTemp.Add("19");
                            }
                        }
                    }
                }

                List<HIC_WAIT_ROOM> lstWait = hicWaitRoomService.GetRoombyRoomCode(strTemp, clsPublic.GstrSysTime);
                nREAD = lstWait.Count;

                for (int j = 0; j < nREAD; j++)
                {
                    nRow += 1;
                    if (nRow > SS2.ActiveSheet.RowCount)
                    {
                        SS2.ActiveSheet.RowCount = nRow;
                        SS2.ActiveSheet.Cells[nRow, 0].Text = lstWait[j].ROOM.Trim();
                        SS2.ActiveSheet.Cells[nRow, 1].Text = lstWait[j].ROOMNAME.Trim();
                        SS2.ActiveSheet.Cells[nRow, 9].Text = "";

                        if (string.Compare(clsPublic.GstrSysTime, "13:00") < 0)
                        {
                            if (lstWait[j].AMSANG == "Y")
                            {
                                SS2.ActiveSheet.Cells[nRow, 9].Text = "◎";
                            }
                        }
                        else
                        {
                            if (lstWait[j].PMSANG == "Y")
                            {
                                SS2.ActiveSheet.Cells[nRow, 9].Text = "◎";
                            }
                        }
                        //의사 면허번호를 읽음
                        strDrNo = comHpcLibBService.GetLicencebyRoom(lstWait[j].ROOM.Trim());

                        SS2.ActiveSheet.Cells[nRow, 6].Text = "";
                        SS2.ActiveSheet.Cells[nRow, 8].Text = "";

                        //상담완료 인원수를 읽음
                        if (strDrNo != "")
                        {
                            List<HIC_SANGDAM_NEW> lstCnt = hicSangdamNewService.GetPanoCntbyDrNo(strDrNo);

                            SS2.ActiveSheet.Cells[nRow, 6].Text = lstCnt.Count.ToString();

                            //대기인원수를 읽음
                            SS2.ActiveSheet.Cells[nRow, 8].Text = hicSangdamWaitService.GetWaitCountbyRoomCd(lstWait[j].ROOM.Trim()).ToString();
                        }
                    }
                }
            }

            nRow += 4;
            SS2.ActiveSheet.RowCount = nRow;
            SS2.ActiveSheet.Cells[nRow - 3, 0].Text = "30";
            SS2.ActiveSheet.Cells[nRow - 3, 1].Text = "1번:시력.소변";

            SS2.ActiveSheet.Cells[nRow - 3, 0].Text = "31";
            SS2.ActiveSheet.Cells[nRow - 3, 1].Text = "3번:혈압";

            SS2.ActiveSheet.Cells[nRow - 3, 0].Text = "32";
            SS2.ActiveSheet.Cells[nRow - 3, 1].Text = "4번:채혈실";

            SS2.ActiveSheet.Cells[nRow - 3, 0].Text = "33";
            SS2.ActiveSheet.Cells[nRow - 3, 1].Text = "접수창구";

            //기존 설정된 대기순번을 표시함
            lblOldWait.Text = "";

            HIC_SANGDAM_WAIT lstSDWait = hicSangdamWaitService.GetNextRoomGubunByWrtNo(FWrtNo);

            if (lstSDWait != null)
            {
                strList = lstSDWait.GUBUN.Trim() + ",";
                strList += lstSDWait.NEXTROOM.Trim();
                nCNT = VB.L(strList, ",");
                for (int i = 0; i < nCNT; i++)
                {
                    strCODE = VB.Pstr(strList, ",", i).Trim();
                    if (strCODE != "")
                    {
                        switch (strCODE)
                        {
                            case "30":
                                strRoom = "[" + string.Format("#0", i) + "]" + "1번(시력.소변) ";
                                break;
                            case "31":
                                strRoom = "[" + string.Format("#0", i) + "]" + "3번(혈압) ";
                                break;
                            case "32":
                                strRoom = "[" + string.Format("#0", i) + "]" + "4번(채혈실) ";
                                break;
                            case "33":
                                strRoom = "[" + string.Format("#0", i) + "]" + "접수창구제출 ";
                                break;
                            default:
                                strRoom = "[" + string.Format("#0", i) + "]" + hicSangdamWaitService.GetRoomNameByRoomCd(strCODE) + " ";
                                break;
                        }
                        lblOldWait.Text += strRoom;
                        for (int j = 0; j < SS2.ActiveSheet.RowCount; j++)
                        {
                            if (SS2.ActiveSheet.Cells[j, 0].Text == strCODE)
                            {
                                SS2.ActiveSheet.Cells[j, i + 2].Text = "1";
                                break;
                            }
                        }
                    }
                }
            }
        }

        void ACTING_CHECK(long nWrtNo, string sysdate, long Pano)
        {
            int nRead = 0;
            int nRow = 0;
            string strPart = "";
            string strJong = "";
            string strOldHaRoom = "";
            string strList = "";
            bool boolSort = false;

            sp.Spread_All_Clear(SS1);
            SS1.ActiveSheet.RowCount = 20;
            
            string strGB = "";

            if (hicJepsuService.GetJepsuCountbyPaNo(nWrtNo, sysdate) > 1)
            {
                strJong = "Y";
            }
            if (strJong == "")
            {
                strJong = "N";
            }

            List<ACTING_CHECK> list = actingCheckService.ACTING_CHECK_WAIT(nWrtNo, sysdate, Pano, strJong);

            SS1.ActiveSheet.RowCount = 0;
            nRead = list.Count;

            strList = ",";
            for (int i = 0; i < nRead; i++)
            {
                strList += list[i].ENTPART.Trim() + ",";
            }

            SS1.ActiveSheet.RowCount = nRead;
            nRow = 0;
            for (int i = 0; i < nRead; i++)
            {
                //정밀청력이 있으면 일반청력 검사 안함
                if (list[i].ENTPART.Trim() == "4")
                {
                    if (VB.InStr(strList, ",A,") > 0)
                    {
                    }
                    else
                    {
                        //상태점검
                        if (hicResultService.Read_Result_Acitve_Status(nWrtNo, sysdate, Pano, list[i].ENTPART.Trim(), strJong) > 0)
                        {
                            nRow += 1;
                            if (nRow > SS1.ActiveSheet.RowCount)
                            {
                                SS1.ActiveSheet.RowCount = nRow;
                                SS1.ActiveSheet.Cells[nRow, 1].Text = list[i].NAME.Trim();
                                SS1.ActiveSheet.Cells[nRow, 3].Text = list[i].ENTPART.Trim();

                                switch (list[i].ENTPART.Trim())
                                {
                                    case "4":   //청력
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "일반청력실";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "04";
                                        break;
                                    case "7":   //폐활량
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "폐기능검사실";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "06";
                                        break;
                                    case "D":   //구강상담
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "구강상담실";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "08";
                                        break;
                                    case "6":   //흉부촬영
                                    case "Q":   //요추촬영
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "흉부촬영실";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "10";
                                        break;
                                    case "H":   //심전도실
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "심전도실";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "11";
                                        break;
                                    case "A":   //정밀청력
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "정밀청력실";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "12";
                                        break;
                                    case "O":   //부인과검사
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "부인과검사실";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "14";
                                        break;
                                    case "E":   //일반상담
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "일반상담";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "15";
                                        break;
                                    default:
                                        SS1.ActiveSheet.Cells[nRow, 0].Text = "기타검사";
                                        SS1.ActiveSheet.Cells[nRow, 2].Text = "20";
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            //검사실,검사파트순으로 정렬
            clsSpread.gSpdSortRow(SS1, 2, ref boolSort, true);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string strRoom = "";
            string strRoom1 = "";
            string strRoom2 = "";
            string strRoom3 = "";
            string strRoom4 = "";
            string strNextRoom = "";
            string strRemark = "";
            int nWait = 0;
            string strSname = "";
            string strSex = "";
            long nAge = 0;
            string strGjJong = "";
            long nWRTNO = 0;

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                FarPoint.Win.Spread.CellType.CheckBoxCellType chk = new FarPoint.Win.Spread.CellType.CheckBoxCellType();
                SS2.ActiveSheet.Cells[0, 2, SS2.ActiveSheet.RowCount, 2].CellType = chk;

                txtWaitRemark.Text = txtWaitRemark.Text.Trim();
                if (txtWaitRemark.Text.Length > 98)
                {
                    MessageBox.Show("대기순번 전달사항 글자수 초과함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int i = 0; i < SS2.ActiveSheet.RowCount; i++)
                {   
                    strRoom = SS2.ActiveSheet.Cells[i, 0].Text.Trim();
                    if (SS2.ActiveSheet.Cells[i, 2].Text == "True") strRoom1 += strRoom;
                    if (SS2.ActiveSheet.Cells[i, 3].Text == "True") strRoom2 += strRoom;
                    if (SS2.ActiveSheet.Cells[i, 4].Text == "True") strRoom3 += strRoom;
                    if (SS2.ActiveSheet.Cells[i, 5].Text == "True") strRoom4 += strRoom;
                }

                if (strRoom1 == "")
                {
                    MessageBox.Show("다음 검사실이 지정 안됨!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (strRoom1.Length > 3)
                {
                    MessageBox.Show("다음 검사실이 중복 지정됨!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                //구강의사,상담의사,흉부촬영실,부인과검사실인 경우 반드시 다음 검사실이 지정되어야 합니다.
                if (strRoom1 == "08" || strRoom1 == "09" || strRoom1 == "10" || strRoom1 == "14" || 
                    (string.Compare(strRoom1, "15") >= 0 && string.Compare(strRoom1, "19") <= 0))
                {
                    if (strRoom2 == "")
                    {
                        MessageBox.Show("흉부촬영/구강/부인과/일반상담실은 반드시 이후 검사실이 지정되어야 합니다!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (strRoom2.Length > 3)
                    {
                        MessageBox.Show("②번 검사실이 중복으로 지정됨!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    //구강의사,상담의사인 경우 반드시 다음 검사실이 지정되어야 합니다.
                    if (strRoom2 == "08" || strRoom2 == "09" || strRoom2 == "10" || strRoom2 == "14" ||
                        (string.Compare(strRoom2, "15") >= 0 && string.Compare(strRoom2, "19") <= 0))
                    {
                        if (strRoom3 == "")
                        {
                            MessageBox.Show("흉부촬영/구강/부인과/일반상담실은 반드시 이후 검사실이 지정되어야 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (string.Compare(strRoom2, "29") <= 0)
                    {
                        if (strRoom3 != "")
                        {
                            MessageBox.Show("흉부촬영/구강/부인과/일반상담실만 다음 검사실 지정이 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    //구강의사,상담의사인 경우 반드시 다음 검사실이 지정되어야 합니다.
                    if (strRoom3 == "08" || strRoom3 == "09" || strRoom3 == "10" || strRoom2 == "14" ||
                        (string.Compare(strRoom3, "15") >= 0 && string.Compare(strRoom3, "19") <= 0))
                    {
                        if (strRoom4 == "")
                        {
                            MessageBox.Show("흉부촬영/구강/부인과/일반상담실은 반드시 이후 검사실이 지정되어야 합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else if (string.Compare(strRoom2, "29") <= 0)
                    {
                        if (strRoom4 != "")
                        {
                            MessageBox.Show("흉부촬영/구강/부인과/일반상담실만 다음 검사실 지정이 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                else if (string.Compare(strRoom2, "29") <= 0)
                {
                    if (strRoom2 != "")
                    {
                        MessageBox.Show("구강/부인과/일반상담실만 다음 검사실 지정이 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (strRoom3 != "")
                    {
                        MessageBox.Show("구강/부인과/일반상담실만 다음 검사실 지정이 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (strRoom4 != "")
                    {
                        MessageBox.Show("구강/부인과/일반상담실만 다음 검사실 지정이 가능합니다.", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                strNextRoom = "";
                if (strRoom2 != "" && strRoom3 != "" && strRoom4 != "")
                {
                    strNextRoom = strRoom2 + "," + strRoom3 + "," + strRoom4;
                }
                else if (strRoom2 != "" && strRoom3 != "")
                {
                    strNextRoom = strRoom2 + "," + strRoom3;
                }
                else if (strRoom2 != "")
                {
                    strNextRoom = strRoom2;
                }

                nWait = hicSangdamWaitService.GetMaxWaitNobyRoomCd(string.Format("0:00", strRoom1));

                //기존 등록된 대기순번을 삭제함
                int result = hicJepsuService.DeleteSangdamWaitbyPaNo(FPaNo);

                if (result < 0)
                {
                    MessageBox.Show("상담대기 등록 대기순번 삭제중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                List<HIC_JEPSU> lst = hicJepsuService.GetItembyPaNo(FPaNo);

                HIC_SANGDAM_WAIT wait = new HIC_SANGDAM_WAIT();
                

                if (lst.Count > 0)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        nWRTNO = lst[i].WRTNO;
                        strGjJong = lst[i].GJJONG;
                        strSname = lst[i].SNAME;
                        strSex = lst[i].SEX;
                        nAge = lst[i].AGE;

                        wait.WRTNO = nWRTNO;
                        wait.GJJONG = strGjJong;
                        wait.GUBUN = strRoom1;
                        wait.SNAME = strSname;
                        wait.SEX = strSex;
                        wait.AGE = nAge;
                        wait.WAITNO = nWait;

                        //상담대상 검진만 등록함
                        int result2 = hicJepsuService.InsertSangdamWaitbyPaNo(wait);

                        if (result2 < 0)
                        {
                            MessageBox.Show("상담대기 순번등록 중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }

                //대기순번 전달사항을 저장함
                if (FstrWaitRemark != "" || chkWait1.Checked == true || chkWait2.Checked == true || txtWaitRemark.Text.Trim() != "")
                {
                    strRemark = "";
                    if (chkWait1.Checked == true || chkWait2.Checked == true || txtWaitRemark.Text.Trim() != "")
                    {
                        if (chkWait1.Checked == true)
                        {
                            strRemark = "Y";
                        }
                        else
                        {
                            strRemark = "N";
                        }
                        if (chkWait2.Checked == true)
                        {
                            strRemark += "Y";
                        }
                        else
                        {
                            strRemark += "N";
                        }
                        strRemark += txtWaitRemark.Text.Trim();
                    }

                    int result2 = hicJepsuService.UpdateWaitRemarkbyWrtNo(strRemark, FWrtNo);

                    if (result2 < 0)
                    {
                        MessageBox.Show("대기순번 전달사항을 등록 중 오류 발생", "RollBack", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            this.Close();
        }
    }
}
