using System;

namespace LaunchPal.Enums
{
    public enum AgencyAbbreviation
    {
        Nasa,
        Esa,
        Fka,
        Cnsa,
        Isro,
        SpX,
        Ula,
        Oa,
        Bo,
    }


    public static class AgencyEnum
    {
        public static string ToAbbreviationString(this AgencyAbbreviation me)
        {
            switch (me)
            {
                case AgencyAbbreviation.Nasa:
                    return "NASA";
                case AgencyAbbreviation.Esa:
                    return "ESA";
                case AgencyAbbreviation.Fka:
                    return "FKA";
                case AgencyAbbreviation.Cnsa:
                    return "CNSA";
                case AgencyAbbreviation.Isro:
                    return "ISRO";
                case AgencyAbbreviation.SpX:
                    return "SpX";
                case AgencyAbbreviation.Ula:
                    return "ULA";
                case AgencyAbbreviation.Oa:
                    return "OA";
                case AgencyAbbreviation.Bo:
                    return "BO";
                default:
                    throw new ArgumentOutOfRangeException(nameof(me), me, null);
            }
        }

        public static string ToFriendlyString(this AgencyAbbreviation me)
        {
            switch (me)
            {
                case AgencyAbbreviation.Nasa:
                    return "NASA";
                case AgencyAbbreviation.Esa:
                    return "European Space Agency";
                case AgencyAbbreviation.Fka:
                    return "Russian Federal Space Agency";
                case AgencyAbbreviation.Cnsa:
                    return "China National Space Administration";
                case AgencyAbbreviation.Isro:
                    return "Indian Space Research Organization";
                case AgencyAbbreviation.SpX:
                    return "SpaceX";
                case AgencyAbbreviation.Ula:
                    return "United Launch Alliance";
                case AgencyAbbreviation.Oa:
                    return "Orbital ATK";
                case AgencyAbbreviation.Bo:
                    return "Blue Origin";
                default:
                    throw new ArgumentOutOfRangeException(nameof(me), me, null);
            }
        }
    }
}
