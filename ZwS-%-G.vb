[IntrabarOrderGeneration = True]
//-----------MaxHlHcExit--------
inputs: ATR_length(20);
inputs: take_effect_multiple(5); //移动止损生效倍数
inputs: retracement_multiple(3); //移动止损回测倍数
variables: take_effect_base(0); //移动止损生效值
variables: retracement_base_d(0); //移动止损回测值-多头
variables: retracement_base_k(0); //移动止损回测值-空头
variables: intrabarpersist MP(0); //多空状态
variables: ATR_value(0);

MP = i_MarketPosition;
ATR_value = AvgTrueRange(ATR_length);
take_effect_base = ATR_value * take_effect_multiple;

if MP <> 1 then
	begin
	retracement_base_d = ATR_value * retracement_multiple;;
	end;
if MP <> -1 then
	begin
	retracement_base_k = ATR_value * retracement_multiple;;
	end;

variables: stopping_base_d(0); //移动止损基数-多头
variables: stopping_base_k(0); //移动止损基数-空头
variables: opening_maximum_h(0); //开仓后价格极值-多头开仓后的最高点
variables: opening_maximum_l(999999); //开仓后价格极值-空头开仓后的最低点
variables: stop_flag_d(0); //移动止损阀门-多头
variables: stop_flag_k(0); //移动止损阀门-空头
variables: intrabarpersist MzyLineD(0); //移动止损线-多头
variables: intrabarpersist MzyLineK(0); //移动止损线-空头

value1 = ZwF_MSL(MP,take_effect_base,opening_maximum_h,opening_maximum_l,stop_flag_d,stop_flag_k);


//-----------MaxHlHcExit--------
inputs:
	FM(3),FZ(1),minPCS(1),LCS(0);
variables:
	PCkz(True),PCS(0);
if MP <> MP[1] then
	begin
	PCkz = True;
	end;
PCS = zwPSP(FM,FZ,minPCS,LCS);
if PCS > 0 then
	begin
	//if ExitName(1) <> "AzyPD" and MP > 0 then
	if PCkz and MP > 0 then
		begin
		if stop_flag_d > 0 then
			begin
			MzyLineD = opening_maximum_h - retracement_base_d;	
			sell ("ZyATRPD") PCS shares next bar at MzyLineD stop;
			{condition1 = Low <= MzyLineD point ;
			if condition1 then
				begin
				opening_maximum_h = 0;
				stop_flag_d = 0;
				PCkz = False;
				end;}
			end;
		end;
	//if ExitName(1) <> "AzyPK" and MP < 0 then
	if PCkz and MP < 0 then
		begin
		if stop_flag_k > 0 then
			begin
			MzyLineK = opening_maximum_l + retracement_base_k;
			buytocover ("ZyATRPK") PCS shares next bar at MzyLineK stop;
			{condition1 = High >= MzyLineK point ;
			if condition1 then
				begin
				opening_maximum_l = 999999;
				stop_flag_k = 0;
				PCkz = False;
				end;}
			end;
		end;
	end;
