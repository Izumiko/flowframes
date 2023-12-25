namespace Flowframes.Utilities
{
    class ColorDataUtils
    {
        public static int GetColorPrimaries(string s) // Defined by the "Color primaries" section of ISO/IEC 23091-4/ITU-T H.273
        {
            s = s.Trim().ToLowerInvariant();
            if (s == "bt709") return 1;
            if (s == "bt470m") return 4;
            if (s == "bt470bg") return 5;
            if (s == "bt601") return 6;
            if (s == "smpte240m") return 7;
            if (s == "film") return 8;
            if (s == "bt2020") return 9;
            if (s == "smpte428") return 10;
            if (s == "smpte431") return 11;
            if (s == "smpte432") return 12;
            return 2; // Fallback: 2 = Unspecified
        }

        public static int GetColorTransfer(string s) // Defined by the "Transfer characteristics" section of ISO/IEC 23091-4/ITU-T H.273
        {
            s = s.Trim().ToLowerInvariant();
            if (s == "bt709") return 1;
            if (s == "gamma22" || s == "bt470m") return 4;
            if (s == "gamma28" || s == "bt470bg") return 5; // BT.470 System B, G (historical)
            if (s == "bt601" || s == "smpte170m") return 6; // BT.601
            if (s == "smpte240m") return 7; // SMPTE 240 M
            if (s == "linear") return 8; // Linear
            //if (s == "?") return 9; // Logarithmic(100 : 1 range)
            //if (s == "?") return 10; // Logarithmic (100 * Sqrt(10) : 1 range)
            if (s == "iec61966-2-4") return 11; // IEC 61966-2-4
            if (s == "bt1361" || s == "bt1361e") return 12; // BT.1361
            if (s == "srgb") return 13; // SRGB
            if (s == "bt2020-10") return 14; // BT.2020 10-bit systems
            if (s == "bt2020-12") return 15; // BT.2020 12-bit systems
            if (s == "smpte2084") return 16; // SMPTE ST 2084, ITU BT.2100 PQ
            if (s == "smpte428") return 17; // SMPTE ST 428
            if (s == "bt2100") return 18; // BT.2100 HLG, ARIB STD-B67
            return 2; // Fallback: 2 = Unspecified
        }

        public static int GetMatrixCoeffs(string s) // Defined by the "Matrix coefficients" section of ISO/IEC 23091-4/ITU-T H.27
        {
            s = s.Trim().ToLowerInvariant();
            if (s == "bt709") return 1;
            if (s == "fcc") return 4; // US FCC 73.628
            if (s == "bt470bg") return 5; // BT.470 System B, G (historical)
            if (s == "bt601" || s == "smpte170m") return 6; // BT.601
            if (s == "smpte240m") return 7; // SMPTE 240 M
            if (s == "ycgco") return 8; // YCgCo
            if (s == "bt2020ncl" || s == "bt2020nc") return 9; // BT.2020 non-constant luminance, BT.2100 YCbCr
            if (s == "bt2020") return 10; // BT.2020 constant luminance
            if (s == "smpte2085") return 11; // SMPTE ST 2085 YDzDx
            // 12: MC_CHROMAT_NCL - Chromaticity-derived non-constant luminance
            // 13: MC_CHROMAT_CL - Chromaticity-derived constant luminance
            // 14: MC_ICTCP BT.2100 - ICtCp
            return 2; // Fallback: 2 = Unspecified
        }

        public static int GetColorRange(string s) // Defined by the "Matrix coefficients" section of ISO/IEC 23091-4/ITU-T H.27
        {
            s = s.Trim().ToLowerInvariant();
            if (s == "tv") return 1; // TV
            if (s == "pc") return 2; // PC/Full
            return 0; // Fallback: Unspecified
        }

        public static string FormatForAom(string colorspace)
        {
            return colorspace.Replace("bt2020-10", "bt2020-10bit").Replace("bt2020-12", "bt2020-12bit");
        }

        #region Get string from int

        public static string GetColorPrimariesString(int n)
        {
            return n switch
            {
                1 => "bt709",
                4 => "bt470m",
                5 => "bt470bg",
                6 => "bt601",
                7 => "smpte240m",
                8 => "film",
                9 => "bt2020",
                10 => "smpte428",
                11 => "smpte431",
                12 => "smpte432",
                _ => "",
            };
        }

        public static string GetColorTransferString(int n)
        {
            return n switch
            {
                1 => "bt709",
                4 => "gamma22",// "bt470m"
                5 => "gamma28",// "bt470bg"
                6 => "bt601",// "smpte170m"
                7 => "smpte240m",
                8 => "linear",
                11 => "iec61966-2-4",
                12 => "bt1361",
                13 => "srgb",
                14 => "bt2020-10",
                15 => "bt2020-12",
                16 => "smpte2084",
                17 => "smpte428",
                18 => "bt2100",
                _ => "",
            };
        }

        public static string GetColorMatrixCoeffsString(int n)
        {
            return n switch
            {
                1 => "bt709",
                4 => "fcc",
                5 => "bt470bg",
                6 => "bt601",
                7 => "smpte240m",
                8 => "ycgco",
                9 => "bt2020ncl",
                10 => "bt2020",
                _ => "",
            };
        }

        public static string GetColorRangeString(int n)
        {
            return n switch
            {
                1 => "tv",
                2 => "pc",
                _ => "",
            };
        }

        #endregion

        #region Get friendly name from int

        public static string GetColorPrimariesName(int n)
        {
            return n switch
            {
                1 => "BT.709",
                2 => "Unspecified",
                4 => "BT.470 System B, G (historical)",
                5 => "BT.470 System M (historical)",
                6 => "BT.601",
                7 => "SMPTE 240",
                8 => "Generic film (color filters using illuminant C)",
                9 => "BT.2020, BT.2100",
                10 => "SMPTE 428 (CIE 1921 XYZ)",
                11 => "SMPTE RP 431-2",
                12 => "SMPTE EG 432-1",
                22 => "EBU Tech. 3213-E",
                _ => "Unknown",
            };
        }

        public static string GetColorTransferName(int n)
        {
            return n switch
            {
                1 => "BT.709",
                2 => "Unspecified",
                4 => "BT.470 System B, G (historical)",
                5 => "BT.470 System M (historical)",
                6 => "BT.601",
                7 => "SMPTE 240 M",
                8 => "Linear",
                9 => "Logarithmic (100 : 1 range)",
                10 => "Logarithmic (100 * Sqrt(10) : 1 range)",
                11 => "IEC 61966-2-4",
                12 => "BT.1361",
                13 => "sRGB or sYCC",
                14 => "BT.2020 10-bit systems",
                15 => "BT.2020 12-bit systems",
                16 => "SMPTE ST 2084, ITU BT.2100 PQ",
                17 => "SMPTE ST 428",
                18 => "BT.2100 HLG, ARIB STD-B67",
                _ => "Unknown",
            };
        }

        public static string GetColorMatrixCoeffsName(int n)
        {
            return n switch
            {
                1 => "BT.709",
                2 => "Unspecified",
                4 => "US FCC 73.628",
                5 => "BT.470 System B, G (historical)",
                6 => "BT.601",
                7 => "SMPTE 240 M",
                8 => "YCgCo",
                9 => "BT.2020 non-constant luminance, BT.2100 YCbCr",
                10 => "BT.2020 constant luminance",
                11 => "SMPTE ST 2085 YDzDx",
                12 => "Chromaticity-derived non-constant luminance",
                13 => "Chromaticity-derived constant luminance",
                14 => "BT.2100 ICtCp",
                _ => "Unknown",
            };
        }

        public static string GetColorRangeName(int n)
        {
            return n switch
            {
                0 => "Unspecified",
                1 => "TV (Limited)",
                2 => "PC (Full)",
                _ => "Unknown",
            };
        }

        #endregion
    }
}
