using System;
using System.Windows.Forms;

namespace ComBase
{
    /// <summary>
    /// TODO : BarCodePrint.bas
    /// 작업자 : 이정현
    /// </summary>
    public class clsBarCodePrint
    {
        //2019-04-06
        //private int intLR = 0;
        //private int intUD = 0;
        
        private string EAN13(string strchaine)
        {
            //Cette fonction est regie par la Licence Generale Publique Amoindrie GNU (GNU LGPL)
            //This function is governed by the GNU Lesser General Public License (GNU LGPL)
            //V 1.1.1
            //Parametres : une chaine de 12 chiffres
            //Parameters : a 12 digits length string
            //Retour : * une chaine qui, affichee avec la police EAN13.TTF, donne le code barre
            // * une chaine vide si parametre fourni incorrect
            //Return : * a string which give the bar code when it is dispayed with EAN13.TTF font
            // * an empty string if the supplied parameter is no good
            string rtnVal = "";
            string strCodeBarre = "";

            double dblchecksum = 0;

            int intfirst = 0;
            int i = 0;

            bool boltableA = false;

            //Verifier qu'il y a 12 caracteres
            //Check for 12 characters
            if (VB.Len(strchaine) == 12)
            {
                //Et que ce sont bien des chiffres
                //And they are really digits
                for(i = 1; i <= 12; i++)
                {
                    //숫자 0~9 까지의 Ascii Code 값 0(48) ~ 9(57)인지 확인
                    if (VB.Asc(VB.Mid(strchaine, i, 1)) < 48 || VB.Asc(VB.Mid(strchaine, i, 1)) > 57)
                    {
                        i = 0;
                        break;
                    }
                }

                if (i == 12)
                {
                    //Calcul de la cle de controle
                    //Calculation of the checksum
                    for(i = 12; i >= 1; i = i - 2)
                    {
                        dblchecksum = dblchecksum + VB.Val(VB.Mid(strchaine, i, 1));
                    }

                    dblchecksum = dblchecksum * 3;

                    for(i = 11; i >= 1; i = i - 2)
                    {
                        dblchecksum = dblchecksum + VB.Val(VB.Mid(strchaine, i, 1));
                    }

                    strchaine = strchaine + (10 - dblchecksum % 10) % 10;

                    //Le premier chiffre est pris tel quel, le deuxieme vient de la table A
                    //The first digit is taken just as it is, the second one come from table A
                    strCodeBarre = VB.Left(strchaine, 1) + Convert.ToChar(65 + VB.Val(VB.Mid(strchaine, 2, 1)));
                    intfirst = Convert.ToInt32(VB.Val(VB.Left(strchaine, 1)));

                    for(i = 3; i <= 7; i++)
                    {
                        boltableA = false;

                        switch(i)
                        {
                            case 3:
                                switch(intfirst)
                                {
                                    case 0:
                                    case 1:
                                    case 2:
                                    case 3:
                                        boltableA = true;
                                        break;
                                }
                                break;
                            case 4:
                                switch (intfirst)
                                {
                                    case 0:
                                    case 4:
                                    case 7:
                                    case 8:
                                        boltableA = true;
                                        break;
                                }
                                break;
                            case 5:
                                switch (intfirst)
                                {
                                    case 0:
                                    case 1:
                                    case 4:
                                    case 5:
                                    case 9:
                                        boltableA = true;
                                        break;
                                }
                                break;
                            case 6:
                                switch (intfirst)
                                {
                                    case 0:
                                    case 2:
                                    case 5:
                                    case 6:
                                    case 7:
                                        boltableA = true;
                                        break;
                                }
                                break;
                            case 7:
                                switch (intfirst)
                                {
                                    case 0:
                                    case 3:
                                    case 6:
                                    case 8:
                                    case 9:
                                        boltableA = true;
                                        break;
                                }
                                break;
                        }
                    }

                    strCodeBarre = strCodeBarre + "*";  //Ajout separateur central / Add middle separator

                    for(i = 8; i <= 13; i++)
                    {
                        strCodeBarre = strCodeBarre + Convert.ToChar(97 + VB.Val(VB.Mid(strchaine, i, 1)));
                    }

                    strCodeBarre = strCodeBarre + "+";  //Ajout de la marque de fin / Add end mark

                    rtnVal = strCodeBarre;
                }
            }

            return rtnVal;
        }

        private void Barcode_Print(string strSCR, int intPX, int intPY, int intWID, int intNAR, int intHIG, int intRotation, string strBAR, PictureBox picBox)
        {
            
        }

