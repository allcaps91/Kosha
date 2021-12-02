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
using System.Windows.Forms;

/// <summary>
/// Class Name      : ComHpcLibB
/// File Name       : frmHcSangEndoOrderClose.cs
/// Description     : 내시경대상자 EMR마감
/// Author          : 이상훈
/// Create Date     : 2020-11-30
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm내시경처방마감.frm(Frm내시경처방마감)" />

namespace ComHpcLibB
{
    public partial class frmHcSangEndoOrderClose : Form
    {
        HeaCodeService heaCodeService = null;
        ComHpcLibBService comHpcLibBService = null;
        HicDoctorService hicDoctorService = null;
        EndoJupmstService endoJupmstService = null;
        HicPatientService hicPatientService = null;
        HeaResvExamService heaResvExamService = null;
        OcsOrdercodeService ocsOrdercodeService = null;

        clsSpread sp = new clsSpread();
        clsHcMain hm = new clsHcMain();
        clsHaBase hb = new clsHaBase();
        clsHcFunc hf = new clsHcFunc();
        clsOrdFunction OF = new clsOrdFunction();

        public frmHcSangEndoOrderClose()
        {
            InitializeComponent();
            SetEvent();
        }

        void SetEvent()
        {
            heaCodeService = new HeaCodeService();
            comHpcLibBService = new ComHpcLibBService();
            hicDoctorService = new HicDoctorService();
            endoJupmstService = new EndoJupmstService();
            hicPatientService = new HicPatientService();
            heaResvExamService = new HeaResvExamService();
            ocsOrdercodeService = new OcsOrdercodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSearch.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
            this.chkAll.Click += new EventHandler(eChkBoxClick);
        }

        string fn_Read_Doctor_Hea()
        {
            string rtnVal = "";

            if (hicDoctorService.Read_Doctor_Hea(clsType.User.IdNumber.To<long>()) > 0)
            {
                rtnVal = "OK";
            }
            else
            {
                rtnVal = "";
            }

            return rtnVal;
        }

