using ComBase;
using ComBase.Properties;
using ComDbB;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace ComMedLibB
{
    public partial class frmMedScreenOrderResult : Form
    {
        private bool OrderSendFlag = false;

        private int nRowAffected = 0;
        private StringBuilder SQL;
        private DataSet ds;
        int pResultCnt;
        int pResultCntDam;

        DataTable dtResult;
        DataTable dtPatient;
        DataTable dtOrder;
        DataTable dtResultDam;

        private string strPrscCiCode;           //처방조제유형코드
        private clsDur.AddMedicine[] DurMed;    //약정보
        private clsDur.ResultInfo[] RsInfo;     //심평원점검결과
        private Dictionary<string, string> DurType;

        clsMedFunction CMF = null;
        frmMedScreenOrderResultDetail frmMedScreenOrderResultDetailx = null;

        public frmMedScreenOrderResult(clsDur.AddMedicine[] argDurMed, clsDur.ResultInfo[] argRsInfo, string argPrscCiCode)
        {
            InitializeComponent();

            DurMed = argDurMed;
            RsInfo = argRsInfo;

            DurType = new Dictionary<string, string>();
            DurTypeInit();

            strPrscCiCode = argPrscCiCode;
        }

        /// <summary>
        /// DUR점검코드 -> 항목 Value
        /// </summary>
        private void DurTypeInit()
        {
            DurType.Add("00", "처방전내병용금기");
            DurType.Add("01", "처방전내연령금기");
            DurType.Add("02", "처방전내안전성");
            DurType.Add("03", "처방전내용량주의(최대용량)");
            DurType.Add("04", "처방전내투여기간주의(최대기간)");
            DurType.Add("05", "처방전내비용효과적인함량,");
            DurType.Add("06", "처방전내임부금기");
            DurType.Add("07", "처방전간병용금기");
            DurType.Add("08", "처방전간동일성분중복");
            DurType.Add("20", "처방전내효능군중복");
            DurType.Add("40", "처방전간효능군중복");
            DurType.Add("21", "처방전내특정기간병용금기");
            DurType.Add("41", "처방전간특정기간병용금기");
            DurType.Add("22", "처방전내선성분병용금기");
            DurType.Add("24", "처방전내분할주의");
            DurType.Add("25", "처방전내노인주의");
            DurType.Add("26", "처방전내약제허가사항주의");
            DurType.Add("42", "처방전간선성분병용금기");
            DurType.Add("23", "처방전내연령성별제한병용금기");
            DurType.Add("43", "처방전간연령성별제한병용금기");
            DurType.Add("90", "특정질병");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int nResult;
            int nChk = 0;
            string strTemp = "";

            for (int i = 0; i < ssDurResult.ActiveSheet.Rows.Count; i++)
            {
                nChk = 0;
                int nResultIndex = (int)VB.Val(ssDurResult.ActiveSheet.Cells[i, 7].Text);
                string strLevel = ssDurResult.ActiveSheet.Cells[i, 1].Text;
                string strSayuCd = ssDurResult.ActiveSheet.Cells[i, 4].Text;
                string strSayu = ssDurResult.ActiveSheet.Cells[i, 5].Text;

                if (ssDurResult.ActiveSheet.Cells[i, 4].Text.Length != 1 && ssDurResult.ActiveSheet.Cells[i, 4].Text != "정보제공")
                {
                    strSayuCd = VB.Left(strSayuCd, 1);
                }

                if ((strSayuCd == "T") && string.IsNullOrWhiteSpace(strSayu)
                        || (strSayuCd == "T") && strSayu == "")
                {
                    MessageBox.Show("심평원DUR B등급 처방결과에 사유를 입력하여 주십시오. \r\n 사유코드 T인 항목에 사유를 입력하여 주십시오.");
                    return;
                }

                //2020-09-02 안정수 추가
                if (strSayuCd == "T")
                {
                    if (strSayu.Length < 2 || strSayu == "t" || strSayu == "T")
                    {
                        MessageBox.Show((i + 1) + "번째 행의 사유를 정확하게 입력해주십시오.");
                        return;
                    }

                    for (int k = 0; k <= strSayu.Length; k++)
                    {
                        if (k == 0)
                        {

                        }
                        else
                        {
                            strTemp = VB.Mid(strSayu, k - 1, 1);
                        }

                        if (strTemp != "")
                        {
                            if (strTemp == VB.Mid(strSayu, k, 1))
                            {
                                nChk += 1;
                            }
                        }
                    }

                    if (nChk > 1 && strSayu.Length < 3)
                    {
                        MessageBox.Show((i + 1) + "번째 행의 사유를 정확하게 입력해주십시오.");
                        return;
                    }
                }
                else
                {
                    if (strSayuCd != "" && strSayuCd.Length < 2)
                    {
                        if (Convert.ToInt32(Convert.ToChar(strSayuCd)) >= 65 && Convert.ToInt32(Convert.ToChar(strSayuCd)) <= 90)
                        {
                            if (strSayu.Length == 1)
                            {
                                MessageBox.Show((i + 1) + "번째 행의 사유코드를 다시 선택하여 사유를 정확하게 입력해주십시오." + "\r\n" + "사유입력방법 -> 사유코드를 재선택");
                                return;
                            }
                        }
                    }
                }

                //내역 DB저장
                SendSayuDetail(RsInfo[nResultIndex], strSayuCd, strSayu);

                if (strLevel == "A")
                {
                    continue;
                }

                nResult = clsDur.DurResultSet.AddReport((int)VB.Val(RsInfo[nResultIndex].m_nIndex), strSayuCd, strSayu);
                if (nResult != 0)
                {
                    MessageBox.Show("사유 입력 실패 : " + nResult);
                    return;
                }
            }

            OrderSendFlag = true;
            this.Close();
        }

        /// <summary>
        /// 입력한 사유코드 및 내역 DB저장 (점검결과 내역 INSERT)
        /// </summary>
        private void SendSayuDetail(clsDur.ResultInfo resultInfo, string argSayuCd, string argSayu)
        {
            string SqlErr = "";
            int nRowAffected = 0;

            if (argSayuCd == "정보제공")
            {
                argSayuCd = "";
            }

            clsDB.setBeginTran(clsDB.DbCon);

            SQL.Clear();
            SQL.AppendLine(" MERGE INTO KOSMOS_OCS.DUR_SAYU_DETAIL ");
            SQL.AppendLine(" USING DUAL ");
            SQL.AppendLine($" ON (SABUN = '{clsType.User.Sabun}' AND BDATE = TO_DATE('{clsDur.DurPrescription.PrscPresDt}','YYYYMMDD') ");
            SQL.AppendLine($" AND DEPTCODE = '{clsOrdFunction.Pat.DeptCode}' AND PTNO = '{clsOrdFunction.Pat.PtNo}' ");
            SQL.AppendLine($" AND RESULTINDEX = {resultInfo.m_nIndex} AND SLIPNO = '{clsDur.DurPrescription.mprscGrantNo}' AND MEDCCDA = '{resultInfo.m_strMedcCDA}') ");
            SQL.AppendLine(" WHEN MATCHED THEN ");
            SQL.AppendLine($" UPDATE SET REASONCD = '{argSayuCd}', REASON = '{argSayu}' ");
            SQL.AppendLine(" WHEN NOT MATCHED THEN ");
            SQL.AppendLine(" INSERT ");
            SQL.AppendLine(" (SABUN, BDATE, DEPTCODE, PTNO, RESULTINDEX, ");
            SQL.AppendLine(" SLIPNO, PTNAME, INSURERTYPE, PREGWMNNYN, ");
            SQL.AppendLine(" MAINSICK, PRSCCICODE, MEDCCDA, MEDCNMA, GNLNMCDA, ");
            SQL.AppendLine(" GNLNMA, DDMQTYFREQA, DDEXECFREQA, MDCNEXECFREQA, \"TYPE\", ");
            SQL.AppendLine(" EXAMTYPECD, \"LEVEL\", MESSAGE, NOTICE, REASONCD, REASON, ");
            SQL.AppendLine(" DPPRSCMAKE, DPPRSCYYMMDD, DPPRSCHMMSS, DPPRSCADMINCODE, DPPRSCGRANTNO,");
            SQL.AppendLine(" DPPRSCADMINNAME, DPPRSCTEL, DPPRSCFAX, DPPRSCNAME, DPPRSCLIC, ");
            SQL.AppendLine(" DPMAKEYYMMDD, DPMAKEHMMSS, DPMAKEADMINCODE, DPMAKEADMINNAME, DPMAKETEL, ");
            SQL.AppendLine(" DPMAKENAME, DPMAKELIC, MEDCCDB, MEDCCNMB, GNLNMCDB, GNLNMB, ");
            SQL.AppendLine(" DDMQTYFREQB, DDEXEFREQB, MDCNEXECFREQB, INPUTIP, INPUTDATETIME, CANCELYN) ");
            SQL.AppendLine($" VALUES('{clsType.User.Sabun}', ");
            SQL.AppendLine($" TO_DATE('{clsDur.DurPrescription.PrscPresDt}','YYYYMMDD'), ");
            SQL.AppendLine($" '{clsOrdFunction.Pat.DeptCode}', ");
            SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}', ");
            SQL.AppendLine($" {resultInfo.m_nIndex}, ");
            SQL.AppendLine($" '{clsDur.DurPrescription.mprscGrantNo}', ");
            SQL.AppendLine($" '{clsDur.DurPrescription.PatNm}', ");
            SQL.AppendLine($" '{clsDur.DurPrescription.InsurerType}', ");
            SQL.AppendLine($" '{clsDur.DurPrescription.PregWmnYN}', ");
            SQL.AppendLine($" '{clsDur.DurPrescription.MainSick}', ");
            SQL.AppendLine($" '{clsDur.DurPrescription.PrscClCode}', ");
            SQL.AppendLine($" '{resultInfo.m_strMedcCDA}', ");
            SQL.AppendLine($" '{resultInfo.m_strMedcNMA}', ");
            SQL.AppendLine($" '{resultInfo.m_strGnlNMCDA}', ");
            SQL.AppendLine($" '{resultInfo.m_strGnlNMA}', ");
            SQL.AppendLine($" '{resultInfo.m_fDDMqtyFreqA}', ");
            SQL.AppendLine($" '{resultInfo.m_fDDExecFreqA}', ");
            SQL.AppendLine($" '{resultInfo.m_nMdcnExecFreqA}', ");
            SQL.AppendLine($" '{resultInfo.m_nType}', ");
            SQL.AppendLine($" '{resultInfo.m_strExamTypeCD}', ");
            SQL.AppendLine($" '{resultInfo.m_nLevel}', ");
            SQL.AppendLine($" '{resultInfo.m_strMessage?.Replace("'", "`")}', ");
            SQL.AppendLine($" '{resultInfo.m_strNotice?.Replace("'", "`")}', ");
            SQL.AppendLine($" '{argSayuCd}', ");
            SQL.AppendLine($" '{argSayu}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscMake}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscYYMMDD}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscHMMSS}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscAdminCode}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscGrantNo}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscAdminName}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscTel}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscFax}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscName}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpPrscLic}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpMakeYYMMDD}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpMakeHMMSS}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpMakeAdminCode}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpMakeAdminName}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpMakeTel}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpMakeName}', ");
            SQL.AppendLine($" '{resultInfo.m_strDpMakeLic}', ");
            SQL.AppendLine($" '{resultInfo.m_strMedcCDB}', ");
            SQL.AppendLine($" '{resultInfo.m_strMedcNMB}', ");
            SQL.AppendLine($" '{resultInfo.m_strGnlNMCDB}', ");
            SQL.AppendLine($" '{resultInfo.m_strGnlNMB}', ");
            SQL.AppendLine($" '{resultInfo.m_fDDMqtyFreqB}', ");
            SQL.AppendLine($" '{resultInfo.m_fDDExecFreqB}', ");
            SQL.AppendLine($" '{resultInfo.m_nMdcnExecFreqB}', ");
            SQL.AppendLine($" '{clsCompuInfo.gstrCOMIP}', ");
            SQL.AppendLine($" sysdate,  ");
            SQL.AppendLine($" 'N')  ");

            SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);

            clsDB.setCommitTran(clsDB.DbCon);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            OrderSendFlag = false;
            this.Close();
        }

        private void frmMedScreenOrderResult_Load(object sender, EventArgs e)
        {
            SQL = new StringBuilder();

            CMF = new clsMedFunction();

            lblPtno.Text = clsOrdFunction.Pat.PtNo;
            lblSname.Text = clsOrdFunction.Pat.sName;
            lblGenderAge.Text = $"{clsOrdFunction.Pat.Sex} / {clsOrdFunction.Pat.Age}";
            lblPregnant.Text = $"임신:{(clsOrdFunction.Pat.Pregnant == "Y" ? "Y" : "N")}";
            //lblLact.Text = $"수유:{clsOrdFunction.Pat.Lact}";

            ssDurResult.ActiveSheet.Rows.Count = 0;
            ssDIFResult.ActiveSheet.Rows.Count = 0;

            //인터페이스 테이블에 정보 주고 처방검토 결과를 받는 것까지.
            DIFScreenResult();

            /*받은 결과를 뿌려준다.
            --심평원 */
            ssDurResult.ActiveSheet.Rows.Count = 0;

            Bitmap SearchImg = new Bitmap(Resources.find);
            FarPoint.Win.Picture picture = new FarPoint.Win.Picture(SearchImg);

            FarPoint.Win.Spread.CellType.TextCellType textCellType = new FarPoint.Win.Spread.CellType.TextCellType();
            textCellType.BackgroundImage = picture;
            textCellType.BackgroundImage.AlignHorz = FarPoint.Win.HorizontalAlignment.Center;
            textCellType.BackgroundImage.AlignVert = FarPoint.Win.VerticalAlignment.Center;
            ssDurResult.ActiveSheet.Columns[7].Visible = false;
            //textCellType.BackgroundImage.Style = FarPoint.Win.RenderStyle.StretchAndScale;

            int nRow = 0;

            //심평원 pdf 64P, Y코드는 최근에 추가
            //A~X, F~X 코드 나오는 콤보박스타입 셋팅
            string[] strAX = new string[22];
            strAX[0] = "A:환자가 미리 내원한 경우";
            strAX[1] = "B:특정성분만 별도 처방할 수 없는 경우(powder 등)";
            strAX[2] = "C:약제가 소실·변질 된 경우(항암제 투여,소아환자의 구토에 한함)";
            strAX[3] = "F:복용기간이 중복되지 않는 경우";
            strAX[4] = "G:주 단위 또는 월 단위로 복용하는 약제";
            strAX[5] = "H:용법·용량을 변경했음에도 팝업이 다시 발생한 경우";
            strAX[6] = "I:투여일수를 변경했음에도 팝업이 다시 발생한 경우";
            strAX[7] = "L:기존 처방약을 복용하지 않고 있거나, 복용을 중단시킨 경우";
            strAX[8] = "K:처방의사 또는 조제약사와 전화통화 안 되는 경우";
            strAX[9] = "P:필요시 투약하는 약제 (PRN)우";
            strAX[10] = "R:입원환자의 기존 외래 처방·조제된 약과 처방전간 점검이 발생하는 경우";
            strAX[11] = "W:용량조절이 필요한 경우";
            strAX[12] = "X:기존 처방전을 조제하지 않은 경우";
            strAX[13] = "D:신경차단술 등 국소치료에 조영제 투여";
            strAX[14] = "Y:조영제 투여 48시간 전 Metformin 투약 중지 조치한 경우";
            strAX[15] = "M:절박 유·조산 또는 습관성 유·조산 치료 시 투여";
            strAX[16] = "N:분만 시(분만유도, 분만촉진, 출혈방지 등)에 투여";
            strAX[17] = "O:유산유도(불가피한 사유로 인한 합법적 인공임신중절 시)";
            strAX[18] = "Q:임신중 검사(융모막채취검사(CVS), 양수검사, 수축자극검사(CST) 등";
            strAX[19] = "S:보조생식술에 투여하는 경우";
            strAX[20] = "V:임부에 해당하지 않는 경우(출산 후 투여, 폐경 등)";
            strAX[21] = "T:Text로 기재";

            string[] strFX = new string[19];
            strFX[0] = "F:복용기간이 중복되지 않는 경우";
            strFX[1] = "G:주 단위 또는 월 단위로 복용하는 약제";
            strFX[2] = "H:용법·용량을 변경했음에도 팝업이 다시 발생한 경우";
            strFX[3] = "I:투여일수를 변경했음에도 팝업이 다시 발생한 경우";
            strFX[4] = "L:기존 처방약을 복용하지 않고 있거나, 복용을 중단시킨 경우";
            strFX[5] = "K:처방의사 또는 조제약사와 전화통화 안 되는 경우";
            strFX[6] = "P:필요시 투약하는 약제 (PRN)우";
            strFX[7] = "R:입원환자의 기존 외래 처방·조제된 약과 처방전간 점검이 발생하는 경우";
            strFX[8] = "W:용량조절이 필요한 경우";
            strFX[9] = "X:기존 처방전을 조제하지 않은 경우";
            strFX[10] = "D:신경차단술 등 국소치료에 조영제 투여";
            strFX[11] = "Y:조영제 투여 48시간 전 Metformin 투약 중지 조치한 경우";
            strFX[12] = "M:절박 유·조산 또는 습관성 유·조산 치료 시 투여";
            strFX[13] = "N:분만 시(분만유도, 분만촉진, 출혈방지 등)에 투여";
            strFX[14] = "O:유산유도(불가피한 사유로 인한 합법적 인공임신중절 시)";
            strFX[15] = "Q:임신중 검사(융모막채취검사(CVS), 양수검사, 수축자극검사(CST) 등";
            strFX[16] = "S:보조생식술에 투여하는 경우";
            strFX[17] = "V:임부에 해당하지 않는 경우(출산 후 투여, 폐경 등)";
            strFX[18] = "T:Text로 기재";

            FarPoint.Win.Spread.CellType.ComboBoxCellType cboFX = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            FarPoint.Win.Spread.CellType.ComboBoxCellType cboAX = new FarPoint.Win.Spread.CellType.ComboBoxCellType();
            cboFX.Items = strFX;
            cboAX.Items = strAX;

            cboFX.ListWidth = 450;
            cboAX.ListWidth = 450;

            cboFX.MaxDrop = 22;
            cboAX.MaxDrop = 22;

            for (int i = 0; i < RsInfo.Length; i++)
            {
                //이미 보낸 사유 및 정보제공 확인 자동으로 전송
                if (AutoSendSayu(RsInfo[i]) == true)
                {
                    continue;
                }

                ssDurResult.ActiveSheet.Rows.Count++;
                ssDurResult.ActiveSheet.Cells[nRow, 0].Text = DurType[RsInfo[i].m_nType];      //코드 - 내용 맵핑
                ssDurResult.ActiveSheet.Cells[nRow, 1].Text = RsInfo[i].m_nLevel;
                ssDurResult.ActiveSheet.Cells[nRow, 2].Text = RsInfo[i].m_strMedcNMB + "\r\n" + RsInfo[i].m_strMedcNMA; //복용약품, 입력약품
                ssDurResult.ActiveSheet.Cells[nRow, 3].Text = RsInfo[i].m_strMessage;
                ssDurResult.ActiveSheet.Cells[nRow, 6].CellType = textCellType;
                ssDurResult.ActiveSheet.Cells[nRow, 6].Locked = true;
                ssDurResult.ActiveSheet.Cells[nRow, 7].Text = i.ToString();

                ssDurResult.ActiveSheet.Rows[nRow].Height = ssDurResult.ActiveSheet.Rows[nRow].GetPreferredHeight() + 8;

                string ChkFlag = ChkTextSayu(RsInfo[i].m_nType, RsInfo[i].m_nLevel);

                if (ChkFlag == "사유기재")
                {
                    ssDurResult.ActiveSheet.Cells[nRow, 4].Text = "T";
                    ssDurResult.ActiveSheet.Cells[nRow, 4].Locked = true;  //코드 바꿀수 없다
                }
                else if (ChkFlag == "정보제공")
                {
                    //사유나 코드기재 불필요
                    ssDurResult.ActiveSheet.Cells[nRow, 4].Locked = true;
                    ssDurResult.ActiveSheet.Cells[nRow, 5].Locked = true;

                    ssDurResult.ActiveSheet.Cells[nRow, 4].Text = "정보제공";
                    ssDurResult.ActiveSheet.Cells[nRow, 5].Text = "정보제공";
                }
                else if (ChkFlag == "절대금기")
                {
                    btnSave.Enabled = false;
                    MessageBox.Show("안정성금기 및 회수의약품 절대금기 내역이 있습니다. 처방을 수정해주십시오.");
                    btnExit.Focus();
                }
                else if (ChkFlag == "A~X")
                {
                    //해당 사유코드에 맞는 콤보박스 셋팅 (셀에)
                    ssDurResult.ActiveSheet.Cells[nRow, 4].CellType = cboAX;
                    ssDurResult.ActiveSheet.Cells[nRow, 4].Locked = false;
                }
                else if (ChkFlag == "F~X")
                {
                    //해당 사유코드에 맞는 콤보박스 셋팅 (셀에)
                    ssDurResult.ActiveSheet.Cells[nRow, 4].CellType = cboFX;
                    ssDurResult.ActiveSheet.Cells[nRow, 4].Locked = false;
                }
                else if (ChkFlag == "D등급")
                {
                    //해당 사유코드에 맞는 콤보박스 셋팅 (셀에)
                    ssDurResult.ActiveSheet.Cells[nRow, 4].CellType = cboAX;
                    ssDurResult.ActiveSheet.Cells[nRow, 4].Locked = false;
                }

                nRow++;
            }

            ssDurResult_Sheet1.SortRows(1, false, false);



            for (int i = 0; i < ssDurResult.ActiveSheet.Columns.Count; i++)
            {
                ssDurResult.ActiveSheet.Columns[i].Width = ssDurResult.ActiveSheet.Columns[i].GetPreferredWidth() + 8;
            }

            ssDurResult.ActiveSheet.Columns[2].Width = 300;
            ssDurResult.ActiveSheet.Columns[4].Width = 100;
            ssDurResult.ActiveSheet.Columns[5].Width = 200;

            /* FIRSTDIS ds[0] V_CUR_ScnResult(결과), ds[1] V_CUR_Patient(환자정보), ds[2] V_CUR_Order(점검오더) */

            ssDIFResult.ActiveSheet.Columns[5, 6].Visible = false;

            if (ds != null)
            {
                //처방검토 결과
                dtResult = ds.Tables[0];
                dtPatient = ds.Tables[1];
                dtOrder = ds.Tables[2];

                ds.Dispose();
                ds = null;
            }

            ssDIFResult.ActiveSheet.Rows.Count = 0;
            ssDIFResult.ActiveSheet.Rows.Count = pResultCnt; /*+ pResultCntDam;*/

            //심평원 DUR 결과 없으면 종료
            if (ssDurResult.ActiveSheet.NonEmptyRowCount == 0 && pResultCnt == 0)
            {
                //앞에서 이미 보낸 사유 및 정보제공 확인은 다시 뜨지 않도록 한다.
                OrderSendFlag = true;
                this.Close();
            }

            //처방검토 결과
            if (pResultCnt > 0)
            {
                for (int i = 0; i < dtResult.Rows.Count; i++)
                {
                    ssDIFResult.ActiveSheet.Cells[i, 0].Text = dtResult.Rows[i]["MODULENM"].ToString().Trim();
                    ssDIFResult.ActiveSheet.Cells[i, 1].Text = dtResult.Rows[i]["LVDESC"].ToString().Trim();
                    ssDIFResult.ActiveSheet.Cells[i, 2].Text = dtResult.Rows[i]["ORDDRUGNM"].ToString().Trim();
                    ssDIFResult.ActiveSheet.Cells[i, 3].Text = dtResult.Rows[i]["SCNMESSAGE"].ToString().Trim();

                    //약물상호작용만 상세정보 있음
                    if (dtResult.Rows[i]["MODULEID"].ToString() == "DDIM")
                    {
                        ssDIFResult.ActiveSheet.Cells[i, 4].CellType = textCellType;
                        ssDIFResult.ActiveSheet.Cells[i, 5].Text = dtResult.Rows[i]["MODULEID"].ToString();
                        ssDIFResult.ActiveSheet.Cells[i, 6].Text = dtResult.Rows[i]["MONOGRAPHID"].ToString();
                    }
                    ssDIFResult.ActiveSheet.Cells[i, 4].Locked = true;

                    ssDIFResult.ActiveSheet.Rows[i].Height = ssDIFResult.ActiveSheet.Rows[i].GetPreferredHeight() + 8;
                }
            }
            //알러지 처방검토 결과 (팝업창을 따로 띄우는걸로 이동한다.)
            //if (pResultCntDam > 0)
            //{
            //    for (int i = 0; i < dtResultDam.Rows.Count; i++)
            //    {
            //        ssDIFResult.ActiveSheet.Cells[pResultCnt + i, 0].Text = dtResultDam.Rows[i]["MODULENM"].ToString().Trim();
            //        ssDIFResult.ActiveSheet.Cells[pResultCnt + i, 1].Text = dtResultDam.Rows[i]["LVDESC"].ToString().Trim();
            //        ssDIFResult.ActiveSheet.Cells[pResultCnt + i, 2].Text = dtResultDam.Rows[i]["ORDDRUGNM"].ToString().Trim();
            //        ssDIFResult.ActiveSheet.Cells[pResultCnt + i, 3].Text = dtResultDam.Rows[i]["SCNMESSAGE"].ToString().Trim();

            //        ssDIFResult.ActiveSheet.Cells[pResultCnt + i, 4].Locked = true;

            //        ssDIFResult.ActiveSheet.Rows[pResultCnt + i].Height = ssDIFResult.ActiveSheet.Rows[pResultCnt + i].GetPreferredHeight() + 8;
            //    }
            //}

            //for (int i = 0; i < ssDIFResult.ActiveSheet.Columns.Count; i++)
            //{
            //    ssDIFResult.ActiveSheet.Columns[i].Width = ssDIFResult.ActiveSheet.Columns[i].GetPreferredWidth() + 8;
            //}

            //심평원 DUR, DIF 결과 둘다 없으면 종료
            //if (RsInfo.Length == 0 && pResultCnt == 0)
            //{
            //    //점검결과 없으면 종료
            //    OrderSendFlag = true;
            //    this.Close();
            //}       
        }


        /// <summary>
        /// 이미 보낸 사유 점검하고 보냄 True - 재전송 False - 신규
        /// </summary>
        /// <param name="resultInfo">점검내역</param>
        /// <returns></returns>
        private bool AutoSendSayu(clsDur.ResultInfo resultInfo)
        {
            bool rtnVal = false;

            string SqlErr = "";
            DataTable dt = null;
            int nResult;

            SQL.Clear();
            SQL.AppendLine("  SELECT REASONCD, REASON                                             ");
            SQL.AppendLine("  FROM KOSMOS_OCS.DUR_SAYU_DETAIL                                     ");
            SQL.AppendLine($"  WHERE SABUN = '{clsType.User.Sabun}'                               ");
            //SQL.AppendLine($"  AND BDATE = TO_DATE('{resultInfo.m_strDpPrscYYMMDD}','YYYYMMDD')   ");
            SQL.AppendLine($"  AND DEPTCODE = '{clsOrdFunction.Pat.DeptCode}'                     ");
            SQL.AppendLine($"  AND PTNO = '{clsOrdFunction.Pat.PtNo}'                             ");
            SQL.AppendLine($"  AND PRSCCICODE = '{strPrscCiCode}'                                 ");
            SQL.AppendLine($"  AND MEDCCDA = '{resultInfo.m_strMedcCDA}'                          ");
            SQL.AppendLine($"  AND \"LEVEL\" = '{resultInfo.m_nLevel}'                            ");
            SQL.AppendLine($"  AND \"TYPE\" = '{resultInfo.m_nType}'                              ");
            SQL.AppendLine($"  AND REASON IS NOT NULL                                             ");
            SQL.AppendLine("   ORDER BY BDATE DESC                                                ");
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

            if (dt.Rows.Count > 0)
            {
                string strReason;
                string strReasonCD;

                strReason = dt.Rows[0]["REASON"].ToString();
                strReasonCD = dt.Rows[0]["REASONCD"].ToString();

                //2020-09-17 추가 
                if (strReason.Length < 2)
                {
                    return rtnVal;
                }

                if (strReasonCD == "T" && (strReason.Length < 2 || strReason == "tt" || strReason == "TT"
                                                               || strReason == "ttt" || strReason == "TTT"))
                {
                    return rtnVal;
                }

                //내역 갱신
                SendSayuDetail(resultInfo, strReasonCD, strReason);

                if (!string.IsNullOrEmpty(strReasonCD))
                {
                    nResult = clsDur.DurResultSet.AddReport((int)VB.Val(resultInfo.m_nIndex), strReasonCD, strReason);
                    if (nResult != 0)
                    {
                        MessageBox.Show("사유 자동 입력 실패 : " + nResult);
                        rtnVal = false;
                    }
                }

                rtnVal = true;
            }


            return rtnVal;
        }

        /// <summary>
        /// 점검코드, 점검등급에 따라 Text사유 필수 및 코드사유 가능 여부 판단 - 리턴:사유기재,정보제공,절대금기,F~X,A~X,D등급
        /// </summary>
        /// <param name="m_nType">점검코드</param>
        /// <param name="m_nLevel">점검등급</param>
        /// <returns></returns>
        private string ChkTextSayu(string m_nType, string m_nLevel)
        {
            //DUR 개발pdf 65p
            string rtnVal;

            //사유기재 필수인항목
            if ((m_nType == "00" || m_nType == "21" || m_nType == "22" || m_nType == "23" || m_nType == "01" || m_nType == "06")
                && m_nLevel == "B")
            {
                rtnVal = "사유기재";
            }
            //F~X, Text
            else if ((m_nType == "07" || m_nType == "41" || m_nType == "42" || m_nType == "43") && m_nLevel == "B")
            {
                rtnVal = "F~X";
            }
            //A~X, Text
            else if ((m_nType == "08" || m_nType == "40") && m_nLevel == "B")
            {
                rtnVal = "A~X";
            }
            //절대금기
            else if (m_nType == "02" && m_nLevel == "C")
            {
                rtnVal = "절대금기";
            }
            //D등급
            else if ((m_nType == "05" || m_nType == "06") && m_nLevel == "D")
            {
                rtnVal = "D등급";
            }
            //정보제공
            else
            {
                rtnVal = "정보제공";
            }

            return rtnVal;
        }

        /// <summary>
        /// 인터페이스 테이블에 정보 주고 처방검토 결과를 받는 것까지.
        /// </summary>
        private void DIFScreenResult()
        {
            try
            {
                clsDB.setBeginTran(clsDB.DbCon);
                //환자정보 TO DIF
                ToDIFPatInfo();
                //질병정보 TO DIF
                ToDIFPatDDCM();
                //신기능정보 TO DIF
                ToDIFPatSCR();
                //알레르기정보 TO DIF
                ToDIFPatDAM();
                //처방정보 TO DIF
                ToDIFPatOrder();
                clsDB.setCommitTran(clsDB.DbCon);

                //처방검토 실행 후 데이터 셋에 
                OracleCommand cmd = new OracleCommand();
                ComDbB.PsmhDb pDbCon = clsDB.DbCon;

                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_DRUG.up_ScreenEXEC";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pPatientID", OracleDbType.Varchar2, 10, clsOrdFunction.Pat.PtNo.Trim(), ParameterDirection.Input);
                cmd.Parameters.Add("pGuID", OracleDbType.Varchar2, 20, clsDur.DurPrescription.mprscGrantNo, ParameterDirection.Input);
                cmd.Parameters.Add("pUserID", OracleDbType.Varchar2, 6, clsType.User.Sabun.Trim(), ParameterDirection.Input);
                cmd.Parameters.Add("pResultCnt", OracleDbType.Int32, ParameterDirection.Output);
                cmd.Parameters.Add("V_CUR_ScnResult", OracleDbType.RefCursor, ParameterDirection.Output);
                cmd.Parameters.Add("V_CUR_Patient", OracleDbType.RefCursor, ParameterDirection.Output);
                cmd.Parameters.Add("V_CUR_Order", OracleDbType.RefCursor, ParameterDirection.Output);

                OracleDataAdapter ODA = new OracleDataAdapter();
                ODA.SelectCommand = cmd;
                //cmd.ExecuteNonQuery();

                ds = new DataSet();
                ODA.Fill(ds);
                pResultCnt = Convert.ToInt32(cmd.Parameters["pResultCnt"].Value.ToString());

                cmd.Dispose();
                cmd = null;

                //알레르기 정보 검토 프로시저 별개로 사용 (팝업창 따로 띄우는걸로 이동한다)
                cmd = new OracleCommand();
                pDbCon = clsDB.DbCon;

                cmd.Connection = pDbCon.Con;
                cmd.InitialLONGFetchSize = 1000;
                cmd.CommandText = "KOSMOS_DRUG.up_ScreenDAM_PHSM";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("pPatientID", OracleDbType.Varchar2, 10, clsOrdFunction.Pat.PtNo.Trim(), ParameterDirection.Input);
                cmd.Parameters.Add("pGuID", OracleDbType.Varchar2, 20, clsDur.DurPrescription.mprscGrantNo, ParameterDirection.Input);
                cmd.Parameters.Add("V_CUR", OracleDbType.RefCursor, ParameterDirection.Output);

                ODA = new OracleDataAdapter();
                ODA.SelectCommand = cmd;
                //cmd.ExecuteNonQuery();

                //알레르기 정보는 4번째 datatable에 2,3번째 커서값은 필요시에 사용
                DataSet dsdam = new DataSet();
                dtResultDam = new DataTable();
                ODA.Fill(dsdam);

                if (dsdam.Tables.Count > 0)
                {
                    dtResultDam = dsdam.Tables[0];
                    cmd.Dispose();
                    cmd = null;

                    //결과수 추가
                    pResultCntDam = dtResultDam.Rows.Count;

                    //알레르기 건 1개라도 있으면 보여주는 팝업창
                    if (pResultCntDam > 0)
                    {
                        using (frmMedScreenAllegyResult frm = new frmMedScreenAllegyResult(dtResultDam))
                        {
                            frm.ShowDialog();
                        }

                        //알러지 창에서 처방취소시 오더전송 중지
                        if (clsDur.gstrDur_알러지취소 == "취소")
                        {
                            OrderSendFlag = false;
                            this.Close();
                        }
                    }
                }

                //처방검토 테이블 데이터 삭제 프로시저 돌리기위함
                DeleteDIFTable();
            }
            catch (Exception ex)
            {
                clsDB.setRollbackTran(clsDB.DbCon);
                return;
            }
        }

        /// <summary>
        /// 처방검토 테이블 데이터 환자정보 삭제
        /// </summary>
        private void DeleteDIFTable()
        {
            string SqlErr = "";
            clsDB.setBeginTran(clsDB.DbCon);

            SQL.Clear();
            SQL.AppendLine(" DELETE KOSMOS_DRUG.SCNINFO_PATIENT    ");
            SQL.AppendLine($" WHERE PATID = '{clsOrdFunction.Pat.PtNo}' ");
            SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);

            SQL.Clear();
            SQL.AppendLine(" DELETE KOSMOS_DRUG.SCNINFO_ORDER    ");
            SQL.AppendLine($" WHERE PATID = '{clsOrdFunction.Pat.PtNo}' ");
            SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);

            SQL.Clear();
            SQL.AppendLine(" DELETE KOSMOS_DRUG.SCNINFO_DDCM    ");
            SQL.AppendLine($" WHERE PATID = '{clsOrdFunction.Pat.PtNo}' ");
            SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);

            SQL.Clear();
            SQL.AppendLine(" DELETE KOSMOS_DRUG.SCNINFO_PATIENTSCR    ");
            SQL.AppendLine($" WHERE PATID = '{clsOrdFunction.Pat.PtNo}' ");
            SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);

            SQL.Clear();
            SQL.AppendLine(" DELETE KOSMOS_DRUG.SCNINFO_DAM    ");
            SQL.AppendLine($" WHERE PATID = '{clsOrdFunction.Pat.PtNo}' ");
            SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);
            clsDB.setCommitTran(clsDB.DbCon);
        }

        /// <summary>
        /// 처방정보 TO DIF
        /// </summary>
        private void ToDIFPatOrder()
        {
            string SqlErr = "";
            int nRowAffected = 0;

            for (int i = 0; i < DurMed.Length; i++)
            {
                if (DurMed[i].strOrderCd == "")
                {
                    continue;
                }

                DateTime Bdate = Convert.ToDateTime(clsOrdFunction.GstrBDate);
                string strEndYMD = Bdate.AddDays(DurMed[i].intmdcnexecfreq - 1).ToShortDateString();

                SQL.Clear();
                SQL.AppendLine("  INSERT INTO KOSMOS_DRUG.SCNINFO_ORDER(                    ");
                SQL.AppendLine("      GROUPID, ORDSEQ, PATID, INSTCD,                       ");
                SQL.AppendLine("      ORDCD, DRGNM, INGRDNM, DOSAGEQTY,                     ");
                SQL.AppendLine("      DOSAGEFRQ, DOSAGECNT, DOSAGEQTYUNIT, DOSAGEMETHOD,    ");
                SQL.AppendLine("      EXECSTRYMD, EXECENDYMD, ORDGB, ORDTYPE,               ");
                SQL.AppendLine("      HOSINYN, DEPTCD, USERID, ORDUSERID,                   ");
                SQL.AppendLine("       KD_CD, CURRENTORDFLAG, SELFDRUGYN ) VALUES (         ");
                SQL.AppendLine($" '{clsDur.DurPrescription.mprscGrantNo}',                  ");
                SQL.AppendLine($" '{i}',                                                    ");
                SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}',                              ");
                SQL.AppendLine($" 'PH',                                                     ");
                SQL.AppendLine($" '{DurMed[i].strOrderCd}',                                 ");
                SQL.AppendLine($" '{DurMed[i].strDrgNm}',                                   ");
                SQL.AppendLine($" '{DurMed[i].strIngrDnm}',                                 ");
                SQL.AppendLine($" {DurMed[i].dblddMqtyFreq},                                ");
                SQL.AppendLine($" {DurMed[i].dblddExecFreq},                                ");
                SQL.AppendLine($" {DurMed[i].intmdcnexecfreq},                              ");
                SQL.AppendLine($" '{DurMed[i].strDrgUnit}',                                 ");
                SQL.AppendLine($" '{DurMed[i].strDrgMethod}',                               ");
                SQL.AppendLine($" '{Bdate.ToShortDateString().Replace("-", "")}',           ");
                SQL.AppendLine($" '{strEndYMD.Replace("-", "")}',                           ");
                SQL.AppendLine($" '{clsOrdFunction.Pat.GbIO}',                              ");
                SQL.AppendLine($" 'I',                                                       ");
                SQL.AppendLine($" 'Y',                                                      ");
                SQL.AppendLine($" '{clsOrdFunction.Pat.DeptCode}',                          ");
                SQL.AppendLine($" '{clsType.User.Sabun}',                                   ");
                SQL.AppendLine($" '{clsType.User.Sabun}',                                   ");
                SQL.AppendLine($" '{DurMed[i].strmedcCD}',                                  ");
                SQL.AppendLine($" 'Y',                                                      ");
                SQL.AppendLine($" '' )                                                      ");

                SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);
            }
        }

        /// <summary>
        /// 알레르기정보 TO DIF
        /// </summary>
        private void ToDIFPatDAM()
        {
            //알러지 정보 연동, 기존 알러지 정보 사용 불가 FirstDIS 제공 솔루션만 사용 가능
            string SqlErr = "";
            DataTable dt = null;
            //DataTable dt1 = null;
            DataTable dt2 = null;

            SQL.Clear();
            SQL.AppendLine("  SELECT DAMCD, DAMTYPE, DAMDESC, RMK           ");
            SQL.AppendLine("   FROM KOSMOS_PMPA.ETC_ALLERGY_MST             ");
            SQL.AppendLine($"  WHERE PANO = '{clsOrdFunction.Pat.PtNo}'     ");
            SQL.AppendLine($"    AND CODE = '100'                           ");
            SQL.AppendLine("   ORDER BY ENTDATE DESC                        ");
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                SQL.Clear();
                SQL.AppendLine(" INSERT INTO KOSMOS_DRUG.SCNINFO_DAM(               ");
                SQL.AppendLine("      PATID, DAMCD, DAMTYPE, DAMNM, RMK) VALUES (   ");
                SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}',                      ");
                SQL.AppendLine($" '{VB.Trim(dt.Rows[i]["DAMCD"].ToString())}',      ");
                SQL.AppendLine($" '{VB.Trim(dt.Rows[i]["DAMTYPE"].ToString())}',    ");
                SQL.AppendLine($" '{VB.Trim(dt.Rows[i]["DAMDESC"].ToString())}',    ");
                SQL.AppendLine($" '{VB.Trim(dt.Rows[i]["RMK"].ToString())}' )       ");

                SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);
            }

            if (dt.Rows.Count != 0)
            {
                dt.Dispose();
                dt = null;
            }

            //ADR 코드 SUCODE 추가로 넣어준다
            SQL.Clear();
            SQL.AppendLine("  SELECT SEQNO                                  ");
            SQL.AppendLine("   FROM KOSMOS_ADM.DRUG_ADR1                    ");
            if (clsOrdFunction.Pat.PtNo == "81000004")
            {
                SQL.AppendLine("  WHERE PTNO = '06043533'               ");
            }
            else
            {
                SQL.AppendLine($"  WHERE PTNO = '{clsOrdFunction.Pat.PtNo}'     ");
            }
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //2019-09-07 ADR보고서 작성된 병원코드-알러지 무조건 띄우도록(조건삭제)
                //SQL.Clear();
                //SQL.AppendLine("  SELECT SEQNO                                                     ");
                //SQL.AppendLine("   FROM KOSMOS_ADM.DRUG_ADR2                                       ");
                ////SQL.AppendLine(" WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')     ");   
                //SQL.AppendLine($" WHERE SEQNO = {VB.Val(dt.Rows[i]["SEQNO"].ToString())}             ");
                //SQL.AppendLine("  UNION ALL                                                        ");
                //SQL.AppendLine("  SELECT SEQNO                                                     ");
                //SQL.AppendLine("   FROM KOSMOS_ADM.DRUG_ADR3                                       ");
                ////SQL.AppendLine(" WHERE (RELATION1 = '1' OR RELATION2 = '1' OR RELATION3 = '1')     ");
                //SQL.AppendLine($" WHERE SEQNO = {VB.Val(dt.Rows[i]["SEQNO"].ToString())}             ");
                //SqlErr = clsDB.GetDataTableREx(ref dt1, SQL.ToString(), clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    SQL.Clear();
                    SQL.AppendLine("  SELECT SUCODE,SUNAMEK                                           ");
                    SQL.AppendLine("   FROM KOSMOS_ADM.DRUG_ADR1_ORDER                                ");
                    SQL.AppendLine($" WHERE SEQNO = {VB.Val(dt.Rows[0]["SEQNO"].ToString())}          ");
                    SqlErr = clsDB.GetDataTableREx(ref dt2, SQL.ToString(), clsDB.DbCon);

                    for (int j = 0; j < dt2.Rows.Count; j++)
                    {
                        SQL.Clear();
                        SQL.AppendLine(" INSERT INTO KOSMOS_DRUG.SCNINFO_DAM(                   ");
                        SQL.AppendLine("      PATID, DAMCD, DAMTYPE, DAMNM) VALUES (            ");
                        SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}',                          ");
                        SQL.AppendLine($" '{VB.Trim(dt2.Rows[j]["SUCODE"].ToString())}',        ");
                        SQL.AppendLine($" '5',                                                  ");   //DAMTYPE 4 KD, 5 병원코드
                        SQL.AppendLine($" '{VB.Trim(dt2.Rows[j]["SUNAMEK"].ToString())}'       )");

                        SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);
                    }
                }
            }
        }

        /// <summary>
        /// 신기능정보 TO DIF
        /// </summary>
        private void ToDIFPatSCR()
        {
            string SqlErr = "";
            string strIndate = "";
            if (!string.IsNullOrEmpty(clsOrdFunction.Pat.InDate))
            {
                strIndate = Convert.ToDateTime(clsOrdFunction.Pat.InDate).AddDays(-1).ToShortDateString();
            }
            else
            {
                strIndate = clsPublic.GstrBdate;
            }

            DataTable dt = null;

            SQL.Clear();
            SQL.AppendLine("  SELECT B.SUBCODE, B.RESULT, TO_CHAR(B.RESULTDATE,'YYYY-MM-DD') RESULTDATE ");
            SQL.AppendLine("   FROM KOSMOS_OCS.EXAM_SPECMST A, KOSMOS_OCS.EXAM_RESULTC B                ");
            SQL.AppendLine($"  WHERE A.PANO = '{clsOrdFunction.Pat.PtNo}'                               ");
            SQL.AppendLine("     AND A.SPECNO = B.SPECNO                                                ");
            SQL.AppendLine("     AND B.SUBCODE = 'CR42A'                                                ");
            SQL.AppendLine("     AND A.RESULTDATE < TRUNC(SYSDATE+1)                                    ");
            SQL.AppendLine($"    AND A.RESULTDATE > TO_DATE('{strIndate}','YYYY-MM-DD')                 ");
            SQL.AppendLine("   ORDER BY B.RESULTDATE DESC, B.SUBCODE                                    ");
            SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

            //최근 검사중 1개만
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            if (dt.Rows.Count >= 1)
            {
                SQL.Clear();
                SQL.AppendLine(" INSERT INTO KOSMOS_DRUG.SCNINFO_PATIENTSCR(    ");
                SQL.AppendLine("      PATID, SEQ, SCRVALUE, SCRDATE) VALUES (   ");
                SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}',                  ");
                SQL.AppendLine($" '{0}',                                        ");
                SQL.AppendLine($" '{dt.Rows[0]["RESULT"].ToString().Replace("<", "").Replace("-", "").Trim()}',          ");
                SQL.AppendLine($" '{dt.Rows[0]["RESULTDATE"].ToString()}' )     ");

                SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);
            }
            //}            
        }

        /// <summary>
        /// 질병정보 TO DIF
        /// </summary>
        private void ToDIFPatDDCM()
        {
            string SqlErr = "";
            DataTable dt = null;

            for (int index = 0; index < clsOrdFunction.Pat_IllCode.Length; index++)
            {
                SQL.Clear();
                SQL.AppendLine("  SELECT ILLCODED, SUBSTR(ILLNAMEE,1,400) AS ILLNAMEE    ");
                SQL.AppendLine("    FROM KOSMOS_PMPA.BAS_ILLS                            ");
                SQL.AppendLine($"  WHERE ILLCODE = '{clsOrdFunction.Pat_IllCode[index]}' ");
                SqlErr = clsDB.GetDataTableREx(ref dt, SQL.ToString(), clsDB.DbCon);

                if (dt.Rows.Count > 0)
                {
                    SQL.Clear();
                    SQL.AppendLine(" INSERT INTO KOSMOS_DRUG.SCNINFO_DDCM(               ");
                    SQL.AppendLine("      PATID, DISEASECD, DISEASENM ) VALUES (         ");
                    SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}',                       ");
                    SQL.AppendLine($" '{dt.Rows[0]["ILLCODED"].ToString().Trim()}',      ");
                    SQL.AppendLine($" '{dt.Rows[0]["ILLNAMEE"].ToString().Trim()}')      ");

                    SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);
                }
            }

        }

        /// <summary>
        /// 환자정보 TO DIF
        /// </summary>
        private void ToDIFPatInfo()
        {
            string SqlErr = "";
            string strBirth = clsOrdFunction.Pat.Birth;
            string strWeight = clsOrdFunction.Pat.WEIGHT;

            if (VB.IsDate(strBirth) == true)
            {
                strBirth = Convert.ToDateTime(strBirth).ToString("yyyyMMdd");
            }
            else
            {
                strBirth = strBirth.Replace("-", "");
            }

            if (strWeight == null)
            {
                strWeight = "";
            }
            else
            {
                strWeight = strWeight.Replace("Kg", "");
            }

            SQL.Clear();
            SQL.AppendLine(" INSERT INTO KOSMOS_DRUG.SCNINFO_PATIENT(        ");
            SQL.AppendLine("      PATID, GENDER, BIRTHDAY, WEIGHT,           ");
            SQL.AppendLine("      PREGFLAG, LACTFLAG) VALUES (               ");
            SQL.AppendLine($" '{clsOrdFunction.Pat.PtNo}',                   ");
            SQL.AppendLine($" '{clsOrdFunction.Pat.Sex}',                    ");
            SQL.AppendLine($" '{strBirth}',  ");
            SQL.AppendLine($" '{strWeight}',");
            SQL.AppendLine($" '{clsOrdFunction.Pat.Pregnant}',               ");
            SQL.AppendLine($"  NULL)                                         "); //수유여부는 확인후

            SqlErr = clsDB.ExecuteNonQueryEx(SQL.ToString(), ref nRowAffected, clsDB.DbCon);
        }

        //private void ssSayu_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        //{
        //    int nRowDur = ssDurResult.ActiveSheet.ActiveCell.Row.Index;
        //    int nRowSayu = e.Row;

        //    if (nRowDur >= 0 && nRowSayu >= 0)
        //    {
        //        string strSelectDurValue = ssDurResult.ActiveSheet.Cells[nRowDur, 0].Text;  //처방모듈명
        //        string strSelectDurLv = ssDurResult.ActiveSheet.Cells[nRowDur, 1].Text;  //등급
        //        //string strSelectCd = ssSayu.ActiveSheet.Cells[nRowSayu, 0].Text;    //선택한코드
        //        //string strSelectSayu = ssSayu.ActiveSheet.Cells[nRowSayu, 1].Text;  //상세사유

        //        //Value 값을 다시 코드값으로
        //        string key = DurType.FirstOrDefault(x => x.Value == strSelectDurValue).Key;

        //        if (ChkTextSayu(key, strSelectDurLv) == "사유기재")
        //        {
        //            if (strSelectCd != "T")
        //            {
        //                //폼 로드 시 사유 필수 셋팅
        //                MessageBox.Show("사유를 직접 입력해야하는 처방점검항목입니다.");                                                
        //            }
        //            ssDurResult.ActiveSheet.SetActiveCell(nRowDur, 5);   //사유입력 활성화
        //        }
        //        else if (ChkTextSayu(key, strSelectDurLv) == "A~X")
        //        {
        //            ssDurResult.ActiveSheet.Cells[nRowDur, 4].Text = strSelectCd;
        //            if (ssDurResult.ActiveSheet.Cells[nRowDur, 4].Text == "T")
        //            {
        //                ssDurResult.ActiveSheet.Cells[nRowDur, 5].Text = "";
        //                ssDurResult.ActiveSheet.SetActiveCell(nRowDur, 5);
        //            }
        //            else
        //            {
        //                ssDurResult.ActiveSheet.Cells[nRowDur, 5].Text = strSelectCd;   //코드입력은 사유도 코드로
        //            }
        //        }
        //        else if (ChkTextSayu(key, strSelectDurLv) == "F~X")
        //        {
        //            if (strSelectCd == "A" || strSelectCd == "B" || strSelectCd == "C")
        //            {
        //                MessageBox.Show("중복처방 항목이 아닙니다. 다른코드를 선택해주십시오.");                        
        //            }
        //            else if (strSelectCd == "T")
        //            {
        //                ssDurResult.ActiveSheet.Cells[nRowDur, 4].Text = strSelectCd;
        //                ssDurResult.ActiveSheet.SetActiveCell(nRowDur, 5);   //사유입력 활성화
        //            }
        //            else
        //            {
        //                ssDurResult.ActiveSheet.Cells[nRowDur, 4].Text = strSelectCd;
        //                ssDurResult.ActiveSheet.Cells[nRowDur, 5].Text = strSelectCd;
        //            }
        //        }
        //        else if (ChkTextSayu(key, strSelectDurLv) == "D등급")
        //        {
        //            if (MessageBox.Show("D등급인 경우 정보제공 아니면 Text사유 전송가능합니다. 사유입력하시겠습니까?", "D등급항목",MessageBoxButtons.YesNo) == DialogResult.Yes)
        //            {
        //                ssDurResult.ActiveSheet.Cells[nRowDur, 4].Text = "T";
        //                ssDurResult.ActiveSheet.SetActiveCell(nRowDur, 5);   //사유입력 활성화                        
        //            }                   
        //        }
        //    }
        //}

        private void ssDurResult_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 6 && e.Row >= 0)
            {
                //frmMedScreenOrderResultDetail frm = new frmMedScreenOrderResultDetail(RsInfo[(int)VB.Val(ssDurResult.ActiveSheet.Cells[e.Row, 7].Text)]);
                //frm.ShowDialog();


                //2020-09-09 추가
                //this.Height = 957;
                //ssDurResult.Height = 185;

                if (ssDurResult.ActiveSheet.Cells[e.Row, 7].Text != "")
                {
                    frmMedScreenOrderResultDetailx = new frmMedScreenOrderResultDetail(RsInfo[(int)VB.Val(ssDurResult.ActiveSheet.Cells[e.Row, 7].Text)]);
                    CMF.setCtrlLoad(panheader5Sub2, frmMedScreenOrderResultDetailx);
                }
            }
        }

        /// <summary>
        /// 약물상호작용 상세정보 화면 띄우기
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ssDIFResult_CellDoubleClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Column == 4 && e.Row >= 0 && ssDIFResult.ActiveSheet.Cells[e.Row, 6].Text != "")
            {
                string strModuleID = ssDIFResult.ActiveSheet.Cells[e.Row, 5].Text;
                string strMonoGraphID = ssDIFResult.ActiveSheet.Cells[e.Row, 6].Text;
                string strAppName = @"C:\cmc\ocsexe\ClinicalRef.exe";
                string strArguments = " ," + strModuleID + "," + strMonoGraphID;

                Process p = new Process();
                p.StartInfo.FileName = strAppName;
                p.StartInfo.Arguments = strArguments;
                p.StartInfo.WindowStyle = ProcessWindowStyle.Normal;

                p.Start();

                //ProcessStartInfo info = new ProcessStartInfo(strAppName);
                //info.WindowStyle = ProcessWindowStyle.Minimized;
                //info.Arguments = "FIRSTDIS.exe";
                //Process.Start(info);
            }
        }

        private void frmMedScreenOrderResult_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (OrderSendFlag == true)
            {
                clsDur.gstrDur_취소 = "전송";
            }
            else
            {
                clsDur.gstrDur_취소 = "취소";
            }
        }

        private void btnSayu_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < ssDurResult.ActiveSheet.NonEmptyRowCount; i++)
            {
                if (ssDurResult.ActiveSheet.Cells[i, 4].Text == "T")
                {
                    ssDurResult.ActiveSheet.Cells[i, 5].Text = txtSayu.Text;
                }
            }
        }

        private void txtSayu_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnSayu.PerformClick();
            }
        }

        void ssDurResult_EditModeOff(object sender, EventArgs e)
        {
            int nCol = this.ssDurResult.ActiveSheet.ActiveColumn.Index;
            int nRow = this.ssDurResult.ActiveSheet.ActiveRow.Index;

            if (ssDurResult.ActiveSheet.Rows.Count == 0) return;

            if (nCol == 4)
            {
                if (ssDurResult.ActiveSheet.Cells[nRow, nCol].Text != ""
                    && (ssDurResult.ActiveSheet.Cells[nRow, nCol + 1].Text == "" || ssDurResult.ActiveSheet.Cells[nRow, nCol + 1].Text.Length < 2))
                {
                    string strCode = VB.Left(ssDurResult.ActiveSheet.Cells[nRow, nCol].Text, 1);

                    if (strCode == "T")
                    {
                        ssDurResult.ActiveSheet.SetActiveCell(nRow, nCol + 1);
                    }
                    else
                    {
                        //ssDurResult.ActiveSheet.Cells[nRow, nCol + 1].Text = strCode; 
                        //2020-09-03 안정수, 사유 자동으로 들어가도록 보완 
                        ssDurResult.ActiveSheet.Cells[nRow, nCol + 1].Text = VB.Mid(ssDurResult.ActiveSheet.Cells[nRow, nCol].Text, 3, ssDurResult.ActiveSheet.Cells[nRow, nCol].Text.Length);
                        ssDurResult.ActiveSheet.SetActiveCell(nRow, nCol + 1);
                    }

                    //ssDurResult.ActiveSheet.Cells[nRow, nCol].Text = strCode;     

                    //2020-09-09 추가 
                    //this.Height = 657;
                    //ssDurResult.Height = 304;
                }
            }
        }

        void ssDurResult_EditChange(object sender, FarPoint.Win.Spread.EditorNotifyEventArgs e)
        {
            int nCol = this.ssDurResult.ActiveSheet.ActiveColumn.Index;
            int nRow = this.ssDurResult.ActiveSheet.ActiveRow.Index;

            if (ssDurResult.ActiveSheet.Rows.Count == 0) return;

            if (nCol == 4)
            {
                if (ssDurResult.ActiveSheet.Cells[nRow, nCol].Text != "")
                {
                    string strCode = VB.Left(ssDurResult.ActiveSheet.Cells[nRow, nCol].Text, 1);

                    if (strCode == "T")
                    {
                        ssDurResult.ActiveSheet.SetActiveCell(nRow, nCol + 1);
                    }
                    else
                    {
                        //ssDurResult.ActiveSheet.Cells[nRow, nCol + 1].Text = strCode; 
                        //2020-09-03 안정수, 사유 자동으로 들어가도록 보완 
                        ssDurResult.ActiveSheet.Cells[nRow, nCol + 1].Text = VB.Mid(ssDurResult.ActiveSheet.Cells[nRow, nCol].Text, 3, ssDurResult.ActiveSheet.Cells[nRow, nCol].Text.Length);
                        ssDurResult.ActiveSheet.SetActiveCell(nRow, nCol + 1);
                    }

                    //ssDurResult.ActiveSheet.Cells[nRow, nCol].Text = strCode;    

                    //2020-09-09 추가 
                    //this.Height = 657;
                    //ssDurResult.Height = 304;
                }
            }
        }

        void ssDurResult_CellClick(object sender, FarPoint.Win.Spread.CellClickEventArgs e)
        {
            if (e.Row >= 0 && e.Column == 6)
            {
                if (ssDurResult.ActiveSheet.Cells[e.Row, 7].Text != "")
                {
                    frmMedScreenOrderResultDetailx = new frmMedScreenOrderResultDetail(RsInfo[(int)VB.Val(ssDurResult.ActiveSheet.Cells[e.Row, 7].Text)]);
                    CMF.setCtrlLoad(panheader5Sub2, frmMedScreenOrderResultDetailx);
                }
            }
        }

        private void btnMacro_Click(object sender, EventArgs e)
        {
            frmMedDurSet01 f = new frmMedDurSet01();
            f.ShowDialog();

            if (f != null)
            {
                f.Dispose();
                f = null;
                clsApi.FlushMemory();
            }
        }

        void txtSayu_KeyUp(object sender, KeyEventArgs e)
        {
            //string SQL = "";
            //string SqlErr = "";
            DataTable dt = null;

            #region //기능키 체크      

            string s = string.Empty;

            if (e.KeyCode == Keys.F1)
            {
                s = "F1";
            }
            else if (e.KeyCode == Keys.F2)
            {
                s = "F2";
            }
            else if (e.KeyCode == Keys.F3)
            {
                s = "F3";
            }
            else if (e.KeyCode == Keys.F4)
            {
                s = "F4";
            }
            else if (e.KeyCode == Keys.F5)
            {
                s = "F5";
            }
            else if (e.KeyCode == Keys.F6)
            {
                s = "F6";
            }
            else if (e.KeyCode == Keys.F7)
            {
                s = "F7";
            }
            else if (e.KeyCode == Keys.F8)
            {
                s = "F8";
            }
            else if (e.KeyCode == Keys.F9)
            {
                s = "F9";
            }
            else if (e.KeyCode == Keys.F10)
            {
                s = "F10";
            }
            #endregion

            if (s != "")
            {
                dt = sel_Resultward(clsDB.DbCon, Convert.ToInt32(clsType.User.IdNumber), s);
                if (ComFunc.isDataTableNull(dt) == false)
                {
                    txtSayu.Paste(dt.Rows[0]["WardName"].ToString().Trim()); //현재커서로 붙여넣기                  
                }
            }
        }

        public DataTable sel_Resultward(PsmhDb pDbCon, long argSabun, string argCode, bool bTO = false)
        {
            string SQL = "";
            string SqlErr = "";
            DataTable dt = null;

            SQL = "";
            SQL += "SELECT                                                  \r\n";
            SQL += "  Code,WardName,ROWID                                   \r\n";
            SQL += " FROM " + ComNum.DB_PMPA + "ETC_DURSAYU_WARD            \r\n";
            SQL += "  WHERE 1=1                                             \r\n";
            SQL += "   AND Sabun =" + argSabun + "                          \r\n";
            if (argCode != "")
            {
                SQL += "   AND Code ='" + argCode + "'                      \r\n";
            }
            SQL += "   ORDER BY CODE                                        \r\n";

            try
            {
                SqlErr = clsDB.GetDataTable(ref dt, SQL, pDbCon);

                if (SqlErr != "")
                {
                    ComFunc.MsgBox("조회중 문제가 발생했습니다");
                    clsDB.SaveSqlErrLog(SqlErr, SQL, pDbCon); //에러로그 저장
                    return null;
                }
            }
            catch (System.Exception ex)
            {
                clsDB.SaveSqlErrLog(ex.Message, SQL, pDbCon); //에러로그 저장
                return null;
            }

            return dt;
        }

        void ssDurResult_KeyUp(object sender, KeyEventArgs e)
        {
            #region //기능키 체크

            string s = string.Empty;

            if (e.KeyCode == Keys.F1)
            {
                s = "F1";
            }
            else if (e.KeyCode == Keys.F2)
            {
                s = "F2";
            }
            else if (e.KeyCode == Keys.F3)
            {
                s = "F3";
            }
            else if (e.KeyCode == Keys.F4)
            {
                s = "F4";
            }
            else if (e.KeyCode == Keys.F5)
            {
                s = "F5";
            }
            else if (e.KeyCode == Keys.F6)
            {
                s = "F6";
            }
            else if (e.KeyCode == Keys.F7)
            {
                s = "F7";
            }
            else if (e.KeyCode == Keys.F8)
            {
                s = "F8";
            }
            else if (e.KeyCode == Keys.F9)
            {
                s = "F9";
            }
            else if (e.KeyCode == Keys.F10)
            {
                s = "F10";
            }
            #endregion

            if (s != "")
            {
                if (ssDurResult.ActiveSheet.Rows.Count > 0)
                {
                    DataTable dt = sel_Resultward(clsDB.DbCon, Convert.ToInt32(clsType.User.IdNumber), s);
                    if (ComFunc.isDataTableNull(dt) == false)
                    {
                        ssDurResult.ActiveSheet.Cells[ssDurResult.ActiveSheet.ActiveRowIndex, 5].Text = dt.Rows[0]["WardName"].ToString().Trim(); //현재커서로 붙여넣기                  
                    }
                }

            }
        }
    }
}
