[IntrabarOrderGeneration = True]
//ZwG-(1-2)-RSI,价差交易，价差(D1-D2)RSI

inputs:RSI_Len(20); //RSI长度参数
variables:IntraBarPersist D1_s_D2(0); //价差(D1-D2)
variables:IntraBarPersist RSI_D1_s_D2(0); //价差(D1-D2)RSI
value1 = ZwF_RSI_D1_s_D2(RSI_Len,D1_s_D2,RSI_D1_s_D2);

inputs:RSI_D_key(65); //RSI阈值
inputs:RSI_K_key(35); //RSI阈值

inputs:reap_profit_BS(10); //获利/止损价差
variables:IntraBarPersist BS_B_DK(0); //多空标志（1：多，-1：空）
variables:IntraBarPersist init_spread(0); //初始价差值(RSI突破时的价差值)
variables:IntraBarPersist ZS_BS(0); //止损价差值(RSI突破时的价差值)
variables:IntraBarPersist ZS_BS_S(0); //实际止损价差值(RSI突破时的价差值)
variables:IntraBarPersist ZY_BS(0); //止损价差值(RSI突破时的价差值)
variables:IntraBarPersist ZY_BS_S(0); //实际止损价差值(RSI突破时的价差值)

variables: IntraBarPersist MP(0); //实际持仓方向（主动模型）  
variables: IntraBarPersist CTT(0); //实际持仓数量（主动模型）  
MP = i_MarketPosition; //持仓方向（1：多头，-1：空头，0：空仓）
CTT = i_CurrentContracts; //持仓数量
GVSetNamedInt("G_MP_RSIms",MP); //设置全局变量“PD_MP_m”（持仓方向）
GVSetNamedInt("G_CTT_RSIms",CTT); //设置全局变量“PD_CTT_m”（持仓数量）

if CurrentBar = 1 then //初始化
	begin
	init_spread = D1_s_D2;
	ZS_BS = D1_s_D2;
	ZY_BS = D1_s_D2;
	end;

if RSI_D1_s_D2[1] crosses above RSI_D_key then //价差(D1-D2)RSI向上突破
	begin
	init_spread = D1_s_D2[1];
	ZS_BS = D1_s_D2[1] + reap_profit_BS;
	ZY_BS = D1_s_D2[1] - reap_profit_BS;
	BS_B_DK = 1;
	if MP >= 0 then
		begin
		ZS_BS_S = ZS_BS; //固定止损
		ZY_BS_S = ZY_BS; //固定止盈
		sellshort("Gk1-S") 1 shares next bar at Open; 
		end;
	end
else if RSI_D1_s_D2[1] crosses below RSI_K_key then //价差(D1-D2)RSI向下突破
	begin
	init_spread = D1_s_D2[1];
	ZS_BS = D1_s_D2[1] - reap_profit_BS;
	ZY_BS = D1_s_D2[1] + reap_profit_BS;
	BS_B_DK = -1;
	if MP <= 0 then
		begin
		ZS_BS_S = ZS_BS; //固定止损
		ZY_BS_S = ZY_BS; //固定止盈
		buy("Gk1-B") 1 shares next bar at Open; 
		end;
	end;

if BS_B_DK > 0 then
	begin
	if MP >= 0 and D1_s_D2 crosses above init_spread then
		begin
		ZS_BS_S = ZS_BS; //固定止损
		ZY_BS_S = ZY_BS; //固定止盈
		sellshort("Gk2-S") 1 shares next bar at Open; 
		end;
	end
else if BS_B_DK < 0 then
	begin
	if MP <= 0 and D1_s_D2 crosses below init_spread then
		begin
		ZS_BS_S = ZS_BS; //固定止损
		ZY_BS_S = ZY_BS; //固定止盈
		buy("Gk2-B") 1 shares next bar at Open; 
		end;
	end;

if MP < 0 then
	begin
	if D1_s_D2 > ZS_BS_S then //固定止损
		begin
		buytocover ("Gzs1-B") next bar at market; //止损（价差扩大）
		end
	else if D1_s_D2 < ZY_BS then //移动止盈
		begin
		buytocover ("Gzy1-B") next bar at market;  //止盈（价差回归）
		end;
	end
else if MP > 0 then
	begin
	if D1_s_D2 < ZS_BS_S then //固定止损
		begin
		sell ("Gzs1-S") next bar at market; //止损（价差收缩）
		end
	else if D1_s_D2 > ZY_BS then //移动止盈
		begin
		sell ("Gzy1-S") next bar at market; //止盈（价差回归）
		end;
	end;

//print(Times,BS_B_DK,MP,D1_s_D2,init_spread,ZS_BS,ZY_BS,ZS_BS_S,ZY_BS_S);
