//ZwI-G-RSI(1-2),价差RSI指标

inputs: RSI_Len(20); //RSI长度参数
variables: D1_s_D2(0); //价差(D1-D2)
variables: RSI_D1_s_D2(0); //价差(D1-D2)RSI
value1 = ZwF_RSI_D1_s_D2(RSI_Len,D1_s_D2,RSI_D1_s_D2);

inputs: RSI_D_key(65); //RSI阈值
inputs: RSI_K_key(35); //RSI阈值

inputs: reap_profit_BS(10); //获利/止损价差
variables:init_spread_D(0); //初始价差值(RSI突破时的价差值)
variables:ZS_BS_D(0); //止损价差值(RSI突破时的价差值)
variables:ZY_BS_D(0); //止损价差值(RSI突破时的价差值)
variables:init_spread_K(0); //初始价差值(RSI突破时的价差值)
variables:ZS_BS_K(0); //止损价差值(RSI突破时的价差值)
variables:ZY_BS_K(0); //止损价差值(RSI突破时的价差值)

variables: IntraBarPersist MP(0); //实际持仓方向（主动模型）  
variables: IntraBarPersist CTT(0); //实际持仓数量（主动模型）  
MP = i_MarketPosition; //持仓方向（1：多头，-1：空头，0：空仓）
CTT = i_CurrentContracts; //持仓数量
GVSetNamedInt("PD_MP_m",MP); //设置全局变量“PD_MP_m”（持仓方向）
GVSetNamedInt("PD_CTT_m",CTT); //设置全局变量“PD_CTT_m”（持仓数量）

if CurrentBar = 1 then
	begin
	init_spread_D = D1_s_D2;
	ZS_BS_D = D1_s_D2 + reap_profit_BS;
	ZY_BS_D = D1_s_D2 - reap_profit_BS;
	init_spread_K = D1_s_D2;
	ZS_BS_K = D1_s_D2 - reap_profit_BS;
	ZY_BS_K = D1_s_D2 + reap_profit_BS;
	end;

if RSI_D1_s_D2 crosses above RSI_D_key then
	begin
	init_spread_D = D1_s_D2;
	ZS_BS_D = D1_s_D2 + reap_profit_BS;
	ZY_BS_D = D1_s_D2 - reap_profit_BS;
	end
else if RSI_D1_s_D2 crosses below RSI_K_key then
	begin
	init_spread_K = D1_s_D2;
	ZS_BS_K = D1_s_D2 - reap_profit_BS;
	ZY_BS_K = D1_s_D2 + reap_profit_BS;
	end;

plot1(D1_s_D2,"D1_s_D2",white);
plot2(init_spread_D,"init_spread_D",red);
plot3(ZS_BS_D,"ZS_BS_D",yellow);
plot4(ZY_BS_D,"ZY_BS_D",yellow);
plot5(init_spread_K,"init_spread_K",cyan);
plot6(ZS_BS_K,"ZS_BS_K",green);
plot7(ZY_BS_K,"ZY_BS_K",green);