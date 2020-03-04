[IntrabarOrderGeneration = True]
//ZwG-BS(B)，配对交易，被动模型
RecalcLastBarAfter(1); //1秒钟计算一次

variables: IntraBarPersist MP(0); //实际持仓方向（被动模型） 
variables: IntraBarPersist CTT(0); //实际持仓数量（被动模型） 
variables: IntraBarPersist GVMP(0); //获取主动模型实际持仓方向 
variables: IntraBarPersist GVCCT(0); //获取主动模型实际持仓数量 

MP = i_MarketPosition; //实际持仓方向（1：多头，-1：空头，0：空仓）
CTT = i_CurrentContracts; //实际持仓数量
GVMP = GVgetNamedInt("G_MP_RSIms",0); //获取主动模型实际持仓方向 
GVCCT = GVgetNamedInt("G_CTT_RSIms",0); //获取主动模型实际持仓数量 
if GVCCT>CTT then //主动模型持仓量大于被动模型持仓量（被动增仓）
	begin
	if GVMP>0 then //主动模型为多头持仓（被动增仓(开仓)-空头）
		Sellshort("KK") GVCCT-CTT shares next bar at market;
	if GVMP<0 then //主动模型为空头持仓（被动增仓(开仓)-多头）
		Buy("KD") GVCCT-CTT shares next bar at market;
	end;
if GVCCT<CTT then //被动模型持仓量小于主动模型持仓量（被动减仓）
	begin
	if MP>0 then //被动模型为多头持仓
		Sell("PD") CTT-GVCCT shares next bar at market; 
	if MP<0 then
		Buytocover("PK") CTT-GVCCT shares next bar at market; 
	end;
//GVSetNamedInt("PPBgu1",PositionProfit(0));

