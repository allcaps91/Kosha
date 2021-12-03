using ComBase; //기본 클래스
using System;
using System.Data;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

/// <summary>
/// Description : OCS 추가오더 알림창
/// Author : 이상훈
/// Create Date : 2017.07.24
/// </summary>
/// <history>
/// </history>
/// <seealso cref="FrmIOCSTray.frm, "/>

namespace ComLibB
{
    public partial class FrmOcsTray : Form
    {
        public FrmOcsTray()
        {
            InitializeComponent();
        }

        string strExit;
        string SQL = "";
        DataTable dt = null;
        string SqlErr = "";
        int intRowAffected = 0; //변경된 Row 받는 변수

        int GnTimerCnt;
        int GnMin;          //메세지 display set time db에서 읽음

        string strSort;

        clsSpread SP = new clsSpread();
        clsIniFile INI = new clsIniFile();


        [DllImport("winmm.DLL", EntryPoint = "PlaySound", SetLastError = true)]
        private static extern bool PlaySound(string szSound, System.IntPtr hMod, PlaySoundFlags flags);
        [System.Flags]
        enum PlaySoundFlags : int
        {
            SND_SYNC = 0x0000,
            SND_ASYNC = 0x0001,
            SND_NODEFAULT = 0x0002,
            SND_LOOP = 0x0008,
            SND_NOSTOP = 0x0010,
            SND_NOWAIT = 0x00002000,
            SND_FILENAME = 0x00020000,
            SND_RESOURCE = 0x00040004
        }


        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timer1.Enabled == false) return;

            if (toolStripStatusLabel2.Text.Trim() == "") toolStripStatusLabel2.Text = Convert.ToString(timer1.Interval);

            //btnView_Click(btnView, e);

            if (알림시화면복귀여부.Checked == true)
            {
                if (ssOrder.ActiveSheet.NonEmptyRowCount > 0 || ssConsult.ActiveSheet.NonEmptyRowCount > 0 || ssCV.ActiveSheet.NonEmptyRowCount > 0)
                {
                    this.Visible = true;                    //폼 보이게...
                    this.ShowInTaskbar = false;             //태스크바에서 안보이게..
                    this.TryOrderCatcher.Visible = true;    //트레이아이콘 보이게..
                }
            }
            else
            {
                this.Visible = false;                       //폼 보이게...
                this.ShowInTaskbar = false;                 //태스크바에서 안보이게..
                this.TryOrderCatcher.Visible = true;        //트레이아이콘 보이게..
            }

            if (벨소리.Checked == true)
            {
                //PlaySound("C:\\WINDOWS\\Media\\notify.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
                PlaySound("C:\\WINDOWS\\Media\\ringin.wav", new System.IntPtr(), PlaySoundFlags.SND_SYNC);
            }

