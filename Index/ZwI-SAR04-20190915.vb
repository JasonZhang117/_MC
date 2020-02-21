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
value1 = ZwF_SAR2(var0,var2,var3,DKKZ,dKcPrice,dZsPrice,kKcPrice,kZsPrice,KcPrice,ZsPrice);
//------------------均线指标------------------//
inputs:	Mlen(120);//均线长度
variables: MA1(Open); //均线
variables: DKKZ_Ma(0); //抛物转换点均线状态
MA1 = AverageFC(Close, Mlen) ;
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
//------------------开仓量、真实止损线------------------//
inputs: InitCap(30000);
variables: dOPW( 0 );//开仓数-多头
variables: kOPW( 0 );//开仓数-空头
variables: dZsPriceT(Low);//止损点位(头寸函数返回)-多头
variables: kZsPriceT(High);//止损点位(头寸函数返回)-空头
value2 = ZwF_ZsP(InitCap,DKKZ,dKcPrice,dZsPrice,kKcPrice,kZsPrice,dOPW,kOPW,dZsPriceT,kZsPriceT);


plot1(var0,"SAR");
//------------------辅助线------------------//
//------------------开仓预备线，开仓------------------//
variables: MPP( 0 ); //理论持仓方向
variables: dKcPriceP( High );
variables: dZsPriceP( Low );
variables: kKcPriceP( Low );
variables: kZsPriceP( High );
variables: PPP( 0 );

variables: dKcPLT( Low );//多头开仓点位-划线用(跳空情况)
variables: kKcPLT( High );//空头开仓点位-划线用(跳空情况)
		
if DKKZ_Ma = 1 and  MPP <> 1 then //均线向上且不持有多单
	begin
	if DKKZ = 11 then //向上突破-》上升通道-》收敛
		begin
		dKcPriceP = dKcPrice;
		dZsPriceP = dZsPriceT;
		if High >= dKcPrice then //开仓条件-开多头
			begin
			dKcPLT = MaxList(Open,dKcPrice); //多头开仓点-跳空情况
			//Text_New_BN(BarNumber, dKcPLT,text("dKC-11---",Round(dKcPLT,0)));
			MPP = 1; //持仓-多头
			end;
		end
	else if DKKZ = 12 then //向上突破-》上升通道-》发散
		begin
		dKcPriceP = dKcPrice;
		dZsPriceP = dZsPriceT;
		if High >= dKcPrice then //开仓条件-开多头
			begin
			dKcPLT = MaxList(Open,dKcPrice); //多头开仓点-跳空情况
			//Text_New_BN(BarNumber, dKcPLT,text("dKC-12---",Round(dKcPLT,0)));
			MPP = 1; //持仓-多头
			end;
		end
	else if DKKZ = 31 then //向上突破-》三角形态
		begin
		dKcPriceP = dKcPrice;
		dZsPriceP = dZsPriceT;
		if High >= dKcPrice then //开仓条件-开多头
			begin
			dKcPLT = MaxList(Open,dKcPrice); //多头开仓点-跳空情况
			//Text_New_BN(BarNumber, dKcPLT,text("dKC-31---",Round(dKcPLT,0)));
			MPP = 1; //持仓-多头
			end;
		end	
	else if DKKZ = 32 then //向上突破-》发散形态
		begin
		dKcPriceP = dKcPrice;
		dZsPriceP = dZsPriceT;
		if High >= dKcPrice then //开仓条件-开多头
			begin
			dKcPLT = MaxList(Open,dKcPrice); //多头开仓点-跳空情况
			//Text_New_BN(BarNumber, dKcPLT,text("dKC-32---",Round(dKcPLT,0)));
			MPP = 1; //持仓-多头
			end;
		end;
	PPP = 1;

	end
