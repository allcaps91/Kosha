using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using ComBase; //기본 클래스

/// <summary>
/// Description : 현금영수증 승인(다우데이터)
/// Author : 박병규
/// Create Date : 2017.09.14
/// </summary>
/// <history>
/// 신용카드 승인과 현금영수 승인 작업창 통합으로 사용안함 주석처리 2018-02-09  KMC
///   ===> frmPmpaEntryCardDaou.cs  사용
/// </history>
/// <seealso cref="frm현금영수승인_다우.frm"/> 

namespace ComPmpaLibB
{
    public partial class frmPmpaEntryCashDaou : Form
    {
        clsApi CA = new clsApi();
        ComFunc CF = new ComFunc();
        clsUser CU = new clsUser();
        clsSpread CS = new clsSpread();
        //clsCard_Daou CD = new clsCard_Daou();
        clsPmpaFunc CPF = new clsPmpaFunc();
        clsPmpaPrint CPP = new clsPmpaPrint();
        Card CC = new Card();
        DataTable Dt = new DataTable();
        DataTable DtSub = new DataTable();
        string SQL = string.Empty;
        string SqlErr = string.Empty;
        int intRowCnt = 0;

        clsPmpaType.AcctReqData RSD = new clsPmpaType.AcctReqData();
        clsPmpaType.AcctResData RD = new clsPmpaType.AcctResData();

        string FstrCashCancel = string.Empty;
        int FintCardApproveGbn; //카드승인 또는 카드취소 또는 익일취소 구분


        public frmPmpaEntryCashDaou()
        {
            InitializeComponent();
            setEvent();
        }

        private void setEvent()
        {
            this.Load += new EventHandler(eFrm_Load);

            //GotFocus
            this.txtCardNo.GotFocus += new EventHandler(eCtl_GotFocus); //카드번호
            this.txtCardAmt.GotFocus += new EventHandler(eCtl_GotFocus); //금액

            //KeyDown
            this.txtCardNo.KeyDown += new KeyEventHandler(eCtl_KeyDown); //카드번호

            //KeyPress
            this.txtCardNo.KeyPress += new KeyPressEventHandler(eCtl_KeyPress); //카드번호
            this.txtCardAmt.KeyPress += new KeyPressEventHandler(eCtl_KeyPress); //금액
            this.txtPtno.KeyPress += new KeyPressEventHandler(eCtl_KeyPress); //등록번호
            this.cboDept.KeyPress += new KeyPressEventHandler(eCtl_KeyPress); //진료과
            this.cboIO.KeyPress += new KeyPressEventHandler(eCtl_KeyPress); //외래/입원

            //LostFocus
            this.cboDept.LostFocus += new EventHandler(eCtl_LostFocus);


            //Click
            this.rdoGubun0.Click += new EventHandler(eCtl_Click);//승인요청
            this.rdoGubun1.Click += new EventHandler(eCtl_Click);//승인취소

            this.btnApproval.Click += new EventHandler(eCtl_Click);//요청
            this.btnCancel.Click += new EventHandler(eCtl_Click);//취소
            this.btnExit.Click += new EventHandler(eCtl_Click);//닫기

            this.rdoBun0.Click += new EventHandler(eCtl_Click);//영수발급
            this.rdoBun1.Click += new EventHandler(eCtl_Click);//자진발급
            this.btnPrint.Click += new EventHandler(eCtl_Click);//현금승인재발행

            this.chkCash.Click += new EventHandler(eCtl_Click); //현금영수증 거부 체크박스
            this.btnSign.Click += new EventHandler(eCtl_Click);//타블렛실행
            this.btnPead.Click += new EventHandler(eCtl_Click);//입력번호 확인
            this.btnCash.Click += new EventHandler(eCtl_Click);//패드번호 입력요청

        }

        private void eCtl_LostFocus(object sender, EventArgs e)
        {
            if (sender == this.cboDept)
            {
                cboDept.Text = cboDept.Text.ToUpper();

                if (cboDept.Text.Trim() == "II")
                    CC.gstrCdGbIo = "O";
            }




        }

        private void eCtl_KeyPress(object sender, KeyPressEventArgs e)
        {
            Process Proc = new Process(); //외부프로그램 실행

            if (sender == this.txtCardNo && e.KeyChar == (Char)13) { chkPrt.Focus(); }

            if (sender == this.txtCardAmt)
            {
                string strName = string.Empty;

                if (txtSname.Text.Trim().Length == 3)
                    strName = VB.Left(txtSname.Text.Trim(), 2) + "○";
                else
                    strName = VB.Left(txtSname.Text.Trim(), 2) + "○○";

                if (e.KeyChar == (Char)13)
                {
                    if (clsPmpaPb.GstrCreditIF != "P")
                    {
                        FileInfo fi = new FileInfo(clsPublic.GstrTabletD_FilePath);
                        if (fi.Exists) //파일존재
                        {
                            CA.Tablet_Shell_Check("tablet_d", "현금영수증"); //특정프로그램이 실행중인지 알아내기 위함
                            //Thread.Sleep(200);

                            if (rdoGubun1.Checked == true)
                            {
                                Proc.StartInfo.FileName = @"c:\cmc\exe\tablet_d.exe"; //실행할 파일명
                                Proc.StartInfo.Arguments = "환불님!" + txtCardAmt.Text.Trim() + "!";
                                Proc.Start();
                            }
                            else
                            {
                                Proc.StartInfo.FileName = @"c:\cmc\exe\tablet_d.exe";
                                Proc.StartInfo.Arguments = strName + txtCardAmt.Text.Trim() + "!";
                                Proc.Start();
                            }
                        }

                        btnPead.Focus();
                    }
                    else
                    {
                        btnCash.Focus();
                    }
                }
            }

            if (sender == this.txtPtno && e.KeyChar == (Char)13)
            {
                if (CF.READ_BARCODE(txtPtno.Text.Trim()) == true)
                {
                    txtPtno.Text = clsPublic.GstrBarPano;
                    cboDept.Text = clsPublic.GstrBarDept;
                    txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
                    txtSname.Text = "";

                    SELECT_JUMIN(txtPtno.Text);

                    if (txtSname.Text != "")
                    {
                        CARD_APPROV_DISPLAY(txtPtno.Text);
                        txtCardNo.Enabled = true;
                    }

                    txtCardNo.Focus();
                }
                else
                {
                    txtPtno.Text = string.Format("{0:D8}", Convert.ToInt32(txtPtno.Text));
                    txtSname.Text = "";

                    SELECT_JUMIN(txtPtno.Text);

                    if (txtSname.Text != "")
                    {
                        CARD_APPROV_DISPLAY(txtPtno.Text);
                        txtCardNo.Enabled = true;
                    }

                    cboDept.Focus();
                }
            }

            if (sender == this.cboDept && e.KeyChar == (Char)13) { cboIO.Focus(); }
            if (sender == this.cboIO && e.KeyChar == (Char)13) { txtCardNo.Focus(); }
        }

