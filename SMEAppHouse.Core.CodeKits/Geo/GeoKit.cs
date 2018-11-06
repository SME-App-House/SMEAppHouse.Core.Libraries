using System.Collections.Generic;

namespace SMEAppHouse.Core.CodeKits.Geo
{
    public sealed class GeoKit
    {
        public List<CountryInfo> CountryInfos { get; set; }
        public GeoKit()
        {
            CountryInfos.Add(new CountryInfo(1, "AED", "United Arab Emirates", "Dirhams"));
            CountryInfos.Add(new CountryInfo(2, "AFN", "Afghanistan", "Afghanis"));
            CountryInfos.Add(new CountryInfo(3, "ALL", "Albania", "Leke"));
            CountryInfos.Add(new CountryInfo(4, "AMD", "Armenia", "Drams"));
            CountryInfos.Add(new CountryInfo(5, "ANG", "Netherlands Antilles", "Guilders (also called Florins)"));
            CountryInfos.Add(new CountryInfo(6, "AOA", "Angola", "Kwanza"));
            CountryInfos.Add(new CountryInfo(7, "ARS", "Argentina", "Pesos"));
            CountryInfos.Add(new CountryInfo(8, "AUD", "Australia", "Dollars"));
            CountryInfos.Add(new CountryInfo(9, "AWG", "Aruba", "Guilders (also called Florins)"));
            CountryInfos.Add(new CountryInfo(10, "AZN", "Azerbaijan", "New Manats"));
            CountryInfos.Add(new CountryInfo(11, "BAM", "Bosnia and Herzegovina", "Convertible Marka"));
            CountryInfos.Add(new CountryInfo(12, "BBD", "Barbados", "Dollars"));
            CountryInfos.Add(new CountryInfo(13, "BDT", "Bangladesh", "Taka"));
            CountryInfos.Add(new CountryInfo(14, "BGN", "Bulgaria", "Leva"));
            CountryInfos.Add(new CountryInfo(15, "BHD", "Bahrain", "Dinars"));
            CountryInfos.Add(new CountryInfo(16, "BIF", "Burundi", "Francs"));
            CountryInfos.Add(new CountryInfo(17, "BMD", "Bermuda", "Dollars"));
            CountryInfos.Add(new CountryInfo(18, "BND", "Brunei Darussalam", "Dollars"));
            CountryInfos.Add(new CountryInfo(19, "BOB", "Bolivia", "Bolivianos"));
            CountryInfos.Add(new CountryInfo(20, "BRL", "Brazil", "Brazil Real"));
            CountryInfos.Add(new CountryInfo(21, "BSD", "Bahamas", "Dollars"));
            CountryInfos.Add(new CountryInfo(22, "BTN", "Bhutan", "Ngultrum"));
            CountryInfos.Add(new CountryInfo(23, "BWP", "Botswana", "Pulas"));
            CountryInfos.Add(new CountryInfo(24, "BYR", "Belarus", "Rubles"));
            CountryInfos.Add(new CountryInfo(25, "BZD", "Belize", "Dollars"));
            CountryInfos.Add(new CountryInfo(26, "CAD", "Canada", "Dollars"));
            CountryInfos.Add(new CountryInfo(27, "CDF", "Congo/Kinshasa", "Congolese Francs"));
            CountryInfos.Add(new CountryInfo(28, "CHF", "Switzerland", "Francs"));
            CountryInfos.Add(new CountryInfo(29, "CLP", "Chile", "Pesos"));
            CountryInfos.Add(new CountryInfo(30, "CNY", "China", "Yuan Renminbi"));
            CountryInfos.Add(new CountryInfo(31, "COP", "Colombia", "Pesos"));
            CountryInfos.Add(new CountryInfo(32, "CRC", "Costa Rica", "Colones"));
            CountryInfos.Add(new CountryInfo(33, "CUP", "Cuba", "Pesos"));
            CountryInfos.Add(new CountryInfo(34, "CVE", "Cape Verde", "Escudos"));
            CountryInfos.Add(new CountryInfo(35, "CYP", "Cyprus", "Pounds (expires 2008-Jan-31)"));
            CountryInfos.Add(new CountryInfo(36, "CZK", "Czech Republic", "Koruny"));
            CountryInfos.Add(new CountryInfo(37, "DJF", "Djibouti", "Francs"));
            CountryInfos.Add(new CountryInfo(38, "DKK", "Denmark", "Kroner"));
            CountryInfos.Add(new CountryInfo(39, "DOP", "Dominican Republic", "Pesos"));
            CountryInfos.Add(new CountryInfo(40, "DZD", "Algeria", "Algeria Dinars"));
            CountryInfos.Add(new CountryInfo(41, "EEK", "Estonia", "Krooni"));
            CountryInfos.Add(new CountryInfo(42, "EGP", "Egypt", "Pounds"));
            CountryInfos.Add(new CountryInfo(43, "ERN", "Eritrea", "Nakfa"));
            CountryInfos.Add(new CountryInfo(44, "ETB", "Ethiopia", "Birr"));
            CountryInfos.Add(new CountryInfo(45, "EUR", "Euro Member Countries", "Euro"));
            CountryInfos.Add(new CountryInfo(46, "FJD", "Fiji", "Dollars"));
            CountryInfos.Add(new CountryInfo(47, "FKP", "Falkland Islands (Malvinas)", "Pounds"));
            CountryInfos.Add(new CountryInfo(48, "GBP", "United Kingdom", "Pounds"));
            CountryInfos.Add(new CountryInfo(49, "GEL", "Georgia", "Lari"));
            CountryInfos.Add(new CountryInfo(50, "GGP", "Guernsey", "Pounds"));
            CountryInfos.Add(new CountryInfo(51, "GHS", "Ghana", "Cedis"));
            CountryInfos.Add(new CountryInfo(52, "GIP", "Gibraltar", "Pounds"));
            CountryInfos.Add(new CountryInfo(53, "GMD", "Gambia", "Dalasi"));
            CountryInfos.Add(new CountryInfo(54, "GNF", "Guinea", "Francs"));
            CountryInfos.Add(new CountryInfo(55, "GTQ", "Guatemala", "Quetzales"));
            CountryInfos.Add(new CountryInfo(56, "GYD", "Guyana", "Dollars"));
            CountryInfos.Add(new CountryInfo(57, "HKD", "Hong Kong", "Dollars"));
            CountryInfos.Add(new CountryInfo(58, "HNL", "Honduras", "Lempiras"));
            CountryInfos.Add(new CountryInfo(59, "HRK", "Croatia", "Kuna"));
            CountryInfos.Add(new CountryInfo(60, "HTG", "Haiti", "Gourdes"));
            CountryInfos.Add(new CountryInfo(61, "HUF", "Hungary", "Forint"));
            CountryInfos.Add(new CountryInfo(62, "IDR", "Indonesia", "Rupiahs"));
            CountryInfos.Add(new CountryInfo(63, "ILS", "Israel", "New Shekels"));
            CountryInfos.Add(new CountryInfo(64, "IMP", "Isle of Man", "Pounds"));
            CountryInfos.Add(new CountryInfo(65, "INR", "India", "Rupees"));
            CountryInfos.Add(new CountryInfo(66, "IQD", "Iraq", "Dinars"));
            CountryInfos.Add(new CountryInfo(67, "IRR", "Iran", "Rials"));
            CountryInfos.Add(new CountryInfo(68, "ISK", "Iceland", "Kronur"));
            CountryInfos.Add(new CountryInfo(69, "JEP", "Jersey", "Pounds"));
            CountryInfos.Add(new CountryInfo(70, "JMD", "Jamaica", "Dollars"));
            CountryInfos.Add(new CountryInfo(71, "JOD", "Jordan", "Dinars"));
            CountryInfos.Add(new CountryInfo(72, "JPY", "Japan", "Yen"));
            CountryInfos.Add(new CountryInfo(73, "KES", "Kenya", "Shillings"));
            CountryInfos.Add(new CountryInfo(74, "KGS", "Kyrgyzstan", "Soms"));
            CountryInfos.Add(new CountryInfo(75, "KHR", "Cambodia", "Riels"));
            CountryInfos.Add(new CountryInfo(76, "KMF", "Comoros", "Francs"));
            CountryInfos.Add(new CountryInfo(77, "KPW", "Korea (North)", "Won"));
            CountryInfos.Add(new CountryInfo(78, "KRW", "Korea (South)", "Won"));
            CountryInfos.Add(new CountryInfo(79, "KWD", "Kuwait", "Dinars"));
            CountryInfos.Add(new CountryInfo(80, "KYD", "Cayman Islands", "Dollars"));
            CountryInfos.Add(new CountryInfo(81, "KZT", "Kazakhstan", "Tenge"));
            CountryInfos.Add(new CountryInfo(82, "LAK", "Laos", "Kips"));
            CountryInfos.Add(new CountryInfo(83, "LBP", "Lebanon", "Pounds"));
            CountryInfos.Add(new CountryInfo(84, "LKR", "Sri Lanka", "Rupees"));
            CountryInfos.Add(new CountryInfo(85, "LRD", "Liberia", "Dollars"));
            CountryInfos.Add(new CountryInfo(86, "LSL", "Lesotho", "Maloti"));
            CountryInfos.Add(new CountryInfo(87, "LTL", "Lithuania", "Litai"));
            CountryInfos.Add(new CountryInfo(88, "LVL", "Latvia", "Lati"));
            CountryInfos.Add(new CountryInfo(89, "LYD", "Libya", "Dinars"));
            CountryInfos.Add(new CountryInfo(90, "MAD", "Morocco", "Dirhams"));
            CountryInfos.Add(new CountryInfo(91, "MDL", "Moldova", "Lei"));
            CountryInfos.Add(new CountryInfo(92, "MGA", "Madagascar", "Ariary"));
            CountryInfos.Add(new CountryInfo(93, "MKD", "Macedonia", "Denars"));
            CountryInfos.Add(new CountryInfo(94, "MMK", "Myanmar (Burma)", "Kyats"));
            CountryInfos.Add(new CountryInfo(95, "MNT", "Mongolia", "Tugriks"));
            CountryInfos.Add(new CountryInfo(96, "MOP", "Macau", "Patacas"));
            CountryInfos.Add(new CountryInfo(97, "MRO", "Mauritania", "Ouguiyas"));
            CountryInfos.Add(new CountryInfo(98, "MTL", "Malta", "Liri (expires 2008-Jan-31)"));
            CountryInfos.Add(new CountryInfo(99, "MUR", "Mauritius", "Rupees"));
            CountryInfos.Add(new CountryInfo(100, "MVR", "Maldives (Maldive Islands)", "Rufiyaa"));
            CountryInfos.Add(new CountryInfo(101, "MWK", "Malawi", "Kwachas"));
            CountryInfos.Add(new CountryInfo(102, "MXN", "Mexico", "Pesos"));
            CountryInfos.Add(new CountryInfo(103, "MYR", "Malaysia", "Ringgits"));
            CountryInfos.Add(new CountryInfo(104, "MZN", "Mozambique", "Meticais"));
            CountryInfos.Add(new CountryInfo(105, "NAD", "Namibia", "Dollars"));
            CountryInfos.Add(new CountryInfo(106, "NGN", "Nigeria", "Nairas"));
            CountryInfos.Add(new CountryInfo(107, "NIO", "Nicaragua", "Cordobas"));
            CountryInfos.Add(new CountryInfo(108, "NOK", "Norway", "Krone"));
            CountryInfos.Add(new CountryInfo(109, "NPR", "Nepal", "Nepal Rupees"));
            CountryInfos.Add(new CountryInfo(110, "NZD", "New Zealand", "Dollars"));
            CountryInfos.Add(new CountryInfo(111, "OMR", "Oman", "Rials"));
            CountryInfos.Add(new CountryInfo(112, "PAB", "Panama", "Balboa"));
            CountryInfos.Add(new CountryInfo(113, "PEN", "Peru", "Nuevos Soles"));
            CountryInfos.Add(new CountryInfo(114, "PGK", "Papua New Guinea", "Kina"));
            CountryInfos.Add(new CountryInfo(115, "PHP", "Philippines", "Pesos"));
            CountryInfos.Add(new CountryInfo(116, "PKR", "Pakistan", "Rupees"));
            CountryInfos.Add(new CountryInfo(117, "PLN", "Poland", "Zlotych"));
            CountryInfos.Add(new CountryInfo(118, "PYG", "Paraguay", "Guarani"));
            CountryInfos.Add(new CountryInfo(119, "QAR", "Qatar", "Rials"));
            CountryInfos.Add(new CountryInfo(120, "RON", "Romania", "New Lei"));
            CountryInfos.Add(new CountryInfo(121, "RSD", "Serbia", "Dinars"));
            CountryInfos.Add(new CountryInfo(122, "RUB", "Russia", "Rubles"));
            CountryInfos.Add(new CountryInfo(123, "RWF", "Rwanda", "Rwanda Francs"));
            CountryInfos.Add(new CountryInfo(124, "SAR", "Saudi Arabia", "Riyals"));
            CountryInfos.Add(new CountryInfo(125, "SBD", "Solomon Islands", "Dollars"));
            CountryInfos.Add(new CountryInfo(126, "SCR", "Seychelles", "Rupees"));
            CountryInfos.Add(new CountryInfo(127, "SDG", "Sudan", "Pounds"));
            CountryInfos.Add(new CountryInfo(128, "SEK", "Sweden", "Kronor"));
            CountryInfos.Add(new CountryInfo(129, "SGD", "Singapore", "Dollars"));
            CountryInfos.Add(new CountryInfo(130, "SHP", "Saint Helena", "Pounds"));
            CountryInfos.Add(new CountryInfo(131, "SLL", "Sierra Leone", "Leones"));
            CountryInfos.Add(new CountryInfo(132, "SOS", "Somalia", "Shillings"));
            CountryInfos.Add(new CountryInfo(133, "SPL", "Seborga", "Luigini"));
            CountryInfos.Add(new CountryInfo(134, "SRD", "Suriname", "Dollars"));
            CountryInfos.Add(new CountryInfo(135, "STD", "São Tome and Principe", "Dobras"));
            CountryInfos.Add(new CountryInfo(136, "SVC", "El Salvador", "Colones"));
            CountryInfos.Add(new CountryInfo(137, "SYP", "Syria", "Pounds"));
            CountryInfos.Add(new CountryInfo(138, "SZL", "Swaziland", "Emalangeni"));
            CountryInfos.Add(new CountryInfo(139, "THB", "Thailand", "Baht"));
            CountryInfos.Add(new CountryInfo(140, "TJS", "Tajikistan", "Somoni"));
            CountryInfos.Add(new CountryInfo(141, "TMM", "Turkmenistan", "Manats"));
            CountryInfos.Add(new CountryInfo(142, "TND", "Tunisia", "Dinars"));
            CountryInfos.Add(new CountryInfo(143, "TOP", "Tonga", "Pa'anga"));
            CountryInfos.Add(new CountryInfo(144, "TRY", "Turkey", "New Lira"));
            CountryInfos.Add(new CountryInfo(145, "TTD", "Trinidad and Tobago", "Dollars"));
            CountryInfos.Add(new CountryInfo(146, "TVD", "Tuvalu", "Tuvalu Dollars"));
            CountryInfos.Add(new CountryInfo(147, "TWD", "Taiwan", "New Dollars"));
            CountryInfos.Add(new CountryInfo(148, "TZS", "Tanzania", "Shillings"));
            CountryInfos.Add(new CountryInfo(149, "UAH", "Ukraine", "Hryvnia"));
            CountryInfos.Add(new CountryInfo(150, "UGX", "Uganda", "Shillings"));
            CountryInfos.Add(new CountryInfo(151, "USD", "United States of America", "Dollars"));
            CountryInfos.Add(new CountryInfo(152, "UYU", "Uruguay", "Pesos"));
            CountryInfos.Add(new CountryInfo(153, "UZS", "Uzbekistan", "Sums"));
            CountryInfos.Add(new CountryInfo(154, "VEB", "Venezuela", "Bolivares (expires 2008-Jun-30)"));
            CountryInfos.Add(new CountryInfo(155, "VEF", "Venezuela", "Bolivares Fuertes"));
            CountryInfos.Add(new CountryInfo(156, "VND", "Viet Nam", "Dong"));
            CountryInfos.Add(new CountryInfo(157, "VUV", "Vanuatu", "Vatu"));
            CountryInfos.Add(new CountryInfo(158, "WST", "Samoa", "Tala"));
            CountryInfos.Add(new CountryInfo(159, "XAF", "Communauté Financière Africaine BEAC", "Francs"));
            CountryInfos.Add(new CountryInfo(160, "XAG", "Silver", "Ounces"));
            CountryInfos.Add(new CountryInfo(161, "XAU", "Gold", "Ounces"));
            CountryInfos.Add(new CountryInfo(162, "XCD", "East Caribbean", "Dollars"));
            CountryInfos.Add(new CountryInfo(163, "XDR", "International Monetary Fund (IMF)", "Special Drawing Rights"));
            CountryInfos.Add(new CountryInfo(164, "XOF", "Communauté Financière Africaine BCEAO", "Francs"));
            CountryInfos.Add(new CountryInfo(165, "XPD", "Palladium", "Ounces"));
            CountryInfos.Add(new CountryInfo(166, "XPF", "Comptoirs Français du Pacifique", "Francs"));
            CountryInfos.Add(new CountryInfo(167, "XPT", "Platinum", "Ounces"));
            CountryInfos.Add(new CountryInfo(168, "YER", "Yemen", "Rials"));
            CountryInfos.Add(new CountryInfo(169, "ZAR", "South Africa", "Rand"));
            CountryInfos.Add(new CountryInfo(170, "ZMK", "Zambia", "Kwacha"));
            CountryInfos.Add(new CountryInfo(171, "ZWD", "Zimbabwe", "Zimbabwe Dollars"));
        }
    }

    public class CountryInfo
    {
        int id;
        string code = string.Empty;
        string name = string.Empty;
        string currency = string.Empty;

        public CountryInfo(int id, string code, string name, string currency)
        {
            this.id = id;
            this.code = code;
            this.name = name;
            this.currency = currency;
        }
    }

}
