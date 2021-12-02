using ComLibB;
using ComDbB; //DB연결
using ComBase; //기본 클래스
using ComSupLibB.Com;
using System;

namespace ComSupLibB.SupEnds
{
    /// <summary>
    /// Class Name      : ComSupLibB.SupEnds 
    /// File Name       : clsComSupEndsSpd.cs
    /// Description     : 진료지원 공통 내시경 스프레드관련 class
    /// Author          : 윤조연
    /// Create Date     : 2017-06-30
    /// Update History  : 
    /// </summary>
    /// <history>  
    /// 
    /// </history> 
    public class clsComSupEndsSpd
    {

        clsMethod method = new clsMethod();
        clsSpread methodSpd = new clsSpread();
        clsComSup sup = new clsComSup();

        #region //내시경 상용결과 관리 - frmComSupEndsSET01.cs
        public enum enmEndsSet
        {
            Jong,JongSub,Title,ROWID
        };

        public string[] sSpdEndsSet = {"종류","세부종류","명칭","ROWID" };
        public int[] nSpdEndsSet = { 30,80,230,80};

        public void sSpd_enmEndsSet(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmEndsSet)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmEndsSet.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmEndsSet.Title, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmEndsSet.Jong, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmEndsSet.JongSub, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmEndsSet.ROWID, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmEndsSet.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmEndsSet.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


        }
        #endregion

        #region //내시경 전화통보명단
        public enum enmTelview
        {
            SName, Pano, Age, Sex, ExName,
            Tel, RDate, TDate, Tongbo, Bigo
        };

        public string[] sSpdTelview = {"성명","등록번호","나이","성별","검사명",
                                "전화번호","검사예약일","전화통지일","통보자","비고" };
        public int[] nSpdTelview = { 70,70,20,20,250,
                               120,80,80,70,50};

