using ComBase;
using ComBase.Controls;
using ComHpcLibB;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

/// <summary>
/// Class Name      : HC_Main
/// File Name       : frmHcOutPacsOrdCreate.cs
/// Description     : 출장검진PACS오더생성
/// Author          : 이상훈
/// Create Date     : 2020-07-27
/// Update History  : 
/// </summary>
/// <history>  
/// </history>
/// <seealso cref= "Frm출장검진PACS오더생성.frm(Frm출장검진PACS오더생성)" />

namespace HC_Main
{
    public partial class frmHcOutPacsOrdCreate : Form
    {
        HicJepsuService hicJepsuService = null;
        HicXrayResultService hicXrayResultService = null;
        HicResultExCodeService hicResultExCodeService = null;
        HicJepsuSunapService hicJepsuSunapService = null;
        XrayPacsOrderService xrayPacsOrderService = null;
        BasBcodeService basBcodeService = null;

        clsHcMain hm = new clsHcMain();
        clsHcOrderSend HOrdSend = new clsHcOrderSend();

        public frmHcOutPacsOrdCreate()
        {
            InitializeComponent();
            SetControl();
            SetEvent();
        }

        void SetControl()
        {

        }

        void SetEvent()
        {
            hicJepsuService = new HicJepsuService();
            hicXrayResultService = new HicXrayResultService();
            hicResultExCodeService = new HicResultExCodeService();
            hicJepsuSunapService = new HicJepsuSunapService();
            xrayPacsOrderService = new XrayPacsOrderService();
            basBcodeService = new BasBcodeService();

            this.Load += new EventHandler(eFormLoad);
            this.btnExit.Click += new EventHandler(eBtnClick);
            this.btnSave.Click += new EventHandler(eBtnClick);
        }

        void eFormLoad(object sender, EventArgs e)
        {
            string strData = "";

            ComFunc.ReadSysDate(clsDB.DbCon);

            dtpOutDate.Text = clsPublic.GstrSysDate;

            List<BAS_BCODE> list = basBcodeService.GetCodeNamebyBGubun("HIC_촬영기사");

            cboExId.Items.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                cboExId.Items.Add(list[i].CODE + "." + list[i].NAME);
            }

            cboExId.SelectedIndex = 0;
        }

        void eBtnClick(object sender, EventArgs e)
        {
            if (sender == btnExit)
            {
                this.Close();
                return;
            }
            else if (sender == btnSave)
            {
                int nREAD = 0;
                string strJobDate = "";
                string strXrayno = "";
                long nPano = 0;
                string strPtNo = "";
                string strExCode = "";
                string strGjjong = "";
                string strXrayName = "";
                string strXChk = "";
                long nWRTNO = 0;
                string strEXID = "";
                string strName = "";
                string strSex = "";
                string strAge = "";
                string strLtdCode = "";
                int result = 0;

                if (cboExId.Text.IsNullOrEmpty())
                {
                    MessageBox.Show("영상촬영기사 사번을 확인해주세요.", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                strJobDate = dtpOutDate.Text;
                strEXID = VB.Pstr(cboExId.Text, ".", 1);

                clsDB.setBeginTran(clsDB.DbCon);

                List<HIC_JEPSU> list = hicJepsuService.GetItembyJepDate(strJobDate);

                nREAD = list.Count;
                for (int i = 0; i < nREAD; i++)
                {
                    nPano = list[i].PANO;
                    nWRTNO = list[i].WRTNO;
                    strXrayno = list[i].XRAYNO;
                    strPtNo = list[i].PTNO;
                    strName = list[i].SNAME;
                    strSex = list[i].SEX;
                    strAge = list[i].AGE.ToString();
                    strGjjong = list[i].GJJONG;
                    strLtdCode = list[i].LTDCODE.ToString();
                   
                    if (!strXrayno.IsNullOrEmpty())
                    {
                        if (hicXrayResultService.GetCountbyPaNoXrayNoJepDate(nPano, strXrayno, strJobDate) == 0)
                        {
                            HIC_RESULT_EXCODE list2 = hicResultExCodeService.GetHicExCodeGroupCodebyWrtNo(nWRTNO);

                            if (list2.IsNullOrEmpty())
                            {
                                strExCode = "";
                                strXrayName = "";
                            }
                            else
                            {
                                strExCode = list2.EXCODE;
                                strXrayName = list2.HNAME;
                            }

                            if (!strExCode.IsNullOrEmpty())
                            {
                                //분진체크
                                if (hicJepsuSunapService.GetCountWrtNo(nWRTNO) > 0)
                                {
                                    strXChk = "Y";
                                }
                                //분진일경우 Chest-dust 적용
                                if (strXChk == "Y")
                                {
                                    strXrayName = "Chest-dust";
                                }

                                //INSERT
                                HIC_XRAY_RESULT item = new HIC_XRAY_RESULT();

                                item.JEPDATE = strJobDate;
                                item.XRAYNO = strXrayno;
                                item.PANO = nPano;
                                item.PTNO = strPtNo;
                                item.SNAME = strName;
                                item.SEX = strSex;
                                item.AGE = long.Parse(strAge);
                                item.GJJONG = strGjjong;
                                item.GBCHUL = "Y";
                                item.LTDCODE = long.Parse(strLtdCode);
                                item.XCODE = strExCode;
                                if (strXChk == "Y")
                                {
                                    item.GBREAD = "2";
                                }
                                else
                                {
                                    item.GBREAD = "1";
                                }
                                item.GBSTS = "0";
                                item.GBCONV = "Y";
                                item.ENTSABUN = long.Parse(clsType.User.IdNumber);
                                item.EXID = strEXID;

                                result = hicXrayResultService.Insert(item);

                                if (result < 0)
                                {
                                    clsDB.setRollbackTran(clsDB.DbCon);
                                    MessageBox.Show("일반검진 방사선촬영 판독 저장중 오류 발생!!!", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                //PACS에 오더를 전송함
                                if (xrayPacsOrderService.GetCountbyPatIdAcdessionNoExamDate(strPtNo, strXrayno, VB.Left(strXrayno, 8)) == 0)
                                {
                                    HOrdSend.HIC_PACS_SEND(nPano, strXrayno, "NW", strPtNo);
                                }
                            }
                        }
                    }
                }

                clsDB.setCommitTran(clsDB.DbCon);

                MessageBox.Show("작업 완료", "확인", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        
    }
}
    