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
using System.IO;
using System.Text;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Act
/// File Name       : frmRoomNoSet.cs
/// Description     : 계측항목 Setting
/// Author          : 이상훈
/// Create Date     : 2019-08-29
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm방번호설정.frm(Frm방번호설정)" />

namespace HC_Act
{
    public partial class frmRoomNoSet : Form
    {
        HicWaitRoomService hicWaitRoomService = null;
        HicDoctorService hicDoctorService = null;
        BasScheduleService basScheduleService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();

        public frmRoomNoSet()
        {
            InitializeComponent();

            SetEvent();
        }

        void SetEvent()
        {
            hicWaitRoomService = new HicWaitRoomService();
            hicDoctorService = new HicDoctorService();
            basScheduleService = new BasScheduleService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            fn_Form_Load();

            clsCompuInfo.SetComputerInfo();
        }

        void fn_Form_Load()
        {
            int nRead = 0;

            fn_Set_Doct_Schedule();

            List<HIC_WAIT_ROOM> list = hicWaitRoomService.GetAll();
            nRead = list.Count;
            SS1.ActiveSheet.RowCount = nRead + 15;
            for (int i = 0; i < nRead; i++)
            {
                SS1.ActiveSheet.Cells[i, 0].Text = list[i].ROOM;
                SS1.ActiveSheet.Cells[i, 1].Text = list[i].ROOMNAME;
                SS1.ActiveSheet.Cells[i, 2].Text = list[i].AMUSE == "Y" ? "True" : "";
                SS1.ActiveSheet.Cells[i, 3].Text = list[i].PMUSE == "Y" ? "True" : "";
                SS1.ActiveSheet.Cells[i, 4].Text = list[i].AMSANG == "Y" ? "True" : "";
                SS1.ActiveSheet.Cells[i, 5].Text = list[i].PMSANG == "Y" ? "True" : "";
            }

            HIC_WAIT_ROOM item = hicWaitRoomService.GetItemByRoom("00");
            if(item.AMUSE == "Y") { chkWait.Checked = true; }
                
        }

        void fn_Set_Doct_Schedule()
        {
            long nSabun = 0;
            int nRead = 0;
            string strDrCode = "";
            string strRoom = "";
            string strAmUse = "";
            string strPmUse = "";
            string strAmSang = "";
            string strPmSang = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            List<HIC_DOCTOR> list = hicDoctorService.GetItemAll();

            nRead = list.Count;
            for (int i = 0; i < nRead; i++)
            {
                strRoom = list[i].ROOM;
                strDrCode = list[i].DRCODE;
                nSabun = list[i].SABUN;
                strAmUse = "N";
                strPmUse = "N";
                strAmSang = "N";
                strPmSang = "N";

                //오늘의 검진 스케쥴을 읽음
                BAS_SCHEDULE lst = basScheduleService.Read_Schedule(clsPublic.GstrSysDate, strDrCode);

                if (lst.GBJIN == "1")
                {
                    strAmUse = "Y";
                }

                if (lst.GBJIN2 == "1")
                {
                    strPmUse = "Y";
                }

                if (lst.GBJIN == "C")
                {
                    strAmUse = "Y";
                    strAmSang = "Y";
                }

                if (lst.GBJIN2 == "C")
                {
                    strPmUse = "Y";
                    strPmSang = "Y";
                }

                HIC_WAIT_ROOM item = new HIC_WAIT_ROOM();

                item.ROOM = strRoom;
                item.AMUSE = strAmUse;
                item.PMUSE = strPmUse;
                item.AMSANG = strAmSang;
                item.PMSANG = strPmSang;

                int result = hicWaitRoomService.UpdatebyRoom(item);

                if (result < 0)
                {
                    MessageBox.Show("방번호 설정값 저장 시 오류가 발생함!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            //문자열에 보관된 내용을 지정한 파일에 보관함
            File.WriteAllText(@"C:\SET_Doct_Schedule.txt", clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime, Encoding.Default);
        }

        void eBtnClick(object sender, EventArgs e)
        {
            string strRoom = "";
            string strName = "";
            string strAmUse = "";
            string strPmUse = "";
            string strROWID = "";
            string strAmSang = "";
            string strPmSang = "";

            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                for (int i = 0; i < SS1.ActiveSheet.NonEmptyRowCount; i++)
                {
                    strRoom = SS1.ActiveSheet.Cells[i, 0].Text.Trim();
                    if (!strRoom.IsNullOrEmpty())
                    {
                        strName = SS1.ActiveSheet.Cells[i, 1].Text.Trim();
                        strAmUse = "N";
                        strPmUse = "N";
                        strAmSang = "N";
                        strPmSang = "N";

                        if (SS1.ActiveSheet.Cells[i, 2].Text == "True") strAmUse = "Y";
                        if (SS1.ActiveSheet.Cells[i, 3].Text == "True") strPmUse = "Y";
                        if (SS1.ActiveSheet.Cells[i, 4].Text == "True") strAmSang = "Y";
                        if (SS1.ActiveSheet.Cells[i, 5].Text == "True") strPmSang = "Y";

                        strROWID = hicWaitRoomService.GetCountbyRoomCd(strRoom);

                        HIC_WAIT_ROOM item = new HIC_WAIT_ROOM();

                        item.ROOM = strRoom;
                        item.ROOMNAME = strName;
                        item.AMUSE = strAmUse;
                        item.PMUSE = strPmUse;
                        item.AMSANG = strAmSang;
                        item.PMSANG = strPmSang;
                        item.RID = strROWID;

                        if (strROWID.IsNullOrEmpty())
                        {
                            int result = hicWaitRoomService.InsertAll(item);

                            if (result < 0)
                            {
                                MessageBox.Show("방번호 설정값 저장 시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            int result = hicWaitRoomService.Update(item);

                            if (result < 0)
                            {
                                MessageBox.Show("방번호 설정값 저장 시 오류가 발생함", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }

                
                strRoom = "00";
                strROWID = hicWaitRoomService.GetCountbyRoomCd(strRoom);
                HIC_WAIT_ROOM item1 = new HIC_WAIT_ROOM();

                item1.ROOM = strRoom;
                item1.ROOMNAME = "부인과검사중";
                item1.AMUSE = "N";
                item1.PMUSE = "N";
                item1.RID = strROWID;

                if (chkWait.Checked == true)
                {
                    item1.AMUSE = "Y";
                    item1.PMUSE = "Y";
                }
                    
                int result1 = hicWaitRoomService.Update(item1);

                if (result1 < 0)
                {
                    MessageBox.Show("부인과검사중 구분 업데이트 오류!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            
                this.Close();
                return;
            }
        }
    }
}