        private void eCtl_KeyDown(object sender, KeyEventArgs e)
        {
            if (sender == this.txtCardNo && e.KeyCode == Keys.Enter)
            {
                if (txtCardNo.Text.Trim() == "") { return; }

                int Index = VB.InStr(txtCardNo.Text, "=");
                if (Index == 0) { return; }

                txtCardNo.Text = VB.Left(txtCardNo.Text, Index - 1);
                txtCardFullNo.Text = VB.Mid(txtCardNo.Text, Index, txtCardNo.Text.Length);

                if (txtCardAmt.Text.Trim() == "0")
                    txtCardAmt.Focus();
                else
                    btnApproval.Focus();
            }





        }

        private void eCtl_GotFocus(object sender, EventArgs e)
        {
            if (sender == this.txtCardNo)
            {
                txtCardNo.SelectionStart = 0;
                txtCardNo.SelectionLength = txtCardNo.Text.Length;
            }

            if (sender == this.txtCardAmt)
            {
                txtCardAmt.SelectionStart = 0;
                txtCardAmt.SelectionLength = txtCardAmt.Text.Length;
            }



        }

        private void eCtl_Click(object sender, EventArgs e)
        {

            #region //승인요청/승인취소 옵션버튼
            if (sender == this.rdoGubun0 || sender == this.rdoGubun1)
            {
                int intSearch = 0;

                Frm_Clear();

                if (rdoGubun0.Checked == true)
                    CC.GstrCardGubun = "0";
                else
                    CC.GstrCardGubun = "1";

                if (sender == this.rdoGubun0) //승인요청
                {
                    rdoGubun0.ForeColor = Color.FromArgb(255, 0, 0);
                    rdoGubun1.ForeColor = Color.FromArgb(0, 0, 132);

                    txtCardNo.Enabled = true; //카드번호
                    txtCardAmt.Enabled = true; // 금액
                    txtTrDate.Enabled = false; //매출일자
                    txtTrDate.BackColor = Color.FromArgb(224, 224, 224);
                    txtOrNo.Enabled = false; //승인번호
                    txtOrNo.BackColor = Color.FromArgb(224, 224, 224);
                    CC.gstrCdPCode = VB.Left(CC.gstrCdPCode, 3) + "+";
                    txtCardNo.Focus();
                }
                else //승인취소
                {
                    rdoGubun1.ForeColor = Color.FromArgb(255, 0, 0);
                    rdoGubun0.ForeColor = Color.FromArgb(0, 0, 132);

                    if (string.Compare(txtOrNo.Text.Trim(), CC.gstrCardOpenDate) < 0)
                    {
                        ComFunc.MsgBox(CC.gstrCardOpenDate + " 이전 DATA는 해당 카드사로 문의하시기 바랍니다.", "승인취소 불가");
                        return;
                    }

                    txtTrDate.Enabled = true;
                    txtTrDate.BackColor = Color.FromArgb(255, 255, 255);
                    txtOrNo.Enabled = true; //승인번호
                    txtOrNo.BackColor = Color.FromArgb(255, 255, 255);
                    CC.gstrCdPCode = VB.Left(CC.gstrCdPCode, 3) + "-";

                    if (CC.gstrCdRemark.Trim() != "")
                    {
                        txtCardFullNo.Text = VB.Left(CC.gstrCdRemark, 40);
                        intSearch = VB.InStr(txtCardFullNo.Text.Trim(), "=");
                        txtCardNo.Text = VB.Left(txtCardNo.Text.Trim(), intSearch - 1);

                        txtOrDate.Text = VB.Mid(CC.gstrCdRemark, 51, 10);
                        txtCardAmt.Text = VB.Mid(CC.gstrCdRemark, 61, 9);
                        CC.gstrOriginDate = VB.Mid(CC.gstrCdRemark, 51, 10); //원승인일자
                        btnApproval.Focus();
                    }
                }
            }
            #endregion

            #region //요청버튼
            if (sender == btnApproval)
            {
                REQUEST_PROCESS();
            }
            #endregion

            #region //취소버튼
            if (sender == this.btnCancel)
            {
                Frm_Clear();
            }
            #endregion

            #region //닫기버튼
            CC.CardVariable_Clear(ref RSD, ref RD);
            CA.Tablet_Shell_Check("tablet_d", "현금영수증"); //특정프로그램이 실행중인지 알아내기 위함
            this.Close();
            #endregion

            #region //영수발급/자진발급 옵션버튼
            if (sender == this.rdoBun0 || sender == this.rdoBun1)
            {
                if (sender == this.rdoBun0)
                {
                    txtCardNo.Text = txtPaNo.Text;
                    txtCardNo.Focus();
                }
                else
                {
                    txtCardNo.Text = "0100001234";
                }
            }
            #endregion

            #region //현금승인재발행
            if (sender == this.btnPrint)
            {
                string strTitle = string.Empty;
                long lngTotAmt = 0;

                if (chkLtd.Checked == true)
                    strTitle = "◈ 현 금 (지출증빙) ◈";
                else
                    strTitle = "◈ 현 금 (소득공제) ◈";

                lngTotAmt = Convert.ToInt64(txtCardAmt.Text);

                SQL = "";
                SQL += ComNum.VBLF + " SELECT FINAME, INSTPERIOD, PERIOD, ";
                SQL += ComNum.VBLF + "        ORIGINNO, CardSeqNo,  ";
                SQL += ComNum.VBLF + "        TO_CHAR(APPROVDATE,'YYYY-MM-DD HH24:MI') APPROVDATE ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + txtPtno.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND ORIGINNO  = '" + txtOrNo.Text.Trim() + "' ";
                SQL += ComNum.VBLF + "    AND Gubun     = '2' ";  //현금
                if (VB.Val(txtCardAmt.Text) > 0)
                    SQL += ComNum.VBLF + "AND TranHeader  ='1' ";
                else
                    SQL += ComNum.VBLF + "AND TranHeader  ='2' ";

                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Dt.Rows.Count > 0)
                {
                    CPP.Report_Print_ReceiptCash(strTitle, VB.Left(txtCardNo.Text.Trim(), 8) + "******", txtOrNo.Text.Trim(), Dt.Rows[0]["APPROVDATE"].ToString(), CC.gstrCdSName, CC.gstrCdPtno, lngTotAmt, ssPrint);
                }

                Dt.Dispose();
                Dt = null;
            }
            #endregion