        string fn_READ_ORDER_CHK(string argPtNo, string argDeptCode)
        {
            string rtnVal = "";
            string SQLT = "";
            string strOK1 = "";
            long nPano = 0;

            HIC_PATIENT list = hicPatientService.GetPaNobyPtNo(argPtNo);

            if (!list.IsNullOrEmpty())
            {
                nPano = list.PANO;
            }

            if (comHpcLibBService.GetHeaCodeOcsOOrderbyDeptCodePtNo(argDeptCode, argPtNo) > 0)
            {
                rtnVal = "";
            }
            else
            {
                rtnVal = "OK";
            }

            if (rtnVal == "OK")
            {
                if (heaResvExamService.GetCountbyPaNoGbExam(nPano) == 0)
                {
                    rtnVal = "";
                }
            }

            return rtnVal;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSearch)
            {
                string strOK = "";
                string strOK1 = "";
                string strHEA = "";
                string strSName = "";
                int nROW = 0;

                List<ENDO_JUPMST> list = new List<ENDO_JUPMST>();

                sp.Spread_All_Clear(SS1);
                SS1_Sheet1.SetRowHeight(-1, 24);

                strSName = txtSName.Text.Trim();

                strHEA = fn_Read_Doctor_Hea();

                if (chkDC.Checked == true)
                {
                    strOK1 = "OK";
                }

                if (strHEA == "OK")
                {
                    chkNSB.Visible = true;

                    if (strOK1.IsNullOrEmpty())
                    {
                        list = endoJupmstService.GetItembySName(strSName, "1");
                    }
                    else
                    {
                        list = endoJupmstService.GetItembySName(strSName, "2");
                    }
                }
                else
                {
                    chkNSB.Visible = false;

                    if (strOK1.IsNullOrEmpty())
                    {
                        list = endoJupmstService.GetItembySName(strSName, "3");
                    }
                    else
                    {
                        list = endoJupmstService.GetItembySName(strSName, "4");
                    }
                }

                if (list.Count > 0)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        strOK = fn_READ_ORDER_CHK(list[i].PTNO, list[i].DEPTCODE);

                        if (strHEA == "OK")
                        {
                            if (list[i].DEPTCODE.Trim() == "TO" || list[i].DEPTCODE.Trim() == "HR")
                            {
                                if (strOK == "OK" || strOK1 == "OK")
                                {
                                    SS1.ActiveSheet.RowCount += 1;
                                    nROW = SS1.ActiveSheet.RowCount;
                                    SS1.ActiveSheet.Cells[nROW-1, 1].Text = list[i].SNAME.Trim();
                                    SS1.ActiveSheet.Cells[nROW-1, 2].Text = list[i].PTNO.Trim();
                                    if (!list[i].ORDERCODE.IsNullOrEmpty())
                                    {
                                        SS1.ActiveSheet.Cells[nROW - 1, 3].Text = list[i].ORDERCODE.Trim();
                                        SS1.ActiveSheet.Cells[nROW - 1, 4].Text = VB.Pstr(fn_Read_SuCode_Chk(list[i].ORDERCODE.Trim()), ",,", 3);

                                        //대장내시경일경우에만 전송
                                        if (list[i].ORDERCODE.Trim() != "00440110" && list[i].ORDERCODE.Trim() != "00440120")
                                        {
                                            SS1.ActiveSheet.Cells[nROW - 1, 5].Text = fn_Read_Endo_Jong(list[i].PTNO);
                                        }
                                    }
                                    
                                    SS1.ActiveSheet.Cells[nROW-1, 6].Text = list[i].DEPTCODE.Trim();
                                }
                            }
                        }
                        else
                        {
                            if (strOK == "OK" || strOK1 == "OK")
                            {
                                SS1.ActiveSheet.RowCount += 1;
                                nROW = SS1.ActiveSheet.RowCount;
                                SS1.ActiveSheet.Cells[nROW, 1].Text = list[i].SNAME.Trim();
                                SS1.ActiveSheet.Cells[nROW, 2].Text = list[i].PTNO.Trim();
                                if (!list[i].ORDERCODE.IsNullOrEmpty())
                                {
                                    SS1.ActiveSheet.Cells[nROW, 3].Text = list[i].ORDERCODE.Trim();
                                    SS1.ActiveSheet.Cells[nROW, 4].Text = VB.Pstr(fn_Read_SuCode_Chk(list[i].ORDERCODE.Trim()), ",,", 3);

                                    //대장내시경일경우에만 전송
                                    if (list[i].ORDERCODE.IsNullOrEmpty())
                                    {
                                        SS1.ActiveSheet.Cells[nROW, 5].Text = fn_Read_Endo_Jong(list[i].PTNO);
                                    }
                                    else if (list[i].ORDERCODE.Trim() != "00440110" && list[i].ORDERCODE.Trim() != "00440120")
                                    {
                                        SS1.ActiveSheet.Cells[nROW, 5].Text = fn_Read_Endo_Jong(list[i].PTNO);
                                    }
                                } 
                                SS1.ActiveSheet.Cells[nROW, 6].Text = list[i].DEPTCODE.Trim();
                            }
                        }
                    }
                }

            }
            else if (sender == btnSave)
            {
                string strPtNo = "";
                string strCODE = "";
                string strORDERCODE = "";
                string strORDERCODE1 = "";
                string strSuCode = "";
                string strSlipNo = "";
                string strDosCode = "";
                string strDRCODE = "";
                string strJong = "";
                string strOK = "";
                string strDeptCode = "";
                string strBun = "";
                int result = 0;

                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    if (SS1.ActiveSheet.Cells[i, 0].Text == "True")
                    {
                        strPtNo = SS1.ActiveSheet.Cells[i, 2].Text;
                        strCODE = SS1.ActiveSheet.Cells[i, 3].Text;
                        strJong = SS1.ActiveSheet.Cells[i, 5].Text;
                        strDeptCode = SS1.ActiveSheet.Cells[i, 6].Text;

                        if (chkDC.Checked == false)
                        {
                            clsDB.setBeginTran(clsDB.DbCon);

                            List<HEA_CODE> list = heaCodeService.GetAllbyGubunCode(strCODE, strJong, strPtNo);

                            if (list.Count > 0)
                            {
                                for (int j = 0; j < list.Count; j++)
                                {
                                    strOK = "";
                                    strDosCode = "";
                                    strORDERCODE = list[j].NAME.Trim();
                                    //strORDERCODE1 = list[j].ORDYN;
                                    strORDERCODE1 = fn_Read_SuCode_Chk(strORDERCODE);

                                    strSlipNo = VB.Pstr(strORDERCODE1, ",,", 1);
                                    strSuCode = VB.Pstr(strORDERCODE1, ",,", 2);
                                    strBun = VB.Pstr(strORDERCODE1, ",,", 4);

                                    strOK = fn_READ_ORDER_CHK1(strPtNo, strORDERCODE);
                                    strOK = list[j].ORDYN;

                                    if (!list[j].GUBUN1.IsNullOrEmpty())
                                    {
                                        strDosCode = list[j].GUBUN1;
                                    }

                                    strDRCODE = comHpcLibBService.Read_Ocs_Doctor(clsType.User.IdNumber.To<long>());

                                    if (strOK == "OK")
                                    {
                                        COMHPC item = new COMHPC();

                                        item.PTNO = strPtNo;
                                        item.BDATE = clsPublic.GstrSysDate;
                                        item.DEPTCODE = strDeptCode;
                                        item.SEQNO = 99;
                                        item.ORDERCODE = strORDERCODE;
                                        item.SUCODE = strSuCode;
                                        item.BUN = strBun;
                                        item.SLIPNO = strSlipNo;
                                        item.REALQTY = 1;
                                        item.QTY = 1;
                                        item.NAL = 1;
                                        item.GBDIV = "1";
                                        item.DOSCODE = strDosCode;
                                        item.GBBOTH = "0";
                                        item.GBINFO = "";
                                        item.GBER = "";
                                        item.GBSELF = "";
                                        item.GBSPC = "";
                                        item.BI = "51";
                                        item.DRCODE = strDRCODE;
                                        item.REMARK = "검사용";
                                        item.GBSUNAP = "1";
                                        item.TUYAKNO = 0;
                                        item.MULTI = "";
                                        item.MULTIREMARK = "";
                                        item.DUR = "";
                                        item.RESV = "";
                                        item.SCODESAYU = "";
                                        item.SCODEREMARK = "";
                                        item.GBSEND = "Y";
                                        item.SABUN = clsType.User.IdNumber;
                                        item.CORDERCODE = strORDERCODE;
                                        item.CSUCODE = strSuCode;
                                        item.CBUN = strBun;
                                        item.IP = clsPublic.GstrIpAddress;

                                        //2021-08-16
                                        item.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item.ORDERCODE, item.SUCODE, "", item.BDATE, item.DEPTCODE);

                                        result = comHpcLibBService.InsertOcsOrder(item);

                                        if (result < 0)
                                        {
                                            clsDB.setRollbackTran(clsDB.DbCon);
                                            MessageBox.Show("내시경 OCS처방전송 오류!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                            return;
                                        }
                                    }
                                }
                            }

                            //NSB수동 등록
                            if (chkNSB.Checked == true)
                            {
                                COMHPC item = new COMHPC();

                                item.PTNO = strPtNo;
                                item.BDATE = clsPublic.GstrSysDate;
                                item.DEPTCODE = strDeptCode;
                                item.SEQNO = 99;
                                item.ORDERCODE = "00440430";
                                item.SUCODE = "NSB";
                                item.BUN = "20";
                                item.SLIPNO = "0044";
                                item.REALQTY = 1;
                                item.QTY = 1;
                                item.NAL = 1;
                                item.GBDIV = "1";
                                item.DOSCODE = "920103";
                                item.GBBOTH = "0";
                                item.GBINFO = "";
                                item.GBER = "";
                                item.GBSELF = "";
                                item.GBSPC = "";
                                item.BI = "51";
                                item.DRCODE = strDRCODE;
                                item.REMARK = "검사용";
                                item.GBSUNAP = "1";
                                item.TUYAKNO = 0;
                                item.MULTI = "";
                                item.MULTIREMARK = "";
                                item.DUR = "";
                                item.RESV = "";
                                item.SCODESAYU = "";
                                item.SCODEREMARK = "";
                                item.GBSEND = "Y";
                                item.SABUN = clsType.User.IdNumber;
                                item.CORDERCODE = "00440430";
                                item.CSUCODE = "NSB";
                                item.CBUN = "20";
                                item.IP = clsPublic.GstrIpAddress;

                                //2021-08-16
                                item.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item.ORDERCODE, item.SUCODE, "", item.BDATE, item.DEPTCODE);

                                result = comHpcLibBService.InsertOcsOrder(item);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("내시경 NSB처방전송 오류!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            //5DW수동 등록
                            if (chk5DW.Checked == true)
                            {
                                COMHPC item = new COMHPC();

                                item.PTNO = strPtNo;
                                item.BDATE = clsPublic.GstrSysDate;
                                item.DEPTCODE = strDeptCode;
                                item.SEQNO = 99;
                                item.ORDERCODE = "00400311";
                                item.SUCODE = "5DWA-S";
                                item.BUN = "20";
                                item.SLIPNO = "0044";
                                item.REALQTY = 1;
                                item.QTY = 1;
                                item.NAL = 1;
                                item.GBDIV = "1";
                                item.DOSCODE = "920103";
                                item.GBBOTH = "0";
                                item.GBINFO = "";
                                item.GBER = "";
                                item.GBSELF = "";
                                item.GBSPC = "";
                                item.BI = "51";
                                item.DRCODE = strDRCODE;
                                item.REMARK = "검사용";
                                item.GBSUNAP = "1";
                                item.TUYAKNO = 0;
                                item.MULTI = "";
                                item.MULTIREMARK = "";
                                item.DUR = "";
                                item.RESV = "";
                                item.SCODESAYU = "";
                                item.SCODEREMARK = "";
                                item.GBSEND = "Y";
                                item.SABUN = clsType.User.IdNumber;
                                item.CORDERCODE = "00400311";
                                item.CSUCODE = "5DWA-S";
                                item.CBUN = "20";
                                item.IP = clsPublic.GstrIpAddress;

                                //2021-08-16
                                item.SUCODE = OF.Mapping_SuCode(clsDB.DbCon, item.ORDERCODE, item.SUCODE, "", item.BDATE, item.DEPTCODE);

                                result = comHpcLibBService.InsertOcsOrder(item);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("내시경 5DW처방전송 오류!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            //OILLS
                            if (comHpcLibBService.GetOcsIllsbyPtNoDeptCodeIllCode(strPtNo, "TO", "Z018") == 0)
                            {
                                result = comHpcLibBService.InsertOcsOills(strPtNo, clsPublic.GstrSysDate, "TO", 1, "Z018");

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("내시경 상병코드전송 오류!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                        else
                        {
                            clsDB.setBeginTran(clsDB.DbCon);

                            result = comHpcLibBService.DeleteOcsOrderTrans(strPtNo, clsType.User.IdNumber);

                            if (result < 0)
                            {
                                clsDB.setRollbackTran(clsDB.DbCon);
                                MessageBox.Show("처방DC전송 오류!!", "확인", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            clsDB.setCommitTran(clsDB.DbCon);
                        }
                    }
                }

                chkNSB.Checked = false;
                chk5DW.Checked = false;

                MessageBox.Show("EMR 데이터 전송을 완료 하였습니다.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);

                eBtnClick(btnSearch, new EventArgs());
            }
        }

        private string fn_Read_Endo_Jong(string pTNO)
        {
            string rtnVal = "";

            COMHPC list = comHpcLibBService.GetSuNapCodebyPtNo(pTNO);

            if (list.IsNullOrEmpty())
            {
                rtnVal = "Z1130";
            }
            else
            {
                rtnVal = list.CODE.Trim();
            }

            return rtnVal;
        }

        private string fn_Read_SuCode_Chk(string argOrderCode)
        {
            string rtnVal = "";

            OCS_ORDERCODE list = ocsOrdercodeService.GetItembyOrderCode(argOrderCode);

            if (!list.IsNullOrEmpty())
            {
                rtnVal = list.SLIPNO.Trim() + ",," + list.SUCODE.Trim() + ",," + list.ORDERNAME.Trim() + ",," + list.BUN.Trim();
            }

            return rtnVal;
        }

        string fn_READ_ORDER_CHK1(string argPtNo, string argOrderCode)
        {
            string rtnVal = "";

            if (comHpcLibBService.GetOcsOorderbyPtnoOrderCode(argPtNo, argOrderCode) > 0)
            {
                rtnVal = "";
            }
            else
            {
                rtnVal = "OK";
            }

            return rtnVal;
        }

        void eFormLoad(object sender, EventArgs e)
        {
            ComFunc.ReadSysDate(clsDB.DbCon);
        }

        void eChkBoxClick(object sender, EventArgs e)
        {
            if (chkAll.Checked == true)
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "True";
                }
            }
            else
            {
                for (int i = 0; i < SS1.ActiveSheet.RowCount; i++)
                {
                    SS1.ActiveSheet.Cells[i, 0].Text = "";
                }
            }
        }
    }
}
