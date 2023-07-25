using FileHelpers;
using System;

namespace ExcelLoaders.DataObjects
{
    [IgnoreFirst(1)]
    [DelimitedRecord("\t")]
    public sealed class FourWeekForecast
    {

        public System.String ROOM_CATG;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String ROOM_CATEGORY;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String MARKET_CODE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String RESV_DATE_DAY;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHAR_DATE_DAY;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHAR_DATE_NO;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMREVENUEPERROOMCATGPERDATE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMNOROOMSPERROOMCATGPERDATE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMNOROOMSPERROOMCATGPERMARKET;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMAVAILABLEROOMSPERROOMCATG;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMREVENUEPERDATE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMNOROOMSPERDATE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CF_TOTAL_AVAILABLE_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CF_ADR;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CF_FINAL_ADR;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMOUT_OF_ORDER_PER_DATE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CF_AVAILABLE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CS_TOTAL_AVAILABLE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CS_OUT_OF_ORDER;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CS_PHYSICAL_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String NO_OF_PHYSICAL_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String OO_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String NO_DEFINITE_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String TOTAL_REVENUE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String REPORT_ID;


    }
}