            #region //현금영수증 거부 체크박스
            if (sender == this.chkCash)
            {
                string strYN = string.Empty;

                if (chkCash.Checked == true)
                    strYN = "1";
                else
                    strYN = "0";

                Cursor.Current = Cursors.WaitCursor;

                clsDB.setBeginTran(clsDB.DbCon);

                try
                {

                    SQL = "";
                    SQL += ComNum.VBLF + " UPDATE " + ComNum.DB_PMPA + "BAS_PATIENT ";
                    SQL += ComNum.VBLF + "    SET CashYN    = '" + strYN + "' ";
                    SQL += ComNum.VBLF + "  WHERE 1     = 1 ";
                    SQL += ComNum.VBLF + "    AND Pano  = '" + CC.gstrCdPtno + "' ";
                    SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                    if (SqlErr != "")
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        ComFunc.MsgBox(SqlErr);
                        clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                        Cursor.Current = Cursors.Default;
                        return;
                    }

                    clsDB.setCommitTran(clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(ex.Message);
                    clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

            }

            #endregion

            #region //타블렛실행
            if (sender == this.btnSign)
            {
                Process Proc = new Process(); //외부프로그램 실행
                string strName = string.Empty;

                if (txtSname.Text.Trim().Length == 3)
                    strName = VB.Left(txtSname.Text.Trim(), 2) + "○";
                else
                    strName = VB.Left(txtSname.Text.Trim(), 2) + "○○";

                if (clsPmpaPb.GstrCreditIF != "P")
                {
                    FileInfo fi = new FileInfo(clsPublic.GstrTabletD_FilePath);
                    if (fi.Exists) //파일존재
                    {
                        CA.Tablet_Shell_Check("tablet_d", "현금영수증"); //특정프로그램이 실행중인지 알아내기 위함
                        //Thread.Sleep(200);

                        if (rdoGubun1.Checked == true)
                        {
                            Proc.StartInfo.FileName = @"c:\cmc\exe\tablet_d.exe"; //실행할 파일명
                            Proc.StartInfo.Arguments = "환불님!" + txtCardAmt.Text.Trim() + "!";
                            Proc.Start();
                        }
                        else
                        {
                            Proc.StartInfo.FileName = @"c:\cmc\exe\tablet_d.exe";
                            Proc.StartInfo.Arguments = strName + txtCardAmt.Text.Trim() + "!";
                            Proc.Start();
                        }

                        if (CC.GstrCardJob == "Menual")
                            txtPtno.Focus();
                        else
                        {
                            txtCardNo.Enabled = true;
                            txtCardNo.Focus();
                        }
                    }
                }
            }
            #endregion

            if (sender == this.btnPead)//입력번호확인
            {
                
            }

            #region //패드번호입력요청
            if (sender == this.btnCash)
            {
                int rc = -1;

                String rep = "";
                byte[] rep_array = new byte[2049];

                Array.Clear(rep_array, 0, rep_array.Length);

                txtCardNo.Text = "";

                //rc = clsCard_Daou.svk_PAD(rep_array, 30);
                rep = System.Text.Encoding.Default.GetString(rep_array);

                if (rc < 0)
                {
                    Tx_Rep.Text = String.Format("오류코드 : {0}", rc);
                    txtStatus.Text = String.Format("오류코드 : {0}", rc);
                }
                else
                {
                    txtCardNo.Text = rep;
                }
            }
            #endregion


        }

