using System.Collections.Generic;

namespace ASCOM.Alpaca.Simulators
{
    internal class ValidAlpacaKeys
    {
        internal static List<string> ValidParameterKeys = new List<string>{
            "ClientID" ,
            "ClientTransactionID",
            "RightAscension",
            "Declination",
            "Id",
            "SensorName",
            "Axis"
        };

        internal static List<string> ValidFormKeys = new List<string> {
            "ClientID",
            "ClientTransactionID",
            "BinX",
            "BinY",
            "CoolerOn",
            "FastReadout",
            "Gain",
            "NumX",
            "NumY",
            "Offset",
            "ReadoutMode",
            "SetCCDTemperature",
            "StartX",
            "StartY",
            "SubExposureDuration",
            "Direction",
            "Duration",
            "Light",
            "Action",
            "Parameters",
            "Command",
            "Raw",
            "Connected",
            "Brightness",
            "Slaved",
            "Altitude",
            "Azimuth",
            "Position",
            "TempComp",
            "AveragePeriod",
            "Reverse",
            "Id",
            "State",
            "Name",
            "Value",
            "Axis",
            "DeclinationRate",
            "RightAscension",
            "Declination",
            "DoesRefraction",
            "GuideRateDeclination",
            "GuideRateRightAscension",
            "Axis",
            "Rate",
            "RightAscensionRate",
            "SideOfPier",
            "SiteElevation",
            "SiteLatitude",
            "SiteLongitude",
            "SlewSettleTime",
            "TargetDeclination",
            "TargetRightAscension",
            "Tracking",
            "TrackingRate",
            "UTCDate",

        };
    }
}