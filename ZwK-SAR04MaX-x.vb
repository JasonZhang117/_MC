[IntrabarOrderGeneration = False]
//Zw-K-SAR04-Ma12-x
variables: var0(0); //抛物线值
variables: var2(0); //抛物线多空标识（1--》多头，抛物线在k线下方，-1--》空头，抛物线在k线上方）
variables: var3(0); //抛物线多空转换点标识（1--》空转多，-1--》多转空，0--》非转换点）

variables: DKKZ(0);//多空控制
variables: dKcPrice(High);//多头开仓点位
variables: dZsPrice(Low);//多头止损点位
variables: kKcPrice(Low);//空头开仓点位
variables: kZsPrice(High);//空头止损点位
variables: KcPrice(Open);//开仓点位
variables: ZsPrice(Open);//止损点位
//------------------指标计算------------------//
//------------------抛物线形态指标------------------//
value1 = ZwF_SAR_B(var0,var2,var3,DKKZ,dKcPrice,dZsPrice,kKcPrice,kZsPrice,KcPrice,ZsPrice);
//------------------均线指标------------------//
inputs: Mlen(120);//均线长度
variables: MA1(Open); //均线
variables: DKKZ_Ma(0); //抛物转换点均线状态
//---均线过滤条件
MA1 = AverageFC(Close, Mlen) ;
//Ma11----------------------
if var3 <> 0 then
	begin
	if MA1[1] > MA1[2] then
		begin
		DKKZ_Ma = 1;
		end
	else if MA1[1] < MA1[2] then
		begin
		DKKZ_Ma = -1;
		end
	else
		begin
		DKKZ_Ma = 0;
		end;
	end;
//Ma12----------------------
if var3 <> 0 then
	begin
	if Close > MA1 then
		begin
		DKKZ_Ma = 1;
		end
	else if Close < MA1 then
		begin
		DKKZ_Ma = -1;
		end
	else
		begin
		DKKZ_Ma = 0;
		end;
	end;
//Ma13----------------------
if var3 <> 0 then
	begin
	if MA1[1] > MA1[2] and Close > MA1 then
		begin
		DKKZ_Ma = 1;
		end
	else if MA1[1] < MA1[2] and Close < MA1 then
		begin
		DKKZ_Ma = -1;
		end
	else
		begin
		DKKZ_Ma = 0;
		end;
	end;
//------------------开仓量、真实止损线------------------//
inputs: InitCap(30000);
variables: dOPW( 0 );//开仓数-多头
variables: kOPW( 0 );//开仓数-空头
variables: dZsPriceT(Low);//止损点位(头寸函数调整后返回)-多头
variables: kZsPriceT(High);//止损点位(头寸函数调整后返回)-空头
value2 = ZwF_ZsP(InitCap,DKKZ,dKcPrice,dZsPrice,kKcPrice,kZsPrice,dOPW,kOPW,dZsPriceT,kZsPriceT);

//------------------交易------------------//
//------------------开仓------------------//
variables: D_SAR(0), K_SAR(0);
variables: MP( 0 ); //实际持仓方向 IntraBarPersist
MP = MarketPosition;

//if var3 = 0 then
//	begin
	if DKKZ_Ma = 1 and MP <> 1 then //均线向上且不持有多单
		begin		
		if DKKZ = 11 then //向上突破-》上升通道-》收敛
			begin
			buy("dKC-11") dOPW shares next bar at dKcPrice stop ;
			K_SAR = 0;
			end
		else if DKKZ = 12 then //向上突破-》上升通道-》发散
			begin
			buy("dKC-12") dOPW shares next bar at dKcPrice stop ;
			K_SAR = 0;
			end
		else if DKKZ = 31 then //向上突破-》三角形态
			begin
			buy("dKC-31") dOPW shares next bar at dKcPrice stop ;
			K_SAR = 0;
			end	
		else if DKKZ = 32 then //向上突破-》发散形态
			begin
			buy("dKC-32") dOPW shares next bar at dKcPrice stop ;
			K_SAR = 0;
			end;
		end;
	if DKKZ_Ma = -1 and MP <> -1 then  //均线向下且不持有空单
		begin		
		if DKKZ = -21 then //向下突破-》下降通道-》收敛
			begin
			sellshort("kKC-21") kOPW shares next bar at kKcPrice stop ;
			D_SAR = 0;
			end
		else if DKKZ = -22 then //向下突破-》下降通道-》发散
			begin
			sellshort("kKC-22") kOPW shares next bar at kKcPrice stop ;
			D_SAR = 0;
			end
		else if DKKZ = -31 then //向下突破-》三角形态
			begin
			sellshort("kKC-31") kOPW shares next bar at kKcPrice stop ;
			D_SAR = 0;
			end
		else if DKKZ = -32 then //向下突破-》发散形态
			begin
			sellshort("kKC-32") kOPW shares next bar at kKcPrice stop ;
			D_SAR = 0;
			end;
		end;		
