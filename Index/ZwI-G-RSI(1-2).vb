//ZwI-G-RSI(1-2),价差RSI指标

inputs: RSI_Len(20); //RSI长度参数
variables: D1_s_D2(0); //价差(D1-D2)
variables: RSI_D1_s_D2(0); //价差(D1-D2)RSI
value1 = ZwF_RSI_D1_s_D2(RSI_Len,D1_s_D2,RSI_D1_s_D2);

inputs: RSI_D_key(65); //RSI阈值
inputs: RSI_K_key(35); //RSI阈值

if BarNumber > maxbarsback then
	begin
	plot3(RSI_D1_s_D2, "RSI_D1_s_D2",white );
	plot6(RSI_D_key,"RSI_D_key",white);
	plot7(RSI_K_key,"RSI_K_key",white);
	end;
if RSI_D1_s_D2 > RSI_D_key then 
	SetPlotColor( 3, red ) 
else if RSI_D1_s_D2 < RSI_K_key then 
	SetPlotColor( 3, green ) ;
