
SELECT LIST( base_owner_name + '.' + base_table_name + '.' +  base_column_name, '|' ORDER BY column_number ) 
FROM sa_describe_query('SELECT *	
FROM micros.v_dtl dtl with(nolock)			
   left outer join micros.chk_dtl cd with(Nolock) on cd.chk_seq = dtl.chk_seq
   left outer join micros.emp_def empDef with(Nolock) on empDef.Emp_Seq = cd.Emp_Seq
   left outer join micros.mi_def mi on mi.mi_seq=M_mi_Seq
   left outer join micros.v_maj_grp_def maj on maj.seq = mi.maj_grp_seq
   left outer join micros.v_fam_grp_def fam on fam.seq = mi.fam_grp_seq
   left outer join MICROS.trans_dtl TRANS on TRANS.trans_seq = DTL.trans_seq
where 1 = 1
	--AND trans.end_date_tm >= DATEADD( hh, 3, convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11)))
	AND trans.end_date_tm <  DATEADD( hh, 3, convert(datetime, substring(convert(varchar, dateadd(day, 0, getdate()), 20), 1, 11))) 
order by trans.end_date_tm DESC, dtl.chk_seq, dtl.trans_seq, dtl.dtl_seq, cd.Chk_num;')
WHERE base_owner_name <> '';

OUTPUT TO 'd:\\DailyData\\POSData\\DailyExport.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    
    QUOTE '';

SELECT *
FROM micros.v_dtl dtl with(nolock)
   left outer join micros.chk_dtl cd with(Nolock) on cd.chk_seq = dtl.chk_seq
   left outer join micros.emp_def empDef with(Nolock) on empDef.Emp_Seq = cd.Emp_Seq
   left outer join micros.mi_def mi on mi.mi_seq=M_mi_Seq
   left outer join micros.v_maj_grp_def maj on maj.seq = mi.maj_grp_seq
   left outer join micros.v_fam_grp_def fam on fam.seq = mi.fam_grp_seq
   left outer join MICROS.trans_dtl TRANS on TRANS.trans_seq = DTL.trans_seq
where dtl.dtl_type in ('M','D')
	AND trans.end_date_tm >= DATEADD( hh, 3, convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11)))
	AND trans.end_date_tm <  DATEADD( hh, 3, convert(datetime, substring(convert(varchar, dateadd(day, 0, getdate()), 20), 1, 11))) 
order by trans.end_date_tm DESC, dtl.chk_seq, dtl.trans_seq, dtl.dtl_seq, cd.Chk_num;

OUTPUT TO 'd:\\DailyData\\POSData\\DailyExport.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    
    QUOTE '';



-- Check Details
SELECT LIST( name, '|' ORDER BY column_number ) 
FROM sa_describe_query('
	SELECT * FROM mfd_check_dtl 
' );
OUTPUT TO 'd:\\DailyData\\POSData\\mfd_check_dtl.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';

SELECT * FROM mfd_check_dtl 
WHERE 1 = 1
	AND Business_Date = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11));
OUTPUT TO 'd:\\DailyData\\POSData\\mfd_check_dtl.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';

	
-- Check Header/Total 
SELECT LIST( name, '|' ORDER BY column_number ) 
FROM sa_describe_query('
	SELECT * FROM mfd_check_ttl
' );
OUTPUT TO 'd:\\DailyData\\POSData\\mfd_check_ttl.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';

SELECT * FROM mfd_check_ttl
WHERE 1 = 1
	AND Business_Date = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11));
OUTPUT TO 'd:\\DailyData\\POSData\\mfd_check_ttl.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';


-- Corrections
SELECT LIST( name, '|' ORDER BY column_number ) 
FROM sa_describe_query('
SELECT * FROM v_fr_dly_corr_ttl 
' );
OUTPUT TO 'd:\\DailyData\\POSData\\v_fr_dly_corr_ttl.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';


SELECT * FROM v_fr_dly_corr_ttl 
WHERE 1 = 1
	AND Business_Date = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11));
OUTPUT TO 'd:\\DailyData\\POSData\\v_fr_dly_corr_ttl.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';


