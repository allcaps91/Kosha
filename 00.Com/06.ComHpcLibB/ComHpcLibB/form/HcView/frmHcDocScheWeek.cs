using ComBase;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public partial class frmHcDocScheWeek : Form
    {

        HicDoctorService hicDoctorService = null;
        BasDoctorService basDoctorService = null;
        BasScheduleService basScheduleService = null;

        public frmHcDocScheWeek()
        {
            InitializeComponent();
            SetControl();
            SetEvents();
        }

        private void SetControl()
        {
            hicDoctorService = new HicDoctorService();
            basDoctorService = new BasDoctorService();
            basScheduleService = new BasScheduleService();
        }

        private void SetEvents()
        {
            this.Load += new EventHandler(eFormLoad);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.btnExit.Click += new EventHandler(eBtnClick);
        }


        void eFormLoad(object sender, EventArgs e)
        {
            //SET_SPREAD();
            Screen_Display();
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnSave)
            {
                //Data_Save();
            }
            else if (sender == btnExit)
            {
                this.Close();
                return;
            }
        }

        private void SET_SPREAD()
        {

        }

        private void Data_Save()
        {
            string strDrCode = "";
            string strDate = "";
            string strDay = "";
            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";

            for (int i = 0; i < SSList.ActiveSheet.RowCount; i++)
            {

                strDrCode = SSList.ActiveSheet.Cells[i, 1].Text;

                int result = basScheduleService.Delete(strDrCode,"1990-01-01","1990-01-06");

                if (result < 0)
                {
                    MessageBox.Show("종전의 자료를 삭제도중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                for (int j = 0; i < 11; i++)
                {
                    strDate = "1990-01-" + VB.Format(j, "00");
                    strDay = "1";

                    if (j == 12) { strDay = "2"; }
                    strGbn = SSList.ActiveSheet.Cells[i, j * 3].Text;
                    strGbn2 = SSList.ActiveSheet.Cells[i, (j * 3)+1].Text;
                    strGbn3 = SSList.ActiveSheet.Cells[i, (j * 3)+2].Text;


                    int result1 = basScheduleService.Insert(strDrCode, strDate, strDay, strGbn, strGbn2, strGbn3);

                    if (result1 < 0)
                    {
                        MessageBox.Show("자료를 신규등록중 오류 발생!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
        }

        private void Screen_Display()
        {
            List<string> strDrList = new List<string>();
            string strDrCode = "";
            string strDeptCode = "";

            int nDAY = 0;
            int nCnt = 0;
            string strGbn = "";
            string strGbn2 = "";
            string strGbn3 = "";

            List <HIC_DOCTOR> list = hicDoctorService.GetListbyReday(clsPublic.GstrSysDate);
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

            List<BAS_DOCTOR> list2 = basDoctorService.GetItembyDrCodes(strDrList);

            SSList.ActiveSheet.RowCount = list2.Count;

            for (int i = 0; i < list2.Count; i++)
            {
                strDeptCode = list2[i].DRDEPT1;
                strDrCode = list2[i].DRCODE;

                SSList.ActiveSheet.Cells[i, 0].Text = strDeptCode;
                SSList.ActiveSheet.Cells[i, 1].Text = strDrCode;
                SSList.ActiveSheet.Cells[i, 2].Text = list2[i].DRNAME;

                List<BAS_SCHEDULE> list3 = basScheduleService.GetItembySchDateDrcode("1990-01-01", "1990-01-06", strDrCode);

                for (int j = 0; j < list3.Count; j++)
                {
                    nDAY = Convert.ToInt32(list3[j].ILJA);
                    strGbn = list3[j].GBJIN;
                    strGbn2 = list3[j].GBJIN2;
                    strGbn3 = list3[j].GBJIN3;

                    SSList.ActiveSheet.Cells[i, (nDAY*3)].Text = strGbn;
                    switch (strGbn)
                    {
                        case "1":
                            //SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = Color.Gold;
                            SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label1.BackColor;
                            break;
                        case "2":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label2.BackColor;
                            break;
                        case "3":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label3.BackColor;
                            break;
                        case "4":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label4.BackColor;
                            break;
                        case "9":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label5.BackColor;
                            break;
                        default:
                            SSList.ActiveSheet.Cells[i, (nDAY * 3)].BackColor = label4.BackColor;
                            break;
                    }

                    SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].Text = strGbn2;
                    switch (strGbn2)
                    {
                        case "1":
                            //SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = Color.Gold;
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label1.BackColor;
                            break;
                        case "2":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label2.BackColor;
                            break;
                        case "3":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label3.BackColor;
                            break;
                        case "4":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label4.BackColor;
                            break;
                        case "9":
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label5.BackColor;
                            break;
                        default:
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = label4.BackColor;
                            break;
                    }

                    SSList.ActiveSheet.Cells[i, (nDAY * 3) + 2].Text = strGbn3;
                    switch (strGbn3)
                    {
                        case "1":
                            //SSList.ActiveSheet.Cells[i, (nDAY * 3) + 1].BackColor = Color.Gold;
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 2].BackColor = label1.BackColor;
                            break;
                        default:
                            SSList.ActiveSheet.Cells[i, (nDAY * 3) + 2].BackColor = label4.BackColor;
                            break;
                    }

                    //컬럼숨기기
                    SSList.ActiveSheet.Columns[(nDAY * 3) + 2].Visible = false;

                }

            }
        }

    }

}