        private void REQUEST_PROCESS()
        {
            string strBun = string.Empty;
            string strTrnGbn = string.Empty; //거래구분(1.지출증빙, 2.소득공제)
            string strCashCancel = string.Empty; //취소사유

            int rc = -1;
            StringBuilder req = new StringBuilder();
            string rep = "";
            byte[] req_array = new byte[2049];
            byte[] rep_array = new byte[2049];


            if (ComQuery.IsJobAuth(this, "C", clsDB.DbCon) == false) { return; }     //권한확인

            txtStatus.Text = "";

            strTrnGbn = (chkLtd.Checked == true) ? "2" : "1";
            if (rdoBun1.Checked == true)
                strTrnGbn = "3";

            CC.gstrCdPtno = txtPtno.Text.Trim();
            CC.gstrCdSName = txtSname.Text.Trim();
            CC.gstrCdDeptCode = VB.Left(cboDept.Text.Trim(), 2);
            CC.gstrCdPart = clsType.User.IdNumber.Trim();
            CC.gstrCdGbIo = VB.Left(cboIO.Text.Trim(), 1);

            if (VB.Val(txtCardAmt.Text) == 0)
            {
                ComFunc.MsgBox("금액을 입력하시기 바랍니다.", "확인");
                return;
            }
            else if (CC.gstrCdPtno == "")
            {
                ComFunc.MsgBox("등록번호를 입력하시기 바랍니다.", "확인");
                return;
            }
            else if (CC.gstrCdGbIo == "")
            {
                ComFunc.MsgBox("외래/입원을 선택하시기 바랍니다.", "확인");
                return;
            }
            else if (CC.gstrCdDeptCode == "")
            {
                ComFunc.MsgBox("진료과목을 선택하시기 바랍니다.", "확인");
                return;
            }
            else if (txtCardNo.Text.Trim() == "")
            {
                ComFunc.MsgBox("승인자료가 존재하지 않습니다.", "확인");
                return;
            }

            strCashCancel = "";
            if (rdoGubun0.Checked == true)
            {
                if (cboCancel.Text.Trim() != "")
                {
                    ComFunc.MsgBox("현금영수증 승인시 취소사유가 입력되면 안됩니다.", "확인");
                    return;
                }
            }
            else if (rdoGubun1.Checked == true)
            {
                if (cboCancel.Text.Trim() == "")
                {
                    ComFunc.MsgBox("현금영수증 승인시 취소사유를 선택해야 합니다.", "확인");
                    return;
                }

                strCashCancel = VB.Left(cboCancel.Text.Trim(), 1);
            }
            FstrCashCancel = strCashCancel;

            SELECT_JUMIN(CC.gstrCdPtno);

            if (rdoGubun0.Checked == true)
                FintCardApproveGbn = 1;
            else if (rdoGubun1.Checked == true)
                FintCardApproveGbn = 2;

            //현금영수증은 S:신용카드 K:주민등록번호, 사업자번호, 핸드폰등
            if (rdoBun0.Checked == true)
                strBun = "K";
            else if (rdoBun1.Checked == true)
                strBun = "S";

            if (FintCardApproveGbn == 1)
                txtStatus.Text = "승인 요청중입니다. 결과를 기다리십시오!";
            else
                txtStatus.Text = "승인취소 요청중입니다.결과를 기다리십시오!";

            btnApproval.Enabled = false;
            btnCancel.Enabled = false;
            btnExit.Enabled = false;

            clsPmpaType.RSD.OrderGb = FintCardApproveGbn.ToString(); //승인요청
            clsPmpaType.RSD.EntryMode = strBun; //카드처리구분(영수발급,자진발급)
            clsPmpaType.RSD.CardData = txtCardNo.Text.Trim(); //카드번호
            clsPmpaType.RSD.CardData2 = txtCardFullNo.Text.Trim();
            clsPmpaType.RSD.CashBun = ""; //개인/사업자구분(자동인식으로 무시)
            clsPmpaType.RSD.SEQNO = CPF.GET_NEXT_CDSEQNO(clsDB.DbCon); //거래일련번호 가져오기
            clsPmpaType.RSD.AApproveNo = txtOrNo.Text.Trim(); //승인번호(취소시)
            clsPmpaType.RSD.ASaleDate = string.Format("{0:YYYYMMDD}", txtOrDate.Text.Trim()); //승인일자(취소시)
            clsPmpaType.RSD.Gubun = "2"; //현금영수증

            if (Tx_DeviceNo.Text.Length != 8)
            {
                ComFunc.MsgBox("단말기 번호가 양식에 맞지 않습니다.");
                return;
            }

            //if (FintCardApproveGbn == 1) //승인요청
            //{
            //    req = CD.insertLeftJustify(req, "0200", 4); // 전문구분
            //    req = CD.insertLeftJustify(req, "40", 2); // 업무구분
            //    req = CD.insertLeftJustify(req, Tx_DeviceNo.Text, 8); // 단말기번호
            //    req = CD.insertLeftZero(req, "0000", 4); // 전표번호

            //    if (txtCardNo.Text.Contains("=") == true) // WCC
            //        req = CD.insertLeftJustify(req, "A", 1);
            //    else
            //        req = CD.insertLeftJustify(req, "@", 1);

            //    req = CD.insertLeftJustify(req, txtCardNo.Text, 37); // 카드정보
            //    req = CD.insertLeftJustify(req, strTrnGbn, 1); // 거래구분
            //    req = CD.insertLeftJustify(req, txtCardAmt.Text, 12); // 거래금액
            //    req = CD.insertLeftJustify(req, "", 12); // 봉사료
            //    req = CD.insertLeftJustify(req, "", 12); // 세금
            //    req = CD.insertLeftJustify(req, "", 12); // 원승인번호
            //    req = CD.insertLeftJustify(req, "", 8); // 원거래일자
            //    req = CD.insertLeftJustify(req, "", 1); // 취소사유코드
            //    req = CD.insertLeftJustify(req, "", 12); // 비과세 처리
            //}
            //else if (FintCardApproveGbn == 2) //승인취소
            //{
            //    req = CD.insertLeftJustify(req, "0420", 4); // 전문구분
            //    req = CD.insertLeftJustify(req, "40", 2); // 업무구분
            //    req = CD.insertLeftJustify(req, Tx_DeviceNo.Text, 8); // 단말기번호
            //    req = CD.insertLeftZero(req, "0000", 4); // 전표번호

            //    if (txtCardNo.Text.Contains("=") == true) // WCC
            //        req = CD.insertLeftJustify(req, "A", 1);
            //    else
            //        req = CD.insertLeftJustify(req, "@", 1);

            //    req = CD.insertLeftJustify(req, txtCardNo.Text, 37); // 카드정보
            //    req = CD.insertLeftJustify(req, strTrnGbn, 1); // 거래구분
            //    req = CD.insertLeftJustify(req, txtCardAmt.Text, 12); // 거래금액
            //    req = CD.insertLeftJustify(req, "", 12); // 봉사료
            //    req = CD.insertLeftJustify(req, "", 12); // 세금
            //    req = CD.insertLeftJustify(req, txtOrNo.Text, 12); // 원승인번호
            //    req = CD.insertLeftJustify(req, txtTrDate.Text, 8); // 원거래일자
            //    req = CD.insertLeftJustify(req, VB.Left(cboCancel.Text, 1), 1); // 취소사유코드
            //}
            //else
            //{
            //    MessageBox.Show("오류");
            //    return;
            //}

            // 요청전문 출력
            Tx_Req.Text = req.ToString();

            if (clsPmpaPb.GstrCreditIF == "T") //Tablet
            {
                req_array = System.Text.Encoding.UTF8.GetBytes(req.ToString());
                //rc = CD.SvkPosSign(req_array, "", 0, rep_array);
                rep = System.Text.Encoding.Default.GetString(rep_array);
            }
            else //SignPad
            {
                req_array = System.Text.Encoding.UTF8.GetBytes(req.ToString());
                //rc = CD.SvkPos(req_array, rep_array);
                rep = System.Text.Encoding.Default.GetString(rep_array);
            }

            ClearResultGridView();

            if (rc < 0)
            {
                Tx_Rep.Text = String.Format("오류코드 : {0}", rc);
                txtStatus.Text = String.Format("오류코드 : {0}", rc);
                btnExit.Enabled = true;
            }
            else
            {
                Tx_Rep.Text = rep;

                //CD.SetResultRep(resultGridView, rep);

                //clsPmpaType.RSD.TotAmt = Convert.ToInt64(string.Format("{0:#########}", txtCardAmt.Text));
                //clsPmpaType.RD.OrderGb = FintCardApproveGbn.ToString();
                //clsPmpaType.RD.MDate = CD.FstrMdate; //승인일시
                //clsPmpaType.RD.ApprovalNo = CD.FstrApprovalNo; //승인번호

                //if (clsPmpaType.RD.ApprovalNo.Trim() == "") //승인메세지
                //{
                //    txtStatus.Text = CD.FstrApprovalRemark;
                //    btnApproval.Enabled = true;
                //    btnCancel.Enabled = true;
                //    btnExit.Enabled = true;
                //    return;
                //}

                if (FintCardApproveGbn == 1)
                    txtStatus.Text = "거래승인이 정상적으로 처리되었습니다.";
                else
                    txtStatus.Text = "정상적으로 취소 처리되었습니다.";

                if (FintCardApproveGbn == 1)
                {
                    Card.GstrCashCard = txtCardNo.Text.Trim();

                    INSERT_CARDAPPROV();

                    if (chkPrt.Checked == true)
                        RECEIPT_PRINT();
                }
                else
                {
                    Card.GstrCashCard = txtCardNo.Text.Trim();

                    INSERT_CARDAPPROV();

                    if (chkPrt.Checked == true)
                    {
                        if (CC.GstrCardJob == "Menual")
                            RECEIPT_PRINT();
                    }
                }

                CA.Tablet_Shell_Check("tablet_d", "현금영수증"); //특정프로그램이 실행중인지 알아내기 위함

                this.Close();
            }
        }

