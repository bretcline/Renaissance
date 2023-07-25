using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrosDataLoader.Data
{

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public partial class RESFUTUREOCCUPANCY
    {

        private RESFUTUREOCCUPANCYG_RESV_TYPE[] lIST_G_RESV_TYPEField;

        private object cF_LOGOField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("G_RESV_TYPE", IsNullable = false)]
        public RESFUTUREOCCUPANCYG_RESV_TYPE[] LIST_G_RESV_TYPE
        {
            get
            {
                return this.lIST_G_RESV_TYPEField;
            }
            set
            {
                this.lIST_G_RESV_TYPEField = value;
            }
        }

        /// <remarks/>
        public object CF_LOGO
        {
            get
            {
                return this.cF_LOGOField;
            }
            set
            {
                this.cF_LOGOField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RESFUTUREOCCUPANCYG_RESV_TYPE
    {

        private byte sORT_COLUMNField;

        private string rESV_TYPEField;

        private RESFUTUREOCCUPANCYG_RESV_TYPELIST_G_CONSIDERED_DATE lIST_G_CONSIDERED_DATEField;

        private byte sUMCHILDRENField;

        private byte sUMCHILDREN1Field;

        private byte sUMCHILDREN2Field;

        private byte sUMCHILDREN3Field;

        private byte sUMCHILDREN4Field;

        private byte sUMCHILDREN5Field;

        private byte sUMADULTSField;

        private byte sUMARRIVALROOMSField;

        private byte sUMDEPARTUREROOMSField;

        private byte sUMBLKRMSDEDUCTField;

        private byte sUMBLKRMSNONDEDUCTField;

        private byte sUMDEFINITEROOMSField;

        private byte sUMTENTATIVEROOMSField;

        private byte sUMOOOROOMSField;

        private byte sUMOOSROOMSField;

        private decimal sUMROOMREVENUEField;

        private decimal sUMEXTRAREVENUEField;

        private byte sUMGUESTSField;

        private decimal pEROCCDEFField;

        private byte pEROCCTENTField;

        private decimal aVGROOMREVENUEField;

        /// <remarks/>
        public byte SORT_COLUMN
        {
            get
            {
                return this.sORT_COLUMNField;
            }
            set
            {
                this.sORT_COLUMNField = value;
            }
        }

        /// <remarks/>
        public string RESV_TYPE
        {
            get
            {
                return this.rESV_TYPEField;
            }
            set
            {
                this.rESV_TYPEField = value;
            }
        }

        /// <remarks/>
        public RESFUTUREOCCUPANCYG_RESV_TYPELIST_G_CONSIDERED_DATE LIST_G_CONSIDERED_DATE
        {
            get
            {
                return this.lIST_G_CONSIDERED_DATEField;
            }
            set
            {
                this.lIST_G_CONSIDERED_DATEField = value;
            }
        }

        /// <remarks/>
        public byte SUMCHILDREN
        {
            get
            {
                return this.sUMCHILDRENField;
            }
            set
            {
                this.sUMCHILDRENField = value;
            }
        }

        /// <remarks/>
        public byte SUMCHILDREN1
        {
            get
            {
                return this.sUMCHILDREN1Field;
            }
            set
            {
                this.sUMCHILDREN1Field = value;
            }
        }

        /// <remarks/>
        public byte SUMCHILDREN2
        {
            get
            {
                return this.sUMCHILDREN2Field;
            }
            set
            {
                this.sUMCHILDREN2Field = value;
            }
        }

        /// <remarks/>
        public byte SUMCHILDREN3
        {
            get
            {
                return this.sUMCHILDREN3Field;
            }
            set
            {
                this.sUMCHILDREN3Field = value;
            }
        }

        /// <remarks/>
        public byte SUMCHILDREN4
        {
            get
            {
                return this.sUMCHILDREN4Field;
            }
            set
            {
                this.sUMCHILDREN4Field = value;
            }
        }

        /// <remarks/>
        public byte SUMCHILDREN5
        {
            get
            {
                return this.sUMCHILDREN5Field;
            }
            set
            {
                this.sUMCHILDREN5Field = value;
            }
        }

        /// <remarks/>
        public byte SUMADULTS
        {
            get
            {
                return this.sUMADULTSField;
            }
            set
            {
                this.sUMADULTSField = value;
            }
        }

        /// <remarks/>
        public byte SUMARRIVALROOMS
        {
            get
            {
                return this.sUMARRIVALROOMSField;
            }
            set
            {
                this.sUMARRIVALROOMSField = value;
            }
        }

        /// <remarks/>
        public byte SUMDEPARTUREROOMS
        {
            get
            {
                return this.sUMDEPARTUREROOMSField;
            }
            set
            {
                this.sUMDEPARTUREROOMSField = value;
            }
        }

        /// <remarks/>
        public byte SUMBLKRMSDEDUCT
        {
            get
            {
                return this.sUMBLKRMSDEDUCTField;
            }
            set
            {
                this.sUMBLKRMSDEDUCTField = value;
            }
        }

        /// <remarks/>
        public byte SUMBLKRMSNONDEDUCT
        {
            get
            {
                return this.sUMBLKRMSNONDEDUCTField;
            }
            set
            {
                this.sUMBLKRMSNONDEDUCTField = value;
            }
        }

        /// <remarks/>
        public byte SUMDEFINITEROOMS
        {
            get
            {
                return this.sUMDEFINITEROOMSField;
            }
            set
            {
                this.sUMDEFINITEROOMSField = value;
            }
        }

        /// <remarks/>
        public byte SUMTENTATIVEROOMS
        {
            get
            {
                return this.sUMTENTATIVEROOMSField;
            }
            set
            {
                this.sUMTENTATIVEROOMSField = value;
            }
        }

        /// <remarks/>
        public byte SUMOOOROOMS
        {
            get
            {
                return this.sUMOOOROOMSField;
            }
            set
            {
                this.sUMOOOROOMSField = value;
            }
        }

        /// <remarks/>
        public byte SUMOOSROOMS
        {
            get
            {
                return this.sUMOOSROOMSField;
            }
            set
            {
                this.sUMOOSROOMSField = value;
            }
        }

        /// <remarks/>
        public decimal SUMROOMREVENUE
        {
            get
            {
                return this.sUMROOMREVENUEField;
            }
            set
            {
                this.sUMROOMREVENUEField = value;
            }
        }

        /// <remarks/>
        public decimal SUMEXTRAREVENUE
        {
            get
            {
                return this.sUMEXTRAREVENUEField;
            }
            set
            {
                this.sUMEXTRAREVENUEField = value;
            }
        }

        /// <remarks/>
        public byte SUMGUESTS
        {
            get
            {
                return this.sUMGUESTSField;
            }
            set
            {
                this.sUMGUESTSField = value;
            }
        }

        /// <remarks/>
        public decimal PEROCCDEF
        {
            get
            {
                return this.pEROCCDEFField;
            }
            set
            {
                this.pEROCCDEFField = value;
            }
        }

        /// <remarks/>
        public byte PEROCCTENT
        {
            get
            {
                return this.pEROCCTENTField;
            }
            set
            {
                this.pEROCCTENTField = value;
            }
        }

        /// <remarks/>
        public decimal AVGROOMREVENUE
        {
            get
            {
                return this.aVGROOMREVENUEField;
            }
            set
            {
                this.aVGROOMREVENUEField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RESFUTUREOCCUPANCYG_RESV_TYPELIST_G_CONSIDERED_DATE
    {

        private RESFUTUREOCCUPANCYG_RESV_TYPELIST_G_CONSIDERED_DATEG_CONSIDERED_DATE g_CONSIDERED_DATEField;

        /// <remarks/>
        public RESFUTUREOCCUPANCYG_RESV_TYPELIST_G_CONSIDERED_DATEG_CONSIDERED_DATE G_CONSIDERED_DATE
        {
            get
            {
                return this.g_CONSIDERED_DATEField;
            }
            set
            {
                this.g_CONSIDERED_DATEField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RESFUTUREOCCUPANCYG_RESV_TYPELIST_G_CONSIDERED_DATEG_CONSIDERED_DATE
    {

        private string d_DATEField;

        private string cONSIDERED_DATEField;

        private string dAY_DESCField;

        private byte aDULTSField;

        private byte cHILDRENField;

        private byte cHILDREN1Field;

        private byte cHILDREN2Field;

        private byte cHILDREN3Field;

        private byte cHILDREN4Field;

        private byte cHILDREN5Field;

        private byte aRRIVAL_ROOMSField;

        private byte dEPARTURE_ROOMSField;

        private decimal nET_ROOMField;

        private decimal pKG_REVField;

        private byte dAY_PICKUPField;

        private byte dAY_REMAINField;

        private byte dEFINITE_ROOMSField;

        private byte tENTATIVE_ROOMSField;

        private byte oUT_OF_SERVICEField;

        private byte oUT_OF_ORDERField;

        private byte gUESTSField;

        private decimal pER_DEF_OCCField;

        private byte pER_TENT_OCCField;

        private decimal aVG_RROOM_EVField;

        /// <remarks/>
        public string D_DATE
        {
            get
            {
                return this.d_DATEField;
            }
            set
            {
                this.d_DATEField = value;
            }
        }

        /// <remarks/>
        public string CONSIDERED_DATE
        {
            get
            {
                return this.cONSIDERED_DATEField;
            }
            set
            {
                this.cONSIDERED_DATEField = value;
            }
        }

        /// <remarks/>
        public string DAY_DESC
        {
            get
            {
                return this.dAY_DESCField;
            }
            set
            {
                this.dAY_DESCField = value;
            }
        }

        /// <remarks/>
        public byte ADULTS
        {
            get
            {
                return this.aDULTSField;
            }
            set
            {
                this.aDULTSField = value;
            }
        }

        /// <remarks/>
        public byte CHILDREN
        {
            get
            {
                return this.cHILDRENField;
            }
            set
            {
                this.cHILDRENField = value;
            }
        }

        /// <remarks/>
        public byte CHILDREN1
        {
            get
            {
                return this.cHILDREN1Field;
            }
            set
            {
                this.cHILDREN1Field = value;
            }
        }

        /// <remarks/>
        public byte CHILDREN2
        {
            get
            {
                return this.cHILDREN2Field;
            }
            set
            {
                this.cHILDREN2Field = value;
            }
        }

        /// <remarks/>
        public byte CHILDREN3
        {
            get
            {
                return this.cHILDREN3Field;
            }
            set
            {
                this.cHILDREN3Field = value;
            }
        }

        /// <remarks/>
        public byte CHILDREN4
        {
            get
            {
                return this.cHILDREN4Field;
            }
            set
            {
                this.cHILDREN4Field = value;
            }
        }

        /// <remarks/>
        public byte CHILDREN5
        {
            get
            {
                return this.cHILDREN5Field;
            }
            set
            {
                this.cHILDREN5Field = value;
            }
        }

        /// <remarks/>
        public byte ARRIVAL_ROOMS
        {
            get
            {
                return this.aRRIVAL_ROOMSField;
            }
            set
            {
                this.aRRIVAL_ROOMSField = value;
            }
        }

        /// <remarks/>
        public byte DEPARTURE_ROOMS
        {
            get
            {
                return this.dEPARTURE_ROOMSField;
            }
            set
            {
                this.dEPARTURE_ROOMSField = value;
            }
        }

        /// <remarks/>
        public decimal NET_ROOM
        {
            get
            {
                return this.nET_ROOMField;
            }
            set
            {
                this.nET_ROOMField = value;
            }
        }

        /// <remarks/>
        public decimal PKG_REV
        {
            get
            {
                return this.pKG_REVField;
            }
            set
            {
                this.pKG_REVField = value;
            }
        }

        /// <remarks/>
        public byte DAY_PICKUP
        {
            get
            {
                return this.dAY_PICKUPField;
            }
            set
            {
                this.dAY_PICKUPField = value;
            }
        }

        /// <remarks/>
        public byte DAY_REMAIN
        {
            get
            {
                return this.dAY_REMAINField;
            }
            set
            {
                this.dAY_REMAINField = value;
            }
        }

        /// <remarks/>
        public byte DEFINITE_ROOMS
        {
            get
            {
                return this.dEFINITE_ROOMSField;
            }
            set
            {
                this.dEFINITE_ROOMSField = value;
            }
        }

        /// <remarks/>
        public byte TENTATIVE_ROOMS
        {
            get
            {
                return this.tENTATIVE_ROOMSField;
            }
            set
            {
                this.tENTATIVE_ROOMSField = value;
            }
        }

        /// <remarks/>
        public byte OUT_OF_SERVICE
        {
            get
            {
                return this.oUT_OF_SERVICEField;
            }
            set
            {
                this.oUT_OF_SERVICEField = value;
            }
        }

        /// <remarks/>
        public byte OUT_OF_ORDER
        {
            get
            {
                return this.oUT_OF_ORDERField;
            }
            set
            {
                this.oUT_OF_ORDERField = value;
            }
        }

        /// <remarks/>
        public byte GUESTS
        {
            get
            {
                return this.gUESTSField;
            }
            set
            {
                this.gUESTSField = value;
            }
        }

        /// <remarks/>
        public decimal PER_DEF_OCC
        {
            get
            {
                return this.pER_DEF_OCCField;
            }
            set
            {
                this.pER_DEF_OCCField = value;
            }
        }

        /// <remarks/>
        public byte PER_TENT_OCC
        {
            get
            {
                return this.pER_TENT_OCCField;
            }
            set
            {
                this.pER_TENT_OCCField = value;
            }
        }

        /// <remarks/>
        public decimal AVG_RROOM_EV
        {
            get
            {
                return this.aVG_RROOM_EVField;
            }
            set
            {
                this.aVG_RROOM_EVField = value;
            }
        }
    }




    class Forecast
    {
    }
}
