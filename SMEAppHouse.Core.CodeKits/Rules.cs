namespace SMEAppHouse.Core.CodeKits
{
    public class Rules
    {
        public enum MonthEnum
        {
            NotSet = 0,
            January = 1,
            February = 2,
            March = 3,
            April = 4,
            May = 5,
            June = 6,
            July = 7,
            August = 8,
            September = 9,
            October = 10,
            November = 11,
            December = 12
        }

        public enum TimeSpanReportOptionEnum
        {
            BothDateAndTimePart,
            DatePartOnly,
            TimePartOnly,
        }

        public enum TimeIntervalTypesEnum
        {
            MilliSeconds = 0,
            Seconds,
            Minutes,
            Hours,
            Days
        }

        /// <summary>
        /// 
        /// ' ':'_'
        /// ', '-'__'
        /// ", ", "__"
        /// "'", "____" like: COTE_D'IVOIRE
        /// </summary>
        public enum WorldCountriesEnum
        {
            UNKNOWN,
            CHINA,
            INDONESIA,
            UNITED_STATES,
            BRAZIL,
            VENEZUELA,
            KAZAKHSTAN,
            RUSSIAN_FEDERATION,
            IRAN,
            UKRAINE,
            EGYPT,
            POLAND,
            INDIA,
            GERMANY,
            THAILAND,
            COLOMBIA,
            BANGLADESH,
            NETHERLANDS,
            CHILE,
            ECUADOR,
            UNITED_ARAB_EMIRATES,
            BULGARIA,
            SERBIA,
            TAIWAN,
            LATVIA,
            FRANCE,
            CZECH_REPUBLIC,
            HONG_KONG,
            MOLDOVA__REPUBLIC_OF,
            CAMBODIA,
            KOREA__REPUBLIC_OF,
            BOSNIA_AND_HERZEGOVINA,
            IRAQ,
            ROMANIA,
            PHILIPPINES,
            PERU,
            NIGERIA,
            SLOVENIA,
            TURKEY,
            KENYA,
            PAKISTAN,
            MEXICO,
            ARGENTINA,
            ITALY,
            MACEDONIA,
            MALAYSIA,
            HONDURAS,
            CANADA,
            AUSTRALIA,
            SPAIN,
            VIETNAM,
            MONGOLIA,
            HUNGARY,
            PALESTINIAN_TERRITORY__OCCUPIED,
            SYRIAN_ARAB_REPUBLIC,
            ALBANIA,
            MADAGASCAR,
            SLOVAKIA,
            JAPAN,
            UZBEKISTAN,
            SINGAPORE,
            NAMIBIA,
            LIBYAN_ARAB_JAMAHIRIYA,
            SWITZERLAND,
            JORDAN,
            SAUDI_ARABIA,
            PANAMA,
            EUROPE,
            NETHERLANDS_ANTILLES,
            ICELAND,
            COSTA_RICA,
            DENMARK,
            ZIMBABWE,
            BOLIVIA,
            SWEDEN,
            GUATEMALA,
            DOMINICAN_REPUBLIC,
            ESTONIA,
            ARMENIA,
            AZERBAIJAN,
            BELARUS,
            FINLAND,
            TURKMENISTAN,
            TANZANIA__UNITED_REPUBLIC_OF,
            NEW_ZEALAND,
            ZAMBIA,
            AUSTRIA,
            GHANA,
            UNITED_KINGDOM,
            CROATIA,
            GREECE,
            NEPAL,
            PARAGUAY,
            ZAIRE,
            SUDAN,
            ALGERIA,
            MACAO,
            NORWAY,
            LITHUANIA,
            HOLLAND,
            COTE_D____IVOIRE,
            TRINIDAD_AND_TOBAGO,
            PORTUGAL,
            EL_SALVADOR,
            LEBANON,
            UGANDA
        }
    }
}