        private void RECEIPT_PRINT()
        {
            string strTitle = string.Empty;
            long lngTotAmt = 0;

            if (chkLtd.Checked == true)
                strTitle = "◈ 현 금 (지출증빙) ◈";
            else
                strTitle = "◈ 현 금 (소득공제) ◈";

            if (FintCardApproveGbn == 1)
                lngTotAmt = clsPmpaType.RSD.TotAmt;
            else
                lngTotAmt = clsPmpaType.RSD.TotAmt * -1;

            CPP.Report_Print_ReceiptCash(strTitle, VB.Left(txtCardNo.Text.Trim(), 8) + "******", clsPmpaType.RD.ApprovalNo.Trim(), clsPmpaType.RD.MDate, CC.gstrCdSName, CC.gstrCdPtno, lngTotAmt, ssPrint);
        }

        private void INSERT_CARDAPPROV()
        {
            string strTranDate = string.Empty;
            string strOrignDate = string.Empty;
            string strRespDate = string.Empty;
            string strSel = string.Empty;

            strSel = (rdoBun0.Checked == true) ? "1" : "0";
            clsPmpaType.RSD.CardSeqNo = CPF.GET_NEXT_CARDSEQNO(clsDB.DbCon);

            if (VB.Left(clsPmpaType.RD.MDate, 2) != "00") //승인일시
            {
                clsPmpaType.RD.MDate = VB.Left(clsPmpaType.RD.MDate, 4) + "-";
                clsPmpaType.RD.MDate += VB.Mid(clsPmpaType.RD.MDate, 5, 2) + "-" + VB.Mid(clsPmpaType.RD.MDate, 7, 2) + " ";
                clsPmpaType.RD.MDate += VB.Mid(clsPmpaType.RD.MDate, 9, 2) + ":" + VB.Mid(clsPmpaType.RD.MDate, 11, 2);
            }

            if (clsPmpaType.RSD.OldAppDate.Trim() != "") { strOrignDate = clsPmpaType.RSD.OldAppDate; }
            if (clsPmpaType.RD.ApprovalTime.Trim() != "") { strRespDate = clsPmpaType.RD.ApprovalDate + clsPmpaType.RD.ApprovalTime; } //승인일시

            Cursor.Current = Cursors.WaitCursor;

            clsDB.setBeginTran(clsDB.DbCon);

            try
            {
                SQL = "";
                SQL += ComNum.VBLF + " INSERT INTO " + ComNum.DB_PMPA + "CARD_APPROV ";
                SQL += ComNum.VBLF + "        (ActDate, CDSEQNO,  Pano,             --회계일자,거래일련번호,등록번호";
                SQL += ComNum.VBLF + "         Jumin, SName, GbIo,                  --주민번호,성명,입/외";
                SQL += ComNum.VBLF + "         DeptCode, PCode, Part,               --진료과,카드수납구분,수납조";
                SQL += ComNum.VBLF + "         TranHeader, CardSeqNo, Remark,       --거래구분,승인일련번호,비고";
                SQL += ComNum.VBLF + "         TranDate, InputMethod, TradeAmt,     --거래일시,입력구분(S:Swipe, K:Key_in),총금액";
                SQL += ComNum.VBLF + "         ApprovDate, OriginNo, OriginNo2,     --승인일시,(원)승인번호,현금승인관련 원승인번호";
                SQL += ComNum.VBLF + "         OriginDate, CardNo, FullCardNo,      --원거래일자,카드번호,전체카드번호";
                SQL += ComNum.VBLF + "         EntSabun, EnterDate, GUBUN,          --입력자사번,입력일시,(1.카드2.현금)";
                SQL += ComNum.VBLF + "         CASHBUN, PtGubun, CashCan,           --사용안함,3.다우카드,현금취소사유";
                SQL += ComNum.VBLF + "         Sel)                                 --1.영수발급, 1이외.자진발급";
                SQL += ComNum.VBLF + " VALUES (TO_DATE('" + clsPublic.GstrSysDate.Trim() + "' ,'YYYY-MM-DD'),  ";  //회계일자
                SQL += ComNum.VBLF + "         " + clsPmpaType.RSD.SEQNO + ", "; //거래일련번호
                SQL += ComNum.VBLF + "         '" + CC.gstrCdPtno.Trim() + "', "; //등록번호
                SQL += ComNum.VBLF + "         '" + CC.gstrCdJumin.Trim() + "', "; // 주민번호
                SQL += ComNum.VBLF + "         '" + CC.gstrCdSName.Trim() + "', "; // 성명
                SQL += ComNum.VBLF + "         '" + CC.gstrCdGbIo.Trim() + "', "; // 입/외
                SQL += ComNum.VBLF + "         '" + CC.gstrCdDeptCode.Trim() + "', "; // 진료과
                SQL += ComNum.VBLF + "         '" + CC.gstrCdPCode.Trim() + "', "; // 카드수납구분
                SQL += ComNum.VBLF + "         '" + CC.gstrCdPart.Trim() + "', "; // 입력조
                SQL += ComNum.VBLF + "         '" + clsPmpaType.RD.OrderGb.Trim() + "', "; // 거래구분
                SQL += ComNum.VBLF + "         " + clsPmpaType.RSD.CardSeqNo + ", "; // 승인일련번호
                SQL += ComNum.VBLF + "         '" + CC.gstrRemark.Trim() + "', "; // 비고
                SQL += ComNum.VBLF + "         TO_DATE('" + clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime + "' ,'YYYY-MM-DD HH24:MI'),  "; // 거래일시
                SQL += ComNum.VBLF + "         '" + clsPmpaType.RSD.EntryMode.Trim() + "', "; // 입력구분
                SQL += ComNum.VBLF + "         " + clsPmpaType.RSD.TotAmt + ", "; // 총금액
                SQL += ComNum.VBLF + "         TO_DATE('" + clsPmpaType.RD.MDate + "' ,'YYYY-MM-DD HH24:MI'), "; // 승인일시
                SQL += ComNum.VBLF + "         '" + clsPmpaType.RD.ApprovalNo.Trim() + "', "; // (원)승인번호

                if (FintCardApproveGbn == 1)
                    SQL += ComNum.VBLF + "        '" + clsPmpaType.RD.ApprovalNo.Trim() + "', "; // (원)승인번호2-승인시
                else if (FintCardApproveGbn == 2)
                    SQL += ComNum.VBLF + "        '" + clsPmpaType.RD.ApprovalNo.Trim() + "', "; // (원)승인번호2 -취소시


                SQL += ComNum.VBLF + "        TO_DATE('" + string.Format("{0:YYYY-MM-DD", clsPmpaType.RD.MDate) + "' ,'YYYY-MM-DD'), "; // 원거래일자
                SQL += ComNum.VBLF + "        '" + txtCardNo.Text.Trim() + "', "; // 카드번호
                SQL += ComNum.VBLF + "        '" + txtCardFullNo.Text.Trim() + "', "; // 전체카드번호
                SQL += ComNum.VBLF + "        '" + clsType.User.IdNumber + "', "; // 입력자사번
                SQL += ComNum.VBLF + "        SysDate, "; // 입력일자
                SQL += ComNum.VBLF + "        '" + clsPmpaType.RSD.Gubun + "', "; // 카드 현금 구분
                SQL += ComNum.VBLF + "        '" + clsPmpaType.RSD.CashBun + "', ";
                SQL += ComNum.VBLF + "        '3', ";
                SQL += ComNum.VBLF + "        '" + FstrCashCancel + "', ";
                SQL += ComNum.VBLF + "        '" + strSel + "' )  "; // 현금영수증 구분
                SqlErr = clsDB.ExecuteNonQuery(SQL, ref intRowCnt, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    ComFunc.MsgBox(SqlErr);
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                clsDB.setCommitTran(clsDB.DbCon);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                ComFunc.MsgBox(ex.Message);
                clsDB.SaveSqlErrLog(ex.Message, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }
        }

        private void ClearResultGridView()
        {
            resultGridView.Rows.Clear();
        }

        private void Frm_Clear()
        {
            txtCardNo.Text = "";
            txtCardAmt.Text = "0";
            txtCardFullNo.Text = "";
            txtTrDate.Text = "";
            txtOrNo.Text = "";
            txtOrDate.Text = "";
            txtAccName.Text = "";
            btnApproval.Enabled = true;
        }

        private void eFrm_Load(object sender, EventArgs e)
        {
            if (ComQuery.isFormAuth(clsDB.DbCon, this) == false) { this.Close(); }       //Form 권한조회
            ComFunc.SetFormInit(clsDB.DbCon, this, "Y", "Y", "Y");                       //Form 기본값 셋팅

            ComFunc.ReadSysDate(clsDB.DbCon);

            frmPmpaEntryCashDaou frm = new frmPmpaEntryCashDaou();

            if (clsPmpaPb.GstrCreditIF != "P")
            {
                btnCash.Visible = false;
                btnSign.Visible = true;
                btnPead.Visible = true;
            }
            else
            {
                btnCash.Visible = true;
                btnSign.Visible = false;
                btnPead.Visible = false;
            }

            frm.Height = 470;

            cboIO.Items.Clear();
            cboIO.Items.Add("O.외래");
            cboIO.Items.Add("I.입원");
            cboIO.Items.Add("H.건진");
            cboIO.Items.Add("T.종검");
            cboIO.SelectedIndex = 0;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT DeptCode ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "BAS_ClinicDept ";
            SQL += ComNum.VBLF + "  WHERE 1 = 1 ";
            SQL += ComNum.VBLF + "    AND PRINTRANKING < 30 ";
            SQL += ComNum.VBLF + "    AND DEPTCODE <> 'PT' ";
            SQL += ComNum.VBLF + "  ORDER BY PrintRanking ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                ComFunc.MsgBox("조회중 문제가 발생했습니다");
                Cursor.Current = Cursors.Default;
                return;
            }

            cboDept.Items.Clear();

            for (int i = 0; i < Dt.Rows.Count; i++)
                cboDept.Items.Add(Dt.Rows[i]["DeptCode"].ToString().Trim());

            Dt.Dispose();
            Dt = null;
            cboDept.SelectedIndex = -1;

            cboGubun.Items.Clear();
            cboGubun.Items.Add("00.개인");
            cboGubun.Items.Add("01.사업자");
            cboGubun.SelectedIndex = 0;

            cboCancel.Items.Clear();
            cboCancel.Items.Add("1.거래취소");
            cboCancel.Items.Add("2.오류발급");
            cboCancel.Items.Add("3.기타");
            cboCancel.SelectedIndex = 0;

            TEXT_CLEAR();

            btnPrint.Enabled = false;
            chkPrt.Checked = false;
            if (Card.GstrCardPrtChk == "Y")
                chkPrt.Checked = true;

            chkLtd.Checked = false;

            ClearResultGridView();

            txtPaNo.Text = "";
            if (CC.gstrCdPtno != "")
            {
                SQL = "";
                SQL += ComNum.VBLF + " SELECT CARDNO ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                SQL += ComNum.VBLF + "  WHERE 1         = 1 ";
                SQL += ComNum.VBLF + "    AND PANO      = '" + CC.gstrCdPtno + "' ";
                SQL += ComNum.VBLF + "    AND ACTDATE   = (SELECT MAX(ACTDATE) ";
                SQL += ComNum.VBLF + "                       FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                SQL += ComNum.VBLF + "                      WHERE 1     = 1 ";
                SQL += ComNum.VBLF + "                        AND PANO  = '" + CC.gstrCdPtno + "' ";
                SQL += ComNum.VBLF + "                        AND GUBUN = '2'";
                SQL += ComNum.VBLF + "                        AND CARDNO <> '0100001234') ";
                SQL += ComNum.VBLF + "   AND GUBUN      = '2'";
                SQL += ComNum.VBLF + "   AND CARDNO     <> '0100001234'";
                SQL += ComNum.VBLF + " ORDER BY ENTERDATE DESC ";
                SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon); //에러로그 저장
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (Dt.Rows.Count > 0)
                    txtPaNo.Text = Dt.Rows[0]["CARDNO"].ToString().Trim();

                Dt.Dispose();
                Dt = null;

                //현금영수증 거부인지 체크
                Dt = CPF.Get_BasPatient(clsDB.DbCon, CC.gstrCdPtno);
                if (Dt.Rows[0]["CashYN"].ToString().Trim() == "1")
                    chkCash.Checked = true;
                else
                    chkCash.Checked = false;

                Dt.Dispose();
                Dt = null;
            }

            if (chkCash.Checked == true)
                eCtl_Click(rdoBun1, null);

            if (CC.gstrCdPCode == "IMAN") //어디에서 카드수납했는지구분
            {
                txtPtno.Text = CC.gstrCdPtno;
                SELECT_JUMIN(CC.gstrCdPtno);
                CARD_APPROV_DISPLAY(CC.gstrCdPtno);

                for (int i = 0; i < cboDept.Items.Count; i++)
                {
                    cboDept.SelectedIndex = i;
                    if (VB.Left(cboDept.Text.Trim(), 2) == CC.gstrCdDeptCode)
                        break;
                    else
                        cboDept.SelectedIndex = 0;
                }

                for (int i = 0; i < cboIO.Items.Count; i++)
                {
                    cboIO.SelectedIndex = i;
                    if (VB.Left(cboIO.Text.Trim(), 2) == CC.gstrCdGbIo)
                        break;
                    else
                        cboIO.SelectedIndex = 0;
                }
            }
            else if (CC.GstrCardJob == "Menual") //작업구분(Menual,Approv,Cancel)
            {
                txtPtno.Text = CC.gstrCdPtno;

                SELECT_JUMIN(CC.gstrCdPtno);
                CARD_APPROV_DISPLAY(CC.gstrCdPtno);
            }
            else if (CC.GstrCardJob == "Menual2") //작업구분(Menual,Approv,Cancel)
            {
                txtPtno.Text = CC.gstrCdPtno;
                txtCardAmt.Text = CC.glngCdAmt.ToString();

                SELECT_JUMIN(CC.gstrCdPtno);
                CARD_APPROV_DISPLAY(CC.gstrCdPtno);

                for (int i = 0; i < cboDept.Items.Count; i++)
                {
                    cboDept.SelectedIndex = i;
                    if (VB.Left(cboDept.Text.Trim(), 2) == CC.gstrCdDeptCode)
                        break;
                    else
                        cboDept.SelectedIndex = 0;
                }

                for (int i = 0; i < cboIO.Items.Count; i++)
                {
                    cboIO.SelectedIndex = i;
                    if (VB.Left(cboIO.Text.Trim(), 2) == CC.gstrCdGbIo)
                        break;
                    else
                        cboIO.SelectedIndex = 0;
                }
            }

            if (CC.GstrCardJob == "Menual")
                txtPtno.Focus();

            if (txtPaNo.Text != "")
                rdoBun0.Checked = true;
            else
                rdoBun1.Checked = true;

            eCtl_Click(btnSign, null);
        }

