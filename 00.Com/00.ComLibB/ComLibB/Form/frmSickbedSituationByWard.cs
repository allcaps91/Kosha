using System;
using System.Data;
using System.Windows.Forms;
using ComDbB; //DB연결
using ComBase; //기본 클래스

namespace ComLibB
{
    /// <summary>
    /// Class Name : frmSickbedSituationByWard
    /// File Name : frmSickbedSituationByWard.cs
    /// Title or Description : 병동별 병상현황
    /// Author : 박창욱
    /// Create Date : 2017-06-01
    /// Update Histroy : 
    ///     06-12 박창욱 : clsDB의 변경에 대응하여 수정
    ///     06-15 박창욱 : 디자인 수정, 공통함수 GetBCODENameCode적용 후 임시 구현한 함수 삭제
    /// </summary>
    /// <history>  
    /// VB\Frm병동별_병상현황.frm(Frm병동별_병상현황) -> frmSickbedSituationByWard.cs 으로 변경함
    /// </history>
    /// <seealso> 
    /// VB\basic\bucode\Frm병동별_병상현황.frm(Frm병동별_병상현황)
    /// </seealso>
    /// <vbp>
    /// default : VB\PSMHH\basic\bucode\bucode.vbp
    /// </vbp>
    public partial class frmSickbedSituationByWard : Form
    {
        public frmSickbedSituationByWard()
        {
            InitializeComponent();
        }

        private void frmSickbedSituationByWard_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth (clsDB.DbCon, this) == false) { this.Close (); return; } //폼 권한 조회
            ComFunc.SetFormInit (clsDB.DbCon, this , "Y" , "Y" , "Y"); //폼 기본값 세팅 등