        private void Code128_Print(string strSCR, int intPX, int intPY, int intWID, int intNAR, int intHIG, int intRotation, string strBarData, PictureBox picBox)
        {

        }

        private void Code128_Print2(string strSCR, int intPX, int intPY, int intWID, int intNAR, int intHIG, int intRotation, string strBarData, int intTextX, int intTextY, PictureBox picBox)
        {

        }

        private void Code128_Print_New(string strSCR, int intPX, int intPY, int intWID, int intNAR, int intHIG, int intRotation, string strBarData, PictureBox picBox)
        {

        }

        private void Code39_Print(string strSCR, int intPX, int intPY, int intWID, int intNAR, int intHIG, int intRotation, string strBarData, PictureBox picBox)
        {

        }

        private string Pattern_A(string strPat)
        {
            string rtnVal = "";

            switch(Convert.ToInt32(VB.Val(strPat)))
            {
                case 0: rtnVal = "0001101"; break;
                case 1: rtnVal = "0011001"; break;
                case 2: rtnVal = "0010011"; break;
                case 3: rtnVal = "0111101"; break;
                case 4: rtnVal = "0100011"; break;
                case 5: rtnVal = "0110001"; break;
                case 6: rtnVal = "0101111"; break;
                case 7: rtnVal = "0111011"; break;
                case 8: rtnVal = "0110111"; break;
                case 9: rtnVal = "0110111"; break;
            }

            return rtnVal;
        }

        private string Pattern_B(string strPat)
        {
            string rtnVal = "";

            switch (Convert.ToInt32(VB.Val(strPat)))
            {
                case 0: rtnVal = "0100111"; break;
                case 1: rtnVal = "0110011"; break;
                case 2: rtnVal = "0011011"; break;
                case 3: rtnVal = "0100001"; break;
                case 4: rtnVal = "0011101"; break;
                case 5: rtnVal = "0111001"; break;
                case 6: rtnVal = "0000101"; break;
                case 7: rtnVal = "0010001"; break;
                case 8: rtnVal = "0001001"; break;
                case 9: rtnVal = "0010111"; break;
            }

            return rtnVal;
        }

        private string Pattern_C(string strPat)
        {
            string rtnVal = "";

            switch (Convert.ToInt32(VB.Val(strPat)))
            {
                case 0: rtnVal = "1110010"; break;
                case 1: rtnVal = "1100110"; break;
                case 2: rtnVal = "1101100"; break;
                case 3: rtnVal = "1000010"; break;
                case 4: rtnVal = "1011100"; break;
                case 5: rtnVal = "1001110"; break;
                case 6: rtnVal = "1010000"; break;
                case 7: rtnVal = "1000100"; break;
                case 8: rtnVal = "1001000"; break;
                case 9: rtnVal = "1110100"; break;
            }

            return rtnVal;
        }