        private void TEXT_CLEAR()
        {
            txtCardNo.Text = "";
            txtCardAmt.Text = "0";
            txtCardFullNo.Text = "";
            txtTrDate.Text = "";
            txtOrNo.Text = "";
            txtOrDate.Text = "";
            txtAccName.Text = "";
            txtPtno.Text = "";
        }

        private void CARD_APPROV_DISPLAY(string ArgPtno)
        {
            string strFlag = string.Empty;

            SQL = "";
            SQL += ComNum.VBLF + " SELECT Decode(tranheader,'1',TRADEAMT,'2', TRADEAMT * -1 ) TRADEAMT, ";
            SQL += ComNum.VBLF + "        PtGubun, Sel, DeptCode, ";
            SQL += ComNum.VBLF + "        GbIO, OriginNo, OriginNo2, ";
            SQL += ComNum.VBLF + "        TO_CHAR(TranDate,'YYYYMMDD') TranDate, ";
            SQL += ComNum.VBLF + "        TranHeader ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
            SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
            SQL += ComNum.VBLF + "    AND Pano          = '" + ArgPtno + "' ";
            SQL += ComNum.VBLF + "    AND ActDate       >= TRUNC(SYSDATE-365) ";
            SQL += ComNum.VBLF + "    AND GUBUN         = '2' ";
            SQL += ComNum.VBLF + "    AND INPUTMETHOD   <> 'T' ";
            SQL += ComNum.VBLF + "  ORDER BY ActDate DESC ,OriginNo2 DESC ,TranDate DESC ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                ComFunc.MsgBox("해당 DATA가 없습니다.");
                Dt.Dispose();
                Dt = null;
                Cursor.Current = Cursors.Default;
                return;
            }

