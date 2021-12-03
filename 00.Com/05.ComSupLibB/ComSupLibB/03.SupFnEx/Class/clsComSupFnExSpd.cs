using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;

namespace ComSupLibB.SupFnEx
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupXray 
    /// File Name       : clsComSupXray.cs
    /// Description     : 진료지원 공통 기능검사 스프레드관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-23
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    public class clsComSupFnExSpd
    {
        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();
        

        #region 컨설트 조회 폼 관련 - frmComSupFnExVIEW01.cs

        public enum enmConsultview
        {
            BDate, FrDept, cDrName, InpDate, ToDept,
            dDrName, FrDrCode, ToDrCode, FrRemark, sDate,
            eDate, ToRemark, EntDate, ErSMS,WorkSTS, ROWID
        };
        public string[] sSpdConsultview = { "의뢰일","의뢰과","의뢰의","회신(결과)일","소견과",
                                     "소견의사","FrDrCode","TODrCode","FrRemark","sDate",
                                     "eDate","ToRemark","EntDate","구분","거동", "ROWID"  };
        public int[] nSpdConsultview = { 100, 100, 100, 100, 70,
                                  80, 100,100,100,70,
                                  70,100,80,85,100,30
                                };

        public void sSpd_Consultview(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmConsultview)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmConsultview.FrRemark, clsSpread.enmSpdType.Text, null, null, null, null, true);
            sup.setColStyle_Text(spd, -1, (int)enmConsultview.FrRemark, true, true, false, 4000); //txt재정의
            //methodSpd.setColStyle(spd, -1, (int)enmConsultview.ToRemark, clsSpread.enmSpdType.Text, null, null, null, null, true);
            sup.setColStyle_Text(spd, -1, (int)enmConsultview.ToRemark, true, true, false, 4000); //txt재정의


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmConsultview.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.FrDrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.ToDrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.FrRemark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.ToRemark, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.sDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.eDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.EntDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmConsultview.ROWID, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmConsultview.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmConsultview.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }

        #endregion

        #region 의사조회 관련 - frmComSupFnExVIEW02.cs

        public enum enmSpdDr { DeptCode, DrCode, DrName };

        public string[] sSpdDr = { "과", "의사코드", "의사명" };

        public int[] nSpdDr = { 30, 70, 100 };


        #endregion

        #region ABR 등록 폼 관련 - frmComSupFnExSET02.cs

        public enum enmAbrEnt
        {
            Chk,ExDate,ExTime, Pano, SName,
            SexAge, ExName,EntSabun,EntName, ROWID
        };
        public string[] sSpdAbrEnt = { "선택","검사일자","시간","등록번호","환자명",
                                        "성별/나이","검사명","입력사번","입력자", "ROWID"  };

        public int[] nSpdAbrEnt = {30,80,40,80,50,
                                   70,210,60,50,100 };

        public void sSpd_AbrEnt(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmAbrEnt)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmAbrEnt.Chk, clsSpread.enmSpdType.CheckBox);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmAbrEnt.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든            
            methodSpd.setColStyle(spd, -1, (int)enmAbrEnt.ROWID, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmAbrEnt.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmAbrEnt.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }

        #endregion

        #region 포스코 예약보기 폼 관련 - frmComSupFnExVIEW04.cs

        public enum enmPosco
        {
            JepDate,Pano,SName,Jumin,jobName
            ,Sabun,SinGu,USG,UGI,GFS
            ,GFS1,CFS,CT1,CT2,CT3
            ,CT4,CT5,Echo1,Echo2,Echo3
        };
        public string[] sSpdPosco = { "접수일","등록번호","성명","주민번호","등록자"
                                      ,"직번","신/구환","USG","UGI","GFS"
                                      ,"수면 GFS","수면 CFS","Chest CT","뇌 CT","경추 CT"
                                      ,"요추 CT","심장CT","심장초음파","경동맥초음파","뇌혈류초음파" };

        public int[] nSpdPosco = { 80,80,50,80,50
                                   ,50,40,80,80,80
                                   ,80,80,80,80,80
                                   ,80,80,80,90,90};

        public void sSpd_Posco(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmPosco)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            //methodSpd.setColStyle(spd, -1, (int)enmPosco.Chk, clsSpread.enmSpdType.CheckBox);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmPosco.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든            
            methodSpd.setColStyle(spd, -1, (int)enmPosco.SinGu, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.USG, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.UGI, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.GFS, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.GFS1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.CFS, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.CT1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.CT2, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.CT3, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.CT4, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmPosco.CT5, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmPosco.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmPosco.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }

        #endregion

        #region 심장초음파 시디복사 대장 폼 관련 - frmComSupFnExLIST01.cs

        public enum enmCdList1 { chk,  BDate, Ptno, SName, DeptCode
                                 , GbIO, Ward, Room,TeamNo, CdDate, sabun,Bigo,XName
                                ,ROWID };
        public string[] sSpdCdList1 = {"선택","처방일자","등록번호","성명","과"
                                        ,"입원외래","병동","호실","팀번호","작업일자","작업자","비고","검사명"
                                    ,"ROWID" };
        public int[] nSpdCdList1 = {30,80,80,60,30
                                    ,30,50,50,50,80,50,100,100
                                    ,80 };

        public void sSpd_CdList1(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmCdList1)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmCdList1.chk, clsSpread.enmSpdType.CheckBox);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmCdList1.chk, clsSpread.HAlign_R, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmCdList1.ROWID, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmCdList1.XName, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmOrderview.Gubun, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmCdList.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmCdList.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }


        #endregion

        #region 심장초음파 시디복사 리스트 폼 관련 - frmComSupFnExLIST01.cs

        public enum enmCdList2 { chk, STS,GbIO, BDate,SeekDate, Ptno, SName, DeptCode,DrCode, Ward, Room, Remark, XCdoe,XJong,XSub,QTY,CdGubun, OrderNo };
        public string[] sSpdCdList2 = {"선택","상태","구분","처방일자","SEEKDATE","등록번호","성명","과","DrCode",
                                   "병동","호실","참고사항","XCode","XJong","XSub","QTY","구분","오더넘버" };
        public int[] nSpdCdList2 = {30,40,30,70,70,70,50,30,50,
                                30,40,100,60,30,30,30,30,80 };

        public void sSpd_CdList2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmCdList2)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.chk, clsSpread.enmSpdType.CheckBox);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmCdList2.chk, clsSpread.HAlign_R, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.SeekDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.XCdoe, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.XJong, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.XSub, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.QTY, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.CdGubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmCdList2.OrderNo, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmCdList2.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmCdList2.BDate, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
        }


        #endregion

        #region //치매척도 결과등록 명단   enum, 배열변수 - frmComSupFnExRSLT05.cs
        /// <summary> 치매척도 결과등록 명단 enm </summary>        
        public enum enmSupFnExRSLT05
        {
            BDate, Pano, SName, Sex, Age,
            DeptCode, DrCode, DrName, gbIO, ExName,
            OrderNo, ROWID
        }
        //치매척도 결과등록 명단 컬럼헤드 배열 </summary>
        public string[] sSpdenmSupFnExRSLT05 = { "처방일","등록번호","성명","성별","나이",
                                                "진료과","의사코드","처방의","입원/외래","검사명",
                                                "Orderno","ROWID" };

        /// <summary> 치매척도 결과등록 명단 컬럼사이즈 배열 </summary>
        public int[] nSpdenmSupFnExRSLT05 = { 80,80,80,35,35,
                                              40,40,80,40,200,
                                              80,80 };


        /// <summary> 치매척도 결과등록 명단 스프레드 표시   </summary>  
        public void sSpd_enmSupFnExRSLT05(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupFnExRSLT05)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 30;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSupFnExRSLT05.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                        
            methodSpd.setColStyle(spd, -1, (int)enmSupFnExRSLT05.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupFnExRSLT05.OrderNo, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupFnExRSLT05.ROWID, clsSpread.enmSpdType.Hide);



        }

        #endregion

    }
}

