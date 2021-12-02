using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스


namespace ComPmpaLibB
{
    /// <summary>
    /// Class Name      : ComPmpaLibB
    /// File Name       : frmPmpaViewOldRightHistory.cs
    /// Description     : 자격조회 이전 history
    /// Author          : 안정수
    /// Create Date     : 2017-09-01
    /// Update History  : 2017-11-02
    /// <history>       
    /// d:\psmh\OPD\oumsad\Frm자격history.frm(Frm자격history) => frmPmpaViewOldRightHistory.cs 으로 변경함
    /// </history>
    /// <seealso>
    /// d:\psmh\OPD\oumsad\Frm자격history.frm(Frm자격history)
    /// </seealso>
    /// </summary>
    public partial class frmPmpaViewOldRightHistory : Form
    {
        ComFunc CF = new ComFunc();
        clsSpread CS = new clsSpread();
        string mstrHelpCode = "";
        public frmPmpaViewOldRightHistory()
        {
            InitializeComponent();
            setEvent();
        }

        public frmPmpaViewOldRightHistory(string GstrHelpCode)
        {
            InitializeComponent();
            setEvent();
            mstrHelpCode = GstrHelpCode;
        }

        void setEvent()
        {
            this.Load += new System.EventHandler(eFormLoad);

            this.btnExit.Click += new EventHandler(eBtnEvent);
            this.btnView.Click += new EventHandler(eBtnEvent);

            this.txtPano.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.chkAll.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
            this.dtpDate.KeyPress += new KeyPressEventHandler(eControl_KeyPress);
        }

        void eControl_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == this.txtPano)
            {
                if(e.KeyChar == 13)
                {
                    chkAll.Focus();
                }
            }

            else if (sender == this.chkAll)
            {
                if (e.KeyChar == 13)
                {
                    dtpDate.Focus();
                }
            }