-- Discounts
SELECT LIST( name, '|' ORDER BY column_number ) 
FROM sa_describe_query('
SELECT * FROM v_fr_dly_discount_ttl 
' );
OUTPUT TO 'd:\\DailyData\\POSData\\v_fr_dly_discount_ttl.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';

SELECT * FROM v_fr_dly_discount_ttl 
WHERE 1 = 1
	AND Business_Date = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11));
OUTPUT TO 'd:\\DailyData\\POSData\\v_fr_dly_discount_ttl.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';



-- Periods
SELECT LIST( name, '|' ORDER BY column_number ) 
FROM sa_describe_query('
SELECT business_date, rvc_num, time_period_num, time_period_name, emp_num, chk_seq FROM micros.v_R_kds_chk_dtl 
' );
OUTPUT TO 'd:\\DailyData\\POSData\\v_R_kds_chk_dtl.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';

SELECT business_date, rvc_num, time_period_num, time_period_name, emp_num, chk_seq FROM micros.v_R_kds_chk_dtl 
WHERE 1 = 1
	AND Business_Date = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11));
OUTPUT TO 'd:\\DailyData\\POSData\\v_R_kds_chk_dtl.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';




	
-- POS Tickets
SELECT LIST( name, '|' ORDER BY column_number ) 
FROM sa_describe_query('
select distinct
    trans_dtl.business_date,
    chk_dtl.rvc_seq,
	rvc_def.obj_num,
	rvc_def.name,
    chk_dtl.emp_seq,
	emp_def.obj_num,
	emp_def.last_name,
    isnull(chk_dtl.tbl_seq,0) AS Tbl_Seq,
    isnull((select obj_num from micros.tbl_def where tbl_seq = chk_dtl.tbl_seq),0) AS Obj_Num,
    isnull((select name from micros.tbl_def where tbl_seq = chk_dtl.tbl_seq),'') AS Obj_Name,
    trans_dtl.chk_seq,
	chk_dtl.chk_num,
    chk_dtl.last_uws_seq,
	uws_def.obj_num,
    chk_dtl.order_type_seq,
	order_type_def.name,
    chk_dtl.chk_open_date_time,
    chk_dtl.chk_open_date_time,
    chk_dtl.chk_clsd_date_time,
    chk_dtl.chk_clsd_date_time,
    chk_dtl.cov_cnt,
    chk_dtl.auto_svc_ttl,
	chk_dtl.other_svc_ttl,
    chk_dtl.sub_ttl,
	chk_dtl.pymnt_ttl,
    chk_dtl.amt_due_ttl,
	chk_dtl.tax_ttl,
    chk_dtl.chk_prntd_cnt,
    chk_dtl.num_dtl,
	chk_dtl.num_mi_dtl,
    chk_dtl.order_type_seq
from micros.chk_dtl as chk_dtl
	, micros.trans_dtl as trans_dtl
    ,micros.uws_def as uws_def,micros.emp_def
    ,micros.order_type_def,micros.rvc_def
where chk_dtl.chk_clsd_date_time is not null
    AND trans_dtl.business_date = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11))
    and chk_dtl.training_status = 0
    and chk_dtl.chk_seq = trans_dtl.chk_seq
    and chk_dtl.last_uws_seq = uws_def.uws_seq
    and chk_dtl.emp_seq = emp_def.emp_seq
    and chk_dtl.order_type_seq = order_type_def.order_type_seq
    and chk_dtl.rvc_seq = rvc_def.rvc_seq
    and not exists(select Check_seq from mfd_check_ttl where Check_seq = trans_dtl.chk_seq)

' );
OUTPUT TO 'd:\\DailyData\\POSData\\Micros_Tickets.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';

