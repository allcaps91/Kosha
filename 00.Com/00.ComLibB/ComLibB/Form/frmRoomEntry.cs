using System;
using System.Data;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmRoomEntry
    /// File Name : frmRoomEntry.cs
    /// Title or Description : 병실 등록 폼
    /// 원무팀 이관
    /// Author : 박창욱
    /// Create Date : 2017-06-12
    /// Update Histroy : 
    /// </summary>
    /// <history>  
    /// VB\BuCode14.frm(FrmRoomEntry) -> frmRoomEntry.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bucode\BuCode14.frm(FrmRoomEntry)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\bucode\\bucode.vbp
    /// </vbp>

    public partial class frmRoomEntry : Form
    {
        private string gstrHelpCode = "";
        private string fstrPassGrade = "";
        private string fstrPassId = "";
        private string gnJobSabun = "";

        public delegate void SetHelpCode(string strHelpCode);
        public event SetHelpCode rSetHelpCode;

        public delegate void EventClose();
        public event EventClose rEventClose;

        public frmRoomEntry()
        {
            InitializeComponent();
        }

        public frmRoomEntry(string strHelpCode, string strPassGrade, string strPassId, string strJobSabun)
        {
            InitializeComponent();
            gstrHelpCode = strHelpCode;
            fstrPassGrade = strPassGrade;
            fstrPassId = strPassId;
            gnJobSabun = strJobSabun;
            
        }

        private void SCREEN_CLEAR()
        {
            lblMsg.Text = "";
            btnSearch.Enabled = true;
            btnSave.Enabled = false;
            btnCancel.Enabled = false;
            btnPrint.Enabled = false;

            ssView_Sheet1.RowCount = 50;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, ssView_Sheet1.ColumnCount - 1].Text = "";
            ssView.Enabled = false;


            //병동 Cell Type Change

            FarPoint.Win.Spread.CellType.TextCellType TextType = new FarPoint.Win.Spread.CellType.TextCellType();

            TextType.CharacterCasing = CharacterCasing.Upper;
            TextType.Multiline = false;
            TextType.MaxLength = 2;
            ssView_Sheet1.Cells[0, 0, ssView_Sheet1.RowCount - 1, 0].CellType = TextType;

            //호실 Cell Type Change
            TextType.MaxLength = 3;
            ssView_Sheet1.Cells[0, 1, ssView_Sheet1.RowCount - 1, 1].CellType = TextType;

            if (fstrPassGrade == "EDPS") //의료정보과
            {
                ssView.Enabled = true;
            }
            else
            {
                ssView.Enabled = false;
            }
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            if (fstrPassGrade.Trim() != "EDPS")
            {
                return;
            }
            if (fstrPassId != "4349")
            {
                return;
            }

            string strMSG = "";
            string strData = "";
            string strGB = "";
            string strJDate = "";
            string strClass = "";
            string nRoom = "";
            int i = 0;
            long nChaAmt = 0;
            long nBadAmt = 0;
            long nRoomP1 = 0;
            long nRoomP2 = 0;
            long nRoomP3 = 0;

            strJDate = "2012-01-15";
            nRoomP1 = 51290;  // AB2201 간호3 등급 일반 병실료
            nRoomP2 = 106660; // AJ240 중환자실 4등급 입원료
            nRoomP3 = 92940; // AJ201 신생아 4등급 집중치료료실

            nRoomP1 = VB.Fix((int)(nRoomP1 * 1.5 + 500) / 1000) * 1000;
            nRoomP2 = VB.Fix((int)(nRoomP2 * 1.5 + 500) / 1000) * 1000;
            nRoomP3 = VB.Fix((int)(nRoomP3 * 1.5 + 500) / 1000) * 1000;

            strMSG = "변경일자2의 내역을 변경일자3의 내역으로,  " + ComNum.VBLF;
            strMSG = strMSG + "변경일자1의 내역을 변경일자2의 내역으로  " + ComNum.VBLF;
            strMSG = strMSG + "이동을 합니다." + ComNum.VBLF + ComNum.VBLF;
            strMSG = strMSG + "    !!  작업을 하시겠습니까? !!  ";
            if (ComFunc.MsgBoxQ(strMSG, "확인") == DialogResult.No)
            {
                return;
            }
            for (i = 1; i < ssView_Sheet1.RowCount; i++)
            {
                //GoSub MOVE_RTN
                moveRtn(ref nBadAmt, ref nRoom, ref strGB, ref strClass, ref strData, ref strJDate,
                        ref nChaAmt, nRoomP1, nRoomP2, nRoomP3, i, 1);
            }
        }

        private void moveRtn(ref long nBadAmt, ref string nRoom, ref string strGB, ref string strClass,
                             ref string strData, ref string strJDate, ref long nChaAmt, long nRoomP1,
                             long nRoomP2, long nRoomP3, int i, int select1or2)
        {
            if (select1or2 == 1)
            {
                nBadAmt = (int)VB.Val(ssView_Sheet1.Cells[i - 1, 9].Text);
                nRoom = ssView_Sheet1.Cells[i - 1, 1].Text;
                strGB = ssView_Sheet1.Cells[i - 1, 6].Text;
                strClass = ssView_Sheet1.Cells[i - 1, 8].Text;

                if (nRoom == "811")
                {
                    return;
                }

                nChaAmt = (int)(VB.Val(ssView_Sheet1.Cells[i - 1, 10].Text));
                if (nChaAmt != 0)
                {
                    switch (strClass)
                    {
                        case "A":
                            nChaAmt = (int)VB.Val(nChaAmt.ToString()) + 7000;
                            break;
                        case "C":
                            if (nRoom == "613" || nRoom == "617")
                            {
                                nChaAmt = (int)VB.Val(nChaAmt.ToString()) + 4000;
                            }
                            else
                            {
                                nChaAmt = (int)VB.Val(nChaAmt.ToString()) + 2000;
                            }
                            break;
                        case "I":
                            nChaAmt = (int)VB.Val(nChaAmt.ToString()) + 2000;
                            break;
                        case "G":
                            nChaAmt = (int)VB.Val(nChaAmt.ToString()) + 4000;
                            break;
                        case "H":
                            nChaAmt = (int)VB.Val(nChaAmt.ToString()) + 4000;
                            break;
                        case "K":
                            nChaAmt = (int)VB.Val(nChaAmt.ToString()) + 4000;
                            break;
                    }
                }
            }
            else
            {
                nChaAmt = (int)VB.Val(ssView_Sheet1.Cells[i - 1, 10].Text);
                nBadAmt = (int)VB.Val(ssView_Sheet1.Cells[i - 1, 9].Text);
                nRoom = VB.Val(ssView_Sheet1.Cells[i - 1, 1].Text).ToString();
            }
            if (nBadAmt > 0)
            {
                //변경내역9 => 변경내역 10으로
                ssView_Sheet1.Cells[i - 1, 77].Text = ssView_Sheet1.Cells[i - 1, 69].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 78].Text = ssView_Sheet1.Cells[i - 1, 70].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 79].Text = ssView_Sheet1.Cells[i - 1, 71].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 80].Text = ssView_Sheet1.Cells[i - 1, 72].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 81].Text = ssView_Sheet1.Cells[i - 1, 73].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 82].Text = ssView_Sheet1.Cells[i - 1, 74].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 83].Text = ssView_Sheet1.Cells[i - 1, 75].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 84].Text = ssView_Sheet1.Cells[i - 1, 76].Text.Trim();

                //변경내역8 => 변경내역 9로
                ssView_Sheet1.Cells[i - 1, 69].Text = ssView_Sheet1.Cells[i - 1, 61].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 70].Text = ssView_Sheet1.Cells[i - 1, 62].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 71].Text = ssView_Sheet1.Cells[i - 1, 63].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 72].Text = ssView_Sheet1.Cells[i - 1, 64].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 73].Text = ssView_Sheet1.Cells[i - 1, 65].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 74].Text = ssView_Sheet1.Cells[i - 1, 66].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 75].Text = ssView_Sheet1.Cells[i - 1, 67].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 76].Text = ssView_Sheet1.Cells[i - 1, 68].Text.Trim();

                //변경내역7 => 변경내역 8로
                ssView_Sheet1.Cells[i - 1, 61].Text = ssView_Sheet1.Cells[i - 1, 53].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 62].Text = ssView_Sheet1.Cells[i - 1, 54].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 63].Text = ssView_Sheet1.Cells[i - 1, 55].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 64].Text = ssView_Sheet1.Cells[i - 1, 56].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 65].Text = ssView_Sheet1.Cells[i - 1, 57].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 66].Text = ssView_Sheet1.Cells[i - 1, 58].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 67].Text = ssView_Sheet1.Cells[i - 1, 59].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 68].Text = ssView_Sheet1.Cells[i - 1, 60].Text.Trim();

                //변경내역 6 => 변경내역 7로
                ssView_Sheet1.Cells[i - 1, 53].Text = ssView_Sheet1.Cells[i - 1, 45].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 54].Text = ssView_Sheet1.Cells[i - 1, 46].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 55].Text = ssView_Sheet1.Cells[i - 1, 47].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 56].Text = ssView_Sheet1.Cells[i - 1, 48].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 57].Text = ssView_Sheet1.Cells[i - 1, 49].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 58].Text = ssView_Sheet1.Cells[i - 1, 50].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 59].Text = ssView_Sheet1.Cells[i - 1, 51].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 60].Text = ssView_Sheet1.Cells[i - 1, 52].Text.Trim();

                //변경내역 5 => 변경내역 6으로
                ssView_Sheet1.Cells[i - 1, 45].Text = ssView_Sheet1.Cells[i - 1, 37].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 46].Text = ssView_Sheet1.Cells[i - 1, 38].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 47].Text = ssView_Sheet1.Cells[i - 1, 39].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 48].Text = ssView_Sheet1.Cells[i - 1, 40].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 49].Text = ssView_Sheet1.Cells[i - 1, 41].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 50].Text = ssView_Sheet1.Cells[i - 1, 42].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 51].Text = ssView_Sheet1.Cells[i - 1, 43].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 52].Text = ssView_Sheet1.Cells[i - 1, 44].Text.Trim();

                //변경내역 4 => 변경내역 5로
                ssView_Sheet1.Cells[i - 1, 37].Text = ssView_Sheet1.Cells[i - 1, 29].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 38].Text = ssView_Sheet1.Cells[i - 1, 30].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 39].Text = ssView_Sheet1.Cells[i - 1, 31].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 40].Text = ssView_Sheet1.Cells[i - 1, 32].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 41].Text = ssView_Sheet1.Cells[i - 1, 33].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 42].Text = ssView_Sheet1.Cells[i - 1, 34].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 43].Text = ssView_Sheet1.Cells[i - 1, 35].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 44].Text = ssView_Sheet1.Cells[i - 1, 36].Text.Trim();

                //변경내역 3 => 변경내역 4로
                ssView_Sheet1.Cells[i - 1, 29].Text = ssView_Sheet1.Cells[i - 1, 21].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 30].Text = ssView_Sheet1.Cells[i - 1, 22].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 31].Text = ssView_Sheet1.Cells[i - 1, 23].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 32].Text = ssView_Sheet1.Cells[i - 1, 24].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 33].Text = ssView_Sheet1.Cells[i - 1, 25].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 34].Text = ssView_Sheet1.Cells[i - 1, 26].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 35].Text = ssView_Sheet1.Cells[i - 1, 27].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 36].Text = ssView_Sheet1.Cells[i - 1, 28].Text.Trim();

                //변경내역2 => 변경내역 3으로
                ssView_Sheet1.Cells[i - 1, 21].Text = ssView_Sheet1.Cells[i - 1, 12].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 22].Text = ssView_Sheet1.Cells[i - 1, 13].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 23].Text = ssView_Sheet1.Cells[i - 1, 14].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 24].Text = ssView_Sheet1.Cells[i - 1, 15].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 25].Text = ssView_Sheet1.Cells[i - 1, 16].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 26].Text = ssView_Sheet1.Cells[i - 1, 17].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 27].Text = ssView_Sheet1.Cells[i - 1, 18].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 28].Text = ssView_Sheet1.Cells[i - 1, 19].Text.Trim();

                //변경내역1 => 변경내역 2로
                ssView_Sheet1.Cells[i - 1, 12].Text = ssView_Sheet1.Cells[i - 1, 3].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 13].Text = ssView_Sheet1.Cells[i - 1, 4].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 14].Text = ssView_Sheet1.Cells[i - 1, 5].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 15].Text = ssView_Sheet1.Cells[i - 1, 6].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 16].Text = ssView_Sheet1.Cells[i - 1, 7].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 17].Text = ssView_Sheet1.Cells[i - 1, 8].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 18].Text = ssView_Sheet1.Cells[i - 1, 9].Text.Trim();
                ssView_Sheet1.Cells[i - 1, 19].Text = ssView_Sheet1.Cells[i - 1, 10].Text.Trim();

                //변경일자1의 내역을 Clear
                strJDate = ssView_Sheet1.Cells[i - 1, 3].Text;

                if (select1or2 == 1)
                {
                    switch (strGB)
                    {
                        case "05":
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP2 + nChaAmt).ToString();
                            break;
                        case "06":
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP3 + nChaAmt).ToString();
                            break;
                        case "07":
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP3 + nChaAmt).ToString();
                            break;
                        default:
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP1 + nChaAmt).ToString();
                            break;
                    }

                    ssView_Sheet1.Cells[i - 1, 10].Text = nChaAmt.ToString();
                }
                else
                {
                    switch (strGB)
                    {
                        case "233":
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP2 + nChaAmt).ToString();
                            break;
                        case "234":
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP2 + nChaAmt).ToString();
                            break;
                        case "368":
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP3 + nChaAmt).ToString();
                            break;
                        case "369":
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP1 + 15000).ToString();
                            break;
                        default:
                            ssView_Sheet1.Cells[i - 1, 9].Text = (nRoomP1 + nChaAmt).ToString();
                            break;
                    }
                }
                ssView_Sheet1.Cells[i - 1, 86].Text = "Y";  //변경
            }
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            SCREEN_CLEAR();
            btnSearch.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (rEventClose != null)
            {
                rEventClose();
            }

            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) return; //권한 확인

            string strROWID = "";
            string strChange = "";
            string strWard = "";
            string strRoom = "";

            int nTBed1 = 0; int nTBed2 = 0; int nTBed3 = 0;
            long nOver1 = 0; long nOver2 = 0; long nOver3 = 0;
            long nIlban1 = 0; long nIlban2 = 0; long nIlban3 = 0;
            string strSex1 = ""; string strSex2 = ""; string strSex3 = "";
            string strDept1 = ""; string strDept2 = ""; string strDept3 = "";
            string strClass1 = ""; string strClass2 = ""; string strClass3 = "";
            string strTrDate1 = ""; string strTrDate2 = ""; string strTrDate3 = "";
            string strGbRoom1 = ""; string strGbRoom2 = ""; string strGbRoom3 = "";

            int nTBed4 = 0; int nTBed5 = 0; int nTBed6 = 0;
            long nOver4 = 0; long nOver5 = 0; long nOver6 = 0;
            long nIlban4 = 0; long nIlban5 = 0; long nIlban6 = 0;
            string strSex4 = ""; string strSex5 = ""; string strSex6 = "";
            string strDept4 = ""; string strDept5 = ""; string strDept6 = "";
            string strClass4 = ""; string strClass5 = ""; string strClass6 = "";
            string strTrDate4 = ""; string strTrDate5 = ""; string strTrDate6 = "";
            string strGbRoom4 = ""; string strGbRoom5 = ""; string strGbRoom6 = "";

            int nTBed7 = 0; int nTBed8 = 0; int nTBed9 = 0; int nTBed10 = 0;
            long nOver7 = 0; long nOver8 = 0; long nOver9 = 0; long nOver10 = 0;
            long nIlban7 = 0; long nIlban8 = 0; long nIlban9 = 0; long nIlban10 = 0;
            string strSex7 = ""; string strSex8 = ""; string strSex9 = ""; string strSex10 = "";
            string strDept7 = ""; string strDept8 = ""; string strDept9 = ""; string strDept10 = "";
            string strClass7 = ""; string strClass8 = ""; string strClass9 = ""; string strClass10 = "";
            string strTrDate7 = ""; string strTrDate8 = ""; string strTrDate9 = ""; string strTrDate10 = "";
            string strGbRoom7 = ""; string strGbRoom8 = ""; string strGbRoom9 = ""; string strGbRoom10 = "";

            int i = 0;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수
            int intRowAffected = 0; //변경된 Row 받는 변수

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);
            

            try
            {
                for (i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    strWard = ssView_Sheet1.Cells[i, 0].Text.Trim();
                    strRoom = ssView_Sheet1.Cells[i, 1].Text.Trim();
                    strROWID = ssView_Sheet1.Cells[i, 85].Text.Trim();
                    strChange = ssView_Sheet1.Cells[i, 86].Text.Trim();

                    if (strROWID != "" && strChange == "Y")
                    {
                        //GoSub Update_Rtn
                        #region Update_Rtn

                        //자료를 SET
                        strTrDate1 = ssView_Sheet1.Cells[i, 3].Text.Trim();
                        strSex1 = ssView_Sheet1.Cells[i, 4].Text.Trim();
                        strDept1 = ssView_Sheet1.Cells[i, 5].Text.Trim();
                        strGbRoom1 = ssView_Sheet1.Cells[i, 6].Text.Trim();
                        nTBed1 = (int)(VB.Val(ssView_Sheet1.Cells[i, 7].Text.Trim()));
                        strClass1 = ssView_Sheet1.Cells[i, 8].Text.Trim();
                        nIlban1 = (int)(VB.Val(ssView_Sheet1.Cells[i, 9].Text.Trim()));
                        nOver1 = (int)(VB.Val(ssView_Sheet1.Cells[i, 10].Text.Trim()));

                        strTrDate2 = ssView_Sheet1.Cells[i, 12].Text.Trim();
                        strSex2 = ssView_Sheet1.Cells[i, 13].Text.Trim();
                        strDept2 = ssView_Sheet1.Cells[i, 14].Text.Trim();
                        strGbRoom2 = ssView_Sheet1.Cells[i, 15].Text.Trim();
                        nTBed2 = (int)(VB.Val(ssView_Sheet1.Cells[i, 16].Text.Trim()));
                        strClass2 = ssView_Sheet1.Cells[i, 17].Text.Trim();
                        nIlban2 = (int)(VB.Val(ssView_Sheet1.Cells[i, 18].Text.Trim()));
                        nOver2 = (int)(VB.Val(ssView_Sheet1.Cells[i, 19].Text.Trim()));

                        strTrDate3 = ssView_Sheet1.Cells[i, 21].Text.Trim();
                        strSex3 = ssView_Sheet1.Cells[i, 22].Text.Trim();
                        strDept3 = ssView_Sheet1.Cells[i, 23].Text.Trim();
                        strGbRoom3 = ssView_Sheet1.Cells[i, 24].Text.Trim();
                        nTBed3 = (int)(VB.Val(ssView_Sheet1.Cells[i, 25].Text.Trim()));
                        strClass3 = ssView_Sheet1.Cells[i, 26].Text.Trim();
                        nIlban3 = (int)(VB.Val(ssView_Sheet1.Cells[i, 27].Text.Trim()));
                        nOver3 = (int)(VB.Val(ssView_Sheet1.Cells[i, 28].Text.Trim()));

                        strTrDate4 = ssView_Sheet1.Cells[i, 29].Text.Trim();
                        strSex4 = ssView_Sheet1.Cells[i, 30].Text.Trim();
                        strDept4 = ssView_Sheet1.Cells[i, 31].Text.Trim();
                        strGbRoom4 = ssView_Sheet1.Cells[i, 32].Text.Trim();
                        nTBed4 = (int)(VB.Val(ssView_Sheet1.Cells[i, 33].Text.Trim()));
                        strClass4 = ssView_Sheet1.Cells[i, 34].Text.Trim();
                        nIlban4 = (int)(VB.Val(ssView_Sheet1.Cells[i, 35].Text.Trim()));
                        nOver4 = (int)(VB.Val(ssView_Sheet1.Cells[i, 36].Text.Trim()));

                        strTrDate5 = ssView_Sheet1.Cells[i, 37].Text.Trim();
                        strSex5 = ssView_Sheet1.Cells[i, 38].Text.Trim();
                        strDept5 = ssView_Sheet1.Cells[i, 39].Text.Trim();
                        strGbRoom5 = ssView_Sheet1.Cells[i, 40].Text.Trim();
                        nTBed5 = (int)(VB.Val(ssView_Sheet1.Cells[i, 41].Text.Trim()));
                        strClass5 = ssView_Sheet1.Cells[i, 42].Text.Trim();
                        nIlban5 = (int)(VB.Val(ssView_Sheet1.Cells[i, 43].Text.Trim()));
                        nOver5 = (int)(VB.Val(ssView_Sheet1.Cells[i, 44].Text.Trim()));

                        strTrDate6 = ssView_Sheet1.Cells[i, 45].Text.Trim();
                        strSex6 = ssView_Sheet1.Cells[i, 46].Text.Trim();
                        strDept6 = ssView_Sheet1.Cells[i, 47].Text.Trim();
                        strGbRoom6 = ssView_Sheet1.Cells[i, 48].Text.Trim();
                        nTBed6 = (int)(VB.Val(ssView_Sheet1.Cells[i, 49].Text.Trim()));
                        strClass6 = ssView_Sheet1.Cells[i, 50].Text.Trim();
                        nIlban6 = (int)(VB.Val(ssView_Sheet1.Cells[i, 51].Text.Trim()));
                        nOver6 = (int)(VB.Val(ssView_Sheet1.Cells[i, 52].Text.Trim()));

                        strTrDate7 = ssView_Sheet1.Cells[i, 53].Text.Trim();
                        strSex7 = ssView_Sheet1.Cells[i, 54].Text.Trim();
                        strDept7 = ssView_Sheet1.Cells[i, 55].Text.Trim();
                        strGbRoom7 = ssView_Sheet1.Cells[i, 56].Text.Trim();
                        nTBed7 = (int)(VB.Val(ssView_Sheet1.Cells[i, 57].Text.Trim()));
                        strClass7 = ssView_Sheet1.Cells[i, 58].Text.Trim();
                        nIlban7 = (int)(VB.Val(ssView_Sheet1.Cells[i, 59].Text.Trim()));
                        nOver7 = (int)(VB.Val(ssView_Sheet1.Cells[i, 60].Text.Trim()));

                        strTrDate8 = ssView_Sheet1.Cells[i, 61].Text.Trim();
                        strSex8 = ssView_Sheet1.Cells[i, 62].Text.Trim();
                        strDept8 = ssView_Sheet1.Cells[i, 63].Text.Trim();
                        strGbRoom8 = ssView_Sheet1.Cells[i, 64].Text.Trim();
                        nTBed8 = (int)(VB.Val(ssView_Sheet1.Cells[i, 65].Text.Trim()));
                        strClass8 = ssView_Sheet1.Cells[i, 66].Text.Trim();
                        nIlban8 = (int)(VB.Val(ssView_Sheet1.Cells[i, 67].Text.Trim()));
                        nOver8 = (int)(VB.Val(ssView_Sheet1.Cells[i, 68].Text.Trim()));

                        strTrDate9 = ssView_Sheet1.Cells[i, 69].Text.Trim();
                        strSex9 = ssView_Sheet1.Cells[i, 70].Text.Trim();
                        strDept9 = ssView_Sheet1.Cells[i, 71].Text.Trim();
                        strGbRoom9 = ssView_Sheet1.Cells[i, 72].Text.Trim();
                        nTBed9 = (int)(VB.Val(ssView_Sheet1.Cells[i, 73].Text.Trim()));
                        strClass9 = ssView_Sheet1.Cells[i, 74].Text.Trim();
                        nIlban9 = (int)(VB.Val(ssView_Sheet1.Cells[i, 75].Text.Trim()));
                        nOver9 = (int)(VB.Val(ssView_Sheet1.Cells[i, 76].Text.Trim()));

                        strTrDate10 = ssView_Sheet1.Cells[i, 77].Text.Trim();
                        strSex10 = ssView_Sheet1.Cells[i, 78].Text.Trim();
                        strDept10 = ssView_Sheet1.Cells[i, 79].Text.Trim();
                        strGbRoom10 = ssView_Sheet1.Cells[i, 80].Text.Trim();
                        nTBed10 = (int)(VB.Val(ssView_Sheet1.Cells[i, 81].Text.Trim()));
                        strClass10 = ssView_Sheet1.Cells[i, 82].Text.Trim();
                        nIlban10 = (int)(VB.Val(ssView_Sheet1.Cells[i, 83].Text.Trim()));
                        nOver10 = (int)(VB.Val(ssView_Sheet1.Cells[i, 84].Text.Trim()));



                        //변경 등록
                        SQL = "";
                        SQL = "UPDATE BAS_ROOM SET ";
                        SQL = SQL + ComNum.VBLF + " TransDate1=TO_DATE('" + strTrDate1 + "','YYYY-MM-DD'),   Sex ='" + strSex1 + "', DeptCode ='" + strDept1 + "', TBed =" + nTBed1 + ", RoomClass ='" + strClass1 + "', WardAmt  =" + nIlban1 + ", OverAmt =" + nOver1 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate2=TO_DATE('" + strTrDate2 + "','YYYY-MM-DD'),   Sex1='" + strSex2 + "', DeptCode1='" + strDept2 + "', TBed1=" + nTBed2 + ", RoomClass1='" + strClass2 + "', IlbanAmt1=" + nIlban2 + ", OverAmt1=" + nOver2 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate3=TO_DATE('" + strTrDate3 + "','YYYY-MM-DD'),   Sex2='" + strSex3 + "', DeptCode2='" + strDept3 + "', TBed2=" + nTBed3 + ", RoomClass2='" + strClass3 + "', IlbanAmt2=" + nIlban3 + ", OverAmt2=" + nOver3 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate4=TO_DATE('" + strTrDate4 + "','YYYY-MM-DD'),   Sex3='" + strSex4 + "', DeptCode3='" + strDept4 + "', TBed3=" + nTBed4 + ", RoomClass3='" + strClass4 + "', IlbanAmt3=" + nIlban4 + ", OverAmt3=" + nOver4 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate5=TO_DATE('" + strTrDate5 + "','YYYY-MM-DD'),   Sex4='" + strSex5 + "', DeptCode4='" + strDept5 + "', TBed4=" + nTBed5 + ", RoomClass4='" + strClass5 + "', IlbanAmt4=" + nIlban5 + ", OverAmt4=" + nOver5 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate6=TO_DATE('" + strTrDate6 + "','YYYY-MM-DD'),   Sex5='" + strSex6 + "', DeptCode5='" + strDept6 + "', TBed5=" + nTBed6 + ", RoomClass5='" + strClass6 + "', IlbanAmt5=" + nIlban6 + ", OverAmt5=" + nOver6 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate7=TO_DATE('" + strTrDate7 + "','YYYY-MM-DD'),   Sex6='" + strSex7 + "', DeptCode6='" + strDept7 + "', TBed6=" + nTBed7 + ", RoomClass6='" + strClass7 + "', IlbanAmt6=" + nIlban7 + ", OverAmt6=" + nOver7 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate8=TO_DATE('" + strTrDate8 + "','YYYY-MM-DD'),   Sex7='" + strSex8 + "', DeptCode7='" + strDept8 + "', TBed7=" + nTBed8 + ", RoomClass7='" + strClass8 + "', IlbanAmt7=" + nIlban8 + ", OverAmt7=" + nOver8 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate9=TO_DATE('" + strTrDate9 + "','YYYY-MM-DD'),   Sex8='" + strSex9 + "', DeptCode8='" + strDept9 + "', TBed8=" + nTBed9 + ", RoomClass8='" + strClass9 + "', IlbanAmt8=" + nIlban9 + ", OverAmt8=" + nOver9 + ",";
                        SQL = SQL + ComNum.VBLF + " TransDate10=TO_DATE('" + strTrDate10 + "','YYYY-MM-DD'), Sex9='" + strSex10 + "',DeptCode9='" + strDept10 + "',TBed9=" + nTBed10 + ",RoomClass9='" + strClass10 + "',IlbanAmt9=" + nIlban10 + ",OverAmt9=" + nOver10 + ",";
                        SQL = SQL + ComNum.VBLF + " GbRoom='" + strGbRoom1 + "',GbRoom1='" + strGbRoom2 + "',GbRoom2='" + strGbRoom3 + "',";
                        SQL = SQL + ComNum.VBLF + " GbRoom3='" + strGbRoom4 + "', ";
                        SQL = SQL + ComNum.VBLF + " GbRoom4='" + strGbRoom5 + "', ";
                        SQL = SQL + ComNum.VBLF + " GbRoom5='" + strGbRoom6 + "', ";
                        SQL = SQL + ComNum.VBLF + " GbRoom6='" + strGbRoom7 + "', ";
                        SQL = SQL + ComNum.VBLF + " GbRoom7='" + strGbRoom8 + "', ";
                        SQL = SQL + ComNum.VBLF + " GbRoom8='" + strGbRoom9 + "', ";
                        SQL = SQL + ComNum.VBLF + " GbRoom9='" + strGbRoom10 + "'";
                        SQL = SQL + ComNum.VBLF + "WHERE ROWID = '" + strROWID + "' ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        #endregion
                    }
                    else if (strROWID == "" && strWard != "" && strRoom != "")
                    {
                        //GoSub INSERT_Rtn
                        #region INSERT_Rtn

                        //자료를 SET
                        strTrDate1 = ssView_Sheet1.Cells[i, 3].Text.Trim();
                        strSex1 = ssView_Sheet1.Cells[i, 4].Text.Trim();
                        strDept1 = ssView_Sheet1.Cells[i, 5].Text.Trim();
                        strGbRoom1 = ssView_Sheet1.Cells[i, 6].Text.Trim();
                        nTBed1 = (int)(VB.Val(ssView_Sheet1.Cells[i, 7].Text.Trim()));
                        strClass1 = ssView_Sheet1.Cells[i, 8].Text.Trim();
                        nIlban1 = (int)(VB.Val(ssView_Sheet1.Cells[i, 9].Text.Trim()));
                        nOver1 = (int)(VB.Val(ssView_Sheet1.Cells[i, 10].Text.Trim()));

                        strTrDate2 = ssView_Sheet1.Cells[i, 12].Text.Trim();
                        strSex2 = ssView_Sheet1.Cells[i, 13].Text.Trim();
                        strDept2 = ssView_Sheet1.Cells[i, 14].Text.Trim();
                        strGbRoom2 = ssView_Sheet1.Cells[i, 15].Text.Trim();
                        nTBed2 = (int)(VB.Val(ssView_Sheet1.Cells[i, 16].Text.Trim()));
                        strClass2 = ssView_Sheet1.Cells[i, 17].Text.Trim();
                        nIlban2 = (int)(VB.Val(ssView_Sheet1.Cells[i, 18].Text.Trim()));
                        nOver2 = (int)(VB.Val(ssView_Sheet1.Cells[i, 19].Text.Trim()));

                        strTrDate3 = ssView_Sheet1.Cells[i, 21].Text.Trim();
                        strSex3 = ssView_Sheet1.Cells[i, 22].Text.Trim();
                        strDept3 = ssView_Sheet1.Cells[i, 23].Text.Trim();
                        strGbRoom3 = ssView_Sheet1.Cells[i, 24].Text.Trim();
                        nTBed3 = (int)(VB.Val(ssView_Sheet1.Cells[i, 25].Text.Trim()));
                        strClass3 = ssView_Sheet1.Cells[i, 26].Text.Trim();
                        nIlban3 = (int)(VB.Val(ssView_Sheet1.Cells[i, 27].Text.Trim()));
                        nOver3 = (int)(VB.Val(ssView_Sheet1.Cells[i, 28].Text.Trim()));

                        strTrDate4 = ssView_Sheet1.Cells[i, 29].Text.Trim();
                        strSex4 = ssView_Sheet1.Cells[i, 30].Text.Trim();
                        strDept4 = ssView_Sheet1.Cells[i, 31].Text.Trim();
                        strGbRoom4 = ssView_Sheet1.Cells[i, 32].Text.Trim();
                        nTBed4 = (int)(VB.Val(ssView_Sheet1.Cells[i, 33].Text.Trim()));
                        strClass4 = ssView_Sheet1.Cells[i, 34].Text.Trim();
                        nIlban4 = (int)(VB.Val(ssView_Sheet1.Cells[i, 35].Text.Trim()));
                        nOver4 = (int)(VB.Val(ssView_Sheet1.Cells[i, 36].Text.Trim()));

                        strTrDate5 = ssView_Sheet1.Cells[i, 37].Text.Trim();
                        strSex5 = ssView_Sheet1.Cells[i, 38].Text.Trim();
                        strDept5 = ssView_Sheet1.Cells[i, 39].Text.Trim();
                        strGbRoom5 = ssView_Sheet1.Cells[i, 40].Text.Trim();
                        nTBed5 = (int)(VB.Val(ssView_Sheet1.Cells[i, 41].Text.Trim()));
                        strClass5 = ssView_Sheet1.Cells[i, 42].Text.Trim();
                        nIlban5 = (int)(VB.Val(ssView_Sheet1.Cells[i, 43].Text.Trim()));
                        nOver5 = (int)(VB.Val(ssView_Sheet1.Cells[i, 44].Text.Trim()));

                        strTrDate6 = ssView_Sheet1.Cells[i, 45].Text.Trim();
                        strSex6 = ssView_Sheet1.Cells[i, 46].Text.Trim();
                        strDept6 = ssView_Sheet1.Cells[i, 47].Text.Trim();
                        strGbRoom6 = ssView_Sheet1.Cells[i, 48].Text.Trim();
                        nTBed6 = (int)(VB.Val(ssView_Sheet1.Cells[i, 49].Text.Trim()));
                        strClass6 = ssView_Sheet1.Cells[i, 50].Text.Trim();
                        nIlban6 = (int)(VB.Val(ssView_Sheet1.Cells[i, 51].Text.Trim()));
                        nOver6 = (int)(VB.Val(ssView_Sheet1.Cells[i, 52].Text.Trim()));

                        strTrDate7 = ssView_Sheet1.Cells[i, 53].Text.Trim();
                        strSex7 = ssView_Sheet1.Cells[i, 54].Text.Trim();
                        strDept7 = ssView_Sheet1.Cells[i, 55].Text.Trim();
                        strGbRoom7 = ssView_Sheet1.Cells[i, 56].Text.Trim();
                        nTBed7 = (int)(VB.Val(ssView_Sheet1.Cells[i, 57].Text.Trim()));
                        strClass7 = ssView_Sheet1.Cells[i, 58].Text.Trim();
                        nIlban7 = (int)(VB.Val(ssView_Sheet1.Cells[i, 59].Text.Trim()));
                        nOver7 = (int)(VB.Val(ssView_Sheet1.Cells[i, 60].Text.Trim()));

                        strTrDate8 = ssView_Sheet1.Cells[i, 61].Text.Trim();
                        strSex8 = ssView_Sheet1.Cells[i, 62].Text.Trim();
                        strDept8 = ssView_Sheet1.Cells[i, 63].Text.Trim();
                        strGbRoom8 = ssView_Sheet1.Cells[i, 64].Text.Trim();
                        nTBed8 = (int)(VB.Val(ssView_Sheet1.Cells[i, 65].Text.Trim()));
                        strClass8 = ssView_Sheet1.Cells[i, 66].Text.Trim();
                        nIlban8 = (int)(VB.Val(ssView_Sheet1.Cells[i, 67].Text.Trim()));
                        nOver8 = (int)(VB.Val(ssView_Sheet1.Cells[i, 68].Text.Trim()));

                        strTrDate9 = ssView_Sheet1.Cells[i, 69].Text.Trim();
                        strSex9 = ssView_Sheet1.Cells[i, 70].Text.Trim();
                        strDept9 = ssView_Sheet1.Cells[i, 71].Text.Trim();
                        strGbRoom9 = ssView_Sheet1.Cells[i, 72].Text.Trim();
                        nTBed9 = (int)(VB.Val(ssView_Sheet1.Cells[i, 73].Text.Trim()));
                        strClass9 = ssView_Sheet1.Cells[i, 74].Text.Trim();
                        nIlban9 = (int)(VB.Val(ssView_Sheet1.Cells[i, 75].Text.Trim()));
                        nOver9 = (int)(VB.Val(ssView_Sheet1.Cells[i, 76].Text.Trim()));

                        strTrDate10 = ssView_Sheet1.Cells[i, 77].Text.Trim();
                        strSex10 = ssView_Sheet1.Cells[i, 78].Text.Trim();
                        strDept10 = ssView_Sheet1.Cells[i, 79].Text.Trim();
                        strGbRoom10 = ssView_Sheet1.Cells[i, 80].Text.Trim();
                        nTBed10 = (int)(VB.Val(ssView_Sheet1.Cells[i, 81].Text.Trim()));
                        strClass10 = ssView_Sheet1.Cells[i, 82].Text.Trim();
                        nIlban10 = (int)(VB.Val(ssView_Sheet1.Cells[i, 83].Text.Trim()));
                        nOver10 = (int)(VB.Val(ssView_Sheet1.Cells[i, 84].Text.Trim()));

                        //신규 등록
                        SQL = "";
                        SQL = "INSERT INTO BAS_ROOM (WARDCODE,ROOMCODE,HBED,GBED,BBED,PANTAMT,RONDAMT,FOODAMT,";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE1, SEX, DEPTCODE, TBED, ROOMCLASS, WARDAMT,  OVERAMT,";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE2, SEX1,DEPTCODE1,TBED1,ROOMCLASS1,ILBANAMT1,OVERAMT1,";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE3, SEX2,DEPTCODE2,TBED2,ROOMCLASS2,ILBANAMT2,OVERAMT2, ";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE4, SEX3,DEPTCODE3,TBED3,ROOMCLASS3,ILBANAMT3,OVERAMT3, ";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE5, SEX4,DEPTCODE4,TBED4,ROOMCLASS4,ILBANAMT4,OVERAMT4, ";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE6, SEX5,DEPTCODE5,TBED5,ROOMCLASS5,ILBANAMT5,OVERAMT5, ";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE7, SEX6,DEPTCODE6,TBED6,ROOMCLASS6,ILBANAMT6,OVERAMT6, ";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE8, SEX7,DEPTCODE7,TBED7,ROOMCLASS7,ILBANAMT7,OVERAMT7, ";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE9, SEX8,DEPTCODE8,TBED8,ROOMCLASS8,ILBANAMT8,OVERAMT8, ";
                        SQL = SQL + ComNum.VBLF + " TRANSDATE10,SEX9,DEPTCODE9,TBED9,ROOMCLASS9,ILBANAMT9,OVERAMT9, ";

                        SQL = SQL + ComNum.VBLF + " GBROOM, GBROOM1, GBROOM2, GBROOM3, GBROOM4, GBROOM5, GBROOM6, GBROOM7, GBROOM8, GBROOM9  ) ";
                        SQL = SQL + ComNum.VBLF + " VALUES('" + strWard + "'," + strRoom + ",0,0,0,0,0,0,";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate1 + "','YYYY-MM-DD'), '" + strSex1 + "', '" + strDept1 + "', " + nTBed1 + ", '" + strClass1 + "', " + nIlban1 + ", " + nOver1 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate2 + "','YYYY-MM-DD'), '" + strSex2 + "', '" + strDept2 + "', " + nTBed2 + ", '" + strClass2 + "', " + nIlban2 + ", " + nOver2 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate3 + "','YYYY-MM-DD'), '" + strSex3 + "', '" + strDept3 + "', " + nTBed3 + ", '" + strClass3 + "', " + nIlban3 + ", " + nOver3 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate4 + "','YYYY-MM-DD'), '" + strSex4 + "', '" + strDept4 + "', " + nTBed4 + ", '" + strClass4 + "', " + nIlban4 + ", " + nOver4 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate5 + "','YYYY-MM-DD'), '" + strSex5 + "', '" + strDept5 + "', " + nTBed5 + ", '" + strClass5 + "', " + nIlban5 + ", " + nOver5 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate6 + "','YYYY-MM-DD'), '" + strSex6 + "', '" + strDept6 + "', " + nTBed6 + ", '" + strClass6 + "', " + nIlban6 + ", " + nOver6 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate7 + "','YYYY-MM-DD'), '" + strSex7 + "', '" + strDept7 + "', " + nTBed7 + ", '" + strClass7 + "', " + nIlban7 + ", " + nOver7 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate8 + "','YYYY-MM-DD'), '" + strSex8 + "', '" + strDept8 + "', " + nTBed8 + ", '" + strClass8 + "', " + nIlban8 + ", " + nOver8 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate9 + "','YYYY-MM-DD'), '" + strSex9 + "', '" + strDept9 + "', " + nTBed9 + ", '" + strClass9 + "', " + nIlban9 + ", " + nOver9 + ",";
                        SQL = SQL + ComNum.VBLF + "TO_DATE('" + strTrDate10 + "','YYYY-MM-DD'),'" + strSex10 + "','" + strDept10 + "'," + nTBed10 + ",'" + strClass10 + "'," + nIlban10 + "," + nOver10 + ",";

                        SQL = SQL + ComNum.VBLF + " '" + strGbRoom1 + "','" + strGbRoom2 + "','" + strGbRoom3 + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strGbRoom4 + "','" + strGbRoom5 + "','" + strGbRoom6 + "', ";
                        SQL = SQL + ComNum.VBLF + " '" + strGbRoom7 + "','" + strGbRoom8 + "','" + strGbRoom9 + "', '" + strGbRoom10 + "' ";
                        SQL = SQL + ComNum.VBLF + " ) ";
                        SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowAffected, clsDB.DbCon);
                        if (SqlErr != "")
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            ComFunc.MsgBox(SqlErr);
                            clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                            Cursor.Current = Cursors.Default;
                            return;
                        }

                        #endregion
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);
                ComFunc.MsgBox("저장하였습니다.");
                Cursor.Current = Cursors.Default;

                SCREEN_CLEAR();
                btnSearch.Focus();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
            }

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";
            string strHead1 = "";
            string strHead2 = "";
            int i = 0;


            for (i = 29; i < 85; i++)
            {
                ssView_Sheet1.Columns[i].Visible = false;
            }

            //Print Head 지정
            strFont1 = "/fn\"굴림체\" /fz\"16\" /fb1 /fi0 /fu0 /fk0 /fs1";
            strFont2 = "/fn\"굴림체\" /fz\"11\" /fb1 /fi0 /fu0 /fk0 /fs2";
            strHead1 = "/l/f1" + VB.Space(40) + "병 동 별  병 실  내 역";
            strHead2 = "/l/f2" + "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + VB.Space(107) + "PAGE : /p";

            ssView_Sheet1.PrintInfo.Header = strFont1 + strHead1 + "/n" + strFont2 + strHead2;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 5;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 200;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = false;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = true;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            for (i = 29; i < 85; i++)
            {
                ssView_Sheet1.Columns[i].Visible = false;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int nRead = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            btnSearch.Enabled = false;

            try
            {

                //해당 자료를 READ
                SQL = "";
                SQL = "     SELECT ROWID,WardCode,RoomCode,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate1,'YYYY-MM-DD') TrDate1,Sex,DeptCode,TBed,RoomClass,WardAmt,OverAmt,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate2,'YYYY-MM-DD') TrDate2,Sex1,DeptCode1,TBed1,RoomClass1,IlbanAmt1,OverAmt1,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate3,'YYYY-MM-DD') TrDate3,Sex2,DeptCode2,TBed2,RoomClass2,IlbanAmt2,OverAmt2,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate4,'YYYY-MM-DD') TrDate4,Sex3,DeptCode3,TBed3,RoomClass3,IlbanAmt3,OverAmt3,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate5,'YYYY-MM-DD') TrDate5,Sex4,DeptCode4,TBed4,RoomClass4,IlbanAmt4,OverAmt4,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate6,'YYYY-MM-DD') TrDate6,Sex5,DeptCode5,TBed5,RoomClass5,IlbanAmt5,OverAmt5,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate7,'YYYY-MM-DD') TrDate7,Sex6,DeptCode6,TBed6,RoomClass6,IlbanAmt6,OverAmt6,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate8,'YYYY-MM-DD') TrDate8,Sex7,DeptCode7,TBed7,RoomClass7,IlbanAmt7,OverAmt7,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate9,'YYYY-MM-DD') TrDate9,Sex8,DeptCode8,TBed8,RoomClass8,IlbanAmt8,OverAmt8,";
                SQL = SQL + ComNum.VBLF + "      TO_CHAR(TransDate10,'YYYY-MM-DD') TrDate10,Sex9,DeptCode9,TBed9,RoomClass9,IlbanAmt9,OverAmt9,";

                SQL = SQL + ComNum.VBLF + "      GbRoom,GbRoom1,GbRoom2, GbRoom3,GbRoom4,GbRoom5,  GbRoom6,GbRoom7,GbRoom8, GbRoom9";
                SQL = SQL + ComNum.VBLF + " FROM BAS_ROOM ";

                if (cboWard.Text != "전체")
                {
                    SQL = SQL + ComNum.VBLF + "WHERE WardCode='" + cboWard.Text + "' ";
                    if (chkNotUse.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + " AND TBed > 0 ";
                    }
                }
                else
                {
                    if (chkNotUse.Checked == true)
                    {
                        SQL = SQL + ComNum.VBLF + "WHERE TBed > 0 ";
                    }
                }
                if (VB.Left(cboGrade.Text, 1) != "*")
                {
                    SQL = SQL + ComNum.VBLF + " AND ROOMCLASS = '" + VB.Left(cboGrade.Text, 1) + "' ";
                }
                SQL = SQL + ComNum.VBLF + "ORDER BY WardCode,RoomCode ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    btnSearch.Enabled = true;
                    return;
                }
                nRead = dt.Rows.Count;
                ssView_Sheet1.RowCount = nRead + 20;
                ssView_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    ssView_Sheet1.Cells[i, 0].Text = dt.Rows[i]["WardCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 1].Text = dt.Rows[i]["RoomCode"].ToString().Trim();
                    //변경 1
                    ssView_Sheet1.Cells[i, 3].Text = dt.Rows[i]["TrDate1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 4].Text = dt.Rows[i]["Sex"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 5].Text = dt.Rows[i]["DeptCode"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 6].Text = dt.Rows[i]["GbRoom"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 7].Text = dt.Rows[i]["TBed"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 8].Text = dt.Rows[i]["RoomClass"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 9].Text = dt.Rows[i]["WardAmt"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 10].Text = dt.Rows[i]["OverAmt"].ToString().Trim();
                    //변경 2
                    ssView_Sheet1.Cells[i, 12].Text = dt.Rows[i]["TrDate2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 13].Text = dt.Rows[i]["Sex1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 14].Text = dt.Rows[i]["DeptCode1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 15].Text = dt.Rows[i]["GbRoom1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 16].Text = dt.Rows[i]["TBed1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 17].Text = dt.Rows[i]["RoomClass1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 18].Text = dt.Rows[i]["IlbanAmt1"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 19].Text = dt.Rows[i]["OverAmt1"].ToString().Trim();
                    //변경 3
                    ssView_Sheet1.Cells[i, 21].Text = dt.Rows[i]["TrDate3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 22].Text = dt.Rows[i]["Sex2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 23].Text = dt.Rows[i]["DeptCode2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 24].Text = dt.Rows[i]["GbRoom2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 25].Text = dt.Rows[i]["TBed2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 26].Text = dt.Rows[i]["RoomClass2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 27].Text = dt.Rows[i]["IlbanAmt2"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 28].Text = dt.Rows[i]["OverAmt2"].ToString().Trim();
                    //변경 4
                    ssView_Sheet1.Cells[i, 29].Text = dt.Rows[i]["TrDate4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 30].Text = dt.Rows[i]["Sex3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 31].Text = dt.Rows[i]["DeptCode3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 32].Text = dt.Rows[i]["GbRoom3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 33].Text = dt.Rows[i]["TBed3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 34].Text = dt.Rows[i]["RoomClass3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 35].Text = dt.Rows[i]["IlbanAmt3"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 36].Text = dt.Rows[i]["OverAmt3"].ToString().Trim();
                    //변경 5
                    ssView_Sheet1.Cells[i, 37].Text = dt.Rows[i]["TrDate5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 38].Text = dt.Rows[i]["Sex4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 39].Text = dt.Rows[i]["DeptCode4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 40].Text = dt.Rows[i]["GbRoom4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 41].Text = dt.Rows[i]["TBed4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 42].Text = dt.Rows[i]["RoomClass4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 43].Text = dt.Rows[i]["IlbanAmt4"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 44].Text = dt.Rows[i]["OverAmt4"].ToString().Trim();
                    //변경 6
                    ssView_Sheet1.Cells[i, 45].Text = dt.Rows[i]["TrDate6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 46].Text = dt.Rows[i]["Sex5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 47].Text = dt.Rows[i]["DeptCode5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 48].Text = dt.Rows[i]["GbRoom5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 49].Text = dt.Rows[i]["TBed5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 50].Text = dt.Rows[i]["RoomClass5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 51].Text = dt.Rows[i]["IlbanAmt5"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 52].Text = dt.Rows[i]["OverAmt5"].ToString().Trim();
                    //변경 7
                    ssView_Sheet1.Cells[i, 53].Text = dt.Rows[i]["TrDate7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 54].Text = dt.Rows[i]["Sex6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 55].Text = dt.Rows[i]["DeptCode6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 56].Text = dt.Rows[i]["GbRoom6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 57].Text = dt.Rows[i]["TBed6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 58].Text = dt.Rows[i]["RoomClass6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 59].Text = dt.Rows[i]["IlbanAmt6"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 60].Text = dt.Rows[i]["OverAmt6"].ToString().Trim();
                    //변경 8
                    ssView_Sheet1.Cells[i, 61].Text = dt.Rows[i]["TrDate8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 62].Text = dt.Rows[i]["Sex7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 63].Text = dt.Rows[i]["DeptCode7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 64].Text = dt.Rows[i]["GbRoom7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 65].Text = dt.Rows[i]["TBed7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 66].Text = dt.Rows[i]["RoomClass7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 67].Text = dt.Rows[i]["IlbanAmt7"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 68].Text = dt.Rows[i]["OverAmt7"].ToString().Trim();
                    //변경 9
                    ssView_Sheet1.Cells[i, 69].Text = dt.Rows[i]["TrDate9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 70].Text = dt.Rows[i]["Sex8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 71].Text = dt.Rows[i]["DeptCode8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 72].Text = dt.Rows[i]["GbRoom8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 73].Text = dt.Rows[i]["TBed8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 74].Text = dt.Rows[i]["RoomClass8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 75].Text = dt.Rows[i]["IlbanAmt8"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 76].Text = dt.Rows[i]["OverAmt8"].ToString().Trim();
                    //변경 10
                    ssView_Sheet1.Cells[i, 77].Text = dt.Rows[i]["TrDate10"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 78].Text = dt.Rows[i]["Sex9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 79].Text = dt.Rows[i]["DeptCode9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 80].Text = dt.Rows[i]["GbRoom9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 81].Text = dt.Rows[i]["TBed9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 82].Text = dt.Rows[i]["RoomClass9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 83].Text = dt.Rows[i]["IlbanAmt9"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 84].Text = dt.Rows[i]["OverAmt9"].ToString().Trim();
                    //ROWID 변경 여부
                    ssView_Sheet1.Cells[i, 85].Text = dt.Rows[i]["ROWID"].ToString().Trim();
                    ssView_Sheet1.Cells[i, 86].Text = "";
                }

                dt.Dispose();
                dt = null;
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                btnSearch.Enabled = true;
            }

            //병동, 호실 Cell Type Change
            FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            textCellType.HotkeyPrefix = System.Drawing.Text.HotkeyPrefix.Hide;
            textCellType.WordWrap = false;

            ssView_Sheet1.Cells[0, 0, nRead - 1, 1].CellType = textCellType;

            if (fstrPassGrade.Trim() == "EDPS")
            {
                btnSave.Enabled = true;
            }
            ssView.Enabled = true;
            btnCancel.Enabled = true;
            btnPrint.Enabled = true;
            ssView.Focus();
        }

        private void btnWardRoom_Click(object sender, EventArgs e)
        {
            frmSickbedSituationByWard frmSSBW = new frmSickbedSituationByWard();
            frmSSBW.Show();
        }

        private void frmRoomEntry_Load(object sender, EventArgs e)
        {

            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            SCREEN_CLEAR ();
            ComFunc.ReadSysDate(clsDB.DbCon);
            SetCbo();
        }

        private void SetCbo()
        {
            int i = 0;
            DataTable dt = null;
            string SQL = "";    //Query문
            string SqlErr = ""; //에러문 받는 변수

            cboWard.Items.Clear();
            cboWard.Items.Add("전체");

            //병동코드를 READ
            try
            {
                SQL = "";
                SQL = "SELECT WardCode FROM BAS_WARD ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WardCode ";

                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                    return;
                }

                for (i = 0; i < dt.Rows.Count; i++)
                {
                    cboWard.Items.Add(dt.Rows[i]["WardCode"].ToString().Trim());
                }

                dt.Dispose();
                dt = null;

            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }

            cboWard.SelectedIndex = 0;

            //A.특A(A) B.특A(B) C.특B(A) D.특B(B) E.특C(A) F.특C(B) G.2인실A
            //H.2인실B I.3인실A J.3인실B K.4인실A L.4인실B M.5인실A
            //N.5인실B O.6인실 P.7인실 Q.8인실 R.9인실 T.격리실 U.ICU
            //V.분만실 W.신생아실 X.인큐베타 Y.화상치료 Z.임신중독실
            cboGrade.Items.Clear();
            cboGrade.Items.Add("*.전체");
            cboGrade.Items.Add("A.특A(A)");
            cboGrade.Items.Add("B.특A(B)");
            cboGrade.Items.Add("C.특B(A)");
            cboGrade.Items.Add("D.특B(B)");
            cboGrade.Items.Add("E.특C(A)");
            cboGrade.Items.Add("F.특C(B)");
            cboGrade.Items.Add("G.2인실A");
            cboGrade.Items.Add("H.2인실B");
            cboGrade.Items.Add("I.3인실A");
            cboGrade.Items.Add("J.3인실B");
            cboGrade.Items.Add("K.4인실A");
            cboGrade.Items.Add("L.4인실B");
            cboGrade.Items.Add("M.5인실A");
            cboGrade.Items.Add("N.5인실B");
            cboGrade.Items.Add("O.6인실");
            cboGrade.Items.Add("P.7인실");
            cboGrade.Items.Add("Q.8인실");
            cboGrade.Items.Add("R.9인실");
            cboGrade.Items.Add("T.격리실");
            cboGrade.Items.Add("U.ICU");
            cboGrade.Items.Add("V.분만실");
            cboGrade.Items.Add("W.신생아실");
            cboGrade.Items.Add("X.인큐베타");
            cboGrade.Items.Add("Y.화상치료");
            cboGrade.Items.Add("Z.임신중독실");
            cboGrade.SelectedIndex = 0;
            cboGrade.Visible = false;

            if (gnJobSabun == "4349" || gnJobSabun == "21403")
            {
                btnAll.Visible = true;
                cboGrade.Visible = true;
            }
        }

        private void ssView_Change(object sender, FarPoint.Win.Spread.ChangeEventArgs e)
        {
            if (fstrPassGrade.Trim() != "EDPS")
            {
                return;
            }
            if (fstrPassId != "4349" && fstrPassId != "13850" && fstrPassId != "21403")
            {
                return;
            }

            ssView_Sheet1.Cells[e.Row, 86].Text = "Y";
        }

        private void ssView_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (fstrPassGrade.Trim() != "EDPS")
            {
                return;
            }
            if (fstrPassId != "4349" && fstrPassId != "13850" && fstrPassId != "21403")
            {
                return;
            }

            string strMSG = "";
            string strData = "";

            int i = 0;
            string nRoom = "";
            long nChaAmt = 0;
            long nBadAmt = 0;
            long nRoomP1 = 0;
            long nRoomP2 = 0;
            long nRoomP3 = 0;
            string strJDate = "";

            strJDate = "2011-01-01";
            nRoomP1 = 51230;     // AB2201 간호3 등급 일반 병실료
            nRoomP2 = 103850;    // AJ240 중환자실 4등급 입원료
            nRoomP3 = 91350;     // AJ201 신생아 4등급 집중치료료실
            
            nRoomP1 = VB.Fix((int)(nRoomP1 * 1.5 + 500) / 1000) * 1000;
            nRoomP2 = VB.Fix((int)(nRoomP2 * 1.5 + 500) / 1000) * 1000;
            nRoomP3 = VB.Fix((int)(nRoomP3 * 1.5 + 500) / 1000) * 1000;

            //병실구분 선택창을 표시함
            if(e.Column == 6 || e.Column==15 || e.Column == 24)
            {
                gstrHelpCode = "";
                frmSelectWard frmSelectWard = new frmSelectWard();
                frmSelectWard.Show();
                if (clsPublic.GstrHelpCode != "")
                {
                    ssView_Sheet1.Cells[e.Row, e.Column].Text = gstrHelpCode;
                    ssView_Sheet1.Cells[e.Row, 86].Text = "Y";
                }
            }

            if(e.Column != 1 && e.Column != 0)
            {
                return;
            }

            strMSG = "변경일자2의 내역을 변경일자3의 내역으로,  " + ComNum.VBLF;
            strMSG = strMSG + "변경일자1의 내역을 변경일자2의 내역으로  " + ComNum.VBLF;
            strMSG = strMSG + "이동을 합니다." + ComNum.VBLF + ComNum.VBLF;
            strMSG = strMSG + "    !!  작업을 하시겠습니까? !!  ";
            if(ComFunc.MsgBoxQ(strMSG, "확인") == DialogResult.No)
            {
                return;
            }

            if(e.Column == 0)
            {
                for(i = 0; i < ssView_Sheet1.RowCount; i++)
                {
                    //GoSub MOVE_RTN
                    move_Rtn(ssView_Sheet1, e, ref nChaAmt, ref nBadAmt, ref nRoom,
                             i, ref strData, ref strJDate, ref nRoomP1, ref nRoomP2, ref nRoomP3);
                }
            }
            else
            {
                //GoSub MOVE_RTN
                move_Rtn(ssView_Sheet1, e, ref nChaAmt, ref nBadAmt, ref nRoom,
                             e.Row, ref strData, ref strJDate, ref nRoomP1, ref nRoomP2, ref nRoomP3);
            }

        }

        private void move_Rtn(SheetView ssView_Sheet1, CellClickEventArgs e, ref long nChaAmt, ref long nBadAmt, ref string nRoom,
                              int i, ref string strData, ref string strJDate, ref long nRoomP1, ref long nRoomP2, ref long nRoomP3)
        {
            nChaAmt = (long)(VB.Val(ssView_Sheet1.Cells[i, 10].Text));
            nBadAmt = (long)(VB.Val(ssView_Sheet1.Cells[i, 9].Text));
            nRoom = ssView_Sheet1.Cells[i, 9].Text;

            if(nBadAmt > 0)
            {
                //변경내역9 => 변경내역 10으로
                ssView_Sheet1.Cells[i, 77].Text = ssView_Sheet1.Cells[i, 69].Text.Trim();
                ssView_Sheet1.Cells[i, 78].Text = ssView_Sheet1.Cells[i, 70].Text.Trim();
                ssView_Sheet1.Cells[i, 79].Text = ssView_Sheet1.Cells[i, 71].Text.Trim();
                ssView_Sheet1.Cells[i, 80].Text = ssView_Sheet1.Cells[i, 72].Text.Trim();
                ssView_Sheet1.Cells[i, 81].Text = ssView_Sheet1.Cells[i, 73].Text.Trim();
                ssView_Sheet1.Cells[i, 82].Text = ssView_Sheet1.Cells[i, 74].Text.Trim();
                ssView_Sheet1.Cells[i, 83].Text = ssView_Sheet1.Cells[i, 75].Text.Trim();
                ssView_Sheet1.Cells[i, 84].Text = ssView_Sheet1.Cells[i, 76].Text.Trim();

                //변경내역8 => 변경내역 9로
                ssView_Sheet1.Cells[i, 69].Text = ssView_Sheet1.Cells[i, 61].Text.Trim(); 
                ssView_Sheet1.Cells[i, 70].Text = ssView_Sheet1.Cells[i, 62].Text.Trim(); 
                ssView_Sheet1.Cells[i, 71].Text = ssView_Sheet1.Cells[i, 63].Text.Trim(); 
                ssView_Sheet1.Cells[i, 72].Text = ssView_Sheet1.Cells[i, 64].Text.Trim(); 
                ssView_Sheet1.Cells[i, 73].Text = ssView_Sheet1.Cells[i, 65].Text.Trim(); 
                ssView_Sheet1.Cells[i, 74].Text = ssView_Sheet1.Cells[i, 66].Text.Trim(); 
                ssView_Sheet1.Cells[i, 75].Text = ssView_Sheet1.Cells[i, 67].Text.Trim(); 
                ssView_Sheet1.Cells[i, 76].Text = ssView_Sheet1.Cells[i, 68].Text.Trim();

                //변경내역7 => 변경내역 8로
                ssView_Sheet1.Cells[i, 61].Text = ssView_Sheet1.Cells[i, 53].Text.Trim(); 
                ssView_Sheet1.Cells[i, 62].Text = ssView_Sheet1.Cells[i, 54].Text.Trim(); 
                ssView_Sheet1.Cells[i, 63].Text = ssView_Sheet1.Cells[i, 55].Text.Trim(); 
                ssView_Sheet1.Cells[i, 64].Text = ssView_Sheet1.Cells[i, 56].Text.Trim(); 
                ssView_Sheet1.Cells[i, 65].Text = ssView_Sheet1.Cells[i, 57].Text.Trim(); 
                ssView_Sheet1.Cells[i, 66].Text = ssView_Sheet1.Cells[i, 58].Text.Trim(); 
                ssView_Sheet1.Cells[i, 67].Text = ssView_Sheet1.Cells[i, 59].Text.Trim(); 
                ssView_Sheet1.Cells[i, 68].Text = ssView_Sheet1.Cells[i, 60].Text.Trim();

                //변경내역 6 => 변경내역 7로
                ssView_Sheet1.Cells[i, 53].Text = ssView_Sheet1.Cells[i, 45].Text.Trim();
                ssView_Sheet1.Cells[i, 54].Text = ssView_Sheet1.Cells[i, 46].Text.Trim(); 
                ssView_Sheet1.Cells[i, 55].Text = ssView_Sheet1.Cells[i, 47].Text.Trim(); 
                ssView_Sheet1.Cells[i, 56].Text = ssView_Sheet1.Cells[i, 48].Text.Trim(); 
                ssView_Sheet1.Cells[i, 57].Text = ssView_Sheet1.Cells[i, 49].Text.Trim(); 
                ssView_Sheet1.Cells[i, 58].Text = ssView_Sheet1.Cells[i, 50].Text.Trim(); 
                ssView_Sheet1.Cells[i, 59].Text = ssView_Sheet1.Cells[i, 51].Text.Trim(); 
                ssView_Sheet1.Cells[i, 60].Text = ssView_Sheet1.Cells[i, 52].Text.Trim(); 

                //변경내역 5 => 변경내역 6으로
                ssView_Sheet1.Cells[i, 45].Text = ssView_Sheet1.Cells[i, 37].Text.Trim();
                ssView_Sheet1.Cells[i, 46].Text = ssView_Sheet1.Cells[i, 38].Text.Trim(); 
                ssView_Sheet1.Cells[i, 47].Text = ssView_Sheet1.Cells[i, 39].Text.Trim(); 
                ssView_Sheet1.Cells[i, 48].Text = ssView_Sheet1.Cells[i, 40].Text.Trim(); 
                ssView_Sheet1.Cells[i, 49].Text = ssView_Sheet1.Cells[i, 41].Text.Trim(); 
                ssView_Sheet1.Cells[i, 50].Text = ssView_Sheet1.Cells[i, 42].Text.Trim(); 
                ssView_Sheet1.Cells[i, 51].Text = ssView_Sheet1.Cells[i, 43].Text.Trim(); 
                ssView_Sheet1.Cells[i, 52].Text = ssView_Sheet1.Cells[i, 44].Text.Trim(); 

                //변경내역 4 => 변경내역 5로
                ssView_Sheet1.Cells[i, 37].Text = ssView_Sheet1.Cells[i, 29].Text.Trim();
                ssView_Sheet1.Cells[i, 38].Text = ssView_Sheet1.Cells[i, 30].Text.Trim(); 
                ssView_Sheet1.Cells[i, 39].Text = ssView_Sheet1.Cells[i, 31].Text.Trim(); 
                ssView_Sheet1.Cells[i, 40].Text = ssView_Sheet1.Cells[i, 32].Text.Trim(); 
                ssView_Sheet1.Cells[i, 41].Text = ssView_Sheet1.Cells[i, 33].Text.Trim(); 
                ssView_Sheet1.Cells[i, 42].Text = ssView_Sheet1.Cells[i, 34].Text.Trim(); 
                ssView_Sheet1.Cells[i, 43].Text = ssView_Sheet1.Cells[i, 35].Text.Trim(); 
                ssView_Sheet1.Cells[i, 44].Text = ssView_Sheet1.Cells[i, 36].Text.Trim(); 

                //변경내역 3 => 변경내역 4로
                ssView_Sheet1.Cells[i, 29].Text = ssView_Sheet1.Cells[i, 21].Text.Trim();
                ssView_Sheet1.Cells[i, 30].Text = ssView_Sheet1.Cells[i, 22].Text.Trim(); 
                ssView_Sheet1.Cells[i, 31].Text = ssView_Sheet1.Cells[i, 23].Text.Trim(); 
                ssView_Sheet1.Cells[i, 32].Text = ssView_Sheet1.Cells[i, 24].Text.Trim(); 
                ssView_Sheet1.Cells[i, 33].Text = ssView_Sheet1.Cells[i, 25].Text.Trim(); 
                ssView_Sheet1.Cells[i, 34].Text = ssView_Sheet1.Cells[i, 26].Text.Trim(); 
                ssView_Sheet1.Cells[i, 35].Text = ssView_Sheet1.Cells[i, 27].Text.Trim(); 
                ssView_Sheet1.Cells[i, 36].Text = ssView_Sheet1.Cells[i, 28].Text.Trim(); 

                //변경내역2 => 변경내역 3으로
                ssView_Sheet1.Cells[i, 21].Text = ssView_Sheet1.Cells[i, 12].Text.Trim();
                ssView_Sheet1.Cells[i, 22].Text = ssView_Sheet1.Cells[i, 13].Text.Trim(); 
                ssView_Sheet1.Cells[i, 23].Text = ssView_Sheet1.Cells[i, 14].Text.Trim(); 
                ssView_Sheet1.Cells[i, 24].Text = ssView_Sheet1.Cells[i, 15].Text.Trim(); 
                ssView_Sheet1.Cells[i, 25].Text = ssView_Sheet1.Cells[i, 16].Text.Trim(); 
                ssView_Sheet1.Cells[i, 26].Text = ssView_Sheet1.Cells[i, 17].Text.Trim(); 
                ssView_Sheet1.Cells[i, 27].Text = ssView_Sheet1.Cells[i, 18].Text.Trim(); 
                ssView_Sheet1.Cells[i, 28].Text = ssView_Sheet1.Cells[i, 19].Text.Trim(); 

                //변경내역1 => 변경내역 2로
                ssView_Sheet1.Cells[i, 12].Text = ssView_Sheet1.Cells[i, 3].Text.Trim();
                ssView_Sheet1.Cells[i, 13].Text = ssView_Sheet1.Cells[i, 4].Text.Trim(); 
                ssView_Sheet1.Cells[i, 14].Text = ssView_Sheet1.Cells[i, 5].Text.Trim(); 
                ssView_Sheet1.Cells[i, 15].Text = ssView_Sheet1.Cells[i, 6].Text.Trim(); 
                ssView_Sheet1.Cells[i, 16].Text = ssView_Sheet1.Cells[i, 7].Text.Trim(); 
                ssView_Sheet1.Cells[i, 17].Text = ssView_Sheet1.Cells[i, 8].Text.Trim(); 
                ssView_Sheet1.Cells[i, 18].Text = ssView_Sheet1.Cells[i, 9].Text.Trim(); 
                ssView_Sheet1.Cells[i, 19].Text = ssView_Sheet1.Cells[i, 10].Text.Trim();

                //변경일자1의 내역을 Clear
                strJDate = ssView_Sheet1.Cells[i, 3].Text;
                switch (nRoom)
                {
                    case "233":
                        ssView_Sheet1.Cells[i, 9].Text = (nRoomP2 + nChaAmt).ToString();
                        break;
                    case "234":
                        ssView_Sheet1.Cells[i, 9].Text = (nRoomP2 + nChaAmt).ToString();
                        break;
                    case "368":
                        ssView_Sheet1.Cells[i, 9].Text = (nRoomP3 + nChaAmt).ToString();
                        break;
                    case "369":
                        ssView_Sheet1.Cells[i, 9].Text = (nRoomP1 + 15000).ToString();
                        break;
                    default:
                        ssView_Sheet1.Cells[i, 9].Text = (nRoomP1 + nChaAmt).ToString();
                        break;
                }

                ssView_Sheet1.Cells[i, 86].Text = "Y";  //변경
            }
        }

        private void ssView_LeaveCell(object sender, LeaveCellEventArgs e)
        {
            if (fstrPassGrade.Trim() != "EDPS")
            {
                return;
            }
            if (fstrPassId != "4349" && fstrPassId != "21403")
            {
                return;
            }

            switch(e.NewColumn)
            {
                case 0:
                    lblMsg.Text = "병동코드는? (더블클릭:변경단위별로 우측으로 1칸이동)";
                    break;
                case 1:
                    lblMsg.Text = "병동코드는? ";
                    break;
                case 3:
                    lblMsg.Text = "변경일자는? (YYYY-MM-DD)";
                    break;
                case 12:
                    lblMsg.Text = "변경일자는? (YYYY-MM-DD) ";
                    break;
                case 21:
                    lblMsg.Text = "변경일자는? (YYYY-MM-DD)";
                    break;
                case 4:
                    lblMsg.Text = "성별 구분은? (M.남자 F.여자 *.공통,X.통계제외)";
                    break;
                case 13:
                    lblMsg.Text = "성별 구분은? (M.남자 F.여자 *.공통,X.통계제외)";
                    break;
                case 22:
                    lblMsg.Text = "성별 구분은? (M.남자 F.여자 *.공통,X.통계제외)";
                    break;
                case 5:
                    lblMsg.Text = "진료과는? (MD,OS,GS, ...)";
                    break;
                case 14:
                    lblMsg.Text = "진료과는? (MD,OS,GS, ...)";
                    break;
                case 23:
                    lblMsg.Text = "진료과는? (MD,OS,GS, ...)";
                    break;
                case 6:
                    lblMsg.Text = "더블클릭을 하면 병실구분 선택창이 표시됩니다.";
                    break;
                case 15:
                    lblMsg.Text = "더블클릭을 하면 병실구분 선택창이 표시됩니다.";
                    break;
                case 24:
                    lblMsg.Text = "더블클릭을 하면 병실구분 선택창이 표시됩니다.";
                    break;
                case 7:
                    lblMsg.Text = "병실별 BED수는? (1-9)";
                    break;
                case 16:
                    lblMsg.Text = "병실별 BED수는? (1-9)";
                    break;
                case 25:
                    lblMsg.Text = "병실별 BED수는? (1-9)";
                    break;
                case 8:
                    lblMsg.Text = "병실의 등급은? (A-Z)";
                    break;
                case 17:
                    lblMsg.Text = "병실의 등급은? (A-Z)";
                    break;
                case 26:
                    lblMsg.Text = "병실의 등급은? (A-Z)";
                    break;
                case 9:
                    lblMsg.Text = "일반환자의 병실료는?";
                    break;
                case 18:
                    lblMsg.Text = "일반환자의 병실료는?";
                    break;
                case 27:
                    lblMsg.Text = "일반환자의 병실료는?";
                    break;
                case 10:
                    lblMsg.Text = "병실차액은?";
                    break;
                case 19:
                    lblMsg.Text = "병실차액은?";
                    break;
                case 28:
                    lblMsg.Text = "병실차액은?";
                    break;
            }
        }
    }
}