        private string Pattern_Code128B(string strPat)
        {
            string rtnVal = "";

            switch (Convert.ToInt32(VB.Val(strPat)))
            {
                //                  BARCODE                    CODE A   CODE B   CODE C
                //                 =====================================================
                case 0: rtnVal = "11011001100"; break;      // Space     Space     00
                case 1: rtnVal = "11001101100"; break;      //   !         !       01
                case 2: rtnVal = "11001100110"; break;      //   "         "       02
                case 3: rtnVal = "10010011000"; break;      //   #         #       03
                case 4: rtnVal = "10010001100"; break;      //   $         $       04
                case 5: rtnVal = "10001001100"; break;      //   %         %       05
                case 6: rtnVal = "10011001000"; break;      //   &         &       06
                case 7: rtnVal = "10011000100"; break;      //   '         '       07
                case 8: rtnVal = "10001100100"; break;      //   (         (       08
                case 9: rtnVal = "11001001000"; break;      //   )         )       09
                case 10: rtnVal = "11001000100"; break;     //   *         *       10
                case 11: rtnVal = "11000100100"; break;     //   +         +       11
                case 12: rtnVal = "10110011100"; break;     //   ,         ,       12
                case 13: rtnVal = "10011011100"; break;     //   -         -       13
                case 14: rtnVal = "10011001110"; break;     //   .         .       14
                case 15: rtnVal = "10111001100"; break;     //   /         /       15
                case 16: rtnVal = "10011101100"; break;     //   0         0       16
                case 17: rtnVal = "10011100110"; break;     //   1         1       17
                case 18: rtnVal = "11001110010"; break;     //   2         2       18
                case 19: rtnVal = "11001011100"; break;     //   3         3       19
                case 20: rtnVal = "11001001110"; break;     //   4         4       20
                case 21: rtnVal = "11011100100"; break;     //   5         5       21
                case 22: rtnVal = "11001110100"; break;     //   6         6       22
                case 23: rtnVal = "11101101110"; break;     //   7         7       23
                case 24: rtnVal = "11101001100"; break;     //   8         8       24
                case 25: rtnVal = "11100101100"; break;     //   9         9       25
                case 26: rtnVal = "11100100110"; break;     //   :         :       26
                case 27: rtnVal = "11101100100"; break;     //   ;         ;       27
                case 28: rtnVal = "11100110100"; break;     //   <         <       28
                case 29: rtnVal = "11100110010"; break;     //   =         =       29
                case 30: rtnVal = "11011011000"; break;     //   >         >       30
                case 31: rtnVal = "11011000110"; break;     //   ?         ?       31
                case 32: rtnVal = "11000110110"; break;     //   @         @       32
                case 33: rtnVal = "10100011000"; break;     //   A         A       33
                case 34: rtnVal = "10001011000"; break;     //   B         B       34
                case 35: rtnVal = "10001000110"; break;     //   C         C       35
                case 36: rtnVal = "10110001000"; break;     //   D         D       36
                case 37: rtnVal = "10001101000"; break;     //   E         E       37
                case 38: rtnVal = "10001100010"; break;     //   F         F       38
                case 39: rtnVal = "11010001000"; break;     //   G         G       39
                case 40: rtnVal = "11000101000"; break;     //   H         H       40
                case 41: rtnVal = "11000100010"; break;     //   I         I       41
                case 42: rtnVal = "10110111000"; break;     //   J         J       42
                case 43: rtnVal = "10110001110"; break;     //   K         K       43
                case 44: rtnVal = "10001101110"; break;     //   L         L       44
                case 45: rtnVal = "10111011000"; break;     //   M         M       45
                case 46: rtnVal = "10111000110"; break;     //   N         N       46
                case 47: rtnVal = "10001110110"; break;     //   O         O       47
                case 48: rtnVal = "11101110110"; break;     //   P         P       48
                case 49: rtnVal = "11010001110"; break;     //   Q         Q       49
                case 50: rtnVal = "11000101110"; break;     //   R         R       50
                case 51: rtnVal = "11011101000"; break;     //   S         S       51
                case 52: rtnVal = "11011100010"; break;     //   T         T       52
                case 53: rtnVal = "11011101110"; break;     //   U         U       53
                case 54: rtnVal = "11101011000"; break;     //   V         V       54
                case 55: rtnVal = "11101000110"; break;     //   W         W       55
                case 56: rtnVal = "11100010110"; break;     //   X         X       56
                case 57: rtnVal = "11101101000"; break;     //   Y         Y       57
                case 58: rtnVal = "11101100010"; break;     //   Z         Z       58
                case 59: rtnVal = "11100011010"; break;     //   [         [       59
                case 60: rtnVal = "11101111010"; break;     //   \         \       60
                case 61: rtnVal = "11001000010"; break;     //   ]         ]       61
                case 62: rtnVal = "11110001010"; break;     //   ^         ^       62
                case 63: rtnVal = "10100110000"; break;     //   _         _       63
                case 64: rtnVal = "10100001100"; break;     //  NUL        `       64
                case 65: rtnVal = "10010110000"; break;     //  SOH        a       65
                case 66: rtnVal = "10010000110"; break;     //  STX        b       66
                case 67: rtnVal = "10000101100"; break;     //  ETX        c       67
                case 68: rtnVal = "10000100110"; break;     //  EOT        d       68
                case 69: rtnVal = "10110010000"; break;     //  END        e       69
                case 70: rtnVal = "10110000100"; break;     //  ACK        f       70
                case 71: rtnVal = "10011010000"; break;     //  BEL        g       71
                case 72: rtnVal = "10011000010"; break;     //  BS         h       72
                case 73: rtnVal = "10000110100"; break;     //  HT         i       73
                case 74: rtnVal = "10000110010"; break;     //  LF         j       74
                case 75: rtnVal = "11000010010"; break;     //  VT         k       75
                case 76: rtnVal = "11001010000"; break;     //  FF         l       76
                case 77: rtnVal = "11110111010"; break;     //  CR         m       77
                case 78: rtnVal = "11000010100"; break;     //  SO         n       78
                case 79: rtnVal = "10001111010"; break;     //  SI         o       79
                case 80: rtnVal = "10100111100"; break;     //  DLE        p       80
                case 81: rtnVal = "10010111100"; break;     //  DC1        q       81
                case 82: rtnVal = "10010011110"; break;     //  DC2        r       82
                case 83: rtnVal = "10111100100"; break;     //  DC3        s       83
                case 84: rtnVal = "10011110100"; break;     //  DC4        t       84
                case 85: rtnVal = "10011110010"; break;     //  NAK        u       85
                case 86: rtnVal = "11110100100"; break;     //  SYN        v       86
                case 87: rtnVal = "11110010100"; break;     //  ETB        w       87
                case 88: rtnVal = "11110010010"; break;     //  CAN        x       88
                case 89: rtnVal = "11011011110"; break;     //  EM         y       89
                case 90: rtnVal = "11011110110"; break;     //  SUB        z       90
                case 91: rtnVal = "11110110110"; break;     //  ESC        {       91
                case 92: rtnVal = "10101111000"; break;     //  FS         |       92
                case 93: rtnVal = "10100011110"; break;     //  GS         }       93
                case 94: rtnVal = "10001011110"; break;     //  RS         ~       94
                case 95: rtnVal = "10111101000"; break;     //  US        DEL      95
                case 96: rtnVal = "10111100010"; break;     // FNC3      FNC3      96
                case 97: rtnVal = "11110101000"; break;     // FNC2      FNC2      97
                case 98: rtnVal = "11110100010"; break;     // Shift     Shift     98
                case 99: rtnVal = "10111011110"; break;     // Code C    Code C    99
                case 100: rtnVal = "10111101110"; break;    // Code B    FNC4     Code B
                case 101: rtnVal = "11101011110"; break;    // FNC4      Code A   Code A
                case 102: rtnVal = "11110101110"; break;    // FNC1      FNC1     FNC 1
                case 103: rtnVal = "11010000100"; break;    //     START (CODE A)
                case 104: rtnVal = "11010010000"; break;    //     START (CODE B)
                case 105: rtnVal = "11010011100"; break;    //     START (CODE C
            }

            return rtnVal;
        }