select distinct
    trans_dtl.business_date,
    chk_dtl.rvc_seq,
	rvc_def.obj_num,
	rvc_def.name,
    chk_dtl.emp_seq,
	emp_def.obj_num,
	emp_def.last_name,
    isnull(chk_dtl.tbl_seq,0) AS Tbl_Seq,
    isnull((select obj_num from micros.tbl_def where tbl_seq = chk_dtl.tbl_seq),0) AS Obj_Num,
    isnull((select name from micros.tbl_def where tbl_seq = chk_dtl.tbl_seq),'') AS Obj_Name,
    trans_dtl.chk_seq,
	chk_dtl.chk_num,
    chk_dtl.last_uws_seq,
	uws_def.obj_num,
    chk_dtl.order_type_seq,
	order_type_def.name,
    chk_dtl.chk_open_date_time,
    chk_dtl.chk_open_date_time,
    chk_dtl.chk_clsd_date_time,
    chk_dtl.chk_clsd_date_time,
    chk_dtl.cov_cnt,
    chk_dtl.auto_svc_ttl,
	chk_dtl.other_svc_ttl,
    chk_dtl.sub_ttl,
	chk_dtl.pymnt_ttl,
    chk_dtl.amt_due_ttl,
	chk_dtl.tax_ttl,
    chk_dtl.chk_prntd_cnt,
    chk_dtl.num_dtl,
	chk_dtl.num_mi_dtl,
    chk_dtl.order_type_seq
from micros.chk_dtl as chk_dtl
	, micros.trans_dtl as trans_dtl
    ,micros.uws_def as uws_def,micros.emp_def
    ,micros.order_type_def,micros.rvc_def
where chk_dtl.chk_clsd_date_time is not null
    AND trans_dtl.business_date = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11))
    and chk_dtl.training_status = 0
    and chk_dtl.chk_seq = trans_dtl.chk_seq
    and chk_dtl.last_uws_seq = uws_def.uws_seq
    and chk_dtl.emp_seq = emp_def.emp_seq
    and chk_dtl.order_type_seq = order_type_def.order_type_seq
    and chk_dtl.rvc_seq = rvc_def.rvc_seq
    and not exists(select Check_seq from mfd_check_ttl where Check_seq = trans_dtl.chk_seq);
OUTPUT TO 'd:\\DailyData\\POSData\\Micros_Tickets.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';	
	
	
	


