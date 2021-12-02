namespace ComBase
{
    public class clsIorderProcessB
    {
        //public static int GstrStaffListIndex;

        //public static object GstrObjectOp;
        //public static string GstrSelectPRN;

        //public static string GstrGbOrderPRM;
        //public static int GnOpIllsIndex;

        //public static string GstrDept;

        public static string GstrSubRate; // OCS_OrderCode의 SubRate

        //public static string GstrConsultOrd;   // 기타 Consult Order
        //public static string GstrConsultChk;
        //public static string GstrDischargeChk;
        //public static string GstrOutDate;
        //public static string GstrOutDept;
        //public static int GnOpNoteIndex;
        //public static int GnIllIndex;

        //public static string GstrVerbalDrCode;
        //public static string GstrSchedual;
        //public static string GstrPickUp;
        //public static string GstrNoMessage;
        //public static string GstrNoMessage1;
        //public static string GstrOrderTimeChk;

        //public static int GnActiveRow;

        //public static string l_str;  // 한글자르기
        //public static int l_Len;

        //public static string GstrOrderName;
        //public static string GstrOrderCode;
        //public static string GstrBun1;

        //public static string GstrCheckJin;

        //public static string GstrUpInCheck;
        //public static int GnIndex;
        //public static string GstrDeptCode1;
        //public static string GstrDate;
        //public static string GstrDrCode1;
        //public static string GstrDrName1;
        //public static string GstrJinIll;
        //public static string GstrJinName;
        //public static string GstrOpIll;
        //public static string GstrOpIllName;
        //public static string GGbMagam;

        //public static string GstrTFlag;
        //public static string GstrOrdDis;
        //public static string GstrRepeat;

        //public static int GnAnHH;
        //public static int GnAnMi;
        //public static string GstrAnNgt;

        /*
                Function HanGul_Cut_Len(ArgLen As Integer, argstrText As String) As String
                                        'v_str As String,
                    Dim i               As Integer
                    Dim nTotLen         As Integer
                    Dim nlen            As Integer
                    'Dim l_str           As String
                    Dim r_str           As String
                    Dim l_check1        As String
                    Dim l_check2        As String
                    Dim l_ascii1        As Integer
                    Dim l_ascii2        As Integer


                    l_str = RTrim(argstrText)
                    l_Len = LenB(l_str)
                    If l_Len = 0 Then
                        l_str = " "
                        HanGul_Cut_Len = ""
                        Exit Function
                    End If
                    r_str = ""

                    i = 1
                    Do While i <= ArgLen
                        l_check1 = IIf(MidH(l_str, i, 1) = "", " ", MidH(l_str, i, 1))
                        l_check2 = IIf(MidH(l_str, i + 1, 1) = "", " ", MidH(l_str, i + 1, 1))

                        l_ascii1 = AscB(l_check1)
                        l_ascii2 = AscB(l_check2)
                        If l_ascii1 >= 176 And l_ascii1 <= 253 And _
                            l_ascii2 >= 161 And l_ascii2 <= 254 Then
                                r_str = r_str + MidH(l_str, i, 2)
                                i = i + 1
                        ElseIf l_ascii1 >= 32 And l_ascii1 <= 126 Then
                                r_str = r_str + MidH(l_str, i, 1)
                        End If
                        i = i + 1
                    Loop

                    l_str = MidH(l_str, i, l_Len)


                    HanGul_Cut_Len = r_str

                End Function



                Private Static Function InStrTok(ByVal SStr As String, ByVal Div As String) As String
                '*******************************************************************
                '// 주어진 문자열을 Token으로 분리해낸다.
                '//
                '// 최종 수정일 : 1998-11-18
                '*******************************************************************


                    Static LsString                 As String       '// SStr을 저장
                    Static Llindex                  As Long         '// SStr의 Cursor를 저장
                    Static LlLength                 As Long         '// SStr의 길이 저장
                    Dim LlPosition                  As Long


                    LlPosition = 0

                    If SStr<> "0" Then
                        LsString = SStr
                        LlLength = Len(SStr)
                        Llindex = 1
                    End If


                    If Llindex > LlLength Then
                        InStrTok = "-1":           Exit Function
                    End If


                    LlPosition = InStr(Llindex, LsString, Div)
                    If LlPosition = 0 Then
                        InStrTok = MidH(LsString, Llindex)
                        Llindex = LlLength + 1
                    Else
                        InStrTok = MidH(LsString, Llindex, LlPosition - Llindex)
                        Llindex = LlPosition + Len(Div)
                    End If


                End Function


                Public Function ChangeChar(ByVal xStr As String, ByVal xFrom As String, ByVal xTo As String) As String
                '*******************************************************************
                '// xStr내에 있는 xFrom을 xTo로 바꾼다.
                '//
                '// 최종 수정일 : 1998-11-18
                '*******************************************************************

                    Dim LsTemp                  As String
                    Dim LsChanged               As String


                    LsTemp = ""
                    LsChanged = ""

                    If Len(xStr) = 0 Then        '// 문자가 없으면 마침
                        ChangeChar = "":         Exit Function
                    End If


                    LsTemp = InStrTok(xStr, xFrom)
                    If LsTemp = "-1" Then
                        ChangeChar = "":         Exit Function
                    End If
                    While LsTemp<> "-1"         '// xFrom문자를 xTo문자로 바꿈
                            LsChanged = LsChanged & LsTemp & xTo
                            LsTemp = InStrTok(0, xFrom)
                    Wend

                    '// xStr의 끝 문자가 xTo와 같으면 그냥 반환
                    '// 다르면 가장끝의 xTo는 제거
                    If Right$(xStr, Len(xFrom)) = xFrom Then
                        ChangeChar = LsChanged
                    Else
                        ChangeChar = LeftH(LsChanged, Len(LsChanged) - Len(xTo))
                    End If


                End Function


                Function DrName_Get(ArgDrCode As String) As String

                    DrName_Get = ""

                    SQL = " SELECT DrName FROM " + ComNum.DB_MED + "OCS_DOCTOR "
                    SQL = SQL & " WHERE Sabun = '" & Trim(ArgDrCode) & "' "

                    result = AdoOpenSet(rs2, SQL)


                    If RowIndicator > 0 Then
                        DrName_Get = Trim(AdoGetString(rs2, "DrName", 0))
                    Else
                        DrName_Get = ""
                    End If


                    rs2.Close: Set rs2 = Nothing


                End Function
                Function DrSabun_Get(ArgDrCode As String) As String

                    DrSabun_Get = ""

                    SQL = " SELECT Sabun FROM " + ComNum.DB_MED + "OCS_DOCTOR "
                    SQL = SQL & " WHERE DrCode = '" & Trim(ArgDrCode) & "' "

                    result = AdoOpenSet(rs2, SQL)


                    If RowIndicator > 0 Then
                        DrSabun_Get = Trim(AdoGetString(rs2, "Sabun", 0))
                    Else
                        DrSabun_Get = ""
                    End If


                    rs2.Close: Set rs2 = Nothing


                End Function


                Function SlipNo_Gubun(ArgSlipNo As String, ArgDoscode As String, argBun As String) As String

                    Select Case Trim(ArgSlipNo)
                        Case "0003", "0004":   SlipNo_Gubun = "Med      999"
                        Case "0005"
                                    If Trim(ArgDoscode) = "97" Or Trim(ArgDoscode) = "99" Then
                                        SlipNo_Gubun = "Med      23"
                                    Else
                                        SlipNo_Gubun = "Med      24"
                                    End If
                        Case "0010" To "0042": SlipNo_Gubun = "Lab      17"
                        Case "0060" To "0065", "0067", "0069" To "0080":
                                                SlipNo_Gubun = "Xray     14"
                        Case "0066":           SlipNo_Gubun = "RI       15"
                        Case "0068":           SlipNo_Gubun = "Sono     16"
                        Case "A1":             SlipNo_Gubun = "V/S      11"
                        Case "A2":             SlipNo_Gubun = "S/O      12"
                        Case "A4":             SlipNo_Gubun = "S/O      13"
                        Case "OR1", "OR2":     SlipNo_Gubun = "OR       22"
                        Case "0044":
                                    If Trim(argBun) = "78" Then
                                        SlipNo_Gubun = "Bmd      19"
                                    Else
                                        SlipNo_Gubun = "Endo     18"
                                    End If
                        Case "TEL":            SlipNo_Gubun = "TEL      21"
                        Case "0106":           SlipNo_Gubun = "JAGA     25"
                        Case Else:             SlipNo_Gubun = "Etc      20"
                    End Select


                End Function



                Public Sub Order_Print_A4()

                    Dim i, j, n, K                  As Integer
                    Dim strOrderCodeP               As String * 6
                    Dim strOrderNameP               As String * 54
                    Dim strContentsP                As String * 6
                    Dim strQtyP                     As String * 4
                    Dim strNalP                     As String * 2
                    Dim strDivP                     As String * 5
                    Dim strImivP                    As String * 20
                    Dim strGroupP                   As String * 5
                    Dim strErP                      As String * 4
                    Dim strJinDateP                 As String
                    Dim strPDateP                   As String
                    Dim strSNameP                   As String * 10
                    Dim strPtnoP                    As String * 8
                    Dim strSexP                     As String * 1
                    Dim strAgeP                     As String * 6
                    Dim strMonthP                   As String
                    Dim strRoomP                    As String * 4
                    Dim strDeptP                    As String * 4
                    Dim strRemarkP                  As String * 60
                    Dim strOrderP                   As String
                    Dim strIllCode                  As String * 8
                    Dim strIllName                  As String * 50
                    Dim strTFlagP                   As String
                    Dim strDateP                    As String
                    Dim strSlipNoP                  As String
                    Dim strGbStatusP                As String
                    Dim strROP                      As String


                    On Error Resume Next


                    strSql = " SELECT  Jumin1 FROM " + ComNum.DB_PMPA + "BAS_PATIENT WHERE Pano = '" & Trim(Pat.PtNo) & "' "

                    result = AdoOpenSet(rs2, strSql)
                    strMonthP = ""
                    If RowIndicator = 1 And Pat.Age< 3 And Trim(AdoGetString(rs2, "Jumin1", 0)) <> "" Then strMonthP = AGE_MONTH_GESAN(AdoGetString(rs2, "Jumin1", 0), GstrBDate) & "개월"
                    rs2.Close: Set rs2 = Nothing


                    strDateP = GstrSysDate

                    GoSub Print_Title

                    n = 0
                    With FrmOrders.SSOrder


                    If GnGbOrderSave = 2 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 16: strSlipNoP = Trim(.Text)
                            .Col = 34: strOrderP = Trim(.Text)
                            .Col = 33: strGbStatusP = Trim(.Text)
                            .Col = 31
                            If Not (Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or Pat.INDATE = GstrBDate Then
                                .Row = K: .Col = 1
                                If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                    If n >= 31 Then
                                        Printer.Print Space(85) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                        Printer.NewPage
                                        GoSub Print_Title: n = 0
                                    End If

                                    .Col = 2: strOrderCodeP = Trim(.Text)
                                    .Col = 3: strOrderNameP = Trim(.Text)
                                    .Col = 5
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                    End If
                                    .Col = 6
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                    End If
                                    .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 15
                                    .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 4: strImivP = Trim(.Text)
                                    .Col = 12: strTFlagP = ""
                                    If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                    If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                    .Col = 13
                                    If Trim(.Text) = "M" Then strTFlagP = " [M]"

                                    .Col = 10
                                    If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                    .Col = 1
                                    If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                    .Col = 21
                                    strRemarkP = ""
                                    strRemarkP = Trim(.Text)

                                    'Printer.Print Space(1) & strErP & strOrderCodeP & strOrderNameP & _
                                                                strContentsP & strQtyP & " " & strImivP & _
                                                                strGroupP & strDivP & strNalP & strTFlagP
                                    '손동현 위를 아래로 바꾼다.
                                    Printer.Print Space(1) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 6) & RPadH(strOrderNameP, 54) & _
                                                                RPadH(strContentsP, 6) & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 20) & _
                                                                RPadH(strGroupP, 5) & RPadH(strDivP, 5) & RPadH(strNalP, 2) & strTFlagP

                                    n = n + 1
                                    If Trim(strRemarkP) <> "" Then
                                        If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                            Printer.Print Space(9) & "* " & strRemarkP: n = n + 1
                                        End If
                                    End If


                                    Printer.Print
                                End If
                            End If
                        Next K
                    ElseIf GnGbOrderSave = 3 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 43
                            If Trim(.Text) = "" Then
                                .Col = 16: strSlipNoP = Trim(.Text)
                                .Col = 34: strOrderP = Trim(.Text)
                                .Col = 33: strGbStatusP = Trim(.Text)
                                .Col = 31
                                If Not(Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or Pat.INDATE = GstrBDate Then
                                    .Row = K: .Col = 1

                                    If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                        If n >= 31 Then
                                            Printer.Print Space(85) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                            Printer.NewPage
                                            GoSub Print_Title: n = 0
                                        End If

                                        .Col = 2: strOrderCodeP = Trim(.Text)
                                        .Col = 3: strOrderNameP = Trim(.Text)
                                        .Col = 5
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                        End If
                                        .Col = 6
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                        End If
                                        .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 15
                                        .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 4: strImivP = Trim(.Text)

                                        .Col = 12: strTFlagP = ""
                                        If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                        If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                        .Col = 13:
                                        If Trim(.Text) = "M" Then strTFlagP = " [M]"
                                        .Col = 10
                                        If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                        .Col = 1
                                        If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                        .Col = 21
                                        strRemarkP = ""
                                        strRemarkP = Trim(.Text)

                                        'Printer.Print Space(1) & strErP & strOrderCodeP & strOrderNameP & _
                                        '                         strContentsP & strQtyP & " " & strImivP & _
                                        '                         strGroupP & strDivP & strNalP & strTFlagP
                                        '손동현 위를 아래로 바꾼다.
                                        Printer.Print Space(1) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 6) & RPadH(strOrderNameP, 54) & _
                                                                    RPadH(strContentsP, 6) & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 20) & _
                                                                    RPadH(strGroupP, 5) & RPadH(strDivP, 5) & RPadH(strNalP, 2) & strTFlagP
                                        n = n + 1
                                        If Trim(strRemarkP) <> "" Then
                                            If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                                Printer.Print Space(9) & "* " & strRemarkP: n = n + 1
                                            End If
                                        End If


                                        Printer.Print
                                    End If
                                End If
                            End If
                        Next K
                    End If

                        Printer.Print Space(85) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                    End With


                    Printer.EndDoc

                    Exit Sub

                '/-------------------------------------------------------------------------------------------

                Print_Title:

                    strPtnoP = Pat.PtNo:       strSNameP = Pat.sName:  strSexP = CStr(Pat.Sex)
                    strAgeP = CStr(Pat.Age) & "세"
                    If Trim(strMonthP) <> "" Then strAgeP = strMonthP
                    strRoomP = Pat.RoomCode:   strDeptP = Pat.DeptCode
                    strJinDateP = GstrBDate: strPDateP = GstrSysDate

                    Printer.Print
                    Printer.Print
                    Printer.Print
                    Printer.Print

                    Printer.FontName = "굴림체"
                    Printer.Font.Size = 8
                    Printer.Print Space(55) & "POHANG SAINT MARY'S HOSPITAL"
                    Printer.Print
                    Printer.Font.Size = 14
                    Printer.Print Space(31) & "PHYSICIAN'S ORDERS"
                    Printer.Font.Size = 8
                    Printer.FontBold = True
                    Printer.Print Space(52) & "--------------------------------"
                    Printer.FontBold = False
                    Printer.Font.Size = 12
                    If Trim(GstrRepeat) = "R" Then
                        Printer.Print Space(2) & "진료일자: " & strJinDateP & " 부터" & Space(30) & "출력일자: " & strDateP
                    Else
                        Printer.Print Space(2) & "진료일자: " & strJinDateP & Space(35) & "출력일자: " & strDateP
                    End If
                    Printer.Print Space(2) & "------------------------------------------------------------------------------------------"
                    Printer.Print Space(2) & "  Name: " & strSNameP & "    Hosp.No.: " & strPtnoP & "    Sex: " & strSexP & _
                                                "   Age: " & strAgeP & " Room: " & strRoomP & "  Dept: " & strDeptP
                    Printer.Print Space(2) & "------------------------------------------------------------------------------------------"
                    Printer.Print Space(19) & " 처방명 " & Space(25) & " 용량" & " 수량" & Space(1) & "용법/경로/검체" & _
                                                Space(2) & "MIX " & "횟수" & Space(1) & "일수"
                    Printer.Print Space(2) & "------------------------------------------------------------------------------------------"
                    'Printer.Print
                    Printer.Font.Size = 10

                    j = 0
                    For i = 1 To FrmOrders.SSIlls.DataRowCnt
                        FrmOrders.SSIlls.Row = i: FrmOrders.SSIlls.Col = 5
                        If FrmOrders.SSIlls.BackColor = RGB(0, 0, 255) Then
                            FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                            FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                            FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                            Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                            j = 1
                            Exit For
                        End If
                    Next i


                    If j = 0 Then
                        FrmOrders.SSIlls.Row = 1
                        FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                        FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                        FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                        Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                    End If

                    Printer.Print


                    Return

                End Sub
                Public Sub Order_Print_B5()

                    'A4지와 B5지의 중간 사이즈로 맞춘다.......  포항성모병원......
                    'A4지로 바꾸고 싶으면 언제라도 Order_Print_A4를 Order_Print로 이름을 바꾸면 된다.
                    '손동현  2000/05/17


                    Dim i, j, n, K                  As Integer
                    Dim strOrderCodeP               As String * 5 '* 6
                    Dim strOrderNameP               As String * 50 '* 54
                    Dim strContentsP                As String * 6
                    Dim strQtyP                     As String * 3 '* 4
                    Dim strNalP                     As String * 2
                    Dim strDivP                     As String * 4 '* 5
                    Dim strImivP                    As String * 20
                    Dim strGroupP                   As String * 5
                    Dim strErP                      As String * 4
                    Dim strJinDateP                 As String
                    Dim strPDateP                   As String
                    Dim strSNameP                   As String * 10
                    Dim strPtnoP                    As String * 8
                    Dim strSexP                     As String * 1
                    Dim strAgeP                     As String * 6
                    Dim strMonthP                   As String
                    Dim strRoomP                    As String * 4
                    Dim strDeptP                    As String * 4
                    Dim strRemarkP                  As String * 60
                    Dim strOrderP                   As String
                    Dim strIllCode                  As String * 8
                    Dim strIllName                  As String * 50
                    Dim strTFlagP                   As String
                    Dim strDateP                    As String
                    Dim strSlipNoP                  As String
                    Dim strGbStatusP                As String
                    Dim strROP                      As String


                    On Error Resume Next


                    strSql = " SELECT  Jumin1 FROM " + ComNum.DB_PMPA + "BAS_PATIENT WHERE Pano = '" & Trim(Pat.PtNo) & "' "

                    result = AdoOpenSet(rs2, strSql)
                    strMonthP = ""
                    If RowIndicator = 1 And Pat.Age< 3 And Trim(AdoGetString(rs2, "Jumin1", 0)) <> "" Then strMonthP = AGE_MONTH_GESAN(AdoGetString(rs2, "Jumin1", 0), GstrBDate) & "개월"
                    rs2.Close: Set rs2 = Nothing


                    strDateP = GstrSysDate

                    GoSub Print_Title

                    n = 0
                    With FrmOrders.SSOrder


                    If GnGbOrderSave = 2 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 16: strSlipNoP = Trim(.Text)
                            .Col = 34: strOrderP = Trim(.Text)
                            .Col = 33: strGbStatusP = Trim(.Text)
                            .Col = 31
                            If Not (Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or Pat.INDATE = GstrBDate Then
                                .Row = K: .Col = 1
                                If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                    If n >= 31 Then
                                        Printer.Print Space(85) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                        Printer.NewPage
                                        GoSub Print_Title: n = 0
                                    End If

                                    .Col = 2: strOrderCodeP = Trim(.Text)
                                    .Col = 3: strOrderNameP = Trim(.Text)
                                    .Col = 5
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                    End If
                                    .Col = 6
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                    End If
                                    .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 15
                                    .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 4: strImivP = Trim(.Text)
                                    .Col = 12: strTFlagP = ""
                                    If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                    If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                    .Col = 13
                                    If Trim(.Text) = "M" Then strTFlagP = " [M]"

                                    .Col = 10
                                    If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                    .Col = 1
                                    If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                    .Col = 21
                                    strRemarkP = ""
                                    strRemarkP = Trim(.Text)


                                    Printer.Print Space(8) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 50) & _
                                                                RPadH(strContentsP, 6) & RPadH(strQtyP, 3) & " " & RPadH(strImivP, 20) & _
                                                                RPadH(strGroupP, 5) & RPadH(strDivP, 4) & RPadH(strNalP, 2) & strTFlagP

                                    n = n + 1
                                    If Trim(strRemarkP) <> "" Then
                                        If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                            Printer.Print Space(15) & "* " & strRemarkP: n = n + 1
                                        End If
                                    End If


                                    Printer.Print
                                End If
                            End If
                        Next K
                    ElseIf GnGbOrderSave = 3 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 43
                            If Trim(.Text) = "" Then
                                .Col = 16: strSlipNoP = Trim(.Text)
                                .Col = 34: strOrderP = Trim(.Text)
                                .Col = 33: strGbStatusP = Trim(.Text)
                                .Col = 31
                                If Not(Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or Pat.INDATE = GstrBDate Then
                                    .Row = K: .Col = 1

                                    If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                        If n >= 31 Then
                                            Printer.Print Space(85) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                            Printer.NewPage
                                            GoSub Print_Title: n = 0
                                        End If

                                        .Col = 2: strOrderCodeP = Trim(.Text)
                                        .Col = 3: strOrderNameP = Trim(.Text)
                                        .Col = 5
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                        End If
                                        .Col = 6
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                        End If
                                        .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 15
                                        .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 4: strImivP = Trim(.Text)

                                        .Col = 12: strTFlagP = ""
                                        If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                        If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                        .Col = 13:
                                        If Trim(.Text) = "M" Then strTFlagP = " [M]"
                                        .Col = 10
                                        If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                        .Col = 1
                                        If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                        .Col = 21
                                        strRemarkP = ""
                                        strRemarkP = Trim(.Text)




                                        Printer.Print Space(8) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 50) & _
                                                                    RPadH(strContentsP, 6) & RPadH(strQtyP, 3) & " " & RPadH(strImivP, 20) & _
                                                                    RPadH(strGroupP, 5) & RPadH(strDivP, 4) & RPadH(strNalP, 2) & strTFlagP
                                        n = n + 1
                                        If Trim(strRemarkP) <> "" Then
                                            If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                                Printer.Print Space(15) & "* " & strRemarkP: n = n + 1
                                            End If
                                        End If


                                        Printer.Print
                                    End If
                                End If
                            End If
                        Next K
                    End If

                        Printer.Print Space(85) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                    End With


                    Printer.EndDoc

                    Exit Sub

                '/-------------------------------------------------------------------------------------------

                Print_Title:

                    strPtnoP = Pat.PtNo:       strSNameP = Pat.sName:  strSexP = CStr(Pat.Sex)
                    strAgeP = CStr(Pat.Age) & "세"
                    If Trim(strMonthP) <> "" Then strAgeP = strMonthP
                    strRoomP = Pat.RoomCode:   strDeptP = Pat.DeptCode
                    strJinDateP = GstrBDate: strPDateP = GstrSysDate

                    Printer.Print
                    Printer.Print
                    Printer.Print
                    Printer.Print

                    Printer.FontName = "굴림체"
                    Printer.Font.Size = 8
                    Printer.Print Space(55) & "POHANG SAINT MARY'S HOSPITAL"
                    Printer.Print
                    Printer.Font.Size = 14
                    Printer.Print Space(31) & "PHYSICIAN'S ORDERS"
                    Printer.Font.Size = 8
                    Printer.FontBold = True
                    Printer.Print Space(52) & "--------------------------------"
                    Printer.FontBold = False
                    Printer.Font.Size = 11
                    If Trim(GstrRepeat) = "R" Then
                        Printer.Print Space(5) & "진료일자: " & strJinDateP & " 부터" & Space(30) & "출력일자: " & strDateP
                    Else
                        Printer.Print Space(5) & "진료일자: " & strJinDateP & Space(48) & "출력일자: " & strDateP
                    End If
                    Printer.Print Space(5) & "------------------------------------------------------------------------------------------"
                    Printer.Print Space(5) & "  Name: " & strSNameP & "    Hosp.No.: " & strPtnoP & "    Sex: " & strSexP & _
                                                "   Age: " & strAgeP & " Room: " & strRoomP & "  Dept: " & strDeptP
                    Printer.Print Space(5) & "------------------------------------------------------------------------------------------"

                    Printer.Font.Size = 10
                    Printer.Print Space(25) & " 처방명 " & Space(26) & " 용량" & " 수량" & Space(1) & "용법/경로/검체" & _
                                                Space(2) & "MIX " & "횟수" & Space(1) & "일수"
                    Printer.Font.Size = 11
                    Printer.Print Space(5) & "------------------------------------------------------------------------------------------"
                    'Printer.Print
                    Printer.Font.Size = 9

                    j = 0
                    For i = 1 To FrmOrders.SSIlls.DataRowCnt
                        FrmOrders.SSIlls.Row = i: FrmOrders.SSIlls.Col = 5
                        If FrmOrders.SSIlls.BackColor = RGB(0, 0, 255) Then
                            FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                            FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                            FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                            Printer.Print Space(12) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                            j = 1
                            Exit For
                        End If
                    Next i


                    If j = 0 Then
                        FrmOrders.SSIlls.Row = 1
                        FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                        FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                        FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                        Printer.Print Space(12) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                    End If

                    Printer.Print


                    Return

                End Sub

                Public Sub Order_Print()

                    'Order_Print_A4 / Order_Print_B5 / Order_Print
                    'Order_Print는  오더지 최종본임 포항성모병원
                    '손동현  2000/06/02
                    '전산과장님 수정요망


                    Dim i, j, K                     As Integer
                    Dim n                           As Double
                    Dim ii                          As Integer
                    Dim strOrderCodeP               As String * 5 '* 6
                    Dim strOrderNameP               As String * 54
                    Dim strContentsP                As String * 6
                    Dim strQtyP                     As String * 4
                    Dim strNalP                     As String * 2
                    Dim strDivP                     As String * 4 '* 5
                    Dim strImivP                    As String * 20 '용법
                    Dim strGroupP                   As String * 4 '* 5
                    Dim strErP                      As String * 4
                    Dim strJinDateP                 As String
                    Dim strPDateP                   As String
                    Dim strSNameP                   As String * 10
                    Dim strPtnoP                    As String * 8
                    Dim strSexP                     As String * 1
                    Dim strAgeP                     As String * 6
                    Dim strMonthP                   As String
                    Dim strRoomP                    As String * 4
                    Dim strDeptP                    As String * 4
                    Dim strRemarkP                  As String * 60
                    Dim strOrderP                   As String
                    Dim strIllCode                  As String * 8
                    Dim strIllName                  As String * 50
                    Dim strTFlagP                   As String
                    Dim strDateP                    As String
                    Dim strSlipNoP                  As String
                    Dim strGbStatusP                As String
                    Dim strROP                      As String


                    On Error Resume Next


                    strSql = " SELECT  Jumin1 FROM " + ComNum.DB_PMPA + "BAS_PATIENT WHERE Pano = '" & Trim(Pat.PtNo) & "' "

                    result = AdoOpenSet(rs2, strSql)
                    strMonthP = ""
                    If RowIndicator = 1 And Pat.Age< 3 And Trim(AdoGetString(rs2, "Jumin1", 0)) <> "" Then strMonthP = AGE_MONTH_GESAN(AdoGetString(rs2, "Jumin1", 0), GstrBDate) & "개월"
                    rs2.Close: Set rs2 = Nothing



                    If Trim(Pat.DeptCode) = "NP" Then
                        strDateP = GstrBDate
                    Else
                        strDateP = GstrSysDate
                    End If


                    GoSub Print_Title


                    n = 0
                    With FrmOrders.SSOrder


                    If GnGbOrderSave = 2 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 16: strSlipNoP = Trim(.Text)
                            .Col = 34: strOrderP = Trim(.Text)
                            .Col = 33: strGbStatusP = Trim(.Text)
                            .Col = 31
                            'If Not (Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or PAT.InDate = GstrBDate Then
                                .Row = K: .Col = 1
                                If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                    If n >= 31 Then
                                        Printer.Print Space(75) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                        Printer.NewPage
                                        GoSub Print_Title: n = 0
                                    End If

                                    .Col = 2: strOrderCodeP = Trim(.Text)

                                    'If (Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                    '    .Col = 3: strOrderNameP = Trim(.Text)
                                    'Else
                                    '    .Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                    'End If

                                    '위(5줄)는 오더코드를 출력하지 않으며 아래(1줄)는 수가 코드를 같이 출력한다. 손동현

                                    .Col = 3: strOrderNameP = Trim(.Text)


                                    .Col = 5
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                    End If
                                    .Col = 6
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                    End If
                                    .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 15
                                    .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 4: strImivP = Trim(.Text)
                                    .Col = 12: strTFlagP = ""
                                    If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                    If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                    .Col = 13
                                    If Trim(.Text) = "M" Then strTFlagP = " [M]"

                                    .Col = 10
                                    If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                    .Col = 1
                                    If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                    .Col = 21
                                    strRemarkP = ""
                                    strRemarkP = Trim(.Text)


                                    Printer.Print Space(5) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 54) & _
                                                                RPadH(strContentsP, 6) & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 20) & _
                                                                "    " & RPadH(strGroupP, 4) & RPadH(strDivP, 4) & RPadH(strNalP, 2) & strTFlagP

                                    n = n + 1
                                    If Trim(strRemarkP) <> "" Then
                                        If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                            Printer.Print Space(13) & "* " & strRemarkP: n = n + 0.5 'n = n + 1
                                        End If
                                    End If
                                    Printer.Print Space(5) & "   ---------------------------------------------------------------------------------------------------------------"
                                End If
                            'End If
                        Next K
                    ElseIf GnGbOrderSave = 3 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 43
                            If Trim(.Text) = "" Then
                                .Col = 16: strSlipNoP = Trim(.Text)
                                .Col = 34: strOrderP = Trim(.Text)
                                .Col = 33: strGbStatusP = Trim(.Text)
                                .Col = 31
                                'If Not (Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or PAT.InDate = GstrBDate Then
                                    .Row = K: .Col = 1
                                    If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                        If n >= 31 Then
                                            Printer.Print Space(75) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                            Printer.NewPage
                                            GoSub Print_Title: n = 0
                                        End If

                                        .Col = 2: strOrderCodeP = Trim(.Text)

                                        'If (Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                        '    .Col = 3: strOrderNameP = Trim(.Text)
                                        'Else
                                        '    .Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                        'End If

                                        '위(5줄)는 수가코드를 출력하지 않으며 아래(1줄)는 수가 코드를 같이 출력한다. 손동현

                                        .Col = 3: strOrderNameP = Trim(.Text)


                                        .Col = 5
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                        End If
                                        .Col = 6
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                        End If
                                        .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 15
                                        .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 4: strImivP = Trim(.Text)

                                        .Col = 12: strTFlagP = ""
                                        If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                        If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                        .Col = 13:
                                        If Trim(.Text) = "M" Then strTFlagP = " [M]"
                                        .Col = 10
                                        If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                        .Col = 1
                                        If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                        .Col = 21
                                        strRemarkP = ""
                                        strRemarkP = Trim(.Text)


                                        Printer.Print Space(5) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 54) & _
                                                                    RPadH(strContentsP, 6) & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 20) & _
                                                                    "    " & RPadH(strGroupP, 4) & RPadH(strDivP, 4) & RPadH(strNalP, 2) & strTFlagP
                                        n = n + 1
                                        If Trim(strRemarkP) <> "" Then
                                            If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                                Printer.Print Space(13) & "* " & strRemarkP: n = n + 0.5 'n = n + 1
                                            End If
                                        End If
                                        Printer.Print Space(5) & "   ---------------------------------------------------------------------------------------------------------------"
                                    End If
                                End If
                            'End If
                        Next K
                    End If


                        Printer.Print Space(75) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                    End With


                    If n< 30 And n <> 0 Then
                        For ii = n To 30 Step 1
                            Printer.Print Space(5) & ""
                            Printer.Print Space(5) & "   ---------------------------------------------------------------------------------------------------------------"
                        Next
                    End If

                    Printer.EndDoc


                    Exit Sub


                '/-------------------------------------------------------------------------------------------

                Print_Title:



                    strPtnoP = Pat.PtNo:       strSNameP = Pat.sName:  strSexP = CStr(Pat.Sex)
                    strAgeP = CStr(Pat.Age) & "세"
                    If Trim(strMonthP) <> "" Then strAgeP = strMonthP
                    strRoomP = Pat.RoomCode:   strDeptP = Pat.DeptCode
                    strJinDateP = GstrBDate:   strPDateP = GstrSysDate

                    Printer.Print
                    Printer.Print
                    Printer.Print
                    Printer.Print

                    Printer.FontName = "굴림체"
                    Printer.Font.Size = 8
                    Printer.Print Space(55) & "POHANG SAINT MARY'S HOSPITAL"
                    Printer.Print
                    Printer.Font.Size = 14
                    Printer.Print Space(31) & "PHYSICIAN'S ORDERS"
                    Printer.Font.Size = 8
                    Printer.FontBold = True
                    Printer.Print Space(52) & "--------------------------------"
                    Printer.FontBold = False

                    Printer.Font.Size = 11

                    If GnGbOrderSave = 2 Then
                        If Trim(GstrRepeat) = "R" Then
                            'strDateP = "2008-12-30 "
                            Printer.Print Space(5) & "진료일자: " & strJinDateP & " 부터" & Space(40) & "출력일자: " & strDateP
                        Else
                            'strDateP = "2008-12-30 "
                            Printer.Print Space(5) & "진료일자: " & strJinDateP & Space(48) & "출력일자: " & strDateP
                        End If
                    ElseIf GnGbOrderSave = 3 Then '추가오더
                        If Trim(GstrRepeat) = "R" Then
                            Printer.Print Space(5) & "진료일자: " & strJinDateP & " 부터" & Space(34) & "출력일자: " & Trim(strDateP) & "(추가처방)"
                        Else
                            Printer.Print Space(5) & "진료일자: " & strJinDateP & Space(42) & "출력일자: " & Trim(strDateP) & "(추가처방)"
                        End If
                    End If


                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "========================================================================================================="
                    Printer.Font.Size = 11
                    Printer.Print Space(3) & "  Name: " & RPadH(strSNameP, 14) & " Hosp.No.: " & strPtnoP & "     Sex: " & strSexP & _
                                                "   Age: " & strAgeP & " Room: " & strRoomP & "  Dept: " & strDeptP
                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "---------------------------------------------------------------------------------------------------------"

                    Printer.Font.Size = 9                                                              '용법/경로/검체
                    Printer.Print Space(18) & "처방명 " & Space(40) & " 일용량" & " 일투량" & Space(1) & "용법/경로/검체" & _
                                                Space(8) & "MIX " & "횟수" & Space(1) & "일수"
                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "========================================================================================================="
                    'Printer.Print
                    Printer.Font.Size = 10

                    j = 0
                    For i = 1 To FrmOrders.SSIlls.DataRowCnt
                        FrmOrders.SSIlls.Row = i: FrmOrders.SSIlls.Col = 5
                        If FrmOrders.SSIlls.BackColor = RGB(0, 0, 255) Then
                            FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                            FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                            FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                            Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                            j = 1
                            Exit For
                        End If
                    Next i


                    If j = 0 Then
                        FrmOrders.SSIlls.Row = 1
                        FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                        FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                        FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                        Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                    End If

                    Printer.Print


                    Printer.Font.Size = 9
                    Printer.Print Space(5) & "   ---------------------------------------------------------------------------------------------------------------"

                    Return



                End Sub
                Public Sub Order_Print_20051219()

                    '간호사 SIGN 추가
                    'Order_Print_A4 / Order_Print_B5 / Order_Print
                    'Order_Print는  오더지 최종본임 포항성모병원
                    '손동현  2000/06/02
                    '전산과장님 수정요망


                    Dim i, j, K                     As Integer
                    Dim n                           As Double
                    Dim ii                          As Integer
                    Dim strOrderCodeP               As String * 5 '* 6
                    Dim strOrderNameP               As String * 54
                    Dim strContentsP                As String * 6
                    Dim strQtyP                     As String * 4
                    Dim strNalP                     As String * 2
                    Dim strDivP                     As String * 4 '* 5
                    Dim strImivP                    As String * 20 '용법
                    Dim strGroupP                   As String * 4 '* 5
                    Dim strErP                      As String * 4
                    Dim strJinDateP                 As String
                    Dim strPDateP                   As String
                    Dim strSNameP                   As String * 10
                    Dim strPtnoP                    As String * 8
                    Dim strSexP                     As String * 1
                    Dim strAgeP                     As String * 6
                    Dim strMonthP                   As String
                    Dim strRoomP                    As String * 4
                    Dim strDeptP                    As String * 4
                    Dim strRemarkP                  As String * 60
                    Dim strOrderP                   As String
                    Dim strIllCode                  As String * 8
                    Dim strIllName                  As String * 50
                    Dim strTFlagP                   As String
                    Dim strDateP                    As String
                    Dim strSlipNoP                  As String
                    Dim strGbStatusP                As String
                    Dim strROP                      As String


                    Dim strUnit                     As String



                    On Error Resume Next


                    strSql = " SELECT  Jumin1 FROM " + ComNum.DB_PMPA + "BAS_PATIENT WHERE Pano = '" & Trim(Pat.PtNo) & "' "

                    result = AdoOpenSet(rs2, strSql)
                    strMonthP = ""
                    If RowIndicator = 1 And Pat.Age< 3 And Trim(AdoGetString(rs2, "Jumin1", 0)) <> "" Then strMonthP = AGE_MONTH_GESAN(AdoGetString(rs2, "Jumin1", 0), GstrBDate) & "개월"
                    rs2.Close: Set rs2 = Nothing
                    Call READ_SYSDATE
                    strDateP = GstrSysDate & " " & GstrSysTime



                    GoSub Print_Title

                    n = 0
                    With FrmOrders.SSOrder


                    If GnGbOrderSave = 2 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 16: strSlipNoP = Trim(.Text)
                            .Col = 34: strOrderP = Trim(.Text)


                            'UNIT READ
                            strUnit = ""
                            If strSlipNoP = "0003" Or strSlipNoP = "0004" Or strSlipNoP = "0005" Then
                                SQL = " SELECT DISPHEADER FROM " + ComNum.DB_MED + "OCS_ORDERCODE "
                                SQL = SQL & " WHERE ORDERCODE = '" & strOrderP & "' "
                                result = AdoOpenSet(rs3, SQL)


                                strUnit = AdoGetString(rs3, "DISPHEADER", 0)


                            End If
                            .Col = 33: strGbStatusP = Trim(.Text)
                            .Col = 31
                            'If Not (Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or PAT.InDate = GstrBDate Then
                                .Row = K: .Col = 1
                                If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                    If n >= 31 Then
                                        Printer.Print Space(75) & "주치의 Sign: " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                        GoSub Line_SET
                                        GoSub Print_Title: n = 0
                                    End If

                                    .Col = 2: strOrderCodeP = Trim(.Text)

                                    'If (Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                    '    .Col = 3: strOrderNameP = Trim(.Text)
                                    'Else
                                    '    .Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                    'End If

                                    '위(5줄)는 오더코드를 출력하지 않으며 아래(1줄)는 수가 코드를 같이 출력한다. 손동현

                                    .Col = 3: strOrderNameP = Trim(.Text)


                                    .Col = 5
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                    End If



                                    If Val(strContentsP) > 0 Then strContentsP = Trim(strContentsP) & strUnit


                                    .Col = 6
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                    End If
                                    .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 15
                                    .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 4: strImivP = Trim(.Text)
                                    .Col = 12: strTFlagP = ""
                                    If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                    If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                    .Col = 13
                                    If Trim(.Text) = "M" Then strTFlagP = " [M]"

                                    .Col = 10
                                    If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                    .Col = 1
                                    If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                    .Col = 21
                                    strRemarkP = ""
                                    strRemarkP = Trim(.Text)

                                    'Printer.Print Space(5) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 35) & " " & RPadH(strContentsP, 8) & " " & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 16) & _
                                                                "    " & RPadH(strGroupP, 3) & RPadH(strDivP, 3) & RPadH(strNalP, 2) & strTFlagP

                                    Printer.Print Space(5) & RPadH(strErP, 4) & RPadH(strOrderNameP, 40) & " " & RPadH(strContentsP, 8) & " " & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 16) & _
                                                                "  " & RPadH(strGroupP, 3) & RPadH(strDivP, 3) & RPadH(strNalP, 2) & Trim(strTFlagP)
                                    'Printer.Print Space(5) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 52)
                                    '
                                    'n = n + 0.5
                                    '
                                    ' Printer.Print Space(50) & RPadH(strContentsP, 8) & " " & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 16) & _
                                                                "    " & RPadH(strGroupP, 3) & RPadH(strDivP, 3) & RPadH(strNalP, 2) & strTFlagP


                                    n = n + 1
                                    If Trim(strRemarkP) <> "" Then
                                        If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                            Printer.Print Space(13) & "* " & strRemarkP: n = n + 0.5 'n = n + 1
                                        End If
                                    End If
                                    Printer.Print Space(5) & "   -----------------------------------------------------------------------------------------------------------------------------"
                                End If
                            'End If
                        Next K
                    ElseIf GnGbOrderSave = 3 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 43
                            If Trim(.Text) = "" Then
                                .Col = 16: strSlipNoP = Trim(.Text)
                                .Col = 34: strOrderP = Trim(.Text)

                                            'UNIT READ
                                strUnit = ""
                                If strSlipNoP = "0003" Or strSlipNoP = "0004" Or strSlipNoP = "0005" Then
                                    SQL = " SELECT DISPHEADER FROM " + ComNum.DB_MED + "OCS_ORDERCODE "
                                    SQL = SQL & " WHERE ORDERCODE = '" & strOrderP & "' "
                                    result = AdoOpenSet(rs3, SQL)


                                    strUnit = AdoGetString(rs3, "DISPHEADER", 0)


                                End If


                                .Col = 33: strGbStatusP = Trim(.Text)
                                .Col = 31
                                'If Not (Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or PAT.InDate = GstrBDate Then
                                    .Row = K: .Col = 1
                                    If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                        If n >= 31 Then
                                            Printer.Print Space(75) & "주치의 Sign: " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                            GoSub Line_SET


                                            GoSub Print_Title: n = 0
                                        End If

                                        .Col = 2: strOrderCodeP = Trim(.Text)

                                        'If (Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                        '    .Col = 3: strOrderNameP = Trim(.Text)
                                        'Else
                                        '    .Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                        'End If

                                        '위(5줄)는 수가코드를 출력하지 않으며 아래(1줄)는 수가 코드를 같이 출력한다. 손동현

                                        .Col = 3: strOrderNameP = Trim(.Text)


                                        .Col = 5
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                        End If


                                        If Val(strContentsP) > 0 Then strContentsP = Trim(strContentsP) & strUnit

                                        .Col = 6
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                        End If
                                        .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 15
                                        .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 4: strImivP = Trim(.Text)

                                        .Col = 12: strTFlagP = ""
                                        If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                        If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                        .Col = 13:
                                        If Trim(.Text) = "M" Then strTFlagP = " [M]"
                                        .Col = 10
                                        If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                        .Col = 1
                                        If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                        .Col = 21
                                        strRemarkP = ""
                                        strRemarkP = Trim(.Text)

                                        'Printer.Print Space(5) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 52) & _
                                        '                         RPadH(strContentsP, 8) & " " & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 16) & _
                                        '                         "    " & RPadH(strGroupP, 3) & RPadH(strDivP, 3) & RPadH(strNalP, 2) & strTFlagP

                                        Printer.Print Space(5) & RPadH(strErP, 4) & RPadH(strOrderNameP, 40) & " " & RPadH(strContentsP, 8) & " " & RPadH(strQtyP, 4) & " " & RPadH(strImivP, 16) & _
                                                                "  " & RPadH(strGroupP, 2) & RPadH(strDivP, 2) & RPadH(strNalP, 2) & Trim(strTFlagP)
                                        n = n + 1
                                        If Trim(strRemarkP) <> "" Then
                                            If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                                Printer.Print Space(13) & "* " & strRemarkP: n = n + 0.5 'n = n + 1
                                            End If
                                        End If
                                        Printer.Print Space(5) & "   -----------------------------------------------------------------------------------------------------------------------------"
                                    End If
                                End If
                            'End If
                        Next K
                    End If


                        Printer.Print Space(65) & "주치의 Sign : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                    End With


                    If n< 30 And n <> 0 Then
                        For ii = n To 30 Step 1
                            Printer.Print Space(5) & ""
                            Printer.Print Space(5) & "   -----------------------------------------------------------------------------------------------------------------------------"
                        Next
                    End If


                    GoSub Line_SET

                    Exit Sub


                Line_SET:
                    Dim nBottom As Double

                        'Printer.Font.Size = 8

                    'Printer.Font.Size = 9
                    Printer.DrawWidth = 4
                    'Printer.DrawStyle = 0

                    nBottom = Printer.CurrentY - 60


                    nBottom = 12900

                    'Printer.Line (9600, 2190)-(9600, 13000)
                    Printer.FontBold = True

                    Printer.Line(7600, 2380)-(7600, nBottom)
                    'Printer.Line (8900, 2380)-(8900, nBottom)


                    Printer.Line (8000, 2700)-(8000, nBottom)


                    Printer.Line (8400, 2700)-(8400, nBottom)

                    Printer.Line (8800, 2700)-(8800, nBottom)

                    Printer.Line (9200, 2700)-(9200, nBottom)

                    Printer.Line (9600, 2700)-(9600, nBottom)

                    Printer.Line (10000, 2700)-(10000, nBottom)

                    Printer.Line (10400, 2700)-(10400, nBottom)

                    Printer.Line (10850, 2700)-(10850, nBottom)


                    Printer.FontBold = False
                    Printer.Font.Size = 8

                    Printer.EndDoc
                    Return

                '/-------------------------------------------------------------------------------------------

                Print_Title:



                    strPtnoP = Pat.PtNo:       strSNameP = Pat.sName:  strSexP = CStr(Pat.Sex)
                    strAgeP = CStr(Pat.Age) & "세"
                    If Trim(strMonthP) <> "" Then strAgeP = strMonthP
                    strRoomP = Pat.RoomCode:   strDeptP = Pat.DeptCode
                    strJinDateP = GstrBDate: strPDateP = GstrSysDate

                    Printer.CurrentX = 1
                    Printer.CurrentY = 1
                    Printer.Print
                    Printer.Print
                    Printer.Print
                    Printer.Print

                    Printer.FontName = "굴림체"
                    Printer.Font.Size = 8
                    Printer.Print Space(55) & "Pohang Saint Mary'S Hospital"
                    Printer.Print
                    Printer.Font.Size = 14
                    Printer.Print Space(31) & "PHYSICIAN'S ORDERS"
                    Printer.Font.Size = 8
                    Printer.FontBold = True
                    Printer.Print Space(52) & "---------------------------------"
                    Printer.FontBold = False

                    Printer.Font.Size = 11

                    If GnGbOrderSave = 2 Then
                        If Trim(GstrRepeat) = "R" Then
                            Printer.Print Space(5) & "처방일자: " & strJinDateP & " 부터" & Space(35) & "출력일자: " & strDateP
                        Else
                            Printer.Print Space(5) & "처방일자: " & strJinDateP & Space(43) & "출력일자: " & strDateP
                        End If
                    ElseIf GnGbOrderSave = 3 Then '추가오더
                        If Trim(GstrRepeat) = "R" Then
                            Printer.Print Space(5) & "처방일자: " & strJinDateP & " 부터" & Space(29) & "출력일자: " & Trim(strDateP) & "(추가처방)"
                        Else
                            Printer.Print Space(5) & "처방일자: " & strJinDateP & Space(37) & "출력일자: " & Trim(strDateP) & "(추가처방)"
                        End If
                    End If



                    j = 0
                    For i = 1 To FrmOrders.SSIlls.DataRowCnt
                        FrmOrders.SSIlls.Row = i: FrmOrders.SSIlls.Col = 5
                        If FrmOrders.SSIlls.BackColor = RGB(0, 0, 255) Then
                            FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                            FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                            FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                            Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                            j = 1
                            Exit For
                        End If
                    Next i


                    If j = 0 Then
                        FrmOrders.SSIlls.Row = 1
                        FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                        FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                        FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                        Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                    End If

                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "========================================================================================================="
                    Printer.Font.Size = 11
                    Printer.Print Space(3) & "  Name: " & RPadH(strSNameP, 14) & " Hosp.No.: " & strPtnoP & "     Sex: " & strSexP & _
                                                "   Age: " & strAgeP & " Room: " & strRoomP & "  Dept: " & strDeptP
                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "---------------------------------------------------------------------------------------------------------"

                    Printer.Font.Size = 8                                                              '용법/경로/검체
                    Printer.Print Space(18) & "처방명 " & Space(25) & " 일용량" & " 일투량" & Space(1) & "용법/경로/검체" & _
                                                Space(1) & "MIX " & "횟수" & Space(1) & "일수" & Space(15) & "Time & Nr`Sign"
                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "========================================================================================================="
                    'Printer.Print
                    Printer.Font.Size = 10




                    'Printer.Print
                    'Printer.CurrentX = 50
                    'Printer.CurrentY = 1
                    'Printer.Font.Size = 9
                    'Printer.Print Space(5) & "   ---------------------------------------------------------------------------------------------------------------"
                    Printer.Font.Size = 8
                    Return



                End Sub

                Public Sub Order_Print_OLD_0602()

                    'Order_Print_A4 / Order_Print_B5 / Order_Print
                    'Order_Print는  오더지 최종본임 포항성모병원
                    '손동현  2000/05/23


                    Dim i, j, n, K                  As Integer
                    Dim strOrderCodeP               As String * 5 '* 6
                    Dim strOrderNameP               As String * 50 '* 54
                    Dim strContentsP                As String * 6
                    Dim strQtyP                     As String * 3 '* 4
                    Dim strNalP                     As String * 2
                    Dim strDivP                     As String * 4 '* 5
                    Dim strImivP                    As String * 15 '* 20 '용법
                    Dim strGroupP As String* 3 '* 5
                    Dim strErP                      As String * 4
                    Dim strJinDateP                 As String
                    Dim strPDateP                   As String
                    Dim strSNameP                   As String * 10
                    Dim strPtnoP                    As String * 8
                    Dim strSexP                     As String * 1
                    Dim strAgeP                     As String * 6
                    Dim strMonthP                   As String
                    Dim strRoomP                    As String * 4
                    Dim strDeptP                    As String * 4
                    Dim strRemarkP                  As String * 60
                    Dim strOrderP                   As String
                    Dim strIllCode                  As String * 8
                    Dim strIllName                  As String * 50
                    Dim strTFlagP                   As String
                    Dim strDateP                    As String
                    Dim strSlipNoP                  As String
                    Dim strGbStatusP                As String
                    Dim strROP                      As String


                    On Error Resume Next


                    strSql = " SELECT  Jumin1 FROM " + ComNum.DB_PMPA + "BAS_PATIENT WHERE Pano = '" & Trim(Pat.PtNo) & "' "

                    result = AdoOpenSet(rs2, strSql)
                    strMonthP = ""
                    If RowIndicator = 1 And Pat.Age< 3 And Trim(AdoGetString(rs2, "Jumin1", 0)) <> "" Then strMonthP = AGE_MONTH_GESAN(AdoGetString(rs2, "Jumin1", 0), GstrBDate) & "개월"
                    rs2.Close: Set rs2 = Nothing
                    Call READ_SYSDATE
                    strDateP = GstrSysDate & " " & GstrSysTime

                    GoSub Print_Title

                    n = 0
                    With FrmOrders.SSOrder


                    If GnGbOrderSave = 2 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 16: strSlipNoP = Trim(.Text)
                            .Col = 34: strOrderP = Trim(.Text)
                            .Col = 33: strGbStatusP = Trim(.Text)
                            .Col = 31
                            If Not (Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or Pat.INDATE = GstrBDate Then
                                .Row = K: .Col = 1
                                If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                    'If n >= 31 Then
                                    If n >= 29 Then
                                        Printer.Print Space(75) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                        GoSub Print_Line
                                        Printer.NewPage
                                        GoSub Print_Title: n = 0
                                    End If

                                    .Col = 2: strOrderCodeP = Trim(.Text)


                                    If (Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                        .Col = 3: strOrderNameP = Trim(.Text)
                                    Else
                                        .Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                    End If

                                    '.Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                    .Col = 5
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                    End If
                                    .Col = 6
                                    If InStr(Trim(.Text), ".") = 0 Then
                                        .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    Else
                                        .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                    End If
                                    .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 15
                                    .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                    .Col = 4: strImivP = Trim(.Text)
                                    .Col = 12: strTFlagP = ""
                                    If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                    If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                    .Col = 13
                                    If Trim(.Text) = "M" Then strTFlagP = " [M]"

                                    .Col = 10
                                    If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                    .Col = 1
                                    If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                    .Col = 21
                                    strRemarkP = ""
                                    strRemarkP = Trim(.Text)


                                    GoSub DosAge_Read


                                    Printer.Print Space(3) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 50) & _
                                                                RPadH(strContentsP, 6) & RPadH(strQtyP, 3) & " " & RPadH(strImivP, 15) & _
                                                                "  " & RPadH(strGroupP, 3) & RPadH(strDivP, 4) & RPadH(strNalP, 2) & strTFlagP

                                    n = n + 1
                                    If Trim(strRemarkP) <> "" Then
                                        If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                            Printer.Print Space(11) & "* " & strRemarkP: n = n + 1
                                        End If
                                    End If
                                    Printer.Print Space(3) & "   -----------------------------------------------------------------------------------------------------------------"
                                End If
                            End If
                        Next K
                    ElseIf GnGbOrderSave = 3 Then
                        For K = 1 To.DataRowCnt
                            .Row = K
                            .Col = 43
                            If Trim(.Text) = "" Then
                                .Col = 16: strSlipNoP = Trim(.Text)
                                .Col = 34: strOrderP = Trim(.Text)
                                .Col = 33: strGbStatusP = Trim(.Text)
                                .Col = 31
                                If Not(Trim(strOrderP) >= "V001" And Trim(strOrderP) <= "V004") Or Trim(.Text) = "M" Or Pat.INDATE = GstrBDate Then
                                    .Row = K: .Col = 1

                                    If Trim(.Text) <> "1" Or(GnReadOrder >= K) Then
                                        'If n >= 31 Then
                                        If n >= 29 Then
                                            Printer.Print Space(75) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                                            Printer.NewPage
                                            GoSub Print_Line
                                            GoSub Print_Title: n = 0
                                        End If

                                        .Col = 2: strOrderCodeP = Trim(.Text)


                                        If (Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                            .Col = 3: strOrderNameP = Trim(.Text)
                                        Else
                                            .Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                        End If


                                        '.Col = 3: strOrderNameP = MidH(Trim(.Text), 9)
                                        .Col = 5
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 5: strContentsP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 5: strContentsP = Format(Val(Trim(.Text)), "###0.##")
                                        End If
                                        .Col = 6
                                        If InStr(Trim(.Text), ".") = 0 Then
                                            .Col = 6: strQtyP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        Else
                                            .Col = 6: strQtyP = Format(Val(Trim(.Text)), "##0.##")
                                        End If
                                        .Col = 8: strNalP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 15
                                        .Col = 9: strGroupP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 7: strDivP = IIf(Trim(.Text) = "0", " ", Trim(.Text))
                                        .Col = 4: strImivP = Trim(.Text)

                                        .Col = 12: strTFlagP = ""
                                        If Trim(.Text) = "T" Then strTFlagP = " [T]"
                                        If Trim(.Text) = "P" Then strTFlagP = " [P]"
                                        .Col = 13:
                                        If Trim(.Text) = "M" Then strTFlagP = " [M]"
                                        .Col = 10
                                        If Trim(.Text) = "E" Then strErP = "응)" Else strErP = "   "

                                        .Col = 1
                                        If Trim(.Text) = "1" And(GnReadOrder >= K) Then strErP = "DC)" Else strErP = strErP

                                        .Col = 21
                                        strRemarkP = ""
                                        strRemarkP = Trim(.Text)


                                        GoSub DosAge_Read



                                        Printer.Print Space(3) & RPadH(strErP, 4) & RPadH(strOrderCodeP, 5) & RPadH(strOrderNameP, 50) & _
                                                                    RPadH(strContentsP, 6) & RPadH(strQtyP, 3) & " " & RPadH(strImivP, 15) & _
                                                                    "  " & RPadH(strGroupP, 3) & RPadH(strDivP, 4) & RPadH(strNalP, 2) & strTFlagP
                                        n = n + 1
                                        If Trim(strRemarkP) <> "" Then
                                            If Not(Trim(strSlipNoP) >= "A1" And Trim(strSlipNoP) <= "A4") Then
                                                Printer.Print Space(11) & "* " & strRemarkP: n = n + 1
                                            End If
                                        End If
                                        Printer.Print Space(3) & "   -----------------------------------------------------------------------------------------------------------------"
                                    End If
                                End If
                            End If
                        Next K
                    End If


                        Printer.Print Space(75) & "주치의 : " & GstrDrName   '과장 : " & GstrStaffName & "     "
                    End With




                    GoSub Print_Line


                    Printer.EndDoc

                    Exit Sub

                '/-------------------------------------------------------------------------------------------
                Print_Line:


                    Printer.Font.Size = 11
                    Printer.Print Space(78) & "Night    "
                    Printer.Font.Size = 4
                    Printer.Print
                    Printer.Font.Size = 11
                    Printer.Print Space(78) & "Day      "
                    Printer.Font.Size = 4
                    Printer.Print
                    Printer.Font.Size = 11
                    Printer.Print Space(78) & "Evening  "
                    Printer.Font.Size = 9

                    '세로줄 (가로/세로)
                    Printer.DrawWidth = 2
                    Printer.Line (9400, 1800)-(9500, 14700)
                    Printer.Line (9900, 2200)-(10000, 14700)
                    Printer.Line (10400, 2200)-(10500, 14700)


                    Return

                '/-------------------------------------------------------------------------------------------

                Print_Title:


                    strPtnoP = Pat.PtNo:       strSNameP = Pat.sName:  strSexP = CStr(Pat.Sex)
                    strAgeP = CStr(Pat.Age) & "세"
                    If Trim(strMonthP) <> "" Then strAgeP = strMonthP
                    strRoomP = Pat.RoomCode:   strDeptP = Pat.DeptCode
                    strJinDateP = GstrBDate: strPDateP = GstrSysDate

                    Printer.Print
                    Printer.Print
                    Printer.Print
                    Printer.Print

                    Printer.FontName = "굴림체"
                    Printer.Font.Size = 8
                    Printer.Print Space(55) & "POHANG SAINT MARY'S HOSPITAL"
                    Printer.Print
                    Printer.Font.Size = 14
                    Printer.Print Space(31) & "PHYSICIAN'S ORDERS"
                    Printer.Font.Size = 8
                    Printer.FontBold = True
                    Printer.Print Space(52) & "--------------------------------"
                    Printer.FontBold = False

                    Printer.Font.Size = 11
                    If Trim(GstrRepeat) = "R" Then
                        Printer.Print Space(5) & "진료일자: " & strJinDateP & " 부터" & Space(40) & "출력일자: " & strDateP
                    Else
                        Printer.Print Space(5) & "진료일자: " & strJinDateP & Space(48) & "출력일자: " & strDateP
                    End If
                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "========================================================================================================"
                    Printer.Print Space(3) & "  Name: " & strSNameP & " Hosp.No.: " & strPtnoP & "   Sex: " & strSexP & _
                                                "   Age: " & strAgeP & " Room: " & strRoomP & " Dept: " & strDeptP & "     Nurse`s Note"
                    Printer.Print Space(3) & "--------------------------------------------------------------------------------------------------------"

                    Printer.Font.Size = 9                                                              '용법/경로/검체
                    Printer.Print Space(18) & " 처방명 " & Space(35) & " 용량" & " 수량" & Space(1) & "용법/검체" & _
                                                Space(5) & "MIX " & "횟수" & Space(1) & "일수"
                    Printer.Font.Size = 10
                    Printer.Print Space(3) & "========================================================================================================"
                    'Printer.Print
                    Printer.Font.Size = 10

                    j = 0
                    For i = 1 To FrmOrders.SSIlls.DataRowCnt
                        FrmOrders.SSIlls.Row = i: FrmOrders.SSIlls.Col = 5
                        If FrmOrders.SSIlls.BackColor = RGB(0, 0, 255) Then
                            FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                            FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                            FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                            Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                            j = 1
                            Exit For
                        End If
                    Next i


                    If j = 0 Then
                        FrmOrders.SSIlls.Row = 1
                        FrmOrders.SSIlls.Col = 2: strROP = Trim(FrmOrders.SSIlls.Text)
                        FrmOrders.SSIlls.Col = 3: strIllCode = FrmOrders.SSIlls.Text
                        FrmOrders.SSIlls.Col = 4: strIllName = FrmOrders.SSIlls.Text
                        Printer.Print Space(5) & "상병명 : " & strROP & "  " & strIllCode & strIllName
                    End If

                    Printer.Print


                    Printer.Font.Size = 9
                    Printer.Print Space(3) & "   -----------------------------------------------------------------------------------------------------------------"

                    Return



                '==============================================================================

                '손동현 추가
                '용법란을 줄이면서 용법이 다 보여지지 않아서 용법을 아래와 같이 줄여서 보여준다.....

                DosAge_Read:

                    Dim strDosAge1      As String
                    Dim strDosAge2      As String


                    FrmOrders.SSclsOrdFunction.Col = 27
                    If Trim(FrmOrders.SSclsOrdFunction.Text) = "1" Then

                        FrmOrders.SSclsOrdFunction.Col = 19

                        strDosAge1 = ""
                        strSql = " SELECT DosFullCode FROM " + ComNum.DB_MED + "OCS_ODOSAGE "
                        SQL " WHERE DosCode = '" & Left(Trim(FrmOrders.SSclsOrdFunction.Text), 2) & "0000" & "'"
                        result = AdoOpenSet(rs2, strSql)
                        If RowIndicator > 0 Then
                            strDosAge1 = Trim(AdoGetString(rs2, "DosFullCode", 0))
                        End If
                        AdoCloseSet rs2


                        strDosAge2 = ""
                        strSql = " SELECT DosFullCode FROM " + ComNum.DB_MED + "OCS_ODOSAGE "
                        SQL " WHERE DosCode = '" & Trim(FrmOrders.SSclsOrdFunction.Text) & "'"
                        result = AdoOpenSet(rs2, strSql)
                        If RowIndicator > 0 Then
                            strDosAge2 = Trim(AdoGetString(rs2, "DosFullCode", 0))
                        End If
                        AdoCloseSet rs2


                        strImivP = strDosAge1 & "/" & strDosAge2


                    End If



                    Return


                End Sub



                Function Data_Send(ByVal argPTNO, ByVal ArgSName, ByVal ArgSex, ByVal ArgAge, ByVal ArgBi, ByVal ArgGbSpc, ByVal ArgWardCode, ByVal ArgEntDate) As String

                ' 병동 OCS이외의 Program에서 처리시 주위사항 : " + ComNum.DB_MED + "OCS_IORDER의 GbSend에는 '*' Setting할 것
                '   public static  Argment GstrActDate, PAT.Sname, PAT.Sex, PAT.Age

                    Dim i                   As Integer
                    Dim j                   As Integer
                    Dim nCNT                As Integer
                    Dim nLab                As Integer
                    Dim strOK               As String
                    Dim strPtNo             As String * 8
                    Dim strSName            As String
                    Dim strSex              As String
                    Dim strBi               As String
                    Dim nAge                As Integer
                    Dim strBun              As String * 2
                    Dim strItemCd           As String * 8


                    GoSub Arg_Move


                    If strOK = "OK" Then GoSub Ipd_Ills_Insert

                    If strOK<> "OK" Then GoSub Error_Message

                    Data_Send = strOK


                    Exit Function


                '/--------------------------------------------------------------------------------------------------/

                Arg_Move:

                    strPtNo = argPTNO:      strSName = ArgSName
                    strSex = ArgSex:        nAge = ArgAge:          strBi = ArgBi
                    strOK = "OK"


                    Return


                '/--------------------------------------------------------------------------------------------------/

                Error_Message:

                    Select Case Trim(strOK)
                        Case "RECOVER":         GstrMsgList = "처방 전송 Flag Error, 전산실 연락 요망"
                    End Select


                    strOK = "NO"
                    adoConnect.RollbackTrans

                    MsgBox GstrMsgList, MB_ICONSTOP, "처방입력작업이 취소됩니다"

                    Return



                '/--------------------------------------------------------------------------------------------------/

                Ipd_Ills_Insert:
                    Dim strAills(5)     As String


                    For i = 0 To 5
                        strAills(i) = ""
                    Next i

                    SQL = " SELECT   IllCode  "
                    SQL = SQL & "   FROM " + ComNum.DB_MED + "OCS_IILLS "
                    SQL = SQL & "  WHERE Ptno    = '" & Trim(strPtNo) & "' "
                    SQL = SQL & "    AND EntDate = TO_DATE('" & ArgEntDate & "','YYYY-MM-DD') "

                    result = AdoOpenSet(Rs, SQL)


                    If RowIndicator > 6 Then RowIndicator = 6


                    j = RowIndicator - 1
                    For i = 0 To j
                        strAills(i) = Trim(AdoGetString(Rs, "IllCode", i))
                    Next i


                    SQL = " UPDATE " + ComNum.DB_PMPA + "IPD_TRANS Set"
                    SQL = SQL & "  IllCode1 = '" & strAills(0) & "', "
                    SQL = SQL & "  IllCode2 = '" & strAills(1) & "', "
                    SQL = SQL & "  IllCode3 = '" & strAills(2) & "', "
                    SQL = SQL & "  IllCode4 = '" & strAills(3) & "', "
                    SQL = SQL & "  IllCode5 = '" & strAills(4) & "', "
                    SQL = SQL & "  IllCode6 = '" & strAills(5) & "'  "
                    SQL = SQL & "  WHERE Pano    = '" & Trim(strPtNo) & "' "
                    'SQL = SQL & "    AND IPDNO = '" & GnIpdNO_OCS & "' "
                    'SQL = SQL & "    AND TRSNO ='" & GnTrsNO_OCS & "' "
                    SQL = SQL & "    AND  TRSNO IN ("
                    SQL = SQL & "         SELECT TRSNO FROM " + ComNum.DB_PMPA + "IPD_NEW_MASTER "
                    SQL = SQL & "          WHERE Pano    = '" & Trim(strPtNo) & "'"
                    SQL = SQL & "            AND GBSTS IN ('0','2')  "
                    SQL = SQL & "            AND OUTDATE IS NULL) "
                    'SQL = SQL & "    AND Amset1  = '0'  "
                    'SQL = SQL & "    AND Amset6 != '*'  )"

                    If result<> -1 Then result = AdoExecute(SQL)


                    If result = -1 Then
                        strOK = "TWIPD"
                        adoConnect.RollbackTrans
                    End If


                    Return



                End Function

        */



    }
}