            ssList_Sheet1.RowCount = Dt.Rows.Count;
            ssList_Sheet1.SetRowHeight(-1, ComNum.SPDROWHT);

            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                //이미 승인취소를 하였는지 점검
                SQL = "";
                SQL += ComNum.VBLF + " SELECT OriginDate ";
                SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
                SQL += ComNum.VBLF + "  WHERE 1             = 1 ";
                SQL += ComNum.VBLF + "    AND Pano          = '" + ArgPtno + "' ";
                SQL += ComNum.VBLF + "    AND OriginNo2     = '" + Dt.Rows[i]["OriginNo"].ToString().Trim() + "' ";
                SQL += ComNum.VBLF + "    AND TranHeader    ='2' "; //승인취소
                SQL += ComNum.VBLF + "    AND INPUTMETHOD   <> 'T' ";
                SQL += ComNum.VBLF + "    AND GUBUN         = '2' "; //카드
                SqlErr = clsDB.GetDataTable(ref DtSub, SQL, clsDB.DbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                    Cursor.Current = Cursors.Default;
                    return;
                }

                if (DtSub.Rows.Count > 0)
                    strFlag = "Y";

                DtSub.Dispose();
                DtSub = null;


                ssList_Sheet1.Cells[i, 0].Text = Dt.Rows[i]["TranDate"].ToString().Trim();
                ssList_Sheet1.Cells[i, 1].Text = Dt.Rows[i]["TradeAmt"].ToString().Trim();
                ssList_Sheet1.Cells[i, 2].Text = Dt.Rows[i]["DeptCode"].ToString().Trim();
                ssList_Sheet1.Cells[i, 3].Text = Dt.Rows[i]["GbIO"].ToString().Trim();
                ssList_Sheet1.Cells[i, 4].Text = Dt.Rows[i]["OriginNo"].ToString().Trim();

                if (Dt.Rows[i]["PtGubun"].ToString().Trim() == "3")
                    ssList_Sheet1.Cells[i, 5].Text = "";
                else
                    ssList_Sheet1.Cells[i, 5].Text = "코세스";

                if (Dt.Rows[i]["TranHeader"].ToString().Trim() == "1") //승인요청
                {
                    if (strFlag == "Y")
                    {
                        strFlag = "Y";
                        ssList_Sheet1.Cells[i, 6].Text = "승인";
                    }
                    else
                    {
                        strFlag = "";
                        ssList_Sheet1.Cells[i, 6].Text = "취소";
                    }
                }
                else if (Dt.Rows[i]["TranHeader"].ToString().Trim() == "2") //취소요청
                {
                    strFlag = "Y";
                    ssList_Sheet1.Cells[i, 6].Text = "승인";
                }