            getSearch ();
        }

        private void getSearch()
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인

            int i = 0;
            int j = 0;
            int nRow = 0;
            int nCol = 0;
            int nTbed = 0;
            int count = 0;
            int count2 = 0;
            string strWard = "";
            string strGbroom = "";

            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수
            DataTable dt = null;
            long[,] totalCount = new long[31, 12];

            Cursor.Current = Cursors.WaitCursor;

            //누적할 변수를 Clear
            for (i = 1; i < 31; i++)
            {
                for (j = 1; j < 12; j++)
                {
                    totalCount[i, j] = 0;
                }
            }
            
            //기타병상 값을 설정
            totalCount[23, 3] = (long)(VB.Val(VB.Left(clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_기타병상", "01"), 3)));   //혈액투석
            totalCount[24, 3] = (long)(VB.Val(VB.Left(clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_기타병상", "02"), 3)));   //복막투석
            totalCount[25, 3] = (long)(VB.Val(VB.Left(clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_기타병상", "03"), 3)));   //ER Bed
            totalCount[26, 3] = (long)(VB.Val(VB.Left(clsVbfunc.GetBCODENameCode(clsDB.DbCon, "1", "BAS_기타병상", "04"), 3)));   //ER S-Car

            for (i = 13; i < 20; i++)
            {
                totalCount[i, 1] = 1;
            }

            try
            {
                //병상현황을 읽음
                SQL = SQL + "SELECT WardCode,GbRoom,TBed,COUNT(RoomCode) As CNT1,SUM(TBed) As CNT2 ";
                SQL = SQL + ComNum.VBLF + " FROM " + ComNum.DB_PMPA + "BAS_ROOM ";
                SQL = SQL + ComNum.VBLF + "WHERE TBed>0 ";
                SQL = SQL + ComNum.VBLF + "  AND GbRoom<>'51' ";
                SQL = SQL + ComNum.VBLF + "GROUP BY WardCode,GbRoom,TBed ";
                SQL = SQL + ComNum.VBLF + "ORDER BY WardCode,GbRoom,TBed ";
                SqlErr = clsDB.GetDataTable(ref dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다.");
                    return;
                }
                if (dt.Rows.Count == 0)
                {
                    dt.Dispose();
                    dt = null;
                    ComFunc.MsgBox("해당 DATA가 없습니다.");
                }
                else
                {
                    for (i = 0; i < dt.Rows.Count; i++)
                    {
                        strWard = dt.Rows[i]["WardCode"].ToString().Trim();
                        strGbroom = dt.Rows[i]["GbRoom"].ToString().Trim();
                        nTbed = Convert.ToInt32(dt.Rows[i]["TBed"].ToString().Trim());
                        count = Convert.ToInt32(dt.Rows[i]["CNT1"].ToString().Trim());
                        count2 = Convert.ToInt32(dt.Rows[i]["CNT2"].ToString().Trim());

                        nRow = 0;

                        switch (strWard)
                        {
                            case "HU":
                                nRow = 1;
                                break;
                            case "3W":
                                nRow = 2;   //개방
                                break;
                            case "3A":
                                nRow = 3;
                                break;
                            case "3B":
                                nRow = 4;
                                break;
                            case "3C":
                                nRow = 5;
                                if (strGbroom == "11") { nRow = 19; }   //분만대기실
                                if (strGbroom == "12") { nRow = 20; }   //분만회복실
                                if (strGbroom == "13") { nRow = 21; }   //산과중독실
                                break;
                            case "4A":
                                nRow = 6;
                                break;
                            case "4W":
                                nRow = 7;
                                break;
                            case "5W":
                                nRow = 8;
                                break;
                            case "6W":
                                nRow = 9;
                                break;
                            case "6A":
                                nRow = 10;
                                break;
                            case "7W":
                                nRow = 11;
                                break;
                            case "8W":
                                nRow = 12;
                                break;
                            case "IU":
                                nRow = 15;
                                if (strGbroom == "05") { nRow = 15; }   //SICU
                                if (strGbroom != "05") { nRow = 16; }   //MICU
                                break;
                            case "IQ":
                                nRow = 18;
                                if (strGbroom == "07") nRow = 17;   //NICU
                                if (strGbroom == "15") nRow = 18;   //인큐베이터
                                break;
                            case "ND":
                                nRow = 27;  //Bassinet
                                if (strGbroom == "14") { nRow = 27; }   //신생아베지넷
                                if (strGbroom == "15") { nRow = 18; }   //인큐베이터
                                if (strGbroom == "07") { nRow = 17; }   //NICU
                                break;
                            case "NR":
                                nRow = 27;
                                if (strGbroom == "14") { nRow = 27; }   //신생아베지넷
                                if (strGbroom == "15") { nRow = 18; }   //인큐베이터
                                if (strGbroom == "07") { nRow = 17; }   //NICU
                                break;
                            default:
                                if (strGbroom == "11") { nRow = 19; }   //분만대기실
                                if (strGbroom == "12") { nRow = 21; }   //분만회복실
                                if (strGbroom == "13") { nRow = 20; }   //산과중독실
                                if (strGbroom == "14") { nRow = 27; }   //신생아베지넷
                                break;
                        }
                        if (strGbroom == "11") { nRow = 19; }   //분만대기실
                        if (strGbroom == "12") { nRow = 21; }   //분만회복실
                        if (strGbroom == "13") { nRow = 20; }   //산과중독실

                        if (nRow == 0)
                            nRow = 29;

                        if (nTbed >= 1 && nTbed <= 7)
                            nCol = nTbed + 3;
                        else
                            nCol = 11;

                        //병실수, 병상수, 인실별 병상수를 누적
                        if (nRow <= 12)  //ch
                        {
                            totalCount[nRow, 1] = totalCount[nRow, 1] + count;  //병실 수
                            totalCount[nRow, 2] = totalCount[nRow, 2] + count2; //병상 수
                            totalCount[nRow, 3] = totalCount[nRow, 3] + count2; //병상 수
                            if (nRow < 15)
                            {
                                totalCount[nRow, nCol] = totalCount[nRow, nCol] + count;
                            }
                        }
                    }
                    dt.Dispose();
                    dt = null;
                }
            }
            catch (Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox(ex.Message);
            }

            //일반병실 합계를 구함
            for (i = 1; i < 13; i++)
            {
                for (j = 1; j < 12; j++)
                {
                    totalCount[13, j] = totalCount[13, j] + totalCount[i, j];
                    if (i != 3) //NP,HU 제외
                        totalCount[14, j] = totalCount[14, j] + totalCount[i, j];
                    if (i <= 3)
                        totalCount[22, j] = totalCount[22, j] + totalCount[i, j];
                    if (i == 3)
                        totalCount[30, j] = totalCount[30, j] + totalCount[i, j];
                }
            }

            //입원병실 합계를 구함
            for (i = 15; i < 22; i++)
            {
                for (j = 2; j < 12; j++)
                {
                    totalCount[22, j] = totalCount[22, j] + totalCount[i, j];
                    if (j == 3)
                        totalCount[30, j] = totalCount[30, j] + totalCount[i, j];
                }
            }

            //총 병상 수를 구함
            for (i = 23; i < 30; i++)
            {
                totalCount[30, 3] = totalCount[30, 3] + totalCount[i, 3]; 
            }

            //일반 병실을 화면에 표시
            for (i = 1; i < 15; i++)
            {
                for (j = 1; j < 12; j++)
                {
                    if (j == 1)
                        ssView_Sheet1.Cells[i + 2, 2].Text = VB.Format(totalCount[i, j], "#,###");
                    else
                        ssView_Sheet1.Cells[i + 2, j + 2].Text = VB.Format(totalCount[i, j], "#,###");
                }
            }

            //집중치료실, 신생아실, 분만실
            for (i = 15; i < 23; i++)
            {
                for (j = 2; j < 4; j++)
                {
                    ssView_Sheet1.Cells[i + 2, j + 2].Text = VB.Format(totalCount[i, j], "#,###");
                }
            }

            //기타병상 및 총병상수
            for (i = 23; i < 31; i++)
            {
                ssView_Sheet1.Cells[i + 2, 5].Text = VB.Format(totalCount[i, 3], "#,###");
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false) return; //권한 확인
            getSearch();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (ComQuery.IsJobAuth(this, "P" , clsDB.DbCon) == false) return; //권한 확인

            string strFont1 = "";
            string strFont2 = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            Cursor.Current = Cursors.WaitCursor;

            strFont1 = "/l/fn\"굴림\" /fz\"15";
            strFont2 = "/fn\"굴림\"/fb0/fu0/fz\"10";

            ssView_Sheet1.PrintInfo.Header = strFont1 + "/c" + "각 병동 병상 현황" + "/n/n/n";
            ssView_Sheet1.PrintInfo.Header = ssView_Sheet1.PrintInfo.Header + strFont2;
            ssView_Sheet1.PrintInfo.Header = ssView_Sheet1.PrintInfo.Header + VB.Space(1) + "인쇄일자 : " + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
            ssView_Sheet1.PrintInfo.Centering = FarPoint.Win.Spread.Centering.Horizontal;
            ssView_Sheet1.PrintInfo.Margin.Top = 100;
            ssView_Sheet1.PrintInfo.Margin.Bottom = 10;
            ssView_Sheet1.PrintInfo.ShowColumnHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowRowHeader = FarPoint.Win.Spread.PrintHeader.Show;
            ssView_Sheet1.PrintInfo.ShowBorder = true;
            ssView_Sheet1.PrintInfo.ShowColor = true;
            ssView_Sheet1.PrintInfo.ShowGrid = true;
            ssView_Sheet1.PrintInfo.ShowShadows = true;
            ssView_Sheet1.PrintInfo.UseMax = false;
            ssView_Sheet1.PrintInfo.PrintType = FarPoint.Win.Spread.PrintType.All;
            ssView.PrintSheet(0);

            Cursor.Current = Cursors.Default;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
