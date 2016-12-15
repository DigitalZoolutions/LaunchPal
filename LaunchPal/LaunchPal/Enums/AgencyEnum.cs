using System;

namespace LaunchPal.Enums
{
    public enum AgencyType
    {
        Nasa,
        Esa,
        Roscosmos,
        ChinaSpaceAdministration,
        IndianSpaceOrganization,
        SpaceX,
        UnitedLaunchAlliance,
        OrbitalATK,
        BlueOrigin,
    }


    public static class AgencyEnum
    {
        public static string ToAbbreviationString(this AgencyType me)
        {
            switch (me)
            {
                case AgencyType.Nasa:
                    return "NASA";
                case AgencyType.Esa:
                    return "ESA";
                case AgencyType.Roscosmos:
                    return "FKA";
                case AgencyType.ChinaSpaceAdministration:
                    return "CNSA";
                case AgencyType.IndianSpaceOrganization:
                    return "ISRO";
                case AgencyType.SpaceX:
                    return "SpX";
                case AgencyType.UnitedLaunchAlliance:
                    return "ULA";
                case AgencyType.OrbitalATK:
                    return "OA";
                case AgencyType.BlueOrigin:
                    return "BO";
                default:
                    throw new ArgumentOutOfRangeException(nameof(me), me, null);
            }
        }

        public static string ToFriendlyString(this AgencyType me)
        {
            switch (me)
            {
                case AgencyType.Nasa:
                    return "NASA";
                case AgencyType.Esa:
                    return "ESA";
                case AgencyType.Roscosmos:
                    return "ROSCOSMOS";
                case AgencyType.ChinaSpaceAdministration:
                    return "CNSA";
                case AgencyType.IndianSpaceOrganization:
                    return "ISRO";
                case AgencyType.SpaceX:
                    return "SpaceX";
                case AgencyType.UnitedLaunchAlliance:
                    return "ULA";
                case AgencyType.OrbitalATK:
                    return "Orbital ATK";
                case AgencyType.BlueOrigin:
                    return "Blue Origin";
                default:
                    throw new ArgumentOutOfRangeException(nameof(me), me, null);
            }
        }
    }
}
