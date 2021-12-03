using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcDocSche : Form
    {
        string[] FstrHolyDay = new string[32];
        string[] FstrYoil = new string[32];
        string[] FstrYoilJin = new string[7];
        string[] FstrYoilJin2 = new string[7];
        string[] FstrYoilJin3 = new string[7];

        clsSpread cSpd = null;

        HicDoctorService hicDoctorService = null;
        BasDoctorService basDoctorService = null;
        BasScheduleService basScheduleService = null;
        BasJobService basJobService = null;

        public frmHcDocSche()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            cSpd = new clsSpread();

            hicDoctorService = new HicDoctorService();
            basDoctorService = new BasDoctorService();
            basScheduleService = new BasScheduleService();
            basJobService = new BasJobService();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnCancel.Click += new EventHandler(eBtnClick);
            this.btnDelete.Click += new EventHandler(eBtnClick);
            this.btnPrint.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnShow.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            int nYY = DateTime.Now.Year;
            int nMM = DateTime.Now.Month;

            for (int i = 1; i < 13; i++)
            {
                cboYYMM.Items.Add(VB.Format(nYY,"0000")+"-"+VB.Format(nMM,"00"));
                nMM = nMM + 1;
                if (nMM ==13) { nYY = nYY + 1; nMM = 1; }
            }
            cboYYMM.SelectedIndex = 0;

            //일수 31일까지 표시
            for (int i = 1; i < 32; i++)
            {
                SSList.ActiveSheet.ColumnHeader.Cells.Get(0, (i*3) - 1).Value = i;
            }

            for (int i = 1; i < 32; i++)
            {
                SSList.ActiveSheet.Columns[(i*3)+1].Visible = false;
            }
            SSList.ActiveSheet.Columns[95].Visible = false;
            SSList.ActiveSheet.Columns[96].Visible = false;

            //진료과 생략
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSearch)
            {
                Screen_Display();
            }
            else if (sender == btnSave)
            {
                Data_Save();
            }
            else if (sender == btnCancel)
            {
                Screen_Clear();
            }
            else if (sender == btnDelete)
            {
                Date_Delete();
            }
            else if (sender == btnPrint)
            {
                Spread_Print();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnShow)
            {
                frmHcDocScheWeek f = new frmHcDocScheWeek();
                f.Show();
            }
        }

        private void Screen_Display()
        {
            List<string> strDrList = new List<string>();

            string strYYMM = "";
            string strFDATE = "";
            string strTDATE = "";
            string strDATE = "";
            string strYoil = "";

            string strDRCODE = "";
            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";
            int nCnt = 0;
            int nDAY = 0;
            int nLastDay = 0;

            ComFunc cF = new ComFunc();

            strYYMM = VB.Left(cboYYMM.Text, 4) + VB.Mid(cboYYMM.Text, 6, 2);
            strFDATE = cboYYMM.Text + "-01";
            strTDATE = cF.READ_LASTDAY(clsDB.DbCon, strFDATE);
            nLastDay = Convert.ToInt32(VB.Right(strTDATE,2));

            for (int i = 0; i < 32; i++)
            {
                FstrHolyDay[i] = ""; FstrYoil[i] = " ";
            }

            List<HIC_DOCTOR> list = hicDoctorService.GetListbyReday(clsPublic.GstrSysDate);

            strDrList.Add("1109");
            nCnt = 1;
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    nCnt = nCnt + 1;
                    strDrList.Add(list[i].DRCODE.Trim());
                }
            }
        
            btnSearch.Enabled = false;
            cboYYMM.Enabled = false;
            cboDEPT.Enabled = false;

            //BAS_SCHEDULE item = basScheduleService.Read_Schedule_CNT(strFDATE, strTDATE);

            if (basScheduleService.Read_Schedule_CNT(strFDATE, strTDATE) == 0)
            {
                MessageBox.Show("해당월의 스케줄을 신규로 만드시겠습니까?", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<BAS_JOB> list2 = basJobService.READ_Holyday(strFDATE, strTDATE);

            for (int i = 0; i < list2.Count; i++)
            {
                nDAY = Convert.ToInt32((list2[i].ILJA));
                FstrHolyDay[nDAY] = "*";
            }

            for (int i = 1; i <= nLastDay; i++)
            {

                strDATE = cboYYMM.Text + "-" + VB.Format(i, "00");
                strYoil = cF.READ_YOIL(clsDB.DbCon, strDATE);

                switch (strYoil)
                {
                    case "월요일": FstrYoil[i] = "1"; break;
                    case "화요일": FstrYoil[i] = "2"; break;
                    case "수요일": FstrYoil[i] = "3"; break;
                    case "목요일": FstrYoil[i] = "4"; break;
                    case "금요일": FstrYoil[i] = "5"; break;
                    case "토요일": FstrYoil[i] = "6"; break;
                    case "일요일": FstrYoil[i] = "7"; break;
                    default: break;
                }

                if (FstrHolyDay[i] != "*" && FstrYoil[i] =="6")
                {
                    FstrHolyDay[i] = "#";
                }

                //헤드값 날짜 및 요일표시
                //SSList.ActiveSheet.ColumnHeader.Cells.Get(0, (i * 3) - 1).Value = i;
                SSList.ActiveSheet.ColumnHeader.Cells.Get(1, (i*3)-1).Value = VB.Left(strYoil, 1);

                SSList.ActiveSheet.RowCount = 1;
                if (FstrHolyDay[i] == "*")
                {
                    SSList.ActiveSheet.Cells[0, (i*3)].BackColor = label1.BackColor;
                }
                else if (FstrHolyDay[i] == "#")
                {
                    SSList.ActiveSheet.Cells[0, (i*3)].BackColor = label2.BackColor;
                }
                else
                {
                    SSList.ActiveSheet.Cells[0, (i*3)].BackColor = label4.BackColor;
                }
            }

            //진료과별 의사코드, 성명을 READ
            List<BAS_DOCTOR> list3 = basDoctorService.GetItembyDrCodes(strDrList);
            SSList.ActiveSheet.RowCount = list3.Count;

            for (int i = 0; i < list3.Count; i++)
            {
                strDRCODE= list3[i].DRCODE;

                SSList.ActiveSheet.Cells[i, 0].Text = list3[i].DRDEPT1;
                SSList.ActiveSheet.Cells[i, 1].Text = list3[i].DRNAME;
                SSList.ActiveSheet.Cells[i, 95].Text = strDRCODE;
                

                if (VB.Right(strDRCODE,2) =="99")
                {
                    SSList.ActiveSheet.Rows[i].Visible = false;
                }



                List<BAS_SCHEDULE> list4 = basScheduleService.GetItembySchDateDrcode(strFDATE, strTDATE, strDRCODE);

                if (list4.Count > 0 )
                { 
                    for (int j = 0; j < list4.Count; j++)
                    {
                        nDAY = Convert.ToInt32(list4[j].ILJA);
                        strGbn = list4[j].GBJIN;
                        strGbn2 = list4[j].GBJIN2;
                        strGbn3 = list4[j].GBJIN3;

                        SSList.ActiveSheet.Cells[i, (nDAY*3)- 1].Text = strGbn;
                        switch (strGbn)
                        {
                            case "1":
                                //SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = Color.Gold;
                                SSList.ActiveSheet.Cells[i, (nDAY*3)-1].BackColor = label1.BackColor;
                                break;
                            case "2":
                                SSList.ActiveSheet.Cells[i, (nDAY*3)-1].BackColor = label2.BackColor;
                                break;
                            case "3":
                                SSList.ActiveSheet.Cells[i, (nDAY*3)-1].BackColor = label3.BackColor;
                                break;
                            case "9":
                                SSList.ActiveSheet.Cells[i, (nDAY*3)-1].BackColor = label9.BackColor;
                                break;
                            default:
                                SSList.ActiveSheet.Cells[i, (nDAY*3)-1].BackColor = label4.BackColor;
                                break;
                        }

                        SSList.ActiveSheet.Cells[i, (nDAY * 3)].Text = strGbn2;
                        switch (strGbn2)
                        {
                            case "1":
                                SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label1.BackColor;
                                break;
                            case "2":
                                SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label2.BackColor;
                                break;
                            case "3":
                                SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label3.BackColor;
                                break;
                            case "9":
                                SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label9.BackColor;
                                break;
                            default:
                                SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label4.BackColor;
                                break;
                        }

                        SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].Text = strGbn3;
                        if(VB.Left(cboDEPT.Text,2) != "PD")
                        {
                            SSList.ActiveSheet.Columns[0].Visible = true;
                        }
                        else
                        {
                            SSList.ActiveSheet.Columns[0].Visible = false;
                        }

                        switch (strGbn3)
                        {
                            case "1":
                                SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label1.BackColor;
                                break;

                            default:
                                SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label4.BackColor;
                                break;
                        }
                    }
                }
                else
                {
                    SSList.ActiveSheet.Cells[i, 96].Text = "Y";

                    List<BAS_SCHEDULE> list5 = basScheduleService.GetItembySchDateDrcode("1990-01-01", "1990-01-06", strDRCODE);
                    for (int j = 1; j <= 6; j++)
                    {
                        FstrYoilJin[j] = " ";
                        FstrYoilJin2[j] = " ";
                    }

                    for (int j = 0; j < list5.Count; j++)
                    {
                        nDAY = Convert.ToInt32(list5[j].ILJA);
                        strGbn = list4[j].GBJIN;
                        strGbn2 = list4[j].GBJIN2;
                        strGbn3 = list4[j].GBJIN3;

                        if (strGbn != " ") { FstrYoilJin[nDAY] = strGbn; }
                        if (strGbn2 != " ") { FstrYoilJin[nDAY] = strGbn2; }
                        if (strGbn3 != " ") { FstrYoilJin[nDAY] = strGbn3; }

                    }


                    for (int j = 1; j <= nLastDay; j++)
                    {
                        if (SSList.ActiveSheet.Cells[i, (j*3)-1].Text == "" && FstrYoil[j] != "7")
                        {
                            strGbn = FstrYoilJin[Convert.ToInt32(FstrYoil[j])];
                            SSList.ActiveSheet.Cells[i, (j*3)-1].Text = strGbn;
                            switch (strGbn)
                            {
                                case "1":
                                    SSList.ActiveSheet.Cells[i, (j*3)-1].BackColor = label1.BackColor;
                                    break;
                                case "2":
                                    SSList.ActiveSheet.Cells[i, (j*3)-1].BackColor = label2.BackColor;
                                    break;
                                case "3":
                                    SSList.ActiveSheet.Cells[i, (j*3)-1].BackColor = label3.BackColor;
                                    break;
                                case "9":
                                    SSList.ActiveSheet.Cells[i, (j*3)-1].BackColor = label9.BackColor;
                                    break;
                                default:
                                    SSList.ActiveSheet.Cells[i, (j*3)-1].BackColor = label4.BackColor;
                                    break;
                            }

                        }

                        if (SSList.ActiveSheet.Cells[i, (j*3)].Text == "" && FstrYoil[j] != "7")
                        {
                            strGbn2 = FstrYoilJin[Convert.ToInt32(FstrYoil[j])];
                            SSList.ActiveSheet.Cells[i, (j*3)].Text = strGbn2;
                            switch (strGbn2)
                            {
                                case "1":
                                    SSList.ActiveSheet.Cells[i, (j*3)].BackColor = label1.BackColor;
                                    break;
                                case "2":
                                    SSList.ActiveSheet.Cells[i, (j*3)].BackColor = label2.BackColor;
                                    break;
                                case "3":
                                    SSList.ActiveSheet.Cells[i, (j*3)].BackColor = label3.BackColor;
                                    break;
                                case "9":
                                    SSList.ActiveSheet.Cells[i, (j*3)].BackColor = label9.BackColor;
                                    break;
                                default:
                                    SSList.ActiveSheet.Cells[i, (j*3)].BackColor = label4.BackColor;
                                    break;
                            }

                        }


                        if (SSList.ActiveSheet.Cells[i, (j*3)+1].Text == "" && FstrYoil[j] != "7")
                        {
                            strGbn3 = FstrYoilJin[Convert.ToInt32(FstrYoil[j])];
                            SSList.ActiveSheet.Cells[i,(j*3)+1].Text = strGbn3;
                            switch (strGbn3)
                            {
                                case "1":
                                    SSList.ActiveSheet.Cells[i, (j*3)+1].BackColor = label1.BackColor;
                                    break;
                                default:
                                    SSList.ActiveSheet.Cells[i, (j*3)+1].BackColor = label4.BackColor;
                                    break;
                            }

                        }

                    }

                }

                //for (int j = 1; j <= nLastDay; j++)
                //{
                //    SSList.ActiveSheet.Columns[(j*3)+1].Visible = false;
                //}
                    

                for (int j = 1; j <= nLastDay; j++)
                {
                    if(FstrHolyDay[j]!= " ")
                    {
                        if (FstrHolyDay[j] == "*")
                        {
                            //SSList.ActiveSheet.Cells[i,j*3]

                        }
                        else if (FstrHolyDay[j] == "#")
                        {

                        }
                    }

                }

             }

            btnCancel.Enabled = true;
         }
        private void Screen_Clear()
        {
            grbYYMM.Enabled = true;
            grbDept.Enabled = true;
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            SSList.ActiveSheet.RowCount = 0;
            SSList.ActiveSheet.RowCount = 30;
            for (int i = 1; i <= SSList.ActiveSheet.ColumnCount; i++)
            {
                SSList.ActiveSheet.Cells[0, i].Text = ""; SSList.ActiveSheet.Cells[0, i].BackColor = label4.BackColor;
            }
        }

        //저장
        private void Data_Save()
        {
            int nLastDay = 0;
            string strFDATE = "";
            string strTDATE = "";
            string strDATE = "";
            string strDayGbn = "";
            string strDRCODE = "";
            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";
            string strYoil = "";
            string strOldGbn = "";
            string strOK = "";
            string strROWID = "";
            ComFunc cF = new ComFunc();

            btnSave.Enabled = false;
            btnCancel.Enabled = false;

            strFDATE = cboYYMM.Text + "-01";
            strTDATE = cF.READ_LASTDAY(clsDB.DbCon, strFDATE);
            nLastDay = Convert.ToInt32(VB.Right(strTDATE, 2));

            for (int i = 1; i <= SSList.ActiveSheet.RowCount; i++)
            {
                strDRCODE = SSList.ActiveSheet.Cells[i-1, 95].Text;

                if (SSList.ActiveSheet.Cells[i-1, 96].Text == "Y")
                {
                    for (int j = 1; j<= nLastDay; j++)
                    {
                        strOK = "OK";
                        strGbn = SSList.ActiveSheet.Cells[i-1, (j*3)-1].Text;
                        strGbn2 = SSList.ActiveSheet.Cells[i-1, (j*3)].Text;
                        strGbn3 = SSList.ActiveSheet.Cells[i-1, (j*3)+1].Text;
                        strDATE = cboYYMM.Text + "-" + VB.Format(j, "00");

                        strROWID = "";
                        BAS_SCHEDULE item = basScheduleService.Read_Schedule(strDATE, strDRCODE);

                        strDayGbn = "1";
                        if (FstrHolyDay[j] == "*") { strDayGbn = "3"; }
                        if (FstrHolyDay[j] == "#") { strDayGbn = "2"; }

                        if (!item.IsNullOrEmpty())
                        {
                            strROWID = item.ROWID;
                            int result = basScheduleService.Update(strDayGbn, strGbn, strGbn2, strGbn3, strROWID);
                            if (result < 0)
                            {
                                MessageBox.Show("의사별 진료스케쥴 UPDATE 오류!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            int result = basScheduleService.Insert(strDRCODE, strDATE, strDayGbn, strGbn, strGbn2, strGbn3);
                            if (result < 0)
                            {
                                MessageBox.Show("의사별 진료스케쥴 INSERT 오류!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    Screen_Clear();
                    cboYYMM.Focus();
                }
            }
        }
        //삭제
        private void Date_Delete()
        {
            int nLastDay = 0;

            string strDrCode = "";
            string strFDATE = "";
            string strTDATE = "";

            ComFunc cF = new ComFunc();

            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnDelete.Enabled = false;

            strFDATE = cboYYMM.Text + "-01";
            strTDATE = cF.READ_LASTDAY(clsDB.DbCon, strFDATE);
            nLastDay = Convert.ToInt32(VB.Right(strTDATE, 2));

            if (ComFunc.MsgBoxQ("정말 삭제를 하시겠습니까?", " 경고", MessageBoxDefaultButton.Button2) == DialogResult.No)
            {
                return;
            }

            for (int i = 1; i <= SSList.ActiveSheet.RowCount; i++)
            {
                strDrCode = SSList.ActiveSheet.Cells[i-1, 95].Text;
                int result = basScheduleService.Delete(strDrCode, strFDATE, strTDATE);
                if (result < 0)
                {
                    MessageBox.Show("의사별 진료스케줄 삭제도중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        //인쇄
        private void Spread_Print()
        {
            string strTitle = "";
            string strHeader = "";
            string strFooter = "";
            bool PrePrint = true;

            ComFunc.ReadSysDate(clsDB.DbCon);

            clsSpread.SpdPrint_Margin setMargin;
            clsSpread.SpdPrint_Option setOption;

            strTitle = "의사별 진료 스케쥴";
            strHeader = cSpd.setSpdPrint_String(strTitle, new Font("굴림체", 15, FontStyle.Bold), clsSpread.enmSpdHAlign.Center, false, true);
            strHeader += cSpd.setSpdPrint_String("진료년월: " + cboYYMM.Text + "          "+ "인쇄일자: " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime,  new Font("굴림체", 10), clsSpread.enmSpdHAlign.Left, false, true);

            setMargin = new clsSpread.SpdPrint_Margin(10, 10, 50, 10, 10, 10);
            setOption = new clsSpread.SpdPrint_Option(PrintOrientation.Landscape, PrintType.All, 0, 0, true, false, true, true, false, false, true);

            cSpd.setSpdPrint(SSList, PrePrint, setMargin, setOption, strHeader, strFooter);
        }
    }
}
