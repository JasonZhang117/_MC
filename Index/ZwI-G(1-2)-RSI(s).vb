//ZwI-G-RSI(1-2),价差RSI指标

inputs: RSI_Len(20); //RSI长度参数
variables: D1_s_D2(0); //价差(D1-D2)
variables: RSI_D1_s_D2(0); //价差(D1-D2)RSI
value1 = ZwF_RSI_D1_s_D2(RSI_Len,D1_s_D2,RSI_D1_s_D2);

inputs: RSI_D_key(65); //RSI阈值
inputs: RSI_K_key(35); //RSI阈值

inputs:reap_profit_BS(10); //获利/止损价差
variables:BS_B_DK(0); //多空标志（1：多，-1：空）
variables:init_spread(0); //初始价差值(RSI突破时的价差值)
variables:ZS_BS(0); //止损价差值(RSI突破时的价差值)
variables:ZS_BS_S(0); //实际止损价差值(RSI突破时的价差值)
variables:ZY_BS(0); //止损价差值(RSI突破时的价差值)
variables:ZY_BS_S(0); //实际止损价差值(RSI突破时的价差值)

if CurrentBar = 1 then //初始化
	begin
	init_spread = D1_s_D2;
	ZS_BS = D1_s_D2;
	ZY_BS = D1_s_D2;
	end;

if RSI_D1_s_D2 crosses above RSI_D_key then //价差(D1-D2)RSI向上突破
	begin
	init_spread = D1_s_D2;
	ZS_BS = D1_s_D2 + reap_profit_BS;
	ZY_BS = D1_s_D2 - reap_profit_BS;
	BS_B_DK = 1;
	end
else if RSI_D1_s_D2 crosses below RSI_K_key then //价差(D1-D2)RSI向下突破
	begin
	init_spread = D1_s_D2;
	ZS_BS = D1_s_D2 - reap_profit_BS;
	ZY_BS = D1_s_D2 + reap_profit_BS;
	BS_B_DK = -1;
	end;

plot1(D1_s_D2,"D1_s_D2",white);
plot2(init_spread,"init_spread");
plot3(ZS_BS,"ZS_BS",yellow);
plot4(ZY_BS,"ZY_BS",yellow);
if BS_B_DK > 0 then
	begin
	SetPlotColor(2, red );
	end
else if  BS_B_DK < 0 then
	begin
	SetPlotColor(2, green );
	end