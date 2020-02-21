//ZwF_MSL，移动止损
inputs: MP(numericseries); //多空状态
inputs: take_effect_base(numericseries); //移动止损生效值

variables: take_effect_base_d(0); //移动止损生效获利阈值-多头
variables: take_effect_base_k(0); //移动止损生效获利阈值-空头
inputs: opening_maximum_h(numericref); //开仓后价格极值-多头开仓后的最高点
inputs: opening_maximum_l(numericref); //开仓后价格极值-空头开仓后的最低点
inputs: stop_flag_d(numericref); //移动止损阀门-多头
inputs: stop_flag_k(numericref); //移动止损阀门-空头

if CurrentBar = 1 then
	begin
	opening_maximum_h = 0;	
	opening_maximum_l = 999999;
	stop_flag_d = 0;
	stop_flag_k = 0;
	end ;

if MP <> 1 then
	begin
	opening_maximum_h = 0;
	stop_flag_d = 0;
	take_effect_base_d = take_effect_base;
	end;
if MP <> -1 then
	begin
	opening_maximum_l = 999999;
	stop_flag_k = 0;
	take_effect_base_k = take_effect_base;
	end;
	
if MP > 0 then 
	begin
	opening_maximum_h = MaxList(opening_maximum_h, High) ;
	//开仓后价格极值超过移动止损生效值
	if opening_maximum_h >= entryprice + take_effect_base_d then 
		begin
		stop_flag_d = 1 ;
		end;
	end;
if MP < 0 then 
	begin
	opening_maximum_l = MinList(opening_maximum_l, Low);
	//开仓后价格极值超过移动止损生效值
	if opening_maximum_l <= entryprice - take_effect_base_k then
		begin
		stop_flag_k = 1 ;
		end;
	end;