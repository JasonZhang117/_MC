[IntrabarOrderGeneration = False]
//ZwS-ATR-Y，移动止损
//-----------移动止损指标计算--------
inputs: ATR_length(20);
inputs: take_effect_multiple(5); //移动止损生效倍数
inputs: retracement_multiple(3); //移动止损回测倍数
variables: take_effect_base(0); //移动止损生效获利阈值
variables: retracement_base_d(0); //移动止损回撤值-多头
variables: retracement_base_k(0); //移动止损回撤值-空头
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


//-----------平仓量计算--------
inputs: FM(3); //持仓份额（分母）
inputs: FZ(1); //平仓份额（分子）
inputs: minPCS(1); //最小平仓量
inputs: LCS(0); //最小留仓量
variables: POC(0); //平仓量

value2 = ZwF_POC(FM,FZ,minPCS,LCS,POC);


//-----------移动止盈--------
variables: PCkz(True); //本次平仓标记
if MP <> MP[1] then
	begin
	PCkz = True;
	end;

if POC > 0 then
	begin
	if PCkz and MP > 0 then
		begin
		if stop_flag_d > 0 then
			begin
			MzyLineD = opening_maximum_h - retracement_base_d;	
			sell ("ATRsD") POC shares next bar at MzyLineD stop;
			{if Low <= MzyLineD then
				begin
				PCkz = False;
				end;}
			end;
		end;
	if PCkz and MP < 0 then
		begin
		if stop_flag_k > 0 then
			begin
			MzyLineK = opening_maximum_l + retracement_base_k;
			buytocover ("ATRsK") POC shares next bar at MzyLineK stop;
			{if High >= MzyLineK then
				begin
				PCkz = False;
				end;}
			end;
		end;
	end;