//	end;
	
//---控制开仓连续开仓
{if MP <> MP[1] Then
	DKKZ = 0; //开仓后}
if var2 > 0 and High >= dKcPrice then
	DKKZ = 0
else if var2 < 0 and Low <= kKcPrice then
	DKKZ = 0;
//---统计突破抛物线次数
if var3 > 0 then //向上突破点
	begin
	K_SAR = 0;
	D_SAR = D_SAR + 1;
	end;
if var3 < 0 then //向下突破点
	begin
	D_SAR = 0;
	K_SAR = K_SAR + 1;
	end;
	

//------------------平仓------------------//
variables: dPcPrice( 0 );//多头平仓点位
variables: kPcPrice( 0 );//空头平仓点位
variables: dZsPriceTT(Low);//固定止损点位(头寸函数调整后返回,根据资金量调整止损点位)-多头
variables: kZsPriceTT(High);//固定止损点位(头寸函数调整后返回，根据资金量调整止损点位)-空头
variables: dZsPriceQT(Low);//固定止损点位(无调整)-多头
variables: kZsPriceQT(High);//固定止损点位(无调整)-空头

if MP <> 1 Then
	begin
	dZsPriceTT = dZsPriceT;
	dZsPriceQT = dZsPrice;
	//资金量足够大时，上述两个值相等
	end;
if MP <> -1 Then
	begin
	kZsPriceTT = kZsPriceT;
	kZsPriceQT = kZsPrice;
	//资金量足够大时，上述两个值相等
	end;
	
//if MP > 0 then //持仓-多头
//	begin
	//固定止损
	sell("dZs-TT") next bar at dZsPriceTT stop;
	//sell("dZs-QT") next bar at dZsPriceQT stop;
	//资金量足够大时，上述两个平仓效果相同
	//动态止损
	//sell("dZs-TX") next bar at dZsPriceT stop;	
	sell("dZs-QX") next bar at dZsPrice stop;
	//资金量足够大时，上述两个平仓效果相同
	{dPcPrice = var0[1] - 1 point * MinMove; //平仓线-设置为上一个下抛物线点****
	sell("dPc-SAR") next bar at dPcPrice stop;
	if K_SAR > 0 and var3 = 0 then		
		sell("dPc-kKc") next bar at kKcPrice stop;}
	//sell("dPc-MA") next bar at MA1[1] - 1 point * MinMove stop; //均线平仓（***胜率低）
//	end
//else if MP < 0 then //持仓-空头
//	begin
	//固定止损
	buytocover("kZs-TT") next bar at kZsPriceTT stop;
	//buytocover("kZs-QT") next bar at kZsPriceQT stop;
	//资金量足够大时，上述两个平仓效果相同
	//动态止损
	//buytocover("kZs-TX") next bar at kZsPriceT stop;	
	buytocover("kZs-QX") next bar at kZsPrice stop;
	//资金量足够大时，上述两个平仓效果相同
	{kPcPrice = var0[1] + 1 point * MinMove; //平仓线-设置为上一个上抛物线点****
	buytocover("kPc-SAR") next bar at kPcPrice stop;
	if D_SAR > 0 and var3 = 0 then		
		buytocover("kPc-dKc") next bar at dKcPrice stop;}
	//buytocover("kPc-MA") next bar at MA1[1] + 1 point * MinMove stop; //均线平仓（***胜率低）
//	end;
	