        public void sSpd_enmTelview(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmTelview)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmTelview.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmTelview.ExName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmTelview.Bun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmTelview.Gubun, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmTelview.GbIO, true);
            methodSpd.setSpdFilter(spd, (int)enmTelview.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

        }

        #endregion

        #region //내시경 물품내역
        public enum enmGumeview
        {
            CDate,JepCode,JepName,Unit,Qty
            ,Bigo
        };

        public string[] sSpdGumeview = {"청구일자","물품코드","물품명","규격","청구수량",
                                        "비고" };
        public int[] nSpdGumeview = { 80,90,300,50,60
                               ,110};

        public void sSpd_enmGumeview(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmGumeview)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmTelview.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmGumeview.JepName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmGumeview.Bigo, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmGumeview.Bun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmGumeview.Gubun, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmGumeview.GbIO, true);
            methodSpd.setSpdFilter(spd, (int)enmGumeview.JepName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

        }

        #endregion

        #region //내시경 소독약품내역 - frmComSupEndsVIEW03.cs
        public enum enmDrugview
        {
            CDate, JepCode, JepName, Unit, Qty
            , Bigo
        };

        public string[] sSpdDrugview = {"청구일자","약품코드","약뭄명/성분명","규격","수량",
                                        "비고" };
        public int[] nSpdDrugview = { 80,90,300,50,60
                               ,110};

        public void sSpd_enmDrugview(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmDrugview)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmDrugview.Pano, clsSpread.enmSpdType.Text);


            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmDrugview.JepName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmDrugview.Bigo, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmDrugview.Bun, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmDrugview.Gubun, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmDrugview.GbIO, true);
            methodSpd.setSpdFilter(spd, (int)enmDrugview.JepName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

        }

        #endregion

        #region //내시경 통합장부 - frmComSupEndsVIEW04.cs
        public enum enmTotalView
        {
            chk,RDate,Ptno, SName,Sex,Age,GBIO, Buse, Diag
            ,OrderName,RTime,DrCode,DrName,Nurse,Gubun
            ,Change,ROWID
        };

        public string[] sSpdTotalView = {"선택","검사일자","등록번호","성명","성별","나이","입원외래","부서","진단명"
                                         ,"시술명","검사시간","의사코드","처치의사","간호사","구분"
                                         ,"수정","ROWID" };
        public int[] nSpdTotalView = {30, 80,60,70,30,30,35,60,300
                                      ,170,90,60,60,200,50
                                      ,30,80 };

        public void sSpd_enmTotalView(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmTotalView)).Length;

            //FarPoint.Win.LineBorder ulborder1 = new FarPoint.Win.LineBorder(System.Drawing.Color.Black); 

            FarPoint.Win.ComplexBorder ulborder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false,false);

            FarPoint.Win.ComplexBorder uborder1 = new FarPoint.Win.ComplexBorder(new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.ThinLine, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None, System.Drawing.Color.Black), new FarPoint.Win.ComplexBorderSide(FarPoint.Win.ComplexBorderSideStyle.None), false, false);

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;

            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            spd.ActiveSheet.ColumnHeader.Columns.Get(-1).Border = ulborder1;
            spd.ActiveSheet.Columns.Get(-1).Border = uborder1;            

            //헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.chk, clsSpread.enmSpdType.CheckBox);
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.Diag, clsSpread.enmSpdType.Text);
            sup.setColStyle_Text(spd, -1, (int)enmTotalView.Diag, true, true, false, 1000); //txt재정의
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.OrderName, clsSpread.enmSpdType.Text);
            sup.setColStyle_Text(spd, -1, (int)enmTotalView.OrderName, true, true, false, 1000);
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.RTime, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.Nurse, clsSpread.enmSpdType.Text);



            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmTotalView.Diag, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmTotalView.OrderName, clsSpread.HAlign_L, clsSpread.VAlign_C);

            //히든
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.DrCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.Gubun, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.Change, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmTotalView.ROWID, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmTotalView.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmTotalView.JepName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

        }

        #endregion

        #region //내시경 일반건진 조직검사 수납여부 - frmComSupEndsVIEW05.cs
        public enum enmHicSunapView
        {
            Sunap,Gubun,Ptno,JepDate,SName,HicWRTNO
            ,ExJong,Sex
        };

        public string[] sSpdHicSunapView = {"수납여부","구분","등록번호","검사일자","성명","검진번호"
                                         ,"검사종류","성별" };
        public int[] nSpdHicSunapView = { 60,100,70,80,80,60
                                          ,60,40  };

        public void sSpd_enmHicSunapView(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {
            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmHicSunapView)).Length;

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
            //methodSpd.setColStyle(spd, -1, (int)enmHicSunapView.Diag, clsSpread.enmSpdType.Text);
            

            //정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmHicSunapView.Diag, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //히든
            //methodSpd.setColStyle(spd, -1, (int)enmHicSunapView.DrCode, clsSpread.enmSpdType.Hide);

            // sort, filter            
            //methodSpd.setSpdSort(spd, (int)enmHicSunapView.GbIO, true);
            //methodSpd.setSpdFilter(spd, (int)enmHicSunapView.JepName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;
            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "수납", false);
            //unary.BackColor =  method.cPaleGreen;
            ////unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmHicSunapView.Sunap, unary); //결과

        }

        #endregion

        #region //내시경 가예약 등록   enum, 배열변수 - frmSupEndsRESV01.cs
        /// <summary> 내시경 가예약 등록 enm </summary>        
        public enum enmSupEndsResv
        {
            RDate, RTime, RDateTime, DrName, SName, Pano, EntDate, EntSabun, Remark, ROWID
        }
        //내시경  가예약 등록  컬럼헤드 배열 </summary>
        public string[] sSpdenmSupEndsResv = { "가예약일자","가예약시간","가예약일","의사","성명","등록번호","등록일자","등록자","참고사항","ROWID"
                                                };

        /// <summary> 내시경  가예약 등록  컬럼사이즈 배열 </summary>
        public int[] nSpdenmSupEndsResv = { 80,50,120,60,60,60,80,50,300,80
                                            };


        /// <summary> 내시경  가예약 등록  스프레드 표시   </summary>  
        public void sSpd_enmSupEndsResv(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupEndsResv)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 45;
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
            methodSpd.setColAlign(spd, (int)enmSupEndsResv.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);



            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsResv.RDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsResv.RTime, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsResv.ROWID, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsResv.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsResv.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsResv.Chk, unary); //결과



        }

        #endregion

        #region //내시경 간호기록 내용입력  enum, 배열변수 - FrmComSupEndsSET02.cs

        #region //간호기록 명단 리스트 1

        /// <summary> 내시경 간호기록 명단 리스트 enm </summary>        
        public enum enmSupEndsSet02A
        {
            Ptno,SName,BDate,RDate,DeptCode
            ,Gubun,JDate,STS, ROWID
        }
        
        public string[] sSpdenmSupEndsSet02A = { "등록번호","성명","처방일자","검사일자","과"
                                                ,"접수구분","JDtae","입력","ROWID"};
                
        public int[] nSpdenmSupEndsSet02A = { 70,60,80,80,30
                                              ,40,80,30,50};                        
        public void sSpd_enmSupEndsSet02A(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupEndsSet02A)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 35;
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
            //methodSpd.setColAlign(spd, (int)enmSupEndsSet02A.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);



            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsSet02.RDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsSet02A.JDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsSet02A.ROWID, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsSet02A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsSet02A.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsSet02A.Chk, unary); //결과



        }

        #endregion

        #region //간호기록 개인 명단 리스트 2

        /// <summary> 내시경 간호기록 개인 리스트 enm </summary>        
        public enum enmSupEndsSet02B
        {
            chk,Ptno, Gubun, BDate,RDate
           ,STS, DrName,NrName,EMRNO, ROWID
        }

        public string[] sSpdenmSupEndsSet02B = { " ","등록번호","구분","발생일자","검사일자"
                                                ,"종류","검사자","간호사","EMRNO","ROWID"};

        public int[] nSpdenmSupEndsSet02B = { 25,80,40,80,80
                                              ,200,70,70,50,80 };
        public void sSpd_enmSupEndsSet02B(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupEndsSet02B)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 20;
            //spd.ActiveSheet.RowHeader.Columns.Get(0).Width = 20;

            spd.VerticalScrollBarWidth = 10;
            spd.HorizontalScrollBarHeight = 10;

            //1.헤더 및 사이즈
            methodSpd.setHeader(spd, colName, size);
            spd.ActiveSheet.Columns.Get(-1).Visible = true;

            //2.컬럼 스타일
            methodSpd.setColStyle(spd, -1, -1, clsSpread.enmSpdType.Label);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsSet02B.chk, clsSpread.enmSpdType.CheckBox);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            //methodSpd.setColAlign(spd, (int)enmSupEndsSet02B.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);



            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsSet02B.RDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsSet02B.EMRNO, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsSet02B.ROWID, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsSet02B.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsSet02B.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsSet02B.Chk, unary); //결과



        }

        #endregion


        #endregion

        #region //내시경 주사 및 향정 사용량 입력   enum, 배열변수 - frmComSupEndsVIEW02.cs

        #region  <summary> 내시경 주사 및 향정 사용량 입력 - 환자정보 enm
        public enum enmSupEndsJusa01
        {
            Pano,SName,Sex,OrderCode,OrderName
                ,JDate,RDrCode,RDate,Buse, Orderno,ROWID
        }
        
        public string[] sSpdenmSupEndsJusa01 = { "등록번호","성명","성/Age(호실)","처방코드","처방명칭"
                ,"접수일자","처방의사","예약일자","시행부서","오더넘버", "ROWID"
                                                };
        
        public int[] nSpdenmSupEndsJusa01 = {80,60,90,80,150
                                            ,80,60,80,100,120,80      };
        
        public void sSpd_enmSupEndsJusa01(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupEndsJusa01)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 25;
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
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa01.OrderName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            

            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsResv.RDate, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa01.OrderCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa01.ROWID, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa01.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa01.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa01.Chk, unary); //결과



        }

        #endregion

        #region  <summary> 내시경 주사 및 향정 사용량 입력 - 주사내역 enm
        public enum enmSupEndsJusa02
        {
            CHK,jepsu,STS,SuCode,SuCode2,OrderCode,OrderName,DosCode,Qty,Nal,Remark, ROWID
        }

        public string[] sSpdenmSupEndsJusa02 = {"선택", "접수완료","상태","코드","처방코드","수가코드","처방명","용법"
                                            ,"수량","날수","참고사항","ROWID"
                                                };

        public int[] nSpdenmSupEndsJusa02 = {35, 60,40,55,55,80,210,120
                                            ,30,30,120,80     };

        public void sSpd_enmSupEndsJusa02(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupEndsJusa02)).Length;

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
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.CHK, clsSpread.enmSpdType.CheckBox);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa02.OrderName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa02.DosCode, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa02.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.jepsu, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.SuCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.OrderCode, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.ROWID, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.SuCode2, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa02.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa02.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa02.Chk, unary); //결과



        }

        #endregion

        #region  <summary> 내시경 주사 및 향정 사용량 입력 - 향정오더 enm
        public enum enmSupEndsJusa03
        {
            STS,SuCode,SuName,Use,OrderCnt
                ,Etc,Change, DrugUnit, OrderNo, PRT
                , Del,DrName, ROWID1,ROWID2
        }

        public string[] sSpdenmSupEndsJusa03 = {"구분","코드","명칭","실사용량","처방개수"
                                            ,"기타","수정","약용량","처방번호","바코드"
                                            ,"제외","의사명","ROWID1","ROWID2"
                                                };

        public int[] nSpdenmSupEndsJusa03 = { 35,80,200,40,35
                                            ,150,40,50,60,35
                                            ,35,60,80,80     };

        public void sSpd_enmSupEndsJusa03(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupEndsJusa03)).Length;

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
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.Use, clsSpread.enmSpdType.Text);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.OrderCnt, clsSpread.enmSpdType.Text);            
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.Del, clsSpread.enmSpdType.CheckBox);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.PRT, clsSpread.enmSpdType.Button,null,"출력");


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa03.SuCode, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa03.SuName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            

            //4.히든                                    
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.OrderNo, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.Etc, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.DrugUnit, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.Change, clsSpread.enmSpdType.Hide);            
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.ROWID1, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa03.ROWID2, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa03.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa03.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa03.Chk, unary); //결과



        }

        #endregion
        
        #region  <summary> 내시경 주사 및 향정 사용량 입력 - 사용량입력 enm
        public enum enmSupEndsJusa04
        {
            CHK, SuCode, SuName, Use, OrderCnt
                , DrName, Change, DrugUnit, OrderNo
                ,   ROWID
        }

        public string[] sSpdenmSupEndsJusa04 = {"선택","코드","명칭","실사용량","처방개수"
                                            ,"의사명","수정","약용량","처방번호"
                                            ,"ROWID"
                                                };

        public int[] nSpdenmSupEndsJusa04 = { 35,80,230,40,35
                                            ,60,40,50,60,40
                                            ,80     };

        public void sSpd_enmSupEndsJusa04(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmSupEndsJusa03)).Length;

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
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa04.CHK, clsSpread.enmSpdType.CheckBox);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);            
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa04.SuCode, clsSpread.HAlign_L, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmSupEndsJusa04.SuName, clsSpread.HAlign_L, clsSpread.VAlign_C);
            

            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa04.RDate, clsSpread.enmSpdType.Hide);            
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa04.Change, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa04.DrugUnit, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa04.OrderNo, clsSpread.enmSpdType.Hide);            
            methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa04.ROWID, clsSpread.enmSpdType.Hide);
            

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa03.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa03.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa03.Chk, unary); //결과



        }

        #endregion

        #endregion

        #region 내시경 대장 및 종검 스케쥴 관리

        #region 개인별 리스트 
        public enum enmPanoList { mm, dd, time, DrName, SName, Pano, Remark }

        public string[] sSpdPanoList = { "월", "일", "시간","의사", "성명", "등록번호", "참고사항" };

        public int[] nSpdPanoList = { 20, 20, 35, 50, 50, 70,3200 };
        
        public void sSpd_enmPanoList(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmPanoList)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 25;
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
            methodSpd.setColAlign(spd, (int)enmPanoList.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.RDate, clsSpread.enmSpdType.Hide);
            
            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa03.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            

            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa03.Chk, unary); //결과



        }

        #endregion

        #region 의사별 오전오후 리스트 
        public enum enmDrList2 {DrName,Am,Pm}

        public string[] sSpdDrList2 = { "의사","오전","오후" };
        public int[] nSpdDrList2 = { 70, 100, 100 };

        public void sSpd_enmDrList2(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmDrList2)).Length;

            //spd.ActiveSheet.ColumnHeader.Cells.Get(0, 0, 0, spd.ActiveSheet.ColumnCount - 1).BackColor = Color.LightGray;
            //spd.Skin = FarPoint.Win.Spread.DefaultSpreadSkins.Office2007;


            spd.ActiveSheet.ColumnHeader.Rows[0].Height = 25;
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
            //methodSpd.setColAlign(spd, (int)enmDrList2.Remark, clsSpread.HAlign_L, clsSpread.VAlign_C);


            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmSupEndsJusa02.RDate, clsSpread.enmSpdType.Hide);

            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmSupEndsJusa03.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmSupEndsJusa03.Chk, unary); //결과



        }

        #endregion

        #endregion

        #region //내시경 오더 취소작업   enum, 배열변수 - frmComSupEndsCLE01.cs
        /// <summary> 내시경 오더 취소작업 enm </summary>        
        public enum enmComSupEndsCLE01A
        {
            chk,Pano,SName,JepDate,RDate,WardCode
            ,RoomCode,DeptCode,DrCode,PacsNo,PacsUid
            ,Seqno,ROWID
        }
        
        public string[] sSpdenmComSupEndsCLE01A = { "선택","등록번호","성명","접수일자","예약일자","병동"
                                                    ,"호실","과","의사코드","PacsNo","PacsUid"
                                                    ,"Seqno","ROWID"   };
        
                
        public int[] nSpdenmComSupEndsCLE01A = { 30,60,80,80,80,40
                                                 ,40,30,50,100,100
                                                 ,50,70   };
        
        
        public void sSpd_enmComSupEndsCLE01A(FarPoint.Win.Spread.FpSpread spd, string[] colName, int[] size, int RowCnt, int ColCnt)
        {

            //스프레드 사이즈
            spd.ActiveSheet.RowCount = RowCnt;
            spd.ActiveSheet.ColumnCount = ColCnt;
            if (ColCnt == 0) spd.ActiveSheet.ColumnCount = Enum.GetValues(typeof(enmComSupEndsCLE01A)).Length;

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
            methodSpd.setColStyle(spd, -1, (int)enmComSupEndsCLE01A.chk, clsSpread.enmSpdType.CheckBox);


            //3.정렬
            methodSpd.setColAlign(spd, -1, clsSpread.HAlign_C, clsSpread.VAlign_C);
            methodSpd.setColAlign(spd, (int)enmComSupEndsCLE01A.PacsUid, clsSpread.HAlign_L, clsSpread.VAlign_C);



            //4.히든                                    
            //methodSpd.setColStyle(spd, -1, (int)enmComSupEndsCLE01A.RDate, clsSpread.enmSpdType.Hide);
            //methodSpd.setColStyle(spd, -1, (int)enmComSupEndsCLE01A.RTime, clsSpread.enmSpdType.Hide);
            methodSpd.setColStyle(spd, -1, (int)enmComSupEndsCLE01A.ROWID, clsSpread.enmSpdType.Hide);


            // 5. 필터
            //methodSpd.setSpdFilter(spd, (int)enmComSupEndsCLE01A.SName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);
            //methodSpd.setSpdFilter(spd, (int)enmComSupEndsCLE01A.OrderName, FarPoint.Win.Spread.AutoFilterMode.EnhancedContextMenu, true);


            // 6. 특정문구 색상 
            //FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule unary;

            //unary = new FarPoint.Win.Spread.UnaryComparisonConditionalFormattingRule(FarPoint.Win.Spread.UnaryComparisonOperator.EqualTo, "*", false);
            //unary.BackColor = method.cSpdCellImpact_Back;
            //unary.ForeColor = method.cSpdCellImpact_Fore;
            //spd.ActiveSheet.SetConditionalFormatting(-1, (int)enmComSupEndsCLE01A.Chk, unary); //결과



        }

        #endregion
    }
}