else if DKKZ_Ma = -1 and  MPP <> -1 then  //均线向下且不持有空单
	begin
	if DKKZ = -21 then //向下突破-》下降通道-》收敛
		begin
		kKcPriceP = kKcPrice;
		kZsPriceP = kZsPriceT;
		if Low <= kKcPrice then //开仓条件-开多头
			begin
			kKcPLT = MinList(Open,kKcPrice);//空头开仓点-跳空情况
			//Text_New_BN(BarNumber, kKcPLT,text("kKC-21---",Round(kKcPLT,0)));
			MPP = -1; //持仓-空头
			end;
		end
	else if DKKZ = -22 then //向下突破-》下降通道-》收敛
		begin
		kKcPriceP = kKcPrice;
		kZsPriceP = kZsPriceT;
		if Low <= kKcPrice then //开仓条件-开多头
			begin
			kKcPLT = MinList(Open,kKcPrice);//空头开仓点-跳空情况
			//Text_New_BN(BarNumber, kKcPLT,text("kKC-22---",Round(kKcPLT,0)));
			MPP = -1; //持仓-空头
			end;
		end
	else if DKKZ = -31 then //向下突破-》三角形态
		begin
		kKcPriceP = kKcPrice;
		kZsPriceP = kZsPriceT;
		if Low <= kKcPrice then //开仓条件-开多头
			begin
			kKcPLT = MinList(Open,kKcPrice);//空头开仓点-跳空情况
			//Text_New_BN(BarNumber, kKcPLT,text("kKC-31---",Round(kKcPLT,0)));
			MPP = -1; //持仓-空头
			end;
		end
	else if DKKZ = -32 then //向下突破-》发散形态
		begin
		kKcPriceP = kKcPrice;
		kZsPriceP = kZsPriceT;
		if Low <= kKcPrice then //开仓条件-开多头
			begin
			kKcPLT = MinList(Open,kKcPrice);//空头开仓点-跳空情况
			//Text_New_BN(BarNumber, kKcPLT,text("kKC-32---",Round(kKcPLT,0)));
			MPP = -1; //持仓-空头
			end;
		end;
	PPP = -1;

	end;	
	
if PPP > 0 then
	begin
	plot2(dKcPrice,"dKcPrice");
	plot3(dZsPriceT,"dZsPriceT");
	end
else if PPP < 0 Then
	begin
	plot4(kKcPrice,"kKcPrice");
	plot5(kZsPriceT,"kZsPriceT");
	end;
	
if var3 > 0 then
	begin
	Text_New_BN (BarNumber, dKcPrice,text(Round(DKKZ,0))); //抛物转换点状态
	end
else if var3 < 0 then
	begin
	Text_New_BN (BarNumber, kKcPrice,text(Round(DKKZ,0))) ; //抛物转换点状态
	end;



//------------------平仓------------------//
variables: dPcPrice( 0 );//多头平仓点位
variables: kPcPrice( 0 );//空头平仓点位
variables: dZsPriceTT(Low);//固定止损点位(头寸函数调整后返回,根据资金量调整止损点位)-多头
variables: kZsPriceTT(High);//固定止损点位(头寸函数调整后返回，根据资金量调整止损点位)-空头
variables: dZsPriceQT(Low);//固定止损点位(无调整)-多头
variables: kZsPriceQT(High);//固定止损点位(无调整)-空头
if MPP <> 1 Then
	begin
	dZsPriceTT = dZsPriceT;
	dZsPriceQT = dZsPrice;
	//资金量足够大时，上述两个值相等
	end
else if MPP <> -1 Then
	begin
	kZsPriceTT = kZsPriceT;
	kZsPriceQT = kZsPrice;
	//资金量足够大时，上述两个值相等
	end;

variables: dPcPL( Low );//多头平仓点位-划线用(最终点位)
variables: dPcPLT( Low );//多头平仓点位-划线用(跳空情况)
variables: kPcPL( High );//空头平仓点位-划线用(最终点位)
variables: kPcPLT( High );//空头平仓点位-划线用(跳空情况)

if MPP > 0 then //持仓-多头
	begin
	dPcPrice = var0[1] - 1 point * MinMove; //平仓线-设置为上一个下抛物线点****
	dPcPL = MaxList(dZsPriceTT,dZsPriceQT,dZsPriceT,dZsPrice,dPcPrice); //多头平仓点-多条件平仓点位选择
	if Low <= dPcPL then //平仓条件-平多头
		begin
		dPcPLT = MinList(Open,Round(dPcPL-0.499,0));//多头平仓点-跳空情况
		//Text_New_BN(BarNumber, dPcPLT,text(dPcPLT,"-D---Pc",Round(dPcPLT-dKcPrice,0)));		
		MPP = 0;//持仓-空仓
		end;
	end
else if MPP < 0 then //持仓-空头		
	begin
	kPcPrice = var0[1] + 1 point * MinMove; //平仓线-设置为上一个上抛物线点****
	kPcPL = MinList(kZsPriceTT,kZsPriceQT,kZsPriceT,kZsPrice,kPcPrice);//空头平仓点-多条件平仓点位选择
	if high >= kPcPL then //平仓条件-平空头
		begin
		kPcPLT = MaxList(Open,Round(kPcPL+0.499,0));//空头平仓点-跳空情况
		//Text_New_BN(BarNumber, kPcPLT,text(kPcPLT,"-K---Pc",Round(kKcPrice-kPcPLT,0)));
		MPP = 0;//持仓-空仓
		end;
	end;
	