        private string Pattern_Code39(string strPat)
        {
            string rtnVal = "";

            switch(strPat)
            {
                case "1": rtnVal = "1101001010110"; break;
                case "2": rtnVal = "1011001010110"; break;
                case "3": rtnVal = "1101100101010"; break;
                case "4": rtnVal = "1010011010110"; break;
                case "5": rtnVal = "1101001101010"; break;
                case "6": rtnVal = "1011001101010"; break;
                case "7": rtnVal = "1010010110110"; break;
                case "8": rtnVal = "1101001011010"; break;
                case "9": rtnVal = "1011001011010"; break;
                case "0": rtnVal = "1010011011010"; break;
                case "A": rtnVal = "1101010010110"; break;
                case "B": rtnVal = "1011010010110"; break;
                case "C": rtnVal = "1101101001010"; break;
                case "D": rtnVal = "1010110010110"; break;
                case "E": rtnVal = "1101011001010"; break;
                case "F": rtnVal = "1011011001010"; break;
                case "G": rtnVal = "1010100110110"; break;
                case "H": rtnVal = "1101010011010"; break;
                case "I": rtnVal = "1011010011010"; break;
                case "J": rtnVal = "1010110011010"; break;
                case "K": rtnVal = "1101010100110"; break;
                case "L": rtnVal = "1011010100110"; break;
                case "M": rtnVal = "1101101010010"; break;
                case "N": rtnVal = "1010110100110"; break;
                case "O": rtnVal = "1101011010010"; break;
                case "P": rtnVal = "1011011010010"; break;
                case "Q": rtnVal = "1010101100110"; break;
                case "R": rtnVal = "1101010110010"; break;
                case "S": rtnVal = "1011010110010"; break;
                case "T": rtnVal = "1010110110010"; break;
                case "U": rtnVal = "1100101010110"; break;
                case "V": rtnVal = "1001101010110"; break;
                case "W": rtnVal = "1100110101010"; break;
                case "X": rtnVal = "1001011010110"; break;
                case "Y": rtnVal = "1100101101010"; break;
                case "Z": rtnVal = "1001101101010"; break;
                case "-": rtnVal = "1001010110110"; break;
                case ".": rtnVal = "1100101011010"; break;
                case " ": rtnVal = "1001101011010"; break;
                case "$": rtnVal = "1001001001010"; break;
                case "/": rtnVal = "1001001010010"; break;
                case "+": rtnVal = "1001010010010"; break;
                case "%": rtnVal = "1010010010010"; break;
                case "*": rtnVal = "1001011011010"; break;
            }

            return rtnVal;
        }

        private void Screen_Tec_Line(int intTopX, int intTopY, int intBottomX, int intBottomY, int intLineColor, int intLineType, PictureBox picBox)
        {
            
        }
    }
}