-- POS TicketDetails
SELECT LIST( name, '|' ORDER BY column_number ) 
FROM sa_describe_query('
select TRANS.BUSINESS_DATE,
        TRANS.RVC_SEQ,
        (select OBJ_NUM
          from MICROS.RVC_DEF
          where RVC_SEQ = TRANS.RVC_SEQ),
        (select NAME
          from MICROS.RVC_DEF
          where RVC_SEQ = TRANS.RVC_SEQ),
        (select OBJ_NUM
          from MICROS.UWS_DEF
          where UWS_SEQ = TRANS.UWS_SEQ),
        (select CHK_NUM
          from MICROS.CHK_DTL
          where CHK_SEQ = TRANS.CHK_SEQ),
        (select ORDER_TYPE_SEQ
          from MICROS.CHK_DTL
          where CHK_SEQ = TRANS.CHK_SEQ),
        TRANS.CHK_SEQ,
        ISNULL(TRANS.SRV_PERIOD_SEQ,0) AS SRV_PERIOD_SEQ,
        TRANS.TRANS_EMP_SEQ,
        TRANS.CHK_EMP_SEQ,
        TRANS.TRAINING_STATUS,
        TRANS.FIXED_PERIOD_SEQ,
        DTL.DATE_TIME,
        DTL.DTL_SEQ,
        DTL.TRANS_SEQ,
        DTL.DTL_TYPE,
        DTL.SEAT,
        DTL.RECORD_TYPE,
        DTL.DTL_INDEX,
        DTL.SHARED_NUMERATOR,
        DTL.SHARED_DENOMINATOR,
        DTL.RPT_INCLUSIVE_TAX_TTL, --26
        DTL.RPT_INCLUSIVE_TAX_TTL_EX,
        DTL.ACTIVE_TAXES,
        DTL.COMM_TTL,
        DTL.CHK_CNT,
        DTL.CHK_TTL,
        DTL.RPT_CNT,
        DTL.RPT_TTL,
        DTL.DTL_ID,
        DTL.DTL_STATUS,
        ISNULL(DTL.OB_DTL05_VOID_FLAG,''F'') AS OB_DTL05_VOID_FLAG,
        M.MI_SEQ,
        (select OBJ_NUM
          from MICROS.MI_DEF
          where MI_SEQ = M.MI_SEQ),
        (select NAME_1
          from MICROS.MI_DEF
          where MI_SEQ = M.MI_SEQ),
        (select NAME_2
          from MICROS.MI_DEF
          where MI_SEQ = M.MI_SEQ),
        M.CRS,
        M.OB_TAX_1_ACTIVE,
        M.OB_TAX_2_ACTIVE,
        M.OB_TAX_3_ACTIVE,
        M.OB_TAX_4_ACTIVE,
        M.OB_TAX_5_ACTIVE,
        M.OB_TAX_6_ACTIVE,
        M.OB_TAX_7_ACTIVE,
        M.OB_TAX_8_ACTIVE,
        M.SLS_ITMZR_SEQ,
        M.DSC_ITMZR,
        M.SVC_ITMZR,
        M.PRICE_LVL,
        M.SURCHARGE_TAX_TTL,
        ISNULL(M.OB_DTL04_RTN,''F'') AS OB_DTL04_RTN,
        T.TMED_SEQ,
        T.OTHER_EMP_SEQ,
        T.OB_TAX_1_EXEMPT,
        T.OB_TAX_2_EXEMPT,
        T.OB_TAX_3_EXEMPT,
        T.OB_TAX_4_EXEMPT,
        T.OB_TAX_5_EXEMPT,
        T.OB_TAX_6_EXEMPT,
        T.OB_TAX_7_EXEMPT,
        T.OB_TAX_8_EXEMPT,
        T.EXPIRATION_DATE,
        T.CHG_TIP_TTL,
        T.FRGN_CNCY_TTL,
        T.FRGN_CNCY_NUM_DECIMAL_PLACES,
        T.FRGN_CNCY_SEQ,
        T.OB_TIPS_PAID,
        T.ALLOCATED_TAX_TTL,
        R.REF,
        R.PARENT_DTL_SEQ,
        R.PARENT_TRANS_SEQ,
        --added by GKolot needed for record that have till to 10 references saved
        (case when DTL.DTL_TYPE = ''R'' then R2.REF+''^''
          +R3.REF+''^''
          +R4.REF+''^''
          +R5.REF+''^''
          +R6.REF+''^''
          +R7.REF+''^''
          +R8.REF+''^''
          +R9.REF+''^''
          +R10.REF
        else null
        end) as REF2,D.DSVC_SEQ,
        D.EMP_MEAL_EMP,
        D.PERCENTAGE,
        D.OB_TAX_1_EXEMPT,
        D.OB_TAX_2_EXEMPT,
        D.OB_TAX_3_EXEMPT,
        D.OB_TAX_4_EXEMPT,
        D.OB_TAX_5_EXEMPT,
        D.OB_TAX_6_EXEMPT,
        D.OB_TAX_7_EXEMPT,
        D.OB_TAX_8_EXEMPT,
        D.PARENT_DTL_SEQ,
        D.PARENT_TRANS_SEQ,
        D.PARENT_DTL_ID,
        D.TID_REF,
        D.TID_INST_ID,
        D.MI_SEQ,
        TRANS.OB_CHK_REOPENED,
        TRANS.OB_CLOSED_CHECK_EDIT,
        M.ITEM_WEIGHT,
        TRANS.TYPE,
        ISNULL((select SUM(DT.RPT_TTL)
          from MICROS.TRANS_DTL as TR
            ,MICROS.DTL as DT
            ,MICROS.DSVC_DTL as DSC
          where TR.TRANS_SEQ = DT.TRANS_SEQ
          and TR.TRANS_SEQ = DSC.TRANS_SEQ
          and DT.DTL_SEQ = DSC.DTL_SEQ
          and DT.DTL_TYPE = ''D''
          and DT.ORDER_TYPE_SEQ is not null
          and DSC.MI_SEQ = M.MI_SEQ
          and DT.ORIG_RVC_SEQ is not null
          and TR.TRAINING_STATUS = 0
          and DSC.PARENT_DTL_SEQ = M.DTL_SEQ
          and DSC.PARENT_TRANS_SEQ = M.TRANS_SEQ),
        0) as DSC_SUM,
        ISNULL((select SUM(DT.RPT_CNT)
          from MICROS.TRANS_DTL as TR
            ,MICROS.DTL as DT
            ,MICROS.DSVC_DTL as DSC
          where TR.TRANS_SEQ = DT.TRANS_SEQ
          and TR.TRANS_SEQ = DSC.TRANS_SEQ
          and DT.DTL_SEQ = DSC.DTL_SEQ
          and DT.DTL_TYPE = ''D''
          and DT.ORDER_TYPE_SEQ is not null
          and DSC.MI_SEQ = M.MI_SEQ
          and DT.ORIG_RVC_SEQ is not null
          and TR.TRAINING_STATUS = 0
          and DSC.PARENT_DTL_SEQ = M.DTL_SEQ
          and DSC.PARENT_TRANS_SEQ = M.TRANS_SEQ),
        0) as DSC_CNT,
        ISNULL((select SUM(DT.RPT_INCLUSIVE_TAX_TTL_EX)
          from MICROS.TRANS_DTL as TR
            ,MICROS.DTL as DT
            ,MICROS.DSVC_DTL as DSC
          where TR.TRANS_SEQ = DT.TRANS_SEQ
          and TR.TRANS_SEQ = DSC.TRANS_SEQ
          and DT.DTL_SEQ = DSC.DTL_SEQ
          and DT.DTL_TYPE = ''D''
          and DT.ORDER_TYPE_SEQ is not null
          and DSC.MI_SEQ = M.MI_SEQ
          and DT.ORIG_RVC_SEQ is not null
          and TR.TRAINING_STATUS = 0
          and DSC.PARENT_DTL_SEQ = M.DTL_SEQ
          and DSC.PARENT_TRANS_SEQ = M.TRANS_SEQ),
        0) as DSC_TAX
from((((((((((((((MICROS.DTL as DTL
          left outer join MICROS.MI_DTL as M
          on DTL.TRANS_SEQ = M.TRANS_SEQ
          and DTL.DTL_SEQ = M.DTL_SEQ)
          left outer join MICROS.TMED_DTL as T
          on DTL.TRANS_SEQ = T.TRANS_SEQ
          and DTL.DTL_SEQ = T.DTL_SEQ)
          left outer join MICROS.REF_DTL as R
          on DTL.TRANS_SEQ = R.TRANS_SEQ
          and DTL.DTL_SEQ = R.DTL_SEQ)
          --added by GKolot
          left outer join MICROS.REF_DTL as R2 --2. Ref
          on DTL.TRANS_SEQ = R2.TRANS_SEQ
          and DTL.DTL_SEQ+1 = R2.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R3 --3. Ref
          on DTL.TRANS_SEQ = R3.TRANS_SEQ
          and DTL.DTL_SEQ+2 = R3.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R4 --4. Ref
          on DTL.TRANS_SEQ = R4.TRANS_SEQ
          and DTL.DTL_SEQ+3 = R4.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R5 --5. Ref
          on DTL.TRANS_SEQ = R5.TRANS_SEQ
          and DTL.DTL_SEQ+4 = R5.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R6 --6. Ref
          on DTL.TRANS_SEQ = R6.TRANS_SEQ
          and DTL.DTL_SEQ+5 = R6.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R7 --7. Ref
          on DTL.TRANS_SEQ = R7.TRANS_SEQ
          and DTL.DTL_SEQ+6 = R7.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R8 --8. Ref
          on DTL.TRANS_SEQ = R8.TRANS_SEQ
          and DTL.DTL_SEQ+7 = R8.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R9 --9. Ref
          on DTL.TRANS_SEQ = R9.TRANS_SEQ
          and DTL.DTL_SEQ+8 = R9.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.REF_DTL as R10 --10. Ref
          on DTL.TRANS_SEQ = R10.TRANS_SEQ
          and DTL.DTL_SEQ+9 = R10.DTL_SEQ
          and DTL.DTL_TYPE = ''R'')
          left outer join MICROS.DSVC_DTL as D
          on DTL.TRANS_SEQ = D.TRANS_SEQ
          and DTL.DTL_SEQ = D.DTL_SEQ))
          ,MICROS.TRANS_DTL as TRANS
where TRANS.TRANS_SEQ = DTL.TRANS_SEQ
    AND TRANS.BUSINESS_DATE = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11))
    and TRANS.CHK_SEQ is not null
    and TRANS.TRAINING_STATUS = 0
    and(DTL.RECORD_TYPE = 'A'
        or DTL.RECORD_TYPE = 'I')
    and not (DTL.DTL_TYPE = 'P'
        or DTL.DTL_TYPE = 'W' )
' );
OUTPUT TO 'd:\\DailyData\\POSData\\Micros_Ticket_Details.csv' 
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';

select TRANS.BUSINESS_DATE,
        TRANS.RVC_SEQ,
        (select OBJ_NUM
          from MICROS.RVC_DEF
          where RVC_SEQ = TRANS.RVC_SEQ),
        (select NAME
          from MICROS.RVC_DEF
          where RVC_SEQ = TRANS.RVC_SEQ),
        (select OBJ_NUM
          from MICROS.UWS_DEF
          where UWS_SEQ = TRANS.UWS_SEQ),
        (select CHK_NUM
          from MICROS.CHK_DTL
          where CHK_SEQ = TRANS.CHK_SEQ),
        (select ORDER_TYPE_SEQ
          from MICROS.CHK_DTL
          where CHK_SEQ = TRANS.CHK_SEQ),
        TRANS.CHK_SEQ,
        ISNULL(TRANS.SRV_PERIOD_SEQ,0) AS SRV_PERIOD_SEQ,
        TRANS.TRANS_EMP_SEQ,
        TRANS.CHK_EMP_SEQ,
        TRANS.TRAINING_STATUS,
        TRANS.FIXED_PERIOD_SEQ,
        DTL.DATE_TIME,
        DTL.DTL_SEQ,
        DTL.TRANS_SEQ,
        DTL.DTL_TYPE,
        DTL.SEAT,
        DTL.RECORD_TYPE,
        DTL.DTL_INDEX,
        DTL.SHARED_NUMERATOR,
        DTL.SHARED_DENOMINATOR,
        DTL.RPT_INCLUSIVE_TAX_TTL, --26
        DTL.RPT_INCLUSIVE_TAX_TTL_EX,
        DTL.ACTIVE_TAXES,
        DTL.COMM_TTL,
        DTL.CHK_CNT,
        DTL.CHK_TTL,
        DTL.RPT_CNT,
        DTL.RPT_TTL,
        DTL.DTL_ID,
        DTL.DTL_STATUS,
        ISNULL(DTL.OB_DTL05_VOID_FLAG,'F') AS OB_DTL05_VOID_FLAG,
        M.MI_SEQ,
        (select OBJ_NUM
          from MICROS.MI_DEF
          where MI_SEQ = M.MI_SEQ),
        (select NAME_1
          from MICROS.MI_DEF
          where MI_SEQ = M.MI_SEQ),
        (select NAME_2
          from MICROS.MI_DEF
          where MI_SEQ = M.MI_SEQ),
        M.CRS,
        M.OB_TAX_1_ACTIVE,
        M.OB_TAX_2_ACTIVE,
        M.OB_TAX_3_ACTIVE,
        M.OB_TAX_4_ACTIVE,
        M.OB_TAX_5_ACTIVE,
        M.OB_TAX_6_ACTIVE,
        M.OB_TAX_7_ACTIVE,
        M.OB_TAX_8_ACTIVE,
        M.SLS_ITMZR_SEQ,
        M.DSC_ITMZR,
        M.SVC_ITMZR,
        M.PRICE_LVL,
        M.SURCHARGE_TAX_TTL,
        ISNULL(M.OB_DTL04_RTN,'F') AS OB_DTL04_RTN,
        T.TMED_SEQ,
        T.OTHER_EMP_SEQ,
        T.OB_TAX_1_EXEMPT,
        T.OB_TAX_2_EXEMPT,
        T.OB_TAX_3_EXEMPT,
        T.OB_TAX_4_EXEMPT,
        T.OB_TAX_5_EXEMPT,
        T.OB_TAX_6_EXEMPT,
        T.OB_TAX_7_EXEMPT,
        T.OB_TAX_8_EXEMPT,
        T.EXPIRATION_DATE,
        T.CHG_TIP_TTL,
        T.FRGN_CNCY_TTL,
        T.FRGN_CNCY_NUM_DECIMAL_PLACES,
        T.FRGN_CNCY_SEQ,
        T.OB_TIPS_PAID,
        T.ALLOCATED_TAX_TTL,
        R.REF,
        R.PARENT_DTL_SEQ,
        R.PARENT_TRANS_SEQ,
        --added by GKolot needed for record that have till to 10 references saved
        (case when DTL.DTL_TYPE = 'R' then R2.REF+'^'
          +R3.REF+'^'
          +R4.REF+'^'
          +R5.REF+'^'
          +R6.REF+'^'
          +R7.REF+'^'
          +R8.REF+'^'
          +R9.REF+'^'
          +R10.REF
        else null
        end) as REF2,D.DSVC_SEQ,
        D.EMP_MEAL_EMP,
        D.PERCENTAGE,
        D.OB_TAX_1_EXEMPT,
        D.OB_TAX_2_EXEMPT,
        D.OB_TAX_3_EXEMPT,
        D.OB_TAX_4_EXEMPT,
        D.OB_TAX_5_EXEMPT,
        D.OB_TAX_6_EXEMPT,
        D.OB_TAX_7_EXEMPT,
        D.OB_TAX_8_EXEMPT,
        D.PARENT_DTL_SEQ,
        D.PARENT_TRANS_SEQ,
        D.PARENT_DTL_ID,
        D.TID_REF,
        D.TID_INST_ID,
        D.MI_SEQ,
        TRANS.OB_CHK_REOPENED,
        TRANS.OB_CLOSED_CHECK_EDIT,
        M.ITEM_WEIGHT,
        TRANS.TYPE,
        ISNULL((select SUM(DT.RPT_TTL)
          from MICROS.TRANS_DTL as TR
            ,MICROS.DTL as DT
            ,MICROS.DSVC_DTL as DSC
          where TR.TRANS_SEQ = DT.TRANS_SEQ
          and TR.TRANS_SEQ = DSC.TRANS_SEQ
          and DT.DTL_SEQ = DSC.DTL_SEQ
          and DT.DTL_TYPE = 'D'
          and DT.ORDER_TYPE_SEQ is not null
          and DSC.MI_SEQ = M.MI_SEQ
          and DT.ORIG_RVC_SEQ is not null
          and TR.TRAINING_STATUS = 0
          and DSC.PARENT_DTL_SEQ = M.DTL_SEQ
          and DSC.PARENT_TRANS_SEQ = M.TRANS_SEQ),
        0) as DSC_SUM,
        ISNULL((select SUM(DT.RPT_CNT)
          from MICROS.TRANS_DTL as TR
            ,MICROS.DTL as DT
            ,MICROS.DSVC_DTL as DSC
          where TR.TRANS_SEQ = DT.TRANS_SEQ
          and TR.TRANS_SEQ = DSC.TRANS_SEQ
          and DT.DTL_SEQ = DSC.DTL_SEQ
          and DT.DTL_TYPE = 'D'
          and DT.ORDER_TYPE_SEQ is not null
          and DSC.MI_SEQ = M.MI_SEQ
          and DT.ORIG_RVC_SEQ is not null
          and TR.TRAINING_STATUS = 0
          and DSC.PARENT_DTL_SEQ = M.DTL_SEQ
          and DSC.PARENT_TRANS_SEQ = M.TRANS_SEQ),
        0) as DSC_CNT,
        ISNULL((select SUM(DT.RPT_INCLUSIVE_TAX_TTL_EX)
          from MICROS.TRANS_DTL as TR
            ,MICROS.DTL as DT
            ,MICROS.DSVC_DTL as DSC
          where TR.TRANS_SEQ = DT.TRANS_SEQ
          and TR.TRANS_SEQ = DSC.TRANS_SEQ
          and DT.DTL_SEQ = DSC.DTL_SEQ
          and DT.DTL_TYPE = 'D'
          and DT.ORDER_TYPE_SEQ is not null
          and DSC.MI_SEQ = M.MI_SEQ
          and DT.ORIG_RVC_SEQ is not null
          and TR.TRAINING_STATUS = 0
          and DSC.PARENT_DTL_SEQ = M.DTL_SEQ
          and DSC.PARENT_TRANS_SEQ = M.TRANS_SEQ),
        0) as DSC_TAX
from((((((((((((((MICROS.DTL as DTL
          left outer join MICROS.MI_DTL as M
          on DTL.TRANS_SEQ = M.TRANS_SEQ
          and DTL.DTL_SEQ = M.DTL_SEQ)
          left outer join MICROS.TMED_DTL as T
          on DTL.TRANS_SEQ = T.TRANS_SEQ
          and DTL.DTL_SEQ = T.DTL_SEQ)
          left outer join MICROS.REF_DTL as R
          on DTL.TRANS_SEQ = R.TRANS_SEQ
          and DTL.DTL_SEQ = R.DTL_SEQ)
          --added by GKolot
          left outer join MICROS.REF_DTL as R2 --2. Ref
          on DTL.TRANS_SEQ = R2.TRANS_SEQ
          and DTL.DTL_SEQ+1 = R2.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R3 --3. Ref
          on DTL.TRANS_SEQ = R3.TRANS_SEQ
          and DTL.DTL_SEQ+2 = R3.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R4 --4. Ref
          on DTL.TRANS_SEQ = R4.TRANS_SEQ
          and DTL.DTL_SEQ+3 = R4.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R5 --5. Ref
          on DTL.TRANS_SEQ = R5.TRANS_SEQ
          and DTL.DTL_SEQ+4 = R5.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R6 --6. Ref
          on DTL.TRANS_SEQ = R6.TRANS_SEQ
          and DTL.DTL_SEQ+5 = R6.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R7 --7. Ref
          on DTL.TRANS_SEQ = R7.TRANS_SEQ
          and DTL.DTL_SEQ+6 = R7.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R8 --8. Ref
          on DTL.TRANS_SEQ = R8.TRANS_SEQ
          and DTL.DTL_SEQ+7 = R8.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R9 --9. Ref
          on DTL.TRANS_SEQ = R9.TRANS_SEQ
          and DTL.DTL_SEQ+8 = R9.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.REF_DTL as R10 --10. Ref
          on DTL.TRANS_SEQ = R10.TRANS_SEQ
          and DTL.DTL_SEQ+9 = R10.DTL_SEQ
          and DTL.DTL_TYPE = 'R')
          left outer join MICROS.DSVC_DTL as D
          on DTL.TRANS_SEQ = D.TRANS_SEQ
          and DTL.DTL_SEQ = D.DTL_SEQ))
          ,MICROS.TRANS_DTL as TRANS
where TRANS.TRANS_SEQ = DTL.TRANS_SEQ
    AND TRANS.BUSINESS_DATE = convert(datetime, substring(convert(varchar, dateadd(day, -1, getdate()), 20), 1, 11))
    and TRANS.CHK_SEQ is not null
    and TRANS.TRAINING_STATUS = 0
    and(DTL.RECORD_TYPE = 'A'
        or DTL.RECORD_TYPE = 'I')
    and not (DTL.DTL_TYPE = 'P'
        or DTL.DTL_TYPE = 'W' );
OUTPUT TO 'd:\\DailyData\\POSData\\Micros_Ticket_Details.csv' APPEND
    FORMAT TEXT
    ENCODING 'UTF-8'
    DELIMITED BY '|'
    QUOTE '';