            if (clsOcsTray.fn_Read_CV(clsDB.DbCon, ssCV) == "OK")
            {
                this.Visible = true;
                this.ShowInTaskbar = false;             //태스크바에서 안보이게..
                this.TryOrderCatcher.Visible = true;    //트레이아이콘 보이게..
                timer3.Enabled = true;
                timer1.Enabled = false;
            }
        }

        private void 화면복귀_Click(object sender, EventArgs e)
        {
            this.Visible = true;                    //폼 보이게...
            this.ShowInTaskbar = false;             //태스크바에서 안보이게..
            this.TryOrderCatcher.Visible = true;    //트레이아이콘 보이게..
        }
        
        private void 알림시화면복귀여부_Click(object sender, EventArgs e)
        {
            if (알림시화면복귀여부.Checked == true)
            {
                //VB.GetSetting("TWIN", "SCREEN", "Y");
                VB.SaveSetting("TWIN", "OCSTRAY", "SCREEN", "Y");
                clsOrdFunction.Set_Screen = "Y";
            }
            else
            {
                VB.SaveSetting("TWIN", "OCSTRAY", "SCREEN", "N");
                clsOrdFunction.Set_Screen = "N";
            }
        }

        private void 벨소리_Click(object sender, EventArgs e)
        {
            if (벨소리.Checked == true)
            {
                VB.SaveSetting("TWIN", "OCSTRAY", "BELL", "Y");
                clsOrdFunction.Set_BELL = "Y";
            }
            else
            {
                VB.SaveSetting("TWIN", "OCSTRAY", "BELL", "N");
                clsOrdFunction.Set_BELL = "N";
            }
        }

        private void 종료_Click(object sender, EventArgs e)
        {
            strExit = "EXIT";
            this.Close();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (timer2.Enabled == true)
            {   
                pictureBox1.Location = new Point(359, 7);
                pictureBox2.Location = new Point(359, 7);
                pictureBox3.Location = new Point(359, 7);
                pictureBox4.Location = new Point(359, 7);
                pictureBox5.Location = new Point(359, 7);
                pictureBox6.Location = new Point(359, 7);
                pictureBox7.Location = new Point(359, 7);
                pictureBox8.Location = new Point(359, 7);

                if (pictureBox1.Visible == true)
                {
                    pictureBox1.Visible = false;
                    pictureBox2.Visible = true;
                }
                else if (pictureBox2.Visible == true)
                {
                    pictureBox2.Visible = false;
                    pictureBox3.Visible = true;
                }
                else if (pictureBox3.Visible == true)
                {
                    pictureBox3.Visible = false;
                    pictureBox4.Visible = true;
                }
                else if (pictureBox4.Visible == true)
                {
                    pictureBox4.Visible = false;
                    pictureBox5.Visible = true;
                }
                else if (pictureBox5.Visible == true)
                {
                    pictureBox5.Visible = false;
                    pictureBox6.Visible = true;
                }
                else if (pictureBox6.Visible == true)
                {
                    pictureBox6.Visible = false;
                    pictureBox7.Visible = true;
                }
                else if (pictureBox7.Visible == true)
                {
                    pictureBox7.Visible = false;
                    pictureBox8.Visible = true;
                }
                else if (pictureBox8.Visible == true)
                {
                    pictureBox8.Visible = false;
                    pictureBox1.Visible = true;
                }
            }
        }

        private void FrmOcsTray_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (strExit == "EXIT")
            {
                e.Cancel = false;
            }
            else if (strExit == "TRAY")
            {
                e.Cancel = true;                        //폼닫기 취소
                this.Visible = false;                   //폼 안보이게...
                this.ShowInTaskbar = false;             //태스크바에서 안보이게..
                this.TryOrderCatcher.Visible = true;    //트레이아이콘 보이게..
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            strExit = "TRAY";
            this.Close();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            timer2.Enabled = true;
            timer3.Enabled = true;
            timer4.Enabled = true;
            toolStripStatusLabel1.Text = "확인하지 않은 추가처방, Consult, CV 를 조회합니다!!";
            btnExit_Click(btnExit, new EventArgs());
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            toolStripStatusLabel1.Text = "추가처방, Consult, CV조회를 종료합니다!";
        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            strExit = "TRAY";
            this.Close();
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            GnTimerCnt += 1;
            if (GnTimerCnt > GnMin) //10분
            {
                if (clsOcsTray.fn_Read_Ocs_Msg(clsDB.DbCon, ssCV) == "OK")
                {
                    this.Show();
                    timer3.Enabled = true;
                }
                if (clsOcsTray.fn_Read_ITransfer(clsDB.DbCon, ssConsult) == "OK") //'컨설트 완료 READ
                {
                    this.Visible = true;
                    this.ShowInTaskbar = false;             //태스크바에서 안보이게..
                    this.TryOrderCatcher.Visible = true;    //트레이아이콘 보이게..
                    timer4.Enabled = true;
                }

                if (clsOcsTray.fn_Read_CV(clsDB.DbCon, ssCV) == "OK")
                {
                    this.Visible = true;
                    this.ShowInTaskbar = false;             //태스크바에서 안보이게..
                    this.TryOrderCatcher.Visible = true;    //트레이아이콘 보이게..
                    timer4.Enabled = true;
                    timer2.Enabled = false;
                }


                GnMin = int.Parse(clsVbfunc.GetBCodeCODE(clsDB.DbCon, "OCS_메세지간격", ""));

                /*
                if (clsVbfunc.GetBCodeCODE("OCS_TRAYDOWN", "") == "Y")
                {
                    SELF_AUTO_DOWN_SHELL(); //자신 DOWN LOAD
                }
                */
                GnTimerCnt = 0;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (int i = 0; i < ssOrder.ActiveSheet.RowCount; i++)
                {
                    if (ssOrder.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        SQL = "";
                        SQL += " UPDATE  ADMIN.OCS_MSG                                             \r";
                        SQL += "    SET STATE = 'Y'                                                     \r";
                        SQL += "  WHERE ROWID ='" + ssOrder.ActiveSheet.Cells[i, 7].Text.Trim() + "'    \r";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            Cursor.Current = Cursors.Default;
        }

        private void btnConsltOK_Click(object sender, EventArgs e)
        {
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (int i = 0; i < ssConsult.ActiveSheet.RowCount; i++)
                {
                    if (ssConsult.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        SQL = "";
                        SQL += " UPDATE ADMIN.OCS_ITRANSFER                                        \r";
                        SQL += "    SET NURSEOK = '*'                                                   \r";
                        SQL += "  WHERE ROWID ='" + ssConsult.ActiveSheet.Cells[i, 15].Text.Trim() + "' \r";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            Cursor.Current = Cursors.Default;
        }

        private void btnCVOK_Click(object sender, EventArgs e)
        {
            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (int i = 0; i < ssConsult.ActiveSheet.RowCount; i++)
                {
                    if (ssConsult.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        SQL = "";
                        SQL += " UPDATE ADMIN.EXAM_RESULTC_CV                                      \r";
                        SQL += "    SET CHKDATE= SYSDATE                                                \r";
                        SQL += "      , CHKSABUN  = '" + txtSabun.Text.Trim() + "'                      \r";
                        SQL += "  WHERE ROWID ='" + ssConsult.ActiveSheet.Cells[i, 15].Text.Trim() + "' \r";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                Cursor.Current = Cursors.Default;
            }

            clsDB.setCommitTran(clsDB.DbCon);
            Cursor.Current = Cursors.Default;
        }

        private void ssOrder_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strPano;
            string strOcsDate;
            string strRowid;

            if (e.ColumnHeader == true) return;

            strPano = ssOrder.ActiveSheet.Cells[e.Row, 3].Text;
            strOcsDate = ssOrder.ActiveSheet.Cells[e.Row, 6].Text;
            strRowid = ssOrder.ActiveSheet.Cells[e.Row, 7].Text;

            frmOrdersView Ord = new frmOrdersView(strPano, strOcsDate, strRowid);
            Ord.Show();
        }

        private void ssConsult_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string sOrdDate, sConDate, sConFrDept, sConFrDr, sConContents, sRsltDate, sConToDept, sConToDr, sConResult;

            if (e.ColumnHeader == true) return;

            sOrdDate = ssConsult.ActiveSheet.Cells[e.Row, 4].Text;
            sConDate = ssConsult.ActiveSheet.Cells[e.Row, 12].Text;
            sConFrDept = ssConsult.ActiveSheet.Cells[e.Row, 5].Text;
            sConFrDr = ssConsult.ActiveSheet.Cells[e.Row, 6].Text;
            sConContents = ssConsult.ActiveSheet.Cells[e.Row, 11].Text;
            sRsltDate = ssConsult.ActiveSheet.Cells[e.Row, 13].Text;
            sConToDept = ssConsult.ActiveSheet.Cells[e.Row, 7].Text;
            sConToDr = ssConsult.ActiveSheet.Cells[e.Row, 8].Text;
            sConResult = ssConsult.ActiveSheet.Cells[e.Row, 14].Text;

            timer4.Enabled = false;

            frmConsultResult Con = new frmConsultResult(sOrdDate, sConDate, sConFrDept, sConFrDr, sConContents, sRsltDate, sConToDept, sConToDr, sConResult);
            Con.Show();

            timer4.Enabled = true;
        }

        private void TryOrderCatcher_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Visible = true;                    //폼 보이게...
            this.ShowInTaskbar = false;             //태스크바에서 안보이게..
            this.TryOrderCatcher.Visible = true;    //트레이아이콘 보이게..
        }

        private void txtSabun_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;

            txtSabun.Text = string.Format("{0:00000}", txtSabun.Text);
            lblName.Text = "";
            lblName.Text = clsVbfunc.GetInSaName(clsDB.DbCon, string.Format("{0:00000}", txtSabun.Text));
        }

        private void FrmOcsTray_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)//폼 권한 조회
            {
                this.Close();
                return; 
            }
            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등

            clsPublic.GstrWardCode = VB.GetSetting("TWIN", "NURVIEW", "WARDCODE");

            clsOrdFunction.Set_Screen = VB.GetSetting("TWIN", "OCSTRAY", "SCREEN");
            clsOrdFunction.Set_BELL = VB.GetSetting("TWIN", "OCSTRAY", "BELL");

            if (clsOrdFunction.Set_Screen == "Y")
            {
                알림시화면복귀여부.Checked = true;
            }
            else
            {
                알림시화면복귀여부.Checked = false;
            }

            if (clsOrdFunction.Set_BELL == "Y")
            {
                벨소리.Checked = true;
            }
            else
            {
                벨소리.Checked = false;
            }

            lblWard.Text = clsPublic.GstrWardCode;
            
            this.Text = "◈추가처방/Consult/CV 확인◈" + "- [" + clsPublic.GstrWardCode + "병동]" + "-[사용자 : " + clsVbfunc.GetInSaName(clsDB.DbCon, string.Format("{0:00000}", clsPublic.GstrJobSabun)) + "]";
        }

        private void ssOrder_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e == null) return;
            if (e.ColumnHeader == true)
            {
                if (e.ColumnHeader == true)
                {
                    if (strSort == "" || strSort == "A")
                    {
                        ssOrder.ActiveSheet.AutoSortColumn(e.Column, true, false);
                        strSort = "D";
                    }
                    else
                    {
                        ssOrder.ActiveSheet.AutoSortColumn(e.Column, false, false);
                        strSort = "A";
                    }
                    return;
                }
            }
        }

        void Spd_AllSelect(FarPoint.Win.Spread.FpSpread spdNm, string sTF)
        {
            for (int i = 0; i < spdNm.ActiveSheet.NonEmptyRowCount; i++)
            {
                spdNm.ActiveSheet.Cells[i, 0].Value = sTF;
            }
        }

        private void btnAllSelect1_Click(object sender, EventArgs e)
        {
            Spd_AllSelect(ssOrder, "True");
        }

        private void btnAllSelect2_Click(object sender, EventArgs e)
        {
            Spd_AllSelect(ssConsult, "True");
        }

        private void btnAllSelect3_Click(object sender, EventArgs e)
        {
            Spd_AllSelect(ssCV, "True");
        }

        private void btnAllCancel1_Click(object sender, EventArgs e)
        {
            Spd_AllSelect(ssOrder, "False");
        }

        private void btnAllCancel2_Click(object sender, EventArgs e)
        {
            Spd_AllSelect(ssConsult, "False");
        }

        private void btnAllCancel3_Click(object sender, EventArgs e)
        {
            Spd_AllSelect(ssCV, "False");
        }
    }
}
