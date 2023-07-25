using FileHelpers;
using System;

namespace ExcelLoaders.DataObjects
{

    [IgnoreFirst(1)]
    [DelimitedRecord("\t")]
    public sealed class ResFutureOccupancy
    {

        public System.String SORT_COLUMN;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String RESV_TYPE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMCHILDREN;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMCHILDREN1;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMCHILDREN2;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMCHILDREN3;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMCHILDREN4;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMCHILDREN5;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMADULTS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMARRIVALROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMDEPARTUREROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMBLKRMSDEDUCT;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMBLKRMSNONDEDUCT;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMDEFINITEROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMTENTATIVEROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMOOOROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMOOSROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMROOMREVENUE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMEXTRAREVENUE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String SUMGUESTS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String PEROCCDEF;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String PEROCCTENT;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String AVGROOMREVENUE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String D_DATE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CONSIDERED_DATE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String DAY_DESC;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String ADULTS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHILDREN;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHILDREN1;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHILDREN2;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHILDREN3;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHILDREN4;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String CHILDREN5;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String ARRIVAL_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String DEPARTURE_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String NET_ROOM;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String PKG_REV;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String DAY_PICKUP;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String DAY_REMAIN;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String DEFINITE_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String TENTATIVE_ROOMS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String OUT_OF_SERVICE;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String OUT_OF_ORDER;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String GUESTS;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String PER_DEF_OCC;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String PER_TENT_OCC;

        [FieldOptional()]
        [FieldQuoted('"', QuoteMode.OptionalForBoth, MultilineMode.AllowForBoth)]
        public System.String AVG_RROOM_EV;
        

    }

}