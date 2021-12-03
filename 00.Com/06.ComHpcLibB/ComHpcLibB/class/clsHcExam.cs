using ComBase;
using ComBase.Controls;
using ComHpcLibB.Dto;
using ComHpcLibB.Service;
using FarPoint.Win.Spread;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace ComHpcLibB
{
    public class clsHcExam
    {
        ComHpcLibBService comHpcLibBService = new ComHpcLibBService();
        ExamMasterService examMasterService = new ExamMasterService();
        ExamMasterSubService examMasterSubService = new ExamMasterSubService();
        ExamSpecodeService examSpecodeService = new ExamSpecodeService();
        ExamResultcService examResultcService = new ExamResultcService();
        ExamSpecmstService examSpecmstService = new ExamSpecmstService();
        ExamOrderService examOrderService = new ExamOrderService();

        /// <summary>
        /// Master Code의 모든 Sub Code 가져오기
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        public string AllSubCode(string argCode)
        {
            int i = 0;
            string rtnVal = string.Empty;

            string sSubCode = string.Empty;
            string sNormal = string.Empty;

            List<EXAM_MASTER_SUB> list = examMasterSubService.GetNormalByCode(argCode);

            sSubCode = "";
            for (i = 0; i < list.Count; i++)
            {
                sNormal = list[i].NORMAL;
                sSubCode = sSubCode + sNormal + "^";
            }

            i = (int)VB.L(sSubCode, "^");
            sSubCode = VB.Pstr(sSubCode, "^", 1, i - 1);

            rtnVal = sSubCode;

            return rtnVal;
        }

        /// <summary>
        /// Master Code의 모든 Sub Code 가져오기2
        /// </summary>
        /// <param name="argCode"></param>
        /// <returns></returns>
        /// <seealso cref="ExHea> ExMain00> AllSubCode2"/>
        public string AllSubCode2(string argCode)
        {
            string rtnVal = string.Empty;
            string sSubCode = string.Empty;
            string sNormal = string.Empty;

            List<EXAM_MASTER_SUB> list = examMasterSubService.GetNormalByCode(argCode);

            sSubCode = "";
            for (int i = 0; i < list.Count; i++)
            {
                sNormal = list[i].NORMAL;
                sSubCode = sSubCode + "'" + sNormal + "',";
            }

            if (VB.Right(sSubCode, 1) == ",")
            {
                sSubCode = VB.Left(sSubCode, sSubCode.Length - 1);
            }

            rtnVal = sSubCode;

            return rtnVal;
        }

        public string GetYNameBySpecCode(string argSpecCode, string argGubun)
        {
            string rtnVal = string.Empty;

            EXAM_SPECODE item = examSpecodeService.GetItemByCodeGubun(argSpecCode, argGubun);    //검체약어

            rtnVal = item.YNAME.Trim();

            return rtnVal;
        }

        public string ChkExcuteExamByPanoCode(string strPano, string strSpecNo, string strMasterCode, string strSubCode)
        {
            string rtnVal = examResultcService.ChkExcuteExamByPanoCode(strPano, strSpecNo, strMasterCode, strSubCode) == "" ? "N" : "Y";    //검체약어

            return rtnVal;
        }

        public string EXAM_SPEC_WRITE_TLA(FpSpread spBarCode)
        {
            string rtnVal = string.Empty;
            string strMasterCode = string.Empty;
            string strDrSpec = string.Empty;
            string strSTRT = string.Empty;
            string strDrComment = string.Empty;
            string strSuCode = string.Empty;
            string strOTime = string.Empty;
            string strOrderDate = string.Empty;
            string strSendDate = string.Empty;
            string strBloodTime = string.Empty;
            string strGbGwaExam = string.Empty;
            string strSpecCode = string.Empty;
            string strWsCode = string.Empty;
            string strTubeCode = string.Empty;
            string strWsPos = string.Empty;
            string strMomo = string.Empty;
            string strEquCode = string.Empty;
            string strGBTLA = string.Empty;
            string strResultIn = string.Empty;
            string strUnitCode = string.Empty;
            string strPiece = string.Empty;
            string strSerialOK = string.Empty;
            string strWsGrp = string.Empty;
            string strPrt = string.Empty;
            string strAllSubCode = string.Empty;
            string strSubCode = string.Empty;
            string strOldData = string.Empty;
            string strNewData = string.Empty;
            string strSpecNo = string.Empty;
            string strWorkSTS = string.Empty;
            string strGbOrder = string.Empty;
            string strWsBar = string.Empty;

            int j = 0;
            int nSerialNo = 0;
            int nSeqNo = 0;
            int result = 0;

            long nOrderNo = 0;

            ComFunc.ReadSysDate(clsDB.DbCon);

            string strJepTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;

            #region 접수할 내역을 spBarcode에 SET
            spBarCode.ActiveSheet.ClearRange(0, 0, spBarCode.ActiveSheet.Rows.Count, spBarCode.ActiveSheet.ColumnCount, true);
            spBarCode.ActiveSheet.Rows.Count = 0;

            //Order내역을 spBarCode에 SET
            int nRow = 0;

            for (int i = 0; i < clsHcType.TORD.OrderCNT; i++)
            {
                strMasterCode = VB.Pstr(clsHcType.TORD.Order[i], "^", 1).Trim();        //검사코드
                strDrSpec = VB.Pstr(clsHcType.TORD.Order[i], "^", 2).Trim();        //의사선택 검체코드
                strSTRT = VB.Pstr(clsHcType.TORD.Order[i], "^", 3).Trim();        //응급여부
                strDrComment = VB.Pstr(clsHcType.TORD.Order[i], "^", 4).Trim();        //의사컴멘트
                strSuCode = VB.Pstr(clsHcType.TORD.Order[i], "^", 5).Trim();        //수가코드,오더코드(rowid)
                strOTime = VB.Pstr(clsHcType.TORD.Order[i], "^", 6).Trim();        //의사채혈희망시각
                strOrderDate = VB.Pstr(clsHcType.TORD.Order[i], "^", 7).Trim();        //오더시각
                strSendDate = VB.Pstr(clsHcType.TORD.Order[i], "^", 8).Trim();        //전송시각
                nOrderNo = VB.Pstr(clsHcType.TORD.Order[i], "^", 9).To<long>();    //orderno

                //채혈희망시각(기본은 현재시각,오더에서 지정한경우는 지젇시각)
                strBloodTime = clsPublic.GstrSysDate + " " + clsPublic.GstrSysTime;
                strGbGwaExam = "N";

                //검사코드를 읽어 용기,검체,결과입력,모모를 READ
                EXAM_MASTER item = examMasterService.GetItemsByMasterCode(strMasterCode);

                if (!item.IsNullOrEmpty())
                {
                    //의사가 검체를 선택하면 우선 적용
                    if (strDrSpec == "")
                    {
                        strSpecCode = item.SPECCODE.Trim();
                    }
                    else
                    {
                        strSpecCode = strDrSpec;
                    }
                    strWsCode = item.WSCODE1.Trim();
                    strWsPos = item.WSCODE1POS.ToString("D:5");
                    strTubeCode = item.TUBECODE.Trim();
                    strResultIn = item.RESULTIN.Trim();
                    strUnitCode = item.UNITCODE.Trim();

                    if (strMasterCode == "BF10" && strDrSpec == "031")
                    {
                        strUnitCode = "043";
                    }

                    strMomo = item.MOTHER.Trim();
                    strEquCode = item.EQUCODE1.Trim();
                    strPiece = item.PIECE.Trim();
                    strGBTLA = item.GBTLA.Trim();


                    strSerialOK = ""; //연속검사 여부
                    if (item.SERIES > 0) { strSerialOK = "OK"; }
                    //과검사(응급실) 여부
                    strGbGwaExam = "N";
                    if (item.CHKGWA == "Y")
                    {
                        strSpecCode = "080"; //기타
                        strTubeCode = "110"; //기타
                        strGbGwaExam = "Y";
                    }

                    strWsGrp = item.WSGRP_TITLE;  //WS1기준으로 대표코드 설정
                }
                else
                {
                    clsPublic.GstrMsgList = strMasterCode + "가 검사마스타에 등록되지 않았습니다.";
                    MessageBox.Show(clsPublic.GstrMsgList, "코드오류");
                    strSpecCode = ""; strWsCode = ""; strTubeCode = "";
                    strWsPos = ""; strMomo = ""; strEquCode = ""; strGBTLA = "";
                    strGbGwaExam = "N";
                }

                if (strSerialOK == "OK")    //연속검사이면
                {
                    strAllSubCode = AllSubCode(strMasterCode);
                    nSerialNo = 0;
                    for (j = 0; j < VB.L(strAllSubCode, "^") - 1; j++)
                    {
                        strSubCode = VB.Pstr(strAllSubCode, "^", j);

                        //서브코드를 SELECT
                        EXAM_MASTER item2 = examMasterService.GetItemsByMasterCode(strSubCode);
                        if (!item2.IsNullOrEmpty())
                        {
                            if (strDrSpec == "")
                            {
                                strSpecCode = item2.SPECCODE.Trim();
                            }
                            else
                            {
                                strSpecCode = strDrSpec;
                            }
                            strWsCode = item2.WSCODE1.Trim();
                            strWsPos = item2.WSCODE1POS.ToString("D:5");
                            strTubeCode = item2.TUBECODE.Trim();
                            strResultIn = item2.RESULTIN.Trim();
                            strUnitCode = item2.UNITCODE.Trim();
                            if (strSubCode == "BF10" && strDrSpec == "031")
                            {
                                strUnitCode = "043";
                            }
                            strEquCode = item2.EQUCODE1.Trim();
                            strWsGrp = item.WSGRP_TITLE;  //WS1기준으로 대표코드 설정
                        }
                        else
                        {
                            clsPublic.GstrMsgList = strMasterCode + "의 SUB코드 " + strSubCode + "가 검사마스타에" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "등록되지 않았습니다. 접수시 누락됩니다.";
                            MessageBox.Show(clsPublic.GstrMsgList, "코드오류");
                            strWsCode = ""; strTubeCode = "";
                        }

                        //바코드분리를 위해 연속검사번호 부여(1번은 00,2번부터는 22,23,24,...)
                        if (j == 1) { nSerialNo = 0; }
                        else { nSerialNo = j + 20; }

                    }
                }
                else if (strMomo == "1")    //모코드를 Sub코드로 가진다면
                {
                    strAllSubCode = AllSubCode(strMasterCode);

                    for (j = 0; j < VB.L(strAllSubCode, "^") - 1; j++)
                    {
                        strSubCode = VB.Pstr(strAllSubCode, "^", j);

                        //서브코드를 SELECT
                        EXAM_MASTER item2 = examMasterService.GetItemsByMasterCode(strSubCode);
                        if (!item2.IsNullOrEmpty())
                        {
                            if (strDrSpec == "")
                            {
                                strSpecCode = item2.SPECCODE.Trim();
                            }
                            else
                            {
                                strSpecCode = strDrSpec;
                            }
                            strWsCode = item2.WSCODE1.Trim();
                            strWsPos = item2.WSCODE1POS.ToString("D:5");
                            strTubeCode = item2.TUBECODE.Trim();
                            strResultIn = item2.RESULTIN.Trim();
                            strUnitCode = item2.UNITCODE.Trim();
                            if (strSubCode == "BF10" && strDrSpec == "031")
                            {
                                strUnitCode = "043";
                            }
                            strEquCode = item2.EQUCODE1.Trim();
                            strPiece = item2.PIECE.Trim();
                            strGBTLA = item2.GBTLA.Trim();
                            strWsGrp = item.WSGRP_TITLE;  //WS1기준으로 대표코드 설정
                        }
                        else
                        {
                            clsPublic.GstrMsgList = strMasterCode + "의 SUB코드 " + strSubCode + "가 검사마스타에" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "등록되지 않았습니다. 접수시 누락됩니다.";
                            MessageBox.Show(clsPublic.GstrMsgList, "코드오류");
                            strWsCode = ""; strTubeCode = "";
                        }


                    }
                }

                #region Spead Show
                strPrt = "Y";

                nRow = nRow + 1;
                if (nRow > spBarCode.ActiveSheet.RowCount)
                {
                    spBarCode.ActiveSheet.RowCount = nRow;
                }

                spBarCode.ActiveSheet.Cells[nRow - 1, 0].Text = strSerialOK == "OK" ? nSerialNo.To<string>().ToString("D:2") : "00";  //바코드장수
                spBarCode.ActiveSheet.Cells[nRow - 1, 1].Text = strSpecCode;                            //검체코드
                spBarCode.ActiveSheet.Cells[nRow - 1, 2].Text = GetBasCode("WS그룹", strWsCode);         //WS그룹
                spBarCode.ActiveSheet.Cells[nRow - 1, 3].Text = strTubeCode;                             //용기코드
                spBarCode.ActiveSheet.Cells[nRow - 1, 4].Text = strWsCode;                               //WS코드
                spBarCode.ActiveSheet.Cells[nRow - 1, 5].Text = strWsPos;                                //WS POS
                spBarCode.ActiveSheet.Cells[nRow - 1, 6].Text = (strSerialOK == "OK" || strMomo == "1") ? strSubCode : strMasterCode;
                spBarCode.ActiveSheet.Cells[nRow - 1, 7].Text = "";                                      //검체번호
                spBarCode.ActiveSheet.Cells[nRow - 1, 8].Text = strSTRT;
                spBarCode.ActiveSheet.Cells[nRow - 1, 9].Text = strDrComment;                            //의사컴멘트
                spBarCode.ActiveSheet.Cells[nRow - 1, 10].Text = GetBasCode("WS약어", strWsCode);        //BarCode Work Station
                spBarCode.ActiveSheet.Cells[nRow - 1, 11].Text = strResultIn;                            //결과입력
                spBarCode.ActiveSheet.Cells[nRow - 1, 12].Text = strUnitCode;                            //결과단위
                spBarCode.ActiveSheet.Cells[nRow - 1, 13].Text = strEquCode;                             //장비코드
                spBarCode.ActiveSheet.Cells[nRow - 1, 14].Text = strSuCode;                              //ROWID
                spBarCode.ActiveSheet.Cells[nRow - 1, 15].Text = strWsGrp;                               //WS대표코드
                spBarCode.ActiveSheet.Cells[nRow - 1, 16].Text = strBloodTime;                           //채혈시각
                spBarCode.ActiveSheet.Cells[nRow - 1, 17].Text = strGbGwaExam;                           //과검사여부(Y/N)
                spBarCode.ActiveSheet.Cells[nRow - 1, 18].Text = strPiece;                               //개별발행여부
                spBarCode.ActiveSheet.Cells[nRow - 1, 19].Text = nOrderNo.To<string>();                  //오더번호
                spBarCode.ActiveSheet.Cells[nRow - 1, 20].Text = strPrt;                                 //실제발행여부
                spBarCode.ActiveSheet.Cells[nRow - 1, 21].Text = strGBTLA;                               //TLA
                #endregion

                //연속검사,검사코드,검체코드가 동일한것이 있으면 01,02,03, ... 번호를 부여하여 바코드를 각각 인쇄함
                j = 0; strOldData = "";

                for (j = 0; j < spBarCode.ActiveSheet.RowCount; j++)
                {
                    strNewData = spBarCode.ActiveSheet.Cells[i, 0].Text;    //연속검사

                    if (string.Compare(strNewData, "21") < 0)   //연속검사 2회분 부터는 분리않함
                    {
                        string strData = spBarCode.ActiveSheet.Cells[i, 0].Text;

                        if (string.Compare(strData, "BT022") >= 0 && string.Compare(strData, "BT023") <= 0)
                        {
                            strNewData = strNewData + "BT" + i.ToString("D:3");   //혈액은 바코드1장에 인쇄
                        }
                        else
                        {
                            strNewData = strNewData + strData;
                        }

                        //JJY(2009-04-16) 바코드 개별 발행이면 별도 발행하도록함
                        if (strPiece == "1")
                        {
                            strNewData = strNewData + spBarCode.ActiveSheet.Cells[i, 1].Text; //검체을 추가 해서 무조건 별도 발행
                        }

                        strNewData = strNewData + spBarCode.ActiveSheet.Cells[i, 1].Text;   //검체코드
                        if (strOldData != strNewData)               //검사코드가 다르면
                        {
                            spBarCode.ActiveSheet.Cells[i, 0].Text = "01";
                            strOldData = strNewData;
                            j = 1;
                        }
                        else                                          //검사코드가 같다면
                        {
                            j = j + 1;
                            spBarCode.ActiveSheet.Cells[i, 0].Text = j.ToString("D:2");
                        }
                    }
                }
            }
            #endregion

            #region DB에 등록처리
            clsDB.setBeginTran(clsDB.DbCon);

            clsHcVariable.gsBarSpecNo = "";

            strOldData = ""; strSpecNo = ""; strSTRT = "R";
            strDrComment = ""; strWorkSTS = ""; nSeqNo = 0;
            strGbOrder = ""; strGbGwaExam = ""; strGBTLA = "";

            for (int i = 0; i < spBarCode.ActiveSheet.RowCount; i++)
            {
                strNewData = spBarCode.ActiveSheet.Cells[i, 0].Text;                //Barcode 장수
                strNewData = strNewData + spBarCode.ActiveSheet.Cells[i, 1].Text;   //검체

                strGBTLA = spBarCode.ActiveSheet.Cells[i, 21].Text;                 //TLA

                if (strGBTLA == "1")
                {
                    strNewData = strNewData + "TLA";                                //Work Station Group =TLA
                }
                else
                {
                    strNewData = strNewData + spBarCode.ActiveSheet.Cells[i, 21].Text;      //Work Station Group
                }

                strNewData = strNewData + spBarCode.ActiveSheet.Cells[i, 3].Text;   //용기약어
                strNewData = strNewData + spBarCode.ActiveSheet.Cells[i, 17].Text;  //과검사
                strPiece = spBarCode.ActiveSheet.Cells[i, 18].Text;                 // 개별발행여부
                strPrt = spBarCode.ActiveSheet.Cells[i, 20].Text;                   //실제발행여부

                //바코드장수,검체코드,WS그룹,용기코드가 틀리면
                //JJY(2009-04-16) 바코드 개별 발행이면 별도 발행하도록함
                if (strPiece == "1")
                {
                    strNewData = strNewData + spBarCode.ActiveSheet.Cells[i, 1].Text + i.ToString(); //검체를 추가해서 무조건 별도 발행
                }

                if (strNewData != strOldData)
                {                    
                    //입력 Data 점검
                    if (CHECK_DATA_ERROR(strSpecCode, ref strWorkSTS, ref strDrComment))
                    {
                        //검체마스타에 INSERT
                        EXAM_SPECMST item = new EXAM_SPECMST
                        {
                            SPECNO = strSpecCode
                           , PANO = clsHcType.TORD.Pano
                           , BI = clsHcType.TORD.Bi
                           , SNAME = clsHcType.TORD.sName
                           , IPDOPD = clsHcType.TORD.IpdOpd
                           , AGE = clsHcType.TORD.Age
                           , AGEMM = clsHcType.TORD.AgeMM.To<long>()
                           , SEX = clsHcType.TORD.Sex
                           , DEPTCODE = clsHcType.TORD.DeptCode
                           , WARD = clsHcType.TORD.Ward
                           , ROOM = clsHcType.TORD.Room
                           , DRCODE = clsHcType.TORD.DrCode
                           , DRCOMMENT = strDrComment
                           , STRT = strSTRT
                           , SPECCODE = strSpecCode
                           , TUBE = strTubeCode
                           , WORKSTS = strWorkSTS
                           , BDATE = clsHcType.TORD.BDate
                           , EMR = "0"
                           , GB_GWAEXAM = strGbGwaExam
                           , HICNO = clsHcType.TORD.HICNO
                           , ORDERNO = nOrderNo
                        };

                        if (!strBloodTime.IsNullOrEmpty()) { item.BLOODDATE = strBloodTime; }
                        if (strGbGwaExam.Equals("Y"))
                        {
                            item.RECEIVEDATE = strJepTime;
                            item.STATUS = "01";
                        }
                        else if (clsHcType.TORD.Job == "입원")
                        {
                            item.STATUS = "00";
                        }
                        else if (clsHcType.TORD.Job == "신검")
                        {
                            item.STATUS = "00";
                        }
                        else if (clsHcType.TORD.Job == "종검")
                        {
                            item.STATUS = "00";
                        }
                        else
                        {
                            if (clsHcType.TORD.Job == "외래" && strWorkSTS == "M")
                            {
                                if (MessageBox.Show("미생물검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    item.RECEIVEDATE = strJepTime;
                                    item.STATUS = "01";
                                }
                                else
                                {
                                    item.STATUS = "00";
                                }
                            }
                            else if (clsHcType.TORD.Job == "외래" && strWorkSTS == "P,M")
                            {
                                if (MessageBox.Show("미생물, 대변검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    item.RECEIVEDATE = strJepTime;
                                    item.STATUS = "01";
                                }
                                else
                                {
                                    item.STATUS = "00";
                                }
                            }
                            else if (clsHcType.TORD.Job == "외래" && strWorkSTS == "P")
                            {
                                if (MessageBox.Show("대변검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    item.RECEIVEDATE = strJepTime;
                                    item.STATUS = "01";
                                }
                                else
                                {
                                    item.STATUS = "00";
                                }
                            }
                            else if (clsHcType.TORD.Job == "외래" && strWorkSTS == "E" && strSpecCode == "084")
                            {
                                if (MessageBox.Show("대변검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                {
                                    item.RECEIVEDATE = strJepTime;
                                    item.STATUS = "01";
                                }
                                else
                                {
                                    item.STATUS = "00";
                                }
                            }


                        }
                        item.ORDERDATE = strOrderDate;
                        item.SENDDATE = strSendDate;

                        //검체마스타에 INSERT
                        if (EXAM_SPECMST_INSERT(item) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsPublic.GstrMsgList = "EXAM_SPECMST INSERT 도중에 오류가 발생함" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "( 검체번호 : " + strSpecNo + " )";
                            MessageBox.Show(clsPublic.GstrMsgList, "오류");
                            rtnVal = "NO";
                            return rtnVal;
                        }

                        //실제발행 대상이면..
                        if (strPrt != "N") { clsHcVariable.gsBarSpecNo = clsHcVariable.gsBarSpecNo + strSpecNo + ","; } //바코드 인쇄할 검체번호

                        strSTRT = "R";
                    }

                    //2014-11-10 출장검진은 검체번호를 7000부터 별도 부여
                    if (clsHcVariable.GbHicChul == true)
                    {
                        strSpecNo = VB.Mid(clsPublic.GstrSysDate, 3, 2) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2);
                        strSpecNo = strSpecNo + comHpcLibBService.SpecNo_HicChul().ToString("D:4");
                    }
                    else
                    {
                        strSpecNo = VB.Mid(clsPublic.GstrSysDate, 3, 2) + VB.Mid(clsPublic.GstrSysDate, 6, 2) + VB.Right(clsPublic.GstrSysDate, 2);
                        strSpecNo = strSpecNo + comHpcLibBService.SpecNo().ToString("D:4");
                    }

                    strSpecCode = spBarCode.ActiveSheet.Cells[i, 1].Text;   //검체코드
                    strTubeCode = spBarCode.ActiveSheet.Cells[i, 3].Text;   //용기코드
                    strGbGwaExam = spBarCode.ActiveSheet.Cells[i, 17].Text; //과검사
                    nOrderNo = spBarCode.ActiveSheet.Cells[i, 19].Text.To<long>();
                    nSeqNo = 0; strWorkSTS = ""; strDrComment = "";
                    strOldData = strNewData;
                }

                //검체번호를 SHEET에 Move
                strSpecNo = spBarCode.ActiveSheet.Cells[i, 7].Text;
                //응급여부 SET
                if (spBarCode.ActiveSheet.Cells[i, 8].Text == "S" || spBarCode.ActiveSheet.Cells[i, 7].Text == "E") { strSTRT = "S"; }
                //검체마스타의 WS약어
                strWsBar = spBarCode.ActiveSheet.Cells[i, 10].Text; //Barcode Work Station                
                if (strWsBar != "")
                {
                    for (j = 0; j < strWorkSTS.Length - 1; j = j + 2)
                    {
                        if (VB.Mid(strWorkSTS, j, 1) == strWsBar) { j = 0; break; }
                    }

                    if (j != 0) { strWorkSTS = strWorkSTS + strWsBar + ","; }
                }
                //spBarCode.Visible = True

                strWsCode = spBarCode.ActiveSheet.Cells[i, 4].Text;      //WS코드
                strMasterCode = spBarCode.ActiveSheet.Cells[i, 6].Text;  //검사코드
                strResultIn = spBarCode.ActiveSheet.Cells[i, 11].Text;   //결과입력
                strUnitCode = spBarCode.ActiveSheet.Cells[i, 12].Text;   //결과단위
                strEquCode = spBarCode.ActiveSheet.Cells[i, 13].Text;    //장비코드
                strSuCode = spBarCode.ActiveSheet.Cells[i, 14].Text;     //ROWID
                strBloodTime = spBarCode.ActiveSheet.Cells[i, 16].Text;  //채혈희망시각

                //의사컴멘트 SET
                if (spBarCode.ActiveSheet.Cells[i, 9].Text.Trim() != "")
                {
                    if (strDrComment != "") { strDrComment = strDrComment + "," + ComNum.VBLF; }
                    strDrComment = strDrComment + strMasterCode + ":" + spBarCode.ActiveSheet.Cells[i, 9].Text;
                }

                strGbOrder = "1";

                #region EXAM_RESULTC_INSERT //검체별 검사내역 INSERT
                strSubCode = strMasterCode;
                nSeqNo = nSeqNo + 1;
                EXAM_RESULTC item2 = new EXAM_RESULTC
                {
                    SPECNO = strSpecNo,
                    RESULTWS = strWsBar,
                    EQUCODE = strEquCode,
                    SEQNO = nSeqNo.ToString("D:3"),
                    PANO = clsHcType.TORD.Pano,
                    MASTERCODE = strMasterCode,
                    SUBCODE = strSubCode,
                    UNIT = GetBasCode("결과단위", strUnitCode)
                };

                if (strResultIn == "1")
                {
                    item2.STATUS = "H";
                }
                else
                {
                    item2.STATUS = "N";
                }

                if (EXAM_RESULTC_INSERT(item2) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsPublic.GstrMsgList = "EXAM_RESULTC INSERT 도중에 오류가" + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "발생하여 RollBack을 합니다." + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "( 검체번호 : " + strSpecNo + " )" + " 검사코드:" + strMasterCode;
                    MessageBox.Show(clsPublic.GstrMsgList, "오류");
                    rtnVal = "NO";
                    return rtnVal;
                }

                strAllSubCode = AllSubCode(strMasterCode);
                if (strAllSubCode != "")
                {
                    for (j = 0; j < VB.L(strAllSubCode, "^"); j++)
                    {
                        strUnitCode = ""; strResultIn = "";

                        strSubCode = VB.Pstr(strAllSubCode, "^", j);
                        //서브코드를 SELECT
                        EXAM_MASTER itemSub = examMasterService.GetItemsByMasterCode(strSubCode);
                        if (!itemSub.IsNullOrEmpty())
                        {
                            strUnitCode = itemSub.UNITCODE.Trim();
                            strResultIn = itemSub.RESULTIN.Trim();
                        }

                        //EXAM_RESULTC_INSERT_SUB
                        nSeqNo = nSeqNo + 1;
                        EXAM_RESULTC item3 = new EXAM_RESULTC
                        {
                            SPECNO = strSpecNo,
                            RESULTWS = strWsBar,
                            EQUCODE = strEquCode,
                            SEQNO = nSeqNo.ToString("D:3"),
                            PANO = clsHcType.TORD.Pano,
                            MASTERCODE = strMasterCode,
                            SUBCODE = strSubCode,
                            UNIT = GetBasCode("결과단위", strUnitCode)
                        };

                        if (strResultIn == "1")
                        {
                            item3.STATUS = "H";
                        }
                        else
                        {
                            item3.STATUS = "N";
                        }

                        if (EXAM_RESULTC_INSERT(item3) == false)
                        {
                            clsDB.setRollbackTran(clsDB.DbCon);
                            clsPublic.GstrMsgList = "EXAM_RESULTC INSERT 도중에 오류가" + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "발생하여 RollBack을 합니다." + ComNum.VBLF;
                            clsPublic.GstrMsgList = clsPublic.GstrMsgList + "( 검체번호 : " + strSpecNo + " )" + " 검사코드:" + strMasterCode;
                            MessageBox.Show(clsPublic.GstrMsgList, "오류");
                            rtnVal = "NO";
                            return rtnVal;
                        }
                    }
                }
                #endregion

                //EXAM_ORDER_UPDATE   
                //검사오더에 FLAG SET
                if (clsHcType.TORD.Job == "외래" && strSuCode != "")
                {
                    EXAM_ORDER itemSub01 = new EXAM_ORDER();
                    itemSub01.SPECCODE = strSpecCode;
                    itemSub01.STRT = strSTRT;
                    itemSub01.DRCOMMENT = strDrComment;
                    itemSub01.SPECNO = strSpecNo;

                    result = examOrderService.UpDate(itemSub01, strSuCode);
                    if (result <= 0)
                    {
                        clsDB.setRollbackTran(clsDB.DbCon);
                        clsPublic.GstrMsgList = "EXAM_ORDER에 검체번호" + strSpecNo + "를 " + ComNum.VBLF;
                        clsPublic.GstrMsgList = clsPublic.GstrMsgList + "UPDATE도중에 오류가 발생하여" + ComNum.VBLF;
                        clsPublic.GstrMsgList = clsPublic.GstrMsgList + "RollBack을 합니다.";
                        MessageBox.Show(clsPublic.GstrMsgList, "오류");
                        rtnVal = "NO";
                        return rtnVal;
                    }
                }
            }

            //검체마스타에 INSERT
            //입력 Data 점검
            if (CHECK_DATA_ERROR(strSpecCode, ref strWorkSTS, ref strDrComment))
            {
                //검체마스타에 INSERT
                EXAM_SPECMST item = new EXAM_SPECMST
                {
                    SPECNO = strSpecCode
                    , PANO = clsHcType.TORD.Pano
                    , BI = clsHcType.TORD.Bi
                    , SNAME = clsHcType.TORD.sName
                    , IPDOPD = clsHcType.TORD.IpdOpd
                    , AGE = clsHcType.TORD.Age
                    , AGEMM = clsHcType.TORD.AgeMM.To<long>()
                    , SEX = clsHcType.TORD.Sex
                    , DEPTCODE = clsHcType.TORD.DeptCode
                    , WARD = clsHcType.TORD.Ward
                    , ROOM = clsHcType.TORD.Room
                    , DRCODE = clsHcType.TORD.DrCode
                    , DRCOMMENT = strDrComment
                    , STRT = strSTRT
                    , SPECCODE = strSpecCode
                    , TUBE = strTubeCode
                    , WORKSTS = strWorkSTS
                    , BDATE = clsHcType.TORD.BDate
                    , EMR = "0"
                    , GB_GWAEXAM = strGbGwaExam
                    , HICNO = clsHcType.TORD.HICNO
                    , ORDERNO = nOrderNo
                };

                if (!strBloodTime.IsNullOrEmpty()) { item.BLOODDATE = strBloodTime; }
                if (strGbGwaExam.Equals("Y"))
                {
                    item.RECEIVEDATE = strJepTime;
                    item.STATUS = "01";
                }
                else if (clsHcType.TORD.Job == "입원")
                {
                    item.STATUS = "00";
                }
                else if (clsHcType.TORD.Job == "신검")
                {
                    item.STATUS = "00";
                }
                else if (clsHcType.TORD.Job == "종검")
                {
                    item.STATUS = "00";
                }
                else
                {
                    if (clsHcType.TORD.Job == "외래" && strWorkSTS == "M")
                    {
                        if (MessageBox.Show("미생물검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            item.RECEIVEDATE = strJepTime;
                            item.STATUS = "01";
                        }
                        else
                        {
                            item.STATUS = "00";
                        }
                    }
                    else if (clsHcType.TORD.Job == "외래" && strWorkSTS == "P,M")
                    {
                        if (MessageBox.Show("미생물, 대변검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            item.RECEIVEDATE = strJepTime;
                            item.STATUS = "01";
                        }
                        else
                        {
                            item.STATUS = "00";
                        }
                    }
                    else if (clsHcType.TORD.Job == "외래" && strWorkSTS == "P")
                    {
                        if (MessageBox.Show("대변검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            item.RECEIVEDATE = strJepTime;
                            item.STATUS = "01";
                        }
                        else
                        {
                            item.STATUS = "00";
                        }
                    }
                    else if (clsHcType.TORD.Job == "외래" && strWorkSTS == "E" && strSpecCode == "084")
                    {
                        if (MessageBox.Show("대변검사입니다. 접수를 하겠습니까? 예:접수, 아니오:미접수", "확인", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            item.RECEIVEDATE = strJepTime;
                            item.STATUS = "01";
                        }
                        else
                        {
                            item.STATUS = "00";
                        }
                    }


                }
                item.ORDERDATE = strOrderDate;
                item.SENDDATE = strSendDate;

                //검체마스타에 INSERT
                if (EXAM_SPECMST_INSERT(item) == false)
                {
                    clsDB.setRollbackTran(clsDB.DbCon);
                    clsPublic.GstrMsgList = "EXAM_SPECMST INSERT 도중에 오류가 발생함" + ComNum.VBLF;
                    clsPublic.GstrMsgList = clsPublic.GstrMsgList + "( 검체번호 : " + strSpecNo + " )";
                    MessageBox.Show(clsPublic.GstrMsgList, "오류");
                    rtnVal = "NO";
                    return rtnVal;
                }

                //실제발행 대상이면..
                if (strPrt != "N") { clsHcVariable.gsBarSpecNo = clsHcVariable.gsBarSpecNo + strSpecNo + ","; } //바코드 인쇄할 검체번호

                strSTRT = "R";
            }

            clsDB.setCommitTran(clsDB.DbCon);
            #endregion

            rtnVal = "OK";

            return rtnVal;
        }

        private bool EXAM_RESULTC_INSERT(EXAM_RESULTC item)
        {
            bool rtnVal = true;

            int result = examResultcService.InsertData(item);
            if (result <= 0)
            {
                rtnVal = false;
            }

            return rtnVal;
        }

        private bool EXAM_SPECMST_INSERT(EXAM_SPECMST item)
        {
            bool rtnVal = true;

            int result = examSpecmstService.InsertData(item);
            if (result <= 0)
            {
                rtnVal = false;
            }

            return rtnVal;
        }

        private bool CHECK_DATA_ERROR(string strSpecCode, ref string strWorkSTS, ref string strDrComment)
        {
            bool rtnVal = true;

            if (strSpecCode == "")
            {
                rtnVal = false;
            }

            if (strWorkSTS != "")
            {
                strWorkSTS = VB.Left(strWorkSTS, strWorkSTS.Length - 1);
            }

            if (strDrComment.Length > 500)
            {
                strDrComment = VB.Left(strDrComment, 500);
            }

            if (clsHcType.TORD.sName.Length > 10)
            {
                clsHcType.TORD.sName = VB.Left(clsHcType.TORD.sName, 10);
            }

            return rtnVal;
        }

        /// <summary>
        /// 코드를 '검체,용기,결과단위,WS, 검체량로 변환한다.
        /// </summary>
        /// <seealso cref="READ_BasCode_NEW"/>>
        /// <param name="strGuBun">검체명,검체약어,용기명,용기약어,결과단위,검체량,WS명,WS약어,WS그룹만으로 사용 가능
        /// ※ 16은 검체량으로 사용</param>
        /// <param name="strCode"></param>
        /// <returns></returns>
        public string GetBasCode(string strGuBun, string strCode)
        {
            string strVal = "";
            string strCGuBun = "";
            string strDTColName = "";
            string SQL = "";
            string SqlErr = ""; //에러문 받는 변수

            DataTable dt = null;

            switch (strGuBun)
            {
                case "검체명":
                    strCGuBun = "14";
                    strDTColName = "NAME";
                    break;
                case "검체약어":
                    strCGuBun = "14";
                    strDTColName = "YNAME";
                    break;
                case "용기명":
                    strCGuBun = "15";
                    strDTColName = "NAME";
                    break;
                case "용기약어":
                    strCGuBun = "15";
                    strDTColName = "YNAME";
                    break;
                case "결과단위":
                    strCGuBun = "20";
                    strDTColName = "NAME";
                    break;
                case "검체량":
                    strCGuBun = "16";
                    strDTColName = "NAME";
                    break;
                case "WS명":
                    strCGuBun = "12";
                    strDTColName = "NAME";
                    break;
                case "WS약어":
                    strCGuBun = "12";
                    strDTColName = "YNAME";
                    break;
                case "WS그룹":
                    strCGuBun = "12";
                    strDTColName = "WSGROUP";
                    break;
                default:
                    return strVal;
            }

            try
            {
                EXAM_SPECODE item = examSpecodeService.GetItemByCodeGubun(strCGuBun, strCode);

                if (!item.IsNullOrEmpty())
                {
                    switch (strDTColName)
                    {
                        case "NAME": strVal = item.NAME.Trim(); break;
                        case "YNAME": strVal = item.YNAME.Trim(); break;
                        case "WSGROUP": strVal = item.WSGROUP.Trim(); break;
                        default: break;
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return strVal;

        }

    }
}