                ssList_Sheet1.Cells[i, 7].Text = Dt.Rows[i]["OriginNo2"].ToString().Trim();
                ssList_Sheet1.Cells[i, 8].Text = Dt.Rows[i]["Sel"].ToString().Trim();
            }

            Dt.Dispose();
            Dt = null;
        }

        private void SELECT_JUMIN(string ArgPtno)
        {
            Dt = CPF.Get_BasPatient(clsDB.DbCon, CC.gstrCdPtno);

            if (Dt.Rows.Count > 0)
            {
                CC.gstrCdJumin = Dt.Rows[0]["JUMIN1"].ToString().Trim() + "*******";
                CC.gstrCdPtno = ArgPtno;
                CC.gstrCdSName = Dt.Rows[0]["SNAME"].ToString().Trim();
                txtSname.Text = CC.gstrCdSName;
            }

            Dt.Dispose();
            Dt = null;
        }

        private void ssList_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            string strTranDate = string.Empty;
            string strOriginNo = string.Empty;
            string strCardCompany = string.Empty;
            string strFlag = string.Empty;
            string strSel = string.Empty;
            string strCardNo = string.Empty;
            string strFullCardNo = string.Empty;
            string strDeptCode = string.Empty;
            string strGbIO = string.Empty;

            strTranDate = VB.Left(ssList_Sheet1.Cells[e.Row, 0].Text, 4) + "-";
            strTranDate += VB.Mid(ssList_Sheet1.Cells[e.Row, 0].Text, 5, 2) + "-";
            strTranDate += VB.Mid(ssList_Sheet1.Cells[e.Row, 0].Text, 7, 2);

            strOriginNo = ssList_Sheet1.Cells[e.Row, 4].Text.Trim(); //승인번호
            strCardCompany = ssList_Sheet1.Cells[e.Row, 5].Text.Trim(); //카드사
            strFlag = ssList_Sheet1.Cells[e.Row, 6].Text.Trim(); //구분
            strSel = ssList_Sheet1.Cells[e.Row, 8].Text.Trim(); //자진발급유무

            rdoGubun1.Checked = true;
            if (strSel == "1") { rdoBun0.Checked = true; }

            cboCancel.Text = "";

            if (strFlag == "Y")
            {
                btnApproval.Enabled = false;
                btnCancel.Enabled = false;
                ComFunc.MsgBox("기 취소승인 자료입니다. 영수증 재발행만 가능합니다.", "확인");
            }
            else
            {
                if (strCardCompany == "코세스")
                {
                    btnApproval.Enabled = false;
                    btnCancel.Enabled = false;
                    ComFunc.MsgBox("코세스에서 거래된 자료입니다. 영수증 재발행만 가능합니다.", "확인");
                }
            }

            SQL = "";
            SQL += ComNum.VBLF + " SELECT PCODE, GBIO, DEPTCODE, ";
            SQL += ComNum.VBLF + "        CardNo, FullCardNo, Period, ";
            SQL += ComNum.VBLF + "        TradeAmt, InstPeriod, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ORIGINDATE,'YYYY-MM-DD') ORIGINDATE, ";
            SQL += ComNum.VBLF + "        TO_CHAR(ACTDATE,'YYYYMMDD') ACTDATE, ";
            SQL += ComNum.VBLF + "        DeptCode, GbIO  ";
            SQL += ComNum.VBLF + "   FROM " + ComNum.DB_PMPA + "CARD_APPROV ";
            SQL += ComNum.VBLF + "  WHERE 1                 = 1 ";
            SQL += ComNum.VBLF + "    AND Pano              = '" + txtPtno.Text + "' ";
            SQL += ComNum.VBLF + "    AND TRUNC(TranDate)   = TO_DATE('" + strTranDate + "','YYYY-MM-DD') ";
            SQL += ComNum.VBLF + "    AND OriginNo          = '" + strOriginNo + "' ";
            SQL += ComNum.VBLF + "    AND TranHeader        = '1' ";
            SqlErr = clsDB.GetDataTable(ref Dt, SQL, clsDB.DbCon);

            if (SqlErr != "")
            {
                ComFunc.MsgBox("조회중 문제가 발생되었습니다.");
                clsDB.SaveSqlErrLog(SqlErr, SQL, clsDB.DbCon);
                Cursor.Current = Cursors.Default;
                return;
            }

            if (Dt.Rows.Count == 0)
            {
                Dt.Dispose();
                Dt = null;
                ComFunc.MsgBox("매출일자 또는 승인번호가 잘못되었습니다.", "확인");
                txtTrDate.Focus();
                return;
            }
            else
            {
                CC.gstrCdGbIo = Dt.Rows[0]["GBIO"].ToString().Trim();
                CC.gstrCdDeptCode = Dt.Rows[0]["DEPTCODE"].ToString().Trim();
                CC.gstrCdPCode = VB.Left(Dt.Rows[0]["PCODE"].ToString().Trim(), 3) + "-";
                CC.gstrCdPart = clsType.User.IdNumber;

                strCardNo = Dt.Rows[0]["CardNo"].ToString().Trim();
                strFullCardNo = Dt.Rows[0]["FullCardNo"].ToString().Trim();
                strDeptCode = Dt.Rows[0]["DeptCode"].ToString().Trim();
                strGbIO = Dt.Rows[0]["GbIO"].ToString().Trim();

                txtOrNo.Text = strOriginNo;
                txtOrDate.Text = Dt.Rows[0]["ORIGINDATE"].ToString().Trim();
                txtCardNo.Text = strCardNo;
                txtTrDate.Text = Dt.Rows[0]["ACTDATE"].ToString().Trim();
                txtCardFullNo.Text = strFullCardNo;
                txtCardAmt.Text = Dt.Rows[0]["TradeAmt"].ToString().Trim();

                cboCancel.Text = "1.거래취소";
            }

            Dt.Dispose();
            Dt = null;

            for (int i = 0; i < cboDept.Items.Count; i++)
            {
                cboDept.SelectedIndex = i;
                if (cboDept.Text.Trim() == strDeptCode)
                    break;
                else
                    cboDept.SelectedIndex = 0;
            }

            for (int i = 0; i < cboIO.Items.Count; i++)
            {
                cboIO.SelectedIndex = i;
                if (cboIO.Text.Trim() == strGbIO)
                    break;
                else
                    cboIO.SelectedIndex = 0;
            }

            btnPrint.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}