            else if (sender == this.dtpDate)
            {
                if (e.KeyChar == 13)
                {
                    btnView.Focus();
                }
            }
        }

        void eFormLoad(object sender, EventArgs e)
        {
            //if (ComQuery.isFormAuth(clsDB.DbCon, this) == false)
            //{
            //    this.Close(); //폼 권한 조회
            //    return;
            //}

            //ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y"); //폼 기본값 세팅 등 

            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

            txtPano.Text = "";

            if (mstrHelpCode != "")
            {
                txtPano.Text = mstrHelpCode;
            }
        }

        void eBtnEvent(object sender, EventArgs e)
        {
            if (sender == this.btnExit)
            {
                this.Close();
            }

            else if (sender == this.btnView)
            {
                if (ComQuery.IsJobAuth(this, "R", clsDB.DbCon) == false)
                {
                    return; //권한 확인
                }
                eGetData();
            }
        }

        void eGetData()
        {
            int i = 0;
            int j = 0;
            int nREAD = 0;

            string strBi = "";
            string strGKiho = "";

            DataTable dt = null;
            string SQL = "";
            string SqlErr = "";

            if (txtPano.Text.Trim() == "")
            {
                return;
            }

            CS.Spread_All_Clear(ssList);

            SQL = "";
            SQL += ComNum.VBLF + "SELECT                                                                     ";
            SQL += ComNum.VBLF + "  TO_CHAR(ACTDATE,'YYYYMMDD') ACTDATE , PANO, DEPTCODE,SNAME,              ";
            SQL += ComNum.VBLF + "  TO_CHAR(SENdtIME ,'YYYY-MM-DD HH24:MI') SENdtIME,M2_SEDAE_NAME,          ";
            SQL += ComNum.VBLF + "  M2_CDATE,M2_DisReg1,M2_DisReg2,M2_DisReg3,M2_DisReg4,M2_DISREG9,         ";
            SQL += ComNum.VBLF + "  M2_SANGSIL,req_sabun,m2_jang_date, M2_DisReg5,                           ";
            SQL += ComNum.VBLF + "  SUBSTR(JUMIN,1,6) || '-' || SUBSTR(JUMIN,7,7)  JUMIN ,                   ";
            SQL += ComNum.VBLF + "  JUMIN_new ,                                                              ";
            SQL += ComNum.VBLF + "  M2_KIHO,M2_GKIHO,M2_JAGEK, M2_DISREG11,                                  ";
            SQL += ComNum.VBLF + "  M2_SEDAE_NAME ,M2_BONIN,M2_CHULGUK, M2_RESTRICT,OBSTYN,  M2_DISREG2_A,M2_DISREG2_B,M2_DISREG2_C   ";
            SQL += ComNum.VBLF + "FROM " + ComNum.DB_PMPA + "OPD_NHIC                                        ";
            SQL += ComNum.VBLF + "WHERE 1=1                                                                  ";
            SQL += ComNum.VBLF + "      AND PANO ='" + txtPano.Text + "'                                     ";
            SQL += ComNum.VBLF + "      AND REQTYPE ='M1'                                                    ";    //자격조회
            SQL += ComNum.VBLF + "      AND JOB_STS ='2'                                                     ";     //정상조회건

            if (dtpDate.Text == "")
            {
                if (chkAll.Checked == true)
                {
                    SQL += ComNum.VBLF + "  AND ACTDATE >=TRUNC(SYSDATE-60)                                  ";
                }
                else
                {
                    SQL += ComNum.VBLF + "  AND ACTDATE=TO_DATE('" + dtpDate.Text + "','YYYY-MM-DD')         ";
                }
            }

            SQL += ComNum.VBLF + "ORDER BY ACTDATE DESC ,DEPTCODE                                            ";

            try
            {

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

                if (dt.Rows.Count > 0)
                {
                    nREAD = dt.Rows.Count;

                    ssList_Sheet1.Rows.Count = nREAD;

                    for (i = 0; i < nREAD; i++)
                    {
                        ssList_Sheet1.Cells[i, 0].Text = dt.Rows[i]["PANO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 1].Text = dt.Rows[i]["SNAME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 2].Text = dt.Rows[i]["SENdtIME"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 3].Text = dt.Rows[i]["DEPTCODE"].ToString().Trim();

                        if (dt.Rows[i]["JUMIN_NEW"].ToString().Trim() != "")
                        {
                            ssList_Sheet1.Cells[i, 4].Text = VB.Left(clsAES.DeAES(dt.Rows[i]["JUMIN_NEW"].ToString().Trim()), 6) + "-"
                                                           + VB.Right(clsAES.DeAES(dt.Rows[i]["JUMIN_NEW"].ToString().Trim()), 7);
                        }
                        else
                        {
                            ssList_Sheet1.Cells[i, 4].Text = dt.Rows[i]["JUMIN"].ToString().Trim();
                        }

                        ssList_Sheet1.Cells[i, 5].Text = "";

                        switch (dt.Rows[i]["M2_JAGEK"].ToString().Trim())
                        {
                            case "1":
                                ssList_Sheet1.Cells[i, 5].Text = "1.지역가입자";
                                break;
                            case "2":
                                ssList_Sheet1.Cells[i, 5].Text = "2.지역세대원";
                                break;

                            case "4":
                                switch (VB.Left(dt.Rows[i]["M2_GKIHO"].ToString().Trim(), 1))
                                {
                                    case "5":
                                    case "6":
                                        ssList_Sheet1.Cells[i, 5].Text = "4.임의계속직장가입자(공단)";
                                        break;

                                    default:
                                        ssList_Sheet1.Cells[i, 5].Text = "4.임의계속직장가입자(직장)";
                                        break;
                                }
                                break;

                            case "5":
                                switch (VB.Left(dt.Rows[i]["M2_GKIHO"].ToString().Trim(), 1))
                                {
                                    case "5":
                                    case "6":
                                        ssList_Sheet1.Cells[i, 5].Text = "5.직장가입자(공단)";
                                        break;

                                    default:
                                        ssList_Sheet1.Cells[i, 5].Text = "5.직장가입자(직장)";
                                        break;
                                }
                                break;

                            case "6":
                                switch (VB.Left(dt.Rows[i]["M2_GKIHO"].ToString().Trim(), 1))
                                {
                                    case "5":
                                    case "6":
                                        ssList_Sheet1.Cells[i, 5].Text = "6.직장피부양자(공단)";
                                        break;

                                    default:
                                        ssList_Sheet1.Cells[i, 5].Text = "6.직장피부양자(직장)";
                                        break;
                                }
                                break;

                            case "7":
                                ssList_Sheet1.Cells[i, 5].Text = "7.의료급여1종";
                                break;

                            case "8":
                                ssList_Sheet1.Cells[i, 5].Text = "8.의료급여2종";
                                break;
                        }

                        strBi = ssList_Sheet1.Cells[i, 5].Text;

                        strGKiho = dt.Rows[i]["M2_GKIHO"].ToString().Trim();
                        ssList_Sheet1.Cells[i, 6].Text = "";

                        switch (VB.Left(strBi, 1))
                        {
                            case "1":
                            case "2":
                                ssList_Sheet1.Cells[i, 7].Text = "13";
                                break;

                            case "4":
                            case "5":
                            case "6":
                                switch (VB.Left(strGKiho, 1))
                                {
                                    case "5":
                                    case "6":
                                        ssList_Sheet1.Cells[i, 7].Text = "11";
                                        break;
                                    default:
                                        ssList_Sheet1.Cells[i, 7].Text = "12";
                                        break;
                                }
                                break;

                            case "7":
                                ssList_Sheet1.Cells[i, 7].Text = "21";
                                break;
                            case "8":
                                ssList_Sheet1.Cells[i, 7].Text = "22";
                                break;
                        }

                        ssList_Sheet1.Cells[i, 7].Text = dt.Rows[i]["M2_KIHO"].ToString().Trim();       //보장기관기호
                        ssList_Sheet1.Cells[i, 8].Text = dt.Rows[i]["M2_GKIHO"].ToString().Trim();      //시설기호
                        ssList_Sheet1.Cells[i, 9].Text = dt.Rows[i]["M2_CDATE"].ToString().Trim() == "" ? "" : dt.Rows[i]["M2_CDATE"].ToString().Trim(); //자격취득일자
                        ssList_Sheet1.Cells[i, 10].Text = dt.Rows[i]["M2_SANGSIL"].ToString().Trim() == "" ? "" : dt.Rows[i]["M2_SANGSIL"].ToString().Trim();   //급여제한일자

                        ssList_Sheet1.Cells[i, 11].Text = dt.Rows[i]["M2_DisReg1"].ToString().Trim();   //희귀난치 H
                        ssList_Sheet1.Cells[i, 12].Text = dt.Rows[i]["M2_DisReg2"].ToString().Trim()+ dt.Rows[i]["M2_DisReg2_A"].ToString().Trim()+ dt.Rows[i]["M2_DisReg2_B"].ToString().Trim()+ dt.Rows[i]["M2_DisReg2_C"].ToString().Trim();   //등록희귀난치 VW
                        ssList_Sheet1.Cells[i, 13].Text = dt.Rows[i]["M2_DisReg9"].ToString().Trim();   //등록결액환자    V
                        ssList_Sheet1.Cells[i, 14].Text = dt.Rows[i]["M2_DISREG11"].ToString().Trim();   //차상위 C, E, F
                        ssList_Sheet1.Cells[i, 15].Text = dt.Rows[i]["M2_DisReg3"].ToString().Trim();   //차상위 C, E, F
                        ssList_Sheet1.Cells[i, 16].Text = dt.Rows[i]["M2_DisReg4"].ToString().Trim();   //중증암 V193, V194
                        ssList_Sheet1.Cells[i, 17].Text = dt.Rows[i]["M2_DisReg5"].ToString().Trim();   //중증암 V193, V194

                        //2014-03-04 본인부담구분코드 추가
                        ssList_Sheet1.Cells[i, 18].Text = dt.Rows[i]["m2_BONIN"].ToString().Trim();     //본인부담구분코드

                        ssList_Sheet1.Cells[i, 19].Text = dt.Rows[i]["m2_jang_date"].ToString().Trim();                     //장애인등록일자
                        ssList_Sheet1.Cells[i, 20].Text = CF.Read_SabunName(clsDB.DbCon, dt.Rows[i]["req_sabun"].ToString().Trim());     //요청사번
                        ssList_Sheet1.Cells[i, 21].Text = dt.Rows[i]["M2_SEDAE_NAME"].ToString().Trim();                    //요청사번
                        ssList_Sheet1.Cells[i, 22].Text = dt.Rows[i]["M2_CHULGUK"].ToString().Trim();                       //출국여부

                        if (dt.Rows[i]["M2_RESTRICT"].ToString().Trim() == "01")
                        {
                            ssList_Sheet1.Cells[i, 23].Text = "무자격자";
                        }
                        else if (dt.Rows[i]["M2_RESTRICT"].ToString().Trim() == "02")
                        {
                            ssList_Sheet1.Cells[i, 23].Text = "자격제한대상자";
                        }
                        else if (dt.Rows[i]["M2_RESTRICT"].ToString().Trim() == "03")
                        {
                            ssList_Sheet1.Cells[i, 23].Text = "외국인등 체납급여제한 대상자";
                        }
                        ssList_Sheet1.Cells[i, 24].Text = dt.Rows[i]["OBSTYN"].ToString().Trim();       //장애여부

                        ssList.ActiveSheet.Rows[i].Height = 18;
                    }
                }
            }
            catch (Exception ex)
            {
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon); //에러로그 저장
            }
            dt.Dispose();
            dt = null;
        }

        void txtPano_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                SendKeys.Send("{TAB}");
                txtPano.Text = ComFunc.SetAutoZero(txtPano.Text, 8);
            }
        }

        void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strTemp = "";
            string strMsg = "";

            if (e.Row >= 0 && (e.Column == 11 || e.Column == 12 || e.Column == 13 || e.Column == 14))
            {
                //  ▷M2_DISREG1(희귀난치대상자)
                // 특정기호(4)+승인일(8)+종료일(8)+승인상병(5) 승인상병최대 5개
                //
                // ▷M2_DISREG2(산전산모대상자->산정특례(희귀)등록대상)
                // 특정기호 (4) + 주민번호(15) + 최초임신확인일(8) + 출산예정일(8) + 진료월일(4)
                //   진료월일은 최대 14개
                //
                // ▷M2_DISREG3(차상위대상)
                // 특정기호 (4) + 시작일(8) + 종료일(8) + 구분(1)
                //   구분:1.차상위1종 2:차상위2종
                //
                // ▷M2_DISREG4(중증암등록대상자->산정특례(암)등록대상자)
                // 특정기호 (4) + 주민번호(15) + 등록일(8) + 종료일(8) + 상병기호(5)

                strMsg = "";

                strTemp = ssList_Sheet1.Cells[e.Row, e.Column].Text.Trim();

                if (strTemp != "")
                {
                    if (e.Column == 11)
                    {
                        strMsg = "희귀난치대상" + "\r\n" + "\r\n";
                        strMsg += "특정기호 : " + VB.Left(strTemp, 4) + "\r\n";
                        strMsg += "승인일자 : " + VB.Mid(strTemp, 5, 8) + "\r\n";
                        strMsg += "종료일자 : " + VB.Mid(strTemp, 13, 8) + "\r\n";
                    }

                    else if (e.Column == 12)
                    {
                        strMsg = "등록희귀V대상" + "\r\n" + "\r\n";
                        strMsg += "특정기호 : " + VB.Left(strTemp, 4) + "\r\n";
                        strMsg += "시작일자 : " + VB.Mid(strTemp, 20, 8) + "\r\n";
                        strMsg += "종료일자 : " + VB.Mid(strTemp, 28, 8) + "\r\n";
                        strMsg += "등록번호 : " + VB.Mid(strTemp, 5, 15) + "\r\n";
                    }

                    else if (e.Column == 13)
                    {
                        strMsg = "산정특례(결핵)등록대상자" + "\r\n" + "\r\n";
                        strMsg += "특정기호 : " + VB.Left(strTemp, 4) + "\r\n";
                        strMsg += "시작일자 : " + VB.Mid(strTemp, 20, 8) + "\r\n";
                        strMsg += "종료일자 : " + VB.Mid(strTemp, 28, 8) + "\r\n";
                        strMsg += "등록번호 : " + VB.Mid(strTemp, 5, 15) + "\r\n";
                    }
                    else if (e.Column == 14)
                    {
                        strMsg = "산정특례(치매)등록대상자" + "\r\n" + "\r\n";
                        strMsg += "특정기호 : " + VB.Left(strTemp, 4) + "\r\n";
                        strMsg += "시작일자 : " + VB.Mid(strTemp, 32, 8) + "\r\n";
                        strMsg += "종료일자 : " + VB.Mid(strTemp, 40, 8) + "\r\n";
                    }

                    else if (e.Column == 15)
                    {
                        strMsg = "차상위2종대상" + "\r\n" + "\r\n";
                        strMsg += "특정기호 : " + VB.Left(strTemp, 4) + "\r\n";
                        strMsg += "시작일자 : " + VB.Mid(strTemp, 5, 8) + "\r\n";
                        strMsg += "종료일자 : " + VB.Mid(strTemp, 13, 8) + "\r\n";
                    }

                    else if (e.Column == 16)
                    {
                        strMsg = "중중암대상" + "\r\n" + "\r\n";
                        strMsg += "특정기호 : " + VB.Left(strTemp, 4) + "\r\n";
                        strMsg += "시작일자 : " + VB.Mid(strTemp, 20, 8) + "\r\n";
                        strMsg += "종료일자 : " + VB.Mid(strTemp, 28, 8) + "\r\n";
                        strMsg += "등록번호 : " + VB.Mid(strTemp, 5, 15) + "\r\n";
                    }

                    ComFunc.MsgBox(strMsg);
                }
            }
        }
    }
}
